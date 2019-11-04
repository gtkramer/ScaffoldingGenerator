using System;
using CommandLine;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Mathematics;
using MathNet.Spatial.Euclidean;
using System.Collections.Generic;
using System.Linq;

namespace AdditiveManufacturing {
	public class Program {
		public static void Main(string[] args) {
			Parser.Default.ParseArguments<Options>(args)
			.WithParsed<Options>(RunOptions);
			//.WithNotParsed<Options>(HandleParseError);
		}

		public static UnitVector3D PerpendicularNormal = UnitVector3D.Create(0, 0, 1);

		public class Options {
			[Option("stl-file", Required = true, HelpText = "Input STL file")]
			public string StlFilePath { get; set; }
			[Option("ascii", Required = false, Default = false, HelpText = "Work with STL files in ASCII, otherwise, assume binary if absent")]
			public bool IsStlAscii { get; set; }

			[Option("critical-angle", Required = false, Default = 45, HelpText = "Critical angle for supporting facets")]
			public float CriticalAngle { get; set; }
			//[Option("dimension-length", Required = false, Default = 0.25, HelpText = "Dimension of minimum area needing supported")]
			//public float DimensionLength { get; set; }
			[Option("tolerance-angle", Required = false, Default = 10, HelpText = "Used for calculating area")]
			public float ToleranceAngle { get; set; }
			[Option("scaffolding-angle", Required = false, Default = 0, HelpText = "Angle at which to place line supports")]
			public float ScaffoldingAngle { get; set; }
			//[Option("support-spacing", Required = false, Default = 0.125, HelpText = "Spacing between line supports")]
			//public float SupportSpacing { get; set; }
			//[Option("plate-spacing", Required = false, Default = 0.5, HelpText = "Spacing above build plate")]
			//public float PlateSpacing { get; set; }

			[Option("x-scaffolding", Required = false, Default = true, HelpText = "Generate X scaffolding")]
			public bool DoXScaffolding { get; set; }
			[Option("y-scaffolding", Required = false, Default = false, HelpText = "Generate Y scaffolding")]
			public bool DoYScaffolding { get; set; }
			[Option("contour-scaffolding", Required = false, Default = false, HelpText = "Generate contour scaffolding")]
			public bool DoContourScaffolding { get; set; }
		}

		/*struct FacetTracker {
			public Facet Facet;
			public bool WasVisited;
		}*/

		private static void RunOptions(Options opts) {
			Facet[] facets = ReadFacetsFromFile(opts.StlFilePath, opts.IsStlAscii);
			List<List<Facet>> unsupportedRegions = BuildUnsupportedRegions(facets, opts.CriticalAngle);
		}

		/// <summary>
		/// Parses facets from file in binary or ASCII
		/// </summary>
		private static Facet[] ReadFacetsFromFile(string stlFilePath, bool isStlAscii) {
			StlReader reader;
			if (isStlAscii) {
				reader = new StlAsciiReader();
			}
			else {
				reader = new StlBinaryReader();
			}
			return reader.Read(stlFilePath);
		}

		/// <summary>
		/// Generate connected regions
		/// A connected region is defined as the list of facets where every facet in the list
		/// shares an edge with another facet
		/// </summary>
		private static List<List<Facet>> BuildUnsupportedRegions(Facet[] facets, double criticalAngle) {
			IEnumerable<Facet> unsupportedFacets = facets.Where((x) => DoesFacetNeedSupported(x, criticalAngle));
			Dictionary<Line3D, List<Facet>> edgeFacetIndex = BuildEdgeFacetIndex(unsupportedFacets);
			List<List<Facet>> unsupportedRegions = new List<List<Facet>>();
			foreach (Facet unsupportedFacet in unsupportedFacets) {
				if (!unsupportedFacet.Visited) {
					unsupportedRegions.Add(ExpandUnsupportedRegion(unsupportedFacet, edgeFacetIndex));
				}
			}
			return unsupportedRegions;
		}

		/// <summary>
		/// Determines whether a facet needs supported with respect to a critical angle
		/// </summary>
		private static bool DoesFacetNeedSupported(Facet facet, double criticalAngle) {
			return facet.Normal.AngleTo(PerpendicularNormal).Degrees > 180 - criticalAngle;
		}

		/// <summary>
		/// Gather facets that share an edge
		/// A KDTree could deterministically index edges by their value if their midpoint is used.
		/// However, an implementation of a KDTree that takes a MathNet.Spatial.Euclidean.Point3D as its key
		/// to avoid a value conversion for every lookup does not exist.  The implementation of the hash function
		/// for a Line3D looks reasonable enough to avoid implementing a complex data structure.  Don't know if
		/// it can guarantee uniqueness and avoid collisions though.  To account for this, and to account for
		/// incorrectness in the STL file, if at any point there are more than two facets that share an edge,
		/// error out
		/// </summary>
		private static Dictionary<Line3D, List<Facet>> BuildEdgeFacetIndex(IEnumerable<Facet> unsupportedFacets) {
			Dictionary<Line3D, List<Facet>> edgeFacetIndex = new Dictionary<Line3D, List<Facet>>();
			foreach (Facet unsupportedFacet in unsupportedFacets) {
				foreach (Line3D edge in unsupportedFacet.Edges) {
					if (edgeFacetIndex.ContainsKey(edge)) {
						List<Facet> connectedFacets = edgeFacetIndex[edge];
						connectedFacets.Add(unsupportedFacet);
						if (connectedFacets.Count > 2) {
							throw new Exception("More than two facets share the same edge");
						}
					}
					else {
						List<Facet> facetList = new List<Facet>() {unsupportedFacet};
						edgeFacetIndex[edge] = facetList;
					}
				}
			}
			return edgeFacetIndex;
		}

		private static List<Facet> ExpandUnsupportedRegion(Facet unsupportedFacet, Dictionary<Line3D, List<Facet>> edgeFacetIndex) {
			List<Facet> unsupportedRegion = new List<Facet>();
			Queue<Facet> adjacentFacets = new Queue<Facet>();
			adjacentFacets.Append(unsupportedFacet);
			while (adjacentFacets.Count != 0) {
				Facet adjacentFacet = adjacentFacets.Dequeue();
				unsupportedRegion.Add(adjacentFacet);
				adjacentFacet.Visited = true;
				EnqueueAdjacentFacets(adjacentFacet, edgeFacetIndex, adjacentFacets);
			}
			return unsupportedRegion;
		}

		private static void EnqueueAdjacentFacets(Facet adjacentFacet, Dictionary<Line3D, List<Facet>> edgeFacetIndex, Queue<Facet> adjacentFacets) {
			foreach (Line3D edge in adjacentFacet.Edges) {
				foreach (Facet facet in edgeFacetIndex[edge]) {
					if (!facet.Visited) {
						adjacentFacets.Enqueue(facet);
					}
				}
			}
		}
	}
}
