using System;
using System.IO;
using ScaffoldingGenerator.Geometry;
using CalcNet.Spatial.Euclidean;
using OpenTK.Mathematics;

namespace ScaffoldingGenerator.IO
{
    public class StlAsciiWriter : StlWriter
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
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string name = "object";
                writer.Write("solid " + name + "\n");
                foreach (Facet facet in facets)
                {
                    Vector3 normal = facet.Normal;
                    writer.Write("\t" + "facet normal " + normal.X + " " + normal.Y + " " + normal.Z + "\n");
                    writer.Write("\t\t" + "outer loop" + "\n");
                    foreach (Point3 vertex in facet.Vertices)
                    {
                        writer.Write("\t\t\t" + "vertex " + vertex.X + " " + vertex.Y + " " + vertex.Z + "\n");
                    }
                    writer.Write("\t\t" + "endloop" + "\n");
                    writer.Write("\t" + "endfacet" + "\n");
                }
                writer.Write("endsolid " + name);
            }
        }
    }
}
