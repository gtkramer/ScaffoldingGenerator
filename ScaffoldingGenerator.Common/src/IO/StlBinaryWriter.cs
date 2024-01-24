using System.IO;
using ScaffoldingGenerator.Geometry;
using System;
using OpenTK.Mathematics;

namespace ScaffoldingGenerator.IO
{
    public class StlBinaryWriter : StlWriter
    {
        public override void Write(string filePath, Polygon3[] facets)
        {
            if (facets.Length == 0)
            {
                throw new Exception("Expected facet definitions");
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(filePath)))
            {
                writer.Write(new byte[80]);
                writer.Write(facets.Length);
                foreach (Polygon3 facet in facets)
                {
                    Vector3 normal = facet.Normal;
                    writer.Write(normal.X);
                    writer.Write(normal.Y);
                    writer.Write(normal.Z);
                    foreach (Point3 vertex in facet.Vertices)
                    {
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
