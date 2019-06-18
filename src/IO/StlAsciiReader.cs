using System.IO;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Mathematics;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MathNet.Spatial.Euclidean;

public class StlAsciiReader : StlReader {
	private class StlAsciiVisitor : StlAsciiBaseVisitor<Facet[]> {
		public override Facet[] VisitSolid(StlAsciiParser.SolidContext solidContext) {
			StlAsciiParser.FacetContext[] facetContexts = solidContext.facet();
			Facet[] facets = new Facet[facetContexts.Length];
			for (int i = 0; i != facetContexts.Length; i++) {
				ITerminalNode[] normalFloats = facetContexts[i].normal().FLOAT();
				Vector3D normal = new Vector3D(float.Parse(normalFloats[0].GetText()), float.Parse(normalFloats[1].GetText()), float.Parse(normalFloats[2].GetText()));

				StlAsciiParser.VertexContext[] vertexContexts = facetContexts[i].loop().vertex();
				Point3D[] vertices = new Point3D[vertexContexts.Length];
				for (int j = 0; j != vertexContexts.Length; j++) {
					ITerminalNode[] vertexFloats = vertexContexts[j].FLOAT();
					vertices[j] = new Point3D(float.Parse(vertexFloats[0].GetText()), float.Parse(vertexFloats[1].GetText()), float.Parse(vertexFloats[2].GetText()));
				}

				facets[i] = new Facet(normal, vertices);
			}
			return facets;
		}
	}

	public override Facet[] Read(string filePath) {
		Facet[] facets;
		using (FileStream fileStream = File.OpenRead(filePath)) {
			AntlrInputStream inputStream = new AntlrInputStream(fileStream);
			StlAsciiLexer lexer = new StlAsciiLexer(inputStream);
			CommonTokenStream tokenStream = new CommonTokenStream(lexer);
			StlAsciiParser parser = new StlAsciiParser(tokenStream);
			StlAsciiVisitor visitor = new StlAsciiVisitor();
			facets = visitor.Visit(parser.solid());
		}
		return facets;
	}
}
