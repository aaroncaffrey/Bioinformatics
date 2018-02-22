using System.Collections.Generic;

namespace BioinformaticsHelperLibrary.InteractionVector.temp
{
    public class InteractionVectorContainer
    {
        public List<VectorSitePart> InteractionVectorList = new List<VectorSitePart>();

        public InteractionVectorContainer()
        {
        }

        public InteractionVectorContainer(InteractionVectorContainer interactionVectorContainer)
        {
            foreach (var interactionVector in interactionVectorContainer.InteractionVectorList)
            {
                InteractionVectorList.Add(new VectorSitePart(interactionVector));
            }
        }
    }
}
