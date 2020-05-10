using System;
using CommandLine;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Mathematics;
using MathNet.Spatial.Euclidean;
using System.Collections.Generic;
using System.Linq;
using AdditiveManufacturing.Extensions;
using AdditiveManufacturing.DataStructures;
using AdditiveManufacturing.GUI;
using Gtk;

/*
http://www.oldschoolpixels.com/?p=390
http://grbd.github.io/posts/2016/06/25/gtksharp-part-3-basic-example-with-vs-and-glade/
*/
namespace AdditiveManufacturing
{
    public class Program
    {
        public static void Main(string[] args) {
            using (RenderWindow render = RenderWindow.CreateInstance()) {
                render.Run();
            }
        }

        public static void MainToolWindow(string[] args)
        {
            Application.Init();

            Application app = new Application("org.gtkramer.ScaffoldingGenerator", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            ToolWindow win = new ToolWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }

        public static void MainCmdLine(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(RunOptions);
            //.WithNotParsed<Options>(HandleParseError);
        }

        public static UnitVector3D PerpendicularNormal = UnitVector3D.Create(0, 0, 1);

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
            try
            {
                Facet[] facets = ReadFacetsFromFile(opts.StlFilePath, opts.IsStlAscii);
                Console.WriteLine("Read " + facets.Length + " facets from file");

                Facet[] unsupportedFacets = GetUnsupportedFacets(facets, opts.CriticalAngle);
                Console.WriteLine("Identified " + unsupportedFacets.Length + " unsupported facets");

                Point3DTree<List<Facet>> edgeFacetIndex = BuildEdgeFacetIndex(unsupportedFacets);
                Console.WriteLine("Created an index with " + edgeFacetIndex.Keys.Length + " edges");

                CreateEdgeFacetAssociation(unsupportedFacets, edgeFacetIndex);
                Console.WriteLine("Association created between facets and edges");

                List<List<Facet>> unsupportedRegions = BuildUnsupportedRegions(unsupportedFacets, edgeFacetIndex);
                Console.WriteLine("Built " + unsupportedRegions.Count + " unsupported regions");
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

        private static Facet[] GetUnsupportedFacets(Facet[] facets, double criticalAngle)
        {
            return facets.Where(facet => DoesFacetNeedSupported(facet, criticalAngle)).ToArray();
        }

        private static bool DoesFacetNeedSupported(Facet facet, double criticalAngle)
        {
            return facet.Normal.AngleTo(PerpendicularNormal).Degrees > 180 - criticalAngle;
        }

        private static Point3DTree<List<Facet>> BuildEdgeFacetIndex(Facet[] facets)
        {
            List<Point3D> keys = new List<Point3D>(facets.Length * 3);
            foreach (Facet facet in facets)
            {
                foreach (Line3D edge in facet.Edges)
                {
                    keys.Add(edge.Midpoint());
                }
            }
            return new Point3DTree<List<Facet>>(keys);
        }

        private static void CreateEdgeFacetAssociation(Facet[] facets, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            foreach (Facet facet in facets)
            {
                foreach (Line3D edge in facet.Edges)
                {
                    Point3D midpoint = edge.Midpoint();
                    List<Facet> edgeFacetList = edgeFacetIndex[midpoint];
                    if (edgeFacetList == null)
                    {
                        edgeFacetList = new List<Facet>(2);
                        edgeFacetIndex[midpoint] = edgeFacetList;
                    }
                    edgeFacetList.Add(facet);
                }
            }
            if (edgeFacetIndex.Values.Where(list => list != null && list.Count > 2).ToArray().Length > 0)
            {
                throw new Exception("Found bad STL file with more than two facets sharing an edge");
            }
        }

        private static List<List<Facet>> BuildUnsupportedRegions(Facet[] unsupportedFacets, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            List<List<Facet>> unsupportedRegions = new List<List<Facet>>();
            foreach (Facet unsupportedFacet in unsupportedFacets)
            {
                if (!unsupportedFacet.Visited)
                {
                    unsupportedRegions.Add(GrowUnsupportedRegion(unsupportedFacet, edgeFacetIndex));
                }
            }
            return unsupportedRegions;
        }

        private static List<Facet> GrowUnsupportedRegion(Facet unsupportedFacet, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            Queue<Facet> adjacentQueue = new Queue<Facet>();
            adjacentQueue.Enqueue(unsupportedFacet);
            unsupportedFacet.Visited = true;

            List<Facet> unsupportedRegion = new List<Facet>();
            while (adjacentQueue.Count != 0)
            {
                Facet adjacentFacet = adjacentQueue.Dequeue();
                unsupportedRegion.Add(adjacentFacet);
                EnqueueAdjacentFacets(adjacentFacet, edgeFacetIndex, adjacentQueue);
            }
            return unsupportedRegion;
        }

        private static void EnqueueAdjacentFacets(Facet adjacentFacet, Point3DTree<List<Facet>> edgeFacetIndex, Queue<Facet> adjacentQueue)
        {
            foreach (Facet facet in GetAdjacentFacets(adjacentFacet, edgeFacetIndex))
            {
                if (!facet.Visited)
                {
                    adjacentQueue.Enqueue(facet);
                    facet.Visited = true;
                }
            }
        }

        private static List<Facet> GetAdjacentFacets(Facet facet, Point3DTree<List<Facet>> edgeFacetIndex)
        {
            List<Facet> adjacentFacets = new List<Facet>(3);
            foreach (Line3D edge in facet.Edges)
            {
                foreach (Facet adjacentFacet in edgeFacetIndex[edge.Midpoint()])
                {
                    if (adjacentFacet != facet)
                    {
                        adjacentFacets.Add(adjacentFacet);
                    }
                }
            }
            return adjacentFacets;
        }
    }
}
