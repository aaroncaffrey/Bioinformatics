using System;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    [Flags]
    public enum ProteinInterfaceInteractionType
    {
        NoInteractionFound = 1,
        InteractionWithNonProteinInterface = 2,
        InteractionWithAnotherProteinInterface = 4,
        InteractionWithAnotherProteinInterfaceAndNonProteinInterface = InteractionWithAnotherProteinInterface | InteractionWithNonProteinInterface
    }
}