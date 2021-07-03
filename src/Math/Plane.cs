namespace CalcNet.Spatial.Euclidean
{
    public class Plane
    {
        public Vector3D Normal { get; }
        public Point3D Point { get; }

        public Plane(Vector3D normal, Point3D point)
        {
            this.Normal = normal;
            this.Point = point;
        }

        public Point3D? GetIntersectionPoint(LineSegment3D segment)
        {
            Point3D p = segment.StartPoint;
            Vector3D m = segment.ToVector3D();
            float t = GetParameter(p, m);
            if (t >= 0f && t <= 1f) {
                return p.Move(m, t);
            }
            else {
                return null;
            }
        }

        private float GetParameter(Point3D v, Vector3D m)
        {
            return this.Normal.DotProduct(this.Point - v) / this.Normal.DotProduct(m);
        }
    }
}
