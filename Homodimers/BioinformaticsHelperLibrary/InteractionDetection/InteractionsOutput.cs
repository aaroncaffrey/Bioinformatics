using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.UserProteinInterface;
using System.Threading;

namespace BioinformaticsHelperLibrary.InteractionDetection
{
    public static class InteractionsOutput
    {
        public enum FormatInternationOutputStyle
        {
            PdbSumStyle,
            DefaultStyle = PdbSumStyle,
        }

        public static string FormatInteractionOutput(string pdbId, List<AtomPair> interactionsList, FormatInternationOutputStyle formatInternationOutputStyle = FormatInternationOutputStyle.DefaultStyle)
        {
            var sb = new StringBuilder();

            sb.AppendLine("List of atom-atom interactions across protein-protein proteinInterface           ");
            sb.AppendLine("---------------------------------------------------------------           ");
            sb.AppendLine("                                                                          ");
            sb.AppendLine("                 PDB code: " + pdbId + "                                           ");
            sb.AppendLine("                 ------------------------------                           ");
            sb.AppendLine("                                                                          ");
            sb.AppendLine("                                                                          ");
            sb.AppendLine("       <----- A T O M   1 ----->       <----- A T O M   2 ----->          ");
            sb.AppendLine("                                                                          ");
            sb.AppendLine("       Atom Atom Res  Res              Atom Atom Res  Res                 ");
            sb.AppendLine("        no. name name no.  Chain        no. name name no.  Chain  Distance");

            for (int interactionIndex = 0; interactionIndex < interactionsList.Count; interactionIndex++)
            {
                AtomPair interactingPair = interactionsList[interactionIndex];


                string line = (interactionIndex + 1).ToString().PadLeft(3) + ". " +
                              interactingPair.Atom1.serial.FieldValue.PadLeft(6) + "  " + interactingPair.Atom1.name.FieldValue.PadRight(4) + interactingPair.Atom1.resName.FieldValue.PadLeft(3) + interactingPair.Atom1.resSeq.FieldValue.PadLeft(5) + "    " + interactingPair.Atom1.chainID.FieldValue +
                              "   <-->" +
                              interactingPair.Atom2.serial.FieldValue.PadLeft(6) + "  " + interactingPair.Atom2.name.FieldValue.PadRight(4) + interactingPair.Atom2.resName.FieldValue.PadLeft(3) + interactingPair.Atom2.resSeq.FieldValue.PadLeft(5) + "    " + interactingPair.Atom2.chainID.FieldValue +
                              Math.Round(interactingPair.Distance, 2).ToString("0.00").PadLeft(10);

                sb.AppendLine(line);
            }

            sb.AppendLine("                                                                          ");
            sb.AppendLine("Number of interactions:          " + interactionsList.Count.ToString().PadLeft(3) + "                                      ");
            sb.AppendLine("                                                                          ");
            sb.AppendLine("                                                                          ");

            return sb.ToString();
        }

        public static string SaveInteractionsOutput(string saveFilename, string[] interactionOutputStrings, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            var fileInfo = new FileInfo(saveFilename);

            if (fileInfo.Directory != null) fileInfo.Directory.Create();

            if (File.Exists(fileInfo.FullName))
            {
                if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                {
                    saveFilename = FileExistsHandler.FindNextFreeOutputFilename(fileInfo.FullName);
                    fileInfo = new FileInfo(saveFilename);
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                {
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                {
                    return null;
                }
            }

            File.WriteAllLines(fileInfo.FullName, interactionOutputStrings);

            return fileInfo.FullName;
        }

        public static string[] MakeInteractionsOutput(CancellationToken cancellationToken, decimal maxAtomInterationDistance, string[] pdbFilesList, Dictionary<string, List<string>> pdbIdChainIdList, ProgressActionSet progressActionSet, bool outputToGui)
        {
            var interactionTasks = new List<Task<string>>();

            ProgressActionSet.StartAction(pdbFilesList.Length, progressActionSet);


            foreach (string pdbFilename in pdbFilesList)
            {
                string _pdbFilename = pdbFilename;
                while (interactionTasks.Count(t => t != null && !t.IsCompleted) >= Environment.ProcessorCount * 10)
                {
                    Task.WaitAny(interactionTasks.ToArray<Task>());
                }

                var interactionTask = Task.Run(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    List<AtomPair> interactionsList = SearchInteractions.FindInteractions(cancellationToken, maxAtomInterationDistance, _pdbFilename, pdbIdChainIdList);

                    string pdbId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(_pdbFilename);
                    if (string.IsNullOrEmpty(pdbId))
                    {
                        //pdbId = _pdbFilename;
                        throw new ArgumentNullException(nameof(pdbFilesList), "The file " + _pdbFilename + " has an invalid name.");
                    }

                    if (interactionsList == null) interactionsList = new List<AtomPair>();

                    interactionsList = interactionsList.OrderBy(o => o.Distance).ToList();

                    var interactionsString = FormatInteractionOutput(pdbId, interactionsList);

                    if (outputToGui) ProgressActionSet.Report(interactionsString, progressActionSet);

                    ProgressActionSet.ProgressAction(1, progressActionSet);
                    //ProgressActionSet.EstimatedTimeRemainingAction(startTicks, );

                    //////Console.WriteLine(_pdbFilename);

                    return interactionsString;
                }, cancellationToken);
                
                interactionTasks.Add(interactionTask);
            }
            
            Task.WaitAll(interactionTasks.Where(t => t != null && !t.IsCompleted).ToArray<Task>());

            var interactionsStringsList = interactionTasks.OrderBy(t=>t.Id).Where(t => t != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted && t.Result != null).Select(t => t.Result).ToArray();

            ProgressActionSet.FinishAction(true, progressActionSet);

            return interactionsStringsList;
        }
    }
}
