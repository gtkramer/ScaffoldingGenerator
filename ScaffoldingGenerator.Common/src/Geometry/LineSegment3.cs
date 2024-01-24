using OpenTK.Mathematics;

namespace ScaffoldingGenerator.Geometry {
    public class LineSegment3 {

        public Point3 StartPoint { get; }
        public Point3 EndPoint { get; }

        public LineSegment3(Point3 startPoint, Point3 endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public Vector3 ToVector3() {
            return EndPoint - StartPoint;
        }
    }
}
