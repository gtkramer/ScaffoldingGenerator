using ScaffoldingGenerator.Geometry;

namespace ScaffoldingGenerator.IO
{
    public abstract class StlReader
    {
        public abstract Polygon3[] Read(string filePath);
    }
}
