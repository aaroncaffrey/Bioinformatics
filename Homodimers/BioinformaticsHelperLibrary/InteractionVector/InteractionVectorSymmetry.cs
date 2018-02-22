using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public static class InteractionVectorSymmetry
    {
        public static decimal VectorSymmetryPercentage(string str, bool mirror = true)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var outsideInteractionVector = "";

            var vectors = str.Split(' ').ToList();

            if (vectors.Select(a => a.Length).Distinct().Count() != 1)
            {
                //throw new ArgumentOutOfRangeException(nameof(str), "Vectors must be the same length");
            }

            var isOdd = vectors.Count % 2 == 1;
            var mid = vectors.Count / 2;

            if (isOdd && vectors.Count > 1) vectors.RemoveAt(mid);

            for (int index = 0; index < vectors.Count; index++)
            {
                var vector = vectors[index];
                if (vector.Contains("+1")) outsideInteractionVector += "1";
                if (vector.Contains("+0")) outsideInteractionVector += "0";

                vectors[index] = vector.Replace("+1", "").Replace("+0", "");
            }

            str = String.Join("", vectors);


            var strSym = StringSymmetry.StringSymmetryPercentage(str, mirror, false);

            var outsideInteractionVectorSym = StringSymmetry.StringSymmetryPercentage(outsideInteractionVector, mirror, false);

            decimal totalLen = str.Length + outsideInteractionVector.Length;

            var finalSym1 = (decimal)strSym * ((decimal)str.Length / (decimal)totalLen);
            var finalSym2 = (decimal)outsideInteractionVectorSym * ((decimal)outsideInteractionVector.Length / (decimal)totalLen);

            var finalSym = finalSym1 + finalSym2;

            finalSym = Math.Round(finalSym, 0);

            return finalSym;
        }
    }
}
