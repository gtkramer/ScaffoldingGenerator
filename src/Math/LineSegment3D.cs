using OpenTK.Mathematics;

namespace CalcNet.Spatial.Euclidean {
    public class LineSegment3D {

        public Point3 StartPoint { get; }
        public Point3 EndPoint { get; }

        public LineSegment3D(Point3 startPoint, Point3 endPoint) {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        public Vector3 ToVector3() {
            return this.EndPoint - this.StartPoint;
        }
    }
}
