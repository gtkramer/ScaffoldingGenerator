using AdditiveManufacturing.Geometry;

namespace AdditiveManufacturing.IO
{
    public abstract class StlReader
    {
        public abstract Facet[] Read(string filePath);
    }
}
