using System;
using System.Collections.Generic;

namespace CalcNet.Spatial.Euclidean {
    public class Vector3D : IEquatable<Vector3D> {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Vector3D(float x, float y, float z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3D(IEnumerable<float> floats) {
            IEnumerator<float> enumerator = floats.GetEnumerator();
            enumerator.MoveNext();
            this.X = enumerator.Current;
            enumerator.MoveNext();
            this.Y = enumerator.Current;
            enumerator.MoveNext();
            this.Z = enumerator.Current;
        }

        public static Vector3D operator +(Vector3D left, Vector3D right) {
            return new Vector3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3D operator -(Vector3D left, Vector3D right) {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3D operator *(Vector3D vector, float scale) {
            return new Vector3D(vector.X * scale, vector.Y * scale, vector.Z * scale);
        }

        public static Vector3D operator *(float scale, Vector3D vector) {
            return vector * scale;
        }

        public static Vector3D operator /(Vector3D vector, float scale) {
            return new Vector3D(vector.X / scale, vector.Y / scale, vector.Z / scale);
        }

        public static Vector3D operator /(float scale, Vector3D vector) {
            return vector / scale;
        }

        public static bool operator ==(Vector3D left, Vector3D right) {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3D left, Vector3D right) {
            return !left.Equals(right);
        }

        public float DotProduct(Vector3D v) {
            return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
        }

        public Vector3D CrossProduct(Vector3D v) {
            return new Vector3D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);
        }

        public float Magnitude() {
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        public float AngleTo(Vector3D v) {
            return (float)(Math.Acos(this.DotProduct(v) / (this.Magnitude() * v.Magnitude())) * 180 / Math.PI);
        }

        public Vector3D Normalize() {
            return this / this.Magnitude();
        }

        public bool Equals(Vector3D? other)
        {
            return !object.ReferenceEquals(other, null) && this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override bool Equals(object? obj) {
            return !object.ReferenceEquals(obj, null) && obj is Point3D p && this.Equals(p);
        }

        public override int GetHashCode() {
            return HashCode.Combine(this.X, this.Y, this.Z);
        }

        public override string ToString()
        {
            return $"<{this.X}, {this.Y}, {this.Z}>";
        }
    }
}
