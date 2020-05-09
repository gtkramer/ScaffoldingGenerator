using System;

namespace AdditiveManufacturing.Mathematics {
	public static class Tolerance {
		public static bool AlmostEqualsAbsolute(float f1, float f2, float maxAbsErr = (float)2.4735252E-6) {
			return Math.Abs(f2 - f1) <= maxAbsErr;
		}

		public static bool AlmostEqualsAbsolute(double d1, double d2, double maxAbsErr = (double)3.861206975615054E-41) {
			return Math.Abs(d2 - d1) <= maxAbsErr;
		}

		public static bool AlmostEqualsRelative(float f1, float f2, float maxRelErr = (float)2.4735252E-8) {
			if (f1 > f2) {
				float fTemp = f2;
				f2 = f1;
				f1 = fTemp;
			}
			return Math.Abs((f2 - f1) / f2) <= maxRelErr;
		}

		public static bool AlmostEqualsRelative(double d1, double d2, double maxRelErr = (double)3.861206975615054E-43) {
			if (d1 > d2) {
				double dTemp = d2;
				d2 = d1;
				d1 = dTemp;
			}
			return Math.Abs((d2 - d1) / d2) <= maxRelErr;
		}

		public static bool AlmostEqualsUlp(float f1, float f2, int maxUlpErr = 10) {
			float diff = Math.Abs(f2 - f1);
			return BitConverter.ToInt32(BitConverter.GetBytes(diff), 0) <= maxUlpErr;
		}

		public static bool AlmostEqualsUlp(double d1, double d2, long maxUlpErr = 10) {
			double diff = Math.Abs(d2 - d1);
			return BitConverter.ToInt64(BitConverter.GetBytes(diff), 0) <= maxUlpErr;
		}
	}
}
