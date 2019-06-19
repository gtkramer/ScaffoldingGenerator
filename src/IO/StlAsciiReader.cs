using System.IO;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Mathematics;
using Antlr4.Runtime;
using MathNet.Spatial.Euclidean;
using System.Linq;

public class StlAsciiReader : StlReader {
	private class SolidVisitor : StlAsciiBaseVisitor<Facet[]> {
		public override Facet[] VisitSolid(StlAsciiParser.SolidContext context) {
			FacetVisitor facetVisitor = new FacetVisitor();
			return context.facet().Select((x) => facetVisitor.VisitFacet(x)).ToArray();
		}
	}

	private class FacetVisitor : StlAsciiBaseVisitor<Facet> {
		public override Facet VisitFacet(StlAsciiParser.FacetContext context) {
			NormalVisitor normalVisitor = new NormalVisitor();
			LoopVisitor loopVisitor = new LoopVisitor();
			return new Facet(normalVisitor.VisitNormal(context.normal()), loopVisitor.VisitLoop(context.loop()));
		}
	}

	private class NormalVisitor : StlAsciiBaseVisitor<Vector3D> {
		public override Vector3D VisitNormal(StlAsciiParser.NormalContext context) {
			float[] floats = context.FLOAT().Select((x) => float.Parse(x.GetText())).ToArray();
			return new Vector3D(floats[0], floats[1], floats[2]);
		}
	}

	private class LoopVisitor : StlAsciiBaseVisitor<Point3D[]> {
		public override Point3D[] VisitLoop(StlAsciiParser.LoopContext context) {
			VertexVisitor vertexVisitor = new VertexVisitor();
			return context.vertex().Select((x) => vertexVisitor.VisitVertex(x)).ToArray();
		}
	}

	private class VertexVisitor : StlAsciiBaseVisitor<Point3D> {
		public override Point3D VisitVertex(StlAsciiParser.VertexContext context) {
			float[] floats = context.FLOAT().Select((x) => float.Parse(x.GetText())).ToArray();
			return new Point3D(floats[0], floats[1], floats[2]);
		}
	}

	public override Facet[] Read(string filePath) {
		string fileContents = File.ReadAllText(filePath);
		AntlrInputStream inputStream = new AntlrInputStream(fileContents.ToLower());
		StlAsciiLexer lexer = new StlAsciiLexer(inputStream);
		CommonTokenStream tokenStream = new CommonTokenStream(lexer);
		StlAsciiParser parser = new StlAsciiParser(tokenStream);
		SolidVisitor visitor = new SolidVisitor();
		return visitor.Visit(parser.solid());
	}
}
