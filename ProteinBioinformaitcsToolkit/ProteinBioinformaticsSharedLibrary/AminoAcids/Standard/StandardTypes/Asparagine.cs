namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Asparagine
    /// </summary>
    public class Asparagine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Asparagine;

        /// <summary>
        /// </summary>
        public new const string Name = "Asparagine";

        /// <summary>
        /// </summary>
        public const string ASN = Code3L;

        /// <summary>
        /// </summary>
        public const string N = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "ASN";

        /// <summary>
        /// </summary>
        public new const string Code1L = "N";

        ////public new const bool Acidic = true;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = false;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = true;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public Asparagine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }

    }
}