namespace ProteinBioinformaticsSharedLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Glutamine
    /// </summary>
    public class Glutamine : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Glutamine;

        /// <summary>
        /// </summary>
        public new const string Name = "Glutamine";

        /// <summary>
        /// </summary>
        public const string GLN = Code3L;

        /// <summary>
        /// </summary>
        public const string Q = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "GLN";

        /// <summary>
        /// </summary>
        public new const string Code1L = "Q";

        //public new const bool Acidic = true;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = false;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = true;
        public new const bool Positive = false;
        public new const bool Small = false;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public Glutamine()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }
    }
}