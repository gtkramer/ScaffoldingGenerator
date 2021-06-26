using ScaffoldingGenerator.Geometry;

namespace ScaffoldingGenerator.IO
{
    public abstract class StlReader
    {
        public abstract Facet[] Read(string filePath);
    }
}
