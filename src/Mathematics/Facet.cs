using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.Mathematics {
	public class Facet {
		public Vector3D Normal;
		public Point3D[] Vertices;
		public Line3D[] Edges;
		public Point3D MinPoint;
		public Point3D MaxPoint;

		public Facet(Vector3D normal, Point3D[] vertices) {
			Normal = normal;
			Vertices = vertices;
			Edges = new Line3D[vertices.Length];
			for (int i = 0; i < Edges.Length; i++) {
				Edges[i] = i + 1 < Edges.Length ? new Line3D(vertices[i], vertices[i + 1]) : new Line3D(vertices[i], vertices[0]);
			}

			double minX, minY, minZ;
			double maxX, maxY, maxZ;
			minX = minY = minZ = double.MaxValue;
			maxX = maxY = maxZ = double.MinValue;
			foreach (Point3D vertex in vertices) {
				if (vertex.X < minX) {
					minX = vertex.X;
				}
				if (vertex.Y < minY) {
					minY = vertex.Y;
				}
				if (vertex.Z < minZ) {
					minZ = vertex.Z;
				}

				if (vertex.X > maxX) {
					maxX = vertex.X;
				}
				if (vertex.Y > maxY) {
					maxY = vertex.Y;
				}
				if (vertex.Z > maxZ) {
					maxZ = vertex.Z;
				}
			}
			MinPoint = new Point3D(minX, minY, minZ);
			MaxPoint = new Point3D(maxX, maxY, maxZ);
		}
	}
}
