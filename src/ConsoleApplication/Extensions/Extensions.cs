using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.Extensions {
    public static class MyExt {
        public static Point3D Midpoint(this Line3D line) {
            Point3D sp = line.StartPoint;
            Point3D ep = line.EndPoint;
            double midX = (sp.X + ep.X) / 2;
            double midY = (sp.Y + ep.Y) / 2;
            double midZ = (sp.Z + ep.Z) / 2;
            return new Point3D(midX, midY, midZ);
        }
    }
}
