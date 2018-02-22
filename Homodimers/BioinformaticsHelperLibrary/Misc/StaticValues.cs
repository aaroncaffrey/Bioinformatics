namespace BioinformaticsHelperLibrary.Misc
{
    public class StaticValues
    {
        /// <summary>
        ///     Carbon Alpha acronym as found in PDB files ATOM records.
        /// </summary>
        public const string CarbonAlpha = "CA";

        /// <summary>
        ///     Chain A is always found at index 0.
        /// </summary>
        public const int ChainA = 0;

        /// <summary>
        ///     Chain B is always found at index 1.
        /// </summary>
        public const int ChainB = 1;

        public const int ProteinInterfaceA = 0;

        public const int ProteinInterfaceB = 1;

        public const int ProteinInterfaceC = 2;


        public static readonly string[] MolNameProteinAcceptedValues = new string[] { null, "", "protein" };
    }
}