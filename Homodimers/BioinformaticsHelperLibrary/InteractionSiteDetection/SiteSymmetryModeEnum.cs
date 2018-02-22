namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public enum ProteinInterfaceSymmetryModeEnum
    {
        Invalid = -1,
        Unknown = 0,
        Straight = 1,
        Diagonal = 2,
        StraightAndDiagonal = 3,
        DiagonalAndTopStraight = 4,
        DiagonalAndBottomStraight = 5,
        StraightAndTopToBottomDiagonal = 6,
        StraightAndBottomToTopDiagonal = 7
    }
}