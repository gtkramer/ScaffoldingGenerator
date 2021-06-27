using System.IO;
using MathNet.Spatial.Euclidean;
using ScaffoldingGenerator.Geometry;
using System;

namespace ScaffoldingGenerator.IO
{
    public class StlBinaryWriter : StlWriter
    {
        public override void Write(string filePath, Facet[] facets)
        {
            if (facets.Length == 0)
            {
                throw new Exception("Expected facet definitions");
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (FileStream stream = File.OpenWrite(filePath))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(new byte[80]);
                writer.Write(facets.Length);
                foreach (Facet facet in facets)
                {
                    Vector3D normal = facet.Normal;
                    writer.Write((float)normal.X);
                    writer.Write((float)normal.Y);
                    writer.Write((float)normal.Z);
                    foreach (Point3D vertex in facet.Vertices)
                    {
                        writer.Write((float)vertex.X);
                        writer.Write((float)vertex.Y);
                        writer.Write((float)vertex.Z);
                    }
                    writer.Write(new byte[2]);
                }
            }
        }
    }
}