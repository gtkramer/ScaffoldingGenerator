using OpenTK.Mathematics;

namespace CalcNet.Spatial.Euclidean
{
    public class Plane
    {
        public Vector3 Normal { get; }
        public Point3 Point { get; }

        public Plane(Vector3 normal, Point3 point)
        {
            this.Normal = normal;
            this.Point = point;
        }

        public Point3? GetIntersectionPoint(LineSegment3D segment)
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
