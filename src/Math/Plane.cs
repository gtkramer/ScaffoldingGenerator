using OpenTK.Mathematics;

namespace CalcNet.Spatial.Euclidean
{
    public class Plane
    {
        public Vector3 Normal { get; }
        public Point3D Point { get; }

        public Plane(Vector3 normal, Point3D point)
        {
            this.Normal = normal;
            this.Point = point;
        }

        public Point3D? GetIntersectionPoint(LineSegment3D segment)
        {
            Point3D p = segment.StartPoint;
            Vector3 m = segment.ToVector3();
            float t = GetParameter(p, m);
            if (t >= 0f && t <= 1f) {
                return p.Move(m, t);
            }
            else {
                return null;
            }
        }

        private float GetParameter(Point3D v, Vector3 m)
        {
            return Vector3.Dot(this.Normal, this.Point - v) / Vector3.Dot(this.Normal, m);
        }
    }
}
