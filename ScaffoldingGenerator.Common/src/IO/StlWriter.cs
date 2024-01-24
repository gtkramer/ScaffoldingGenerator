using ScaffoldingGenerator.Geometry;

namespace ScaffoldingGenerator.IO
{
    public abstract class StlWriter
    {
        public abstract void Write(string filePath, Polygon3[] facets);
    }
}
