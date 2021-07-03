using OpenTK.Mathematics;

namespace ScaffoldingGenerator.Geometry {
    public class LineSegment3 {

        public Point3 StartPoint { get; }
        public Point3 EndPoint { get; }

        public LineSegment3(Point3 startPoint, Point3 endPoint) {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        public Vector3 ToVector3() {
            return this.EndPoint - this.StartPoint;
        }
    }
}
