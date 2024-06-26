using CommandLine;

namespace ScaffoldingGenerator {
    public class Options
    {
        #pragma warning disable 8618
        [Option("input", Required = true, HelpText = "Input STL file")]
        public string StlInputPath { get; set; }
        [Option("output", Required = true, HelpText = "Output STL file")]
        public string StlOutputPath { get; set; }
        #pragma warning restore 8618
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
}
