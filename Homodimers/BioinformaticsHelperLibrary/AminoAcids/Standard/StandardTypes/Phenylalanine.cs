namespace BioinformaticsHelperLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Phenylalanine
    /// </summary>
    public class Phenylalanine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Phenylalanine;

        /// <summary>
        /// </summary>
        public new const string Name = "Phenylalanine";

        /// <summary>
        /// </summary>
        public const string PHE = Code3L;

        /// <summary>
        /// </summary>
        public const string F = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "PHE";

        /// <summary>
        /// </summary>
        public new const string Code1L = "F";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = true;
        public new const bool Charged = false;
        public new const bool Hydrophobic = true;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = false;
        public new const bool Positive = false;
        public new const bool Small = false;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public Phenylalanine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}