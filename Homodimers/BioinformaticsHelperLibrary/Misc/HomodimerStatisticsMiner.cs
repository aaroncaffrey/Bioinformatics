//-----------------------------------------------------------------------
// <copyright file="HomodimerStatisticsMiner.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bio;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.InteractionDetection;
using BioinformaticsHelperLibrary.InteractionVector;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.TaskManagement;
using BioinformaticsHelperLibrary.UserProteinInterface;

namespace BioinformaticsHelperLibrary.Misc
{
    public class HomodimerStatisticsMiner
    {
        /// <summary>
        ///     Makes spreadsheets with scientific data outputs about given proteins.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFolders">The location of the PDB files</param>
        /// <param name="pdbIdList">The PDB files which should be used.</param>
        /// <param name="consoleTextBox"></param>
        /// <param name="progressBar">User proteinInterface progress bar for user feedback.</param>
        /// <param name="estimatedTimeRemainingLabel">User proteinInterface estimated time remaining label for user feedback.</param>
        /// <param name="requestedTotalThreads"></param>
        /// <returns>Returns the generated spreadsheets with scientific data.</returns>
        public static List<List<SpreadsheetCell[]>> MakeHomodimerStatisticsSpreadsheetsList(CancellationToken cancellationToken, decimal maxAtomInterationDistance, string[] pdbFolders, List<string> pdbIdList = null, Dictionary<string, List<string>> pdbIdChainIdList = null, ProgressActionSet progressActionSet = null, int requestedTotalThreads = -1)
        {
            if (pdbFolders == null || pdbFolders.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFolders));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            if (progressActionSet == null )
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            // this method creates
            // 1. a list of interactions
            // 2. a list of symmetry percentage
            // 3. an "expected" heatmap by combining every possible a/b amino acid combination
            // 4. an actual heatmap for the proteinInterfaces
            // 5. normalised versions of both of the heatmaps

            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFolders));

            //var interactionRecordList = new List<ProteinInteractionRecord>();
            //var interactionMatchPercentageList = new List<InteractionMatchPercentage>();
            //var wholeProteinChainsAminoAcidCounter = new List<AminoAcidChainComposition>();
            //var interactionChainsAminoAcidCounter = new List<AminoAcidChainComposition>();
            //var interactionsAminoAcidToAminoAcidCounter = new AminoAcidPairCompositionMatrix();

            ////var wholeProteinAminoAcidToAminoAcidCounter2x2 = new AminoAcidPairCompositionMatrix(); // composition of every amino acid paired in every possible combination

            var workDivision = new WorkDivision<HomodimersStatisticsMinerTaskResult>(pdbFilesArray.Length, requestedTotalThreads);


            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);

            

            int checkAllFilesProcessed = 0;
            var lockCheckAllFilesProcessed = new object();

            var pdbFilesProcessed = new bool[pdbFilesArray.Length];
            Array.Clear(pdbFilesProcessed, 0, pdbFilesProcessed.Length);

            for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
            {
                int localThreadIndex = threadIndex;

                Task<HomodimersStatisticsMinerTaskResult> task = Task.Run(() =>
                {
                    var result = new HomodimersStatisticsMinerTaskResult();

                    for (int pdbFileNumber = workDivision.ThreadFirstIndex[localThreadIndex]; pdbFileNumber <= workDivision.ThreadLastIndex[localThreadIndex]; pdbFileNumber++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        lock (lockCheckAllFilesProcessed)
                        {
                            checkAllFilesProcessed++;
                            pdbFilesProcessed[pdbFileNumber] = true;
                        }

                        try
                        {
                            string pdbFilename = pdbFilesArray[pdbFileNumber];
                            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                            // Check if the file found is included in the white list.
                            if ( /*pdbIdList != null && */!pdbIdList.Contains(proteinId))
                            {
                                ProgressActionSet.Report("Error: " + proteinId + " was not in the PDB ID white list.", progressActionSet);
                                continue;
                            }

                            List<AtomPair> interactions = SearchInteractions.FindInteractions(cancellationToken, maxAtomInterationDistance, pdbFilename, pdbIdChainIdList);

                            // Make a list to save interactions found.
                            var interactionMatchPercentage = new InteractionMatchPercentage(proteinId);

                            var chainAminoAcidCounterA1X1 = new AminoAcidChainComposition(proteinId, "A");
                            var chainAminoAcidCounterB1X1 = new AminoAcidChainComposition(proteinId, "B");

                            var chainInteractionAminoAcidCounterA = new AminoAcidChainComposition(proteinId, "A");
                            var chainInteractionAminoAcidCounterB = new AminoAcidChainComposition(proteinId, "B");

                            if (interactions != null && interactions.Count > 0)
                            {
                                interactionMatchPercentage.IncrementTotalInteractions(interactions.Count);

                                for (int interactionsIndex = 0; interactionsIndex < interactions.Count; interactionsIndex++)
                                {
                                    chainInteractionAminoAcidCounterA.IncrementAminoAcidCount(interactions[interactionsIndex].Atom1.resName.FieldValue);

                                    chainInteractionAminoAcidCounterB.IncrementAminoAcidCount(interactions[interactionsIndex].Atom2.resName.FieldValue);

                                    result.InteractionRecordList.Add(new ProteinInteractionRecord(proteinId, interactionsIndex + 1, interactions[interactionsIndex]));
                                    interactionMatchPercentage.AddResidueSequenceIndex(StaticValues.ChainA, interactions[interactionsIndex].Atom1.resSeq.FieldValue);
                                    interactionMatchPercentage.AddResidueSequenceIndex(StaticValues.ChainB, interactions[interactionsIndex].Atom2.resSeq.FieldValue);
                                    result.InteractionsAminoAcidToAminoAcidCounter.IncrementAminoAcidCount(interactions[interactionsIndex].Atom1.resName.FieldValue, interactions[interactionsIndex].Atom2.resName.FieldValue);
                                }
                            }

                            var chainIdList = pdbIdChainIdList != null ? (pdbIdChainIdList.ContainsKey(proteinId) ? pdbIdChainIdList[proteinId].ToArray() : null) : null;

                            ProteinChainListContainer proteinFileChains = ProteinDataBankFileOperations.PdbAtomicChains(pdbFilename, chainIdList, 2, 2, true);

                            if (proteinFileChains == null || proteinFileChains.ChainList == null || proteinFileChains.ChainList.Count != 2 ||
                                proteinFileChains.ChainList[StaticValues.ChainA] == null || proteinFileChains.ChainList[StaticValues.ChainA].AtomList == null || proteinFileChains.ChainList[StaticValues.ChainA].AtomList.Count == 0 ||
                                proteinFileChains.ChainList[StaticValues.ChainB] == null || proteinFileChains.ChainList[StaticValues.ChainB].AtomList == null || proteinFileChains.ChainList[StaticValues.ChainB].AtomList.Count == 0)
                            {
                                if (!File.Exists(pdbFilename))
                                {
                                    ProgressActionSet.Report("Error: " + pdbFilename + " (" + proteinId + ") file not found", progressActionSet);
                                }
                                else
                                {
                                    int proteinFileChainCount = ProteinDataBankFileOperations.PdbAtomicChainsCount(pdbFilename);
                                    ProgressActionSet.Report("Error: " + proteinId + " did not have exactly 2 chains (" + proteinFileChainCount + " chains found) - skipping.", progressActionSet);
                                }

                                continue;
                            }

                            // count total of how many of each type of amino acids are in Chain A.
                            for (int atomIndexA = 0; atomIndexA < proteinFileChains.ChainList[StaticValues.ChainA].AtomList.Count; atomIndexA++)
                            {
                                chainAminoAcidCounterA1X1.IncrementAminoAcidCount(proteinFileChains.ChainList[StaticValues.ChainA].AtomList[atomIndexA].resName.FieldValue);
                            }

                            // count total of how many of each type of amino acids are in Chain B.
                            for (int atomIndexB = 0; atomIndexB < proteinFileChains.ChainList[StaticValues.ChainB].AtomList.Count; atomIndexB++)
                            {
                                chainAminoAcidCounterB1X1.IncrementAminoAcidCount(proteinFileChains.ChainList[StaticValues.ChainB].AtomList[atomIndexB].resName.FieldValue);
                            }

                            interactionMatchPercentage.CalculatePercentage();
                            result.InteractionMatchPercentageList.Add(interactionMatchPercentage);
                            result.WholeProteinChainsAminoAcidCounter.Add(chainAminoAcidCounterA1X1);
                            result.WholeProteinChainsAminoAcidCounter.Add(chainAminoAcidCounterB1X1);
                            result.InteractionChainsAminoAcidCounter.Add(chainInteractionAminoAcidCounterA);
                            result.InteractionChainsAminoAcidCounter.Add(chainInteractionAminoAcidCounterB);
                        }
                        finally
                        {
                            workDivision.IncrementItemsCompleted(1);

                            ProgressActionSet.ProgressAction(1, progressActionSet);
                            ProgressActionSet.EstimatedTimeRemainingAction(workDivision.StartTicks, workDivision.ItemsCompleted, workDivision.ItemsToProcess, progressActionSet);
                        }
                    }

                    return result;
                }, cancellationToken);
                workDivision.TaskList.Add(task);
            }


            workDivision.WaitAllTasks();

            ProgressActionSet.FinishAction(true, progressActionSet);

            // merge all instances of the results 
            var spreadsheetTaskResult = new HomodimersStatisticsMinerTaskResult();
            foreach (var task in workDivision.TaskList.Where(t => t != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted && t.Result != null))
            {
                if (task.Result.InteractionChainsAminoAcidCounter != null && task.Result.InteractionChainsAminoAcidCounter.Count > 0)
                {
                    spreadsheetTaskResult.InteractionChainsAminoAcidCounter.AddRange(task.Result.InteractionChainsAminoAcidCounter);
                }

                if (task.Result.InteractionMatchPercentageList != null && task.Result.InteractionMatchPercentageList.Count > 0)
                {
                    spreadsheetTaskResult.InteractionMatchPercentageList.AddRange(task.Result.InteractionMatchPercentageList);
                }

                if (task.Result.InteractionRecordList != null && task.Result.InteractionRecordList.Count > 0)
                {
                    spreadsheetTaskResult.InteractionRecordList.AddRange(task.Result.InteractionRecordList);
                }

                if (task.Result.InteractionsAminoAcidToAminoAcidCounter != null)
                {
                    if (task.Result.InteractionsAminoAcidToAminoAcidCounter.AminoAcidToAminoAcid != null)
                    {
                        foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                        {
                            var totalGroups = AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups);

                            for (int x = 0; x < totalGroups; x++)
                            {
                                for (int y = 0; y < totalGroups; y++)
                                {
                                    spreadsheetTaskResult.InteractionsAminoAcidToAminoAcidCounter.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][x, y] +=
                                              task.Result.InteractionsAminoAcidToAminoAcidCounter.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][x, y];
                                }
                            }
                        }
                    }
                }

                if (task.Result.WholeProteinChainsAminoAcidCounter != null && task.Result.WholeProteinChainsAminoAcidCounter.Count > 0)
                {
                    spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter.AddRange(task.Result.WholeProteinChainsAminoAcidCounter);
                }
            }


            if (pdbFilesProcessed.Count(file => file == false) > 0)
            {
                ProgressActionSet.Report("ERROR: " + pdbFilesProcessed.Count(file => file == false) + " PDB FILES WERE SKIPPED! 0x01", progressActionSet);
            }
            else
            {
                ProgressActionSet.Report("CHECK: NO PDB FILES WERE SKIPPED! 0x01", progressActionSet);
            }

            if (checkAllFilesProcessed != pdbFilesArray.Length)
            {
                ProgressActionSet.Report("ERROR: " + (pdbFilesArray.Length - checkAllFilesProcessed) + " PDB FILES WERE SKIPPED! 0x02", progressActionSet);
            }
            else
            {
                ProgressActionSet.Report("CHECK: NO PDB FILES WERE SKIPPED! 0x02", progressActionSet);
            }


            spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter = spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter.OrderBy(a => a.ProteinId).ThenBy(b => b.ChainId).ToList();
            spreadsheetTaskResult.InteractionChainsAminoAcidCounter = spreadsheetTaskResult.InteractionChainsAminoAcidCounter.OrderBy(a => a.ProteinId).ThenBy(b => b.ChainId).ToList();

            AminoAcidChainComposition wholeProteinChainsTotals = AminoAcidChainComposition.TotalFromAminoAcidChainCompositionList(spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter);
            AminoAcidChainComposition interactionChainsTotals = AminoAcidChainComposition.TotalFromAminoAcidChainCompositionList(spreadsheetTaskResult.InteractionChainsAminoAcidCounter);
            AminoAcidPairCompositionMatrix wholeProteinAminoAcidToAminoAcidCounter1X1 = AminoAcidChainComposition.ConvertToMatrix(wholeProteinChainsTotals);

            var results = new List<List<SpreadsheetCell[]>>();

            {
                /* start test */
                var spreadsheet1 = new List<SpreadsheetCell[]>();
                spreadsheet1.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% TEST SHEET 0"), });
                spreadsheet1.Add(new[] { new SpreadsheetCell("TEST SHEET 0"), });
                foreach (AminoAcidChainComposition item in spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter)
                {
                    //spreadsheet1.Add(item.ProteinId);
                    //spreadsheet1.Add(item.ChainId);
                    spreadsheet1.Add(item.SpreadsheetDataRow());
                }
                results.Add(spreadsheet1);
                spreadsheet1 = null;
                /* end test */
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            {
                var spreadsheet2 = new List<SpreadsheetCell[]>();
                spreadsheet2.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% L Interaction Symmetry"), });
                spreadsheet2.Add(new[] { new SpreadsheetCell("Homodimers - List - Interaction Count And Interaction Match Percentage (Symmetry Measurement)")});
                spreadsheet2.Add(InteractionMatchPercentage.SpreadsheetColumnHeadersRow());
                var range2 = spreadsheetTaskResult.InteractionMatchPercentageList.Select(record => record.SpreadsheetDataRow()).ToList();
                //range2.Sort();
                range2 = range2
                    .OrderBy(a => a[0].CellData)
                    .ThenBy(a => a[1].CellData)
                    .ThenBy(a => a[2].CellData)
                    .ThenBy(a => a[3].CellData)
                    .ThenBy(a => a[4].CellData)
                    .ThenBy(a => a[5].CellData)
                    .ThenBy(a => a[6].CellData)
                    .ThenBy(a => a[7].CellData)
                    .ThenBy(a => a[8].CellData)
                    .ToList();
                spreadsheet2.AddRange(range2);
                range2 = null;
                results.Add(spreadsheet2);

                var spreadsheetHistogram2 = new List<SpreadsheetCell[]>();
                spreadsheetHistogram2.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% HG Interaction Symmetry"), });
                spreadsheetHistogram2.Add(new[] { new SpreadsheetCell("Homodimers - List - Interaction Count And Interaction Match Percentage (Symmetry Measurement) Histogram") });
                spreadsheetHistogram2.AddRange(Histogram.MatrixToHistogram(spreadsheet2.ToArray(), Histogram.MakeBinDecimals(0, 100, 9, 10), new[] { 6, 7, 8 }, 2, -1, true));
                results.Add(spreadsheetHistogram2);

                spreadsheet2 = null;
                spreadsheetHistogram2 = null;
            }

            //
            {
                var spreadsheet3 = new List<SpreadsheetCell[]>();
                spreadsheet3.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% L Interaction Records"), });
                spreadsheet3.Add(new[] { new SpreadsheetCell("Homodimers - List - Protein Interaction Record"), });
                spreadsheet3.Add(ProteinInteractionRecord.TsvColumnHeadersRow());
                var range3 = spreadsheetTaskResult.InteractionRecordList.Select(record => record.SpreadsheetDataRow()).ToList();
                //range3.Sort();
                range3 = range3
                    .OrderBy(a => a[0].CellData)
                    .ThenBy(a => a[1].CellData)
                    .ThenBy(a => a[3].CellData)
                    .ThenBy(a => a[5].CellData)
                    .ThenBy(a => a[13].CellData)
                    .ThenBy(a => a[15].CellData)
                    .ToList();
                spreadsheet3.AddRange(range3);
                range3 = null;

                results.Add(spreadsheet3);

                var spreadsheetHistogram3 = new List<SpreadsheetCell[]>();
                spreadsheetHistogram3.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% L Interaction Records Histogram"), });
                spreadsheetHistogram3.Add(new[] { new SpreadsheetCell("Homodimers - List - Protein Interaction Record - Histogram"), });
                spreadsheetHistogram3.AddRange(Histogram.MatrixToHistogram(spreadsheet3.ToArray(), Histogram.MakeBinDecimals(0m, 5m, 0m, 0.05m), new[] { 1 }, 2, -1, true));
                results.Add(spreadsheetHistogram3);

                //spreadsheet3 = Histogram.InsertMatrixOverwrite(spreadsheet3.ToArray(), histogram3, 2, Histogram.MaxColumns(spreadsheet3.ToArray()) + 1).ToList();
                spreadsheet3 = null;
                spreadsheetHistogram3 = null;
            }
            //

            {
                var spreadsheet4 = new List<SpreadsheetCell[]>();
                spreadsheet4.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% L Interaction Count - A-Z"), });
                spreadsheet4.Add(new[] { new SpreadsheetCell("Homodimers - List - Protein Amino Acid Count - Interactions - A to Z"), });
                spreadsheet4.Add(AminoAcidChainComposition.SpreadsheetTitleRow());
                var range4 = spreadsheetTaskResult.InteractionChainsAminoAcidCounter.Select(record => record.SpreadsheetDataRow()).ToList();
                //range4.Sort();
                range4 = range4
                    .OrderBy(a => a[0].CellData)
                    .ThenBy(a => a[1].CellData)
                    .ToList();
                spreadsheet4.AddRange(range4);
                range4 = null;
                spreadsheet4.Add(interactionChainsTotals.SpreadsheetDataRow());
                results.Add(spreadsheet4);
                spreadsheet4 = null;
            }
            //

            {

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    

                    var spreadsheet5 = new List<SpreadsheetCell[]>();
                    spreadsheet5.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% L Interaction Count - Groups " + enumAminoAcidGroups), });
                    spreadsheet5.Add(new[] { new SpreadsheetCell("Homodimers - List - Protein Amino Acid Count - Interactions - Acid Groups " + enumAminoAcidGroups), });
                    spreadsheet5.Add(AminoAcidChainComposition.SpreadsheetGroupsTitleRow(enumAminoAcidGroups));
                    var range5 = spreadsheetTaskResult.InteractionChainsAminoAcidCounter.Select(record => record.SpreadsheetGroupsDataRow(enumAminoAcidGroups)).ToList();
                    //range4.Sort();
                    range5 = range5
                        .OrderBy(a => a[0].CellData)
                        .ThenBy(a => a[1].CellData)
                        .ToList();
                    spreadsheet5.AddRange(range5);
                    range5 = null;
                    spreadsheet5.Add(interactionChainsTotals.SpreadsheetGroupsDataRow(enumAminoAcidGroups));

                    results.Add(spreadsheet5);
                    spreadsheet5 = null;
                }
            }
            //

            {
                var spreadsheet6 = new List<SpreadsheetCell[]>();
                spreadsheet6.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% L Entire Count - A-Z"), });
                spreadsheet6.Add(new[] { new SpreadsheetCell("Homodimers - List - Protein Amino Acid Count - All Atoms - A to Z"), });
                spreadsheet6.Add(AminoAcidChainComposition.SpreadsheetTitleRow());
                var range6 = spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter.Select(record => record.SpreadsheetDataRow()).ToList();
                //range6.Sort();
                range6 = range6
                    .OrderBy(a => a[0].CellData)
                    .ThenBy(a => a[1].CellData)
                    .ToList();
                spreadsheet6.AddRange(range6);
                range6 = null;
                spreadsheet6.Add(wholeProteinChainsTotals.SpreadsheetDataRow());
                results.Add(spreadsheet6);

                var spreadsheetHistogram6 = new List<SpreadsheetCell[]>();
                spreadsheetHistogram6.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% L Entire Count - A-Z - Historgram"), });
                spreadsheetHistogram6.Add(new[] { new SpreadsheetCell("Homodimers - List - Protein Amino Acid Count - All Atoms - A to Z - Histogram"), });
                spreadsheetHistogram6.AddRange(Histogram.MatrixToHistogram(spreadsheet6.ToArray(), Histogram.MakeBinDecimals(0, 10500, 0, 500), new[] { 28 }, 2, -1, true));
                spreadsheetHistogram6.Add(new []{ new SpreadsheetCell(""), });
                spreadsheetHistogram6.AddRange(Histogram.MatrixToHistogram(spreadsheet6.ToArray(), Histogram.MakeBinDecimals(0, 1000, 0, 100), new[] { 28 }, 2, -1, true));
                results.Add(spreadsheetHistogram6);
                
                spreadsheet6 = null;
                spreadsheetHistogram6 = null;
            }
            //

            {
                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    

                    var spreadsheet7 = new List<SpreadsheetCell[]>();
                    spreadsheet7.Add(new[] {new SpreadsheetCell("%batch_number%%batch_letter% L Entire Count - Groups " + enumAminoAcidGroups),});
                    
                    spreadsheet7.Add(new[] { new SpreadsheetCell("Homodimers - List - Protein Amino Acid Count - All Atoms - Acid Groups " + enumAminoAcidGroups), });
                    spreadsheet7.Add(AminoAcidChainComposition.SpreadsheetGroupsTitleRow(enumAminoAcidGroups));
                    var range7 = spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter.Select(record => record.SpreadsheetGroupsDataRow(enumAminoAcidGroups)).ToList();
                    //range7.Sort();
                    range7 = range7
                        .OrderBy(a => a[0].CellData)
                        .ThenBy(a => a[1].CellData)
                        .ToList();
                    spreadsheet7.AddRange(range7);
                    range7 = null;
                    spreadsheet7.Add(wholeProteinChainsTotals.SpreadsheetGroupsDataRow(enumAminoAcidGroups));

                    results.Add(spreadsheet7);
                    spreadsheet7 = null;
                }
            }


            // convert to percentage for creating mean average protein composition 
            var meanProteinComposition = new AminoAcidChainComposition("Mean Composition", "-");

            foreach (AminoAcidChainComposition aminoAcidChainComposition in spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter)
            {
                // get percentage for row
                AminoAcidChainComposition percentage = AminoAcidChainComposition.ConvertToPercentage(aminoAcidChainComposition);

                // add percentage to overall tally

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    for (int x = 0; x < AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups); x++)
                    {
                        meanProteinComposition.AminoAcidGroupsCount[(int)enumAminoAcidGroups][x] += (percentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][x]/spreadsheetTaskResult.WholeProteinChainsAminoAcidCounter.Count);
                    }
                }

            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            {
                /* start test */
                var spreadsheet8 = new List<SpreadsheetCell[]>();
                spreadsheet8.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% TEST SHEET 1"), }); // Worksheet name.
                spreadsheet8.Add(new[] { new SpreadsheetCell("TEST SHEET 1"), }); // Spreadsheet title

                spreadsheet8.Add(new[] { new SpreadsheetCell(string.Empty), });
                spreadsheet8.Add(meanProteinComposition.SpreadsheetDataRow());
                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    
                    spreadsheet8.Add(meanProteinComposition.SpreadsheetGroupsDataRow(enumAminoAcidGroups));
                }
                results.Add(spreadsheet8);
                spreadsheet8 = null;
                /* end test */
            }

            AminoAcidPairCompositionMatrix meanProteinMatrix = AminoAcidChainComposition.ConvertToMatrix(meanProteinComposition);

            {
                var spreadsheet9 = new List<SpreadsheetCell[]>();
                spreadsheet9.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% HM All Atoms 3x3"), }); // Worksheet name.

                spreadsheet9.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Average Chain Composition"), }); // Spreadsheet title.

                //spreadsheet9.Add(new[] { new SpreadsheetCell(string.Empty), });
                //spreadsheet9.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Average Chain Composition - Percentage Composition - A to Z"), }); // Section title.
                //spreadsheet9.AddRange(meanProteinMatrix.SpreadsheetAminoAcidColorGroupsHeatMap());

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    

                    spreadsheet9.Add(new[] {new SpreadsheetCell(string.Empty),});
                    spreadsheet9.Add(new[] {new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Average Chain Composition - Percentage Composition - Acid Groups " + enumAminoAcidGroups),}); // Section title.
                    spreadsheet9.AddRange(meanProteinMatrix.SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                }
                results.Add(spreadsheet9);
                spreadsheet9 = null;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //if (outputAllAtoms1x1)
            //{
            AminoAcidPairCompositionMatrix wholeProteinAminoAcidToAminoAcidCounterPercentage1X1 = AminoAcidPairCompositionMatrix.CalculatePercentageMatrix(wholeProteinAminoAcidToAminoAcidCounter1X1);
            {
                var spreadsheet10 = new List<SpreadsheetCell[]>();
                spreadsheet10.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% HM All Atoms 1x1")}); // Worksheet name.

                spreadsheet10.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Overall Composition") }); // Spreadsheet title.

                //spreadsheet10.Add(new[] { new SpreadsheetCell(string.Empty)});
                //spreadsheet10.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Overall Percentage Composition - A to Z")}); // Section title.
                //spreadsheet10.AddRange(wholeProteinAminoAcidToAminoAcidCounterPercentage1X1.SpreadsheetAminoAcidColorGroupsHeatMap());

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    spreadsheet10.Add(new[] {new SpreadsheetCell(string.Empty)});
                    spreadsheet10.Add(new[] {new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Overall Percentage Composition - Acid Groups " + enumAminoAcidGroups)}); // Section title.
                    spreadsheet10.AddRange(wholeProteinAminoAcidToAminoAcidCounterPercentage1X1.SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                }

                AminoAcidPairCompositionMatrix wholeProteinAminoAcidToAminoAcidCounterNormalised1X1 = AminoAcidPairCompositionMatrix.NormalizeWithCompositionMatrix(wholeProteinAminoAcidToAminoAcidCounterPercentage1X1, UniProtProteinDatabaseComposition.AminoAcidCompositionAsMatrix());

                //spreadsheet10.Add(new[] { new SpreadsheetCell(string.Empty)});
                //spreadsheet10.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Overall UniProt Normalised - A to Z ")}); // Section title.
                //spreadsheet10.AddRange(wholeProteinAminoAcidToAminoAcidCounterNormalised1X1.SpreadsheetAminoAcidColorGroupsHeatMap());

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    spreadsheet10.Add(new[] {new SpreadsheetCell(string.Empty)});
                    spreadsheet10.Add(new[] {new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Overall UniProt Normalised - Acid Groups " + enumAminoAcidGroups)}); // Section title.
                    spreadsheet10.AddRange(wholeProteinAminoAcidToAminoAcidCounterNormalised1X1.SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                }

                AminoAcidPairCompositionMatrix wholeProteinAminoAcidToAminoAcidCounterDifference1X1 = AminoAcidPairCompositionMatrix.DifferenceWithCompositionMatrix(wholeProteinAminoAcidToAminoAcidCounterPercentage1X1, UniProtProteinDatabaseComposition.AminoAcidCompositionAsMatrix());

                //spreadsheet10.Add(new[] { new SpreadsheetCell(string.Empty)});
                //spreadsheet10.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Overall A to Z - UniProt Difference")}); // Section title.
                //spreadsheet10.AddRange(wholeProteinAminoAcidToAminoAcidCounterDifference1X1.SpreadsheetAminoAcidColorGroupsHeatMap());

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    spreadsheet10.Add(new[] {new SpreadsheetCell(string.Empty)});
                    spreadsheet10.Add(new[] {new SpreadsheetCell("Homodimers - Amino Acid Heat Map - All Atoms - Overall Acid Groups " + enumAminoAcidGroups + " - UniProt Difference")}); // Section title.
                    spreadsheet10.AddRange(wholeProteinAminoAcidToAminoAcidCounterDifference1X1.SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                }

                results.Add(spreadsheet10);
                spreadsheet10 = null;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            {
                AminoAcidPairCompositionMatrix interactionsAminoAcidToAminoAcidCounterPercentage = AminoAcidPairCompositionMatrix.CalculatePercentageMatrix(spreadsheetTaskResult.InteractionsAminoAcidToAminoAcidCounter);

                var spreadsheet11 = new List<SpreadsheetCell[]>();
                spreadsheet11.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% HM Interactions Only")}); // Worksheet name.

                spreadsheet11.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Interactions Only")}); // Spreadsheet title.

                //spreadsheet11.Add(new[] { new SpreadsheetCell(string.Empty)});
                //spreadsheet11.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Interactions Only - A to Z")}); // Section title.
                //spreadsheet11.AddRange(spreadsheetTaskResult.InteractionsAminoAcidToAminoAcidCounter.SpreadsheetAminoAcidColorGroupsHeatMap());

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    spreadsheet11.Add(new[] {new SpreadsheetCell(string.Empty)});
                    spreadsheet11.Add(new[] {new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Interactions Only - Acid Groups " + enumAminoAcidGroups)}); // Section title.
                    spreadsheet11.AddRange(spreadsheetTaskResult.InteractionsAminoAcidToAminoAcidCounter.SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                }

                AminoAcidPairCompositionMatrix interactionsAminoAcidToAminoAcidCounterNormalised = AminoAcidPairCompositionMatrix.NormalizeWithCompositionMatrix(interactionsAminoAcidToAminoAcidCounterPercentage, UniProtProteinDatabaseComposition.AminoAcidCompositionAsMatrix());

                //spreadsheet11.Add(new[] { new SpreadsheetCell(string.Empty)});
                //spreadsheet11.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Interactions Only - A to Z - UniProt Normalised")}); // Section title.
                //spreadsheet11.AddRange(interactionsAminoAcidToAminoAcidCounterNormalised.SpreadsheetAminoAcidColorGroupsHeatMap());

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    spreadsheet11.Add(new[] {new SpreadsheetCell(string.Empty)});
                    spreadsheet11.Add(new[] {new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Interactions Only - Acid Groups "+enumAminoAcidGroups+" - UniProt Normalised")}); // Section title.
                    spreadsheet11.AddRange(interactionsAminoAcidToAminoAcidCounterNormalised.SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                }

                AminoAcidPairCompositionMatrix interactionsAminoAcidToAminoAcidCounterDifference = AminoAcidPairCompositionMatrix.DifferenceWithCompositionMatrix(interactionsAminoAcidToAminoAcidCounterPercentage, UniProtProteinDatabaseComposition.AminoAcidCompositionAsMatrix());

                //spreadsheet11.Add(new[] { new SpreadsheetCell(string.Empty)});
                //spreadsheet11.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Interactions Only - A to Z - UniProt Difference")}); // Section title.
                //spreadsheet11.AddRange(interactionsAminoAcidToAminoAcidCounterDifference.SpreadsheetAminoAcidColorGroupsHeatMap());

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    spreadsheet11.Add(new[] {new SpreadsheetCell(string.Empty)});
                    spreadsheet11.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Interactions Only - Acid Groups " + enumAminoAcidGroups + " - UniProt Difference") }); // Section title.
                    spreadsheet11.AddRange(interactionsAminoAcidToAminoAcidCounterDifference.SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                }

                results.Add(spreadsheet11);

                spreadsheet11 = null;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            {
                var spreadsheet12 = new List<SpreadsheetCell[]>();
                spreadsheet12.Add(new[] { new SpreadsheetCell("%batch_number%%batch_letter% HM Interactions v Homodimers")}); // Worksheet name. 

                spreadsheet12.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Difference between homodimer composition and homodimer interactions")}); // Spreadsheet title
                spreadsheet12.Add(new[] { new SpreadsheetCell(string.Empty)});

                //spreadsheet12.Add(new[] { new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Difference between homodimer composition and homodimer interactions - A to Z")}); // Section title
                //spreadsheet12.AddRange(AminoAcidPairCompositionMatrix.DifferenceWithCompositionMatrix(wholeProteinAminoAcidToAminoAcidCounterPercentage1X1, spreadsheetTaskResult.InteractionsAminoAcidToAminoAcidCounter).SpreadsheetAminoAcidColorGroupsHeatMap());
                //spreadsheet12.Add(new[] { new SpreadsheetCell(string.Empty)});

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    spreadsheet12.Add(new[] {new SpreadsheetCell("Homodimers - Amino Acid Heat Map - Difference between homodimer composition and homodimer interactions - Acid Groups " + enumAminoAcidGroups)}); // Section title.
                    spreadsheet12.AddRange(AminoAcidPairCompositionMatrix.DifferenceWithCompositionMatrix(wholeProteinAminoAcidToAminoAcidCounterPercentage1X1, spreadsheetTaskResult.InteractionsAminoAcidToAminoAcidCounter).SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
                    spreadsheet12.Add(new[] {new SpreadsheetCell(string.Empty)});
                }

                results.Add(spreadsheet12);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            return results;
        }

        /// <summary>
        ///     This method iterates through the provided FASTA files creating separate calculated outputs for each of them.
        /// </summary>
        /// <param name="fastaFiles">The FASTA files to process.</param>
        /// <param name="pdbFilesFolders">The locations where PDB files may be found.</param>
        /// <param name="spreadsheetSaveFilenameTemplate">A template filename to save the outputs.</param>
        /// <param name="saveTsv"></param>
        /// <param name="saveXl"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progressActionSet"></param>
        /// <param name="fileExistsOptions"></param>
        public static void MakeHomodimerStatisticsSpreadsheetsAndOutputFiles(decimal maxAtomInterationDistance, string[] fastaFiles, string[] pdbFilesFolders, string spreadsheetSaveFilenameTemplate, bool saveTsv, bool saveXl, CancellationToken cancellationToken, ProgressActionSet progressActionSet = null, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            if (fastaFiles == null || fastaFiles.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fastaFiles));
            }

            if (pdbFilesFolders == null || pdbFilesFolders.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilesFolders));
            }

            if (!saveTsv && !saveXl)
            {
                throw new ArgumentOutOfRangeException(nameof(saveTsv));
            }

            for (int fastaFileNumber = 0; fastaFileNumber < fastaFiles.Length; fastaFileNumber++)
            {
                string fastaFilename = fastaFiles[fastaFileNumber];

                if (string.IsNullOrWhiteSpace(fastaFilename))
                {
                    continue;    
                }

                ProgressActionSet.Report("Attempting to open file: " + fastaFilename, progressActionSet);
                
                List<ISequence> sequences = SequenceFileHandler.LoadSequenceFile(fastaFilename, StaticValues.MolNameProteinAcceptedValues);

                var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequences);

                if ((sequences == null) || (sequences.Count == 0))
                {

                    ProgressActionSet.Report("Error could not load file: " + fastaFilename, progressActionSet);
                    continue;
                }
                ProgressActionSet.Report("Loaded " + sequences.Count + " sequences from file: " + fastaFilename, progressActionSet);

                List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequences);
                string appendFilename = FileAndPathMethods.FullPathToFilename(fastaFilename);

                ProgressActionSet.Report("Creating spreadsheets...", progressActionSet);
                Stopwatch stopwatch = Stopwatch.StartNew();
                var spreadsheetList = MakeHomodimerStatisticsSpreadsheetsList(cancellationToken, maxAtomInterationDistance, pdbFilesFolders, pdbIdList, pdbIdChainIdList, progressActionSet);
                stopwatch.Stop();
                ProgressActionSet.Report("Finished calculating spreadsheet data [Elapsed: " + stopwatch.Elapsed.ToString(@"dd\:hh\:mm\:ss") + "]", progressActionSet);

                if (cancellationToken.IsCancellationRequested)
                {
                    //UserProteinInterfaceOperations.ProgressBarReset(progressBar, 0, 100, 0);
                    ////UserProteinInterfaceOperations.LabelEstimatedTimeRemainingUpdate(estimatedTimeRemaining, 0, 1, 1);

                    ProgressActionSet.StartAction(100, progressActionSet);
                    ProgressActionSet.ProgressAction(100, progressActionSet);
                    ProgressActionSet.FinishAction(false, progressActionSet);
                    ProgressActionSet.Report("Cancelled.", progressActionSet);
                    break;
                }

                
                for (int spreadsheetIndex = 0; spreadsheetIndex < spreadsheetList.Count; spreadsheetIndex++)
                {
                    var spreadsheet = spreadsheetList[spreadsheetIndex];

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    

                    // Remove the first row (which has the name for use in a worksheet title, not currently used)
                    var sheetName = spreadsheet[0][0].CellData;
                    var spreadsheetName = spreadsheet[1][0].CellData;
                    spreadsheet.RemoveAt(0);

                    // "c:/dResults/Results - %date% %time% - %fasta_filename% - %spreadsheet_name%.tsv"
                    string saveFilename = spreadsheetSaveFilenameTemplate;

                    saveFilename = saveFilename.Replace("%spreadsheet_name%", spreadsheetName);
                    saveFilename = saveFilename.Replace("%fasta_filename%", appendFilename);
                    saveFilename = saveFilename.Replace("%date%", DateTime.Now.ToString("yyyy-MM-dd"));
                    saveFilename = saveFilename.Replace("%time%", DateTime.Now.ToString("HH.mm.ss"));
                    saveFilename = saveFilename.Replace("%batch_number%", "");//string.Empty + (fastaFileNumber + 1));
                    saveFilename = saveFilename.Replace("%batch_letter%", "");//SpreadsheetFileHandler.AlphabetLetterRollOver(spreadsheetIndex));

                    sheetName = sheetName.Replace("%spreadsheet_name%", spreadsheetName);
                    sheetName = sheetName.Replace("%fasta_filename%", appendFilename);
                    sheetName = sheetName.Replace("%date%", DateTime.Now.ToString("yyyy-MM-dd"));
                    sheetName = sheetName.Replace("%time%", DateTime.Now.ToString("HH.mm.ss"));
                    sheetName = sheetName.Replace("%batch_number%", "");//string.Empty + (fastaFileNumber + 1));
                    sheetName = sheetName.Replace("%batch_letter%", "");//SpreadsheetFileHandler.AlphabetLetterRollOver(spreadsheetIndex));


                    //var tsvFilename = new FileInfo(FileAndPathMethods.RemoveFileExtension(saveFilename) + ".tsv");

                    var xlFilename = new FileInfo(FileAndPathMethods.RemoveFileExtension(saveFilename) + ".xlsx");

                    var savedFiles = SpreadsheetFileHandler.SaveSpreadsheet(xlFilename.FullName, new[] {sheetName}, spreadsheet, null, saveTsv, saveXl, fileExistsOptions);

                    ProgressActionSet.ReportFilesSaved(savedFiles, progressActionSet);
                }
            }

            ProgressActionSet.Report("Finished processing files.", progressActionSet);
        }
    }
}