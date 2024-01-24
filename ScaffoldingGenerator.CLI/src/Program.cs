using CommandLine;
using ScaffoldingGenerator.IO;
using ScaffoldingGenerator.Geometry;
using System.Collections.Generic;
using System;

namespace ScaffoldingGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions);
            //.WithNotParsed<Options>(HandleParseError);
        }

        private static void RunOptions(Options opts)
        {
            Mesh3 model = new Mesh3(Algorithms.ReadFacetsFromFile(opts.StlInputPath, opts.IsStlAscii));
            List<Polygon3> scaffoldingFacets = Algorithms.GenerateScaffolding(model, opts.CriticalAngle, opts.DimensionLength, opts.ToleranceAngle, opts.ScaffoldingAngle, opts.SupportSpacing, opts.PlateSpacing, opts.DoXScaffolding, opts.DoYScaffolding, opts.DoContourScaffolding);
            Algorithms.WriteFacetsToFile(scaffoldingFacets.ToArray(), opts.StlOutputPath);
        }
    }
}
