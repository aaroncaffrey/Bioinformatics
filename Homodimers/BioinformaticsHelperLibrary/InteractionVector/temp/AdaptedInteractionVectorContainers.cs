using System;
using System.Collections.Generic;
using BioinformaticsHelperLibrary.InteractionVector.temp;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class PaddedInteractionVectorContainers
    {
        public InteractionVectorContainer Unmodified;

        public InteractionVectorContainer ForwardLeftAligned;

        public InteractionVectorContainer ForwardRightAligned;

        public InteractionVectorContainer ReverseLeftAligned;

        public InteractionVectorContainer ReverseRightAligned;

        public List<InteractionVectorContainer> InteractionVectorContainerList = new List<InteractionVectorContainer>();

        public PaddedInteractionVectorContainers(InteractionVectorContainer interactionVectorContainer)
        {
            Unmodified = new InteractionVectorContainer(interactionVectorContainer);


            ForwardLeftAligned = VectorController.AdaptVectorsNormalizeLength(interactionVectorContainer, VectorAlignment.AlignLeft, VectorPaddingValue.NoInteraction);
            ForwardLeftAligned = VectorController.AdaptVectorsInsertOutsideSiteInteractionWildcard(ForwardLeftAligned, VectorOutsideSiteInteractionFlagPosition.InsertLeft);

            ForwardRightAligned = VectorController.AdaptVectorsNormalizeLength(interactionVectorContainer, VectorAlignment.AlignRight, VectorPaddingValue.NoInteraction);
            ForwardRightAligned = VectorController.AdaptVectorsInsertOutsideSiteInteractionWildcard(ForwardRightAligned, VectorOutsideSiteInteractionFlagPosition.InsertLeft);

            ReverseLeftAligned = new InteractionVectorContainer(interactionVectorContainer);
            ReverseLeftAligned = ReverseLeftAligned.


            InteractionVectorContainerList.Add(Unmodified);
            InteractionVectorContainerList.Add(ForwardLeftAligned);
            InteractionVectorContainerList.Add(ForwardRightAligned);
            InteractionVectorContainerList.Add(ReverseLeftAligned);
            InteractionVectorContainerList.Add(ReverseRightAligned);
        }



    }
}
