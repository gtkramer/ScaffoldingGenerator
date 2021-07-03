using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using OpenTK.Mathematics;

namespace CalcNet.Spatial.Euclidean {
    public class Point3 : IEquatable<Point3>, IEqualityComparer<Point3> {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Point3(float x, float y, float z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Point3 operator +(Point3 point, Vector3 vector) {
            return new Point3(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        public static Point3 operator -(Point3 point, Vector3 vector) {
            return new Point3(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
        }

        public static Vector3 operator -(Point3 left, Point3 right) {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static bool operator ==(Point3 left, Point3 right) {
            return left.Equals(right);
        }

        public static bool operator !=(Point3 left, Point3 right) {
            return !left.Equals(right);
        }

        public Point3 Move(Vector3 m, float t) {
            return this + t * m;
        }

        public static Point3 MidPoint(Point3 p1, Point3 p2) {
            return new Point3((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
        }

        public float DistanceTo(Point3 point) {
            return (this - point).Length;
        }

        public bool Equals(Point3? other)
        {
            return !object.ReferenceEquals(other, null) && this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override bool Equals(object? obj) {
            return !object.ReferenceEquals(obj, null) && obj is Point3 p && this.Equals(p);
        }

        public override int GetHashCode() {
            return HashCode.Combine(this.X, this.Y, this.Z);
        }

        public bool Equals(Point3? x, Point3? y)
        {
            return X.Equals(y);
        }

        public int GetHashCode([DisallowNull] Point3 obj)
        {
            return obj.GetHashCode();
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y}, {this.Z})";
        }
    }
}
