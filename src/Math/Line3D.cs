namespace CalcNet.Spatial.Euclidean {
    public class Line3D {

        public Point3D StartPoint { get; }
        public Point3D EndPoint { get; }

        public Line3D(Point3D startPoint, Point3D endPoint) {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        public Vector3D ToVector3D() {
            return this.EndPoint - this.StartPoint;
        }
    }
}
