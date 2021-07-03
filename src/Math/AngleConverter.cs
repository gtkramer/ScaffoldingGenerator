using System;
namespace CalcNet.Spatial.Euclidean {
    public static class AngleConverter {
        public static float RadToDeg(float radians) {
            return radians * 180 / MathF.PI;
        }

        public static double RadToDeg(double radians) {
            return radians * 180 / Math.PI;
        }

        public static float DegToRad(float degrees) {
            return degrees * MathF.PI / 180;
        }

        public static double DegToRad(double degrees) {
            return degrees * Math.PI / 180;
        }
    }
}
