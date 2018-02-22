using System.Collections.Generic;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class ProteinInterfaceAnalysisResultData
    {
        public int[] DetectedBestStageIndexes = null;

        /// <summary>
        ///     InteractionProteinInterfaceClusteringHierarchyData
        /// </summary>
        public List<InteractionProteinInterfaceClusteringHierarchyData> InteractionProteinInterfaceClusteringHierarchyData = null;

        /// <summary>
        ///     InteractionsBetweenProteinInterfacesList
        /// </summary>
        public InteractionBetweenProteinInterfacesListContainer InteractionsBetweenProteinInterfacesList = null;

        public ClusteringFullResultListContainer ProteinInterfacesClusteringResult;

        //public int NumberChainsWithCorrectProteinInterfaceTally = -1;

        /// <summary>
        ///     ProteinInterfacePositionDataList
        /// </summary>
        public List<ProteinInterfaceSequenceAndPositionData> ProteinInterfacesSequenceAndPositionDataList = null;

        public ProteinInterfaceAnalysisResultData(int[] detectedBestStageIndexes, ClusteringFullResultListContainer proteinInterfacesClusteringResult, List<InteractionProteinInterfaceClusteringHierarchyData> interactionProteinInterfaceClusteringHierarchyData, InteractionBetweenProteinInterfacesListContainer interactionsBetweenProteinInterfacesList, List<ProteinInterfaceSequenceAndPositionData> proteinInterfacesSequenceAndPositionDataList)
        {
            DetectedBestStageIndexes = detectedBestStageIndexes;
            ProteinInterfacesClusteringResult = proteinInterfacesClusteringResult;
            InteractionProteinInterfaceClusteringHierarchyData = interactionProteinInterfaceClusteringHierarchyData;
            InteractionsBetweenProteinInterfacesList = interactionsBetweenProteinInterfacesList;
            ProteinInterfacesSequenceAndPositionDataList = proteinInterfacesSequenceAndPositionDataList;
        }
    }
}