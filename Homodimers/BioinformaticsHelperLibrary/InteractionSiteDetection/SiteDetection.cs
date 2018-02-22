//-----------------------------------------------------------------------
// <copyright file="ProteinInterfaceDetection.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.Measurements;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class ProteinInterfaceDetection
    {


        /// <summary>
        ///     Removes groups from the cluster which do not meet the proteinInterface definition criteria.  Also provides a count of proteinInterfaces per
        ///     stage.
        /// </summary>
        /// <param name="proteinId"></param>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="fullClusteringResult"></param>
        /// <param name="chainStageProteinInterfaceCount"></param>
        /// <param name="clusteringProteinInterfaceDensityDetectionOptions"></param>
        /// <param name="minimumProteinInterfaceAlphas"></param>
        /// <param name="maximumProteinInterfaceAlphas"></param>
        /// <param name="minimumProteinInterfaceDensity"></param>
        /// <param name="maximumProteinInterfaceDensity"></param>
        /// <returns></returns>
        public static ClusteringFullResultListContainer DetectProteinInterfaces(
            string proteinId,
            ProteinChainListContainer chainInteractingAtomLists,
            ClusteringFullResultListContainer fullClusteringResult,
            out List<List<int>> chainStageProteinInterfaceCount,
            ClusteringProteinInterfaceDensityDetectionOptions clusteringProteinInterfaceDensityDetectionOptions,// = ClusteringProteinInterfaceDensityDetectionOptions.ResidueSequenceIndex,
            decimal minimumProteinInterfaceDensity,
            decimal maximumProteinInterfaceDensity = -1,
            int minimumProteinInterfaceAlphas = 4,
            int maximumProteinInterfaceAlphas = -1
            )
        {
            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(fullClusteringResult))
            {
                throw new ArgumentOutOfRangeException(nameof(fullClusteringResult));
            }


            //var resultChainsStagesGroupsMembers = new List<List<List<List<int>>>>();
            var resultChainsStagesGroupsMembers = new ClusteringFullResultListContainer();

            chainStageProteinInterfaceCount = new List<List<int>>();

            for (int chainIndex = 0; chainIndex < fullClusteringResult.ChainList.Count; chainIndex++)
            {
                //var thisChain = new List<List<List<int>>>();

                var thisChain = new ClusteringFullResultListContainer.Chain();
                resultChainsStagesGroupsMembers.ChainList.Add(thisChain);

                var chainProteinInterfacesCount = new List<int>();
                chainStageProteinInterfaceCount.Add(chainProteinInterfacesCount);

                // Check every stage to see which has the lowest and highest number of suspected interaction proteinInterfaces.
                for (int stageIndex = 0; stageIndex < fullClusteringResult.ChainList[chainIndex].StageList.Count; stageIndex++)
                {
                    //var thisStage = new List<List<int>>();
                    var thisStage = new ClusteringFullResultListContainer.Chain.Stage();
                    thisChain.StageList.Add(thisStage);

                    int proteinInterfacesAtThisStage = 0;

                    // For every group, check if they have enough members (carbon alpha atoms) to meet the requirements of being an interaction proteinInterface.
                    for (int groupIndex = 0; groupIndex < fullClusteringResult.ChainList[chainIndex].StageList[stageIndex].ClusterList.Count; groupIndex++)
                    {
                        //var thisGroup = new List<int>();
                        var thisGroup = new ClusteringFullResultListContainer.Chain.Stage.Cluster();
                        thisStage.ClusterList.Add(thisGroup);

                        int groupTotalCarbonAlphas = fullClusteringResult.ChainList[chainIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count;

                        if (((minimumProteinInterfaceAlphas > -1) && (groupTotalCarbonAlphas < minimumProteinInterfaceAlphas)) || ((maximumProteinInterfaceAlphas > -1) && groupTotalCarbonAlphas > maximumProteinInterfaceAlphas))
                        {
                            continue;
                        }

                        //var interactingGroup = clusters.ChainList[chainIndex].StageList[stageIndex].ClusterList[groupIndex].Select(member => chainInteractingAtomLists.ChainList[chainIndex].AtomList[member]).ToList();
                        var interactingGroup = new ProteinAtomListContainer
                        {
                            AtomList = fullClusteringResult.ChainList[chainIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Select(member => chainInteractingAtomLists.ChainList[chainIndex].AtomList[member]).ToList()
                        };

                        decimal totalProteinInterfaceLength;

                        switch (clusteringProteinInterfaceDensityDetectionOptions)
                        {
                            case ClusteringProteinInterfaceDensityDetectionOptions.ResidueSequenceIndex:
                                totalProteinInterfaceLength = Clustering.FindProteinInterfaceDistanceFromResidueSequenceIndex(interactingGroup);
                                break;

                            case ClusteringProteinInterfaceDensityDetectionOptions.ShortestRoutePath:
                                decimal bestPathDistance;
                                Clustering.BruteForceTravellingSalesmanProblemSolver(interactingGroup, out bestPathDistance);
                                totalProteinInterfaceLength = bestPathDistance;
                                break;

                            case ClusteringProteinInterfaceDensityDetectionOptions.LongestDistanceBetweenInteractions:
                                decimal longestDistance;
                                Clustering.FindLongestDistanceBetweenNodes(interactingGroup, out longestDistance);
                                totalProteinInterfaceLength = longestDistance;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        // The length is the maximum distance between any two values/points or the total distance between them.
                        decimal interactionProteinInterfaceDensity = Math.Round((groupTotalCarbonAlphas/totalProteinInterfaceLength)*100, 0);

                        if (((minimumProteinInterfaceDensity <= -1) || (interactionProteinInterfaceDensity >= minimumProteinInterfaceDensity)) && ((maximumProteinInterfaceDensity <= -1) || (interactionProteinInterfaceDensity <= maximumProteinInterfaceDensity)))
                        {
                            proteinInterfacesAtThisStage++;
                            //stageTotalProteinInterfaces[chainIndex].StageList[stageIndex]++;
                            for (int memberIndex = 0; memberIndex < fullClusteringResult.ChainList[chainIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count; memberIndex++)
                            {
                                int thisMember = fullClusteringResult.ChainList[chainIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex];
                                thisGroup.AtomIndexList.Add(thisMember);
                            }
                            if (proteinInterfacesAtThisStage >= 4)
                            {
                                ////////Console.WriteLine(proteinInterfacesAtThisStage);
                            }
                        }
                    }

                    chainProteinInterfacesCount.Add(proteinInterfacesAtThisStage);
                }
            }

            return resultChainsStagesGroupsMembers;
        }


        public static ClusteringResultProteinInterfaceListContainer SortProteinInterfacesByResidueSequenceIndexAverage(ClusteringResultProteinInterfaceListContainer interactionProteinInterfaces)
        {
            if (ParameterValidation.IsClusteringResultProteinInterfaceListContainerNullOrEmpty(interactionProteinInterfaces))
            {
                throw new ArgumentOutOfRangeException(nameof(interactionProteinInterfaces));
            }

            bool outOfOrder = true;
            while (outOfOrder)
            {
                outOfOrder = false;
                for (int chainIndex = 0; chainIndex < interactionProteinInterfaces.ChainList.Count; chainIndex++)
                {
                    for (int proteinInterfaceIndex = 1; proteinInterfaceIndex < interactionProteinInterfaces.ChainList[chainIndex].ProteinInterfaceList.Count; proteinInterfaceIndex++)
                    {
                        ClusteringResultProteinInterfaceListContainer.Chain.ProteinInterface proteinInterface = interactionProteinInterfaces.ChainList[chainIndex].ProteinInterfaceList[proteinInterfaceIndex];
                        ClusteringResultProteinInterfaceListContainer.Chain.ProteinInterface lastProteinInterface = interactionProteinInterfaces.ChainList[chainIndex].ProteinInterfaceList[proteinInterfaceIndex - 1];

                        decimal proteinInterfaceAverage = CalculateProteinInterfaceAverageResidueSequenceIndex(proteinInterface);
                        decimal lastProteinInterfaceAverage = CalculateProteinInterfaceAverageResidueSequenceIndex(lastProteinInterface);

                        if (proteinInterfaceAverage < lastProteinInterfaceAverage)
                        {
                            interactionProteinInterfaces.ChainList[chainIndex].ProteinInterfaceList[proteinInterfaceIndex] = lastProteinInterface;
                            interactionProteinInterfaces.ChainList[chainIndex].ProteinInterfaceList[proteinInterfaceIndex - 1] = proteinInterface;
                            outOfOrder = true;
                        }
                    }
                }
            }

            return interactionProteinInterfaces;
        }

        public static int[] FindNextProteinInterfaceGroupIndexes(
            ProteinChainListContainer chainInteractingAtomLists,
            ClusteringFullResultListContainer clusters,
            ClusteringFullResultListContainer proteinInterfaces,
            int[] stageIndexes,
            int[] lastIndexes)
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

            if (ParameterValidation.IsIntArrayNullOrEmpty(stageIndexes))
            {
                throw new ArgumentOutOfRangeException(nameof(stageIndexes));
            }

            if (ParameterValidation.IsIntArrayNullOrEmpty(lastIndexes))
            {
                throw new ArgumentOutOfRangeException(nameof(lastIndexes));
            }

            int totalChains = chainInteractingAtomLists.ChainList.Count;
            var nextProteinInterfaceIndexes = new int[totalChains];

            for (int chainIndex = 0; chainIndex < totalChains; chainIndex++)
            {
                nextProteinInterfaceIndexes[chainIndex] = -1;
                int lastIndex = lastIndexes[chainIndex];
                int stageIndex = stageIndexes[chainIndex];
                int totalGroups = proteinInterfaces.ChainList[chainIndex].StageList[stageIndex].ClusterList.Count;

                if (stageIndex > -1)
                {
                    for (int groupIndex = (lastIndex + 1); groupIndex < totalGroups; groupIndex++)
                    {
                        ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterface = proteinInterfaces.ChainList[chainIndex].StageList[stageIndex].ClusterList[groupIndex];
                        int totalAtoms = 0;

                        if (proteinInterface != null && proteinInterface.AtomIndexList != null && proteinInterface.AtomIndexList.Count > 0)
                        {
                            totalAtoms = proteinInterface.AtomIndexList.Count;
                        }

                        if (totalAtoms > 0)
                        {
                            nextProteinInterfaceIndexes[chainIndex] = groupIndex;
                            break;
                        }
                    }
                }
            }

            return nextProteinInterfaceIndexes;
        }



        public static int[] FindNumberProteinInterfacesPerChain(ClusteringFullResultListContainer proteinInterfaces, int[] detectedBestClusterStagesIndexes)
        {
            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(proteinInterfaces))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfaces));
            }

            if (ParameterValidation.IsIntArrayNullOrEmpty(detectedBestClusterStagesIndexes))
            {
                throw new ArgumentOutOfRangeException(nameof(detectedBestClusterStagesIndexes));
            }

            int totalChains = proteinInterfaces.ChainList.Count;
            var result = new int[totalChains];

            for (int chainIndex = 0; chainIndex < totalChains; chainIndex++)
            {
                int stageIndex = detectedBestClusterStagesIndexes[chainIndex];

                if (stageIndex > -1)
                {
                    int chainCalculatedProteinInterfaceCount = proteinInterfaces.ChainList[chainIndex].StageList[stageIndex].ClusterList.Count(proteinInterface => proteinInterface != null && proteinInterface.AtomIndexList != null && proteinInterface.AtomIndexList.Count > 0);

                    result[chainIndex] = chainCalculatedProteinInterfaceCount;
                }
                else
                {
                    result[chainIndex] = 0;
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFilename"></param>
        /// <param name="pdbIdChainIdList"></param>
        /// <param name="pdbFileChains"></param>
        /// <param name="singularAaToAaInteractions"></param>
        /// <param name="fullClusteringResult"></param>
        /// <returns></returns>
        public static ProteinInterfaceAnalysisResultData AnalyseProteinInterfaces(
            CancellationToken cancellationToken,
            decimal maxAtomInterationDistance,
            decimal minimumProteinInterfaceDensity,
            string pdbFilename,
            Dictionary<string, List<string>> pdbIdChainIdList,
            ProteinChainListContainer pdbFileChains,
            ProteinChainListContainer singularAaToAaInteractions,
            ClusteringFullResultListContainer fullClusteringResult)
        {
            if (ParameterValidation.IsLoadFilenameInvalid(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(singularAaToAaInteractions))
            {
                throw new ArgumentOutOfRangeException(nameof(singularAaToAaInteractions));
            }

            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(fullClusteringResult))
            {
                throw new ArgumentOutOfRangeException(nameof(fullClusteringResult));
            }

            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            List<List<int>> chainStageProteinInterfaceCount;

            // Find how many proteinInterfaces at each stage.
            ClusteringFullResultListContainer proteinInterfacesClusteringResult = DetectProteinInterfaces(proteinId, singularAaToAaInteractions, fullClusteringResult, out chainStageProteinInterfaceCount,ClusteringProteinInterfaceDensityDetectionOptions.ResidueSequenceIndex, minimumProteinInterfaceDensity);

            // Find the last stage having required number of proteinInterfaces.
            int[] detectedBestClusterStagesIndexes = ProteinInterfaceTreeOptimalStageDetection.FindFinalProteinInterfaceStageIndexes(singularAaToAaInteractions, fullClusteringResult, proteinInterfacesClusteringResult, chainStageProteinInterfaceCount);

            int totalChains = singularAaToAaInteractions.ChainList.Count;

            var interactionProteinInterfaceClusteringHierarchyDataList = new List<InteractionProteinInterfaceClusteringHierarchyData>();

            int[] numberProteinInterfacesPerChain = FindNumberProteinInterfacesPerChain(proteinInterfacesClusteringResult, detectedBestClusterStagesIndexes);

            for (int chainIndex = 0; chainIndex < totalChains; chainIndex++)
            {
                int stageIndex = detectedBestClusterStagesIndexes[chainIndex];

                string chainIdLetter = SpreadsheetFileHandler.AlphabetLetterRollOver(chainIndex);

                var interactionProteinInterfaceClusteringHierarchyData = new InteractionProteinInterfaceClusteringHierarchyData(proteinId, chainIdLetter, numberProteinInterfacesPerChain[chainIndex], stageIndex + 1, fullClusteringResult.ChainList[chainIndex].StageList.Count);

                interactionProteinInterfaceClusteringHierarchyDataList.Add(interactionProteinInterfaceClusteringHierarchyData);
            }

            InteractionBetweenProteinInterfacesListContainer interactionBetweenProteinInterfacesContainer = CrossProteinInterfaceInteractions.FindInteractionsBetweenAnyProteinInterfaces(cancellationToken, maxAtomInterationDistance, pdbFilename, pdbIdChainIdList, pdbFileChains, singularAaToAaInteractions, fullClusteringResult, proteinInterfacesClusteringResult, detectedBestClusterStagesIndexes);

            List<ProteinInterfaceSequenceAndPositionData> analyseProteinInterfacesSequenceAndPositionData = AnalyseProteinInterfacesSequenceAndPositionData(pdbFilename, pdbIdChainIdList, pdbFileChains, singularAaToAaInteractions, proteinInterfacesClusteringResult, detectedBestClusterStagesIndexes, interactionBetweenProteinInterfacesContainer);

            var result = new ProteinInterfaceAnalysisResultData(
                detectedBestClusterStagesIndexes,
                proteinInterfacesClusteringResult,
                interactionProteinInterfaceClusteringHierarchyDataList,
                interactionBetweenProteinInterfacesContainer,
                analyseProteinInterfacesSequenceAndPositionData
                );

            return result;
        }

        public static decimal CalculateProteinInterfaceAverageResidueSequenceIndex(ClusteringResultProteinInterfaceListContainer.Chain.ProteinInterface proteinInterface)
        {
            if (ParameterValidation.IsProteinInterfaceNullOrEmpty(proteinInterface))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterface));
            }

            decimal sumTotal = 0.0m;
            int sumAtoms = 0;

            foreach (ATOM_Record atom in proteinInterface.AtomList)
            {
                int residueSequenceIndex;
                if (int.TryParse(atom.resSeq.FieldValue, out residueSequenceIndex))
                {
                    sumTotal += residueSequenceIndex;
                    sumAtoms++;
                }
            }

            decimal average = -1m;

            if (sumAtoms > 0)
            {
                average = sumTotal/sumAtoms;
            }

            return average;
        }

        /// <summary>
        ///     Get the lowest (minimum) and highest (maximum) residue sequence index (as found in the pdb file) in an interaction
        ///     proteinInterface.
        /// </summary>
        /// <param name="proteinInterface"></param>
        /// <param name="singularAaToAaInteractions"></param>
        /// <param name="chainIndex"></param>
        /// <returns></returns>
        public static MinMax MinMaxResidueSequenceIndex(ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterface, ProteinChainListContainer singularAaToAaInteractions, int chainIndex)
        {
            if (ParameterValidation.IsClusterNullOrEmpty(proteinInterface))
            {
                throw new ArgumentNullException(nameof(proteinInterface));
            }

            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(singularAaToAaInteractions))
            {
                throw new ArgumentNullException(nameof(singularAaToAaInteractions));
            }

            if (ParameterValidation.IsChainIndexInvalid(chainIndex))
            {
                throw new ArgumentOutOfRangeException(nameof(chainIndex));
            }

            int proteinInterfaceMin = 0;
            int proteinInterfaceMax = 0;

            for (int memberIndex = 0; memberIndex < proteinInterface.AtomIndexList.Count; memberIndex++)
            {
                int member = proteinInterface.AtomIndexList[memberIndex];

                ATOM_Record atom = singularAaToAaInteractions.ChainList[chainIndex].AtomList[member];

                var residueSequenceIndex = ProteinDataBankFileOperations.NullableTryParseInt32(atom.resSeq.FieldValue);

                if (residueSequenceIndex == null) continue;

                if (memberIndex == 0 || residueSequenceIndex < proteinInterfaceMin)
                {
                    proteinInterfaceMin = residueSequenceIndex.Value;
                }

                if (memberIndex == 0 || residueSequenceIndex > proteinInterfaceMax)
                {
                    proteinInterfaceMax = residueSequenceIndex.Value;
                }
            }

            return new MinMax(proteinInterfaceMin, proteinInterfaceMax);
        }


        public static int CalculateProteinInterfaceLength(int minResidueSequenceIndex, int maxResidueSequenceIndex)
        {
            return (maxResidueSequenceIndex - minResidueSequenceIndex) + 1;
        }

        public static int FindProteinInterfaceLength(
            ProteinChainListContainer singularAaToAaInteractions,
            ClusteringFullResultListContainer proteinInterfacesClusteringResult,
            int[] detectedBestStages,
            int chainIndex,
            int proteinInterfaceIndex)
        {
            List<ClusteringFullResultListContainer.Chain.Stage.Cluster> proteinInterfaceList = proteinInterfacesClusteringResult.ChainList[chainIndex].StageList[detectedBestStages[chainIndex]].ClusterList;

            List<ClusteringFullResultListContainer.Chain.Stage.Cluster> nonEmptyProteinInterfaceList = proteinInterfaceList.Where(a => a != null && a.AtomIndexList != null && a.AtomIndexList.Count > 0).ToList();

            ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterface = nonEmptyProteinInterfaceList[proteinInterfaceIndex];

            int proteinInterfaceLength = 0;

            //if (proteinInterface.AtomIndexList.Count == 0)
            //{
            //    return 0;
            //}

            MinMax minMaxResidueSequenceIndex = MinMaxResidueSequenceIndex(proteinInterface, singularAaToAaInteractions, chainIndex);

            proteinInterfaceLength = CalculateProteinInterfaceLength(minMaxResidueSequenceIndex.Min, minMaxResidueSequenceIndex.Max);

            return proteinInterfaceLength;
        }

        public static int AtomIndexPositionInProteinInterface(
            ProteinChainListContainer singularAaToAaInteractions,
            ClusteringFullResultListContainer proteinInterfacesClusteringResult,
            int[] detectedBestStages,
            int chainIndex,
            int proteinInterfaceIndex,
            ATOM_Record atomPositionToFind)
        {
            List<ClusteringFullResultListContainer.Chain.Stage.Cluster> proteinInterfaceList = proteinInterfacesClusteringResult.ChainList[chainIndex].StageList[detectedBestStages[chainIndex]].ClusterList;

            List<ClusteringFullResultListContainer.Chain.Stage.Cluster> nonEmptyProteinInterfaceList = proteinInterfaceList.Where(a => a != null && a.AtomIndexList != null && a.AtomIndexList.Count > 0).ToList();

            if (proteinInterfaceIndex > nonEmptyProteinInterfaceList.Count - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfaceIndex), proteinInterfaceIndex, "proteinInterfaceIndex was greater than the number of proteinInterfaces found.");
            }

            ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterface = nonEmptyProteinInterfaceList[proteinInterfaceIndex];

            if (proteinInterface.AtomIndexList.Count == 0)
            {
                return -1;
            }

            MinMax minMaxResidueSequenceIndex = MinMaxResidueSequenceIndex(proteinInterface, singularAaToAaInteractions, chainIndex);

            int proteinInterfaceLength = CalculateProteinInterfaceLength(minMaxResidueSequenceIndex.Min, minMaxResidueSequenceIndex.Max);

            var residueSequenceToFind = ProteinDataBankFileOperations.NullableTryParseInt32(atomPositionToFind.resSeq.FieldValue);

            if (residueSequenceToFind == null) return -1;

            int index = residueSequenceToFind.Value - minMaxResidueSequenceIndex.Min; // zero based

            return index;
        }

        /// <summary>
        ///     This method returns an array showing whether an amino acid has an interaction with any amino acids in a proteinInterface of
        ///     another chain
        /// </summary>
        /// <returns></returns>
        public static bool[] AminoAcidInteractionVector(
            ProteinChainListContainer singularAaToAaInteractions,
            ClusteringFullResultListContainer proteinInterfacesClusteringResult,
            int[] detectedBestStages,
            InteractionBetweenProteinInterfacesListContainer interactionsBetweenProteinInterfacesContainer,
            //int proteinInterfaceLength,
            int sourceChainIndex,
            int sourceProteinInterfaceIndex,
            int sourceResidueIndex
            )
        {
            // find the largest proteinInterface to make vector the same size
            int maxProteinInterfaceLength = 0;

            for (int chainIndex = 0; chainIndex < proteinInterfacesClusteringResult.ChainList.Count; chainIndex++)
            {
                List<ClusteringFullResultListContainer.Chain.Stage.Cluster> proteinInterfaceList = proteinInterfacesClusteringResult.ChainList[chainIndex].StageList[detectedBestStages[chainIndex]].ClusterList;
                List<ClusteringFullResultListContainer.Chain.Stage.Cluster> nonEmptyProteinInterfaceList = proteinInterfaceList.Where(a => a != null && a.AtomIndexList != null && a.AtomIndexList.Count > 0).ToList();

                for (int proteinInterfaceIndex = 0; proteinInterfaceIndex < nonEmptyProteinInterfaceList.Count; proteinInterfaceIndex++)
                {
                    ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterface = nonEmptyProteinInterfaceList[proteinInterfaceIndex];

                    if (proteinInterface.AtomIndexList == null || proteinInterface.AtomIndexList.Count == 0)
                    {
                        continue;
                    }

                    int length = FindProteinInterfaceLength(singularAaToAaInteractions, proteinInterfacesClusteringResult, detectedBestStages, chainIndex, proteinInterfaceIndex);

                    if (length > maxProteinInterfaceLength)
                    {
                        maxProteinInterfaceLength = length;
                    }
                }
            }


            // find interactions matching the current chain id and proteinInterface id and res id... res id is different from resSeq in the pdb

            var result = new bool[maxProteinInterfaceLength];

            List<InteractionBetweenProteinInterfaces> matchingInteractions = interactionsBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Where(a => (a.Atom1.FullProteinInterfaceId.ChainId == sourceChainIndex && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == sourceProteinInterfaceIndex) || (a.Atom2.FullProteinInterfaceId.ChainId == sourceChainIndex && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == sourceProteinInterfaceIndex)).ToList();


            if (matchingInteractions.Count == 0)
            {
                return result;
            }

            foreach (InteractionBetweenProteinInterfaces interaction in matchingInteractions)
            {
                if (interaction.Atom1.FullProteinInterfaceId.ChainId == sourceChainIndex && interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId == sourceProteinInterfaceIndex && ProteinDataBankFileOperations.NullableTryParseInt32(interaction.Atom1.Atom.resSeq.FieldValue) == sourceResidueIndex)
                {
                    // where in the proteinInterface oppoproteinInterface proteinInterface is Atom2?
                    int index = AtomIndexPositionInProteinInterface(singularAaToAaInteractions, proteinInterfacesClusteringResult, detectedBestStages, interaction.Atom2.FullProteinInterfaceId.ChainId, interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId, interaction.Atom2.Atom);
                    result[index] = true;
                }

                else if (interaction.Atom2.FullProteinInterfaceId.ChainId == sourceChainIndex && interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId == sourceProteinInterfaceIndex && ProteinDataBankFileOperations.NullableTryParseInt32(interaction.Atom2.Atom.resSeq.FieldValue) == sourceResidueIndex)
                {
                    int index = AtomIndexPositionInProteinInterface(singularAaToAaInteractions, proteinInterfacesClusteringResult, detectedBestStages, interaction.Atom1.FullProteinInterfaceId.ChainId, interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId, interaction.Atom1.Atom);
                    result[index] = true;
                }
            }

            return result;
        }

        /// <summary>
        ///     This method returns the total number of interactions an atom has with non-proteinInterfaces in oppoproteinInterface chains.  The method
        ///     requires the object returned by the FindInteractionsBetweenProteinInterfaces method.
        /// </summary>
        /// <param name="interactionBetweenProteinInterfacesContainer"></param>
        /// <param name="sourceChainIndex"></param>
        /// <param name="atomToFind"></param>
        /// <returns></returns>
        public static int CountAtomInteractionsOutsideProteinInterface(
            InteractionBetweenProteinInterfacesListContainer interactionBetweenProteinInterfacesContainer,
            int sourceChainIndex,
            ATOM_Record atomToFind
            )
        {
            int totalInteractionsOutsideProteinInterfaces = interactionBetweenProteinInterfacesContainer.InteractionBetweenNonProteinInterfacesList.Count(
                a => (a.Atom1.FullProteinInterfaceId.ChainId == sourceChainIndex && a.Atom1.Atom == atomToFind) ||
                     (a.Atom2.FullProteinInterfaceId.ChainId == sourceChainIndex && a.Atom2.Atom == atomToFind));

            return totalInteractionsOutsideProteinInterfaces;
        }

        /// <summary>
        ///     Load proteinInterface data from the PDB file based on a list of already detected proteinInterfaces.
        ///     The detected proteinInterfaces may be missing data such as other atoms or residues which are also in the proteinInterface but were not
        ///     directly interacting.
        ///     The positions and lengths of the proteinInterfaces are also calculated.
        /// </summary>
        /// <param name="pdbFilename"></param>
        /// <param name="pdbFileChains"></param>
        /// <param name="singularAaToAaInteractions"></param>
        /// <param name="proteinInterfacesClusteringResult"></param>
        /// <param name="detectedBestStages"></param>
        /// <param name="interactionBetweenProteinInterfacesContainer"></param>
        /// <returns></returns>
        public static List<ProteinInterfaceSequenceAndPositionData> AnalyseProteinInterfacesSequenceAndPositionData(
            string pdbFilename,
            Dictionary<string, List<string>> pdbIdChainIdList,
            ProteinChainListContainer pdbFileChains,
            ProteinChainListContainer singularAaToAaInteractions,
            ClusteringFullResultListContainer proteinInterfacesClusteringResult,
            int[] detectedBestStages,
            InteractionBetweenProteinInterfacesListContainer interactionBetweenProteinInterfacesContainer)
        {
            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            if (!File.Exists(pdbFilename))
            {
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(singularAaToAaInteractions))
            {
                throw new ArgumentOutOfRangeException(nameof(singularAaToAaInteractions));
            }

            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(proteinInterfacesClusteringResult))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfacesClusteringResult));
            }

            if (ParameterValidation.IsIntArrayNullOrEmpty(detectedBestStages))
            {
                throw new ArgumentOutOfRangeException(nameof(detectedBestStages));
            }

            // ProteinInterfaces are clusters with non-proteinInterfaces removed.

            var result = new List<ProteinInterfaceSequenceAndPositionData>();
            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);
            int totalChains = proteinInterfacesClusteringResult.ChainList.Count;

            for (int chainIndex = 0; chainIndex < totalChains; chainIndex++)
            {
                int stageIndex = detectedBestStages[chainIndex];
                string chainIdLetter = SpreadsheetFileHandler.AlphabetLetterRollOver(chainIndex);

                List<ClusteringFullResultListContainer.Chain.Stage.Cluster> proteinInterfaceList = proteinInterfacesClusteringResult.ChainList[chainIndex].StageList[stageIndex].ClusterList;

                List<ClusteringFullResultListContainer.Chain.Stage.Cluster> nonEmptyProteinInterfaceList = proteinInterfaceList.Where(a => a != null && a.AtomIndexList != null && a.AtomIndexList.Count > 0).ToList();

                // loop through each proteinInterface
                for (int proteinInterfaceIndex = 0; proteinInterfaceIndex < nonEmptyProteinInterfaceList.Count; proteinInterfaceIndex++)
                {
                    ClusteringFullResultListContainer.Chain.Stage.Cluster proteinInterface = nonEmptyProteinInterfaceList[proteinInterfaceIndex];

                    // Find min and max residue sequence index value in the proteinInterface
                    
                    MinMax proteinInterfaceResidueSequenceIndexes = MinMaxResidueSequenceIndex(proteinInterface, singularAaToAaInteractions, chainIndex);
                    int proteinInterfaceLength = CalculateProteinInterfaceLength(proteinInterfaceResidueSequenceIndexes.Min, proteinInterfaceResidueSequenceIndexes.Max);

                    string proteinInterfaceIdLetter = SpreadsheetFileHandler.AlphabetLetterRollOver(proteinInterfaceIndex);

                    var proteinInterfacePositionData = new ProteinInterfaceSequenceAndPositionData
                    {
                        FullProteinInterfaceId = new FullProteinInterfaceId(proteinId, chainIndex, proteinInterfaceIndex, proteinInterfaceResidueSequenceIndexes.Min, proteinInterfaceResidueSequenceIndexes.Max),
                        ChainIdLetter = chainIdLetter,
                        
                        ProteinInterfaceIdLetter = proteinInterfaceIdLetter,
                        
                        StartPosition = proteinInterfaceResidueSequenceIndexes.Min,
                        EndPosition = proteinInterfaceResidueSequenceIndexes.Max,
                        ProteinInterfaceLength = CalculateProteinInterfaceLength(proteinInterfaceResidueSequenceIndexes.Min, proteinInterfaceResidueSequenceIndexes.Max)
                    };
                    proteinInterfacePositionData.AminoAcidSequenceAllResidueSequenceIndexes = new ProteinInterfaceAminoAcidMetaData[proteinInterfacePositionData.ProteinInterfaceLength];
                    
                    proteinInterfacePositionData.AminoAcidSequenceAll1L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsAll1L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly1L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsNone1L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly1L = "";

                    proteinInterfacePositionData.AminoAcidSequenceAll3L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsAll3L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly3L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsNone3L = "";
                    proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly3L = "";

                    //int foundAtomCount = 0;

                    const string placeholder1L = "_";
                    const string placeholder3L = "___";

                    for (int residueSequenceIndex = proteinInterfaceResidueSequenceIndexes.Min; residueSequenceIndex <= proteinInterfaceResidueSequenceIndexes.Max; residueSequenceIndex++)
                    {
                        /* questions
                         * 1. does this reside interact with another reside which is also part of a proteinInterface?
                         * 2. if not, does this reside interact at all?
                         */

                        var proteinInterfaceAminoAcidMetaData = new ProteinInterfaceAminoAcidMetaData();
                        proteinInterfacePositionData.AminoAcidSequenceAllResidueSequenceIndexes[proteinInterfacePositionData.AminoAcidSequenceAll1L.Length] = proteinInterfaceAminoAcidMetaData;

                        ATOM_Record foundAtomInsidePdbFile = AtomSearchMethods.FindAtomInsidePdbFileChain(pdbFileChains, chainIndex, residueSequenceIndex);

                        if (foundAtomInsidePdbFile == null)
                        {
                            // Non-CA atom is loaded here in case of missing CA atom to find the AA code for the resSeq index
                            var chainIdList = pdbIdChainIdList != null ? (pdbIdChainIdList.ContainsKey(proteinId) ? pdbIdChainIdList[proteinId].ToArray() : null) : null;

                            ProteinChainListContainer pdbFileChains2 = ProteinDataBankFileOperations.PdbAtomicChains(pdbFilename, chainIdList, -1, -1, false);
                            foundAtomInsidePdbFile = AtomSearchMethods.FindAtomInsidePdbFileChain(pdbFileChains2, chainIndex, residueSequenceIndex);
                        }

                        proteinInterfaceAminoAcidMetaData.PdbResidueSequenceIndex = residueSequenceIndex;
                        proteinInterfaceAminoAcidMetaData.ArrayMemberIndex = pdbFileChains.ChainList[chainIndex].AtomList.IndexOf(foundAtomInsidePdbFile);
                        proteinInterfaceAminoAcidMetaData.OppoproteinInterfaceInteractions = new bool[proteinInterfaceLength];


                        if (foundAtomInsidePdbFile != null)
                        {
                            proteinInterfacePositionData.AminoAcidSequenceAll1L += AminoAcidConversions.AminoAcidNameToCode1L(foundAtomInsidePdbFile.resName.FieldValue);

                            proteinInterfacePositionData.AminoAcidSequenceAll3L += foundAtomInsidePdbFile.resName.FieldValue.PadRight(3, '_');
                        }
                        else
                        {
                            proteinInterfacePositionData.AminoAcidSequenceAll1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone1L += placeholder1L;

                            proteinInterfacePositionData.AminoAcidSequenceAll3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone3L += placeholder3L;

                            proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionType = ProteinInterfaceInteractionType.NoInteractionFound;
                            proteinInterfaceAminoAcidMetaData.NonProteinInterfaceInteractionResidueNames1L += placeholder1L;
                            proteinInterfaceAminoAcidMetaData.NonProteinInterfaceInteractionResidueNames3L += placeholder3L;
                            continue;
                        }

                        List<ATOM_Record> foundAtomInteractingWithAnotherProteinInterface = AtomSearchMethods.FindAtomInteractingWithOtherProteinInterfaces(foundAtomInsidePdbFile, interactionBetweenProteinInterfacesContainer, FindAtomInteractingWithAnotherProteinInterfaceOptions.FindAtomsInteractingWithOtherProteinInterfaces);
                        List<ATOM_Record> foundAtomInteractingWithNonProteinInterface = AtomSearchMethods.FindAtomInteractingWithOtherProteinInterfaces(foundAtomInsidePdbFile, interactionBetweenProteinInterfacesContainer, FindAtomInteractingWithAnotherProteinInterfaceOptions.FindAtomsInteractingWithNonProteinInterfaces);

                        proteinInterfaceAminoAcidMetaData.OppoproteinInterfaceInteractions = AminoAcidInteractionVector(singularAaToAaInteractions, proteinInterfacesClusteringResult, detectedBestStages, interactionBetweenProteinInterfacesContainer, chainIndex, proteinInterfaceIndex, residueSequenceIndex);
                        
                        proteinInterfaceAminoAcidMetaData.ResidueName1L = AminoAcidConversions.AminoAcidNameToCode1L(foundAtomInsidePdbFile.resName.FieldValue);
                        proteinInterfaceAminoAcidMetaData.ResidueName3L = foundAtomInsidePdbFile.resName.FieldValue.PadRight(3, '_');

                        if (foundAtomInteractingWithAnotherProteinInterface != null)
                        {
                            foreach (ATOM_Record atom in foundAtomInteractingWithAnotherProteinInterface)
                            {
                                proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionResidueNames1L += AminoAcidConversions.AminoAcidNameToCode1L(atom.resName.FieldValue);
                                proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionResidueNames3L += atom.resName.FieldValue.PadRight(3, '_');
                            }
                        }

                        if (foundAtomInteractingWithNonProteinInterface != null)
                        {
                            foreach (ATOM_Record atom in foundAtomInteractingWithNonProteinInterface)
                            {
                                proteinInterfaceAminoAcidMetaData.NonProteinInterfaceInteractionResidueNames1L += AminoAcidConversions.AminoAcidNameToCode1L(atom.resName.FieldValue);
                                proteinInterfaceAminoAcidMetaData.NonProteinInterfaceInteractionResidueNames3L += atom.resName.FieldValue.PadRight(3, '_');
                            }
                        }

                        if (foundAtomInteractingWithAnotherProteinInterface != null && foundAtomInteractingWithAnotherProteinInterface.Count > 0)
                        {
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll1L += AminoAcidConversions.AminoAcidNameToCode1L(foundAtomInsidePdbFile.resName.FieldValue);
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly1L += AminoAcidConversions.AminoAcidNameToCode1L(foundAtomInsidePdbFile.resName.FieldValue);
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone1L += placeholder1L;

                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll3L += foundAtomInsidePdbFile.resName.FieldValue.PadRight(3, '_');
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly3L += foundAtomInsidePdbFile.resName.FieldValue.PadRight(3, '_');
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone3L += placeholder3L;

                            proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionType = ProteinInterfaceInteractionType.InteractionWithAnotherProteinInterface;

                            if (foundAtomInteractingWithNonProteinInterface != null && foundAtomInteractingWithNonProteinInterface.Count > 0)
                            {
                                proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionType |= ProteinInterfaceInteractionType.InteractionWithNonProteinInterface;
                            }
                        }
                        else if (foundAtomInteractingWithNonProteinInterface != null && foundAtomInteractingWithNonProteinInterface.Count > 0)
                        {
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll1L += AminoAcidConversions.AminoAcidNameToCode1L(foundAtomInsidePdbFile.resName.FieldValue);
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly1L += AminoAcidConversions.AminoAcidNameToCode1L(foundAtomInsidePdbFile.resName.FieldValue);
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone1L += placeholder1L;

                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll3L += foundAtomInsidePdbFile.resName.FieldValue.PadRight(3, '_');
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly3L += foundAtomInsidePdbFile.resName.FieldValue.PadRight(3, '_');
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone3L += placeholder3L;

                            proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionType = ProteinInterfaceInteractionType.InteractionWithNonProteinInterface;
                        }
                        else
                        {
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly1L += placeholder1L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone1L += AminoAcidConversions.AminoAcidNameToCode1L(foundAtomInsidePdbFile.resName.FieldValue);

                            proteinInterfacePositionData.AminoAcidSequenceInteractionsAll3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly3L += placeholder3L;
                            proteinInterfacePositionData.AminoAcidSequenceInteractionsNone3L += foundAtomInsidePdbFile.resName.FieldValue.PadRight(3, '_');

                            proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionType = ProteinInterfaceInteractionType.NoInteractionFound;
                        }
                    }

                    result.Add(proteinInterfacePositionData);
                }
            }

            return result;
        }
    }
}