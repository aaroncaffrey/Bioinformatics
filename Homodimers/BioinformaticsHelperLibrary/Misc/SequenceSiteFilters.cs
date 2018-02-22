//-----------------------------------------------------------------------
// <copyright file="SequenceProteinInterfaceFilters.cs" company="Aaron Caffrey">
//     Copyright (c) 2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bio;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.UserProteinInterface;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class SequenceProteinInterfaceFilters
    {
        public static void ConfirmSequencesSaved(List<string> pdbIdList, List<string> saveFilenameList, ProgressActionSet progressActionSet)
        {
            if (pdbIdList == null) throw new ArgumentNullException(nameof(pdbIdList));
            // Confirm the total number of sequences saved is equal to original number loaded

            if (saveFilenameList == null || saveFilenameList.Count == 0)
            {
                ProgressActionSet.Report("Warning: no files were saved", progressActionSet);
                return;
            }

            List<ISequence> confirmSequencesList = SequenceFileHandler.LoadSequenceFileList(saveFilenameList, StaticValues.MolNameProteinAcceptedValues);

            List<string> confirmPdbIdList = FilterProteins.SequenceListToPdbIdList(confirmSequencesList);

            if (pdbIdList.Count == confirmPdbIdList.Count)
            {
                ProgressActionSet.Report("All sequences sorted into categories", progressActionSet);
            }
            else
            {
                var missingPdbIdList = new List<string>(pdbIdList);

                foreach (string pdbId in confirmPdbIdList)
                {
                    missingPdbIdList.Remove(pdbId);
                }

                ProgressActionSet.Report("Some sequences are unaccounted for: " + string.Join(", ", missingPdbIdList), progressActionSet);
            }
        }
        
        public static void FilterProteinInterfaceLengths(
            CancellationToken cancellationToken,
            decimal maxAtomInterationDistance,
            decimal minimumProteinInterfaceDensity,
            string[] sequenceListFileArray,
            string[] pdbFileDirectoryLocationArray,
            string filterProteinInterfacesLengthOutputFilename,
            bool filterProteinInterfaceCountsWithoutLengths,
            bool filterProteinInterfaceCountsWithLengths,
            FileExistsHandler.FileExistsOptions fileExistsOptions, 
            ProgressActionSet progressActionSet)
        {
            if (sequenceListFileArray == null) throw new ArgumentNullException(nameof(sequenceListFileArray));
            if (pdbFileDirectoryLocationArray == null) throw new ArgumentNullException(nameof(pdbFileDirectoryLocationArray));
            if (filterProteinInterfacesLengthOutputFilename == null) throw new ArgumentNullException(nameof(filterProteinInterfacesLengthOutputFilename));
            if (!filterProteinInterfaceCountsWithoutLengths && !filterProteinInterfaceCountsWithLengths)
            {
                ProgressActionSet.Report("Cancelled: No filter options selected.", progressActionSet);
                return;
            }

            // Check all sequence files are found
            var missingSequenceFiles = sequenceListFileArray.Where(sequenceFile => !string.IsNullOrWhiteSpace(sequenceFile) && !File.Exists(sequenceFile)).ToList();

            if (missingSequenceFiles.Count > 0)
            {
                foreach (string missingSequenceFile in missingSequenceFiles)
                {
                    //throw new FileNotFoundException(sequenceFile);

                    ProgressActionSet.Report("Warning: Sequence file missing: " + missingSequenceFile, progressActionSet);
                }

                ProgressActionSet.Report("Cancelled: missing sequence files.", progressActionSet);
                return;
            }

            // Check all pdb folders are found
            var missingDirectoryList = pdbFileDirectoryLocationArray.Where(pdbDirectory => !string.IsNullOrWhiteSpace(pdbDirectory) && !Directory.Exists(pdbDirectory)).ToList();
            if (missingDirectoryList.Count > 0)
            {
                foreach (string pdbDirectory in missingDirectoryList)
                {
                    //throw new DirectoryNotFoundException(pdbDirectory);
                    ProgressActionSet.Report("Warning: Structure file directory missing: " + pdbDirectory, progressActionSet);
                }

                ProgressActionSet.Report("Cancelled: missing structure file directory.", progressActionSet);
                return;
            }

            const string proteinInterfacesTemplateText = "%proteinInterfaces%";

            if (string.IsNullOrWhiteSpace(filterProteinInterfacesLengthOutputFilename) || !filterProteinInterfacesLengthOutputFilename.Contains(proteinInterfacesTemplateText))
            {
                throw new ArgumentOutOfRangeException(nameof(filterProteinInterfacesLengthOutputFilename));
            }

            // Load fasta sequence files
            List<ISequence> sequenceList = SequenceFileHandler.LoadSequenceFileList(sequenceListFileArray, StaticValues.MolNameProteinAcceptedValues);

            // Get a list of the PDB Unique IDs with unique chain IDs which are wanted, ignoring others which may be present e.g. dna
            var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequenceList);

            // Get list of PDB Unique IDs
            List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);

            // Check PDB Unique IDs were successfully loaded
            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                //throw new Exception("PDB ID List is empty or could not be loaded.");

                ProgressActionSet.Report("Error: Sequence list could not be loaded", progressActionSet);
                return;
            }

            // 3: Get a list of PDB files found in user specified directory

            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFileDirectoryLocationArray));
            
            // Check all PDB files are found
            List<string> missingPdbFilesList = ProteinDataBankFileOperations.CheckForMissingPdbFiles(pdbFilesArray, pdbIdList);

            if (missingPdbFilesList != null && missingPdbFilesList.Count > 0)
            {
                ProgressActionSet.Report("Missing PDB Files: " + string.Join(", ", missingPdbFilesList), progressActionSet);
            }

            
            
            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);
            

            int progressIncrement = 0;

            var proteinInterfacesCountResultWithLengths = new Dictionary<string, List<string>>();

            var startTicks = DateTime.Now.Ticks;

            // 4: Loop through each pdb file
            for (int pdbFileNumber = 0; pdbFileNumber < pdbFilesArray.Length + 1; pdbFileNumber++) // +1 is for progress update
            {
                if (progressIncrement > 0)
                {
                    ProgressActionSet.ProgressAction(progressIncrement, progressActionSet);
                    progressIncrement = 0;
                    if (pdbFileNumber >= pdbFilesArray.Length)
                    {
                        break;
                    }
                }
                ProgressActionSet.EstimatedTimeRemainingAction(startTicks, pdbFileNumber, pdbFilesArray.Length, progressActionSet);
                
                progressIncrement++;

                // get unique id of pdb file
                string pdbFilename = pdbFilesArray[pdbFileNumber];
                string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                // check pdb unique id was in the loaded sequence list
                if (!pdbIdList.Contains(proteinId))
                {
                    continue;
                }

                // perform clustering to detect interaction proteinInterfaces
                ClusterProteinDataBankFileResult clusterPdbFileResult = Clustering.ClusterProteinDataBankFile(cancellationToken, maxAtomInterationDistance, minimumProteinInterfaceDensity, pdbFilename, pdbIdChainIdList, ClusteringMethodOptions.ClusterWithResidueSequenceIndex, -1, -1, null);

                if (clusterPdbFileResult == null)
                {
                    continue;
                }

                int[] proteinInterfacesCount = new int[clusterPdbFileResult.ClusteringFullResultListContainer.ChainList.Count];

                for (int chainIndex = 0; chainIndex < clusterPdbFileResult.ClusteringFullResultListContainer.ChainList.Count; chainIndex++)
                {
                    int totalProteinInterfaces = clusterPdbFileResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList.Count(proteinInterface => proteinInterface.FullProteinInterfaceId.ChainId == chainIndex);

                    proteinInterfacesCount[chainIndex] = totalProteinInterfaces;

                }

                var proteinInterfacesCountStr = string.Join(" ", proteinInterfacesCount.OrderBy(x => x));

                List<ProteinInterfaceSequenceAndPositionData> proteinInterfaces = clusterPdbFileResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList;
                int[] proteinInterfacesLength = new int[proteinInterfaces.Count];

                for (int index = 0; index < proteinInterfaces.Count; index++)
                {
                    ProteinInterfaceSequenceAndPositionData proteinInterface = proteinInterfaces[index];

                    proteinInterfacesLength[index] = proteinInterface.ProteinInterfaceLength;
                }

                var proteinInterfacesLengthStr = string.Join(" ", proteinInterfacesLength.Distinct().OrderBy(x => x));

                if (proteinInterfacesLength.Length == 0) proteinInterfacesLengthStr = 0.ToString();

                var chainsCountStr = clusterPdbFileResult.ClusteringFullResultListContainer.ChainList.Count;

                if (filterProteinInterfaceCountsWithoutLengths)
                {
                    var combinedKeyAll = "chains [" + chainsCountStr + "] proteinInterfaces [" + proteinInterfacesCountStr + "]";

                    if (!proteinInterfacesCountResultWithLengths.ContainsKey(combinedKeyAll))
                    {
                        proteinInterfacesCountResultWithLengths.Add(combinedKeyAll, new List<string>());
                    }

                    proteinInterfacesCountResultWithLengths[combinedKeyAll].Add(proteinId);
                }

                if (filterProteinInterfaceCountsWithLengths)
                {
                    var combinedKeyWithLengths = "chains [" + chainsCountStr + "] proteinInterfaces [" + proteinInterfacesCountStr + "] lengths [" + proteinInterfacesLengthStr + "]";

                    if (!proteinInterfacesCountResultWithLengths.ContainsKey(combinedKeyWithLengths))
                    {
                        proteinInterfacesCountResultWithLengths.Add(combinedKeyWithLengths, new List<string>());
                    }

                    proteinInterfacesCountResultWithLengths[combinedKeyWithLengths].Add(proteinId);
                }
            }

            var confirmSaveList = new List<string>();

            foreach (var kvp in proteinInterfacesCountResultWithLengths)
            {
                var seq2 = new List<ISequence>(sequenceList);
                seq2 = FilterProteins.RemoveSequences(cancellationToken, seq2, kvp.Value, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);

                var saveFilename = filterProteinInterfacesLengthOutputFilename;
                saveFilename = saveFilename.Replace(proteinInterfacesTemplateText, kvp.Key);
                
                var actualSavedFilename = SequenceFileHandler.SaveSequencesAsFasta(seq2, saveFilename, true, fileExistsOptions, progressActionSet);

                if (!string.IsNullOrWhiteSpace(actualSavedFilename))
                {
                    confirmSaveList.Add(actualSavedFilename);
                }
            }

            // Confirm the total number of sequences saved is equal to original number loaded
            ConfirmSequencesSaved(pdbIdList, confirmSaveList, progressActionSet);

            ProgressActionSet.FinishAction(true, progressActionSet);
        }
    }
}
