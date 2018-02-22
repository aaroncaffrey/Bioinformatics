//-----------------------------------------------------------------------
// <copyright file="FilterProteins.cs" company="Aaron Caffrey">
//     Copyright (c) Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

//using System.Runtime.InteropServices;
//using DocumentFormat.OpenXml.EMMA;
//using DocumentFormat.OpenXml.Spreadsheet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.Extensions;
using BioinformaticsHelperLibrary.InteractionDetection;
using BioinformaticsHelperLibrary.TaskManagement;
using BioinformaticsHelperLibrary.UserProteinInterface;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class FilterProteins
    {
        /// <summary>
        ///     Options for the RemoveSequences method.  Either to remove the specified ids, or to keep the specified ids and
        ///     remove any others.
        /// </summary>
        public enum RemoveSequencesOptions
        {
            /// <summary>
            ///     Remove sequences in list.
            /// </summary>
            RemoveSequencesInList,

            /// <summary>
            ///     Remove sequences not in list.
            /// </summary>
            RemoveSequencesNotInList
        }

        /// <summary>
        ///     This method converts a sequence list into a distinct PDB ID list.
        /// </summary>
        /// <param name="sequenceList"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static List<string> SequenceListToPdbIdList(List<ISequence> sequenceList, bool distinct = true)
        {
            if (sequenceList == null)// || sequenceList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceList));
            }

            if (distinct)
            {
                return sequenceList.Select(a => SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID).PdbId).Distinct().ToList();
            }
            else
            {
                return sequenceList.Select(a => SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID).PdbId).ToList();
            }
        }

        /// <summary>
        ///     This method removes each PDB ID which does not contain chemical interactions.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFolders"></param>
        /// <param name="pdbIdList"></param>
        /// <param name="uiProgramStatusControlSet"></param>
        /// <returns></returns>
        public static List<string> RemoveSequencesWithoutInteractions(CancellationToken cancellationToken, decimal maxAtomInterationDistance, string[] pdbFolders, List<string> pdbIdList = null, Dictionary<string, List<string>> pdbIdChainIdList = null, ProgressActionSet progressActionSet = null, int totalThreads = -1)
        {
            if (pdbFolders == null || pdbFolders.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFolders));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFolders));

            ProteinDataBankFileOperations.ShowMissingPdbFiles(pdbFilesArray, pdbIdList, progressActionSet);

            var workDivision = new WorkDivision<List<string>>(pdbFilesArray.Length, totalThreads);

            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                Task<List<string>> task = Task.Run(() =>
                {
                    var taskResult = new List<string>();

                    for (int pdbFileNumber = workDivision.ThreadFirstIndex[localThreadIndex]; pdbFileNumber <= workDivision.ThreadLastIndex[localThreadIndex]; pdbFileNumber++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        try
                        {
                            string pdbFilename = pdbFilesArray[pdbFileNumber];
                            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                            // Check if the file found is included in the white list.
                            if (pdbIdList != null && !pdbIdList.Contains(proteinId))
                            {
                                continue;
                            }

                            List<AtomPair> interactions = SearchInteractions.FindInteractions(cancellationToken, maxAtomInterationDistance, pdbFilename, pdbIdChainIdList, true);

                            if (interactions == null || interactions.Count == 0) continue;

                            if (!taskResult.Contains(proteinId))
                            {
                                taskResult.Add(proteinId);
                            }
                        }
                        finally
                        {
                            workDivision.IncrementItemsCompleted(1);
                            ProgressActionSet.ProgressAction(1, progressActionSet);
                            ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                        }
                    }

                    return taskResult;
                });

                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            var result = new List<string>();

            foreach (var task in workDivision.TaskList.Where(t => t != null && t.Result != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted))
            {
                result.AddRange(task.Result);
            }

            result = result.Distinct().ToList();

            return result;
        }

        /// <summary>
        ///     Generate stats of interactions... also removes proteins not meeting minimum interactions requirement.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFolders"></param>
        /// <param name="pdbIdList"></param>
        /// <param name="progressBar"></param>
        /// <param name="estimatedTimeRemainingLabel"></param>
        /// <returns></returns>
        public static Dictionary<string, decimal> CalculateStructureSymmetry(CancellationToken cancellationToken, decimal maxAtomInterationDistance, string[] pdbFolders, List<string> pdbIdList = null, Dictionary<string, List<string>> pdbIdChainIdList = null, ProgressActionSet progressActionSet = null, int totalThreads = -1)
        {
            if (pdbFolders == null || pdbFolders.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFolders));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentOutOfRangeException(nameof(progressActionSet));
            }

            const int requiredNumberOfChains = 2;

            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFolders));

            WorkDivision<Dictionary<string, decimal>> workDivision = new WorkDivision<Dictionary<string, decimal>>(pdbFilesArray.Length, totalThreads);

            ProteinDataBankFileOperations.ShowMissingPdbFiles(pdbFilesArray, pdbIdList, progressActionSet);

            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                Task<Dictionary<string, decimal>> task = Task.Run(() =>
                {
                    var taskResult = new Dictionary<string, decimal>();

                    for (int pdbFileNumber = workDivision.ThreadFirstIndex[localThreadIndex]; pdbFileNumber <= workDivision.ThreadLastIndex[localThreadIndex]; pdbFileNumber++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        try
                        {
                            string pdbFilename = pdbFilesArray[pdbFileNumber];
                            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                            // Check if the file found is included in the white list.
                            if (pdbIdList != null && !pdbIdList.Contains(proteinId))
                            {
                                continue;
                            }

                            var chainIdList = pdbIdChainIdList != null ? (proteinId != null && pdbIdChainIdList.ContainsKey(proteinId) ? pdbIdChainIdList[proteinId].ToArray() : null) : null;

                            // Get atom chains.
                            ProteinChainListContainer proteinFileChains = ProteinDataBankFileOperations.PdbAtomicChains(pdbFilename, chainIdList, requiredNumberOfChains, requiredNumberOfChains, true);

                            if (proteinFileChains == null || proteinFileChains.ChainList == null || proteinFileChains.ChainList.Count != 2 ||
                                proteinFileChains.ChainList[StaticValues.ChainA] == null || proteinFileChains.ChainList[StaticValues.ChainA].AtomList == null || proteinFileChains.ChainList[StaticValues.ChainA].AtomList.Count == 0 ||
                                proteinFileChains.ChainList[StaticValues.ChainB] == null || proteinFileChains.ChainList[StaticValues.ChainB].AtomList == null || proteinFileChains.ChainList[StaticValues.ChainB].AtomList.Count == 0)
                            {
                                continue;
                            }

                            // Make a list to save interactions found.
                            var interactionMatchPercentage = new InteractionMatchPercentage(proteinId);

                            List<AtomPair> interactions = SearchInteractions.FindInteractions(cancellationToken, maxAtomInterationDistance, pdbFilename, pdbIdChainIdList);

                            interactionMatchPercentage.IncrementTotalInteractions(interactions.Count);

                            for (int interactionsIndex = 0; interactionsIndex < interactions.Count; interactionsIndex++)
                            {
                                interactionMatchPercentage.AddResidueSequenceIndex(StaticValues.ChainA, interactions[interactionsIndex].Atom1.resSeq.FieldValue);
                                interactionMatchPercentage.AddResidueSequenceIndex(StaticValues.ChainB, interactions[interactionsIndex].Atom2.resSeq.FieldValue);
                            }

                            InteractionMatchPercentage.CalculatePercentageResult calculatedPercentage = interactionMatchPercentage.CalculatePercentage();

                            taskResult.Add(interactionMatchPercentage.ProteinId, calculatedPercentage.InteractionMatchPercentageAverage);
                        }
                        finally
                        {
                            workDivision.IncrementItemsCompleted(1);

                            ProgressActionSet.ProgressAction(1, progressActionSet);
                            ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                        }
                    }

                    return taskResult;
                }, cancellationToken);

                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            var result = new Dictionary<string, decimal>();

            foreach (var task in workDivision.TaskList.Where(t => t != null && t.Result != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted))
            {
                foreach (var kvp in task.Result)
                {
                    //if (result.ContainsKey(kvp.Key))
                    //{
                    //    Console.WriteLine("Key already exists: '" + kvp.Key + "'");
                    //}
                    result.Add(kvp.Key, kvp.Value);
                }
            }

            return result;
        }

        /// <summary>
        ///     This method removes sequences not having the required number of chains.
        /// </summary>
        /// <returns></returns>
        public static List<ISequence> RemoveSequencesWithIncorrectNumberOfChains(CancellationToken cancellationToken, List<ISequence> sequenceList, int numberOfChainsRequired = 2, ProgressActionSet progressActionSet = null)
        {
            if (sequenceList == null || sequenceList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceList));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            var pdbIdListNotDistinct = FilterProteins.SequenceListToPdbIdList(sequenceList, false);

            ProgressActionSet.StartAction(pdbIdListNotDistinct.Count, progressActionSet);

            var workDivision = new WorkDivision<List<string>>(pdbIdListNotDistinct.Count);

            for (var threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                var localThreadIndex = threadIndex;
                Task<List<string>> task = Task.Run(() =>
                {
                    var taskResult = pdbIdListNotDistinct.Where((a, pdbIdIndex) =>
                    {
                        if (pdbIdIndex < workDivision.ThreadFirstIndex[localThreadIndex] || pdbIdIndex > workDivision.ThreadLastIndex[localThreadIndex]) return false;

                        workDivision.IncrementItemsCompleted(1);
                        ProgressActionSet.ProgressAction(1, progressActionSet);
                        ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);

                        return pdbIdListNotDistinct.Count(b => a == b) != numberOfChainsRequired;
                    }).ToList();

                    return taskResult;
                }, cancellationToken);

                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            var sequencesWithIncorrectNumberOfChains = new List<string>();

            foreach (var task in workDivision.TaskList.Where(t => t != null && t.Result != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted))
            {
                sequencesWithIncorrectNumberOfChains.AddRange(task.Result);
            }

            var result = RemoveSequences(cancellationToken, sequenceList, sequencesWithIncorrectNumberOfChains);

            ProgressActionSet.FinishAction(true, progressActionSet);

            return result;
        }

        public class DimerSequenceTypeCategories<T>
        {
            public List<T> HeteroDimerPdbIdList = new List<T>();
            public List<T> HomoDimerPdbIdList = new List<T>();
            public List<T> HomologyDimerPdbIdList = new List<T>();
            //public List<string> NonDimerPdbIdList = new List<string>();
        }

        public enum DimerType
        {
            HeteroDimer,
            HomologyDimer,
            HomoDimer
        }

        public static DimerType FindDimerType(ISequence alignableSequenceA, ISequence alignableSequenceB, decimal minimumHeterodimerSimilarityRequired = 30.0m, decimal minimumHomodimerSimiliarityRequired = 90.0m)
        {
            var sequenceSimilarity = AlignedSequenceSimilarityPercentage(alignableSequenceA, alignableSequenceB);

            //var sequenceSimilarity = 0m;

            //if (sequenceSimilarity < minimumHomodimerSimiliarityRequired)
            //{

            //}

            //if (sequenceSimilarity < minimumHomodimerSimiliarityRequired)
            //{
            //    sequenceSimilarity = AlignedSequenceSimilarityPercentage(alignableSequenceA, alignableSequenceB.GetReversedSequence());
            //}

            if (sequenceSimilarity <= minimumHeterodimerSimilarityRequired)
            {
                return DimerType.HeteroDimer;
            }
            else if (sequenceSimilarity >= minimumHomodimerSimiliarityRequired)
            {
                return DimerType.HomoDimer;
            }
            else
            {
                return DimerType.HomologyDimer;
            }

        }

        /// <summary>
        ///     Perform sequence alignment on the chains of each protein to see if it is a homodimer or heterodimer
        /// </summary>
        /// <returns></returns>
        public static DimerSequenceTypeCategories<string> SplitDimersHomoHetero(CancellationToken cancellationToken, List<ISequence> sequences, decimal minimumHeterodimerSimilarityRequired = 30.0m, decimal minimumHomodimerSimiliarityRequired = 90.0m, ProgressActionSet progressActionSet = null, int totalThreads = -1)
        {
            if (sequences == null || sequences.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequences));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            var workDivision = new WorkDivision<DimerSequenceTypeCategories<string>>(sequences.Count, totalThreads);

            ProgressActionSet.StartAction(sequences.Count, progressActionSet);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                Task<DimerSequenceTypeCategories<string>> task = Task.Run(() =>
                {
                    var taskResult = new DimerSequenceTypeCategories<string>();

                    for (int sequencesIndexA = workDivision.ThreadFirstIndex[localThreadIndex]; sequencesIndexA <= workDivision.ThreadLastIndex[localThreadIndex]; sequencesIndexA++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        string proteinIdA = SequenceIdSplit.SequenceIdToPdbIdAndChainId(sequences[sequencesIndexA].ID).PdbId;

                        for (int sequencesIndexB = 0; sequencesIndexB < sequences.Count; sequencesIndexB++)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            // Don't align the same sequence index. Skip calculating indexes already calculated. Perform alignment operation if protein id is the same.
                            var proteinIdB = SequenceIdSplit.SequenceIdToPdbIdAndChainId(sequences[sequencesIndexB].ID).PdbId;

                            if (sequencesIndexA == sequencesIndexB || sequencesIndexB < sequencesIndexA || proteinIdA != proteinIdB)
                            {
                                continue;
                            }

                            var dimerType = FindDimerType(sequences[sequencesIndexA], sequences[sequencesIndexB], minimumHeterodimerSimilarityRequired, minimumHomodimerSimiliarityRequired);

                            if (dimerType == DimerType.HeteroDimer)
                            {
                                taskResult.HeteroDimerPdbIdList.Add(proteinIdA);
                            }
                            else if (dimerType == DimerType.HomoDimer)
                            {
                                taskResult.HomoDimerPdbIdList.Add(proteinIdA);
                            }
                            else if (dimerType == DimerType.HomologyDimer)
                            {
                                taskResult.HomologyDimerPdbIdList.Add(proteinIdA);
                            }
                        }

                        workDivision.IncrementItemsCompleted(1);
                        ProgressActionSet.ProgressAction(1, progressActionSet);
                        ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                    }

                    return taskResult;
                }, cancellationToken);

                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            var dimerSequenceTypeCategories = new DimerSequenceTypeCategories<string>();

            foreach (var task in workDivision.TaskList.Where(t => t != null && t.IsCompleted && !t.IsFaulted && !t.IsCanceled && t.Result != null))
            {
                dimerSequenceTypeCategories.HeteroDimerPdbIdList.AddRange(task.Result.HeteroDimerPdbIdList);
                dimerSequenceTypeCategories.HomoDimerPdbIdList.AddRange(task.Result.HomoDimerPdbIdList);
                dimerSequenceTypeCategories.HomologyDimerPdbIdList.AddRange(task.Result.HomologyDimerPdbIdList);
            }

            dimerSequenceTypeCategories.HeteroDimerPdbIdList = dimerSequenceTypeCategories.HeteroDimerPdbIdList.Distinct().ToList();
            dimerSequenceTypeCategories.HomoDimerPdbIdList = dimerSequenceTypeCategories.HomoDimerPdbIdList.Distinct().ToList();
            dimerSequenceTypeCategories.HomologyDimerPdbIdList = dimerSequenceTypeCategories.HomologyDimerPdbIdList.Distinct().ToList();


            return dimerSequenceTypeCategories;
        }


        /// <summary>
        ///     Perform sequence alignment on the chains of each protein to see if it is a homodimer.
        /// </summary>
        /// <returns></returns>
        public static DimerSequenceTypeCategories<ISequence> SplitDimerTypes(CancellationToken cancellationToken, List<ISequence> sequences, decimal minimumHeterodimerSimilarityRequired = 30.0m, decimal minimumHomodimerSimiliarityRequired = 90.0m, ProgressActionSet progressActionSet = null, int totalThreads = -1)
        {
            if (sequences == null || sequences.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequences));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            var splitDimers = SplitDimersHomoHetero(cancellationToken, sequences, minimumHeterodimerSimilarityRequired, minimumHomodimerSimiliarityRequired, progressActionSet, totalThreads);

            var heterodimers = new List<ISequence>(sequences);
            var homodimers = new List<ISequence>(sequences);
            var homologydimers = new List<ISequence>(sequences);

            heterodimers = RemoveSequences(cancellationToken, heterodimers, splitDimers.HeteroDimerPdbIdList, RemoveSequencesOptions.RemoveSequencesNotInList);
            homodimers = RemoveSequences(cancellationToken, homodimers, splitDimers.HomoDimerPdbIdList, RemoveSequencesOptions.RemoveSequencesNotInList);
            homologydimers = RemoveSequences(cancellationToken, homologydimers, splitDimers.HomologyDimerPdbIdList, RemoveSequencesOptions.RemoveSequencesNotInList);

            var result = new DimerSequenceTypeCategories<ISequence>()
            {
                HomoDimerPdbIdList = homodimers,
                HeteroDimerPdbIdList = heterodimers,
                HomologyDimerPdbIdList = homologydimers,
            };

            return result;
        }

        public static decimal AlignedSequenceSimilarityPercentage(ISequence alignableSequenceA, ISequence alignableSequenceB)
        {
            if (alignableSequenceA == null || alignableSequenceA.Count == 0 || alignableSequenceB == null || alignableSequenceB.Count == 0)
            {
                return 0;
            }

            var firstSequence = alignableSequenceA;
            var secondSequence = alignableSequenceB;

            var needlemanWunschAligner = new NeedlemanWunschAligner();
            try
            {
                /*
                for (var index = 0; index < alignableSequenceA.Count; index++)
                {
                if (alignableSequenceA[index] == Convert.ToByte('X'))
                {
                alignableSequenceA[index] = Convert.ToByte('L');

                }
                }
                */
                if (!alignableSequenceA.Contains(Convert.ToByte('X')) && !alignableSequenceB.Contains(Convert.ToByte('X')))
                {

                    IList<IPairwiseSequenceAlignment> alignedList = needlemanWunschAligner.AlignSimple(alignableSequenceA, alignableSequenceB);

                    if (alignedList != null && alignedList.Count > 0 && alignedList[0].PairwiseAlignedSequences.Count > 0)
                    {
                        firstSequence = alignedList[0].PairwiseAlignedSequences[0].FirstSequence;
                        secondSequence = alignedList[0].PairwiseAlignedSequences[0].SecondSequence;
                    }
                }
            }
            catch (KeyNotFoundException)
            {
            }
            catch (ArgumentException)
            {
            }

            return SequenceSimilarityPercentage(firstSequence, secondSequence);
        }

        public static decimal SequenceSimilarityPercentage(ISequence alignedSequenceA, ISequence alignedSequenceB)
        {
            if (alignedSequenceA == null || alignedSequenceA.Count == 0 || alignedSequenceB == null || alignedSequenceB.Count == 0)
            {
                return 0;
            }

            if (alignedSequenceA.SequenceEqual(alignedSequenceB))
            {
                return 100.00m;
            }

            int sequenceEquality = 0;
            var sequenceLengthMin = alignedSequenceA.Count < alignedSequenceB.Count ? alignedSequenceA.Count : alignedSequenceB.Count;
            var sequenceLengthMax = alignedSequenceA.Count > alignedSequenceB.Count ? alignedSequenceA.Count : alignedSequenceB.Count;

            var groups = AminoAcidGroups.AminoAcidGroups.GetSubgroupAminoAcidsCodesStrings(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb);

            for (var proteinSequenceIndex = 0; proteinSequenceIndex < sequenceLengthMin; proteinSequenceIndex++)
            {
                if (alignedSequenceA[proteinSequenceIndex] == alignedSequenceB[proteinSequenceIndex])
                {
                    sequenceEquality++;
                    continue;
                }

                var aminoAcidA = Convert.ToChar(alignedSequenceA[proteinSequenceIndex]);
                var aminoAcidB = Convert.ToChar(alignedSequenceB[proteinSequenceIndex]);

                if (groups.Any(t => t.Contains(aminoAcidA) && t.Contains(aminoAcidB)))
                {
                    sequenceEquality++;
                }
            }

            if (sequenceEquality == 0)
            {
                return 0;
            }

            return Math.Round(((decimal)sequenceEquality / (decimal)sequenceLengthMax) * 100.0m, 0);
        }

        public static List<ISequence> RemoveDuplicates(CancellationToken cancellationToken, List<ISequence> sequences, ProgressActionSet progressActionSet, int totalThreads = -1)
        {
            if (sequences == null || sequences.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequences));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            var pdbIdList = SequenceListToPdbIdList(sequences);
            var pdbIdSequences = pdbIdList.Select(a => sequences.Where(b => SequenceIdSplit.SequenceIdToPdbIdAndChainId(b.ID).PdbId == a).ToList()).ToList();

            var workDivision = new WorkDivision(pdbIdList.Count, totalThreads);


            ProgressActionSet.StartAction(pdbIdList.Count, progressActionSet);

            var done = new List<ISequence>();
            var remove = new List<ISequence>();
            var removeLock = new object();



            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                var task = Task.Run(() =>
                {
                    for (int index = workDivision.ThreadFirstIndex[localThreadIndex]; index <= workDivision.ThreadLastIndex[localThreadIndex]; index++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        var iterationPdbId = pdbIdList[index];
                        var iterationPdbIdSeqs = pdbIdSequences[index];// sequences.Where(a => SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID).PdbId == pdbId).ToList();

                        //var seq = sequences[index];
                        //var seqid = SequenceIdSplit.SequenceIdToPdbIdAndChainId(seq.ID).PdbId.ToUpperInvariant();
                        lock (done)
                        {
                            if (iterationPdbIdSeqs.All(done.Contains)) continue;
                        }

                        foreach (var pdbIdSeqSet in pdbIdSequences)
                        {
                            if (pdbIdSeqSet == iterationPdbIdSeqs) continue;

                            foreach (var pdbIdSeq in pdbIdSeqSet)
                            {
                                foreach (var iterationPdbIdSeq in iterationPdbIdSeqs)
                                {

                                }
                            }

                        }

                        // find sequences equal to the current iteration item
                        //var equalseq = sequences.Where(a => a.SequenceEqual(seq)).ToList();


                        /*
                        var equalseq = sequences.Where(a => AlignedSequenceSimilarityPercentage(seq,a) >= 90).ToList();

                        

                        // get a list of pdbids, ordered alphabetically
                        var equalseqids = equalseq.Select(p => SequenceIdSplit.SequenceIdToPdbIdAndChainId(p.ID).PdbId.ToUpperInvariant()).OrderBy(p => p).ToList();
                        
                        // one or more of the chains might have a difference sequence and so not in the list, update by the ids in the list
                        //equalseq = sequences.Where(p => equalseqids.Contains(SequenceIdSplit.SequenceIdToPdbIdAndChainId(p.ID).PdbId.ToUpperInvariant())).ToList();

                        // add this iteration item and all others with the same sequence to a list to skip in future
                        lock (done)
                        {
                            done.AddRange(equalseq);
                        }

                        // keep the very last item in the list and all with the same pdbid that it has
                        var keepid = equalseqids.Last();
                        var equalseqkeep = equalseq.Where(p => SequenceIdSplit.SequenceIdToPdbIdAndChainId(p.ID).PdbId.ToUpperInvariant() == keepid).ToList();

                        // remove the sequences to keep from the removal list
                        equalseq = equalseq.Where(a => !equalseqkeep.Contains(a)).ToList();
                        
                        lock (remove)
                        {
                            remove.AddRange(equalseq);
                        }
                        */
                        workDivision.IncrementItemsCompleted(1);
                        ProgressActionSet.ProgressAction(1, progressActionSet);
                        ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                    }
                }, cancellationToken);

                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();


            var remove2 = remove.Distinct().ToList();


            return RemoveSequences(cancellationToken, sequences, remove2.Select(p => SequenceIdSplit.SequenceIdToPdbIdAndChainId(p.ID).PdbId.ToUpperInvariant()).ToList());

        }


        /// <summary>
        ///     This method removes specified ids from the list of sequences.
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="sequencesToKeepOrRemove"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static List<ISequence> RemoveSequences(CancellationToken cancellationToken, List<ISequence> sequences, List<string> sequencesToKeepOrRemove, RemoveSequencesOptions options = RemoveSequencesOptions.RemoveSequencesInList, int totalThreads = -1)
        {
            if (sequences == null || sequences.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequences));
            }

            if (sequencesToKeepOrRemove == null)
            {
                throw new ArgumentOutOfRangeException(nameof(sequencesToKeepOrRemove));
            }

            
            if (sequencesToKeepOrRemove != null)// && sequencesToKeepOrRemove.Count > 0)
            {
                var workDivision = new WorkDivision<List<int>>(sequences.Count, totalThreads);

                for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
                {
                    int localThreadIndex = threadIndex;

                    Task<List<int>> task = Task.Run(() =>
                    {
                        var taskResult = new List<int>();

                        for (int sequencesIndex = workDivision.ThreadFirstIndex[localThreadIndex]; sequencesIndex <= workDivision.ThreadLastIndex[localThreadIndex]; sequencesIndex++)
                        {
                            string proteinId = SequenceIdSplit.SequenceIdToPdbIdAndChainId(sequences[sequencesIndex].ID).PdbId;

                            if (((options == RemoveSequencesOptions.RemoveSequencesInList) && (sequencesToKeepOrRemove.Contains(proteinId))) ||
                                ((options == RemoveSequencesOptions.RemoveSequencesNotInList) && (!sequencesToKeepOrRemove.Contains(proteinId))))
                            {
                                taskResult.Add(sequencesIndex);
                            }

                            workDivision.IncrementItemsCompleted(1);
                        }

                        return taskResult;
                    }, cancellationToken);

                    workDivision.TaskList.Add(task);
                }

                workDivision.WaitAllTasks();

                var sequenceIndexesToRemove = new List<int>();

                foreach (var task in workDivision.TaskList.Where(t => t != null && t.Result != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted))
                {
                    sequenceIndexesToRemove.AddRange(task.Result);
                }

                sequenceIndexesToRemove = sequenceIndexesToRemove.Distinct().ToList();

                sequenceIndexesToRemove.Sort();

                for (int sequenceIndexesToRemoveIndex = sequenceIndexesToRemove.Count - 1; sequenceIndexesToRemoveIndex >= 0; sequenceIndexesToRemoveIndex--)
                {
                    sequences.RemoveAt(sequenceIndexesToRemove[sequenceIndexesToRemoveIndex]);
                }
            }

            return sequences;
        }

        /// <summary>
        ///     This method removes sequences from the list which are not proteins (e.g. DNA, RNA, Hybrid).
        /// </summary>
        /// <returns></returns>
        public static List<ISequence> RemoveNonProteinAlphabetSequences(CancellationToken cancellationToken, List<ISequence> sequences, ProgressActionSet progressActionSet, int totalThreads = -1)
        {
            if (sequences == null || sequences.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequences));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            WorkDivision<List<string>> workDivision = new WorkDivision<List<string>>(sequences.Count, totalThreads);

            ProgressActionSet.StartAction(sequences.Count, progressActionSet);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;
                Task<List<string>> task = Task.Run(() =>
                {
                    var taskResult = new List<string>();

                    for (int index = workDivision.ThreadFirstIndex[localThreadIndex]; index <= workDivision.ThreadLastIndex[localThreadIndex]; index++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        string proteinId = SequenceIdSplit.SequenceIdToPdbIdAndChainId(sequences[index].ID).PdbId;

                        if (sequences[index].Alphabet != Alphabets.Protein)
                        {
                            taskResult.Add(proteinId);
                        }

                        workDivision.IncrementItemsCompleted(1);
                        ProgressActionSet.ProgressAction(1, progressActionSet);
                        ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                    }

                    return taskResult;
                }, cancellationToken);

                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            var result = new List<string>();

            foreach (var task in workDivision.TaskList.Where(t => t != null && t.Result != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted))
            {
                result.AddRange(task.Result);
            }

            result = result.Distinct().ToList();

            List<ISequence> seq = RemoveSequences(cancellationToken, sequences, result);

            return seq;
        }


        public static List<string> RemoveMultipleStructureModels(CancellationToken cancellationToken, string[] pdbFolders, List<string> pdbIdList = null, ProgressActionSet progressActionSet = null, int totalThreads = -1)
        {
            if (pdbFolders == null || pdbFolders.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFolders));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFolders));

            WorkDivision<List<string>> workDivision = new WorkDivision<List<string>>(pdbFilesArray.Length, totalThreads);

            ProteinDataBankFileOperations.ShowMissingPdbFiles(pdbFilesArray, pdbIdList, progressActionSet);

            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                Task<List<string>> task = Task.Run(() =>
                {
                    var taskResult = new List<string>();

                    for (int pdbFileNumber = workDivision.ThreadFirstIndex[localThreadIndex]; pdbFileNumber <= workDivision.ThreadLastIndex[localThreadIndex]; pdbFileNumber++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        try
                        {
                            string pdbFilename = pdbFilesArray[pdbFileNumber];
                            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                            // Check if the file found is included in the white list.
                            if (pdbIdList != null && !pdbIdList.Contains(proteinId))
                            {
                                continue;
                            }

                            int modelCount = ProteinDataBankFileOperations.PdbModelCount(pdbFilename, 1);

                            if (modelCount > 1)
                            {
                                if (!taskResult.Contains(proteinId))
                                {
                                    taskResult.Add(proteinId);
                                }
                            }
                        }
                        finally
                        {
                            workDivision.IncrementItemsCompleted(1);
                            ProgressActionSet.ProgressAction(1, progressActionSet);
                            ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                        }
                    }

                    return taskResult;
                }, cancellationToken);
                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            ProgressActionSet.FinishAction(true, progressActionSet);

            var result = new List<string>();

            foreach (var task in workDivision.TaskList.Where(t => t != null && t.Result != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted))
            {
                result.AddRange(task.Result);
            }

            result = result.Distinct().ToList();

            return result;
        }

        public static List<string> RemoveStructuresWithIncorrectNumberOfChains(CancellationToken cancellationToken, string[] pdbFolders, List<string> pdbIdList = null, Dictionary<string, List<string>> pdbIdChainIdList = null, int numberChainsRequired = 2, ProgressActionSet progressActionSet = null, int totalThreads = -1)
        {
            if (pdbFolders == null || pdbFolders.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFolders));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            var pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFolders);

            pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, pdbFilesArray);

            ProteinDataBankFileOperations.ShowMissingPdbFiles(pdbFilesArray, pdbIdList, progressActionSet);

            WorkDivision<List<string>> workDivision = new WorkDivision<List<string>>(pdbFilesArray.Length, totalThreads);

            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                Task<List<string>> task = Task.Run(() =>
                {
                    var taskResult = new List<string>();

                    for (int pdbFileNumber = workDivision.ThreadFirstIndex[localThreadIndex]; pdbFileNumber <= workDivision.ThreadLastIndex[localThreadIndex]; pdbFileNumber++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        try
                        {
                            string pdbFilename = pdbFilesArray[pdbFileNumber];
                            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                            // Check if the file found is included in the white list.
                            if (pdbIdList != null && !pdbIdList.Contains(proteinId))
                            {
                                continue;
                            }

                            var sequenceChainIdList = pdbIdChainIdList != null ? (pdbIdChainIdList.ContainsKey(proteinId) ? pdbIdChainIdList[proteinId].ToArray() : null) : null;
                            int chainCount = ProteinDataBankFileOperations.PdbAtomicChainsCount(pdbFilename, sequenceChainIdList, numberChainsRequired);

                            if (chainCount != numberChainsRequired)
                            {
                                if (!taskResult.Contains(proteinId))
                                {
                                    taskResult.Add(proteinId);
                                }
                            }
                        }
                        finally
                        {
                            workDivision.IncrementItemsCompleted(1);

                            ProgressActionSet.ProgressAction(1, progressActionSet);
                            ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                        }
                    }

                    return taskResult;
                }, cancellationToken);
                workDivision.TaskList.Add(task);
            }

            workDivision.WaitAllTasks();

            ProgressActionSet.FinishAction(true, progressActionSet);

            var result = new List<string>();

            foreach (var task in workDivision.TaskList.Where(t => t != null && t.Result != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted))
            {
                result.AddRange(task.Result);
            }

            result = result.Distinct().ToList();

            return result;
        }
    }
}