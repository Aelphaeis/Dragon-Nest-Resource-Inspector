using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonNest.ResourceInspector
{
    public static class Levenshtein
    {
        public static Int32 Distance(String a, String b)
        {

            if (string.IsNullOrEmpty(a))
                return (!string.IsNullOrEmpty(b)) ? b.Length : 0;

            if (string.IsNullOrEmpty(b))
                return (!string.IsNullOrEmpty(a)) ? a.Length : 0;

            int[,] d = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= d.GetUpperBound(0); i += 1)
                d[i, 0] = i;
            for (int i = 0; i <= d.GetUpperBound(1); i += 1)
                d[0, i] = i;
            for (int i = 1; i <= d.GetUpperBound(0); i += 1)
                for (Int32 j = 1; j <= d.GetUpperBound(1); j += 1)
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + ((!(a[i - 1] == b[j - 1]))? 1 : 0));

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }

        public static double Percentage(String a, String b)
        {
            double i = Distance(a, b);
            double d = (a.Length > b.Length) ? a.Length : b.Length;
            return (d - i) / d;
        }
    }

}
