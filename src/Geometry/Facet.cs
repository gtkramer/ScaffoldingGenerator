using CalcNet.Spatial.Euclidean;
using System.Linq;

namespace ScaffoldingGenerator.Geometry
{
    public class Facet
    {
        public Vector3D Normal;
        public Point3D[] Vertices;
        public Point3D Centroid;
        public Line3D[] Edges;
        public Point3D[] EdgeMidPoints;

        public Facet(Vector3D normal, Point3D[] vertices)
        {
            Normal = normal;
            Vertices = vertices;
            Centroid = new Point3D(vertices.Sum(vertex => vertex.X) / vertices.Length, vertices.Sum(vertex => vertex.Y) / vertices.Length, vertices.Sum(vertex => vertex.Z) / vertices.Length);
            Edges = new Line3D[vertices.Length];
            EdgeMidPoints = new Point3D[Edges.Length];
            for (int i = 0; i < Edges.Length; i++)
            {
                Edges[i] = i + 1 < Edges.Length ? new Line3D(vertices[i], vertices[i + 1]) : new Line3D(vertices[i], vertices[0]);
                EdgeMidPoints[i] = Point3D.MidPoint(Edges[i].StartPoint, Edges[i].EndPoint);
            }
        }

        public override string ToString()
        {
            string output = "Normal\n" + Normal.ToString() + "\nVertices:\n";
            output += string.Join("\n", Vertices.Select((x) => x.ToString()));
            return output;
        }
    }
}
