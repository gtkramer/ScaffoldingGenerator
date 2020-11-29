using System;
using CommandLine;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Geometry;
using MathNet.Spatial.Euclidean;
using System.Collections.Generic;
using System.Linq;
using AdditiveManufacturing.DataStructures;
using AdditiveManufacturing.GUI;

/*
http://www.oldschoolpixels.com/?p=390
http://grbd.github.io/posts/2016/06/25/gtksharp-part-3-basic-example-with-vs-and-glade/
*/
namespace AdditiveManufacturing
{
    public class Program
    {
        public static void Main(string[] args) {
            //using (RenderWindow renderWindow = RenderWindow.CreateInstance()) {
            //    renderWindow.Run();
            //}
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(RunOptions);
            //.WithNotParsed<Options>(HandleParseError);
        }

        public static UnitVector3D PerpendicularNormal = UnitVector3D.Create(0, 0, 1);
        private static IEqualityComparer<Point3D> Point3DComparer = new Point3DComparer();

        public class Options
        {
            [Option("stl-file", Required = true, HelpText = "Input STL file")]
            public string StlFilePath { get; set; }
            [Option("ascii", Required = false, Default = false, HelpText = "Work with STL files in ASCII, otherwise, assume binary if absent")]
            public bool IsStlAscii { get; set; }

            [Option("critical-angle", Required = false, Default = 45, HelpText = "Critical angle for supporting facets")]
            public double CriticalAngle { get; set; }
            [Option("dimension-length", Required = false, Default = 0.25, HelpText = "Dimension of minimum area needing supported")]
            public double DimensionLength { get; set; }
            [Option("tolerance-angle", Required = false, Default = 10, HelpText = "Used for calculating area")]
            public double ToleranceAngle { get; set; }
            [Option("scaffolding-angle", Required = false, Default = 0, HelpText = "Angle at which to place line supports")]
            public double ScaffoldingAngle { get; set; }
            [Option("support-spacing", Required = false, Default = 0.125, HelpText = "Spacing between line supports")]
            public double SupportSpacing { get; set; }
            [Option("plate-spacing", Required = false, Default = 0.5, HelpText = "Spacing above build plate")]
            public double PlateSpacing { get; set; }

            [Option("x-scaffolding", Required = false, Default = true, HelpText = "Generate X scaffolding")]
            public bool DoXScaffolding { get; set; }
            [Option("y-scaffolding", Required = false, Default = false, HelpText = "Generate Y scaffolding")]
            public bool DoYScaffolding { get; set; }
            [Option("contour-scaffolding", Required = false, Default = false, HelpText = "Generate contour scaffolding")]
            public bool DoContourScaffolding { get; set; }
        }

        private static void RunOptions(Options opts)
        {
            // TODO: Make sure the best versions of these generic collections are being used in the right places.
            // Use arrays when size is known and is not expected to change
            // Otherwise, use a list
            try
            {
                Facet[] facets = ReadFacetsFromFile(opts.StlFilePath, opts.IsStlAscii);
                Console.WriteLine("Read " + facets.Length + " facets from file");

                Facet[] unsupportedFacets = facets.Where(facet => DoesFacetNeedSupported(facet, opts.CriticalAngle)).ToArray();
                Console.WriteLine("Identified " + unsupportedFacets.Length + " unsupported facets");

                Point3DTree<List<Facet>> edgeFacetIndex = new Point3DTree<List<Facet>>(GetEdgeFacetKeys(unsupportedFacets));
                Console.WriteLine("Created an index with " + edgeFacetIndex.Keys.Length + " edges");

                CreateEdgeFacetAssociation(unsupportedFacets, edgeFacetIndex);
                Console.WriteLine("Association created between facets and edges");

                List<Region> unsupportedRegions = BuildUnsupportedRegions(unsupportedFacets, edgeFacetIndex);
                Console.WriteLine("Built " + unsupportedRegions.Count + " unsupported regions");

                List<Region> largeRegions = unsupportedRegions.Where(region => IsLargeRegion(region, edgeFacetIndex, opts.DimensionLength, opts.ToleranceAngle)).ToList();
                Console.WriteLine("Removed " + (unsupportedRegions.Count - largeRegions.Count) + " small unsupported regions");

                // Create line and contour scaffolding with filtered regions
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }

        private static Facet[] ReadFacetsFromFile(string stlFilePath, bool isStlAscii)
        {
            StlReader reader;
            if (isStlAscii)
            {
                reader = new StlAsciiReader();
            }
            else
            {
                reader = new StlBinaryReader();
            }
            return reader.Read(stlFilePath);
        }

        private static bool DoesFacetNeedSupported(Facet facet, double criticalAngle)
        {
            return facet.Normal.AngleTo(PerpendicularNormal).Degrees > 180 - criticalAngle;
        }

        private static Point3D[] GetEdgeFacetKeys(Facet[] facets)
        {
            List<Point3D> keys = new List<Point3D>(facets.Length * 3);
            foreach (Facet facet in facets)
            {
                keys.AddRange(facet.EdgeMidPoints);
            }
            return keys.Distinct(Point3DComparer).ToArray();
        }

        private static void CreateEdgeFacetAssociation(Facet[] facets, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            foreach (Facet facet in facets)
            {
                foreach (Point3D edgeMidPoint in facet.EdgeMidPoints)
                {
                    List<Facet> edgeFacetList = edgeFacetIndex[edgeMidPoint];
                    if (edgeFacetList == null)
                    {
                        edgeFacetList = new List<Facet>(2);
                        edgeFacetIndex[edgeMidPoint] = edgeFacetList;
                    }
                    edgeFacetList.Add(facet);
                }
            }
            if (edgeFacetIndex.Values.Where(list => list != null && list.Count > 2).ToArray().Length > 0)
            {
                throw new Exception("Found bad STL file with more than two facets sharing an edge");
            }
        }

        private static List<Region> BuildUnsupportedRegions(Facet[] unsupportedFacets, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            Point3DTree<bool> facetVisitedIndex = new Point3DTree<bool>(unsupportedFacets.Select(facet => facet.Centroid).ToArray());
            List<Region> unsupportedRegions = new List<Region>();
            foreach (Facet unsupportedFacet in unsupportedFacets)
            {
                if (!facetVisitedIndex[unsupportedFacet.Centroid])
                {
                    unsupportedRegions.Add(new Region(GrowUnsupportedRegion(unsupportedFacet, edgeFacetIndex, facetVisitedIndex)));
                }
            }
            return unsupportedRegions;
        }

        private static List<Facet> GrowUnsupportedRegion(Facet unsupportedFacet, Point3DTree<List<Facet>> edgeFacetIndex, Point3DTree<bool> facetVisitedIndex)
        {
            Queue<Facet> adjacentQueue = new Queue<Facet>();
            adjacentQueue.Enqueue(unsupportedFacet);
            facetVisitedIndex[unsupportedFacet.Centroid] = true;

            List<Facet> unsupportedRegion = new List<Facet>();
            while (adjacentQueue.Count != 0)
            {
                Facet adjacentFacet = adjacentQueue.Dequeue();
                unsupportedRegion.Add(adjacentFacet);
                EnqueueAdjacentFacets(adjacentFacet, adjacentQueue, edgeFacetIndex, facetVisitedIndex);
            }
            return unsupportedRegion;
        }

        private static void EnqueueAdjacentFacets(Facet adjacentFacet, Queue<Facet> adjacentQueue, Point3DTree<List<Facet>> edgeFacetIndex, Point3DTree<bool> facetVisitedIndex)
        {
            foreach (Facet facet in GetAdjacentFacets(adjacentFacet, edgeFacetIndex))
            {
                if (!facetVisitedIndex[facet.Centroid])
                {
                    adjacentQueue.Enqueue(facet);
                    facetVisitedIndex[facet.Centroid] = true;
                }
            }
        }

        private static List<Facet> GetAdjacentFacets(Facet facet, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            List<Facet> adjacentFacets = new List<Facet>(3);
            foreach (Point3D edgeMidPoint in facet.EdgeMidPoints)
            {
                foreach (Facet adjacentFacet in edgeFacetIndex[edgeMidPoint])
                {
                    if (adjacentFacet != facet)
                    {
                        adjacentFacets.Add(adjacentFacet);
                    }
                }
            }
            return adjacentFacets;
        }

        private static bool IsLargeRegion(Region region, Point3DTree<List<Facet>> edgeFacetIndex, double dimensionLength, double toleranceAngle)
        {
            bool isLargeRegion = false;
            List<Vector3D> largeDiagonals = GetLargeDiagonals(GetBoundingVertices(region, edgeFacetIndex), dimensionLength);
            for (int i = 0; i != largeDiagonals.Count && !isLargeRegion; i++)
            {
                Vector3D diagonal1 = largeDiagonals[i];
                for (int j = i; j != largeDiagonals.Count && !isLargeRegion; j++)
                {
                    Vector3D diagonal2 = largeDiagonals[j];
                    double angleBetween = diagonal1.AngleTo(diagonal2).Degrees;
                    if (angleBetween >= 90 - toleranceAngle && angleBetween <= 90 + toleranceAngle) {
                        isLargeRegion = true;
                    }
                }
            }
            return isLargeRegion;
        }

        private static List<Point3D> GetBoundingVertices(Region region, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            List<Point3D> boundingVertices = new List<Point3D>();
            foreach (Facet facet in region.Facets)
            {
                foreach (Point3D edgeMidPoint in facet.EdgeMidPoints)
                {
                    if (edgeFacetIndex[edgeMidPoint].Count == 1)
                    {
                        boundingVertices.Add(edgeMidPoint);
                    }
                }
            }
            return boundingVertices;
        }

        private static List<Vector3D> GetLargeDiagonals(List<Point3D> boundingVertices, double dimensionLength)
        {
            List<Vector3D> diagonals = new List<Vector3D>();
            for (int i = 0; i != boundingVertices.Count; i++)
            {
                Point3D point1 = boundingVertices[i];
                for (int j = i; j != boundingVertices.Count; j++) {
                    Point3D point2 = boundingVertices[j];
                    if (point1.DistanceTo(point2) >= dimensionLength)
                    {
                        diagonals.Add(new Vector3D(point2.X - point1.X, point2.Y - point1.Y, point2.Z - point1.Z));
                    }
                }
            }
            return diagonals;
        }
    }
}
