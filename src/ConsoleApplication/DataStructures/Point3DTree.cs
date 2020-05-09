using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.DataStructures {
	public class Point3DTree<T> where T : class {
		private static IComparer<Point3D>[] CoordComparers = {new Point3DXComparer(), new Point3DYComparer(), new Point3DZComparer()};
		private static IEqualityComparer<Point3D> DistinctComparer = new Point3DComparer();
		public Point3D[] Keys;
		public T[] Values;

		public Point3DTree(IEnumerable<Point3D> points) {
			Keys = points.Distinct(DistinctComparer).ToArray();
			Values = new T[Keys.Length];
			BuildRecursively(Keys, 0, Keys.Length, 0);
		}

		private void BuildRecursively(Point3D[] points, int index, int length, int level) {
			if (length > 1) {
				int upperIndex = index + length - 1;
				int medianIndex = (index + upperIndex) / 2;
				Array.Sort(points, index, length, CoordComparers[level % CoordComparers.Length]);
				int nextLevel = level + 1;
				BuildRecursively(points, index, medianIndex - index, nextLevel);
				BuildRecursively(points, medianIndex + 1, upperIndex - medianIndex, nextLevel);
			}
		}

		public T this[Point3D key] {
			get {
				return Values[GetValueIndexRecursively(key, 0, Keys.Length, 0)];
			}
			set {
				Values[GetValueIndexRecursively(key, 0, Keys.Length, 0)] = value;
			}
		}

		private int GetValueIndexRecursively(Point3D key, int index, int length, int level) {
			int upperIndex = index + length - 1;
			int medianIndex = (index + upperIndex) / 2;
			int comparison = CoordComparers[level % CoordComparers.Length].Compare(key, Keys[medianIndex]);
			if (comparison == 0) {
				return medianIndex;
			} else if (length > 1) {
				int nextLevel = level + 1;
				if (comparison < 0) {
					return GetValueIndexRecursively(key, index, medianIndex - index, nextLevel);
				} else {
					return GetValueIndexRecursively(key, medianIndex + 1, upperIndex - medianIndex, nextLevel);
				}
			} else {
				throw new KeyNotFoundException(key.ToString());
			}
		}
	}
}