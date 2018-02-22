namespace ProteinBioinformaticsSharedLibrary.AminoAcids.NonStandard.NonStandardTypes
{
    /// <summary>
    ///     Selenomethionine
    /// </summary>
    public class Selenomethionine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int)NonStandard.NonStandardAminoAcids.Selenomethionine;

        /// <summary>
        /// </summary>
        public new const string Name = "Selenomethionine";

        /// <summary>
        /// </summary>
        public const string MSE = Code3L;

        /// <summary>
        /// </summary>
        public const string M = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "MSE";

        /// <summary>
        /// </summary>
        public new const string Code1L = "M";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = true;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = false;
        public new const bool Positive = false;
        public new const bool Small = false;
        public new const bool Sulphur = true;
        public new const bool Tiny = false;

        public Selenomethionine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}