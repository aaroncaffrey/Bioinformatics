//-----------------------------------------------------------------------
// <copyright file="VectorController.cs" company="Aaron Caffrey">
//     Copyright (c) 2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bio;
using BioinformaticsHelperLibrary.Dssp;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.TaskManagement;
using BioinformaticsHelperLibrary.Upgma;
using BioinformaticsHelperLibrary.UserProteinInterface;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public static class VectorController
    {
        public static string VectorProteinInterfaceWholeTreeHeader(VectorProteinInterfaceWhole a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            return Newick.TreeHeaderSafeName(a.FullProteinInterfaceId.ToString() + "_" + (a.ReversedSequence ? "R_" : "F_") + (a.ReversedInteractions ? "RI_" : "") + string.Join("", a.InteractionBools().Select(Convert.ToInt32)) + "_" + (a.VectorProteinInterfacePartList.Count(b => b.InteractionToNonProteinInterface) > 0 ? 1 : 0));
        }

        /*
        public static void ClusterVectorDistanceMatrixUpgma(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, decimal[,] vectorDistanceMatrix, int minimumOutputTreeLeafs, out List<string> vectorNames, out List<List<UpgmaNode>> nodeList, out List<List<string>> treeList, ProgressActionSet progressActionSet)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (vectorDistanceMatrix == null) throw new ArgumentNullException(nameof(vectorDistanceMatrix));

            vectorNames = vectorProteinInterfaceWholeList.Select(VectorProteinInterfaceWholeTreeHeader).ToList();

            List<string> finalTreeLeafOrderList;
            UpgmaClustering.Upgma(vectorDistanceMatrix, vectorNames, minimumOutputTreeLeafs, out nodeList, out treeList, out finalTreeLeafOrderList, false, progressActionSet);
        }
        */

        public static void BestDistanceMatrixWithPartsAlignment(CancellationToken cancellationToken, List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, VectorDistanceMeasurementValues vectorDistanceMeasurementValues, out double[,] optimisticDistanceMatrix,/* out double[,] pessimisticDistanceMatrix,*/ ProgressActionSet progressActionSet)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (vectorDistanceMeasurementValues == null) throw new ArgumentNullException(nameof(vectorDistanceMeasurementValues));

            var totalVectors = vectorProteinInterfaceWholeList.Count;
            
            var optimisticDistanceMatrix2 = new double[totalVectors, totalVectors];
            //var pessimisticDistanceMatrix2 = new double[totalVectors, totalVectors];

            var workDivision = new WorkDivision(vectorProteinInterfaceWholeList.Count, -1);

            ProgressActionSet.StartAction(vectorProteinInterfaceWholeList.Count, progressActionSet);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                var task = Task.Run(() =>
                {
                    for (int indexX = workDivision.ThreadFirstIndex[localThreadIndex]; indexX <= workDivision.ThreadLastIndex[localThreadIndex]; indexX++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                        var vectorProteinInterfaceWholeX = vectorProteinInterfaceWholeList[indexX];

                        for (int indexY = 0; indexY < vectorProteinInterfaceWholeList.Count; indexY++)
                        {
                            if (indexX >= indexY)
                            {
                                continue;
                            }

                            var vectorProteinInterfaceWholeY = vectorProteinInterfaceWholeList[indexY];

                            if (vectorProteinInterfaceWholeX.FullProteinInterfaceId == vectorProteinInterfaceWholeY.FullProteinInterfaceId) continue;

                            double optimisticDistance;
                            //double pessimisticDistance;
                            BestDistanceWithPartsAlignment(vectorProteinInterfaceWholeX, vectorProteinInterfaceWholeY, vectorDistanceMeasurementValues, out optimisticDistance/*, out pessimisticDistance*/);

                            var lengthDifference = Math.Abs(vectorProteinInterfaceWholeX.ProteinInterfaceLength - vectorProteinInterfaceWholeY.ProteinInterfaceLength);

                            var lengthDistance = lengthDifference * vectorDistanceMeasurementValues.DifferentLengthProteinInterface;

                            optimisticDistance += lengthDistance;
                            //pessimisticDistance += lengthDistance;

                            optimisticDistanceMatrix2[indexX, indexY] = optimisticDistance;
                            //pessimisticDistanceMatrix2[indexX, indexY] = pessimisticDistance;

                            optimisticDistanceMatrix2[indexY, indexX] = optimisticDistance;
                            //pessimisticDistanceMatrix2[indexY, indexX] = pessimisticDistance;
                        }

                        workDivision.IncrementItemsCompleted(1);
                        ProgressActionSet.ProgressAction(1, progressActionSet);
                        ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                    }

                }, cancellationToken);

                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            ProgressActionSet.FinishAction(true, progressActionSet);

            optimisticDistanceMatrix = optimisticDistanceMatrix2;
            //pessimisticDistanceMatrix = pessimisticDistanceMatrix2;
        }

        public static void BestDistanceWithPartsAlignment(VectorProteinInterfaceWhole vectorProteinInterfaceWhole1, VectorProteinInterfaceWhole vectorProteinInterfaceWhole2, VectorDistanceMeasurementValues vectorDistanceMeasurementValues, out double optimisticDistance/*, out double pessimisticDistance*/)
        {
            if (vectorProteinInterfaceWhole1 == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWhole1));
            if (vectorProteinInterfaceWhole2 == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWhole2));
            if (vectorDistanceMeasurementValues == null) throw new ArgumentNullException(nameof(vectorDistanceMeasurementValues));

            var proteinInterfaceLength1 = vectorProteinInterfaceWhole1.ProteinInterfaceLength;
            var proteinInterfaceLength2 = vectorProteinInterfaceWhole2.ProteinInterfaceLength;

            var proteinInterfaceLengthDifference = Math.Abs(proteinInterfaceLength1 - proteinInterfaceLength2);

            var longerProteinInterfaceLength = proteinInterfaceLength1 > proteinInterfaceLength2 ? proteinInterfaceLength1 : proteinInterfaceLength2;
            var shorterProteinInterfaceLength = proteinInterfaceLength1 < proteinInterfaceLength2 ? proteinInterfaceLength1 : proteinInterfaceLength2;

            var longerProteinInterface = proteinInterfaceLength1 > proteinInterfaceLength2 ? vectorProteinInterfaceWhole1 : vectorProteinInterfaceWhole2;
            var shorterProteinInterface = longerProteinInterface == vectorProteinInterfaceWhole1 ? vectorProteinInterfaceWhole2 : vectorProteinInterfaceWhole1;

            /*
             * A: Longer ProteinInterface
             * B: Shorter ProteinInterface
             * 
             * A: Length 5
             *    0    1    2    3    4
             * A: 0000 0000 0000 0000 0000
             * 
             * B: Length 3
             *       0    1    2    3    4 [proteinInterfacePartIndex]
             * B: 0: 0000 0000 0000 ____ ____ [shorterPartOffset = 0;] 3: proteinInterfacePartIndex>shorterProteinInterfaceLength+shorterPartOffset 
             * B: 1: ____ 0000 0000 0000 ____ [shorterPartOffset = 1;] 1: proteinInterfacePartIndex<shorterPartOffset
             * B: 2: ____ ____ 0000 0000 0000 [shorterPartOffset = 2;] 
             *    []         
             * 
             * LEN(A) - LEN(B) = 2
             * 
             * 1. Loop through every proteinInterface part
             * 2. If (part offset > proteinInterface part index OR part offset)
             * 
             */

            /* F 111000
             * F 101010
             *
             * F 111000
             * R 010101
             * 
             * R 000111
             * R 010101
             * 
             * R 000111
             * F 101010
             * 
             */

            optimisticDistance = double.MaxValue;
            //pessimisticDistance = double.MinValue;
            const int directionCount = 1;

            for (var direction = 0; direction < directionCount; direction++)
            {
                for (var shorterPartOffset = 0; shorterPartOffset <= proteinInterfaceLengthDifference; shorterPartOffset++)
                {
                    double optimisticOffsetDistanceResult = 0;
                    //double pessimisticOffsetDistanceResult = 0;

                    for (var proteinInterfacePartIndex = 0; proteinInterfacePartIndex < longerProteinInterfaceLength; proteinInterfacePartIndex++)
                    {
                        double optimisticPartDistanceResult = 0;
                        //double pessimisticPartDistanceResult = 0;

                        if (proteinInterfacePartIndex < shorterPartOffset || proteinInterfacePartIndex >= shorterProteinInterfaceLength + shorterPartOffset)
                        {
                            var longerProteinInterfaceBools = longerProteinInterface.VectorProteinInterfacePartList[proteinInterfacePartIndex].InteractionFlagBools;

                            var distanceForProteinInterface = VectorDistanceValue(vectorDistanceMeasurementValues, longerProteinInterfaceBools, new bool[longerProteinInterfaceBools.Length]);

                            optimisticPartDistanceResult = distanceForProteinInterface;
                            //pessimisticPartDistanceResult = distanceForProteinInterface;

                            var distanceForNonProteinInterface = VectorDistanceValue(vectorDistanceMeasurementValues, longerProteinInterface.VectorProteinInterfacePartList[proteinInterfacePartIndex].InteractionToNonProteinInterface, false);

                            optimisticPartDistanceResult += distanceForNonProteinInterface;
                            //pessimisticPartDistanceResult += distanceForNonProteinInterface;
                        }
                        else
                        {
                            var longerProteinInterfaceBools = longerProteinInterface.VectorProteinInterfacePartList[proteinInterfacePartIndex].InteractionFlagBools;
                            var shorterProteinInterfaceBools = shorterProteinInterface.VectorProteinInterfacePartList[proteinInterfacePartIndex - shorterPartOffset].InteractionFlagBools;

                            var longerProteinInterfaceBoolsCopy = new bool[longerProteinInterfaceBools.Length];
                            Array.Copy(longerProteinInterfaceBools, longerProteinInterfaceBoolsCopy, longerProteinInterfaceBools.Length);

                            var shorterProteinInterfaceBoolsCopy = new bool[shorterProteinInterfaceBools.Length];
                            Array.Copy(shorterProteinInterfaceBools, shorterProteinInterfaceBoolsCopy, shorterProteinInterfaceBools.Length);
                            
                            if (direction == 1)
                            {
                                Array.Reverse(longerProteinInterfaceBoolsCopy);
                            }

                            double optimisticPartDistance;
                            //double pessimisticPartDistance;

                            CustomDistance(longerProteinInterfaceBoolsCopy, shorterProteinInterfaceBoolsCopy, vectorDistanceMeasurementValues, out optimisticPartDistance/*, out pessimisticPartDistance*/);

                            optimisticPartDistanceResult = optimisticPartDistance;
                            //pessimisticPartDistanceResult = pessimisticPartDistance;

                            var isInteractionToNonProteinInterface1 = longerProteinInterface.VectorProteinInterfacePartList[proteinInterfacePartIndex].InteractionToNonProteinInterface;
                            var isInteractionToNonProteinInterface2 = shorterProteinInterface.VectorProteinInterfacePartList[proteinInterfacePartIndex - shorterPartOffset].InteractionToNonProteinInterface;
                            
                            var distanceForNonProteinInterface = VectorDistanceValue(vectorDistanceMeasurementValues, isInteractionToNonProteinInterface1, isInteractionToNonProteinInterface2);

                            optimisticPartDistanceResult += distanceForNonProteinInterface;
                            //pessimisticPartDistanceResult += distanceForNonProteinInterface;
                        }

                        optimisticOffsetDistanceResult += optimisticPartDistanceResult;
                        //pessimisticOffsetDistanceResult += pessimisticPartDistanceResult;
                    }

                    if (optimisticOffsetDistanceResult < optimisticDistance)
                    {
                        optimisticDistance = optimisticOffsetDistanceResult;
                    }

                    //if (pessimisticOffsetDistanceResult > pessimisticDistance)
                    //{
                    //    pessimisticDistance = pessimisticOffsetDistanceResult;
                    //}
                }
            }
        }

        public static int MaxProteinInterfaceLength(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));

            return vectorProteinInterfaceWholeList.Select(a => a.ProteinInterfaceLength).Max();
        }

        /*
        private static int[] LastPdbChainResidueIndexes(string pdbFilename)
        {
            //var result = new Dictionary<string,int>();
            var pdbFile = new ProteinDataBankFile(pdbFilename, new []{ ATOM_Record.ATOM_Field.FieldName });

            //var x = ProteinDataBankFileOperations.PdbAtomAcidList();

            var atomList = pdbFile.ProteinDataBankFileRecordList.Where(a => a.GetType() == typeof (ATOM_Record)).Select(a=>(ATOM_Record)a).ToList();

            var chainIdList = atomList.Select(a=>a.chainID.FieldValue.ToUpperInvariant()).Distinct().ToList();

            var result = new int[chainIdList.Count];

            for (int index = 0; index < chainIdList.Count; index++)
            {
                var chainId = chainIdList[index];
                var maxResidueIndex = atomList.Where(a => a.chainID.FieldValue.ToUpperInvariant() == chainId).Select(a => int.Parse(a.resSeq.FieldValue)).Max();

                result[index] = maxResidueIndex;
            }

            return result;
        }
        */

        private static VectorProteinInterfaceWhole MakeVectorProteinInterfaceWhole(string pdbFilename, ProteinInterfaceSequenceAndPositionData proteinInterfaceSequenceAndPositionData, bool reversedSequence, bool reversedInteractions)
        {
            if (pdbFilename == null) throw new ArgumentNullException(nameof(pdbFilename));
            if (proteinInterfaceSequenceAndPositionData == null) throw new ArgumentNullException(nameof(proteinInterfaceSequenceAndPositionData));

            ProteinInterfaceAminoAcidMetaData[] proteinInterfaceAminoAcidMetaDataArray = proteinInterfaceSequenceAndPositionData.AminoAcidSequenceAllResidueSequenceIndexes;
            
            var vectorProteinInterfaceWhole = new VectorProteinInterfaceWhole
            {
                FullProteinInterfaceId = new FullProteinInterfaceId(proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId),
                ProteinInterfaceLength = proteinInterfaceSequenceAndPositionData.ProteinInterfaceLength,
                FirstResidueSequenceIndex = proteinInterfaceSequenceAndPositionData.StartPosition,
                LastResidueSequenceIndex = proteinInterfaceSequenceAndPositionData.EndPosition,
                ReversedInteractions = reversedInteractions,
                ReversedSequence = reversedSequence,
            };

            //vectorProteinInterfaceWhole.FullSequenceLength = LastPdbChainResidueIndexes(pdbFilename)[vectorProteinInterfaceWhole.FullProteinInterfaceId.ChainId];

            vectorProteinInterfaceWhole.SecondaryStructure = ProteinInterfaceSecondaryStructureLoader.ProteinInterfaceSecondaryStructure(pdbFilename, SpreadsheetFileHandler.AlphabetLetterRollOver(vectorProteinInterfaceWhole.FullProteinInterfaceId.ChainId), vectorProteinInterfaceWhole.FirstResidueSequenceIndex, vectorProteinInterfaceWhole.LastResidueSequenceIndex, vectorProteinInterfaceWhole.ReversedSequence);

            for (int proteinInterfaceAminoAcidMetaDataArrayIndex = 0; proteinInterfaceAminoAcidMetaDataArrayIndex < proteinInterfaceAminoAcidMetaDataArray.Length; proteinInterfaceAminoAcidMetaDataArrayIndex++)
            {
              
                ProteinInterfaceAminoAcidMetaData proteinInterfaceAminoAcidMetaData = proteinInterfaceAminoAcidMetaDataArray[proteinInterfaceAminoAcidMetaDataArrayIndex];

                var vectorProteinInterfacePart = new VectorProteinInterfacePart(proteinInterfaceAminoAcidMetaData.OppoproteinInterfaceInteractions.Length)
                {
                    FullProteinInterfaceId = new FullProteinInterfaceId(proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId), 
                    ResidueId = proteinInterfaceAminoAcidMetaDataArrayIndex, 
                    SourceAminoAcid1L = proteinInterfaceAminoAcidMetaData.ResidueName1L, 
                    SourceAminoAcid3L = proteinInterfaceAminoAcidMetaData.ResidueName3L, 
                    InteractionAminoAcids1L = proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionResidueNames1L, 
                    InteractionNonProteinInterfaceAminoAcids1L = proteinInterfaceAminoAcidMetaData.NonProteinInterfaceInteractionResidueNames1L, 
                    InteractionFlagBools = new bool[proteinInterfaceAminoAcidMetaData.OppoproteinInterfaceInteractions.Length]
                };

                vectorProteinInterfaceWhole.VectorProteinInterfacePartList.Add(vectorProteinInterfacePart);

                Array.Copy(proteinInterfaceAminoAcidMetaData.OppoproteinInterfaceInteractions, vectorProteinInterfacePart.InteractionFlagBools, proteinInterfaceAminoAcidMetaData.OppoproteinInterfaceInteractions.Length);

                if (reversedInteractions)
                {
                    Array.Reverse(vectorProteinInterfacePart.InteractionFlagBools);
                }

                vectorProteinInterfacePart.InteractionToNonProteinInterface = proteinInterfaceAminoAcidMetaData.ProteinInterfaceInteractionType.HasFlag(ProteinInterfaceInteractionType.InteractionWithNonProteinInterface);
            }

            if (vectorProteinInterfaceWhole.ReversedSequence)
            {
                vectorProteinInterfaceWhole.VectorProteinInterfacePartList.Reverse();
            }

            return vectorProteinInterfaceWhole;
        }


        /// <summary>
        ///     This method returns a dictionary entry for each protein id (pdb id), with a list of interaction vectors
        /// </summary>
        /// <returns></returns>
        public static List<VectorProteinInterfaceWhole> LoadProteinInterfaceVectorFromFiles(
            CancellationToken cancellationToken,
            decimal maxAtomInterationDistance,
            decimal minimumProteinInterfaceDensity,
            string[] sequenceListFileArray,
            string[] pdbFileDirectoryLocationArray,
            ProgressActionSet progressActionSet)
        {
            if (sequenceListFileArray == null) throw new ArgumentNullException(nameof(sequenceListFileArray));
            if (pdbFileDirectoryLocationArray == null) throw new ArgumentNullException(nameof(pdbFileDirectoryLocationArray));

            var vectorProteinInterfaceWholeList = new List<VectorProteinInterfaceWhole>();

            // 1: Open list of sequences already cleaned to have only symmetrical homodimers (fasta file only contains 100% symmetrical homodimers with all other junk removed - but could have any number of proteinInterfaces per chain)
            List<ISequence> sequenceList = SequenceFileHandler.LoadSequenceFileList(sequenceListFileArray, StaticValues.MolNameProteinAcceptedValues);

            var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequenceList);

            // 2: Get a list of the unique ids for the sequences
            List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceListFileArray), "Error loading PDB ID list");
            }

            // 3: Get a list of PDB files found in user specified directory
            string[] pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFileDirectoryLocationArray);



            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);
            


            var startTicks = DateTime.Now.Ticks;

            // 4: Loop through each pdb file
            for (int pdbFileNumber = 0; pdbFileNumber < pdbFilesArray.Length; pdbFileNumber++) // +1 is for progress update
            {
                ProgressActionSet.ProgressAction(1, progressActionSet);

                ProgressActionSet.EstimatedTimeRemainingAction(startTicks, pdbFileNumber + 1, pdbFilesArray.Length, progressActionSet);

                // get unique id of pdb file
                string pdbFilename = pdbFilesArray[pdbFileNumber];
                string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                // check pdb unique id was in the loaded sequence list
                if (!pdbIdList.Contains(proteinId))
                {
                    continue;
                }

                ClusterProteinDataBankFileResult clusterPdbFileResult = Clustering.ClusterProteinDataBankFile(cancellationToken, maxAtomInterationDistance, minimumProteinInterfaceDensity, pdbFilename, pdbIdChainIdList, ClusteringMethodOptions.ClusterWithResidueSequenceIndex, -1, -1, progressActionSet);

                if (clusterPdbFileResult == null)
                {
                    continue;
                }

                List<ProteinInterfaceSequenceAndPositionData> proteinInterfaceSequenceAndPositionDataList = clusterPdbFileResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList;
                proteinInterfaceSequenceAndPositionDataList = proteinInterfaceSequenceAndPositionDataList.OrderBy(a => a.FullProteinInterfaceId.ProteinId).ThenBy(a => a.FullProteinInterfaceId.ChainId).ThenBy(a => a.FullProteinInterfaceId.ProteinInterfaceId).ToList();

                for (int proteinInterfaceSequenceAndPositionDataListIndex = 0; proteinInterfaceSequenceAndPositionDataListIndex < proteinInterfaceSequenceAndPositionDataList.Count; proteinInterfaceSequenceAndPositionDataListIndex++)
                {
                    ProteinInterfaceSequenceAndPositionData proteinInterfaceSequenceAndPositionData = proteinInterfaceSequenceAndPositionDataList[proteinInterfaceSequenceAndPositionDataListIndex];

                    var seq = sequenceList.FirstOrDefault(a =>
                    {
                        var p = SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID);
                        return (p.PdbId.ToUpperInvariant() == proteinInterfaceSequenceAndPositionData.FullProteinInterfaceId.ProteinId.ToUpperInvariant() && p.ChainId.ToUpperInvariant() == proteinInterfaceSequenceAndPositionData.ChainIdLetter.ToUpperInvariant());
                    });

                    var seqLen = seq != null ? seq.Count : -1;
                    
                    var vectorProteinInterfaceWholeFwd = MakeVectorProteinInterfaceWhole(pdbFilename, proteinInterfaceSequenceAndPositionData, false, false);
                    vectorProteinInterfaceWholeFwd.FullSequenceLength = seqLen;

                    vectorProteinInterfaceWholeList.Add(vectorProteinInterfaceWholeFwd);

                    var vectorProteinInterfaceWholeRev = MakeVectorProteinInterfaceWhole(pdbFilename, proteinInterfaceSequenceAndPositionData, true, false);
                    vectorProteinInterfaceWholeRev.FullSequenceLength = seqLen;

                    vectorProteinInterfaceWholeList.Add(vectorProteinInterfaceWholeRev);                        
                }
            }

            ProgressActionSet.FinishAction(true, progressActionSet);

            vectorProteinInterfaceWholeList = vectorProteinInterfaceWholeList.OrderBy(a => a.FullProteinInterfaceId.ProteinId).ThenBy(a => a.FullProteinInterfaceId.ChainId).ThenBy(a => a.FullProteinInterfaceId.ProteinInterfaceId).ToList();

            return vectorProteinInterfaceWholeList;
        }

        public static string[] SaveVectorDistanceMatrixSpreadsheet(string saveFilename, List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, double[,] vectorDistanceMatrix)
        {
            if (saveFilename == null) throw new ArgumentNullException(nameof(saveFilename));
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (vectorDistanceMatrix == null) throw new ArgumentNullException(nameof(vectorDistanceMatrix));

            var spreadsheet = new SpreadsheetCell[vectorDistanceMatrix.GetLength(0) + 1, vectorDistanceMatrix.GetLength(1) + 1];

            for (var indexX = 0; indexX < vectorDistanceMatrix.GetLength(0); indexX++)
            {
                spreadsheet[0, indexX + 1] = new SpreadsheetCell(vectorProteinInterfaceWholeList[indexX].FullProteinInterfaceId.ToString());
            }

            spreadsheet[0, 0] = new SpreadsheetCell("");

            for (var indexY = 0; indexY < vectorDistanceMatrix.GetLength(1); indexY++)
            {
                spreadsheet[indexY + 1, 0] = new SpreadsheetCell(vectorProteinInterfaceWholeList[indexY].FullProteinInterfaceId.ToString());
            }

            for (var indexX = 0; indexX < vectorDistanceMatrix.GetLength(0); indexX++)
            {
                for (var indexY = 0; indexY < vectorDistanceMatrix.GetLength(1); indexY++)
                {
                    spreadsheet[indexX + 1, indexY + 1] = new SpreadsheetCell(vectorDistanceMatrix[indexX, indexY]);
                }
            }

            var savedFiles = SpreadsheetFileHandler.SaveSpreadsheet(saveFilename, null, spreadsheet);

            return savedFiles;
        }

        public static void CustomDistance(bool[] array1, bool[] array2, VectorDistanceMeasurementValues vectorDistanceMeasurementValues, out double optimisticDistance/*, out double pessimisticDistance*/, int offsetStep = 1)
        {
            if (array1 == null) throw new ArgumentNullException(nameof(array1));
            if (array2 == null) throw new ArgumentNullException(nameof(array2));
            if (vectorDistanceMeasurementValues == null) throw new ArgumentNullException(nameof(vectorDistanceMeasurementValues));

            var lengthDifference = Math.Abs(array1.Length - array2.Length);
            optimisticDistance = double.MaxValue;
            //pessimisticDistance = double.MinValue;

            for (var index = 0; index <= lengthDifference; index += offsetStep)
            {
                double neutralOffsetDistance = CustomDistanceOffset(array1, array2, vectorDistanceMeasurementValues, index);

                if (neutralOffsetDistance < optimisticDistance)
                {
                    optimisticDistance = neutralOffsetDistance;
                }

                //if (neutralOffsetDistance > pessimisticDistance)
                //{
                //    pessimisticDistance = neutralOffsetDistance;
                //}
            }
        }

        public static double CustomDistanceOffset(bool[] array1, bool[] array2, VectorDistanceMeasurementValues vectorDistanceMeasurementValues, int offset)
        {
            if (array1 == null) throw new ArgumentNullException(nameof(array1));
            if (array2 == null) throw new ArgumentNullException(nameof(array2));
            if (vectorDistanceMeasurementValues == null) throw new ArgumentNullException(nameof(vectorDistanceMeasurementValues));

            if (offset > array1.Length && offset > array2.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (array1 == null || array1.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(array1));
            }

            if (array2 == null || array2.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(array2));
            }

            bool[] shorterArray;
            bool[] longerArray;

            if (array1.Length > array2.Length)
            {
                if (offset + array2.Length > array1.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset));
                }

                longerArray = array1;
                shorterArray = new bool[longerArray.Length];
                Array.Copy(array2, 0, shorterArray, offset, array2.Length);
            }
            else if (array2.Length > array1.Length)
            {
                if (offset + array1.Length > array2.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset));
                }

                longerArray = array2;
                shorterArray = new bool[longerArray.Length];
                Array.Copy(array1, 0, shorterArray, offset, array1.Length);
            }
            else
            {
                shorterArray = array1;
                longerArray = array2;
            }

            var distance = VectorDistanceValue(vectorDistanceMeasurementValues, shorterArray, longerArray);

            return distance;
        }

        public static double VectorDistanceValue(VectorDistanceMeasurementValues vectorDistanceMeasurementValues, bool[] isInteraction1, bool[] isInteraction2)
        {
            if (vectorDistanceMeasurementValues == null) throw new ArgumentNullException(nameof(vectorDistanceMeasurementValues));
            if (isInteraction1 == null) throw new ArgumentNullException(nameof(isInteraction1));
            if (isInteraction2 == null) throw new ArgumentNullException(nameof(isInteraction2));

            if (isInteraction1.Length != isInteraction2.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(isInteraction1), "Interaction vectors must be the same size");    
            }

            double distance = 0;

            for (var index = 0; index < isInteraction1.Length; index++)
            {
                distance += VectorDistanceValue(vectorDistanceMeasurementValues, isInteraction1[index], isInteraction2[index]);
            }

            return distance;
        }

        public static double VectorDistanceValue(VectorDistanceMeasurementValues vectorDistanceMeasurementValues, bool isInteraction1, bool isInteraction2)
        {
            if (vectorDistanceMeasurementValues == null) throw new ArgumentNullException(nameof(vectorDistanceMeasurementValues));

            if (isInteraction1 && isInteraction2)
            {
                return vectorDistanceMeasurementValues.InteractionAndInteraction;
            }
            
            if (!isInteraction1 && !isInteraction2)
            {
                return vectorDistanceMeasurementValues.NonInteractionAndNonInteraction;
            }
            
            return vectorDistanceMeasurementValues.InteractionAndNonInteraction;
        }
    }
}