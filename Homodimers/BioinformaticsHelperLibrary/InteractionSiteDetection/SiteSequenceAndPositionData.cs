namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class ProteinInterfaceSequenceAndPositionData
    {
        public FullProteinInterfaceId FullProteinInterfaceId = new FullProteinInterfaceId();
        public ProteinInterfaceAminoAcidMetaData[] AminoAcidSequenceAllResidueSequenceIndexes;
        public string AminoAcidSequenceAll1L;
        public string AminoAcidSequenceInteractionsAll1L;
        public string AminoAcidSequenceInteractionsInsideProteinInterfacesOnly1L;
        public string AminoAcidSequenceInteractionsNone1L;
        public string AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly1L;

        public string AminoAcidSequenceAll3L;
        public string AminoAcidSequenceInteractionsAll3L;
        public string AminoAcidSequenceInteractionsInsideProteinInterfacesOnly3L;
        public string AminoAcidSequenceInteractionsNone3L;
        public string AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly3L;

        //public int ChainIdIndex;
        public string ChainIdLetter;
        public int EndPosition;
        //public string ProteinId;
        //public int ProteinInterfaceIdIndex;
        public string ProteinInterfaceIdLetter;
        public int ProteinInterfaceLength;
        public int StartPosition;
    }
}