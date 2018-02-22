//-----------------------------------------------------------------------
// <copyright file="Clustering.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bio;
using BioinformaticsHelperLibrary.Dimers;
using BioinformaticsHelperLibrary.InteractionDetection;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.Measurements;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.UserProteinInterface;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class Clustering
    {
        /// <summary>
        ///     This method performs single-link clustering on a single specified PDB file ATOM records.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFilename"></param>
        /// <param name="pdbIdChainIdList"></param>
        /// <param name="clusteringMethodOptions"></param>
        /// <param name="maximumGroupSize"></param>
        /// <param name="maximumDistance"></param>
        /// <param name="progressActionSet"></param>
        public static ClusterProteinDataBankFileResult ClusterProteinDataBankFile(CancellationToken cancellationToken,
            decimal maxAtomInterationDistance,
            decimal minimumProteinInterfaceDensity,
            string pdbFilename,
            Dictionary<string, List<string>> pdbIdChainIdList,
            ClusteringMethodOptions clusteringMethodOptions,
            int maximumGroupSize = -1,
            int maximumDistance = -1,
            ProgressActionSet progressActionSet = null)
        {
            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename), pdbFilename, "pdbFilename was " + ParameterValidation.NullEmptyOrWhiteSpaceToString(pdbFilename));
            }

            if (!File.Exists(pdbFilename))
            {
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            var proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            var chainIdList = pdbIdChainIdList != null ? (proteinId != null && pdbIdChainIdList.ContainsKey(proteinId) ? pdbIdChainIdList[proteinId].ToArray() : null) : null;

            var clusterProteinDataBankFileResult = new ClusterProteinDataBankFileResult
            {
                PdbFilename = pdbFilename,
                ProteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename),
                ClusteringMethodOptions = clusteringMethodOptions,
                MaximumGroupSize = maximumGroupSize,
                MaximumDistance = maximumDistance,
                PdbFileChains = ProteinDataBankFileOperations.PdbAtomicChains(pdbFilename, chainIdList, 1, 26+26, true),
            };

            clusterProteinDataBankFileResult.AaToAaInterationsList = SearchInteractions.FindInteractions(cancellationToken, maxAtomInterationDistance, clusterProteinDataBankFileResult.ProteinId, pdbIdChainIdList, clusterProteinDataBankFileResult.PdbFileChains); //, false, -1, true, 2);

            if (clusterProteinDataBankFileResult.AaToAaInterationsList == null || clusterProteinDataBankFileResult.AaToAaInterationsList.Count == 0)
            {
                return null;
            }

            // a list of atoms found to be interacting with the oppoproteinInterface chain
            ProteinChainListContainer singularAaToAaInteractions = ProteinDataBankFileOperations.AtomPairListToUnpairedAtomLists(clusterProteinDataBankFileResult.AaToAaInterationsList, true);


            switch (clusteringMethodOptions)
            {
                case ClusteringMethodOptions.ClusterWithPosition3D:
                    clusterProteinDataBankFileResult.ClusteringFullResultListContainer = ClusterByDistance3D(singularAaToAaInteractions, maximumGroupSize, maximumDistance, progressActionSet);
                    break;
                case ClusteringMethodOptions.ClusterWithResidueSequenceIndex:
                    clusterProteinDataBankFileResult.ClusteringFullResultListContainer = ClusterByDistanceResidueSequenceIndex(singularAaToAaInteractions, maximumGroupSize, maximumDistance, progressActionSet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clusteringMethodOptions), clusteringMethodOptions, "Unknown or unrecognised enumeration option");
            }


            clusterProteinDataBankFileResult.ProteinInterfaceAnalysisResultData = ProteinInterfaceDetection.AnalyseProteinInterfaces(cancellationToken, maxAtomInterationDistance, minimumProteinInterfaceDensity, pdbFilename, pdbIdChainIdList, clusterProteinDataBankFileResult.PdbFileChains, singularAaToAaInteractions, clusterProteinDataBankFileResult.ClusteringFullResultListContainer);

            clusterProteinDataBankFileResult.PredictedFinalChainClusteringStageList = new List<ClusteringFullResultListContainer.Chain.Stage>();

            for (int chainIndex = 0; chainIndex < clusterProteinDataBankFileResult.ProteinInterfaceAnalysisResultData.DetectedBestStageIndexes.Length; chainIndex++)
            {
                int stageIndex = clusterProteinDataBankFileResult.ProteinInterfaceAnalysisResultData.DetectedBestStageIndexes[chainIndex];

                // For 'cluster result'
                // var clusteringStage = clusterProteinDataBankFileResult.ClusteringFullResultListContainer.ChainList[chainIndex].StageList[stageIndex];

                // For 'proteinInterface result'
                ClusteringFullResultListContainer.Chain.Stage clusteringStage = clusterProteinDataBankFileResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesClusteringResult.ChainList[chainIndex].StageList[stageIndex];

                clusterProteinDataBankFileResult.PredictedFinalChainClusteringStageList.Add(clusteringStage);
            }

            return clusterProteinDataBankFileResult;
        }



        public static void SaveProteinInterfaceInteractionsSpreadsheet(string saveFilename, Dictionary<string, InteractionBetweenProteinInterfacesListContainer> interactionsBetweenProteinInterfacesDictionary, Dictionary<string, int[]> proteinInterfaceCountDictionary, int numberProteinInterfacesRequired = -1) //)
        {
            if (ParameterValidation.IsSaveAsFilenameInvalid(saveFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(saveFilename));
            }

            if (ParameterValidation.IsDictionaryNullOrEmpty(interactionsBetweenProteinInterfacesDictionary))
            {
                throw new ArgumentOutOfRangeException(nameof(interactionsBetweenProteinInterfacesDictionary));
            }

            var spreadsheetRowList = new List<List<SpreadsheetCell>>();
            

            var spreadsheetColumnHeadersRow = ProteinInteractionRecord.TsvColumnHeadersRow().ToList();

            var pdbIdExcludeList = new List<string>();

            if (numberProteinInterfacesRequired != -1)
            {
                foreach (var interactionsBetweenProteinInterfacesKvp in interactionsBetweenProteinInterfacesDictionary)
                {
                    string proteinId = interactionsBetweenProteinInterfacesKvp.Key;
                    InteractionBetweenProteinInterfacesListContainer interactionsBetweenProteinInterfaces = interactionsBetweenProteinInterfacesKvp.Value;

                    if (interactionsBetweenProteinInterfaces == null)
                    {
                        continue;
                    }

                    bool excludePdb = false;

                    if (proteinInterfaceCountDictionary.ContainsKey(proteinId))
                    {
                        int[] proteinInterfaceCount = proteinInterfaceCountDictionary[proteinId];

                        for (int proteinInterfaceCountIndex = 0; proteinInterfaceCountIndex < proteinInterfaceCount.Length; proteinInterfaceCountIndex++)
                        {
                            int chainTotalProteinInterfaces = proteinInterfaceCount[proteinInterfaceCountIndex];

                            if (chainTotalProteinInterfaces != numberProteinInterfacesRequired)
                            {
                                excludePdb = true;
                            }
                        }
                    }
                    else
                    {
                        excludePdb = true;
                    }

                    if (excludePdb)
                    {
                        pdbIdExcludeList.Add(proteinId);
                    }
                }
            }

            foreach (var interactionsBetweenProteinInterfacesKvp in interactionsBetweenProteinInterfacesDictionary)
            {
                string proteinId = interactionsBetweenProteinInterfacesKvp.Key;
                List<InteractionBetweenProteinInterfaces> interactionsBetweenProteinInterfacesList = interactionsBetweenProteinInterfacesKvp.Value.InteractionBetweenProteinInterfacesList;

                if (interactionsBetweenProteinInterfacesList == null)
                {
                    continue;
                }

                if (pdbIdExcludeList.Contains(proteinId))
                {
                    continue;
                }


                foreach (InteractionBetweenProteinInterfaces interactionsBetween in interactionsBetweenProteinInterfacesList)
                {
                    var atomPair = new AtomPair(
                        interactionsBetween.Atom1.Atom,
                        interactionsBetween.Atom2.Atom,
                        Point3D.Distance3D(PointConversions.AtomPoint3D(interactionsBetween.Atom1.Atom), PointConversions.AtomPoint3D(interactionsBetween.Atom2.Atom))
                        );

                    var interactionRecord = new ProteinInteractionRecord(proteinId, 0, atomPair)
                    {
                        ProteinInterfaceIdA = SpreadsheetFileHandler.AlphabetLetterRollOver(interactionsBetween.Atom1.FullProteinInterfaceId.ProteinInterfaceId),
                        ProteinInterfaceIdB = SpreadsheetFileHandler.AlphabetLetterRollOver(interactionsBetween.Atom2.FullProteinInterfaceId.ProteinInterfaceId)
                    };

                    spreadsheetRowList.Add(interactionRecord.SpreadsheetDataRow().ToList());
                }
            }

            spreadsheetRowList = spreadsheetRowList.OrderBy(a => string.Join(" ", a)).ToList();
            spreadsheetRowList.Insert(0, spreadsheetColumnHeadersRow);

            SpreadsheetFileHandler.SaveSpreadsheet(saveFilename, null, spreadsheetRowList);
        }

        /// <summary>
        ///     This method converts a list of atoms into a list of 3d coordinates/positions.
        /// </summary>
        /// <param name="proteinAtomListContainer"></param>
        /// <returns></returns>
        public static List<Point3D> AtomRecordListToPoint3DList(ProteinAtomListContainer proteinAtomListContainer)
        {
            if (ParameterValidation.IsProteinAtomListContainerNullOrEmpty(proteinAtomListContainer))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinAtomListContainer));
            }

            var result = new List<Point3D>();

            result.AddRange(proteinAtomListContainer.AtomList.Select(t => new Point3D(t.x.FieldValue, t.y.FieldValue, t.z.FieldValue)).ToList());

            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="chainInteractingAtomList"></param>
        /// <param name="proteinInterfaceGroupMembers"></param>
        /// <returns></returns>
        public static int FindProteinInterfaceDistanceFromResidueSequenceIndex(ProteinAtomListContainer chainInteractingAtomList)
        {
            if (ParameterValidation.IsProteinAtomListContainerNullOrEmpty(chainInteractingAtomList))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomList));
            }

            int minimumResidueSequenceIndex = 0;
            int maximumResidueSequenceIndex = 0;

            for (int atomIndex = 0; atomIndex < chainInteractingAtomList.AtomList.Count; atomIndex++)
            {
                ATOM_Record atom = chainInteractingAtomList.AtomList[atomIndex];

                int residueSequenceIndex;

                if (int.TryParse(atom.resSeq.FieldValue, out residueSequenceIndex))
                {
                    if ((atomIndex == 0) || (residueSequenceIndex < minimumResidueSequenceIndex))
                    {
                        minimumResidueSequenceIndex = residueSequenceIndex;
                    }

                    if ((atomIndex == 0) || (residueSequenceIndex > maximumResidueSequenceIndex))
                    {
                        maximumResidueSequenceIndex = residueSequenceIndex;
                    }
                }
            }

            return Math.Abs(maximumResidueSequenceIndex - minimumResidueSequenceIndex) + 1;
        }

        /// <summary>
        ///     This method clusters all lists of list of atoms by the distance specified.  This method is not concerned with the
        ///     attribute used to calculate the distance matrix as this should be done by a separate method.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="distances"></param>
        /// <param name="maximumGroupSize"></param>
        /// <param name="maximumDistance"></param>
        /// <returns></returns>
        public static ClusteringFullResultListContainer ClusterByDistance(ProteinChainListContainer chainInteractingAtomLists, decimal[][,] distances, int maximumGroupSize = -1, int maximumDistance = -1, ProgressActionSet progressActionSet = null)
        {
            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            if (ParameterValidation.IsDecimalArrayNullOrEmpty(distances))
            {
                throw new ArgumentOutOfRangeException(nameof(distances));
            }

            var clustersStagesGroupsMembers = new ClusteringFullResultListContainer();

            var tasks = new List<Task<ClusteringFullResultListContainer.Chain>>();

            int maxThreads = Environment.ProcessorCount;

            for (int chainIndex = 0; chainIndex < chainInteractingAtomLists.ChainList.Count; chainIndex++)
            {
                int chainIndexLocal = chainIndex;
                tasks.Add(Task.Run(() => ClusterByDistance(chainInteractingAtomLists.ChainList[chainIndexLocal], distances[chainIndexLocal], maximumGroupSize, maximumDistance, progressActionSet)));

                while (maxThreads > 0 && tasks.Count(task => task != null && !task.IsCompleted) >= maxThreads)
                {
                    try
                    {
                        Task[] tasksToWaitFor = tasks.Where(task => task != null && !task.IsCompleted).ToArray<Task>();
                        if (tasksToWaitFor.Length > 0)
                        {
                            Task.WaitAny(tasksToWaitFor);
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }

            try
            {
                Task[] tasksToWaitFor = tasks.Where(task => task != null && !task.IsCompleted).ToArray<Task>();

                if (tasksToWaitFor.Length > 0)
                {
                    Task.WaitAll(tasksToWaitFor);
                }
            }
            catch (AggregateException)
            {
            }

            //for (var taskIndex = 0; taskIndex < tasks.Length; taskIndex++)
            for (int taskIndex = 0; taskIndex < tasks.Count; taskIndex++)
            {
                if (tasks[taskIndex] != null)
                {
                    clustersStagesGroupsMembers.ChainList.Add(tasks[taskIndex].Result);
                    tasks[taskIndex].Dispose();
                    tasks[taskIndex] = null;
                }
                else
                {
                    //clustersStagesGroupsMembers.ChainList.Add(new List<List<List<int>>>());
                    clustersStagesGroupsMembers.ChainList.Add(new ClusteringFullResultListContainer.Chain());
                }
            }

            return clustersStagesGroupsMembers;
        }

        /// <summary>
        ///     This method clusters a single list of atoms by the distance specified.  This method is not concerned with the
        ///     attribute used to calculate the distance matrix as this should be done by a separate method.
        /// </summary>
        /// <param name="chainInteractingAtomList"></param>
        /// <param name="distances"></param>
        /// <param name="maximumGroupSize"></param>
        /// <param name="maximumDistance"></param>
        /// <param name="clusterIndex"></param>
        /// <returns></returns>
        public static ClusteringFullResultListContainer.Chain /*List<List<List<int>>>*/ ClusterByDistance(ProteinAtomListContainer chainInteractingAtomList, decimal[,] distances, int maximumGroupSize = -1, int maximumDistance = -1, ProgressActionSet progressActionSet = null)
        {
            if (ParameterValidation.IsProteinAtomListContainerNullOrEmpty(chainInteractingAtomList))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomList));
            }

            if (ParameterValidation.IsDecimalArrayNullOrEmpty(distances))
            {
                throw new ArgumentOutOfRangeException(nameof(distances));
            }

            //var stagesGroupsMembers = new List<List<List<int>>>();
            var stagesGroupsMembers = new ClusteringFullResultListContainer.Chain();

            var index = new[] {-1, -1};

            while (true)
            {
                ClusteringFullResultListContainer.Chain.Stage lastStageClone = null;
                int stageIndex = stagesGroupsMembers.StageList.Count - 1;

                if (stagesGroupsMembers.StageList.Count > 0)
                {
                    //lastStageClone = new List<List<int>>();
                    lastStageClone = new ClusteringFullResultListContainer.Chain.Stage();

                    for (int groupIndex = 0; groupIndex < stagesGroupsMembers.StageList[stageIndex].ClusterList.Count; groupIndex++)
                    {
                        //var groupClone = new List<int>(stagesGroupsMembers.StageList[stageIndex].ClusterList[groupIndex].AtomIndexList);
                        lastStageClone.ClusterList.Add(new ClusteringFullResultListContainer.Chain.Stage.Cluster {AtomIndexList = new List<int>(stagesGroupsMembers.StageList[stageIndex].ClusterList[groupIndex].AtomIndexList)});
                    }
                }

                ClusteringFullResultListContainer.Chain.Stage clusterStage = ClusterByDistanceNextStage(lastStageClone, distances, ref index, maximumGroupSize, maximumDistance);

                if ( /*(index[0] != -1) && (index[1] != -1) &&*/ (clusterStage != null) && (clusterStage.ClusterList != null) && (clusterStage.ClusterList.Count > 0))
                {
                    bool equal = true;
                    if ((stageIndex > 0) && (stagesGroupsMembers.StageList.Count > stageIndex) && (clusterStage.ClusterList.Count == stagesGroupsMembers.StageList[stageIndex].ClusterList.Count))
                    {
                        for (int groupIndex = 0; groupIndex < stagesGroupsMembers.StageList[stageIndex].ClusterList.Count; groupIndex++)
                        {
                            if (!stagesGroupsMembers.StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.SequenceEqual(clusterStage.ClusterList[groupIndex].AtomIndexList))
                            {
                                equal = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        equal = false;
                    }

                    if (!equal)
                    {
                        stagesGroupsMembers.StageList.Add(clusterStage);
                    }
                }

                if ((index[0] == -1) || (index[1] == -1))
                {
                    break;
                }
            }

            return stagesGroupsMembers;
        }


        /// <summary>
        ///     This method calculates a distance array/matrix from a list of atoms 3d positions.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <returns></returns>
        public static decimal[,] CalculateDistanceMatrixFrom3DPosition(ProteinAtomListContainer chainInteractingAtomLists)
        {
            if (ParameterValidation.IsProteinAtomListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            return CalculateDistanceMatrixFrom3DPosition(AtomRecordListToPoint3DList(chainInteractingAtomLists));
        }

        /// <summary>
        ///     This method calculates a distance array/matrix from a list of 3d positions.
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static decimal[,] CalculateDistanceMatrixFrom3DPosition(List<Point3D> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException(nameof(positions));
            }

            if (positions.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(positions));
            }

            var distances = new decimal[positions.Count, positions.Count];
            Array.Clear(distances, 0, distances.Length);

            // Calculate distance between every possible pair of atoms in the same chain.
            for (int indexA = 0; indexA < positions.Count; indexA++)
            {
                for (int indexB = 0; indexB < positions.Count; indexB++)
                {
                    if (indexA == indexB)
                    {
                        // Distance between the same point will always be zero.
                        //continue;
                    }
                    else if (indexB >= indexA)
                    {
                        distances[indexA, indexB] = Point3D.Distance3D(positions[indexA], positions[indexB]);
                    }
                    else
                    {
                        // No need to calculate - already done.
                        distances[indexA, indexB] = distances[indexB, indexA];
                    }
                }
            }

            return distances;
        }

        public static int[] BruteForceTravellingSalesmanProblemSolver(ProteinAtomListContainer chainInteractingAtomList, out decimal bestPathDistance)
        {
            if (ParameterValidation.IsProteinAtomListContainerNullOrEmpty(chainInteractingAtomList))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomList));
            }

            return BruteForceTravellingSalesmanProblemSolver(AtomRecordListToPoint3DList(chainInteractingAtomList), out bestPathDistance);
        }

        public static int[] BruteForceTravellingSalesmanProblemSolver(List<Point3D> points, out decimal bestPathDistance)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            if (points.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(points));
            }

            int totalNodes = points.Count;
            var bestPath = new int[totalNodes];
            var currentPath = new int[totalNodes];
            decimal[,] distances = CalculateDistanceMatrixFrom3DPosition(points);
            bestPathDistance = 0;

            Array.Clear(currentPath, 0, currentPath.Length);
            Array.Clear(bestPath, 0, bestPath.Length);

            // To further improve this algorithm, "if ((x >= divroundup(max, 2) && (x % 2 == 0)) continue;" could be added - but no time to test now
            while (true)
            {
                bool findNextPathAgain;
                // Find next path.
                do
                {
                    findNextPathAgain = false;
                    currentPath[totalNodes - 1]++;

                    // Increase the node numbers where applicable.
                    for (int nodeIndex = totalNodes - 1; nodeIndex >= 0; nodeIndex--)
                    {
                        if (currentPath[nodeIndex] >= totalNodes)
                        {
                            currentPath[nodeIndex] = 0;
                            if (nodeIndex > 0)
                            {
                                currentPath[nodeIndex - 1]++;
                            }
                            else
                            {
                                return bestPath;
                            }
                        }
                    }

                    // Make sure none of the nodes are the same node.
                    for (int nodeIndex1 = totalNodes - 1; nodeIndex1 >= 0; nodeIndex1--)
                    {
                        for (int nodeIndex2 = totalNodes - 1; nodeIndex2 >= 0; nodeIndex2--)
                        {
                            if ((nodeIndex1 != nodeIndex2) && (currentPath[nodeIndex1] == currentPath[nodeIndex2]))
                            {
                                findNextPathAgain = true;
                                break;
                            }
                        }
                        if (findNextPathAgain)
                        {
                            break;
                        }
                    }
                } while (findNextPathAgain);

                // Calculate total distance.
                decimal distance = 0.0m;
                for (int nodeIndex = totalNodes - 1; nodeIndex > 0; nodeIndex--)
                {
                    distance += distances[currentPath[nodeIndex], currentPath[nodeIndex - 1]];
                }

                if ((distance < bestPathDistance && distance > 0) || (bestPathDistance == 0))
                {
                    bestPathDistance = distance;

                    for (int nodeIndex = totalNodes - 1; nodeIndex >= 0; nodeIndex--)
                    {
                        bestPath[nodeIndex] = currentPath[nodeIndex];
                    }
                }
            }
        }

        public static void SaveModeSpreadsheet(string saveFilename, Dictionary<string, ProteinInterfaceSymmetryModeValues> symmetryModeDictionary, Dictionary<string, InteractionBetweenProteinInterfacesListContainer> interactionsBetweenProteinInterfacesDictionary)
        {
            if (string.IsNullOrWhiteSpace(saveFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(saveFilename), saveFilename, "Parameter was " + ParameterValidation.NullEmptyOrWhiteSpaceToString(saveFilename));
            }

            if (symmetryModeDictionary == null) // || symmetryModeDictionary.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(symmetryModeDictionary));
            }

            if (interactionsBetweenProteinInterfacesDictionary == null) // || interactionsBetweenProteinInterfacesDictionary.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(interactionsBetweenProteinInterfacesDictionary));
            }

            var spreadsheet = new List<SpreadsheetCell[]>();

            var spreadsheetColumnHeadersRow = new[]
            {
                new SpreadsheetCell("PDB ID"),
                new SpreadsheetCell("Mode Category"),
                new SpreadsheetCell("Straight Interactions Total"),
                new SpreadsheetCell("Diagonal Interactions Total"),
                new SpreadsheetCell("Straight Interactions %"),
                new SpreadsheetCell("Diagonal Interactions %"),
                new SpreadsheetCell("CASA-CBSA"),
                new SpreadsheetCell("CASB-CBSB"),
                new SpreadsheetCell("CASA-CBSB"),
                new SpreadsheetCell("CASB-CBSA"),
                new SpreadsheetCell("CASA-CBSA %"),
                new SpreadsheetCell("CASB-CBSB %"),
                new SpreadsheetCell("CASA-CBSB %"),
                new SpreadsheetCell("CASB-CBSA %"),
            };

            //spreadsheet.Add(String.Join(columnDelimer, spreadsheetHeader));

            foreach (var symmetryModeKvp in symmetryModeDictionary)
            {
                string proteinId = symmetryModeKvp.Key;
                ProteinInterfaceSymmetryModeValues proteinInterfaceModes = symmetryModeKvp.Value;

                if (proteinInterfaceModes != null)
                {
                    //var x = proteinInterfaceModes.InteractionsBetweenProteinInterfaces;
                    int totalInteractions = proteinInterfaceModes.NumberInteractionsDiagonal + proteinInterfaceModes.NumberInteractionsStraight;
                    decimal straightPercentage = totalInteractions > 0 ? Math.Round((proteinInterfaceModes.NumberInteractionsStraight/(decimal) totalInteractions)*100, 2) : 0;
                    decimal diagonalPercentage = totalInteractions > 0 ? Math.Round((proteinInterfaceModes.NumberInteractionsDiagonal/(decimal) totalInteractions)*100, 2) : 0;

                    var modeNumber = (int) proteinInterfaceModes.ProteinInterfaceSymmetryModeEnum;

                    if (modeNumber > (int) ProteinInterfaceSymmetryModeEnum.Invalid)
                    {
                        var row = new[]
                        {
                            new SpreadsheetCell(symmetryModeKvp.Key),
                            new SpreadsheetCell(modeNumber),
                            new SpreadsheetCell(proteinInterfaceModes.NumberInteractionsStraight),
                            new SpreadsheetCell(proteinInterfaceModes.NumberInteractionsDiagonal),
                            new SpreadsheetCell(straightPercentage),
                            new SpreadsheetCell(diagonalPercentage),
                        };

                        if (interactionsBetweenProteinInterfacesDictionary != null && interactionsBetweenProteinInterfacesDictionary.ContainsKey(proteinId))
                        {
                            InteractionBetweenProteinInterfacesListContainer interactionsBetweenProteinInterfaces = interactionsBetweenProteinInterfacesDictionary[proteinId];

                            if (interactionsBetweenProteinInterfaces != null && interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList != null)
                            {
                                const int proteinInterfaceIdA = StaticValues.ProteinInterfaceA;
                                const int proteinInterfaceIdB = StaticValues.ProteinInterfaceB;

                                const int chainIdA = StaticValues.ChainA;
                                const int chainIdB = StaticValues.ChainB;

                                int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A = interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA) +
                                                                       interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA);

                                int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B = interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB) +
                                                                       interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB);

                                int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B = interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB) +
                                                                       interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA);

                                int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A = interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA) +
                                                                       interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB);


                                //int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A = interactionsBetweenProteinInterfaces[proteinInterfaceA, proteinInterfaceA].Count;
                                //int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B = interactionsBetweenProteinInterfaces[proteinInterfaceB, proteinInterfaceB].Count;
                                //int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B = interactionsBetweenProteinInterfaces[proteinInterfaceA, proteinInterfaceB].Count;
                                //int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A = interactionsBetweenProteinInterfaces[proteinInterfaceB, proteinInterfaceA].Count;

                                totalInteractions = Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A + Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B + Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B + Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A;

                                decimal Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A_Percentage = totalInteractions > 0 ? Math.Round((Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A/(decimal) totalInteractions)*100, 2) : 0;
                                decimal Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B_Percentage = totalInteractions > 0 ? Math.Round((Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B/(decimal) totalInteractions)*100, 2) : 0;
                                decimal Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B_Percentage = totalInteractions > 0 ? Math.Round((Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B/(decimal) totalInteractions)*100, 2) : 0;
                                decimal Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A_Percentage = totalInteractions > 0 ? Math.Round((Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A/(decimal) totalInteractions)*100, 2) : 0;

                                var rowAppend = new[]
                                {
                                    new SpreadsheetCell(Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A),
                                    new SpreadsheetCell(Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B),
                                    new SpreadsheetCell(Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B),
                                    new SpreadsheetCell(Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A),
                                    new SpreadsheetCell(Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A_Percentage),
                                    new SpreadsheetCell(Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B_Percentage),
                                    new SpreadsheetCell(Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B_Percentage),
                                    new SpreadsheetCell(Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A_Percentage),
                                };

                                row = row.Concat(rowAppend).ToArray();
                            }
                        }

                        spreadsheet.Add(row);
                    }
                }
            }

            //spreadsheet.Sort();
            spreadsheet = spreadsheet.OrderBy(a => a[0].CellData).ThenBy(a=> a[1].CellData).ToList();

            spreadsheet.Insert(0, spreadsheetColumnHeadersRow);

            //bool saveTsv = true;
            //bool saveXl = true;
            SpreadsheetFileHandler.SaveSpreadsheet(saveFilename, null, spreadsheet); //, saveTsv, saveXl);
        }

        public static void SaveProteinInterfaceDataSpreadsheet(string saveFilename, List<ProteinInterfaceSequenceAndPositionData> proteinInterfaceSequenceAndPositionDataList, int numberProteinInterfacesRequired = -1)
        {
            if (string.IsNullOrWhiteSpace(saveFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(saveFilename), saveFilename, "Parameter was " + ParameterValidation.NullEmptyOrWhiteSpaceToString(saveFilename));
            }

            if (proteinInterfaceSequenceAndPositionDataList == null || proteinInterfaceSequenceAndPositionDataList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfaceSequenceAndPositionDataList));
            }

            var spreadsheet = new List<SpreadsheetCell[]>();

            var spreadsheetColumnHeadersRow = new[]
            {
                new SpreadsheetCell("PDB ID"),
                new SpreadsheetCell("Chain ID"),
                new SpreadsheetCell("ProteinInterface ID"),
                new SpreadsheetCell("ProteinInterface Length"),
                new SpreadsheetCell("ProteinInterface Start Position"),
                new SpreadsheetCell("ProteinInterface End Position"),
                new SpreadsheetCell("ProteinInterface Amino Acids"),
                new SpreadsheetCell("ProteinInterface Interactions Amino Acids"),
                new SpreadsheetCell("Inside ProteinInterface"),
                new SpreadsheetCell("Outside ProteinInterface"),
                new SpreadsheetCell("No Interaction"),
            };

            var pdbIdExcludeList = new List<string>();

            var lockPdbIdExcludeList = new object();


            if (numberProteinInterfacesRequired != -1)
            {
                foreach (ProteinInterfaceSequenceAndPositionData proteinInterfaceSequenceAndPositionData in proteinInterfaceSequenceAndPositionDataList)
                {
                    if (!pdbIdExcludeList.Contains(proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId.ProteinId))
                    {
                        // Check the chain has N proteinInterfaces.
                        if ((proteinInterfaceSequenceAndPositionDataList.Count(data => data.FullProteinInterfaceId.ProteinId == proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId.ProteinId && data.ChainIdLetter == proteinInterfaceSequenceAndPositionData.ChainIdLetter) != numberProteinInterfacesRequired) ||
                            // Check the protein has minimum number of chains which have proteinInterfaces
                            (proteinInterfaceSequenceAndPositionDataList.Count(data => data.FullProteinInterfaceId.ProteinId == proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId.ProteinId) <= numberProteinInterfacesRequired))
                        {
                            pdbIdExcludeList.Add(proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId.ProteinId);
                        }
                    }
                }
            }

            foreach (ProteinInterfaceSequenceAndPositionData proteinInterfaceSequenceAndPositionData in proteinInterfaceSequenceAndPositionDataList)
            {
                if (!pdbIdExcludeList.Contains(proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId.ProteinId))
                {
                    var row = new[]
                    {
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId.ProteinId),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.ChainIdLetter),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.ProteinInterfaceIdLetter),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.ProteinInterfaceLength),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.StartPosition),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.EndPosition),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.AminoAcidSequenceAll1L),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.AminoAcidSequenceInteractionsAll1L),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.AminoAcidSequenceInteractionsInsideProteinInterfacesOnly1L),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.AminoAcidSequenceInteractionsOutsideProteinInterfacesOnly1L),
                        new SpreadsheetCell(proteinInterfaceSequenceAndPositionData.AminoAcidSequenceInteractionsNone1L),
                        //proteinInterfaceSequenceAndPositionData.AminoAcidSequenceAllResidueSequenceIndexes
                    };

                    spreadsheet.Add(row);
                }
            }

            //spreadsheet.Sort();
            spreadsheet = spreadsheet.OrderBy(r => r[0].CellData).ThenBy(r=>r[1].CellData).ThenBy(r=>r[2].CellData).ToList();

            spreadsheet.Insert(0, spreadsheetColumnHeadersRow);

            SpreadsheetFileHandler.SaveSpreadsheet(saveFilename, null, spreadsheet);
        }

        /// <summary>
        ///     This method advances the progress of the clustering process by one stage.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="distances"></param>
        /// <param name="nextSmallestDistanceIndex"></param>
        /// <param name="maximumGroupSize"></param>
        /// <param name="maximumDistance"></param>
        /// <returns></returns>
        public static ClusteringFullResultListContainer.Chain.Stage ClusterByDistanceNextStage(ClusteringFullResultListContainer.Chain.Stage groups, decimal[,] distances, ref int[] nextSmallestDistanceIndex, int maximumGroupSize = -1, int maximumDistance = -1)
        {
            if (distances == null)
            {
                throw new ArgumentOutOfRangeException(nameof(distances));
            }

            if (nextSmallestDistanceIndex == null)
            {
                throw new ArgumentOutOfRangeException(nameof(nextSmallestDistanceIndex));
            }

            if (groups == null)
            {
                if (nextSmallestDistanceIndex != null && nextSmallestDistanceIndex.Length == 2 && nextSmallestDistanceIndex[0] != -1 && nextSmallestDistanceIndex[1] != -1)
                {
                    throw new ArgumentNullException(nameof(groups));
                }
            }


            bool[] groupAlreadyExists;

            // Check the indexes haven't already been clustered - repeat loop until indexes not clustered together are found or the end of the distance matrix is reached.
            while (true)
            {
                groupAlreadyExists = new[] {false, false};

                nextSmallestDistanceIndex = DistanceMatrixCalculations.NextSmallestDistanceIndex(distances, nextSmallestDistanceIndex);

                if ((nextSmallestDistanceIndex[0] == -1) || (nextSmallestDistanceIndex[1] == -1))
                {
                    var isMemberGrouped = new bool[distances.GetLength(0)];
                    Array.Clear(isMemberGrouped, 0, isMemberGrouped.Length);

                    if (groups != null)
                    {
                        // It is the last time the method will be called, so assign a group to any ungrouped objects.
                        for (int groupIndex = 0; groupIndex < groups.ClusterList.Count; groupIndex++)
                        {
                            for (int memberIndex = 0; memberIndex < groups.ClusterList[groupIndex].AtomIndexList.Count; memberIndex++)
                            {
                                isMemberGrouped[groups.ClusterList[groupIndex].AtomIndexList[memberIndex]] = true;
                            }
                        }

                        for (int isMemberGroupedIndex = 0; isMemberGroupedIndex < isMemberGrouped.Length; isMemberGroupedIndex++)
                        {
                            if (!isMemberGrouped[isMemberGroupedIndex])
                            {
                                //var newGroup = new List<int>();
                                var newGroup = new ClusteringFullResultListContainer.Chain.Stage.Cluster();
                                newGroup.AtomIndexList.Add(isMemberGroupedIndex);
                                groups.ClusterList.Add(newGroup);
                            }
                        }
                    }

                    return groups;
                }

                if ((groups != null) && (groups.ClusterList != null) && (groups.ClusterList.Count > 0))
                {
                    bool alreadyClusteredIndexesTogether = false;

                    // Search for indexes, to check if part of current group - if group with both already exists then skip to next.
                    for (int groupIndex = 0; groupIndex < groups.ClusterList.Count; groupIndex++)
                    {
                        if ((groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[0])) && (groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[1])))
                        {
                            alreadyClusteredIndexesTogether = true;
                            break;
                        }
                        if (groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[0]))
                        {
                            groupAlreadyExists[0] = true;
                        }
                        else if (groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[1]))
                        {
                            groupAlreadyExists[1] = true;
                        }
                    }

                    if (!alreadyClusteredIndexesTogether)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            //indexLastA = index[0];
            //indexLastB = index[1];

            if ((nextSmallestDistanceIndex[0] == -1) || (nextSmallestDistanceIndex[1] == -1))
            {
                // Break out of while true loop when the next smallest distance index method returns -1 as no distance index left to process.
                return null;
            }

            if ((maximumDistance > -1) && (Math.Abs(distances[nextSmallestDistanceIndex[0], nextSmallestDistanceIndex[1]]) > maximumDistance))
            {
                // Check if maximum distance constraint will be violated.
                nextSmallestDistanceIndex[0] = -1;
                nextSmallestDistanceIndex[1] = -1;
                return null;
            }

            if ((groups != null) && (groups.ClusterList != null) && (groups.ClusterList.Count == 1) && (groups.ClusterList[0].AtomIndexList.Count == (distances.GetUpperBound(0) - distances.GetLowerBound(0)) + 1))
            {
                // Check if all indexes are in a group together - if so, clustering is finished.
                nextSmallestDistanceIndex[0] = -1;
                nextSmallestDistanceIndex[1] = -1;
                return null;
            }

            if (groups != null && groups.ClusterList != null)
            {
                for (int groupIndex = 0; groupIndex < groups.ClusterList.Count; groupIndex++)
                {
                    if ((groupAlreadyExists[0]) && (groupAlreadyExists[1]))
                    {
                        break;
                    }

                    // Loop through all existing groups to check if either index is already a member, and if so, add its partner.
                    if ((groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[0])) && (!groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[1])) && (!groupAlreadyExists[1]) && ((maximumGroupSize <= -1) || (groups.ClusterList[groupIndex].AtomIndexList.Count < maximumGroupSize)))
                    {
                        groups.ClusterList[groupIndex].AtomIndexList.Add(nextSmallestDistanceIndex[1]);
                        groupAlreadyExists[1] = true;
                        break;
                    }

                    if ((groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[1])) && (!groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[0])) && (!groupAlreadyExists[0]) && ((maximumGroupSize <= -1) || (groups.ClusterList[groupIndex].AtomIndexList.Count < maximumGroupSize)))
                    {
                        groups.ClusterList[groupIndex].AtomIndexList.Add(nextSmallestDistanceIndex[0]);
                        groupAlreadyExists[0] = true;
                        break;
                    }
                }
            }
            else
            {
                //groups = new List<List<int>>();
                groups = new ClusteringFullResultListContainer.Chain.Stage();
            }

            if ((!groupAlreadyExists[0]) && (!groupAlreadyExists[1]))
            {
                //var newGroup = new List<int>();
                var newGroup = new ClusteringFullResultListContainer.Chain.Stage.Cluster();

                newGroup.AtomIndexList.Add(nextSmallestDistanceIndex[0]);
                newGroup.AtomIndexList.Add(nextSmallestDistanceIndex[1]);
                groups.ClusterList.Add(newGroup);
                return groups;
            }

            // Merge any groups having either index.
            var groupsToMerge = new List<int>();

            for (int groupIndex = 0; groupIndex < groups.ClusterList.Count; groupIndex++)
            {
                if (groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[0]) || groups.ClusterList[groupIndex].AtomIndexList.Contains(nextSmallestDistanceIndex[1]))
                {
                    if (!groupsToMerge.Contains(groupIndex))
                    {
                        groupsToMerge.Add(groupIndex);
                    }
                }
            }

            if ((groupsToMerge != null) && (groupsToMerge.Count > 1))
            {
                groupsToMerge.Sort();

                // Merge groups containing both.
                //var newGroups = new List<List<int>>();
                var newGroups = new ClusteringFullResultListContainer.Chain.Stage();

                //var newGroup = new List<int>();
                var newGroup = new ClusteringFullResultListContainer.Chain.Stage.Cluster();

                for (int groupToMergeIndex = groupsToMerge.Count - 1; groupToMergeIndex >= 0; groupToMergeIndex--)
                {
                    //var testGroup = new List<int>(newGroup.AtomIndexList);
                    var testGroup = new ClusteringFullResultListContainer.Chain.Stage.Cluster {AtomIndexList = new List<int>(newGroup.AtomIndexList)};

                    testGroup.AtomIndexList.AddRange(groups.ClusterList[groupsToMerge[groupToMergeIndex]].AtomIndexList);
                    testGroup.AtomIndexList = testGroup.AtomIndexList.Distinct().ToList();

                    if (maximumGroupSize > -1)
                    {
                        if (testGroup.AtomIndexList.Count > maximumGroupSize)
                        {
                            newGroups.ClusterList.Add(newGroup);
                            testGroup = null;
                            newGroup.AtomIndexList = groups.ClusterList[groupsToMerge[groupToMergeIndex]].AtomIndexList.Distinct().ToList();
                        }
                    }
                    else
                    {
                        newGroup = testGroup;
                    }

                    newGroup.AtomIndexList.Sort();
                }

                if ((newGroup != null) && (newGroup.AtomIndexList != null) && (newGroup.AtomIndexList.Count > 0))
                {
                    newGroups.ClusterList.Add(newGroup);
                    newGroup = null;
                }

                if ((newGroups != null) && (newGroups.ClusterList != null) && (newGroups.ClusterList.Count > 0))
                {
                    for (int groupToMergeIndex = groupsToMerge.Count - 1; groupToMergeIndex >= 0; groupToMergeIndex--)
                    {
                        groups.ClusterList.RemoveAt(groupsToMerge[groupToMergeIndex]);
                    }

                    for (int newGroupsIndex = newGroups.ClusterList.Count - 1; newGroupsIndex >= 0; newGroupsIndex--)
                    {
                        if ((newGroups.ClusterList[newGroupsIndex].AtomIndexList.Count > 0) && ((maximumGroupSize <= -1) || (newGroups.ClusterList[newGroupsIndex].AtomIndexList.Count <= maximumGroupSize)))
                        {
                            groups.ClusterList.Add(newGroups.ClusterList[newGroupsIndex]);
                        }
                    }
                }
            }

            // Remove any duplicates that somehow got in - shouldn't happen actually - just to be on the safe side in case of future code changes
            for (int groupIndex = groups.ClusterList.Count - 1; groupIndex >= 0; groupIndex--)
            {
                if (groups != null && groups.ClusterList != null && groups.ClusterList[groupIndex] != null)
                {
                    groups.ClusterList[groupIndex].AtomIndexList.Sort();
                    groups.ClusterList[groupIndex].AtomIndexList = groups.ClusterList[groupIndex].AtomIndexList.Distinct().ToList();
                }

                ////////Console.WriteLine("group " + groupIndex + ": " + string.Join(", ", groups.ClusterList[groupIndex].AtomIndexList));
            }

            return groups;
        }

        /// <summary>
        ///     This method calculates a distance array/matrix from a list of atoms residue sequence index positions.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <returns></returns>
        public static decimal[,] CalculateDistanceMatrixFromResidueSequenceIndex(ProteinAtomListContainer chainInteractingAtomLists)
        {
            if (ParameterValidation.IsProteinAtomListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            List<Point1D> positions = chainInteractingAtomLists.AtomList.Select(PointConversions.AtomResidueSequencePoint1D).ToList();

            return CalculateDistanceMatrixFromResidueSequenceIndex(positions);
        }

        /// <summary>
        ///     This method calculates a distance array/matrix from a list of residue sequence index positions.
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static decimal[,] CalculateDistanceMatrixFromResidueSequenceIndex(List<Point1D> positions)
        {
            if (ParameterValidation.IsListNullOrEmpty(positions))
            {
                throw new ArgumentOutOfRangeException(nameof(positions));
            }

            var distances = new decimal[positions.Count, positions.Count];
            Array.Clear(distances, 0, distances.Length);

            // Calculate distance between every possible pair of atoms in the same chain.
            for (int indexA = 0; indexA < positions.Count; indexA++)
            {
                for (int indexB = 0; indexB < positions.Count; indexB++)
                {
                    if (indexA == indexB)
                    {
                        // Distance between the same point will always be zero.
                        //continue;
                    }
                    else if (indexB >= indexA)
                    {
                        distances[indexA, indexB] = Point1D.Distance1D(positions[indexA], positions[indexB]);
                    }
                    else
                    {
                        // In this case, indexB is less than indexA, so no need to calculate - already done.
                        distances[indexA, indexB] = distances[indexB, indexA];
                    }
                }
            }

            return distances;
        }

        /// <summary>
        /// </summary>
        /// <param name="interactingAtomList"></param>
        /// <param name="longestDistance"></param>
        /// <returns></returns>
        public static int[] FindLongestDistanceBetweenNodes(ProteinAtomListContainer interactingAtomList, out decimal longestDistance)
        {
            if (ParameterValidation.IsProteinAtomListContainerNullOrEmpty(interactingAtomList))
            {
                throw new ArgumentOutOfRangeException(nameof(interactingAtomList));
            }

            return FindLongestDistanceBetweenNodes(AtomRecordListToPoint3DList(interactingAtomList), out longestDistance);
        }

        /// <summary>
        ///     Finds the longest distance between points in a list.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="longestDistance"></param>
        /// <returns>Returns the indexes in the list/array where the longest distance was found.</returns>
        public static int[] FindLongestDistanceBetweenNodes(List<Point3D> points, out decimal longestDistance)
        {
            if (ParameterValidation.IsListNullOrEmpty(points))
            {
                throw new ArgumentOutOfRangeException(nameof(points));
            }

            decimal[,] distances = CalculateDistanceMatrixFrom3DPosition(points);
            var indexes = new[] {0, 0};

            for (int distancesIndex1 = 0; distancesIndex1 < points.Count; distancesIndex1++)
            {
                for (int distancesIndex2 = 0; distancesIndex2 < points.Count; distancesIndex2++)
                {
                    if ((distancesIndex1 != distancesIndex2) && (distances[distancesIndex1, distancesIndex2] > distances[indexes[0], indexes[1]]))
                    {
                        indexes[0] = distancesIndex1;
                        indexes[1] = distancesIndex2;
                    }
                }
            }

            longestDistance = distances[indexes[0], indexes[1]];

            return indexes;
        }


        /// <summary>
        ///     This method clusters by the distance between the residue sequence indexes of atoms.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="maximumGroupSize"></param>
        /// <param name="maximumDistance"></param>
        /// <returns></returns>
        public static ClusteringFullResultListContainer ClusterByDistanceResidueSequenceIndex(ProteinChainListContainer chainInteractingAtomLists, int maximumGroupSize = -1, int maximumDistance = -1, ProgressActionSet progressActionSet = null)
        {
            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            var distances = new decimal[chainInteractingAtomLists.ChainList.Count][,];
            for (int clusterIndex = 0; clusterIndex < chainInteractingAtomLists.ChainList.Count; clusterIndex++)
            {
                distances[clusterIndex] = CalculateDistanceMatrixFromResidueSequenceIndex(chainInteractingAtomLists.ChainList[clusterIndex]);
            }

            ClusteringFullResultListContainer clusters = ClusterByDistance(chainInteractingAtomLists, distances, maximumGroupSize, maximumDistance, progressActionSet);

            return clusters;
        }

        /// <summary>
        ///     This method performs clustering on sequence ids found in the FASTA files.  The ids corresponding PDB file is
        ///     searched for in the specified PDB Files Folders, which is loaded and clustered.
        /// </summary>
        /// <param name="fastaFiles"></param>
        /// <param name="pdbFilesFolders"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progressBar"></param>
        /// <param name="estimatedTimeRemainingLabel"></param>
        /// <param name="consoleTextBox"></param>
        /// <param name="clusteringMethodOptions"></param>
        public static void ClusterAllForProteinInterfaceAndSymmetryDetection(
            decimal maxAtomInterationDistance,
            decimal minimumProteinInterfaceDensity,
            string[] fastaFiles,
            string[] pdbFilesFolders,
            string matlabOutputFilenameTemplate,
            string proteinInterfaceInteractionListOutputFilename,
            string proteinInterfaceDataOutputFilename,
            string densityAllOutputFilename,
            string interactionModesOutputFilename,
            ClusteringMethodOptions clusteringMethodOptions,
            CancellationToken cancellationToken,
            ProgressActionSet progressActionSet,
            int totalThreads = -1,
            int maximumGroupSize = -1,
            int maximumDistance = -1)
        {
            const string templateVariableFastaFilename = "%fasta_filename%";

            if (fastaFiles == null || fastaFiles.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fastaFiles));
            }

            if (pdbFilesFolders == null || pdbFilesFolders.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilesFolders));
            }

            if (string.IsNullOrWhiteSpace(matlabOutputFilenameTemplate))
            {
                throw new ArgumentOutOfRangeException(nameof(matlabOutputFilenameTemplate));
            }

            if (string.IsNullOrWhiteSpace(proteinInterfaceInteractionListOutputFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfaceInteractionListOutputFilename));
            }

            if (string.IsNullOrWhiteSpace(proteinInterfaceDataOutputFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfaceDataOutputFilename));
            }

            if (string.IsNullOrWhiteSpace(densityAllOutputFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(densityAllOutputFilename));
            }

            if (string.IsNullOrWhiteSpace(interactionModesOutputFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(interactionModesOutputFilename));
            }

            fastaFiles = fastaFiles.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            pdbFilesFolders = pdbFilesFolders.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            if (fastaFiles.Length != 1)
            {
                ProgressActionSet.Report("Error: You may only cluster 1 FASTA file per run.  Please select just one FASTA File.", progressActionSet);
                return;
            }

            if (totalThreads <= 0)
            {
                totalThreads = Environment.ProcessorCount;
            }

            proteinInterfaceInteractionListOutputFilename = proteinInterfaceInteractionListOutputFilename.Replace(templateVariableFastaFilename, FileAndPathMethods.FullPathToFilename(fastaFiles[0]));
            proteinInterfaceDataOutputFilename = proteinInterfaceDataOutputFilename.Replace(templateVariableFastaFilename, FileAndPathMethods.FullPathToFilename(fastaFiles[0]));
            //proteinInterfaceDataTwoProteinInterfacesPerChainFilename = proteinInterfaceDataTwoProteinInterfacesPerChainFilename.Replace(templateVariableFastaFilename, ProteinDataBankFileOperations.FullPathToFilename(fastaFiles[0]));
            interactionModesOutputFilename = interactionModesOutputFilename.Replace(templateVariableFastaFilename, FileAndPathMethods.FullPathToFilename(fastaFiles[0]));
            //densityOnlyTwoProteinInterfacesPerChainOutputFilename = densityOnlyTwoProteinInterfacesPerChainOutputFilename.Replace(templateVariableFastaFilename, ProteinDataBankFileOperations.FullPathToFilename(fastaFiles[0]));
            densityAllOutputFilename = densityAllOutputFilename.Replace(templateVariableFastaFilename, FileAndPathMethods.FullPathToFilename(fastaFiles[0]));
            matlabOutputFilenameTemplate = matlabOutputFilenameTemplate.Replace(templateVariableFastaFilename, FileAndPathMethods.FullPathToFilename(fastaFiles[0]));


            int[] numberSequencesLoaded;
            List<ISequence> sequences = SequenceFileHandler.LoadSequenceFileList(fastaFiles, StaticValues.MolNameProteinAcceptedValues, out numberSequencesLoaded, true);
            var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequences);

            List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequences, true);
            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders));
            var tasks = new List<Task<ClusterAllTaskResult>>();

            for (int numberSequencesLoadedIndex = numberSequencesLoaded.GetLowerBound(0); numberSequencesLoadedIndex <= numberSequencesLoaded.GetUpperBound(0); numberSequencesLoadedIndex++)
            {
                if (numberSequencesLoaded[numberSequencesLoadedIndex] > 0)
                {
                    ProgressActionSet.Report("Loaded " + numberSequencesLoaded[numberSequencesLoadedIndex] + " sequences from file: " + fastaFiles[numberSequencesLoadedIndex], progressActionSet);
                }
                else
                {
                    ProgressActionSet.Report("Error could not load file: " + fastaFiles[numberSequencesLoadedIndex], progressActionSet);
                }
            }

            // Setup user proteinInterface.
            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);
            ProgressActionSet.Report("Clustering " + pdbIdList.Count + " proteins", progressActionSet);

            ProteinDataBankFileOperations.ShowMissingPdbFiles(pdbFilesArray, pdbIdList, progressActionSet);

            var tasksCombinedResult = new ClusterAllTaskResult();

            for (int pdbFileNumber = 0; pdbFileNumber < pdbFilesArray.Length; pdbFileNumber++)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string pdbFilename = pdbFilesArray[pdbFileNumber];
                    string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                    // Check if the file found is included in the white list.
                    if (pdbIdList != null && !pdbIdList.Contains(proteinId))
                    {
                        ProgressActionSet.Report(proteinId + " was not in the white list - skipping", progressActionSet);
                        continue;
                    }

                    while (tasks.Count(t => t != null && !t.IsCompleted) >= totalThreads)
                    {
                        Task[] tasksToWait = tasks.Where(t => t != null && !t.IsCompleted).ToArray<Task>();

                        if (tasksToWait.Length > 0)
                        {
                            Task.WaitAny(tasksToWait);
                        }
                    }

                    int localPdbFileNumber = pdbFileNumber;
                    Task<ClusterAllTaskResult> task = Task.Run(() =>
                    {
                        var taskResult = new ClusterAllTaskResult();

                        try
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            ClusterProteinDataBankFileResult fullClusterPdbResult = ClusterProteinDataBankFile(cancellationToken, maxAtomInterationDistance, minimumProteinInterfaceDensity, pdbFilename, pdbIdChainIdList, clusteringMethodOptions, maximumGroupSize, maximumDistance, progressActionSet);

                            if (fullClusterPdbResult == null)
                            {
                                ProgressActionSet.Report("Could not cluster file#" + localPdbFileNumber + ": " + proteinId, progressActionSet);
                                return taskResult;
                            }


                            int[] numberProteinInterfacesPerChain = ProteinInterfaceDetection.FindNumberProteinInterfacesPerChain(fullClusterPdbResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesClusteringResult, fullClusterPdbResult.ProteinInterfaceAnalysisResultData.DetectedBestStageIndexes);

                            taskResult.ProteinInterfaceCountDictionary.Add(proteinId, numberProteinInterfacesPerChain);

                            // For proteins with 2 chains, and 2 proteinInterfaces per chain, check symmetry mode
                            if (numberProteinInterfacesPerChain.Length == 2 && numberProteinInterfacesPerChain[StaticValues.ChainA] == 2 && numberProteinInterfacesPerChain[StaticValues.ChainB] == 2)
                            {
                                ProteinInterfaceSymmetryModeValues symmetry = HomodimerSymmetry.CheckDimer2X2ProteinInterfaceSymmetry(fullClusterPdbResult.ProteinInterfaceAnalysisResultData.InteractionsBetweenProteinInterfacesList);

                                if (symmetry != null)
                                {
                                    taskResult.SymmetryModeDictionary.Add(proteinId, symmetry);
                                }
                                else
                                {
                                    ProgressActionSet.Report(proteinId + ": Symmetry detection error", progressActionSet);
                                }
                            }

                            if (fullClusterPdbResult.ProteinInterfaceAnalysisResultData.InteractionsBetweenProteinInterfacesList != null)
                            {
                                taskResult.InteractionsBetweenProteinInterfacesDictionary.Add(proteinId, fullClusterPdbResult.ProteinInterfaceAnalysisResultData.InteractionsBetweenProteinInterfacesList);
                            }

                            if (fullClusterPdbResult.ProteinInterfaceAnalysisResultData.InteractionProteinInterfaceClusteringHierarchyData != null && fullClusterPdbResult.ProteinInterfaceAnalysisResultData.InteractionProteinInterfaceClusteringHierarchyData.Count > 0)
                            {
                                foreach (InteractionProteinInterfaceClusteringHierarchyData interactionProteinInterfaceClusteringHierarchyData in fullClusterPdbResult.ProteinInterfaceAnalysisResultData.InteractionProteinInterfaceClusteringHierarchyData)
                                {
                                    taskResult.InteractionProteinInterfaceClusteringHierarchySpreadsheet.Add(interactionProteinInterfaceClusteringHierarchyData);
                                }
                            }

                            if (fullClusterPdbResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList != null && fullClusterPdbResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList.Count > 0)
                            {
                                taskResult.ProteinInterfacePositionDataList.AddRange(fullClusterPdbResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList);
                            }

                            ////UserProteinInterfaceOperations.LabelEstimatedTimeRemainingUpdate(estimatedTimeRemainingLabel, startTicks, localPdbFileNumber + 1, pdbFilesArray.Length);
                            //UserProteinInterfaceOperations.ProgressBarIncrement(progressBar, 1);

                            ProgressActionSet.ProgressAction(1, progressActionSet);
                        }
                        catch (OperationCanceledException)
                        {
                        }

                        return taskResult;
                    }, cancellationToken);

                    tasks.Add(task);
                }
                catch (OperationCanceledException)
                {
                    //UserProteinInterfaceOperations.ProgressBarReset(progressBar, 0, 100, 0);
                    ////UserProteinInterfaceOperations.LabelEstimatedTimeRemainingUpdate(estimatedTimeRemainingLabel, 0, 1, 1);
                    ProgressActionSet.StartAction(100, progressActionSet);
                    ProgressActionSet.ProgressAction(100, progressActionSet);
                    ProgressActionSet.FinishAction(false, progressActionSet);
                    break;
                }
            }

            // Wait for all tasks to complete before saving results
            try
            {
                Task[] tasksToWait = tasks.Where(task => task != null && !task.IsCompleted).ToArray<Task>();
                if (tasksToWait.Length > 0)
                {
                    Task.WaitAll(tasksToWait);
                }
            }
            catch (AggregateException)
            {
            }

            //RemoveCompletedTasks(tasks);

            if (!cancellationToken.IsCancellationRequested)
            {
                foreach (var task in tasks.Where(t => t != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted && t.Result != null))
                {
                    if (task.Result.ProteinInterfacePositionDataList != null && task.Result.ProteinInterfacePositionDataList.Count > 0)
                    {
                        tasksCombinedResult.ProteinInterfacePositionDataList.AddRange(task.Result.ProteinInterfacePositionDataList);
                    }

                    if (task.Result.InteractionProteinInterfaceClusteringHierarchySpreadsheet != null && task.Result.InteractionProteinInterfaceClusteringHierarchySpreadsheet.InteractionProteinInterfaceDensityDataList != null && task.Result.InteractionProteinInterfaceClusteringHierarchySpreadsheet.InteractionProteinInterfaceDensityDataList.Count > 0)
                    {
                        tasksCombinedResult.InteractionProteinInterfaceClusteringHierarchySpreadsheet.InteractionProteinInterfaceDensityDataList.AddRange(task.Result.InteractionProteinInterfaceClusteringHierarchySpreadsheet.InteractionProteinInterfaceDensityDataList);
                    }

                    if (task.Result.InteractionsBetweenProteinInterfacesDictionary != null && task.Result.InteractionsBetweenProteinInterfacesDictionary.Count > 0)
                    {
                        foreach (var item in task.Result.InteractionsBetweenProteinInterfacesDictionary)
                        {
                            tasksCombinedResult.InteractionsBetweenProteinInterfacesDictionary.Add(item.Key, item.Value);
                        }
                    }

                    if (task.Result.SymmetryModeDictionary != null && task.Result.SymmetryModeDictionary.Count > 0)
                    {
                        foreach (var item in task.Result.SymmetryModeDictionary)
                        {
                            tasksCombinedResult.SymmetryModeDictionary.Add(item.Key, item.Value);
                        }
                    }

                    if (task.Result.ProteinInterfaceCountDictionary != null && task.Result.ProteinInterfaceCountDictionary.Count > 0)
                    {
                        foreach (var item in task.Result.ProteinInterfaceCountDictionary)
                        {
                            tasksCombinedResult.ProteinInterfaceCountDictionary.Add(item.Key, item.Value);
                        }
                    }
                }
                tasksCombinedResult.InteractionsBetweenProteinInterfacesDictionary = tasksCombinedResult.InteractionsBetweenProteinInterfacesDictionary.OrderBy(x => x.Key).ToDictionary(t => t.Key, t => t.Value);
                tasksCombinedResult.InteractionProteinInterfaceClusteringHierarchySpreadsheet.InteractionProteinInterfaceDensityDataList = tasksCombinedResult.InteractionProteinInterfaceClusteringHierarchySpreadsheet.InteractionProteinInterfaceDensityDataList.OrderBy(x => x.ProteinId).ThenBy(x => x.ChainId).ThenBy(x => x.ClusteringStageProteinInterfaceFoundAt).ToList();
                tasksCombinedResult.ProteinInterfaceCountDictionary = tasksCombinedResult.ProteinInterfaceCountDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                tasksCombinedResult.ProteinInterfacePositionDataList = tasksCombinedResult.ProteinInterfacePositionDataList.OrderBy(x => x.FullProteinInterfaceId.ProteinId).ThenBy(x => x.ChainIdLetter).ThenBy(x => x.ProteinInterfaceIdLetter).ToList();
                tasksCombinedResult.SymmetryModeDictionary = tasksCombinedResult.SymmetryModeDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);


                // Save proteinInterface information spreadsheet
                for (int proteinInterfaceNumber = -1; proteinInterfaceNumber <= 3; proteinInterfaceNumber++)
                {
                    if (proteinInterfaceNumber == 0) continue;

                    string proteinInterfaceDataOutputFilenameN = proteinInterfaceDataOutputFilename.Replace("%n%", (proteinInterfaceNumber == -1 ? "any" : "" + proteinInterfaceNumber));

                    SaveProteinInterfaceDataSpreadsheet(proteinInterfaceDataOutputFilenameN, tasksCombinedResult.ProteinInterfacePositionDataList, proteinInterfaceNumber);

                    ProgressActionSet.Report("Saved interaction proteinInterfaces data results [" + (proteinInterfaceNumber == -1 ? "all proteinInterfaces" : proteinInterfaceNumber + " proteinInterfaces per chain") + "]: " + proteinInterfaceDataOutputFilenameN, progressActionSet);
                }

                // ProteinInterfaces interaction list
                for (int proteinInterfaceNumber = -1; proteinInterfaceNumber <= 3; proteinInterfaceNumber++)
                {
                    if (proteinInterfaceNumber == 0) continue;

                    string proteinInterfaceInteractionListOutputFilenameN = proteinInterfaceInteractionListOutputFilename.Replace("%n%", (proteinInterfaceNumber == -1 ? "any" : "" + proteinInterfaceNumber));

                    SaveProteinInterfaceInteractionsSpreadsheet(proteinInterfaceInteractionListOutputFilenameN, tasksCombinedResult.InteractionsBetweenProteinInterfacesDictionary, tasksCombinedResult.ProteinInterfaceCountDictionary, proteinInterfaceNumber);

                    ProgressActionSet.Report("Saved interaction proteinInterfaces list results [" + (proteinInterfaceNumber == -1 ? "all proteinInterfaces" : proteinInterfaceNumber + " proteinInterfaces per chain") + "]: " + proteinInterfaceInteractionListOutputFilenameN, progressActionSet);
                }

                // Save density filename spreadsheets
                for (int proteinInterfaceNumber = -1; proteinInterfaceNumber <= 3; proteinInterfaceNumber++)
                {
                    if (proteinInterfaceNumber == 0) continue;

                    string densityAllOutputFilenameN = densityAllOutputFilename.Replace("%n%", (proteinInterfaceNumber == -1 ? "any" : "" + proteinInterfaceNumber));

                    tasksCombinedResult.InteractionProteinInterfaceClusteringHierarchySpreadsheet.SaveToFile(densityAllOutputFilenameN, proteinInterfaceNumber);

                    ProgressActionSet.Report("Saved interaction proteinInterface density results [" + (proteinInterfaceNumber == -1 ? "all proteinInterfaces" : proteinInterfaceNumber + " proteinInterfaces per chain") + "]: " + densityAllOutputFilenameN, progressActionSet);
                }

                // Save interaction mode spreadsheet
                SaveModeSpreadsheet(interactionModesOutputFilename, tasksCombinedResult.SymmetryModeDictionary, tasksCombinedResult.InteractionsBetweenProteinInterfacesDictionary);
                ProgressActionSet.Report("Saved interaction mode category results [2 proteinInterfaces per chain]: " + interactionModesOutputFilename, progressActionSet);

                // Update user proteinInterface with user feedback that clustering is finished.
                ProgressActionSet.Report("Clustering is finished.", progressActionSet);
            }
            else
            {
                ProgressActionSet.Report("Clustering was cancelled.", progressActionSet);
            }

            //UserProteinInterfaceOperations.ProgressBarReset(progressBar, 0, 100, 0);
            //UserProteinInterfaceOperations.EstimatedTimeRemainingTimeSpan(0, 1, 1);

            ProgressActionSet.StartAction(100, progressActionSet);
        }

        /// <summary>
        ///     This method clusters by the distance between the 3d positions of atoms.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="maximumGroupSize"></param>
        /// <param name="maximumDistance"></param>
        /// <returns></returns>
        public static ClusteringFullResultListContainer ClusterByDistance3D(
            ProteinChainListContainer chainInteractingAtomLists,
            int maximumGroupSize = -1,
            int maximumDistance = -1,
            ProgressActionSet progressActionSet = null)
        {
            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            var distances = new decimal[chainInteractingAtomLists.ChainList.Count][,];
            for (int clusterIndex = 0; clusterIndex < chainInteractingAtomLists.ChainList.Count; clusterIndex++)
            {
                distances[clusterIndex] = CalculateDistanceMatrixFrom3DPosition(chainInteractingAtomLists.ChainList[clusterIndex]);
            }

            ClusteringFullResultListContainer clusters = ClusterByDistance(chainInteractingAtomLists, distances, maximumGroupSize, maximumDistance);

            return clusters;
        }
    }
}