using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class ProteinInterfaceTreeOptimalStageDetection
    {
        public static int[] FindFinalProteinInterfaceStageIndexes(
            ProteinChainListContainer chainInteractingAtomLists,
            ClusteringFullResultListContainer clusters,
            ClusteringFullResultListContainer proteinInterfaces,
            List<List<int>> chainStageProteinInterfaceCount)
        {
            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(clusters))
            {
                throw new ArgumentOutOfRangeException(nameof(clusters));
            }

            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(proteinInterfaces))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfaces));
            }

            if (ParameterValidation.IsListNullOrEmpty(chainStageProteinInterfaceCount))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            int totalChains = chainInteractingAtomLists.ChainList.Count;
            var result = new int[totalChains];

            for (int chainIndex = 0; chainIndex < totalChains; chainIndex++)
            {
                result[chainIndex] = -1;

                // Find the Max proteinInterfaces in proteinInterfaces list
                int chainMaxProteinInterfaceCount = chainStageProteinInterfaceCount[chainIndex].Max();
                int chainMaxProteinInterfaceCountLastIndex = chainStageProteinInterfaceCount[chainIndex].LastIndexOf(chainMaxProteinInterfaceCount);

                for (int stageIndex = chainMaxProteinInterfaceCountLastIndex + 1; stageIndex < proteinInterfaces.ChainList[chainIndex].StageList.Count; stageIndex++)
                {
                    ClusteringFullResultListContainer.Chain.Stage proteinInterfacesLastStage = proteinInterfaces.ChainList[chainIndex].StageList[stageIndex - 1];
                    ClusteringFullResultListContainer.Chain.Stage proteinInterfacesThisStage = proteinInterfaces.ChainList[chainIndex].StageList[stageIndex];

                    // Check proteinInterfaces in proteinInterfacesThisStage contains the member of proteinInterfaces in proteinInterfacesLastStage
                    foreach (ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterfaceLastStage in proteinInterfacesLastStage.ClusterList)
                    {
                        foreach (int lastProteinInterfaceMember in proteinInterfaceLastStage.AtomIndexList)
                        {
                            bool lastProteinInterfaceMemberFound = false;

                            foreach (ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterfaceThisStage in proteinInterfacesThisStage.ClusterList)
                            {
                                if (proteinInterfaceThisStage.AtomIndexList.Contains(lastProteinInterfaceMember))
                                {
                                    lastProteinInterfaceMemberFound = true;
                                    break;
                                }
                            }

                            if (!lastProteinInterfaceMemberFound)
                            {
                                result[chainIndex] = stageIndex - 1;
                                break;
                            }
                        }

                        if (result[chainIndex] == stageIndex - 1)
                        {
                            break;
                        }
                    }

                    if (result[chainIndex] == stageIndex - 1)
                    {
                        break;
                    }
                }

                if (result[chainIndex] == -1)
                {
                    result[chainIndex] = chainMaxProteinInterfaceCountLastIndex;
                }
            }

            return result;
        }

        public static int[] FindLastMatchingProteinInterfaceStageIndexes(List<List<int>> chainStageProteinInterfaceCount, int numberProteinInterfacesRequred = 2)
        {
            if (ParameterValidation.IsListNullOrEmpty(chainStageProteinInterfaceCount))
            {
                throw new ArgumentOutOfRangeException(nameof(chainStageProteinInterfaceCount));
            }

            if (numberProteinInterfacesRequred < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberProteinInterfacesRequred));
            }

            var bestIndex = new int[chainStageProteinInterfaceCount.Count];

            for (int chainIndex = 0; chainIndex < chainStageProteinInterfaceCount.Count; chainIndex++)
            {
                bestIndex[chainIndex] = -1;

                for (int stageIndex = chainStageProteinInterfaceCount[chainIndex].Count - 1; stageIndex >= 0; stageIndex--)
                {
                    int proteinInterfaceCount = chainStageProteinInterfaceCount[chainIndex][stageIndex];

                    if (numberProteinInterfacesRequred == -1)
                    {
                        if ((bestIndex[chainIndex] == -1) || (proteinInterfaceCount > chainStageProteinInterfaceCount[chainIndex][bestIndex[chainIndex]]))
                        {
                            bestIndex[chainIndex] = stageIndex;
                        }
                    }

                    if ((proteinInterfaceCount == numberProteinInterfacesRequred) && ((numberProteinInterfacesRequred > -1) && ((bestIndex[chainIndex] == -1) || (chainStageProteinInterfaceCount[chainIndex][bestIndex[chainIndex]] != numberProteinInterfacesRequred))))
                    {
                        bestIndex[chainIndex] = stageIndex;
                    }
                }
            }
            return bestIndex;
        }
    }
}
