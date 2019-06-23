using System;
using System.IO;
using AdditiveManufacturing.Mathematics;
using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.IO {
	public class StlBinaryReader : StlReader {
		public override Facet[] Read(string filePath) {
			int facetCount = GetFacetCount(filePath);
			using (FileStream stream = File.OpenRead(filePath))
			using (BinaryReader reader = new BinaryReader(stream)) {
				reader.ReadBytes(84);
				Facet[] facets = new Facet[facetCount];
				for (int currFacet = 0; currFacet != facetCount; currFacet++) {
					Vector3D normal = new Vector3D(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
					Point3D[] vertices = new Point3D[3];
					for (int currVertex = 0; currVertex != vertices.Length; currVertex++) {
						vertices[currVertex] = new Point3D(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
					}
					facets[currFacet] = new Facet(normal, vertices);
					reader.ReadBytes(2);
				}
				return facets;
			}
		}

		private int GetFacetCount(string filePath) {
			FileInfo fileInfo = new FileInfo(filePath);
			long fileLength = fileInfo.Length;
			if ((fileLength - 84) % 50 != 0) {
				throw new Exception("Corrupt file");
			}
			int facetCount = (int)((fileLength - 84) / 50);
			if (facetCount <= 0) {
				throw new Exception("No facets exist");
			}
			return facetCount;
		}
	}
}
