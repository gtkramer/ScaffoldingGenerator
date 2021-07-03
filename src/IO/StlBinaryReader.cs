using System;
using System.IO;
using ScaffoldingGenerator.Geometry;
using OpenTK.Mathematics;

namespace ScaffoldingGenerator.IO
{
    public class StlBinaryReader : StlReader
    {
        public override Facet[] Read(string filePath)
        {
            int facetCount = GetFacetCount(filePath);
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath))) {
                reader.ReadBytes(84);
                Facet[] facets = new Facet[facetCount];
                for (int f = 0; f != facetCount; f++)
                {
                    Vector3 normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Point3[] vertices = new Point3[3];
                    for (int v = 0; v != vertices.Length; v++)
                    {
                        vertices[v] = new Point3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    facets[f] = new Facet(normal, vertices);
                    reader.ReadBytes(2);
                }
                return facets;
            }
        }

        private int GetFacetCount(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long fileLength = fileInfo.Length;
            if ((fileLength - 84) % 50 != 0)
            {
                throw new InvalidOperationException("Corrupt binary STL file");
            }
            int facetCount = (int)((fileLength - 84) / 50);
            if (facetCount <= 0)
            {
                throw new InvalidOperationException("No facets exist in binary STL file");
            }
            return facetCount;
        }
    }
}
