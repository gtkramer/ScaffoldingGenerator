using System.IO;
using MathNet.Spatial.Euclidean;
using AdditiveManufacturing.Mathematics;

namespace AdditiveManufacturing.IO {
	public class StlBinaryWriter : StlWriter {
		public override void Write(string filePath, Facet[] facets) {
			using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(filePath))) {
				if (facets.Length == 0) {
					throw new IOException("Expected facet definitions");
				}

				writer.Write(new byte[80]);
				writer.Write(facets.Length);
				foreach (Facet facet in facets) {
					Vector3D normal = facet.Normal;
					writer.Write(normal.X);
					writer.Write(normal.Y);
					writer.Write(normal.Z);

					foreach (Point3D vertex in facet.Vertices) {
						writer.Write(vertex.X);
						writer.Write(vertex.Y);
						writer.Write(vertex.Z);
					}

					writer.Write(new byte[2]);
				}
			}
		}
	}
}
