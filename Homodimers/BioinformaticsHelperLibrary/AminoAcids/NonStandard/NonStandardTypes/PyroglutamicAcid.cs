namespace BioinformaticsHelperLibrary.AminoAcids.NonStandard.NonStandardTypes
{
    /// <summary>
    ///     PyroglutamicAcid
    /// </summary>
    public class PyroglutamicAcid : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) NonStandard.NonStandardAminoAcids.PyroglutamicAcid;

        /// <summary>
        /// </summary>
        public new const string Name = "Pyroglutamic acid";

        /// <summary>
        /// </summary>
        public const string PCA = Code3L;

        /// <summary>
        /// </summary>
        public const string E = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "PCA";

        /// <summary>
        /// </summary>
        public new const string Code1L = "E";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = true;
        public new const bool Hydrophobic = false;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = false;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public PyroglutamicAcid()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}