namespace BioinformaticsHelperLibrary.AminoAcids.NonStandard
{
    /// <summary>
    ///     Non Standard Amino Acids (resolved to standard amino acids)
    /// </summary>
    public enum NonStandardAminoAcids
    {
        /// <summary>
        ///     Selenomethionine
        /// </summary>
        Selenomethionine = Standard.StandardAminoAcids.Methionine,

        /// <summary>
        ///     Pyroglutamic acid
        /// </summary>
        PyroglutamicAcid = Standard.StandardAminoAcids.GlutamicAcid
    }
}