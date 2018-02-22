using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Bio;
using BioinformaticsHelperLibrary.UserProteinInterface;

namespace BioinformaticsHelperLibrary.Misc
{
    public class CleanProteinHandler
    {
        /// <summary>
        ///     Protein operation option flags.
        /// </summary>
        [Flags]
        public enum ProteinOperation
        {
            None = 0,
            LoadFile = 1,
            RemoveNonProteinAlphabetInSequence = 2,
            RemoveWrongNumberOfChainsInSequence = 4,
            RemoveExactDuplicatesInSequence = 8,
            RemoveNonHomodimersInSequence = 16,
            RemoveWrongNumberOfChainsInStructure = 32,
            RemoveMultipleModelsInStructure = 64,
            RemoveNonInteractingProteinsInStructure = 128,
            RemoveNonSymmetricalInStructure = 256,
            Finished = 512
        }

        /// <summary>
        ///     Returns a string explaination of a given protein filtering operation for dispaly on the user proteinInterface.
        /// </summary>
        /// <param name="proteinOperation"></param>
        /// <returns></returns>
        public static string ProteinOperationString(ProteinOperation proteinOperation)
        {
            switch (proteinOperation)
            {
                case ProteinOperation.RemoveNonProteinAlphabetInSequence:
                    return "non-protein alphabet (ensure data not contaminated with DNA/RNA/Hybrids)";
                case ProteinOperation.RemoveWrongNumberOfChainsInSequence:
                    return "wrong number of chain in sequence file (only proteins with 2 chains wanted)";
                case ProteinOperation.RemoveExactDuplicatesInSequence:
                    return "exact duplicates (exact duplicates are removed to not distort result set)";
                case ProteinOperation.RemoveNonHomodimersInSequence:
                    return "non-homodimers (sequence alignment is performed to ensure chains are 90% similar)";
                case ProteinOperation.RemoveWrongNumberOfChainsInStructure:
                    return "wrong number of chain in structure file (only proteins with 2 chains wanted)";
                case ProteinOperation.RemoveMultipleModelsInStructure:
                    return "multiple models";
                case ProteinOperation.RemoveNonInteractingProteinsInStructure:
                    return "proteins not containing interactions (no carbon alpha in oppoproteinInterface chains within 5 angstrom)";
                case ProteinOperation.RemoveNonSymmetricalInStructure:
                    return "non symmetrical (interaction sequence index in A/B mismatch)";
                default:
                    throw new ArgumentOutOfRangeException(nameof(proteinOperation));
            }
        }

        /// <summary>
        ///     Filters the given FASTA files and PDB files with the given options and saves the results to disk.  Data needs to be
        ///     cleaned for two reasons, firstly to not pollute or distort the results, and secondly to save unnecessary processing
        ///     operations.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFilesFolders"></param>
        /// <param name="fastaFiles"></param>
        /// <param name="proteinOperationOptionFlags"></param>
        /// <param name="saveFastaFilenameTemplate"></param>
        /// <param name="consoleTextBox"></param>
        /// <param name="progressBar"></param>
        /// <param name="estimatedTimeRemaining"></param>
        public static void CleanProteins(CancellationToken cancellationToken, decimal maxAtomInterationDistance, string[] pdbFilesFolders, string[] fastaFiles, ProteinOperation proteinOperationOptionFlags, string saveFastaFilenameTemplate, ProgressActionSet progressActionSet, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            if (pdbFilesFolders == null || pdbFilesFolders.Length == 0)
            {
                if (proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveMultipleModelsInStructure) || proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveNonInteractingProteinsInStructure) || proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveNonSymmetricalInStructure) || proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveWrongNumberOfChainsInStructure))
                {
                    throw new ArgumentOutOfRangeException(nameof(pdbFilesFolders));
                }
            }

            if (fastaFiles == null || fastaFiles.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fastaFiles));
            }

            if (string.IsNullOrWhiteSpace(saveFastaFilenameTemplate))
            {
                throw new ArgumentOutOfRangeException(nameof(saveFastaFilenameTemplate));
            }

            string[] pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders);
            //List<string> pdbIdList = null;
            int beforeCount = 0;
            int afterCount = 0;
            string saveFilename = saveFastaFilenameTemplate;
            var currentProteinOperation = ProteinOperation.LoadFile;
            int[] numberSequencesLoaded;
            var sequences = new List<ISequence>[3];

            //UserProteinInterfaceOperations.TextBoxClear(consoleTextBox);
            ProgressActionSet.Report("Filtering proteins.", progressActionSet);

            // Load fasta/sequence files.
            sequences[0] = SequenceFileHandler.LoadSequenceFileList(fastaFiles, StaticValues.MolNameProteinAcceptedValues, out numberSequencesLoaded, true);
            var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequences[0]);

            for (int numberSequencesLoadedIndex = 0; numberSequencesLoadedIndex < numberSequencesLoaded.Length; numberSequencesLoadedIndex++)
            {
                if (numberSequencesLoaded[numberSequencesLoadedIndex] > 0)
                {
                    ProgressActionSet.Report("Loaded " + numberSequencesLoaded[numberSequencesLoadedIndex] / 2 + " proteins from file: " + fastaFiles[numberSequencesLoadedIndex], progressActionSet);
                }
                else
                {
                    ProgressActionSet.Report("Error could not load file: " + fastaFiles[numberSequencesLoadedIndex], progressActionSet);
                }
            }

            if (numberSequencesLoaded.Count(a => a > 0) == 0)
            {
                return;
            }

            // Replace placeholder variable names.
            saveFilename = saveFilename.Replace("%date%", DateTime.Now.ToString("yyyy-MM-dd"));
            saveFilename = saveFilename.Replace("%time%", DateTime.Now.ToString("HH.mm.ss"));

            // Save initial loaded sequences.

            if (File.Exists(saveFilename))
            {
                if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                {
                    saveFilename = FileExistsHandler.FindNextFreeOutputFilename(saveFilename);
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                {
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                {
                    return;
                }
            }

            // Removes any entries not having a protein alphabet.
            while (currentProteinOperation != ProteinOperation.Finished)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                currentProteinOperation = (ProteinOperation) ((int) currentProteinOperation*2);
                sequences[1] = null;
                sequences[2] = null;
                var sequencesDescriptions = new string[3];

                if (currentProteinOperation == ProteinOperation.Finished)
                {
                    break;
                }
                if (currentProteinOperation == ProteinOperation.RemoveNonProteinAlphabetInSequence && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveNonProteinAlphabetInSequence))
                {
                    continue;
                }
                if (currentProteinOperation == ProteinOperation.RemoveWrongNumberOfChainsInSequence && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveWrongNumberOfChainsInSequence))
                {
                    continue;
                }
                if (currentProteinOperation == ProteinOperation.RemoveExactDuplicatesInSequence && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveExactDuplicatesInSequence))
                {
                    continue;
                }
                if (currentProteinOperation == ProteinOperation.RemoveNonHomodimersInSequence && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveNonHomodimersInSequence))
                {
                    continue;
                }
                if (currentProteinOperation == ProteinOperation.RemoveWrongNumberOfChainsInStructure && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveWrongNumberOfChainsInStructure))
                {
                    continue;
                }
                if (currentProteinOperation == ProteinOperation.RemoveMultipleModelsInStructure && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveMultipleModelsInStructure))
                {
                    continue;
                }
                if (currentProteinOperation == ProteinOperation.RemoveNonInteractingProteinsInStructure && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveNonInteractingProteinsInStructure))
                {
                    continue;
                }
                if (currentProteinOperation == ProteinOperation.RemoveNonSymmetricalInStructure && !proteinOperationOptionFlags.HasFlag(ProteinOperation.RemoveNonSymmetricalInStructure))
                {
                    continue;
                }

                // Count sequences before operation.
                beforeCount = sequences[0].Count/2;

                // Update user about what is happening.
                ProgressActionSet.Report("", progressActionSet);
                ProgressActionSet.Report("Removing " + ProteinOperationString(currentProteinOperation) + " entries [from " + beforeCount + " proteins]", progressActionSet);

                // Start stopwatch to count duration of operation.
                Stopwatch stopwatch = Stopwatch.StartNew();

                // Perform specified operation.
                switch (currentProteinOperation)
                {
                    case ProteinOperation.RemoveNonProteinAlphabetInSequence:
                    {
                        sequencesDescriptions[0] = "01 - Removed non-protein sequences (sequence filter)";
                        sequences[0] = FilterProteins.RemoveNonProteinAlphabetSequences(cancellationToken, sequences[0], progressActionSet);
                        break;
                    }

                    case ProteinOperation.RemoveWrongNumberOfChainsInSequence:
                    {
                        sequencesDescriptions[0] = "02 - Removed non-dimers (sequence filter)";
                        sequences[0] = FilterProteins.RemoveSequencesWithIncorrectNumberOfChains(cancellationToken, sequences[0], 2, progressActionSet);
                        break;
                    }

                    case ProteinOperation.RemoveExactDuplicatesInSequence:
                    {
                        sequencesDescriptions[0] = "03 - Removed exact duplicates (sequence filter)";
                        sequences[0] = FilterProteins.RemoveDuplicates(cancellationToken, sequences[0], progressActionSet);
                        break;
                    }
                    case ProteinOperation.RemoveNonHomodimersInSequence:
                    {
                        // homodimers - all types - unfiltered for interactions or symmetry
                        
                        var result = FilterProteins.SplitDimerTypes(cancellationToken, sequences[0], 30, 90, progressActionSet);

                        sequencesDescriptions[0] = "04 - Homodimers only (sequence filter)";
                        sequences[0] = result.HomoDimerPdbIdList;

                        sequencesDescriptions[1] = "04 - Heterodimers only (sequence filter)";
                        sequences[1] = result.HeteroDimerPdbIdList;

                        sequencesDescriptions[2] = "04 - Homology dimers only (sequence filter)";
                        sequences[2] = result.HomologyDimerPdbIdList;
                        break;
                    }
                    case ProteinOperation.RemoveMultipleModelsInStructure:
                    {
                        sequencesDescriptions[0] = "05 - Removed multiple models (structure filter)";
                        List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequences[0]);
                        pdbIdList = FilterProteins.RemoveMultipleStructureModels(cancellationToken, pdbFilesFolders, pdbIdList, progressActionSet);
                        sequences[0] = FilterProteins.RemoveSequences(cancellationToken, sequences[0], pdbIdList, FilterProteins.RemoveSequencesOptions.RemoveSequencesInList);
                        break;
                    }
                    case ProteinOperation.RemoveWrongNumberOfChainsInStructure:
                    {
                        sequencesDescriptions[0] = "06 - Removed non-dimers (structure filter)";
                        List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequences[0]);

                        //var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequences[0]);

                        pdbIdList = FilterProteins.RemoveStructuresWithIncorrectNumberOfChains(cancellationToken, pdbFilesFolders, pdbIdList, pdbIdChainIdList, 2, progressActionSet);
                        sequences[0] = FilterProteins.RemoveSequences(cancellationToken, sequences[0], pdbIdList, FilterProteins.RemoveSequencesOptions.RemoveSequencesInList);
                        break;
                    }
                    case ProteinOperation.RemoveNonInteractingProteinsInStructure:
                    {
                        // Make copy of sequences as we will split the list into two parts - with and without interactions.
                        sequences[1] = new List<ISequence>(sequences[0]);

                        // Get pdb id list from sequences, to check for pdb file, load, perform processing.
                        List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequences[0]);

                        // Makes a list of sequences with interactions.
                        pdbIdList = FilterProteins.RemoveSequencesWithoutInteractions(cancellationToken, maxAtomInterationDistance, pdbFilesFolders, pdbIdList, pdbIdChainIdList, progressActionSet);

                        // Remove any protein not in the list, keep the ones in the list.
                        sequencesDescriptions[0] = "08 - dimers - with interactions - unfiltered for symmetry";
                        sequences[0] = FilterProteins.RemoveSequences(cancellationToken, sequences[0], pdbIdList, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);

                        sequencesDescriptions[1] = "07 - dimers - no observed interactions";
                        sequences[1] = FilterProteins.RemoveSequences(cancellationToken, sequences[1], pdbIdList, FilterProteins.RemoveSequencesOptions.RemoveSequencesInList);
                        break;
                    }

                    case ProteinOperation.RemoveNonSymmetricalInStructure:
                    {
                        // Make copy of sequences as we will split the list into two parts - with and without symmetry.
                        List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequences[0]);
                        sequences[1] = new List<ISequence>(sequences[0]);
                        sequences[2] = new List<ISequence>(sequences[0]);
                        Dictionary<string, decimal> symmetryPercentage = FilterProteins.CalculateStructureSymmetry(cancellationToken, maxAtomInterationDistance, pdbFilesFolders, pdbIdList, pdbIdChainIdList, progressActionSet);

                        var pdbSymmetrical = new List<string>();
                        var pdbPartSymmetrical = new List<string>();
                        var pdbNonSymmetrical = new List<string>();

                        foreach (var symmetryPercentageKeyValuePair in symmetryPercentage)
                        {
                            if (symmetryPercentageKeyValuePair.Value == 0.0m)
                            {
                                pdbNonSymmetrical.Add(symmetryPercentageKeyValuePair.Key);
                            }
                            else if (symmetryPercentageKeyValuePair.Value == 100.0m)
                            {
                                pdbSymmetrical.Add(symmetryPercentageKeyValuePair.Key);
                            }
                            else if (symmetryPercentageKeyValuePair.Value > 0.0m && symmetryPercentageKeyValuePair.Value < 100.0m)
                            {
                                pdbPartSymmetrical.Add(symmetryPercentageKeyValuePair.Key);
                            }
                            else
                            {
                                ProgressActionSet.Report("Error: Out of bounds symmetry value of " + symmetryPercentageKeyValuePair.Value + " was found in " + symmetryPercentageKeyValuePair.Key + ".", progressActionSet);
                            }
                        }

                        sequencesDescriptions[0] = "11 - dimers - with interactions - 100% symmetrical";
                        sequences[0] = FilterProteins.RemoveSequences(cancellationToken, sequences[0], pdbSymmetrical, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);

                        sequencesDescriptions[1] = "10 - dimers - with interactions - 1% to 99% symmetrical";
                        sequences[1] = FilterProteins.RemoveSequences(cancellationToken, sequences[1], pdbPartSymmetrical, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);

                        sequencesDescriptions[2] = "09 - dimers - with interactions - 0% symmetrical";
                        sequences[2] = FilterProteins.RemoveSequences(cancellationToken, sequences[2], pdbNonSymmetrical, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);

                        break;
                    }
                }

                // Stop stopwatch immediately after operation.
                stopwatch.Stop();

                // Count sequences after operation.
                afterCount = sequences[0].Count/2;

                if (!cancellationToken.IsCancellationRequested)
                {
                    for (int sequencesIndex = sequences.GetLowerBound(0); sequencesIndex <= sequences.GetUpperBound(0); sequencesIndex++)
                    {
                        if (sequences[sequencesIndex] != null)
                        {
                            // Find free filename to save the latest sequence results of operations.
                            string localSaveFilename = saveFilename;
                            localSaveFilename = localSaveFilename.Replace("%fasta_filename%", sequencesDescriptions[sequencesIndex]);


                            bool skipFile = false;

                            if (File.Exists(localSaveFilename))
                            {
                                if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                                {
                                    localSaveFilename = FileExistsHandler.FindNextFreeOutputFilename(localSaveFilename);
                                }
                                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                                {
                                }
                                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                                {
                                    skipFile = true;
                                }
                            }


                            if (!skipFile)
                            {
                                // Save the sequence results to previous set filename.
                                string savedFile/*s*/ = SequenceFileHandler.SaveSequencesAsFasta(sequences[sequencesIndex], localSaveFilename);

                                // Inform user that file has been saved.
                                //foreach (char savedFile in savedFiles)
                                //{
                                ProgressActionSet.Report("Saved file: " + savedFile, progressActionSet);
                                //}
                            }
                        }
                    }

                    // Update the user about the results.
                    ProgressActionSet.Report("Removed " + (beforeCount - afterCount) + " proteins. [" + afterCount + " proteins remaining]. Elapsed: " + stopwatch.Elapsed.ToString(@"dd\:hh\:mm\:ss"), progressActionSet);
                }
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                ProgressActionSet.Report("Finished all selected filtering operations.", progressActionSet);
            }
            else
            {
                ProgressActionSet.Report("Cancelled.", progressActionSet);
                //UserProteinInterfaceOperations.ProgressBarReset(progressBar, 0, 100, 0);
                ////UserProteinInterfaceOperations.LabelEstimatedTimeRemainingUpdate(estimatedTimeRemaining, 0, 1, 1);

                ProgressActionSet.StartAction(100, progressActionSet);
                ProgressActionSet.ProgressAction(100, progressActionSet);
                ProgressActionSet.FinishAction(false, progressActionSet);
            }
        }
    }
}