using System;
using System.IO;
using ScaffoldingGenerator.IO;
using ScaffoldingGenerator.Geometry;
using Antlr4.Runtime;
using CalcNet.Spatial.Euclidean;
using System.Linq;

public class StlAsciiReader : StlReader
{
    public override Facet[] Read(string filePath)
    {
        string fileContents = File.ReadAllText(filePath);
        AntlrInputStream inputStream = new AntlrInputStream(fileContents.ToLower());
        StlAsciiLexer lexer = new StlAsciiLexer(inputStream);
        CommonTokenStream tokenStream = new CommonTokenStream(lexer);
        StlAsciiParser parser = new StlAsciiParser(tokenStream);
        SolidVisitor visitor = new SolidVisitor();
        Facet[] facets = visitor.Visit(parser.solid());
        if (parser.NumberOfSyntaxErrors != 0)
        {
            throw new Exception("Corrupt file");
        }
        return facets;
    }

    private class SolidVisitor : StlAsciiBaseVisitor<Facet[]>
    {
        public override Facet[] VisitSolid(StlAsciiParser.SolidContext context)
        {
            FacetVisitor facetVisitor = new FacetVisitor();
            return context.facet().Select((x) => facetVisitor.VisitFacet(x)).ToArray();
        }
    }

    private class FacetVisitor : StlAsciiBaseVisitor<Facet>
    {
        public override Facet VisitFacet(StlAsciiParser.FacetContext context)
        {
            NormalVisitor normalVisitor = new NormalVisitor();
            LoopVisitor loopVisitor = new LoopVisitor();
            return new Facet(normalVisitor.VisitNormal(context.normal()), loopVisitor.VisitLoop(context.loop()));
        }
    }

    private class NormalVisitor : StlAsciiBaseVisitor<Vector3D>
    {
        public override Vector3D VisitNormal(StlAsciiParser.NormalContext context)
        {
            return new Vector3D(context.FLOAT().Select((x) => float.Parse(x.GetText())));
        }
    }

    private class LoopVisitor : StlAsciiBaseVisitor<Point3D[]>
    {
        public override Point3D[] VisitLoop(StlAsciiParser.LoopContext context)
        {
            VertexVisitor vertexVisitor = new VertexVisitor();
            return context.vertex().Select((x) => vertexVisitor.VisitVertex(x)).ToArray();
        }
    }

    private class VertexVisitor : StlAsciiBaseVisitor<Point3D>
    {
        public override Point3D VisitVertex(StlAsciiParser.VertexContext context)
        {
            return new Point3D(context.FLOAT().Select((x) => float.Parse(x.GetText())));
        }
    }
}
