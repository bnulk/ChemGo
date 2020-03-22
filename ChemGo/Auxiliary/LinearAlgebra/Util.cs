using System;
using System.Collections.Generic;
using System.Text;

namespace ChemGo.Auxiliary.LinearAlgebra
{
    static class Util
    {
        public static double NR_SIGN(double a, double b)
        {
            return (b >= 0.0 ? Math.Abs(a) : -Math.Abs(a));
        }

        public static double pythag(double a, double b)
        /* compute (a2 + b2)^1/2 without destructive underflow or overflow */
        {
            double absa, absb;
            absa = Math.Abs(a);
            absb = Math.Abs(b);
            if (absa > absb) return absa * Math.Sqrt(1.0 + (absb / absa) * (absb / absa));
            else return (absb == 0.0 ? 0.0 : absb * Math.Sqrt(1.0 + (absa / absb) * (absa / absb)));
        }

        public static void SWAP<T> (ref T a, ref T b)
        {
            T dum = a;
            a = b;
            b = dum;
        }
    }
}
