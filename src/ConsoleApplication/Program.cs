using System;
using CommandLine;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Geometry;
using MathNet.Spatial.Euclidean;
using System.Collections.Generic;
using System.Linq;
using AdditiveManufacturing.DataStructures;
using AdditiveManufacturing.GUI;
using AdditiveManufacturing.Extensions;
using MathNet.Spatial.Units;

/*
http://www.oldschoolpixels.com/?p=390
http://grbd.github.io/posts/2016/06/25/gtksharp-part-3-basic-example-with-vs-and-glade/
*/
namespace AdditiveManufacturing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //MainRenderWindow(args);
            //MainToolWindow(args);
            MainCmdLine(args);
        }

        private static void MainRenderWindow(string[] args)
        {
            using (RenderWindow renderWindow = RenderWindow.CreateInstance()) {
                renderWindow.Run();
            }
        }

        private static void MainToolWindow(string[] args)
        {
            Gtk.Application.Init();
            ToolWindow toolWindow = ToolWindow.CreateInstance();
            Gtk.Application.Run();
        }

        private static void MainCmdLine(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(RunOptions);
            //.WithNotParsed<Options>(HandleParseError);
        }

        // Pointing in Z direction
        private static UnitVector3D XYNormal = UnitVector3D.Create(0, 0, 1);
        // Pointing in Y direction
        private static UnitVector3D XZNormal = UnitVector3D.Create(0, 1, 0);
        // Pointing in X direction
        private static UnitVector3D YZNormal = UnitVector3D.Create(1, 0, 0);

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
            [Option("scaffolding-angle", Required = false, Default = 0, HelpText = "Angle at which to rotate line supports")]
            public double ScaffoldingAngle { get; set; }
            [Option("support-spacing", Required = false, Default = 0.125, HelpText = "Spacing between line supports")]
            public double SupportSpacing { get; set; }
            [Option("plate-spacing", Required = false, Default = 0.5, HelpText = "Scaffolded space between model and build plate")]
            public double PlateSpacing { get; set; }

            [Option("x-scaffolding", Required = false, Default = false, HelpText = "Generate X scaffolding")]
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
            // Consider places where we can avoid a conversion of .ToArray/.ToList and rely on the IEnumerable interface
            try
            {
                Polygon3D model = new Polygon3D(ReadFacetsFromFile(opts.StlFilePath, opts.IsStlAscii));
                Console.WriteLine("Read " + model.Facets.Length + " facets from file");

                Facet[] unsupportedFacets = model.Facets.Where(facet => DoesFacetNeedSupported(facet, opts.CriticalAngle)).ToArray();
                Console.WriteLine("Identified " + unsupportedFacets.Length + " unsupported facets");

                Point3DTree<List<Facet>> edgeFacetIndex = new Point3DTree<List<Facet>>(GetEdgeFacetKeys(unsupportedFacets));
                Console.WriteLine("Created an index with " + edgeFacetIndex.Keys.Length + " edges");

                CreateEdgeFacetAssociation(unsupportedFacets, edgeFacetIndex);
                Console.WriteLine("Association created between facets and edges");

                List<Polygon3D> unsupportedRegions = BuildUnsupportedRegions(unsupportedFacets, edgeFacetIndex);
                Console.WriteLine("Built " + unsupportedRegions.Count + " unsupported regions");

                List<Polygon3D> largeRegions = unsupportedRegions.Where(region => IsLargeRegion(region, edgeFacetIndex, opts.DimensionLength, opts.ToleranceAngle)).ToList();
                Console.WriteLine("Removed " + (unsupportedRegions.Count - largeRegions.Count) + " small unsupported regions");

                List<Facet> scaffoldingFacets = new List<Facet>();
                if (opts.DoXScaffolding || opts.DoYScaffolding) {
                    List<UnitVector3D> supportNormals = new List<UnitVector3D>();
                    Angle scaffoldingAngle = Angle.FromDegrees(opts.ScaffoldingAngle);
                    if (opts.DoXScaffolding) {
                        supportNormals.Add(YZNormal.Rotate(XYNormal, scaffoldingAngle));
                    }
                    if (opts.DoYScaffolding) {
                        supportNormals.Add(XZNormal.Rotate(XYNormal, scaffoldingAngle));
                    }
                    Console.WriteLine("Made support normals");
                    foreach (UnitVector3D supportNormal in supportNormals) {
                        scaffoldingFacets.AddRange(GenerateLineScaffolding(model, largeRegions, supportNormal, opts.SupportSpacing, opts.PlateSpacing));
                    }
                }
                // if (opts.DoContourScaffolding) {
                //     scaffoldingFacets.AddRange(GenerateContourScaffolding(largeRegions, opts.PlateSpacing);
                // }
                StlAsciiWriter writer = new StlAsciiWriter();
                writer.Write("out.stl", scaffoldingFacets.ToArray());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
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
            return facet.Normal.AngleTo(XYNormal).Degrees > 180 - criticalAngle;
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

        private static List<Polygon3D> BuildUnsupportedRegions(Facet[] unsupportedFacets, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            Point3DTree<bool> facetVisitedIndex = new Point3DTree<bool>(unsupportedFacets.Select(facet => facet.Centroid).ToArray());
            List<Polygon3D> unsupportedRegions = new List<Polygon3D>();
            foreach (Facet unsupportedFacet in unsupportedFacets)
            {
                if (!facetVisitedIndex[unsupportedFacet.Centroid])
                {
                    unsupportedRegions.Add(new Polygon3D(GrowUnsupportedRegion(unsupportedFacet, edgeFacetIndex, facetVisitedIndex)));
                }
            }
            return unsupportedRegions;
        }

        private static Facet[] GrowUnsupportedRegion(Facet unsupportedFacet, Point3DTree<List<Facet>> edgeFacetIndex, Point3DTree<bool> facetVisitedIndex)
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
            return unsupportedRegion.ToArray();
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

        private static bool IsLargeRegion(Polygon3D region, Point3DTree<List<Facet>> edgeFacetIndex, double dimensionLength, double toleranceAngle)
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

        private static List<Point3D> GetBoundingVertices(Polygon3D region, Point3DTree<List<Facet>> edgeFacetIndex)
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

        private static List<Facet> GenerateLineScaffolding(Polygon3D model, List<Polygon3D> regions, UnitVector3D supportNormal, double supportSpacing, double plateSpacing) {
            List<Facet> scaffolding = new List<Facet>();
            foreach (Polygon3D region in regions) {
                foreach (List<Point3D> intersectionPoints in GetLineSupportIntersections(region, supportNormal, supportSpacing)) {
                    scaffolding.AddRange(CreateTesselatedLineSupport(intersectionPoints, supportNormal, plateSpacing, model));
                }
            }
            Console.WriteLine("Generated line scaffolding");
            return scaffolding;
        }

        private static List<List<Point3D>> GetLineSupportIntersections(Polygon3D region, UnitVector3D supportNormal, double supportSpacing) {
            List<List<Point3D>> intersectionPointSets = new List<List<Point3D>>();

            List<Plane> planes = GetLineSupportPlanes(region, supportNormal, supportSpacing);
            foreach (Plane plane in planes) {
                List<Point3D> intersectionPoints = GetLineScaffoldingIntersections(region, plane);
                if (intersectionPoints.Count != 0) {
                    intersectionPointSets.Add(intersectionPoints);
                }
            }
            Console.WriteLine("Aggregated intersection points between planes and region");
            return intersectionPointSets;
        }

        private static List<Plane> GetLineSupportPlanes(Polygon3D region, UnitVector3D supportNormal, double supportSpacing) {
            List<Plane> planes = new List<Plane>();

            planes.Add(new Plane(supportNormal, region.CenterPoint));
            int numIntervals = (int)(region.MinPoint.DistanceTo(region.MaxPoint) / supportSpacing / 2) + 1;
            for (int i = 1; i != numIntervals; i++) {
                double shift = i * supportSpacing;
                Point3D positivePoint = region.CenterPoint.Move(supportNormal, shift);
                if (region.ContainsPoint(positivePoint)) {
                    planes.Add(new Plane(supportNormal, positivePoint));
                }
                Point3D negativePoint = region.CenterPoint.Move(supportNormal, -shift);
                if (region.ContainsPoint(negativePoint)) {
                    planes.Add(new Plane(supportNormal, negativePoint));
                }
            }
            Console.WriteLine("Found " + planes.Count + " planes of support for region");
            return planes;
        }

        private static List<Point3D> GetLineScaffoldingIntersections(Polygon3D region, Plane support) {
            List<Point3D> intersections = new List<Point3D>();
            foreach (Facet facet in region.Facets) {
                foreach (Line3D edge in facet.Edges) {
                    try {
                        Point3D? intersection = support.IntersectionWith(edge);
                        if (intersection.HasValue) {
                            intersections.Add(intersection.Value);
                        }
                    }
                    catch (InvalidOperationException ioe) {
                        if (ioe.Message.Equals("Line lies in the plane", StringComparison.OrdinalIgnoreCase)) {
                            Console.WriteLine("Adding both endpoints of line because line lies in the plane");
                            intersections.Add(edge.StartPoint);
                            intersections.Add(edge.EndPoint);
                        }
                    }
                }
            }
            Console.WriteLine("Found " + intersections.Count + " intersection points between plane and region");
            return intersections;
        }

        private static IEnumerable<Facet> CreateTesselatedLineSupport(List<Point3D> intersectionPoints, UnitVector3D supportNormal, double plateSpacing, Polygon3D model) {
            Point3D[] xySortedPoints = intersectionPoints.OrderBy(point => point.X).OrderBy(point => point.Y).ToArray();
            double xyLength = xySortedPoints[0].ToPoint2D().DistanceTo(xySortedPoints[xySortedPoints.Length - 1].ToPoint2D());
            double xyAvgSegmentLength = xyLength / (xySortedPoints.Length - 1);

            double intersectionMinZ = intersectionPoints.OrderBy(points => points.Z).First().Z;
            Console.WriteLine("Intersection min Z:" + intersectionMinZ);
            double intersectionMaxZ = intersectionPoints.OrderBy(points => points.Z).Last().Z;
            Console.WriteLine("Intersection max Z:" + intersectionMaxZ);
            double buildPlateZ = model.MinPoint.Z - plateSpacing;
            double zMinLength = intersectionMinZ - buildPlateZ;

            int numRows = 2;
            int numCols = xySortedPoints.Length;
            Point3D[,] pointGrid = new Point3D[numRows, numCols];
            for (int row = 0; row != numRows; row++) {
                for (int col = 0; col != numCols; col++) {
                    Point3D referencePoint = xySortedPoints[col];
                    if (row == 0) {
                        pointGrid[row, col] = referencePoint;
                    }
                    else if (row == 1) {
                        pointGrid[row, col] = new Point3D(referencePoint.X, referencePoint.Y, buildPlateZ);
                    }
                }
            }
            Console.WriteLine("Filled grid of points");

            List<Facet> scaffoldingFacets = new List<Facet>();
            for (int row = 0; row != numRows - 1; row++) {
                for (int col = 0; col != numCols - 1; col++) {
                    Point3D p1 = pointGrid[row + 1, col + 1];
                    Point3D p2 = pointGrid[row + 1, col];
                    Point3D p3 = pointGrid[row, col];
                    Point3D p4 = pointGrid[row, col + 1];

                    scaffoldingFacets.Add(TesselatePoints(p3, p2, p1));
                    scaffoldingFacets.Add(TesselatePoints(p1, p4, p3));
                }
            }
            Console.WriteLine("Created tesselation from grid");
            return scaffoldingFacets;
        }

        private static Facet TesselatePoints(Point3D v1, Point3D v2, Point3D v3) {
            Vector3D AB = v1.VectorTo(v2);
            Console.WriteLine("Created vector u from " + v1 + " and " + v2);
            Vector3D AC = v1.VectorTo(v3);
            Console.WriteLine("Created vector v from " + v1 + " and " + v3);
            Vector3D normal = AB.CrossProduct(AC);
            Console.WriteLine("Tesselated a facet");
            return new Facet(normal, new Point3D[]{v1, v2, v3});
        }
    }
}
