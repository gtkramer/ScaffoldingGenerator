using CommandLine;

namespace ScaffoldingGenerator {
    public class Options
    {
        #pragma warning disable 8618
        [Option("stl-file", Required = true, HelpText = "Input STL file")]
        public string StlFilePath { get; set; }
        #pragma warning restore 8618
        [Option("ascii", Required = false, Default = false, HelpText = "Work with STL files in ASCII, otherwise, assume binary if absent")]
        public bool IsStlAscii { get; set; }

        [Option("critical-angle", Required = false, Default = 45, HelpText = "Critical angle for supporting facets")]
        public float CriticalAngle { get; set; }
        [Option("dimension-length", Required = false, Default = 0.25, HelpText = "Dimension of minimum area needing supported")]
        public float DimensionLength { get; set; }
        [Option("tolerance-angle", Required = false, Default = 10, HelpText = "Used for calculating area")]
        public float ToleranceAngle { get; set; }
        [Option("scaffolding-angle", Required = false, Default = 0, HelpText = "Angle at which to rotate line supports")]
        public float ScaffoldingAngle { get; set; }
        [Option("support-spacing", Required = false, Default = 0.125, HelpText = "Spacing between line supports")]
        public float SupportSpacing { get; set; }
        [Option("plate-spacing", Required = false, Default = 0.5, HelpText = "Scaffolded space between model and build plate")]
        public float PlateSpacing { get; set; }

        [Option("x-scaffolding", Required = false, Default = false, HelpText = "Generate X scaffolding")]
        public bool DoXScaffolding { get; set; }
        [Option("y-scaffolding", Required = false, Default = false, HelpText = "Generate Y scaffolding")]
        public bool DoYScaffolding { get; set; }
        [Option("contour-scaffolding", Required = false, Default = false, HelpText = "Generate contour scaffolding")]
        public bool DoContourScaffolding { get; set; }
    }
}
