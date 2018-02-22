namespace BioinformaticsHelperLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Serine
    /// </summary>
    public class Serine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Serine;

        /// <summary>
        /// </summary>
        public new const string Name = "Serine";

        /// <summary>
        /// </summary>
        public const string SER = Code3L;

        /// <summary>
        /// </summary>
        public const string S = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "SER";

        /// <summary>
        /// </summary>
        public new const string Code1L = "S";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = false;
        public new const bool Hydroxylic = true;
        public new const bool Negative = false;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = true;
        public new const bool Sulphur = false;
        public new const bool Tiny = true;

        public Serine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}