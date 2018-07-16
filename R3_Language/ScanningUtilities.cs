using System;

namespace R3_Language
{
    /// <summary>
    /// 
    /// </summary>
    public static class ScanningUtilities
    {

        public static string CharToString(char c)
        {
            return c.ToString();
        }

        public static double IntToDouble(int i)
        {
            return Convert.ToDouble(i);
        }

        public static int DoubleToInt(double d)
        {
            return Convert.ToInt32(d);
        }

        public static int RoundToInt(double x) { return (int)Math.Round(x); }

        public static int Ceil(double x) { return (int)Math.Ceiling(x); }

        public static int Floor(double x) { return (int)Math.Floor(x); }

        public static double RoundTo(double x, int places)
        {
            double scale = Math.Pow(10, places);
            return Math.Round(x * scale) / scale;
        }

        private static double _tolerance = 0.0000001;

        public static Boolean EqualsApprox(double d1, double d2)
        {
            return ScanningUtilities.EqualsApprox(d1, d2, _tolerance);
        }

        public static Boolean EqualsApprox(double d1, double d2, double relativeTolerance)
        {
            double tol = Math.Max(relativeTolerance * Math.Max(Math.Abs(d1), Math.Abs(d2)),
                    Double.MinValue);
            // TO FIX: 0.0 is never approx-equal to any non-zero value.  Is this fixable?
            // Take exp() of both numbers?
            return (d1 - tol <= d2 && d2 <= d1 + tol) && (d2 - tol <= d1 && d1 <= d2 + tol);
        }

    
    }
}
