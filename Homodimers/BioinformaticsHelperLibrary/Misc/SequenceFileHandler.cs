using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using Bio.Extensions;
using Bio.IO;
using Bio.IO.FastA;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.UserProteinInterface;

namespace BioinformaticsHelperLibrary.Misc
{
    public class SequenceFileHandler
    {
        /// <summary>
        ///     This method loads a single sequence file.  FASTA is the preferred format.
        /// </summary>
        /// <param name="sequenceFilename"></param>
        /// <param name="molNames"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static List<ISequence> LoadSequenceFile(string sequenceFilename, string[] molNames, bool distinct = true)
        {
            if (string.IsNullOrWhiteSpace(sequenceFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceFilename));
            }

            if (!File.Exists(sequenceFilename))
            {
                throw new FileNotFoundException(sequenceFilename);
            }

            List<ISequence> sequences = null;

            ISequenceParser sequenceParser = null;
            try
            {
                sequenceParser = SequenceParsers.FindParserByFileName(sequenceFilename);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                // just forward exception for now
                throw new DirectoryNotFoundException(directoryNotFoundException.Message, directoryNotFoundException.InnerException);
            }


            if (sequenceParser != null)
            {
                sequences = sequenceParser.Parse().ToList();
                sequenceParser.Close();


                if (distinct)
                {
                    sequences = sequences.Distinct().ToList();
                }
            }

            if (sequences != null && sequences.Count > 0 && molNames != null && molNames.Length > 0)
            {
                sequences = sequences.Where(a => molNames.Contains(SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID).Mol)).ToList();
            }

            return sequences;
        }

        public static List<ISequence> LoadSequenceFileList(List<string> sequenceFilenames, string[] molNames, bool distinct = true)
        {
            return LoadSequenceFileList(sequenceFilenames.ToArray(), molNames, distinct);
        }

        /// <summary>
        ///     This method loads multiple sequence files.  FASTA is the preferred format.
        /// </summary>
        /// <param name="sequenceFilenames"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static List<ISequence> LoadSequenceFileList(string[] sequenceFilenames, string[] molNames, bool distinct = true)
        {
            if (sequenceFilenames == null || sequenceFilenames.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceFilenames));
            }

            int[] numberSequencesLoaded;
            return LoadSequenceFileList(sequenceFilenames, molNames, out numberSequencesLoaded, distinct);
        }

        public static List<ISequence> LoadSequenceFileList(List<string> sequenceFilenames, string[] molNames, out int[] numberSequencesLoaded, bool distinct = true)
        {
            return LoadSequenceFileList(sequenceFilenames.ToArray(), molNames, out numberSequencesLoaded, distinct);
        }

        /// <summary>
        ///     This method loads multiple sequence files.  FASTA is the preferred format.
        /// </summary>
        /// <param name="sequenceFilenames"></param>
        /// <param name="numberSequencesLoaded"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static List<ISequence> LoadSequenceFileList(string[] sequenceFilenames, string[] molNames, out int[] numberSequencesLoaded, bool distinct = true)
        {
            if (sequenceFilenames == null || sequenceFilenames.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceFilenames));
            }

            var sequences = new List<ISequence>();
            numberSequencesLoaded = new int[sequenceFilenames.Length];

            for (int sequenceFilenameIndex = sequenceFilenames.GetLowerBound(0); sequenceFilenameIndex <= sequenceFilenames.GetUpperBound(0); sequenceFilenameIndex++)
            {
                List<ISequence> nextSequences = LoadSequenceFile(sequenceFilenames[sequenceFilenameIndex], molNames, distinct);

                if ((nextSequences != null) && (nextSequences.Count > 0))
                {
                    nextSequences = nextSequences.Where(a => !ProteinDataBankFileOperations.PdbIdBadList.Contains(SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID).PdbId.ToUpperInvariant())).ToList();

                    numberSequencesLoaded[sequenceFilenameIndex] = nextSequences.Count;
                    sequences.AddRange(nextSequences);
                }
                else
                {
                    numberSequencesLoaded[sequenceFilenameIndex] = 0;
                }
            }



            if (distinct)
            {
                sequences = sequences.Distinct().ToList();
            }

            return sequences;
        }


        /// <summary>
        ///     Save to disk a list of sequences in CSV/TSV format.
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="filename"></param>
        public static string[] SaveSequencesAsSpreadsheet(List<ISequence> sequences, string filename, bool tsvFormat = false, bool xlsxFormat = true)
        {
            if (sequences == null || sequences.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequences));
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentOutOfRangeException(nameof(filename));
            }

            if (!tsvFormat && !xlsxFormat)
            {
                throw new ArgumentOutOfRangeException(nameof(tsvFormat), tsvFormat, "No file formats were selected");
            }

            var headerColumnsRow = new[]
            {
                new SpreadsheetCell("PDB ID"),
                new SpreadsheetCell("Chain"),
                new SpreadsheetCell("Sequence"), 
            };

            var rowList = new List<SpreadsheetCell[]>();

            rowList.Add(headerColumnsRow);

            foreach (ISequence sequence in sequences)
            {
                SequenceIdSplit.SequenceIdToPdbIdAndChainIdResult id = SequenceIdSplit.SequenceIdToPdbIdAndChainId(sequence.ID);
                
                var row = new[]
                {
                    new SpreadsheetCell(id.PdbId),
                    new SpreadsheetCell(id.ChainId),
                    new SpreadsheetCell(sequence.ConvertToString()), 
                };

                rowList.Add(row);
            }

            string[] filesSavedStrings = SpreadsheetFileHandler.SaveSpreadsheet(filename, null, rowList, null, tsvFormat, xlsxFormat);

            return filesSavedStrings;
        }

        public static string AddSequenceAndProteinCountToFilename(List<ISequence> sequenceList, string saveFilename)
        {
            string path = Path.GetDirectoryName(saveFilename);
            if (!string.IsNullOrEmpty(path) && (path[path.Length - 1] != '\\' && path[path.Length - 1] != '/'))
            {
                path = path + "/";
            }

            string file = Path.GetFileNameWithoutExtension(saveFilename);
            string fileExt = Path.GetExtension(saveFilename);

            if (!string.IsNullOrEmpty(fileExt) && fileExt[0] != '.')
            {
                fileExt = "." + fileExt;
            }


            var pdbIdList = FilterProteins.SequenceListToPdbIdList(sequenceList);
            var totalPdbIds = pdbIdList.Count;
            var totalSequences = sequenceList.Count;

            string sequenceProteinStr = " [" + totalPdbIds + " proteins - " + totalSequences + " sequences]";

            if (!saveFilename.Contains(sequenceProteinStr))
            {
                saveFilename = path + file + sequenceProteinStr + fileExt;
            }

            return saveFilename;
        }

        /// <summary>
        ///     Save to disk a list of sequences in FASTA format.
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="saveFilename"></param>
        public static string SaveSequencesAsFasta(List<ISequence> sequences, string saveFilename, bool appendSequenceCountToFilename = true, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename, ProgressActionSet progressActionSet = null)
        {
            if (sequences == null) // || sequences.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sequences));
            }

            if (string.IsNullOrWhiteSpace(saveFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(saveFilename));
            }

            string result = null; // new List<string>();


            if (appendSequenceCountToFilename)
            {
                saveFilename = AddSequenceAndProteinCountToFilename(sequences, saveFilename);
            }

            // make sure directory exists
            var fileInfo = new FileInfo(saveFilename);

            if (fileInfo.Exists)
            {
                if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                {
                    fileInfo = new FileInfo(FileExistsHandler.FindNextFreeOutputFilename(fileInfo.FullName));

                    if (progressActionSet != null )
                    {
                        ProgressActionSet.Report("Save sequence: already exists, appended number: " + fileInfo.FullName, progressActionSet);
                    }
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                {
                    if (progressActionSet != null )
                    {
                        ProgressActionSet.Report("Save sequence: overwriting file: " + fileInfo.FullName, progressActionSet);
                    }
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                {
                    if (progressActionSet != null )
                    {
                        ProgressActionSet.Report("Save sequence: skipped file, already exists: " + fileInfo.FullName, progressActionSet);
                    }

                    return result;
                }
            }
            else
            {
                if (progressActionSet != null )
                {
                    ProgressActionSet.Report("Save sequence: new file: " + fileInfo.FullName, progressActionSet);
                }                
            }

            if (fileInfo.Directory != null) fileInfo.Directory.Create();


            var formatter = new FastAFormatter(fileInfo.FullName);
            formatter.Write(sequences);
            formatter.Close();
            result = fileInfo.FullName;


            return result;
        }
    }
}