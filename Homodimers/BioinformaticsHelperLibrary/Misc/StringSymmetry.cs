using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio.Util;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class StringSymmetry
    {
        public static decimal StringSymmetryPercentage(string str, bool mirror = true, bool round = true)
        {
            if (String.IsNullOrEmpty(str)) return 0m; //throw new ArgumentNullException(nameof(str));

            if (string.IsNullOrWhiteSpace(str) || str.Length == 1) return 100m;

            var isOdd = str.Length % 2 == 1;
            var mid = str.Length / 2;

            var str1 = str.Substring(0, mid);
            var str2 = string.Join("", str.Substring(mid + (isOdd ? 1 : 0), str.Length - (mid + (isOdd ? 1 : 0))));

            if (mirror)
            {
                //str2 = str2.Reverse();
                var x = str2.ToCharArray();
                Array.Reverse(x);
                str2 = new string(x);
            }

            var eq = 0m;// isOdd ? 1m : 0m;

            for (var i = 0; i < str1.Length; i++)
            {
                if (str1[i] == str2[i]) eq++;
            }

            var len = str1.Length;// + (isOdd ? 1 : 0);

            eq = ((decimal) eq/(decimal) len)*100m;

            if (round)
            {
                eq = Math.Round(eq, 0);
            }

            return eq;

        }
    }
}
