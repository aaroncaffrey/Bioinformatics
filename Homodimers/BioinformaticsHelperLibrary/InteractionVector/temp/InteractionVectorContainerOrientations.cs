using System.Collections.Generic;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.InteractionVector.temp
{
    public class InteractionVectorContainerOrientations
    {
        public PaddedInteractionVectorContainers InteractionVectorContainerUnchanged;

        public PaddedInteractionVectorContainers InteractionVectorContainerReversedChainA;

        public PaddedInteractionVectorContainers InteractionVectorContainerReversedChainB;

        public PaddedInteractionVectorContainers InteractionVectorContainerReversedChainAAndB;

        public Dictionary<string, PaddedInteractionVectorContainers> AdaptedInteractionVectorContainersList = new Dictionary<string, PaddedInteractionVectorContainers>();

        public InteractionVectorContainerOrientations(InteractionVectorContainer interactionVectorContainer)
        {
            InteractionVectorContainerUnchanged = new PaddedInteractionVectorContainers(new InteractionVectorContainer(interactionVectorContainer));
            AdaptedInteractionVectorContainersList.Add("InteractionVectorContainerUnchanged", InteractionVectorContainerUnchanged);

            InteractionVectorContainerReversedChainA = new PaddedInteractionVectorContainers(VectorController.AdaptVectorsReverseBitOrder(interactionVectorContainer, StaticValues.ChainA));
            AdaptedInteractionVectorContainersList.Add("InteractionVectorContainerReversedChainA", InteractionVectorContainerReversedChainA);

            InteractionVectorContainerReversedChainB = new PaddedInteractionVectorContainers(VectorController.AdaptVectorsReverseBitOrder(interactionVectorContainer, StaticValues.ChainB));
            AdaptedInteractionVectorContainersList.Add("InteractionVectorContainerReversedChainB", InteractionVectorContainerReversedChainB);

            InteractionVectorContainerReversedChainAAndB = new PaddedInteractionVectorContainers(VectorController.AdaptVectorsReverseBitOrder(interactionVectorContainer));
            AdaptedInteractionVectorContainersList.Add("InteractionVectorContainerReversedChainAAndB", InteractionVectorContainerReversedChainAAndB);
        }
    }
}
