namespace BioinformaticsHelperLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Cysteine
    /// </summary>
    public class Cysteine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Cysteine;

        /// <summary>
        /// </summary>
        public new const string Name = "Cysteine";

        /// <summary>
        /// </summary>
        public const string CYS = Code3L;

        /// <summary>
        /// </summary>
        public const string C = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "CYS";

        /// <summary>
        /// </summary>
        public new const string Code1L = "C";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = true;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = true;
        public new const bool Sulphur = true;
        public new const bool Tiny = true;

        public Cysteine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}