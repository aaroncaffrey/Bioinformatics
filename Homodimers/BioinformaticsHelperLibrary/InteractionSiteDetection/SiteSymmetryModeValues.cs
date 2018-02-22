namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class ProteinInterfaceSymmetryModeValues
    {
        public int NumberInteractionsDiagonal;
        public int NumberInteractionsStraight;
        public ProteinInterfaceSymmetryModeEnum ProteinInterfaceSymmetryModeEnum;

        public ProteinInterfaceSymmetryModeValues(int numberInteractionsDiagonal, int numberInteractionsStraight, ProteinInterfaceSymmetryModeEnum proteinInterfaceSymmetryModeEnum)
        {
            NumberInteractionsDiagonal = numberInteractionsDiagonal;
            NumberInteractionsStraight = numberInteractionsStraight;
            ProteinInterfaceSymmetryModeEnum = proteinInterfaceSymmetryModeEnum;
        }
    }
}