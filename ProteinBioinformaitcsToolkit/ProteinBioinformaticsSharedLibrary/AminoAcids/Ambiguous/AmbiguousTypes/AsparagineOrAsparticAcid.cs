namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Ambiguous.AmbiguousTypes
{
    /// <summary>
    ///     AsparagineOrAsparticAcid
    /// </summary>
    public class AsparagineOrAsparticAcid : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) AmbiguousAminoAcids.AsparagineOrAsparticAcid;

        /// <summary>
        /// </summary>
        public new const string Name = "Asparagine Or Aspartic Acid";

        /// <summary>
        /// </summary>
        public const string ASX = Code3L;

        /// <summary>
        /// </summary>
        public const string B = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "ASX";

        /// <summary>
        /// </summary>
        public new const string Code1L = "B";

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

        public AsparagineOrAsparticAcid()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}