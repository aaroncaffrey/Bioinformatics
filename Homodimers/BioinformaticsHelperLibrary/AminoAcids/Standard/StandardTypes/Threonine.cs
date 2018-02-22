namespace BioinformaticsHelperLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Threonine
    /// </summary>
    public class Threonine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Threonine;

        /// <summary>
        /// </summary>
        public new const string Name = "Threonine";

        /// <summary>
        /// </summary>
        public const string THR = Code3L;

        /// <summary>
        /// </summary>
        public const string T = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "THR";

        /// <summary>
        /// </summary>
        public new const string Code1L = "T";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = true;
        public new const bool Hydroxylic = true;
        public new const bool Negative = false;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = true;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public Threonine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}