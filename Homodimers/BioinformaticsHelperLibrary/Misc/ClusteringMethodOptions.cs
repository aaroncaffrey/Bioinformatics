namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    ///     Enum of possible options to use when clustering.
    /// </summary>
    public enum ClusteringMethodOptions
    {
        /// <summary>
        ///     Use the x, y, z field values of ATOM records.
        /// </summary>
        ClusterWithPosition3D,

        /// <summary>
        ///     Use the resSeq field values of ATOM records.
        /// </summary>
        ClusterWithResidueSequenceIndex
    }
}