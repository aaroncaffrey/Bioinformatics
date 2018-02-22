namespace BioinformaticsHelperLibrary.AminoAcids.Additional.AdditionalTypes
{
    /// <summary>
    ///     Pyrrolysine
    /// </summary>
    public class Pyrrolysine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) AdditionalAminoAcids.Pyrrolysine;

        /// <summary>
        /// </summary>
        public new const string Name = "Pyrrolysine";

        /// <summary>
        /// </summary>
        public const string PYL = Code3L;

        /// <summary>
        /// </summary>
        public const string O = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "PYL";

        /// <summary>
        /// </summary>
        public new const string Code1L = "O";

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

        public Pyrrolysine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}