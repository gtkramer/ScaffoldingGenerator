using System.IO;
using AdditiveManufacturing.Mathematics;
using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.IO {
	public class StlAsciiWriter : StlWriter {
		public override void Write(string filePath, Facet[] facets) {
			using (StreamWriter writer = new StreamWriter(File.OpenWrite(filePath))) {
				if (facets.Length == 0) {
					throw new IOException("Expected facet definitions");
				}

				string name = "object";
				writer.Write("solid " + name + "\n");
				foreach (Facet facet in facets) {
					Vector3D normal = facet.Normal;
					writer.Write("\t" + "facet normal " + normal.X + " " + normal.Y + " " + normal.Z + "\n");
					writer.Write("\t\t" + "outer loop" + "\n");
					foreach (Point3D vertex in facet.Vertices) {
						writer.Write("\t\t\t" + "vertex " + vertex.X + " " + vertex.Y + " " + vertex.Z + "\n");
					}
					writer.Write("\t\t" + "endloop" + "\n");
					writer.Write("\t" + "endfacet" + "\n");
				}
				writer.Write("endsolid " + name);
			}
		}
	}
}
