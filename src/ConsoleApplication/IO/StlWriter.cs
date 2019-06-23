using AdditiveManufacturing.Mathematics;

namespace AdditiveManufacturing.IO {
	public abstract class StlWriter {
		public abstract void Write(string filePath, Facet[] facets);
	}
}
