namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     AsparticAcid
    /// </summary>
    public class AsparticAcid : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.AsparticAcid;

        /// <summary>
        /// </summary>
        public new const string Name = "Aspartic Acid";

        /// <summary>
        /// </summary>
        public const string ASP = Code3L;

        /// <summary>
        /// </summary>
        public const string D = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "ASP";

        /// <summary>
        /// </summary>
        public new const string Code1L = "D";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = true;
        public new const bool Hydrophobic = false;
        public new const bool Hydroxylic = false;
        public new const bool Negative = true;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = true;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public AsparticAcid()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}