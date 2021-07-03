using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalcNet.Spatial.Euclidean {
    public class Point3D : IEquatable<Point3D>, IEqualityComparer<Point3D> {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Point3D(float x, float y, float z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3D(IEnumerable<float> floats) {
            IEnumerator<float> enumerator = floats.GetEnumerator();
            enumerator.MoveNext();
            this.X = enumerator.Current;
            enumerator.MoveNext();
            this.Y = enumerator.Current;
            enumerator.MoveNext();
            this.Z = enumerator.Current;
        }

        public static Point3D operator +(Point3D point, Vector3D vector) {
            return new Point3D(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        public static Point3D operator -(Point3D point, Vector3D vector) {
            return new Point3D(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
        }

        public static Vector3D operator -(Point3D left, Point3D right) {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static bool operator ==(Point3D left, Point3D right) {
            return left.Equals(right);
        }

        public static bool operator !=(Point3D left, Point3D right) {
            return !left.Equals(right);
        }

        public Point3D Move(Vector3D m, float t) {
            return this + t * m;
        }

        public static Point3D MidPoint(Point3D p1, Point3D p2) {
            return new Point3D((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
        }

        public float DistanceTo(Point3D point) {
            return (this - point).Magnitude();
        }

        public bool Equals(Point3D? other)
        {
            return !object.ReferenceEquals(other, null) && this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override bool Equals(object? obj) {
            return !object.ReferenceEquals(obj, null) && obj is Point3D p && this.Equals(p);
        }

        public override int GetHashCode() {
            return HashCode.Combine(this.X, this.Y, this.Z);
        }

        public bool Equals(Point3D? x, Point3D? y)
        {
            return X.Equals(y);
        }

        public int GetHashCode([DisallowNull] Point3D obj)
        {
            return obj.GetHashCode();
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y}, {this.Z})";
        }
    }
}
