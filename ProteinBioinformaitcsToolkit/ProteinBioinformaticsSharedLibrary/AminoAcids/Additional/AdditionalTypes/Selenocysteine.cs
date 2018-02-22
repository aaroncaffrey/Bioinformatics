namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Additional.AdditionalTypes
{
    /// <summary>
    ///     Selenocysteine
    /// </summary>
    public class Selenocysteine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) AdditionalAminoAcids.Selenocysteine;

        /// <summary>
        /// </summary>
        public new const string Name = "Selenocysteine";

        /// <summary>
        /// </summary>
        public const string SEC = Code3L;

        /// <summary>
        /// </summary>
        public const string U = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "SEC";

        /// <summary>
        /// </summary>
        public new const string Code1L = "U";

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

        public Selenocysteine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }

        
    }
}