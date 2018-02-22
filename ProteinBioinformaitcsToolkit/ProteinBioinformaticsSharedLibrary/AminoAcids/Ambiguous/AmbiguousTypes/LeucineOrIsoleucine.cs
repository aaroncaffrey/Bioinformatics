namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Ambiguous.AmbiguousTypes
{
    /// <summary>
    ///     LeucineOrIsoleucine
    /// </summary>
    public class LeucineOrIsoleucine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) AmbiguousAminoAcids.LeucineOrIsoleucine;

        /// <summary>
        /// </summary>
        public new const string Name = "Leucine Or Isoleucine";

        /// <summary>
        /// </summary>
        public const string XLE = Code3L;

        /// <summary>
        /// </summary>
        public const string J = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "XLE";

        /// <summary>
        /// </summary>
        public new const string Code1L = "J";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = false;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = false;
        public new const bool Positive = false;
        public new const bool Small = false;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public LeucineOrIsoleucine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}