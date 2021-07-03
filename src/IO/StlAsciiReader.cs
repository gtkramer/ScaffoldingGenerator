using System;
using System.IO;
using ScaffoldingGenerator.IO;
using ScaffoldingGenerator.Geometry;
using Antlr4.Runtime;
using System.Linq;
using OpenTK.Mathematics;
using System.Collections.Generic;

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

    private class NormalVisitor : StlAsciiBaseVisitor<Vector3>
    {
        public override Vector3 VisitNormal(StlAsciiParser.NormalContext context)
        {
            IEnumerable<float> parsed = context.FLOAT().Select((x) => float.Parse(x.GetText()));
            IEnumerator<float> enumerator = parsed.GetEnumerator();
            enumerator.MoveNext();
            float x = enumerator.Current;
            enumerator.MoveNext();
            float y = enumerator.Current;
            enumerator.MoveNext();
            float z = enumerator.Current;
            return new Vector3(x, y, z);
        }
    }

    private class LoopVisitor : StlAsciiBaseVisitor<Point3[]>
    {
        public override Point3[] VisitLoop(StlAsciiParser.LoopContext context)
        {
            VertexVisitor vertexVisitor = new VertexVisitor();
            return context.vertex().Select((x) => vertexVisitor.VisitVertex(x)).ToArray();
        }
    }

    private class VertexVisitor : StlAsciiBaseVisitor<Point3>
    {
        public override Point3 VisitVertex(StlAsciiParser.VertexContext context)
        {
            IEnumerable<float> parsed = context.FLOAT().Select((x) => float.Parse(x.GetText()));
            IEnumerator<float> enumerator = parsed.GetEnumerator();
            enumerator.MoveNext();
            float x = enumerator.Current;
            enumerator.MoveNext();
            float y = enumerator.Current;
            enumerator.MoveNext();
            float z = enumerator.Current;
            return new Point3(x, y, z);
        }
    }
}
