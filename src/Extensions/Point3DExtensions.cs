using System;
using MathNet.Spatial.Euclidean;

namespace ScaffoldingGenerator.Extensions {
    public static class Point3DExtensions {
        public static Point3D Move(this Point3D point, UnitVector3D direction, double scalar) {
            return point + direction.ScaleBy(scalar);
        }

        public static double DistanceTo2D(this Point3D start, Point3D end) {
            double diffX = end.X - start.X;
            double diffY = end.Y - start.Y;
            return Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }
    }
}
