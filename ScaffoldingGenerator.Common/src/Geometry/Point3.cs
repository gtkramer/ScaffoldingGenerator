using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace ScaffoldingGenerator.Geometry {
    public class Point3 : IEquatable<Point3>, IEqualityComparer<Point3> {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Point3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
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

        public bool Equals(Point3? point)
        {
            return !object.ReferenceEquals(point, null) && X == point.X && Y == point.Y && Z == point.Z;
        }

        public override bool Equals(object? obj) {
            return !object.ReferenceEquals(obj, null) && obj is Point3 point && Equals(point);
        }

        public override int GetHashCode() {
            return HashCode.Combine(X, Y, Z);
        }

        public bool Equals(Point3? p1, Point3? p2)
        {
            return !object.ReferenceEquals(p1, null) && p1.Equals(p2);
        }

        public int GetHashCode(Point3 point)
        {
            return point.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
