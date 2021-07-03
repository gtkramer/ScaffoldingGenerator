using CalcNet.Spatial.Euclidean;
using System.Linq;
using OpenTK.Mathematics;

namespace ScaffoldingGenerator.Geometry
{
    public class Facet
    {
        public Vector3 Normal;
        public Point3[] Vertices;
        public Point3 Centroid;
        public LineSegment3D[] Edges;
        public Point3[] EdgeMidPoints;

        public Facet(Vector3 normal, Point3[] vertices)
        {
            Normal = normal;
            Vertices = vertices;
            Centroid = new Point3(vertices.Sum(vertex => vertex.X) / vertices.Length, vertices.Sum(vertex => vertex.Y) / vertices.Length, vertices.Sum(vertex => vertex.Z) / vertices.Length);
            Edges = new LineSegment3D[vertices.Length];
            EdgeMidPoints = new Point3[Edges.Length];
            for (int i = 0; i < Edges.Length; i++)
            {
                Edges[i] = i + 1 < Edges.Length ? new LineSegment3D(vertices[i], vertices[i + 1]) : new LineSegment3D(vertices[i], vertices[0]);
                EdgeMidPoints[i] = Point3.MidPoint(Edges[i].StartPoint, Edges[i].EndPoint);
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
