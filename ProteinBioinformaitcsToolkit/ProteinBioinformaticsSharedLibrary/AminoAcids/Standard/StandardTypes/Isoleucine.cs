namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Isoleucine
    /// </summary>
    public class Isoleucine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Isoleucine;

        /// <summary>
        /// </summary>
        public new const string Name = "Isoleucine";

        /// <summary>
        /// </summary>
        public const string ILE = Code3L;

        /// <summary>
        /// </summary>
        public const string I = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "ILE";

        /// <summary>
        /// </summary>
        public new const string Code1L = "I";

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

        public Isoleucine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}