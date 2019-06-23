using System;
using CommandLine;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Mathematics;

namespace AdditiveManufacturing {
	public class Program {
		public static void Main(string[] args) {
			Parser.Default.ParseArguments<Options>(args)
			.WithParsed<Options>(RunOptions);
			//.WithNotParsed<Options>(HandleParseError);
		}

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

		private static void RunOptions(Options opts) {
			try {
				StlReader reader;
				if (opts.IsStlAscii) {
					reader = new StlAsciiReader();
				}
				else {
					reader = new StlBinaryReader();
				}
				Facet[] facets = reader.Read(opts.StlFilePath);
				Console.WriteLine("Parsed " + facets.Length + " facets");
				foreach (Facet facet in facets) {
					Console.WriteLine(facet);
				}
			}
			catch (Exception ex) {
				Console.Error.WriteLine(ex.Message);
				Environment.Exit(1);
			}
		}
	}
}
