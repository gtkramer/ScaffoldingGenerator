using AdditiveManufacturing.Mathematics;

namespace AdditiveManufacturing.IO {
	public abstract class StlReader {
		public abstract Facet[] Read(string filePath);
	}
}
