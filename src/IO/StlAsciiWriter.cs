using System;
using System.IO;
using AdditiveManufacturing.Geometry;
using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.IO
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
                    Vector3D normal = facet.Normal;
                    writer.Write("\t" + "facet normal " + normal.X + " " + normal.Y + " " + normal.Z + "\n");
                    writer.Write("\t\t" + "outer loop" + "\n");
                    foreach (Point3D vertex in facet.Vertices)
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
