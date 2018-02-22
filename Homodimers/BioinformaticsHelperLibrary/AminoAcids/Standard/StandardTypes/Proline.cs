namespace BioinformaticsHelperLibrary.AminoAcids.Standard.StandardTypes
{
    /// <summary>
    ///     Proline
    /// </summary>
    public class Proline : AminoAcidProperties<bool>
    {
        /// <summary>
        /// </summary>
        public new const int Number = (int) StandardAminoAcids.Proline;

        /// <summary>
        /// </summary>
        public new const string Name = "Proline";

        /// <summary>
        /// </summary>
        public const string PRO = Code3L;

        /// <summary>
        /// </summary>
        public const string P = Code1L;

        /// <summary>
        /// </summary>
        public new const string Code3L = "PRO";

        /// <summary>
        /// </summary>
        public new const string Code1L = "P";

        //public new const bool Acidic = false;
        public new const bool Aliphatic = false;
        public new const bool Aromatic = false;
        public new const bool Charged = false;
        public new const bool Hydrophobic = false;
        public new const bool Hydroxylic = false;
        public new const bool Negative = false;
        public new const bool Polar = false;
        public new const bool Positive = false;
        public new const bool Small = true;
        public new const bool Sulphur = false;
        public new const bool Tiny = false;

        public Proline()
            : base(number: Number, name: Name, code1L: Code1L, code3L: Code3L, /*acidic: Acidic,*/ aliphatic: Aliphatic, aromatic: Aromatic, charged: Charged, hydrophobic: Hydrophobic, hydroxylic: Hydroxylic, negative: Negative, polar: Polar, positive: Positive, small: Small, sulphur: Sulphur, tiny: Tiny)
        {
        }

        
    }
}