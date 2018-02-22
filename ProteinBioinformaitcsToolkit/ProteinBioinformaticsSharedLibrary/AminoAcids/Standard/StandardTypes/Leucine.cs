namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Leucine
    /// </summary>
    public class Leucine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Leucine;

        /// <summary>
        /// </summary>
        public new const string Name = "Leucine";

        /// <summary>
        /// </summary>
        public const string LEU = Code3L;

        /// <summary>
        /// </summary>
        public const string L = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "LEU";

        /// <summary>
        /// </summary>
        public new const string Code1L = "L";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = true;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = true;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = false;
        public new const bool Positive = false;
        public new const bool Small = false;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public Leucine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}