using MathNet.Spatial.Euclidean;

namespace ScaffoldingGenerator.Extensions {
    public static class Ext {
        public static Point3D Move(this Point3D point, UnitVector3D direction, double scalar) {
            return (point.ToVector3D() + direction.ScaleBy(scalar)).ToPoint3D();
        }

        public static Point2D ToPoint2D(this Point3D point) {
            return new Point2D(point.X, point.Y);
        }
    }
}
