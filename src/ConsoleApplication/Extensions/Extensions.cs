using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.Extensions {
	public static class MyExt {
		public static Point3D Midpoint(this Line3D line) {
			return Point3D.MidPoint(line.StartPoint, line.EndPoint);
		}
	}
}
