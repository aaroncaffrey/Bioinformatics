namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Ambiguous.AmbiguousTypes
{
    /// <summary>
    ///     UnspecifiedOrUnknownAminoAcid
    /// </summary>
    public class UnspecifiedOrUnknownAminoAcid : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;

        /// <summary>
        /// </summary>
        public new const string Name = "Unspecified Or Unknown Amino Acid";

        /// <summary>
        /// </summary>
        public const string XAA = Code3L;

        /// <summary>
        /// </summary>
        public const string X = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "XAA";

        /// <summary>
        /// </summary>
        public /*new*/ const string Code3LAlt = "UNK";

        /// <summary>
        /// </summary>
        public new const string Code1L = "X";

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

        public UnspecifiedOrUnknownAminoAcid()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}