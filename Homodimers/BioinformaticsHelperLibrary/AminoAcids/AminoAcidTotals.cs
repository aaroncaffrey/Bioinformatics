using System;
using BioinformaticsHelperLibrary.AminoAcids.Additional;
using BioinformaticsHelperLibrary.AminoAcids.Ambiguous;
using BioinformaticsHelperLibrary.AminoAcids.Standard;

namespace BioinformaticsHelperLibrary.AminoAcids
{
    public class AminoAcidTotals
    {
        /// <summary>
        ///     Returns the total number of standard Amino Acids (20)
        /// </summary>
        /// <returns></returns>
        public static int TotalStandardAminoAcids()
        {
            return Enum.GetNames(typeof (StandardAminoAcids1L)).Length;
        }

        /// <summary>
        ///     Returns the total number of additional Amino Acids (2)
        /// </summary>
        /// <returns></returns>
        public static int TotalAdditionalAminoAcids()
        {
            return Enum.GetNames(typeof (AdditionalAminoAcids1L)).Length;
        }

        /// <summary>
        ///     Returns the total number of ambiguous Amino Acids (4)
        /// </summary>
        /// <returns></returns>
        public static int TotalAmbiguousAminoAcids()
        {
            return Enum.GetNames(typeof (AmbiguousAminoAcids1L)).Length;
        }

        /// <summary>
        ///     Returns the total number of standard plus additional plus ambiguous Amino Acids which have known representations
        ///     (26)
        /// </summary>
        /// <returns></returns>
        public static int TotalAminoAcids()
        {
            return TotalStandardAminoAcids() + TotalAdditionalAminoAcids() + TotalAmbiguousAminoAcids();
        }
    }
}