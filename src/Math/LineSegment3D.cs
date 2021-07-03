namespace CalcNet.Spatial.Euclidean {
    public class LineSegment3D {

        public Point3D StartPoint { get; }
        public Point3D EndPoint { get; }

        public LineSegment3D(Point3D startPoint, Point3D endPoint) {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        public Vector3D ToVector3D() {
            return this.EndPoint - this.StartPoint;
        }
    }
}
