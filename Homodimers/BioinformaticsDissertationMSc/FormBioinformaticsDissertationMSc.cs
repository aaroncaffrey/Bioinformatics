//-----------------------------------------------------------------------
// <copyright file="FormBioinformaticsDissertation.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bio;
using BioinformaticsDissertationMSc.Properties;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.Dssp;
using BioinformaticsHelperLibrary.InteractionDetection;
using BioinformaticsHelperLibrary.InteractionVector;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProproteinInterface;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.UserProteinInterface;
using BioinformaticsHelperLibrary.Measurements;
using System.Diagnostics;
using BioinformaticsHelperLibrary.TypeConversions;

namespace BioinformaticsDissertationMSc
{
    /// <summary>
    ///     The main user ProteinInterface code.
    /// </summary>
    public partial class FormBioinformaticsDissertationMSc : Form
    {
        /// <summary>
        ///     A list of child tasks.
        /// </summary>
        private readonly List<Task> _taskList = new List<Task>();

        /// <summary>
        ///     Cancellation token source for child threads.
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();


        public FormBioinformaticsDissertationMSc()
        {
            InitializeComponent();
        }

        public void StartAction(int totalItems)
        {
            UserProteinInterfaceOperations.ProgressBarReset(progressBarGeneral, 0, totalItems, 0);
        }

        public void ProgressAction(int updateValue)
        {
            UserProteinInterfaceOperations.ProgressBarIncrement(progressBarGeneral, updateValue);
        }

        public void FinishAction(bool success)
        {
            UserProteinInterfaceOperations.ProgressBarIncrement(progressBarGeneral, progressBarGeneral.Maximum - progressBarGeneral.Value);
        }

        public void StatusTextAction(string message)
        {
            UserProteinInterfaceOperations.TextBoxAppendLine(textBoxUserFeedback, message);
        }

        public void EstimatedTimeRemainingAction(long startTicks, long itemsCompleted, long itemsTotal)
        {
            UserProteinInterfaceOperations.LabelEstimatedTimeRemainingUpdate(labelEstimatedTimeRemaining, startTicks, itemsCompleted, itemsTotal);
        }

        /// <summary>
        ///     Enable or disable behaviour triggering controls on the user proteinInterface.
        /// </summary>
        /// <param name="enabledValue">True to enable.  False to disable.</param>
        public void SetControlsEnabledProperty(bool enabledValue)
        {
            var doNotDisable = new Control[]
            {
                this,
                textBoxUserFeedback,
                progressBarGeneral,
                labelEstimatedTimeRemaining,
                buttonCancelCalculateOutputSpreadsheets,
                buttonCancelClusteringAlgorithm,
                buttonCancelFilterFastaFiles,
                buttonCancelShowInteractions,
                tabControlOperationSelection,
                tabPageClustering,
                tabPageFastaToSpreadsheet,
                tabPageFilterFastaFiles,
                tabPagePdbToSpreadsheet,
                tabPageShowInteractions,
                tabPageSpreadsheets,
                tabPageUniProt,
                splitContainer1,
                splitContainer2,
                splitContainer1.Panel1,
                splitContainer1.Panel2,
                splitContainer2.Panel1,
                splitContainer2.Panel2
            };

            foreach (Control control in Controls)
            {
                //if (!doNotDisable.Contains(control))
                {
                    UserProteinInterfaceOperations.SetControlsEnabledProperty(control, doNotDisable, enabledValue);
                }
            }
        }

        /// <summary>
        ///     Starts the filtering task when the filter button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDoFilterFastaFiles_Click(object sender, EventArgs e)
        {
            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;

            SetControlsEnabledProperty(false);

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            CleanProteinHandler.ProteinOperation proteinOperationOptionFlags =
                (checkBoxFilterNonProteinAlphabetEntries.Checked ? CleanProteinHandler.ProteinOperation.RemoveNonProteinAlphabetInSequence : CleanProteinHandler.ProteinOperation.None) |
                (checkBoxFilterEntriesWithoutTwoChainsInSequence.Checked ? CleanProteinHandler.ProteinOperation.RemoveWrongNumberOfChainsInSequence : CleanProteinHandler.ProteinOperation.None) |
                (checkBoxFilterDuplicateEntries.Checked ? CleanProteinHandler.ProteinOperation.RemoveExactDuplicatesInSequence : CleanProteinHandler.ProteinOperation.None) |
                (checkBoxFilterNonHomodimerEntries.Checked ? CleanProteinHandler.ProteinOperation.RemoveNonHomodimersInSequence : CleanProteinHandler.ProteinOperation.None) |
                (checkBoxFilterMultipleModels.Checked ? CleanProteinHandler.ProteinOperation.RemoveMultipleModelsInStructure : CleanProteinHandler.ProteinOperation.None) |
                (checkBoxFilterEntriesWithoutTwoChainsInStructure.Checked ? CleanProteinHandler.ProteinOperation.RemoveWrongNumberOfChainsInStructure : CleanProteinHandler.ProteinOperation.None) |
                (checkBoxFilterNonInteractingEntries.Checked ? CleanProteinHandler.ProteinOperation.RemoveNonInteractingProteinsInStructure : CleanProteinHandler.ProteinOperation.None) |
                (checkBoxFilterNonSymmetricalEntries.Checked ? CleanProteinHandler.ProteinOperation.RemoveNonSymmetricalInStructure : CleanProteinHandler.ProteinOperation.None);

            string saveFilename = comboBoxOutputFilterFastaFilesSaveFilename.Text;

            //_cancellationTokenSource = new CancellationTokenSource();
            Task task = Task.Run(() => CleanProteinHandler.CleanProteins(_cancellationTokenSource.Token, maxAtomInterationDistance, textBoxInputPdbFilesFolders.Lines, textBoxInputFastaFiles.Lines, proteinOperationOptionFlags, saveFilename, progressActionSet), _cancellationTokenSource.Token);
            Task task2 = task.ContinueWith(continueTask => SetControlsEnabledProperty(true), _cancellationTokenSource.Token);
            _taskList.Add(task);
            _taskList.Add(task2);
        }

        /// <summary>
        ///     Shows an open file dialog to select fasta files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectInputFastaFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialogFasta.ShowDialog() == DialogResult.OK)
            {
                textBoxInputFastaFiles.Lines = openFileDialogFasta.FileNames;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDoCalculateOutputSpreadsheets_Click(object sender, EventArgs e)
        {
            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;

            SetControlsEnabledProperty(false);

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);


            string saveFilename = comboBoxOutputSaveSpreadsheetsFilename.Text;

            //_cancellationTokenSource = new CancellationTokenSource();

            Task task = Task.Run(() => HomodimerStatisticsMiner.MakeHomodimerStatisticsSpreadsheetsAndOutputFiles(maxAtomInterationDistance,
                textBoxInputFastaFiles.Lines,
                textBoxInputPdbFilesFolders.Lines,
                saveFilename,
                false,
                true,
                _cancellationTokenSource.Token,
                progressActionSet), _cancellationTokenSource.Token);

            Task task2 = task.ContinueWith(t => SetControlsEnabledProperty(true), _cancellationTokenSource.Token);
            _taskList.Add(task);
            _taskList.Add(task2);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectInputPdbFilesFolders_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogPdbFilesFolder.ShowDialog() == DialogResult.OK)
            {
                textBoxInputPdbFilesFolders.Text = folderBrowserDialogPdbFilesFolder.SelectedPath.Replace('\\', '/');
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectFilterFastaFilesSaveFilename_Click(object sender, EventArgs e)
        {
            if (openFileDialogFasta.ShowDialog() == DialogResult.OK)
            {
                comboBoxOutputFilterFastaFilesSaveFilename.Text = openFileDialogFasta.FileName.Replace('\\', '/');
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectOutputSpreadsheetSaveFilename_Click(object sender, EventArgs e)
        {
            if (saveFileDialogSpreadsheet.ShowDialog() == DialogResult.OK)
            {
                comboBoxOutputSaveSpreadsheetsFilename.Text = saveFileDialogSpreadsheet.FileName.Replace('\\', '/');
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDoClusteringAlgorithm_Click(object sender, EventArgs e)
        {
            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;

            SetControlsEnabledProperty(false);

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            string proteinInterfaceDataFilename = comboBoxOutputClusteringProteinInterfaceDataOutputFilename.Text;

            string densityAllFilename = comboBoxOutputClusteringDensityAllFilename.Text;

            string matlabVizualizationFilenameTemplate = comboBoxOutputClusteringMatlabVisualizationFilenameTemplate.Text; // matlabVizualizationFilenameTemplate.Text;
            string interactionModesFilename = comboBoxOutputClusteringInteractionModesFilename.Text;
            string proteinInterfaceInteractionListFilename = comboBoxOutputClusteringProteinInterfaceInteractionListFilename.Text;

            //_cancellationTokenSource = new CancellationTokenSource();
            Task task = Task.Run(() =>
            {
                Clustering.ClusterAllForProteinInterfaceAndSymmetryDetection(maxAtomInterationDistance, minimumProteinInterfaceDensity, textBoxInputFastaFiles.Lines, textBoxInputPdbFilesFolders.Lines, matlabVizualizationFilenameTemplate, proteinInterfaceInteractionListFilename, proteinInterfaceDataFilename, densityAllFilename, interactionModesFilename, ClusteringMethodOptions.ClusterWithResidueSequenceIndex, _cancellationTokenSource.Token,
                    totalThreads: -1,
                    maximumGroupSize: -1,
                    maximumDistance: -1,
                    progressActionSet: progressActionSet);
                SetControlsEnabledProperty(true);
            }, _cancellationTokenSource.Token);
            _taskList.Add(task);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancelClusteringAlgorithm_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
            _taskList.Clear();
            SetControlsEnabledProperty(true);
        }

        private void buttonCancelCalculateOutputSpreadsheets_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
            _taskList.Clear();

            SetControlsEnabledProperty(true);
        }


        private void buttonCancelFilterFastaFiles_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
            _taskList.Clear();
            SetControlsEnabledProperty(true);
        }


        private void buttonDoSaveUniProtSpreadsheet_Click(object sender, EventArgs e)
        {
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            string saveFilename = comboBoxUniprotSpreadsheetSaveFilename.Text;

            UniProtProteinDatabaseComposition.SaveUniProtSpreadsheet(saveFilename, progressActionSet);
        }

        private void buttonDoShowPdbInteractions_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);

            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;

            bool outputToFile = checkBoxShowInteractionsOutputToFile.Checked;
            bool outputToGui = checkBoxShowInteractionsOutputToGUI.Checked;

            string outputFilename = textBoxShowInteractionsOutputFilename.Text;

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            //_cancellationTokenSource = new CancellationTokenSource();

            Task task = Task.Run(() =>
            {
                try
                {

                    var fastaFiles = textBoxInputFastaFiles.Lines.Where(a => !String.IsNullOrWhiteSpace(a) && File.Exists(a)).ToArray();
                    //var pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;
                    var sequenceList = SequenceFileHandler.LoadSequenceFileList(fastaFiles, StaticValues.MolNameProteinAcceptedValues);
                    //var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);
                    //var pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders);
                    var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequenceList);

                    var pdbFileList =
                        textBoxShowInteractionsPdbFileList.Lines.Where(a => !String.IsNullOrWhiteSpace(a) && File.Exists(a)).ToArray();

                    var interactionStrings = InteractionsOutput.MakeInteractionsOutput(_cancellationTokenSource.Token, maxAtomInterationDistance, pdbFileList, pdbIdChainIdList, progressActionSet, outputToGui);

                    if (outputToFile)
                    {
                        var actualSavedFilename = InteractionsOutput.SaveInteractionsOutput(outputFilename, interactionStrings);
                        if (!string.IsNullOrWhiteSpace(actualSavedFilename))
                        {
                            ProgressActionSet.Report("Saved: " + actualSavedFilename, progressActionSet);
                        }
                        else
                        {
                            ProgressActionSet.Report("Could not save: " + outputFilename, progressActionSet);
                        }
                    }

                }
                finally
                {
                    SetControlsEnabledProperty(true);
                }
            }, _cancellationTokenSource.Token);
            _taskList.Add(task);

            //Task task2 = task.ContinueWith(t => SetControlsEnabledProperty(true), _cancellationTokenSource.Token);
            //tasks.Add(task2);
        }

        private void buttonSelectShowInteractionsInputPdbFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialogShowInteractionsSelectInputPdbFiles.ShowDialog() == DialogResult.OK)
            {
                textBoxShowInteractionsPdbFileList.Lines = openFileDialogShowInteractionsSelectInputPdbFiles.FileNames;
            }
        }

        private void buttonSelectConvertFastaFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialogFasta.ShowDialog() == DialogResult.OK)
            {
                textBoxConvertFastaToSpreadsheetInputFastaFiles.Lines = openFileDialogFasta.FileNames;
            }
        }

        private void buttonDoConvertFastaToSpreadsheet_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);

            UserProteinInterfaceOperations.ProgressBarReset(progressBarGeneral, 0, textBoxConvertFastaToSpreadsheetInputFastaFiles.Lines.Length, 0);
            bool xlsxFormat = checkBoxConvertPdbXLSX.Checked;
            bool tsvFormat = checkBoxConvertPdbTSV.Checked;

            foreach (string fastaFilename in textBoxConvertFastaToSpreadsheetInputFastaFiles.Lines)
            {
                List<ISequence> sequences = SequenceFileHandler.LoadSequenceFile(fastaFilename, StaticValues.MolNameProteinAcceptedValues);

                UserProteinInterfaceOperations.TextBoxAppendLine(textBoxUserFeedback, "Converting: " + fastaFilename, true);
                string[] savedFiles = SequenceFileHandler.SaveSequencesAsSpreadsheet(sequences, fastaFilename, tsvFormat, xlsxFormat);

                foreach (string savedFile in savedFiles)
                {
                    UserProteinInterfaceOperations.TextBoxAppendLine(textBoxUserFeedback, "Saved: " + savedFile, true);
                }

                UserProteinInterfaceOperations.ProgressBarIncrement(progressBarGeneral, 1);
            }

            SetControlsEnabledProperty(true);
        }

        private void buttonSelectConvertPdbFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialogShowInteractionsSelectInputPdbFiles.ShowDialog() == DialogResult.OK)
            {
                textBoxConvertPdbToSpreadsheetInputPdbFiles.Lines = openFileDialogShowInteractionsSelectInputPdbFiles.FileNames;
            }
        }

        private void buttonDoConvertPdbToSpreadsheet_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            UserProteinInterfaceOperations.ProgressBarReset(progressBarGeneral, 0, textBoxConvertFastaToSpreadsheetInputFastaFiles.Lines.Length, 0);

            bool xlsxFormat = checkBoxConvertPdbXLSX.Checked;
            bool tsvFormat = checkBoxConvertPdbTSV.Checked;


            foreach (string pdbFilename in textBoxConvertPdbToSpreadsheetInputPdbFiles.Lines)
            {
                string pdbId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                var proteinDataBankFile = new ProteinDataBankFile(pdbFilename);

                UserProteinInterfaceOperations.TextBoxAppendLine(textBoxUserFeedback, "Converting: " + pdbFilename, true);


                string[] savedFiles = SpreadsheetFileHandler.SaveSpreadsheet(pdbFilename, null, proteinDataBankFile.Lines.Select(a => a.Select(b => new SpreadsheetCell(b)).ToList()).ToList(), progressActionSet, tsvFormat, xlsxFormat);

                foreach (string savedFile in savedFiles)
                {
                    UserProteinInterfaceOperations.TextBoxAppendLine(textBoxUserFeedback, "Saved: " + savedFile, true);
                }

                UserProteinInterfaceOperations.ProgressBarIncrement(progressBarGeneral, 1);
            }

            SetControlsEnabledProperty(true);
        }

        private void buttonShowInteractionsGetPdbFilesFromPdbFilesFolders_Click(object sender, EventArgs e)
        {
            textBoxShowInteractionsPdbFileList.Lines = ProteinDataBankFileOperations.GetPdbFilesArray(textBoxInputPdbFilesFolders.Lines);
        }

        private void buttonConverPdbGetPdbFilesFromPdbFilesFolders_Click(object sender, EventArgs e)
        {
            textBoxConvertPdbToSpreadsheetInputPdbFiles.Lines = ProteinDataBankFileOperations.GetPdbFilesArray(textBoxInputPdbFilesFolders.Lines);
        }

        private void buttonConvertFastaGetFastaFilesFromFastaFilesInputTextBox_Click(object sender, EventArgs e)
        {
            textBoxConvertFastaToSpreadsheetInputFastaFiles.Lines = textBoxInputFastaFiles.Lines;
        }

        private void buttonCancelShowInteractions_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
            SetControlsEnabledProperty(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkBoxFilterNonProteinAlphabetEntries.Checked = true;
            checkBoxFilterEntriesWithoutTwoChainsInSequence.Checked = true;
            checkBoxFilterDuplicateEntries.Checked = true;
            checkBoxFilterNonHomodimerEntries.Checked = true;
        }

        private void buttonSelectNoneSequenceFilters_Click(object sender, EventArgs e)
        {
            checkBoxFilterNonProteinAlphabetEntries.Checked = false;
            checkBoxFilterEntriesWithoutTwoChainsInSequence.Checked = false;
            checkBoxFilterDuplicateEntries.Checked = false;
            checkBoxFilterNonHomodimerEntries.Checked = false;
        }

        private void buttonSelectAllStrcutureFilters_Click(object sender, EventArgs e)
        {
            checkBoxFilterEntriesWithoutTwoChainsInStructure.Checked = true;
            checkBoxFilterMultipleModels.Checked = true;
            checkBoxFilterNonInteractingEntries.Checked = true;
            checkBoxFilterNonSymmetricalEntries.Checked = true;
        }

        private void buttonSelectNoneStructureFilters_Click(object sender, EventArgs e)
        {
            checkBoxFilterEntriesWithoutTwoChainsInStructure.Checked = false;
            checkBoxFilterMultipleModels.Checked = false;
            checkBoxFilterNonInteractingEntries.Checked = false;
            checkBoxFilterNonSymmetricalEntries.Checked = false;
        }

        private void FormBioinformaticsDissertationMSc_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            var fastaFiles = textBoxInputFastaFiles.Lines;
            var pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;
            var sequenceList = SequenceFileHandler.LoadSequenceFileList(fastaFiles, StaticValues.MolNameProteinAcceptedValues);
            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);
            var pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders);

            ProteinDataBankFileOperations.ShowMissingPdbFiles(pdbFilesArray, pdbIdList, progressActionSet, true, false);
            SetControlsEnabledProperty(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            var fastaFiles = textBoxInputFastaFiles.Lines;
            var pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;
            var sequenceList = SequenceFileHandler.LoadSequenceFileList(fastaFiles, StaticValues.MolNameProteinAcceptedValues);
            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);
            var pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders);

            ProteinDataBankFileOperations.ShowMissingPdbFiles(pdbFilesArray, pdbIdList, progressActionSet, false, true);
            SetControlsEnabledProperty(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            var pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;
            var pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders, false);
            var pdbFilesPdbIdList = pdbFilesArray.Select(ProteinDataBankFileOperations.PdbIdFromPdbFilename).ToList();
            var pdbFilesPdbIdListDuplicates = pdbFilesPdbIdList.Where((id, index) => pdbFilesPdbIdList.Count(a => a == id) > 1).Distinct().ToList();

            if (pdbFilesPdbIdListDuplicates.Count > 0)
            {
                var d = string.Join(", ", pdbFilesPdbIdListDuplicates);
                ProgressActionSet.Report("Duplicate PDB Files [" + pdbFilesPdbIdListDuplicates.Count + "]: " + d, progressActionSet);
            }
            else
            {
                ProgressActionSet.Report("No duplicate PDB Files were found.", progressActionSet);
            }
            SetControlsEnabledProperty(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            ProgressActionSet.Report("Comparing fasta files for differences in record unique ids", progressActionSet);

            var fastaFiles = textBoxInputFastaFiles.Lines.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
            var sequenceList = new List<ISequence>[fastaFiles.Length];
            var pdbIdList = new List<string>[fastaFiles.Length];

            for (int index = 0; index < fastaFiles.Length; index++)
            {
                sequenceList[index] = SequenceFileHandler.LoadSequenceFile(fastaFiles[index], StaticValues.MolNameProteinAcceptedValues);
                pdbIdList[index] = FilterProteins.SequenceListToPdbIdList(sequenceList[index]);
            }

            for (int index = 0; index < fastaFiles.Length; index++)
            {
                ProgressActionSet.Report("Comparing " + fastaFiles[index] + " [" + sequenceList[index].Count + " sequences / " + pdbIdList[index].Count + " entries]", progressActionSet);

                for (int index2 = 0; index2 < fastaFiles.Length; index2++)
                {
                    if (index == index2) continue;

                    var diff = pdbIdList[index].Where(a => !pdbIdList[index2].Contains(a)).ToList();

                    ProgressActionSet.Report("- " + fastaFiles[index2] + (diff.Count > 0 ? " does not have: " + string.Join(", ", diff) : " has all entries present in file"), progressActionSet);
                }

                ProgressActionSet.Report("", progressActionSet);
            }

            ProgressActionSet.Report("Finished comparing fasta files", progressActionSet);

            SetControlsEnabledProperty(true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            ProgressActionSet.Report("Showing distinct list of pdbids for each fasta file", progressActionSet);

            var fastaFiles = textBoxInputFastaFiles.Lines.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
            var sequenceList = new List<ISequence>[fastaFiles.Length];
            var pdbIdList = new List<string>[fastaFiles.Length];

            for (int index = 0; index < fastaFiles.Length; index++)
            {
                sequenceList[index] = SequenceFileHandler.LoadSequenceFile(fastaFiles[index], StaticValues.MolNameProteinAcceptedValues);
                pdbIdList[index] = FilterProteins.SequenceListToPdbIdList(sequenceList[index], true).OrderBy(a => a).ToList();


                ProgressActionSet.Report(fastaFiles[index] + ":  " + String.Join(" ", pdbIdList[index]), progressActionSet);
            }

            ProgressActionSet.Report("Finished listing pdbids", progressActionSet);

            SetControlsEnabledProperty(true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            ProgressActionSet.Report("Showing distinct list of pdbids for each fasta file", progressActionSet);

            var info = File.ReadAllLines(@"c:\d\render dl info.csv");
            //Pdb Id, Chain Id,Dir,Seq Len, BS Len,BS Start, BS End,BS All SS,BS Seq 1L

            var infoDictionary = new Dictionary<string, string[]>();
            for (int index = 0; index < info.Length; index++)
            {
                if (index == 0) continue;

                var line = info[index];

                var cols = line.Split(new char[] { ',' }, StringSplitOptions.None);

                if (cols.All(string.IsNullOrWhiteSpace)) continue;

                infoDictionary.Add(cols[0] + "-" + cols[1], cols);
            }

            var fastaFiles = textBoxInputFastaFiles.Lines.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
            var sequenceList = new List<ISequence>[fastaFiles.Length];
            var pdbIdList = new List<string>[fastaFiles.Length];

            StringBuilder html = new StringBuilder();

            for (int index = 0; index < fastaFiles.Length; index++)
            {
                sequenceList[index] = SequenceFileHandler.LoadSequenceFile(fastaFiles[index], StaticValues.MolNameProteinAcceptedValues);
                pdbIdList[index] = FilterProteins.SequenceListToPdbIdList(sequenceList[index], true).OrderBy(a => a).ToList();
                html.Append("<table>" + Environment.NewLine);
                foreach (var pdbId in pdbIdList[index])
                {

                    var colsA = infoDictionary[pdbId + "-A"];
                    var colsB = infoDictionary[pdbId + "-B"];

                    //var pdbIdA = cols[0];
                    //var chainIdA = colsA[1];
                    //var dirA = colsA[2];
                    var seqLenA = colsA[3];
                    var bsLenA = colsA[4];
                    var bsStartA = colsA[5];
                    var bsEndA = colsA[6];
                    var bsSsA = colsA[7];
                    var bsAaA = colsA[8];

                    //var chainIdB = colsB[1];
                    //var dirB = colsB[2];
                    var seqLenB = colsB[3];
                    var bsLenB = colsB[4];
                    var bsStartB = colsB[5];
                    var bsEndB = colsB[6];
                    var bsSsB = colsB[7];
                    var bsAaB = colsB[8];

                    var infoA = "Chain: A (Length: " + seqLenA + ")" +
                                "<br/>ProteinInterface RI: " + bsLenA + "(" + bsStartA + ".." + bsEndA + ")" +
                                "<br/>ProteinInterface AA: " + bsAaA +
                                "<br/>ProteinInterface SS: " + bsSsA;

                    var infoB = "Chain: B (Length: " + seqLenB + ")" +
                                "<br/>ProteinInterface RI: " + bsLenB + "(" + bsStartB + ".." + bsEndB + ")" +
                                "<br/>ProteinInterface AA: " + bsAaB +
                                "<br/>ProteinInterface SS: " + bsSsB;


                    html.Append("<tr>" + Environment.NewLine);
                    html.Append(" <td>" + pdbId + "<br />" + infoA + "</td>" + Environment.NewLine);
                    html.Append(" <td>" + pdbId + "<br />" + infoB + "</td>" + Environment.NewLine);
                    html.Append(" <td><img src=\"http://www.ebi.ac.uk/pdbe/static/entry/" + pdbId.ToLowerInvariant() + "_depoproteinInterfaced_chain_front_image-800x800.png" + "\" width=\"100px\" /></td>" + Environment.NewLine);
                    html.Append(" <td><img src=\"http://www.ebi.ac.uk/pdbe/static/entry/" + pdbId.ToLowerInvariant() + "_depoproteinInterfaced_chain_side_image-800x800.png" + "\" width=\"100px\" /></td>" + Environment.NewLine);
                    html.Append(" <td><img src=\"http://www.ebi.ac.uk/pdbe/static/entry/" + pdbId.ToLowerInvariant() + "_depoproteinInterfaced_chain_top_image-800x800.png" + "\" width=\"100px\" /></td>" + Environment.NewLine);
                    html.Append(" <td><img src=\"http://www.ebi.ac.uk/pdbe/static/entry/" + pdbId.ToLowerInvariant() + "_depoproteinInterfaced_chemically_distinct_molecules_front_image-800x800.png" + "\" width=\"100px\" /></td>" + Environment.NewLine);
                    html.Append(" <td><img src=\"http://www.ebi.ac.uk/pdbe/static/entry/" + pdbId.ToLowerInvariant() + "_depoproteinInterfaced_chemically_distinct_molecules_side_image-800x800.png" + "\" width=\"100px\" /></td>" + Environment.NewLine);
                    html.Append(" <td><img src=\"http://www.ebi.ac.uk/pdbe/static/entry/" + pdbId.ToLowerInvariant() + "_depoproteinInterfaced_chemically_distinct_molecules_top_image-800x800.png" + "\" width=\"100px\" /></td>" + Environment.NewLine);
                    html.Append("</tr>" + Environment.NewLine);
                }
                html.Append("</table>" + Environment.NewLine);
            }
            File.WriteAllText(@"c:\users\aaron\desktop\render.html", html.ToString());

            ProgressActionSet.Report("Finished listing pdbids", progressActionSet);

            SetControlsEnabledProperty(true);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;
            // 1. get vectors
            // 2. get motifs
            // 3. query proproteinInterface
            // 4. store results

            SetControlsEnabledProperty(false);

            string outputFolderName = textBoxTreeAndVectorsOutputFolder.Text;

            var minimumOutputTreeLeafs = Convert.ToInt32(numericUpDownMinimumTreeLeafs.Text);

            var minimumIsPercentage = radioButtonMinimumLeafPercentageOfSet.Checked;

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            string[] fastaFiles = textBoxInputFastaFiles.Lines;

            string[] pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;

            List<ISequence> sequenceList = SequenceFileHandler.LoadSequenceFileList(fastaFiles, StaticValues.MolNameProteinAcceptedValues);

            List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);

            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders));

            ProgressActionSet.Report(@"Task: Loading proteinInterfaces and calculating vectors", progressActionSet);
            List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList = VectorController.LoadProteinInterfaceVectorFromFiles(_cancellationTokenSource.Token, maxAtomInterationDistance, minimumProteinInterfaceDensity, fastaFiles, pdbFilesFolders, progressActionSet);

            var motifData = ProproteinInterfaceSpreadsheetRecord.MotifSpreadsheetData(vectorProteinInterfaceWholeList);
            var motifDistinctDict = MotifHitSpreadsheetRecord.MotifDistinctWithCount(motifData);

            bool verbose = false;

            ProgressActionSet.StartAction(motifDistinctDict.Count * 3, progressActionSet);
            var task = Task.Run(() =>
            {
                foreach (ScanProproteinInterfaceParameters.TargetProteinDatabases targetProteinDatabases in Enum.GetValues(typeof(ScanProproteinInterfaceParameters.TargetProteinDatabases)))
                {
                    if (targetProteinDatabases == ScanProproteinInterfaceParameters.TargetProteinDatabases.Default) continue;

                    var motifTasks = new List<Task>();
                    foreach (var motif in motifDistinctDict/*.Where(a=>!a.Value.MotifTooGeneral)*/.OrderBy(a => a.Value.TotalFwd + a.Value.TotalRev + a.Value.TotalMix))
                    {
                        var localMotif = motif;

                        var localTargetProteinDatabases = targetProteinDatabases;

                        var task2 = Task.Run(() =>
                        {
                            var scanProproteinInterfaceParameters = new ScanProproteinInterfaceParameters() { sig = localMotif.Value.Motif, db = localTargetProteinDatabases };

                            if (verbose)
                            {
                                ProgressActionSet.Report("Requesting: " + localMotif.Value.Motif, progressActionSet);
                            }

                            if (!ProproteinInterfaceServiceClient.IsProproteinInterfaceResponseCached(scanProproteinInterfaceParameters))
                            {
                                var o = ProproteinInterfaceServiceClient.LoadProproteinInterfaceResponse(scanProproteinInterfaceParameters);
                            }
                        });

                        motifTasks.Add(task2);

                        while (motifTasks.Count(t => !t.IsCompleted) >= 10)
                        {
                            Task.WaitAny(motifTasks.Where(t => !t.IsCompleted).ToArray());
                        }

                        ProgressActionSet.ProgressAction(1, progressActionSet);
                    }
                    Task.WaitAll(motifTasks.Where(t => !t.IsCompleted).ToArray());
                }

                SetControlsEnabledProperty(true);
            });

            _taskList.Add(task);
        }

        private void buttonCalculateProteinInterfaceVector_Click(object sender, EventArgs e)
        {
            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;

            SetControlsEnabledProperty(false);



            string outputFolderName = textBoxTreeAndVectorsOutputFolder.Text;

            var minimumOutputTreeLeafs = Convert.ToInt32(numericUpDownMinimumTreeLeafs.Text);

            var minimumIsPercentage = radioButtonMinimumLeafPercentageOfSet.Checked;

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            string[] fastaFiles = textBoxInputFastaFiles.Lines;



            List<ISequence> sequenceList = SequenceFileHandler.LoadSequenceFileList(fastaFiles, StaticValues.MolNameProteinAcceptedValues);

            List<string> pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);

            string[] pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;
            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders));




            //bool outputIndividualVectorSpreadsheet = true;
            bool outputProteinInterfaceAndTreeDataSpreadsheet = true;
            bool outputClutoDistanceMatrixes = false;
            bool outputDistanceMatrixSpreadsheets = false;
            bool outputNewickTrees = true;

            var vectorDistanceMeasurementValues = new VectorDistanceMeasurementValues
                (
                numericUpDownDistanceInteractionAndInteraction.Text,
                numericUpDownDistanceInteractionAndNonInteraction.Text,
                numericUpDownDistanceNonInteractionAndNonInteraction.Text,
                numericUpDownDistanceDifferentLengthProteinInterfaces.Text
                );

            FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.SelectFileExistsOption(radioButtonOverwriteFile3.Checked, radioButtonAppendNumberToFilename3.Checked, radioButtonSkipFile3.Checked);

            bool outputOptimistic = true;
            //bool outputPessimistic = false;

            Task task = Task.Run(() =>
            {
                ProgressActionSet.Report(@"Task: Loading proteinInterfaces and calculating vectors", progressActionSet);

                List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList = VectorController.LoadProteinInterfaceVectorFromFiles(_cancellationTokenSource.Token, maxAtomInterationDistance, minimumProteinInterfaceDensity, fastaFiles, pdbFilesFolders, progressActionSet);

                if (minimumIsPercentage)
                {
                    minimumOutputTreeLeafs = (int)(((double)minimumOutputTreeLeafs / (double)100) * (double)vectorProteinInterfaceWholeList.Count);
                }
                ProgressActionSet.Report(@"Info: minimum number of leaf is " + minimumOutputTreeLeafs + " / " + vectorProteinInterfaceWholeList.Count, progressActionSet);


                //if (outputIndividualVectorSpreadsheet)
                //{
                //    TreeOutputs.Report("Task: Making individual vector list", progressActionSet);
                //    string outputFilename = FileAndPathMethods.MergePathAndFilename(outputFolderName, "spreadsheets", "individual vector list");
                //    string[] savedFiles = VectorController.SaveIndividualVectorSpreadsheet(outputFilename, vectorProteinInterfaceWholeList, progressActionSet);
                //    TreeOutputs.ReportFilesSaved(savedFiles, progressActionSet);
                //}

                ProgressActionSet.Report(@"Task: Making optimistic distance matrixes", progressActionSet);
                Task.Delay(1000).Wait();

                double[,] optimisticDistanceMatrix;
                //double[,] pessimisticDistanceMatrix;
                VectorController.BestDistanceMatrixWithPartsAlignment(_cancellationTokenSource.Token, vectorProteinInterfaceWholeList, vectorDistanceMeasurementValues, out optimisticDistanceMatrix/*, out pessimisticDistanceMatrix*/, progressActionSet);

                if (outputDistanceMatrixSpreadsheets)
                {
                    if (outputOptimistic)
                    {
                        ProgressActionSet.Report(@"Task: Saving optimistic distance matrixes", progressActionSet);
                        Task.Delay(1000).Wait();

                        string outputFilenameOptimisticDistanceMatrix = FileAndPathMethods.MergePathAndFilename(outputFolderName, @"spreadsheets", @"distance matrix optimistic");
                        string[] savedFilesOptimisticDistanceMatrix = VectorController.SaveVectorDistanceMatrixSpreadsheet(outputFilenameOptimisticDistanceMatrix, vectorProteinInterfaceWholeList, optimisticDistanceMatrix);
                        ProgressActionSet.ReportFilesSaved(savedFilesOptimisticDistanceMatrix, progressActionSet);
                    }

                    //if (outputPessimistic)
                    //{
                    //    string outputFilenamePessimisticDistanceMatrix = FileAndPathMethods.MergePathAndFilename(outputFolderName, @"spreadsheets", @"distance matrix pessimistic");
                    //    string[] savedFilesPessimisticDistanceMatrix = VectorController.SaveVectorDistanceMatrixSpreadsheet(outputFilenamePessimisticDistanceMatrix, vectorProteinInterfaceWholeList, pessimisticDistanceMatrix);
                    //    ProgressActionSet.ReportFilesSaved(savedFilesPessimisticDistanceMatrix, progressActionSet);
                    //}
                }

                if (outputClutoDistanceMatrixes)
                {
                    ProgressActionSet.Report(@"Task: Saving distance matrixes and metadata in cluto format", progressActionSet);
                    Task.Delay(1000).Wait();

                    //TreeOutputs.OutputClutoFiles(outputFolderName, vectorProteinInterfaceWholeList, outputOptimistic ? optimisticDistanceMatrix : null, /*outputPessimistic ? pessimisticDistanceMatrix : null,*/ fileExistsOptions, progressActionSet);
                }


                if (outputNewickTrees)
                {
                    if (outputOptimistic)
                    {


                        ProgressActionSet.Report(@"Task: Making upgma optimistic newick trees", progressActionSet);
                        Task.Delay(1000).Wait();

                        const string subfolder = @"newick tree optimistic";

                        List<string> finalTreeLeafOrderList;
                        TreeOutputs.OutputTrees(vectorProteinInterfaceWholeList, optimisticDistanceMatrix, minimumOutputTreeLeafs, outputFolderName, subfolder, out finalTreeLeafOrderList, fileExistsOptions, progressActionSet);

                        if (outputProteinInterfaceAndTreeDataSpreadsheet)
                        {
                            ProgressActionSet.Report(@"Task: Making tree companion spreadsheets", progressActionSet);
                            Task.Delay(1000).Wait();

                            TreeOutputs.MakeTreeCompanionSpreadsheet(pdbFilesArray, pdbIdList, sequenceList, vectorProteinInterfaceWholeList, outputFolderName, @"tree data optimistic", finalTreeLeafOrderList, fileExistsOptions, progressActionSet);
                        }
                    }


                    //if (outputPessimistic)
                    //{
                    //    ProgressActionSet.Report(@"Task: Making upgma pessimistic newick trees", progressActionSet);
                    //    const string subfolder = @"newick tree pessimistic";
                    //
                    //    List<string> finalTreeLeafOrderList;
                    //    TreeOutputs.OutputTrees(vectorProteinInterfaceWholeList, pessimisticDistanceMatrix, minimumOutputTreeLeafs, outputFolderName, subfolder, out finalTreeLeafOrderList, fileExistsOptions, progressActionSet);
                    //
                    //    if (outputProteinInterfaceAndTreeDataSpreadsheet)
                    //    {
                    //        TreeOutputs.MakeTreeCompanionSpreadsheet(pdbFilesArray, pdbIdList, sequenceList, vectorProteinInterfaceWholeList, outputFolderName, @"tree data pessimistic", finalTreeLeafOrderList, fileExistsOptions, progressActionSet);
                    //    }
                    //}
                }
                ProgressActionSet.Report(@"Task: All tasks complete", progressActionSet);
                SetControlsEnabledProperty(true);
            }, _cancellationTokenSource.Token);

            _taskList.Add(task);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var aminoAcids = AminoAcidConversions.StandardAminoAcidsString();

            var propNames = new AminoAcidProperties<bool>().PropertyNames;

            var maxlen = propNames.Select(a => a.Length).Max();

            var tablen = 1;// maxlen + 5;

            var title = " ";
            foreach (var prop in propNames)
            {
                title += prop.PadLeft(tablen) + "\t";
            }



            textBoxUserFeedback.AppendText(title + Environment.NewLine);


            foreach (var aaCode in aminoAcids)
            {
                //var name = AminoAcidConversions.AminoAcidCodeToName(aaCode.ToString());

                var aa = AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(aaCode);

                var row = aaCode + "\t" + String.Join("", aa.PropertiesArray().Select(a => a ? "X".PadLeft(tablen) + "\t" : "".PadLeft(tablen) + "\t"));

                textBoxUserFeedback.AppendText(row + Environment.NewLine);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogOutput.ShowDialog() == DialogResult.OK)
            {
                textBoxFilterProteinInterfaceTotalsAndLengthsOutputFolder.Text = folderBrowserDialogOutput.SelectedPath;
            }
        }

        private void buttonFilterProteinInterfaceLengths_Click(object sender, EventArgs e)
        {
            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;

            SetControlsEnabledProperty(false);

            var filterProteinInterfaceCountWithoutLengths = checkBoxDistinctProteinInterfaceValues.Checked;
            var filterProteinInterfaceCountWithLengths = checkBoxDistinctProteinInterfaceValuesWithLengthValues.Checked;

            const string fileMask = "%proteinInterfaces%.fasta";

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            string[] fastaFiles = textBoxInputFastaFiles.Lines;

            string[] pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;

            string filterProteinInterfaceLengthsOutputFilename = FileAndPathMethods.MergePathAndFilename(textBoxFilterProteinInterfaceTotalsAndLengthsOutputFolder.Text, fileMask);

            FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.SelectFileExistsOption(radioButtonOverwriteFile2.Checked, radioButtonAppendNumberToFilename2.Checked, radioButtonSkipFile2.Checked);

            Task task = Task.Run(() => SequenceProteinInterfaceFilters.FilterProteinInterfaceLengths(_cancellationTokenSource.Token, maxAtomInterationDistance, minimumProteinInterfaceDensity, fastaFiles, pdbFilesFolders, filterProteinInterfaceLengthsOutputFilename, filterProteinInterfaceCountWithoutLengths, filterProteinInterfaceCountWithLengths, fileExistsOptions, progressActionSet), _cancellationTokenSource.Token);
            _taskList.Add(task);

            Task task2 = task.ContinueWith(continueTask => SetControlsEnabledProperty(true), _cancellationTokenSource.Token);
            _taskList.Add(task2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SetControlsEnabledProperty(false);
            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            ProgressActionSet.Report("Counting amino acids found in each fasta file", progressActionSet);

            var fastaFiles = textBoxInputFastaFiles.Lines.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
            var sequenceList = new List<ISequence>[fastaFiles.Length];
            var pdbIdList = new List<string>();

            var totalAminoAcids = 0L;
            var totalSequences = 0L;


            for (int index = 0; index < fastaFiles.Length; index++)
            {
                sequenceList[index] = SequenceFileHandler.LoadSequenceFile(fastaFiles[index], StaticValues.MolNameProteinAcceptedValues, false);
                //pdbIdList[index] = FilterProteins.SequenceListToPdbIdList(sequenceList[index], true).OrderBy(a => a).ToList();

                var localPdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList[index], true).OrderBy(a => a).ToList();


                totalSequences += sequenceList[index].Count;

                long aminoAcids = sequenceList[index].Select(a => a.Count).Sum();

                totalAminoAcids += aminoAcids;

                ProgressActionSet.Report(fastaFiles[index] + ":  " + aminoAcids + " sum total amino acids in " + localPdbIdList.Count + " entries split into " + sequenceList[index].Count + " sequences", progressActionSet);

                pdbIdList.AddRange(localPdbIdList.Where(a => !pdbIdList.Contains(a)).ToList());
            }

            ProgressActionSet.Report("overall:  " + totalAminoAcids + " sum total amino acids in " + pdbIdList.Count + " entries split into " + totalSequences + " sequences", progressActionSet);

            ProgressActionSet.Report("Finished counting amino acids", progressActionSet);

            SetControlsEnabledProperty(true);
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            long[] counter = new long[26];

            //var fasta = File.ReadAllLines(@"c:\d\msc results - 100% symmetrical - 1 to 1 proteinInterfaces per chain\newick tree optimistic\fasta\iteration [1] treeid [1] proteinInterfaces [4620].fasta");
            //var fasta = File.ReadAllLines(@"c:\d\msc results - 100% symmetrical - 1 to 1 proteinInterfaces per chain\newick tree optimistic\fasta\iteration [1] treeid [1] proteinInterfaces [4620].fasta");

            var fasta = File.ReadAllLines(@"c:\d\old - msc results - 100% symmetrical - 1 to 1 proteinInterfaces per chain\1 to 1 aa dist proteinInterface.txt");
            var protein = true;

            foreach (var line in fasta)
            {
                if (String.IsNullOrWhiteSpace(line)) continue;

                if (line[0] == '>')
                {
                    protein = line.Contains("mol:protein");
                    continue;
                }
                if (!protein) continue;

                foreach (var ch in line.ToUpperInvariant())
                {
                    if (ch >= 'A' && ch <= 'Z')
                    {
                        counter[ch - 'A']++;
                    }
                }
            }

            var order = @"LAGSVEITRDKPNFQYMHWCBJOUXZ";

            //counter = counter.Select((a, i) => counter[order.IndexOf(Convert.ToChar(i + 65))]).ToArray();
            counter = order.Select(aa => counter[aa - 65]).ToArray();
            for (int index = 0; index < counter.Length; index++)
            {
                textBoxUserFeedback.AppendText(order[index] + " " + counter[index] + "" + Environment.NewLine);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var fastaFile = @"c:\d\fasta - homo\2015-06-03 20.26.00 - 11 - dimers - with interactions - 100% symmetrical [8120 proteins - 16240 sequences].fasta";
            //var fastaFile = @"c:\d\fasta - homo\homo - 100% symmetrical\chains [2] proteinInterfaces [1 1] [1155 proteins - 2310 sequences].fasta";

            var sequenceList = SequenceFileHandler.LoadSequenceFile(fastaFile, StaticValues.MolNameProteinAcceptedValues, false);

            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList, true).OrderBy(a => a).ToList();

            string[] pdbFilesFolders = new[] { @"C:\pdb\" };
            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders));

            var sequenceSS = new StringBuilder();

            foreach (var file in pdbFilesArray)
            {
                //var dsspFile = file + ".dssp";

                //if (!File.Exists(dsspFile)) continue;


                var secondaryStructureStr = ProteinInterfaceSecondaryStructureLoader.ProteinInterfaceSecondaryStructure(file);//, proteinProteinInterface.ChainIdLetter);
                sequenceSS.Append(secondaryStructureStr);

                //foreach (var ss in secondaryStructure)
                //{
                //    if (!String.IsNullOrWhiteSpace(ss.FieldSecondaryStructure.FieldValue))
                //    {
                //        sequenceSS.Append(ss.FieldSecondaryStructure.FieldValue);
                //    }
                //    else
                //    {
                //        sequenceSS.Append("_");
                //    }
                //}
                sequenceSS.AppendLine();// += Environment.NewLine;

            }

            textBoxUserFeedback.AppendText(sequenceSS.ToString());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //long ssH = 0, ssB = 0, ssG = 0, ssT = 0, ss_ = 0, ssE = 0, ssS = 0, ssI =0;

            //var dataArray1 = File.ReadAllLines(@"C:\Users\aaron\Downloads\ss.txt\ss.txt");
            var dataArray1 = File.ReadAllLines(@"C:\Users\aaron\Desktop\1-1ssfull.txt");

            var dataArray = new List<string>();

            if (dataArray1.Any(a => a.Length > 0 && a[0] == '>'))
            {

                var inSS = false;

                var element = "";

                foreach (var line in dataArray1)
                {
                    if (String.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == '>')
                    {
                        inSS = line.Contains("secstr");
                        if (!String.IsNullOrWhiteSpace(element))
                        {
                            dataArray.Add(element);
                            element = "";
                        }
                        continue;
                    }

                    if (inSS)
                    {
                        element += line.Replace(" ", "_");
                    }

                }
                if (!String.IsNullOrWhiteSpace(element))
                {
                    dataArray.Add(element);
                    element = "";
                }
            }
            else
            {
                dataArray = dataArray1.ToList();
            }

            //var dataArray = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var ssTypeDistribution = new Dictionary<string, long>();
            var ssPrincipleTypeCount = new Dictionary<string, long> { { "H", 0 }, { "B", 0 }, { "G", 0 }, { "T", 0 }, { "_", 0 }, { "E", 0 }, { "S", 0 }, { "I", 0 } };
            var ssLongestTypeCount = new Dictionary<string, long> { { "H", 0 }, { "B", 0 }, { "G", 0 }, { "T", 0 }, { "_", 0 }, { "E", 0 }, { "S", 0 }, { "I", 0 } };

            long totalSumFreq = 0;
            long totalSecStructChars = 0;

            //var freqList = new List<long>();
            //foreach (var dataArrayLine in dataArray.Where(a => !String.IsNullOrWhiteSpace(a)))
            //{
            //    var dataLineSplit = dataArrayLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //    long secStructFreq = Convert.ToInt64(dataLineSplit[1]);
            //    if (!freqList.Contains(secStructFreq)) freqList.Add(secStructFreq);

            //}
            //freqList = freqList.OrderByDescending(a => a);
            foreach (var dataArrayLine in dataArray.Where(a => !String.IsNullOrWhiteSpace(a)))
            {
                var dataLineSplit = dataArrayLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (dataLineSplit.Length > 2) throw new Exception("Problems!");

                var secStructWord = dataLineSplit[0].Replace(' ', '_');

                long secStructFreq = dataLineSplit.Length > 1 ? Convert.ToInt64(dataLineSplit[1]) : 1;

                if (secStructFreq <= 0) throw new Exception("Problems!");

                var longestSsType = new Dictionary<char, long>();

                char? currentSsType = null;
                var currentSsTypeTotal = 0;

                for (int index = 0; index < secStructWord.Length; index++)
                {
                    var c = secStructWord[index];
                    if (currentSsType == null) currentSsType = c;

                    if (currentSsType == c)
                    {
                        currentSsTypeTotal++;
                    }

                    if (currentSsType != c || index == secStructWord.Length - 1)
                    {
                        var key2 = c != ' ' ? c : '_';

                        if (!longestSsType.ContainsKey(key2))
                        {
                            longestSsType.Add(key2, 0);
                        }

                        if (longestSsType[key2] < currentSsTypeTotal)
                        {
                            longestSsType[key2] = currentSsTypeTotal;
                        }
                        currentSsType = c;
                        currentSsTypeTotal = 1;
                    }

                    totalSumFreq += secStructFreq;

                    var key = "" + c;
                    if (!ssTypeDistribution.ContainsKey(key))
                    {
                        ssTypeDistribution.Add(key, secStructFreq);
                    }
                    else
                    {
                        ssTypeDistribution[key] += secStructFreq;
                    }
                }

                var maxSsTypeLength = longestSsType.Select(a => a.Value).Max();
                var maxSsType = longestSsType.Where(a => a.Value == maxSsTypeLength).ToList();
                if (maxSsType.Count == 1)
                {
                    var maxSsTypeChar = maxSsType[0].Key;
                    ssLongestTypeCount["" + maxSsTypeChar]++;
                }

                var word_h = secStructWord.Count(p => p == 'H');
                var word_b = secStructWord.Count(p => p == 'B');
                var word_g = secStructWord.Count(p => p == 'G');
                var word_t = secStructWord.Count(p => p == 'T');
                var word__ = secStructWord.Count(p => p == '_' || p == ' ');
                var word_e = secStructWord.Count(p => p == 'E');
                var word_s = secStructWord.Count(p => p == 'S');
                var word_i = secStructWord.Count(p => p == 'I');

                if (word_h > secStructWord.Length / 2) ssPrincipleTypeCount["H"]++;
                else if (word_b > secStructWord.Length / 2) ssPrincipleTypeCount["B"]++;
                else if (word_g > secStructWord.Length / 2) ssPrincipleTypeCount["G"]++;
                else if (word_t > secStructWord.Length / 2) ssPrincipleTypeCount["T"]++;
                else if (word__ > secStructWord.Length / 2) ssPrincipleTypeCount["_"]++;
                else if (word_e > secStructWord.Length / 2) ssPrincipleTypeCount["E"]++;
                else if (word_s > secStructWord.Length / 2) ssPrincipleTypeCount["S"]++;
                else if (word_i > secStructWord.Length / 2) ssPrincipleTypeCount["I"]++;
                else continue;

                totalSecStructChars++;
            }

            textBoxUserFeedback.AppendText("SS Type Distribution: " + totalSumFreq + Environment.NewLine);

            foreach (var kvp in ssTypeDistribution)
            {
                //var p = ((decimal) kvp.Value/(decimal) totalSumFreq);
                var p = "";

                var text = kvp.Key + "\t" + kvp.Value + "\t" + p;

                textBoxUserFeedback.AppendText(text + Environment.NewLine);
            }

            textBoxUserFeedback.AppendText(Environment.NewLine);
            textBoxUserFeedback.AppendText("Principle SS Type: " + totalSecStructChars + Environment.NewLine);

            foreach (var kvp in ssPrincipleTypeCount)
            {
                //var p = ((decimal) kvp.Value/(decimal) totalSecStructChars);
                var p = "";
                var text = kvp.Key + "\t" + kvp.Value + "\t" + p;

                textBoxUserFeedback.AppendText(text + Environment.NewLine);
            }

            textBoxUserFeedback.AppendText(Environment.NewLine);
            textBoxUserFeedback.AppendText("Longest SS Type: " + Environment.NewLine);

            foreach (var kvp in ssLongestTypeCount)
            {
                //var p = ((decimal) kvp.Value/(decimal) totalSecStructChars);
                var p = "";
                var text = kvp.Key + "\t" + kvp.Value + "\t" + p;

                textBoxUserFeedback.AppendText(text + Environment.NewLine);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var dataArray1 = File.ReadAllLines(@"C:\Users\aaron\Desktop\1-1ssfull.txt");

            for (int index = 0; index < dataArray1.Length; index++)
            {
                var line = dataArray1[index];
                if (String.IsNullOrWhiteSpace(line)) continue;


                var bestCount = new Dictionary<char, int>();

                char lastc = '\0';
                var totalc = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    var c = line[i];


                    if ((lastc != '\0' && c != lastc) || (i == line.Length - 1))
                    {
                        if (c == lastc && i == line.Length - 1) totalc++;

                        if (!bestCount.ContainsKey(lastc)) bestCount.Add(lastc, 0);

                        if (totalc > bestCount[lastc]) bestCount[lastc] = totalc;

                        totalc = 0;
                    }

                    totalc++;
                    lastc = c;
                }
                var max = bestCount.Select(b => b.Value).Max();
                var totalSame = bestCount.Count(a => a.Value == max);
                if (totalSame == 1)
                {
                    dataArray1[index] = "" + bestCount.Where(a => a.Value == max).Select(a => a.Key).FirstOrDefault();
                }
                else
                {
                    dataArray1[index] = "?";
                }

            }

            foreach (var line in dataArray1)
            {
                textBoxUserFeedback.AppendText(line + Environment.NewLine);
            }


        }

        private void button15_Click(object sender, EventArgs e)
        {
            // get list of ids from fasta file, load dssp files, add _ for any missing indexes

            var fastaFile = @"c:\d\pdb_seqres.fasta";
            //var fastaFile = @"c:\d\fasta - homo\homo - 100% symmetrical\chains [2] proteinInterfaces [1 1] [1155 proteins - 2310 sequences].fasta";

            var sequenceList = SequenceFileHandler.LoadSequenceFile(fastaFile, StaticValues.MolNameProteinAcceptedValues, false);

            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList, true).OrderBy(a => a).ToList();

            string[] pdbFilesFolders = new[] { @"C:\pdb\" };
            string[] pdbFilesArray = ProteinDataBankFileOperations.RemoveNonWhiteListedPdbIdFromPdbFilesArray(pdbIdList, ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders));

            var dsspTypes = "_BEGHIST";

            //var sequenceSS = new StringBuilder();
            long[,] counter = new long[26, dsspTypes.Length];

            foreach (var file in pdbFilesArray)
            {
                var dsspFile = file + ".dssp";

                if (!File.Exists(dsspFile)) continue;

                var secondaryStructure = DsspFormatFile.LoadDsspFile(dsspFile);


                foreach (var ss in secondaryStructure)
                {
                    if (ss.FieldAminoAcid.FieldValue[0] >= 'A' && ss.FieldAminoAcid.FieldValue[0] <= 'Z')
                    {
                        if (!String.IsNullOrWhiteSpace(ss.FieldSecondaryStructure.FieldValue))
                        {
                            counter[ss.FieldAminoAcid.FieldValue[0] - 65, dsspTypes.IndexOf(ss.FieldSecondaryStructure.FieldValue[0])]++;
                        }
                        else
                        {
                            counter[ss.FieldAminoAcid.FieldValue[0] - 65, dsspTypes.IndexOf('_')]++;
                        }
                    }
                }

            }

            textBoxUserFeedback.AppendText("SS ");
            for (var aa = 0; aa < counter.GetLength(0); aa++)
            {
                textBoxUserFeedback.AppendText("" + Convert.ToChar(aa + 65) + " ");
            }
            textBoxUserFeedback.AppendText(Environment.NewLine);

            for (var ss = 0; ss < counter.GetLength(1); ss++)
            {
                textBoxUserFeedback.AppendText("" + dsspTypes[ss] + " ");
                for (var aa = 0; aa < counter.GetLength(0); aa++)
                {
                    textBoxUserFeedback.AppendText("" + counter[aa, ss] + " ");
                }
                textBoxUserFeedback.AppendText(Environment.NewLine);
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            var lines = File.ReadAllLines(@"c:\d\1-1 noint ss to aa.txt");

            var dsspTypes = "_BEGHIST";

            //var sequenceSS = new StringBuilder();
            long[,] counter = new long[26, dsspTypes.Length];

            foreach (var line in lines)
            {
                var split = line.Split(new[] { '\t', ',' });

                var ss = split[0];
                var aa = split[1];

                if (ss.Length != aa.Length) continue;

                for (var i = 0; i < ss.Length; i++)
                {
                    counter[aa[i] - 65, dsspTypes.IndexOf(ss[i])]++;
                }
            }

            textBoxUserFeedback.AppendText("SS ");
            for (var aa = 0; aa < counter.GetLength(0); aa++)
            {
                textBoxUserFeedback.AppendText("" + Convert.ToChar(aa + 65) + " ");
            }
            textBoxUserFeedback.AppendText(Environment.NewLine);

            for (var ss = 0; ss < counter.GetLength(1); ss++)
            {
                textBoxUserFeedback.AppendText("" + dsspTypes[ss] + " ");
                for (var aa = 0; aa < counter.GetLength(0); aa++)
                {
                    textBoxUserFeedback.AppendText("" + counter[aa, ss] + " ");
                }
                textBoxUserFeedback.AppendText(Environment.NewLine);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            // find the most common consequtive ss lengths

            var dsspTypes = "_BEGHIST";

            var ss = new Dictionary<char, Dictionary<int, long>>();

            foreach (var dsspType in dsspTypes)
            {
                ss.Add(dsspType, new Dictionary<int, long>());
            }

            var pdbIds = File.ReadAllLines(@"c:\d\1-1 pdbids.txt");

            //var ssFile = @"C:\Users\aaron\Downloads\ss.txt\ss.txt";
            var ssFile = @"c:\d\1-1 dssp all.txt";

            var data = File.ReadAllLines(ssFile);

            var secStrList = new List<string>();

            var inSecStr = false;
            var secStr = "";
            for (int index = 0; index < data.Length; index++)
            {
                var line = data[index];
                if (String.IsNullOrEmpty(line)) continue;

                if (line[0] == '>')
                {
                    if (!String.IsNullOrWhiteSpace(secStr)) secStrList.Add(secStr.Replace(' ', '_'));
                    secStr = "";
                    inSecStr = line.Contains("secstr");

                    if (inSecStr)
                    {
                        inSecStr = pdbIds.Any(a => line.ToUpperInvariant().Contains(a));
                    }

                    continue;
                }

                if (inSecStr)
                {
                    secStr += line;

                    if (index == data.Length - 1)
                    {
                        secStrList.Add(secStr.Replace(' ', '_'));
                        secStr = "";
                    }
                }
            }



            foreach (var ssStr in secStrList)
            {
                char lastc = ssStr[0];
                int countc = 0;
                for (int index = 0; index < ssStr.Length; index++)
                {
                    var c = ssStr[index];

                    if (c != lastc)
                    {
                        if (index > 0 && index != ssStr.Length - 1)
                        {
                            if (ssStr[index - 1] == lastc && ssStr[index + 1] == lastc) continue;
                        }

                        if (!ss.ContainsKey(c)) ss.Add(c, new Dictionary<int, long>());
                        var dict = ss[c];
                        if (!dict.ContainsKey(countc)) dict.Add(countc, 0);
                        dict[countc]++;

                        countc = 0;
                    }

                    countc++;
                    lastc = c;
                }
            }

            var result = " " + String.Join(" ", ss.Select(a => a.Key).ToList()) + Environment.NewLine;
            var min = 1;//ss.Select(a => a.Value.Where(c=>c).Select(b=>b.Key).Min()).Min();
            var max = 22;//ss.Select(a => a.Value.Select(b=>b.Key).Max()).Max();

            for (var i = min; i <= max; i++)
            {
                result += i + " ";
                foreach (var dsspType in dsspTypes)
                {
                    var dict = ss[dsspType];

                    if (dict.ContainsKey(i))
                    {
                        result += dict[i] + " ";
                    }
                    else
                    {
                        result += "0 ";
                    }
                }

                result += Environment.NewLine;
            }

            textBoxUserFeedback.Text = result;
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        public string WhiteListFilterFastaFile(string fastaFile, string saveFastaFilename, string[] pdbIdwhiteListSplit)
        {
            var sequenceList = SequenceFileHandler.LoadSequenceFile(fastaFile, new string[] { null, "", "na", "protein", "rna", "dna" });// StaticValues.MolNameProteinAcceptedValues);

            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);

            CancellationToken ct = new CancellationToken();

            var filteredSequenceList = FilterProteins.RemoveSequences(ct, sequenceList, pdbIdwhiteListSplit.ToList(), FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);

            var filteredSequencesSaveFilename = String.IsNullOrWhiteSpace(saveFastaFilename) ? fastaFile : saveFastaFilename;

            string savedFile = SequenceFileHandler.SaveSequencesAsFasta(filteredSequenceList, filteredSequencesSaveFilename, true, FileExistsHandler.FileExistsOptions.AppendNumberToFilename, null);



            return savedFile;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            var pdbIdwhiteList = FilterPdbIdWhiteListTextBox.Text;

            var pdbIdwhiteListSplit = pdbIdwhiteList.Split(new char[] { ' ', ',', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var fastaFiles = textBoxInputFastaFiles.Lines.Where(a => !String.IsNullOrWhiteSpace(a) && File.Exists(a)).ToArray();

            foreach (var fastaFile in fastaFiles)
            {
                var savedFile = WhiteListFilterFastaFile(fastaFile, null, pdbIdwhiteListSplit);
                textBoxUserFeedback.AppendText("Saved: " + savedFile + Environment.NewLine);
            }
        }

        public class BetaSheetSpreadsheet
        {
            public string PdbId;
            public string ChainId;
            public string SiteId;
            public string InterfaceAA;
            public string InterfaceSS;
            public string PercentageE;
            public string PercentageEorB;

            public string[] ToArray()
            {


                return new string[]
                {
                    PdbId, ChainId,SiteId,InterfaceAA,InterfaceSS, PercentageE, PercentageEorB
                };
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //decimal maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            //decimal minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;

            //CategoriseBetaStrands(5.0m, 25.0m);
            //CategoriseBetaStrands(5.0m, 50.0m);
            //CategoriseBetaStrands(5.0m, 75.0m);
            //CategoriseBetaStrands(5.0m, 100.0m);

        }

        public void CategoriseBetaStrands(decimal maxAtomInterationDistance, decimal minimumProteinInterfaceDensity)
        {
            var betaSheetSpreadsheet = new List<BetaSheetSpreadsheet>();
            var betaSheetSpreadsheet2 = new List<BetaSheetSpreadsheet>();
            var betaSheetSpreadsheet3 = new List<BetaSheetSpreadsheet>();

            betaSheetSpreadsheet.Add(new BetaSheetSpreadsheet { PdbId = "PDB ID", ChainId = "Chain ID", SiteId = "Interface ID", InterfaceAA = "Amino Acids", InterfaceSS = "Secondary Structure", PercentageE = "% E", PercentageEorB = "% E or B" });
            betaSheetSpreadsheet2.Add(new BetaSheetSpreadsheet { PdbId = "PDB ID", ChainId = "Chain ID", SiteId = "Interface ID", InterfaceAA = "Amino Acids", InterfaceSS = "Secondary Structure", PercentageE = "% E", PercentageEorB = "% E or B" });
            betaSheetSpreadsheet3.Add(new BetaSheetSpreadsheet { PdbId = "PDB ID", ChainId = "Chain ID", SiteId = "Interface ID", InterfaceAA = "Amino Acids", InterfaceSS = "Secondary Structure", PercentageE = "% E", PercentageEorB = "% E or B" });



            /*
            load the filtered fasta file
            get the pdb ids
            get the secondary structure for those pdb ids
            filter the pdb ids for proteinInterface secondary structure of beta-strands
            save new fasta file
            */

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            var fastaFiles = textBoxInputFastaFiles.Lines.Where(a => !String.IsNullOrWhiteSpace(a) && File.Exists(a)).ToArray();

            var fastaFile = fastaFiles.First();

            var pdbFilesFolders = textBoxInputPdbFilesFolders.Lines;
            var sequenceList = SequenceFileHandler.LoadSequenceFile(fastaFile, new string[] { null, "", "na", "protein", "rna", "dna" });// StaticValues.MolNameProteinAcceptedValues);

            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);
            var pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders);

            var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequenceList);

            CancellationToken ct = new CancellationToken();

            List<string> pureBetaStrandsPdbIds = new List<string>();
            List<string> mixedBetaStrandsPdbIds = new List<string>();
            List<string> otherSecondaryStructuresPdbIds = new List<string>();
            List<string> noDsspSecondaryStructuresPdbIds = new List<string>();

            var mixedBetaStrandsPercentageCategories = new Dictionary<decimal, List<string>>();

            pdbFilesArray = pdbFilesArray.Where(a => pdbIdList.Contains(ProteinDataBankFileOperations.PdbIdFromPdbFilename(a))).ToArray();
            //progressBarGeneral.Maximum = pdbFilesArray.Length;
            //progressBarGeneral.Value = 0;

            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);

            var startTicks = DateTime.Now.Ticks;
            var itemsCompleted = 0;

            foreach (var pdbFilename in pdbFilesArray)//.Where(a => pdbIdList.Contains(ProteinDataBankFileOperations.PdbIdFromPdbFilename(a))))
            {
                itemsCompleted++;

                ProgressActionSet.ProgressAction(1, progressActionSet);
                ProgressActionSet.EstimatedTimeRemainingAction(startTicks, itemsCompleted, pdbFilesArray.Length, progressActionSet);


                var proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                //if (proteinId.ToUpperInvariant() != "1A0A")
                //{
                //    continue;
                //}

                // get protein proteinInterfaces
                var clusteringResult = Clustering.ClusterProteinDataBankFile(ct, maxAtomInterationDistance, minimumProteinInterfaceDensity, pdbFilename, pdbIdChainIdList, ClusteringMethodOptions.ClusterWithResidueSequenceIndex);

                if (clusteringResult == null)
                {
                    continue;
                }

                /*
                loop through each chain, each proteinInterface, get start/end position
                load dssp for those positions
                */

                // get the total number of chains having proteinInterfaces
                var totalChains = clusteringResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList.Select(a => a.ChainIdLetter).Distinct().Count();

                // create a secondary structure string appending all proteinInterfaces together, for testing

                var mergedAminoAcids = "";
                var mergedSecondaryStructure = "";

                var mergedChainAminoAcids = new Dictionary<string, string>();
                var mergedChainSecondaryStructure = new Dictionary<string, string>();


                //
                foreach (var proteinProteinInterface in clusteringResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList)
                {
                    var interfaceSS = ProteinInterfaceSecondaryStructureLoader.ProteinInterfaceSecondaryStructure(pdbFilename, proteinProteinInterface.ChainIdLetter, proteinProteinInterface.StartPosition, proteinProteinInterface.EndPosition, false);


                    decimal e = !string.IsNullOrEmpty(interfaceSS) ? ((decimal)interfaceSS.Length - (decimal)interfaceSS.Replace("E", "").Length) / (decimal)interfaceSS.Length : 0.00m;
                    decimal eb = !string.IsNullOrEmpty(interfaceSS) ? ((decimal)interfaceSS.Length - (decimal)interfaceSS.Replace("E", "").Replace("B", "").Length) / (decimal)interfaceSS.Length : 0.00m;

                    betaSheetSpreadsheet.Add(new BetaSheetSpreadsheet() { PdbId = proteinId, ChainId = proteinProteinInterface.ChainIdLetter, SiteId = proteinProteinInterface.ProteinInterfaceIdLetter, InterfaceAA = proteinProteinInterface.AminoAcidSequenceAll1L, InterfaceSS = interfaceSS, PercentageE = String.Format("{0:0.00}", e), PercentageEorB = String.Format("{0:0.00}", eb) });

                    mergedAminoAcids += proteinProteinInterface.AminoAcidSequenceAll1L;
                    mergedSecondaryStructure += interfaceSS;

                    if (!mergedChainAminoAcids.ContainsKey(proteinProteinInterface.ChainIdLetter)) { mergedChainAminoAcids.Add(proteinProteinInterface.ChainIdLetter, ""); }
                    mergedChainAminoAcids[proteinProteinInterface.ChainIdLetter] += proteinProteinInterface.AminoAcidSequenceAll1L;

                    if (!mergedChainSecondaryStructure.ContainsKey(proteinProteinInterface.ChainIdLetter)) { mergedChainSecondaryStructure.Add(proteinProteinInterface.ChainIdLetter, ""); }
                    mergedChainSecondaryStructure[proteinProteinInterface.ChainIdLetter] += interfaceSS;
                }

                foreach (var x in mergedChainAminoAcids)
                {
                    var aminoAcids = x.Value;
                    var interfaceSS = mergedChainSecondaryStructure[x.Key];

                    if (string.IsNullOrWhiteSpace(aminoAcids) && string.IsNullOrWhiteSpace(interfaceSS)) continue;

                    decimal e = !string.IsNullOrEmpty(interfaceSS) ? ((decimal)interfaceSS.Length - (decimal)interfaceSS.Replace("E", "").Length) / (decimal)interfaceSS.Length : 0.00m;
                    decimal eb = !string.IsNullOrEmpty(interfaceSS) ? ((decimal)interfaceSS.Length - (decimal)interfaceSS.Replace("E", "").Replace("B", "").Length) / (decimal)interfaceSS.Length : 0.00m;

                    betaSheetSpreadsheet2.Add(new BetaSheetSpreadsheet() { PdbId = proteinId, ChainId = x.Key, SiteId = "Merged", InterfaceAA = aminoAcids, InterfaceSS = interfaceSS, PercentageE = String.Format("{0:0.00}", e), PercentageEorB = String.Format("{0:0.00}", eb) });


                }


                if (!string.IsNullOrWhiteSpace(mergedAminoAcids) || !string.IsNullOrWhiteSpace(mergedSecondaryStructure))
                {
                    var interfaceSS = mergedSecondaryStructure;
                    decimal e = !string.IsNullOrEmpty(interfaceSS) ? ((decimal)interfaceSS.Length - (decimal)interfaceSS.Replace("E", "").Length) / (decimal)interfaceSS.Length : 0.00m;
                    decimal eb = !string.IsNullOrEmpty(interfaceSS) ? ((decimal)interfaceSS.Length - (decimal)interfaceSS.Replace("E", "").Replace("B", "").Length) / (decimal)interfaceSS.Length : 0.00m;

                    betaSheetSpreadsheet3.Add(new BetaSheetSpreadsheet() { PdbId = proteinId, ChainId = "Merged", SiteId = "Merged", InterfaceAA = mergedAminoAcids, InterfaceSS = mergedSecondaryStructure, PercentageE = String.Format("{0:0.00}", e), PercentageEorB = String.Format("{0:0.00}", eb) });
                }

                if (String.IsNullOrWhiteSpace(mergedSecondaryStructure))
                {
                    noDsspSecondaryStructuresPdbIds.Add(proteinId);
                }
                else if (mergedSecondaryStructure.Contains("E") && mergedSecondaryStructure.Replace("E", "").Replace("B", "").Replace("_", "").Length == 0)
                {
                    pureBetaStrandsPdbIds.Add(proteinId);
                }
                else if (mergedSecondaryStructure.Contains("E"))
                {
                    mixedBetaStrandsPdbIds.Add(proteinId);

                    var betaSecondaryStructuresLength = mergedSecondaryStructure.Length - mergedSecondaryStructure.Replace("E", "").Replace("B", "").Replace("_", "").Length;
                    //decimal percentageBetaStrand = (decimal)betaSecondaryStructuresLength / (decimal)secondaryStructure.Length;

                    //decimal percentageBetaStrandRounded = Math.Round(percentageBetaStrand, 1) * 100;
                    //decimal percentageBetaStrandRounded = Math.Round(percentageBetaStrand, 1) * 100;

                    //if (!mixedBetaStrandsPercentageCategories.ContainsKey(percentageBetaStrandRounded))
                    //{
                    //    mixedBetaStrandsPercentageCategories.Add(percentageBetaStrandRounded, new List<string>());
                    //}
                    //mixedBetaStrandsPercentageCategories[percentageBetaStrandRounded].Add(proteinId);
                }
                else
                {
                    otherSecondaryStructuresPdbIds.Add(proteinId);
                }

                //progressBarGeneral.Value++;

            }

            textBoxUserFeedback.AppendText("Finished processing, saving..." + Environment.NewLine);

            var parameterStr = "[atomic distance " + maxAtomInterationDistance + " a] [interface density " + minimumProteinInterfaceDensity + " pct]";

            var pureBetaStandsSequenceList = new List<ISequence>(sequenceList);
            pureBetaStandsSequenceList = FilterProteins.RemoveSequences(ct, pureBetaStandsSequenceList, pureBetaStrandsPdbIds, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);
            var pureBetaStrandsSequencesSaveFilename = @"c:\d\f\pure beta strands " + parameterStr + ".fasta";
            string savedFile1 = SequenceFileHandler.SaveSequencesAsFasta(pureBetaStandsSequenceList, pureBetaStrandsSequencesSaveFilename, true, FileExistsHandler.FileExistsOptions.AppendNumberToFilename, null);
            textBoxUserFeedback.AppendText("Saved: " + savedFile1 + Environment.NewLine);


            var mixedBetaStandsSequenceList = new List<ISequence>(sequenceList);
            mixedBetaStandsSequenceList = FilterProteins.RemoveSequences(ct, mixedBetaStandsSequenceList, mixedBetaStrandsPdbIds, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);
            var mixedBetaStrandsSequencesSaveFilename = @"c:\d\f\mixed beta strands " + parameterStr + ".fasta";
            string savedFile2 = SequenceFileHandler.SaveSequencesAsFasta(mixedBetaStandsSequenceList, mixedBetaStrandsSequencesSaveFilename, true, FileExistsHandler.FileExistsOptions.AppendNumberToFilename, null);
            textBoxUserFeedback.AppendText("Saved: " + savedFile2 + Environment.NewLine);

            foreach (var p in mixedBetaStrandsPercentageCategories)
            {
                var mixedBetaStandsPercentageCategoriesSequenceList = new List<ISequence>(sequenceList);
                mixedBetaStandsPercentageCategoriesSequenceList = FilterProteins.RemoveSequences(ct, mixedBetaStandsPercentageCategoriesSequenceList, p.Value, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);
                var mixedBetaStrandsPercentageCategoriesSequencesSaveFilename = @"c:\d\f\mixed beta strands [strand " + p.Key + " pct] " + parameterStr + ".fasta";
                string savedFilePct = SequenceFileHandler.SaveSequencesAsFasta(mixedBetaStandsPercentageCategoriesSequenceList, mixedBetaStrandsPercentageCategoriesSequencesSaveFilename, true, FileExistsHandler.FileExistsOptions.AppendNumberToFilename, null);
                textBoxUserFeedback.AppendText("Saved: " + savedFilePct + Environment.NewLine);
            }

            var otherSecondaryStructuresSequenceList = new List<ISequence>(sequenceList);
            otherSecondaryStructuresSequenceList = FilterProteins.RemoveSequences(ct, otherSecondaryStructuresSequenceList, otherSecondaryStructuresPdbIds, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);
            var otherSecondaryStructuresSequencesSaveFilename = @"c:\d\f\other secondary structures " + parameterStr + ".fasta";
            string savedFile3 = SequenceFileHandler.SaveSequencesAsFasta(otherSecondaryStructuresSequenceList, otherSecondaryStructuresSequencesSaveFilename, true, FileExistsHandler.FileExistsOptions.AppendNumberToFilename, null);
            textBoxUserFeedback.AppendText("Saved: " + savedFile3 + Environment.NewLine);

            var noDsspSecondaryStructuresSequenceList = new List<ISequence>(sequenceList);
            noDsspSecondaryStructuresSequenceList = FilterProteins.RemoveSequences(ct, noDsspSecondaryStructuresSequenceList, noDsspSecondaryStructuresPdbIds, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);
            var noDsspSecondaryStructuresSequencesSaveFilename = @"c:\d\f\no dssp secondary structures " + parameterStr + ".fasta";
            string savedFile4 = SequenceFileHandler.SaveSequencesAsFasta(noDsspSecondaryStructuresSequenceList, noDsspSecondaryStructuresSequencesSaveFilename, true, FileExistsHandler.FileExistsOptions.AppendNumberToFilename, null);
            textBoxUserFeedback.AppendText("Saved: " + savedFile4 + Environment.NewLine);

            var spreadsheets = new List<SpreadsheetCell[,]>();

            spreadsheets.Add(ConvertTypes.String2DArrayToSpreadsheetCell2DArray(ConvertTypes.StringJagged2DArrayTo2DArray(betaSheetSpreadsheet.Select(a => a.ToArray()).ToArray())));
            spreadsheets.Add(ConvertTypes.String2DArrayToSpreadsheetCell2DArray(ConvertTypes.StringJagged2DArrayTo2DArray(betaSheetSpreadsheet2.Select(a => a.ToArray()).ToArray())));
            spreadsheets.Add(ConvertTypes.String2DArrayToSpreadsheetCell2DArray(ConvertTypes.StringJagged2DArrayTo2DArray(betaSheetSpreadsheet3.Select(a => a.ToArray()).ToArray())));

            SpreadsheetFileHandler.SaveSpreadsheet(@"c:\d\f\ss " + parameterStr + ".xlsx", new string[] { "interfaces <=" + maxAtomInterationDistance + "a && >=" + minimumProteinInterfaceDensity + "%d", "merged interfaces", "merged chains and interfaces" }, spreadsheets, progressActionSet);

            textBoxUserFeedback.AppendText("Finished saving, done..." + Environment.NewLine);
        }

        public class BetaStrandInfo
        {
            public int Start;
            public int End;
            public string SecondaryStructure;
            public string DistanceFromInterface;
        }

        public static List<BetaStrandInfo> FindBetaStrandBlocks(string secondaryStructure)
        {
            var result = new List<BetaStrandInfo>();

            //var isStrand = false;
            var strandSecondaryStructure = "";
            var nonStrandSecondaryStructure = "";
            var consecutiveNonStrandCount = 0;
            var start = -1;
            const int errorMargin = 1;

            for (var x = 0; x < secondaryStructure.Length; x++)
            {
                var ss = secondaryStructure[x];

                if (ss == 'E' || ss == 'B')
                {
                    consecutiveNonStrandCount = 0;
                    if (strandSecondaryStructure == "") start = x;

                    if (nonStrandSecondaryStructure != "")
                    {
                        strandSecondaryStructure += nonStrandSecondaryStructure;
                        nonStrandSecondaryStructure = "";
                    }
                    //isStrand = true;

                    strandSecondaryStructure += ss;
                }
                else
                {
                    if (strandSecondaryStructure != "")
                    {
                        consecutiveNonStrandCount++;
                        nonStrandSecondaryStructure += ss;
                    }
                }

                if (consecutiveNonStrandCount > errorMargin || x == secondaryStructure.Length - 1)
                {
                    if (strandSecondaryStructure != "")
                    {
                        result.Add(new BetaStrandInfo { SecondaryStructure = strandSecondaryStructure, Start = start + 1, End = start + strandSecondaryStructure.Length });
                        strandSecondaryStructure = "";
                        start = -1;
                    }

                    nonStrandSecondaryStructure = "";
                    //isStrand = false;
                }

            }

            //result = result.Where(a => a.SecondaryStructure.Length >= 4).ToList();
            return result;
        }

        public static decimal FindLowestBetaStrandDistance(Point3D[][] betaStrandPointsList, int x, int y)
        {
            // beta strand, 
            var lowest = decimal.MaxValue;

            for (var atomA = 0; atomA < betaStrandPointsList[x].Length; atomA++)
            {
                for (var atomB = 0; atomB < betaStrandPointsList[y].Length; atomB++)
                {
                    var d = Point3D.Distance3D(betaStrandPointsList[x][atomA], betaStrandPointsList[y][atomB]);

                    if (d < lowest)
                    {
                        lowest = d;
                    }
                }
            }

            return lowest;
        }

        public static BetaStrandInfo MatchBetaStrandInfoToProteinInterface(List<BetaStrandInfo> betaStrandInfoList, int proteinInterfaceStart, int proteinInterfaceEnd)
        {
            // find the nearest beta strand to interface position?

            BetaStrandInfo result = null;

            var bestDistance = -1;

            var interfaceMid = proteinInterfaceStart + ((proteinInterfaceEnd - proteinInterfaceStart) / 2);

            foreach (var r in betaStrandInfoList)
            {
                // find closest distance to centre of interface
                var testInterfaceMid = r.Start + ((r.End - r.Start) / 2);

                var distance = Math.Abs(interfaceMid - testInterfaceMid);

                r.DistanceFromInterface = "" + distance;

                if (result == null || distance < bestDistance)
                {
                    bestDistance = distance;

                    result = r;
                }
            }

            return result;
        }

        public List<List<BetaStrandInfo>> SingleLinkBetaStrands(decimal maxBetaStrandDistance, decimal[,] distanceMatrix, List<BetaStrandInfo> betaStrandInfoList)
        {

            var result = new List<List<BetaStrandInfo>>();

            var doneMask = new bool[distanceMatrix.GetLength(0), distanceMatrix.GetLength(1)];

            for (var a = 0; a < betaStrandInfoList.Count; a++)
            {
                result.Add(new List<BetaStrandInfo>() { betaStrandInfoList[a] });
            }

            for (var x = 0; x < distanceMatrix.GetLength(0); x++)
            {
                for (var y = 0; y < distanceMatrix.GetLength(0); y++)
                {
                    var d = distanceMatrix[x, y];

                    if (d <= maxBetaStrandDistance)
                    {
                        // merge the x,y groups together, if not already


                        var xGroup = result.FirstOrDefault(a => a.Contains(betaStrandInfoList[x]));
                        var yGroup = result.FirstOrDefault(a => a.Contains(betaStrandInfoList[y]));
                        if (xGroup != yGroup)
                        {
                            xGroup.AddRange(yGroup);
                            yGroup.Clear();
                            result.Remove(yGroup);
                        }

                    }
                }
            }



            return result;

        }

        public class BetaStrandClusterInfo
        {
            public string PdbId;
            public string ChainId;
            
            public string InterfaceId;
            public string InterfaceAA;
            public string InterfaceSS;
            public string PercentageE;
            public string PercentageEorB;
            public string InterfaceStrand;
            public string StrandDistanceFromInterface;
            public string TotalBetaStrands;

            //public string BetaStrandsClusteredWithInterface10;
            //public string BetaStrandsClusteredWithInterface15;
            //public string BetaStrandsClusteredWithInterface20;
            //public string BetaStrandsClusteredWithInterface25;
            //public string BetaStrandsClusteredWithInterface30;
            //public string BetaStrandsClusteredWithInterface35;
            //public string BetaStrandsClusteredWithInterface40;
            public string BetaStrandsClusteredWithInterface45;
            public string BetaStrandsClusteredWithInterface50;
            public string BetaStrandsClusteredWithInterface55;
            public string BetaStrandsClusteredWithInterface60;
            public string BetaStrandsClusteredWithInterface65;
            public string BetaStrandsClusteredWithInterface70;
            public string BetaStrandsClusteredWithInterface75;
            public string BetaStrandsClusteredWithInterface80;
            public string BetaStrandsClusteredWithInterface85;
            public string BetaStrandsClusteredWithInterface90;
            //public string BetaStrandsClusteredWithInterface95;

            public string FullSecondaryStructure;

            public string[] ToArray()
            {
                return new string[]
                {
                    PdbId, ChainId, InterfaceId, InterfaceAA,InterfaceSS,
                    PercentageE, PercentageEorB,
                    InterfaceStrand, StrandDistanceFromInterface, TotalBetaStrands,
                    //BetaStrandsClusteredWithInterface10,
                    //BetaStrandsClusteredWithInterface15,
                    //BetaStrandsClusteredWithInterface20,
                    //BetaStrandsClusteredWithInterface25,
                    //BetaStrandsClusteredWithInterface30,
                    //BetaStrandsClusteredWithInterface35,
                    //BetaStrandsClusteredWithInterface40,
                    BetaStrandsClusteredWithInterface45,
                    BetaStrandsClusteredWithInterface50,
                    BetaStrandsClusteredWithInterface55,
                    BetaStrandsClusteredWithInterface60,
                    BetaStrandsClusteredWithInterface65,
                    BetaStrandsClusteredWithInterface70,
                    BetaStrandsClusteredWithInterface75,
                    BetaStrandsClusteredWithInterface80,
                    BetaStrandsClusteredWithInterface85,
                    BetaStrandsClusteredWithInterface90,
                    //BetaStrandsClusteredWithInterface95,
                    FullSecondaryStructure
                };
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            var maxAtomInterationDistance = numericUpDownMaxAtomicInteractionDistance.Value;
            var minimumProteinInterfaceDensity = numericUpDownMinInterfaceInteractionDensity.Value;
            var maxBetaStrandDistance = numericUpDownMaxBetaSheetStrandAtomicDistance.Value;

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            var parameterStr = "";// "[interaction distance 5.0 a] [beta strand distance 8.0 a]";

            var headers = new List<string>();

            var sList = new List<SpreadsheetCell[,]>() { };

            for (var interactionDistance = 5.0m; interactionDistance <= 8.0m; interactionDistance+=3.0m)
            {
                //if (interactionDistance != 5.0m && interactionDistance != 8.0m) { continue; }

                for (var interfaceDensity = 10.0m; interfaceDensity <= 100.0m; interfaceDensity+=10.0m)
                {
                    for (var dimerType = 0; dimerType <= 3; dimerType++)
                    {
                        var dimerStr = "";

                        if (dimerType == 0) { dimerStr = "dmrs"; }
                        else if (dimerType == 1) { dimerStr = "hm-dmrs"; }
                        else if (dimerType == 2) { dimerStr = "htr-dmrs"; }
                        else if (dimerType == 3) { dimerStr = "hmlgy-dmrs"; }
                        else { throw new Exception(); }

                        var s = BetaStrandCluster(dimerType, interactionDistance, interfaceDensity);

                        sList.Add(s);
                        
                        headers.Add(dimerStr + " " + interactionDistance + "A " + interfaceDensity + "%");
                    }
                }
            }

            SpreadsheetFileHandler.SaveSpreadsheet(@"c:\d\f\beta strands count " + parameterStr + ".xlsx", headers.ToArray(), sList, progressActionSet);
        }

        public SpreadsheetCell[,] BetaStrandCluster(int dimerType, decimal maxAtomInterationDistance, decimal minimumProteinInterfaceDensity)//, decimal maxBetaStrandDistance)
        {
            textBoxUserFeedback.AppendText("dimerType: " + dimerType + ", maxAtomInterationDistance: " + maxAtomInterationDistance + ", minimumProteinInterfaceDensity: " + minimumProteinInterfaceDensity + "." + Environment.NewLine);


            var fastaDimers5a = @"C:\d\filter-30pc\dimers with interactions at 5a\5a Dimers\5a Dimers [4129 proteins - 8258 sequences].fasta";
            var fastaHeteroDimers5a = @"C:\d\filter-30pc\dimers with interactions at 5a\5a Heterodimers\5a Heterodimers only (sequence filter) [4309 proteins - 8618 sequences] [402 proteins - 804 sequences].fasta";
            var fastaHomoDimers5a = @"C:\d\filter-30pc\dimers with interactions at 5a\5a Homodimers\5a Homodimers only (sequence filter) [19044 proteins - 38088 sequences] [3644 proteins - 7288 sequences].fasta";
            var fastaHomologyDimers5a = @"C:\d\filter-30pc\dimers with interactions at 5a\5a Homology dimers\5a Homology dimers only (sequence filter) [1132 proteins - 2264 sequences] [83 proteins - 166 sequences].fasta";


            var fastaDimers8a = @"C:\d\filter-30pc\dimers with interactions at 8a\8a Dimers\8a dimers [5870 proteins - 11740 sequences].fasta";
            var fastaHeteroDimers8a = @"C:\d\filter-30pc\dimers with interactions at 8a\8a Heterodimers\8a Heterodimers only (sequence filter) [5322 proteins - 10644 sequences] [495 proteins - 990 sequences].fasta";
            var fastaHomoDimers8a = @"C:\d\filter-30pc\dimers with interactions at 8a\8a Homodimers\8a Homodimers only (sequence filter) [27195 proteins - 54390 sequences] [5274 proteins - 10548 sequences].fasta";
            var fastaHomologyDimers8a = @"C:\d\filter-30pc\dimers with interactions at 8a\8a Homology dimers\8a Homology dimers only (sequence filter) [1497 proteins - 2994 sequences] [101 proteins - 202 sequences].fasta";

            

            var fastaFile = "";
            if (maxAtomInterationDistance == 5.0m)
            {
                if (dimerType == 0) { fastaFile = fastaDimers5a; }
                else if (dimerType == 1) { fastaFile = fastaHeteroDimers5a; }
                else if (dimerType == 2) { fastaFile = fastaHomoDimers5a; }
                else if (dimerType == 3) { fastaFile = fastaHomologyDimers5a; }
                else { throw new Exception(); }
            }
            else if (maxAtomInterationDistance == 8.0m)
            {
                if (dimerType == 0) { fastaFile = fastaDimers8a; }
                else if (dimerType == 1) { fastaFile = fastaHeteroDimers8a; }
                else if (dimerType == 2) { fastaFile = fastaHomoDimers8a; }
                else if (dimerType == 3) { fastaFile = fastaHomologyDimers8a; }
                else { throw new Exception(); }
            }
            else
            {
                throw new Exception();
            }

            textBoxUserFeedback.AppendText(fastaFile + Environment.NewLine);

            var betaStrandClusterInfo = new List<BetaStrandClusterInfo>();
            betaStrandClusterInfo.Add(new BetaStrandClusterInfo()
            {
                PdbId = "PDB ID",
                ChainId = "Chain ID",
                InterfaceId = "Interface ID",
                InterfaceAA = "Interface Amino Acids",
                InterfaceSS = "Interface Secondary Structure",
                PercentageE = "% B-Strand",
                PercentageEorB = "% B-Strand or B-Bridge",
                StrandDistanceFromInterface = "Strand Distance From Interface",
                InterfaceStrand = "Interface Strand",
                TotalBetaStrands = "Total Beta-Strands",

                //BetaStrandsClusteredWithInterface10 = "B-Shts @ 1.0 A",
                //BetaStrandsClusteredWithInterface15 = "B-Shts @ 1.5 A",
                //BetaStrandsClusteredWithInterface20 = "B-Shts @ 2.0 A",
                //BetaStrandsClusteredWithInterface25 = "B-Shts @ 2.5 A",
                //BetaStrandsClusteredWithInterface30 = "B-Shts @ 3.0 A",
                //BetaStrandsClusteredWithInterface35 = "B-Shts @ 3.5 A",
                //BetaStrandsClusteredWithInterface40 = "B-Shts @ 4.0 A",
                BetaStrandsClusteredWithInterface45 = "B-Shts @ 4.5 A",
                BetaStrandsClusteredWithInterface50 = "B-Shts @ 5.0 A",
                BetaStrandsClusteredWithInterface55 = "B-Shts @ 5.5 A",
                BetaStrandsClusteredWithInterface60 = "B-Shts @ 6.0 A",
                BetaStrandsClusteredWithInterface65 = "B-Shts @ 6.5 A",
                BetaStrandsClusteredWithInterface70 = "B-Shts @ 7.0 A",
                BetaStrandsClusteredWithInterface75 = "B-Shts @ 7.5 A",
                BetaStrandsClusteredWithInterface80 = "B-Shts @ 8.0 A",
                BetaStrandsClusteredWithInterface85 = "B-Shts @ 8.5 A",
                BetaStrandsClusteredWithInterface90 = "B-Shts @ 9.0 A",
                //BetaStrandsClusteredWithInterface95 = "B-Shts @ 9.5 A",

                FullSecondaryStructure = "Full Secondary Structure"
            });

            var progressActionSet = new ProgressActionSet(StartAction, FinishAction, ProgressAction, StatusTextAction, EstimatedTimeRemainingAction);

            var fastaFiles = textBoxInputFastaFiles.Lines.Where(a => !String.IsNullOrWhiteSpace(a) && File.Exists(a)).ToArray();

            //var fastaFile = fastaFiles.First();

            var pdbFilesFolders = new string[] { @"C:\pdb\" };// textBoxInputPdbFilesFolders.Lines;
            var sequenceList = SequenceFileHandler.LoadSequenceFile(fastaFile, new string[] { null, "", "na", "protein", "rna", "dna" });// StaticValues.MolNameProteinAcceptedValues);

            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);
            var pdbFilesArray = ProteinDataBankFileOperations.GetPdbFilesArray(pdbFilesFolders);

            var pdbIdChainIdList = ProteinDataBankFileOperations.PdbIdChainIdList(sequenceList);

            CancellationToken ct = new CancellationToken();

            var categoriesNumberOfBetaSheetsTogether = new Dictionary<int, List<string>>();

            var noProteinInterfaces = new List<string>();

            pdbFilesArray = pdbFilesArray.Where(a => pdbIdList.Contains(ProteinDataBankFileOperations.PdbIdFromPdbFilename(a))).ToArray();

            ProgressActionSet.StartAction(pdbFilesArray.Length, progressActionSet);

            var itemsCompleted = 0;
            var startTicks = DateTime.Now.Ticks;

            foreach (var pdbFilename in pdbFilesArray)
            {
                itemsCompleted++;

                ProgressActionSet.ProgressAction(1, progressActionSet);
                ProgressActionSet.EstimatedTimeRemainingAction(startTicks, itemsCompleted, pdbFilesArray.Length, progressActionSet);

                var proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

                var dsspFilename = pdbFilename + ".dssp";

                if (!File.Exists(dsspFilename)) continue;

                //var secondaryStructureFile = DsspFormatFile.LoadDsspFile(dsspFilename);
                


                var clusteringResult = Clustering.ClusterProteinDataBankFile(ct, maxAtomInterationDistance, minimumProteinInterfaceDensity, pdbFilename, pdbIdChainIdList, ClusteringMethodOptions.ClusterWithResidueSequenceIndex);

                // does the chain contain pleated beta strands?

                var proteinInterfaceSecondaryStructure = "";

                if (clusteringResult == null)
                {
                    Debug.WriteLine("No clustering result: " + proteinId + " " + pdbFilename);
                    continue;
                }

                if (clusteringResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList.Count == 0 || clusteringResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList == null)
                {
                    //textBoxUserFeedback.AppendText(proteinId + " did not have any proteinInterfaces");
                    noProteinInterfaces.Add(proteinId);
                }

                foreach (var proteinProteinInterface in clusteringResult.ProteinInterfaceAnalysisResultData.ProteinInterfacesSequenceAndPositionDataList)
                {
                    //if (proteinId == "3BZY")
                    //{

                    //}
                    var secondaryStructureStr =          ProteinInterfaceSecondaryStructureLoader.ProteinInterfaceSecondaryStructure(pdbFilename, proteinProteinInterface.ChainIdLetter, 1);
                    proteinInterfaceSecondaryStructure = ProteinInterfaceSecondaryStructureLoader.ProteinInterfaceSecondaryStructure(pdbFilename, proteinProteinInterface.ChainIdLetter, proteinProteinInterface.StartPosition, proteinProteinInterface.EndPosition, false);

                    var betaStrandBlocksList = FindBetaStrandBlocks(secondaryStructureStr);

                    var matchingProteinInterfaceBlock = MatchBetaStrandInfoToProteinInterface(betaStrandBlocksList, proteinProteinInterface.StartPosition, proteinProteinInterface.EndPosition);


                    //var matchingProteinInterfaceBlock = matchingProteinInterfaceBlockIndex != -1 ? betaStrandBlocksList[matchingProteinInterfaceBlockIndex] : null;

                    //var proteinInterfaceCoordinates = matchingProteinInterfaceBlock != null ? clusteringResult.PdbFileChains.ChainList[proteinProteinInterface.FullProteinInterfaceId.ChainId].AtomList.Where(a => int.Parse(a.resSeq.FieldValue) >= matchingProteinInterfaceBlock.Start && int.Parse(a.resSeq.FieldValue) <= matchingProteinInterfaceBlock.End).Select(b => new Point3D(b.x.FieldValue, b.y.FieldValue, b.z.FieldValue)) : null;

                    // get coordinates for each beta strand block

                    var blockCoordinates = new Point3D[betaStrandBlocksList.Count][];

                    for (var x = 0; x < betaStrandBlocksList.Count; x++)
                    {
                        var strand = betaStrandBlocksList[x];

                        blockCoordinates[x] = clusteringResult.PdbFileChains.ChainList[proteinProteinInterface.FullProteinInterfaceId.ChainId].AtomList
                            .Where(a => int.Parse(a.resSeq.FieldValue) >= strand.Start && int.Parse(a.resSeq.FieldValue) <= strand.End)
                            .Select(b => new Point3D(b.x.FieldValue, b.y.FieldValue, b.z.FieldValue)).ToArray();
                    }

                    var distanceMatrix = new decimal[betaStrandBlocksList.Count, betaStrandBlocksList.Count];

                    for (var x = 0; x < betaStrandBlocksList.Count; x++)
                    {
                        for (var y = 0; y < betaStrandBlocksList.Count; y++)
                        {
                            distanceMatrix[x, y] = FindLowestBetaStrandDistance(blockCoordinates, x, y);
                        }
                    }



                    //var betaStrandClusters10 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(1.0m, distanceMatrix, betaStrandBlocksList) : null;
                    //var betaStrandClusters15 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(1.5m, distanceMatrix, betaStrandBlocksList) : null;
                    //var betaStrandClusters20 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(2.0m, distanceMatrix, betaStrandBlocksList) : null;
                    //var betaStrandClusters25 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(2.5m, distanceMatrix, betaStrandBlocksList) : null;
                    //var betaStrandClusters30 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(3.0m, distanceMatrix, betaStrandBlocksList) : null;
                    //var betaStrandClusters35 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(3.5m, distanceMatrix, betaStrandBlocksList) : null;
                    //var betaStrandClusters40 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(4.0m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters45 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(4.5m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters50 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(5.0m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters55 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(5.5m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters60 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(6.0m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters65 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(6.5m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters70 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(7.0m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters75 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(7.5m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters80 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(8.0m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters85 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(8.5m, distanceMatrix, betaStrandBlocksList) : null;
                    var betaStrandClusters90 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(9.0m, distanceMatrix, betaStrandBlocksList) : null;
                    //var betaStrandClusters95 = distanceMatrix != null && betaStrandBlocksList != null ? SingleLinkBetaStrands(9.5m, distanceMatrix, betaStrandBlocksList) : null;









                    // check if the proteinInterface is clustered with other strands
                    //var betaStandClusterWithProteinInterface10 = matchingProteinInterfaceBlock != null && betaStrandClusters10 != null ? betaStrandClusters10.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    //var betaStandClusterWithProteinInterface15 = matchingProteinInterfaceBlock != null && betaStrandClusters15 != null ? betaStrandClusters15.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    //var betaStandClusterWithProteinInterface20 = matchingProteinInterfaceBlock != null && betaStrandClusters20 != null ? betaStrandClusters20.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    //var betaStandClusterWithProteinInterface25 = matchingProteinInterfaceBlock != null && betaStrandClusters25 != null ? betaStrandClusters25.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    //var betaStandClusterWithProteinInterface30 = matchingProteinInterfaceBlock != null && betaStrandClusters30 != null ? betaStrandClusters30.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    //var betaStandClusterWithProteinInterface35 = matchingProteinInterfaceBlock != null && betaStrandClusters35 != null ? betaStrandClusters35.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    //var betaStandClusterWithProteinInterface40 = matchingProteinInterfaceBlock != null && betaStrandClusters40 != null ? betaStrandClusters40.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface45 = matchingProteinInterfaceBlock != null && betaStrandClusters45 != null ? betaStrandClusters45.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface50 = matchingProteinInterfaceBlock != null && betaStrandClusters50 != null ? betaStrandClusters50.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface55 = matchingProteinInterfaceBlock != null && betaStrandClusters55 != null ? betaStrandClusters55.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface60 = matchingProteinInterfaceBlock != null && betaStrandClusters60 != null ? betaStrandClusters60.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface65 = matchingProteinInterfaceBlock != null && betaStrandClusters65 != null ? betaStrandClusters65.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface70 = matchingProteinInterfaceBlock != null && betaStrandClusters70 != null ? betaStrandClusters70.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface75 = matchingProteinInterfaceBlock != null && betaStrandClusters75 != null ? betaStrandClusters75.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface80 = matchingProteinInterfaceBlock != null && betaStrandClusters80 != null ? betaStrandClusters80.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface85 = matchingProteinInterfaceBlock != null && betaStrandClusters85 != null ? betaStrandClusters85.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    var betaStandClusterWithProteinInterface90 = matchingProteinInterfaceBlock != null && betaStrandClusters90 != null ? betaStrandClusters90.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;
                    //var betaStandClusterWithProteinInterface95 = matchingProteinInterfaceBlock != null && betaStrandClusters95 != null ? betaStrandClusters95.FirstOrDefault(a => a.Contains(matchingProteinInterfaceBlock)) : null;

                    decimal e = !string.IsNullOrEmpty(proteinInterfaceSecondaryStructure) ? ((decimal)proteinInterfaceSecondaryStructure.Length - (decimal)proteinInterfaceSecondaryStructure.Replace("E", "").Length) / (decimal)proteinInterfaceSecondaryStructure.Length : 0.00m;
                    decimal eb = !string.IsNullOrEmpty(proteinInterfaceSecondaryStructure) ? ((decimal)proteinInterfaceSecondaryStructure.Length - (decimal)proteinInterfaceSecondaryStructure.Replace("E", "").Replace("B", "").Length) / (decimal)proteinInterfaceSecondaryStructure.Length : 0.00m;


                    betaStrandClusterInfo.Add(new BetaStrandClusterInfo()
                    {
                        PdbId = proteinId,
                        ChainId = proteinProteinInterface.ChainIdLetter,
                        InterfaceId = proteinProteinInterface.ProteinInterfaceIdLetter,
                        InterfaceAA = proteinProteinInterface.AminoAcidSequenceAll1L,
                        InterfaceSS = proteinInterfaceSecondaryStructure,
                        PercentageE = String.Format("{0:0.00}", e),
                        PercentageEorB = String.Format("{0:0.00}", eb),
                        StrandDistanceFromInterface = matchingProteinInterfaceBlock != null ? matchingProteinInterfaceBlock.DistanceFromInterface : "",
                        InterfaceStrand = matchingProteinInterfaceBlock != null ? matchingProteinInterfaceBlock.SecondaryStructure : "",
                        TotalBetaStrands = "" + betaStrandBlocksList.Count,

                        //BetaStrandsClusteredWithInterface10 = "" + (betaStandClusterWithProteinInterface10 != null ? betaStandClusterWithProteinInterface10.Count : 0),
                        //BetaStrandsClusteredWithInterface15 = "" + (betaStandClusterWithProteinInterface15 != null ? betaStandClusterWithProteinInterface15.Count : 0),
                        //BetaStrandsClusteredWithInterface20 = "" + (betaStandClusterWithProteinInterface20 != null ? betaStandClusterWithProteinInterface20.Count : 0),
                        //BetaStrandsClusteredWithInterface25 = "" + (betaStandClusterWithProteinInterface25 != null ? betaStandClusterWithProteinInterface25.Count : 0),
                        //BetaStrandsClusteredWithInterface30 = "" + (betaStandClusterWithProteinInterface30 != null ? betaStandClusterWithProteinInterface30.Count : 0),
                        //BetaStrandsClusteredWithInterface35 = "" + (betaStandClusterWithProteinInterface35 != null ? betaStandClusterWithProteinInterface35.Count : 0),
                        //BetaStrandsClusteredWithInterface40 = "" + (betaStandClusterWithProteinInterface40 != null ? betaStandClusterWithProteinInterface40.Count : 0),
                        BetaStrandsClusteredWithInterface45 = "" + (betaStandClusterWithProteinInterface45 != null ? betaStandClusterWithProteinInterface45.Count : 0),
                        BetaStrandsClusteredWithInterface50 = "" + (betaStandClusterWithProteinInterface50 != null ? betaStandClusterWithProteinInterface50.Count : 0),
                        BetaStrandsClusteredWithInterface55 = "" + (betaStandClusterWithProteinInterface55 != null ? betaStandClusterWithProteinInterface55.Count : 0),
                        BetaStrandsClusteredWithInterface60 = "" + (betaStandClusterWithProteinInterface60 != null ? betaStandClusterWithProteinInterface60.Count : 0),
                        BetaStrandsClusteredWithInterface65 = "" + (betaStandClusterWithProteinInterface65 != null ? betaStandClusterWithProteinInterface65.Count : 0),
                        BetaStrandsClusteredWithInterface70 = "" + (betaStandClusterWithProteinInterface70 != null ? betaStandClusterWithProteinInterface70.Count : 0),
                        BetaStrandsClusteredWithInterface75 = "" + (betaStandClusterWithProteinInterface75 != null ? betaStandClusterWithProteinInterface75.Count : 0),
                        BetaStrandsClusteredWithInterface80 = "" + (betaStandClusterWithProteinInterface80 != null ? betaStandClusterWithProteinInterface80.Count : 0),
                        BetaStrandsClusteredWithInterface85 = "" + (betaStandClusterWithProteinInterface85 != null ? betaStandClusterWithProteinInterface85.Count : 0),
                        BetaStrandsClusteredWithInterface90 = "" + (betaStandClusterWithProteinInterface90 != null ? betaStandClusterWithProteinInterface90.Count : 0),
                        //BetaStrandsClusteredWithInterface95 = "" + (betaStandClusterWithProteinInterface95 != null ? betaStandClusterWithProteinInterface95.Count : 0),

                        FullSecondaryStructure = secondaryStructureStr != null ? secondaryStructureStr : ""
                    });


                    //if (betaStandClusterWithProteinInterface != null)
                    //{
                    //    if (!categoriesNumberOfBetaSheetsTogether.ContainsKey(betaStandClusterWithProteinInterface.Count)) categoriesNumberOfBetaSheetsTogether.Add(betaStandClusterWithProteinInterface.Count, new List<string>());

                    //    categoriesNumberOfBetaSheetsTogether[betaStandClusterWithProteinInterface.Count].Add(proteinId);
                    //}
                }
            }

            textBoxUserFeedback.AppendText("Finished processing, saving..." + Environment.NewLine);
            //var parameterStr = "";// "[interaction distance " + maxAtomInterationDistance + " a] [interface density " + minimumProteinInterfaceDensity + " pct]";// [beta strand distance " + maxBetaStrandDistance + " a]";

            var spreadsheet = ConvertTypes.String2DArrayToSpreadsheetCell2DArray(ConvertTypes.StringJagged2DArrayTo2DArray(betaStrandClusterInfo.Select(a => a.ToArray()).ToArray()));

            return spreadsheet;
            //SpreadsheetFileHandler.SaveSpreadsheet(@"c:\d\f\beta strands count " + parameterStr + ".xlsx",new string[] { "beta-strand count" }, spreadsheet);





            //textBoxUserFeedback.AppendText("The following " + noProteinInterfaces.Count + " proteins had no detected proteinInterfaces: " + String.Join(", ", noProteinInterfaces) + Environment.NewLine);

            //foreach (var entry in categoriesNumberOfBetaSheetsTogether)
            //{
            //var sequenceListCopy = new List<ISequence>(sequenceList);
            //sequenceListCopy = FilterProteins.RemoveSequences(ct, sequenceListCopy, entry.Value, FilterProteins.RemoveSequencesOptions.RemoveSequencesNotInList);
            //var otherSecondaryStructuresSequencesSaveFilename = @"c:\d\f\interface within beta strands " + entry.Key.ToString().PadLeft(3, '0') + " " + parameterStr + ".fasta";
            //string savedFile3 = SequenceFileHandler.SaveSequencesAsFasta(sequenceListCopy, otherSecondaryStructuresSequencesSaveFilename, true, FileExistsHandler.FileExistsOptions.AppendNumberToFilename, null);
            //textBoxUserFeedback.AppendText("Saved: " + savedFile3 + Environment.NewLine);
            //}

            //textBoxUserFeedback.AppendText("Finished saving, done..." + Environment.NewLine);
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            var fasta30pctSid = @"c:\d\dimers 30pct sequence identity filter.fasta";
            var fasta90pctSid = @"c:\d\dimers 90pct sequence identity filter.fasta";

            var fastaFilesFolderWithDuplicates = @"c:\d\filter-none\";

            var fastaFiles = Directory.GetFiles(fastaFilesFolderWithDuplicates, "*.fasta", SearchOption.AllDirectories);

            foreach (var fastaFile in fastaFiles)
            {

                var sequenceList30pct = SequenceFileHandler.LoadSequenceFile(fasta30pctSid, new string[] { null, "", "na", "protein", "rna", "dna" });// StaticValues.MolNameProteinAcceptedValues);
                var pdbIdList30pct = FilterProteins.SequenceListToPdbIdList(sequenceList30pct);
                var filename30pctSid = fastaFile.Replace("filter-none", "filter-30pc");

                var sequenceList90pct = SequenceFileHandler.LoadSequenceFile(fasta90pctSid, new string[] { null, "", "na", "protein", "rna", "dna" });// StaticValues.MolNameProteinAcceptedValues);
                var pdbIdList90pct = FilterProteins.SequenceListToPdbIdList(sequenceList90pct);
                var filename90pctSid = fastaFile.Replace("filter-none", "filter-90pc");

                var sequenceList = SequenceFileHandler.LoadSequenceFile(fastaFile, new string[] { null, "", "na", "protein", "rna", "dna" });// StaticValues.MolNameProteinAcceptedValues);
                var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);

                var savedFile30 = WhiteListFilterFastaFile(fastaFile, filename30pctSid, pdbIdList30pct.ToArray());
                textBoxUserFeedback.AppendText("Saved: " + savedFile30 + Environment.NewLine);

                var savedFile90 = WhiteListFilterFastaFile(fastaFile, filename90pctSid, pdbIdList90pct.ToArray());
                textBoxUserFeedback.AppendText("Saved: " + savedFile90 + Environment.NewLine);
            }



        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            
        }
    }
}