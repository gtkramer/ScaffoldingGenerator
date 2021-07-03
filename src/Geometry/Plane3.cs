using OpenTK.Mathematics;

namespace ScaffoldingGenerator.Geometry
{
    public class Plane3
    {
        public Vector3 Normal { get; }
        public Point3 Point { get; }

        public Plane3(Vector3 normal, Point3 point)
        {
            this.Normal = normal;
            this.Point = point;
        }

        public Point3? GetIntersectionPoint(LineSegment3 segment)
        {
            Point3 p = segment.StartPoint;
            Vector3 m = segment.ToVector3();
            float t = GetParameter(p, m);
            if (t >= 0f && t <= 1f) {
                return p.Move(m, t);
            }
            else {
                return null;
            }
        }

        private float GetParameter(Point3 v, Vector3 m)
        {
            return Vector3.Dot(this.Normal, this.Point - v) / Vector3.Dot(this.Normal, m);
        }
    }
}
