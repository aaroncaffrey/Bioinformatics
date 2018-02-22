namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Tryptophan
    /// </summary>
    public class Tryptophan : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Tryptophan;

        /// <summary>
        /// </summary>
        public new const string Name = "Tryptophan";

        /// <summary>
        /// </summary>
        public const string TRP = Code3L;

        /// <summary>
        /// </summary>
        public const string W = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "TRP";

        /// <summary>
        /// </summary>
        public new const string Code1L = "W";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = true;
        public new const bool Charged = false;
        public new const bool Hydrophobic = true;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = false;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public Tryptophan()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}