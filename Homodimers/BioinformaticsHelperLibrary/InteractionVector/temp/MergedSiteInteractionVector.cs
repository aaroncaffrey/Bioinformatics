using System;
using System.Collections.Generic;
using System.Linq;
using BioinformaticsHelperLibrary.InteractionSiteDetection;

namespace BioinformaticsHelperLibrary.InteractionVector.temp
{
    public class MergedSiteInteractionVector
    {
        public FullSiteId FullSiteId = new FullSiteId();
        
        public bool[] InteractionVectorLeftAligned;
        public bool[] InteractionVectorRightAligned;

        public bool[] InteractionVectorReversedLeftAligned;
        public bool[] InteractionVectorReversedRightAligned;


        public bool[] ConvertInteractionVectorContainerToBoolArray(InteractionVectorContainer interactionVectorContainer, FullSiteId fullSiteId)
        {
            var mergedBools = new bool[interactionVectorContainer.InteractionVectorList.Where(a => a.FullSiteId == fullSiteId).Select(a => a.InteractionFlagBools.Length).Sum()];

            var offset = 0;
            foreach (var interactionVector in interactionVectorContainer.InteractionVectorList.Where(a => a.FullSiteId == fullSiteId))
            {
                Array.Copy(interactionVector.InteractionFlagBools, 0, mergedBools, offset, interactionVector.InteractionFlagBools.Length);
                offset += interactionVector.InteractionFlagBools.Length;
            }

            return mergedBools;
        }

        public MergedSiteInteractionVector(InteractionVectorContainerOrientations interactionVectorContainerOrders)
        {
            var fullSiteIdList = interactionVectorContainerOrders.InteractionVectorContainerUnchanged.InteractionVectorContainerUnchanged.InteractionVectorList.Select(a => a.FullSiteId).Distinct().ToList();

            Dictionary<string, Dictionary<FullSiteId, bool[]>> mergedVectorsDictionary = new Dictionary<string, Dictionary<FullSiteId, bool[]>>();

            foreach (var fullSiteId in fullSiteIdList)
            {
                //foreach (var adaptedInteractionVectorContainersKvp in interactionVectorContainerOrders.AdaptedInteractionVectorContainersList.Where(a=>a.Value.InteractionVectorContainerUnchanged.InteractionVectorList.))
                //{
                //    var name = adaptedInteractionVectorContainersKvp.Key;
                //    var value = adaptedInteractionVectorContainersKvp.Value;

                //    var originalSiteLength = value.InteractionVectorContainerUnchanged.InteractionVectorList.
                //    ConvertInteractionVectorContainerToBoolArray(value.InteractionVectorContainerLeftAlignedWithWildcard);
                //}
            }
        }
    }
}
