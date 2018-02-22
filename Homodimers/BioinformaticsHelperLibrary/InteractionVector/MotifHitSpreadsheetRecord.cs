using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProproteinInterface;
using BioinformaticsHelperLibrary.TypeConversions;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class MotifHitSpreadsheetRecord
    {
        public string Motif;
        //public string MotifTooGeneral;
        public string TotalTimesSuggestedFwd;
        public string TotalTimesSuggestedRev;
        public string TotalTimesSuggestedMix;
        public string TotalTimesSuggestedOverall;

        //public string TotalTimesSuggestedFwdInHeterodimers;
        //public string TotalTimesSuggestedRevInHeterodimers;
        //public string TotalTimesSuggestedMixInHeterodimers;

        //public string TotalTimesSuggestedFwdInHomodimers;
        //public string TotalTimesSuggestedRevInHomodimers;
        //public string TotalTimesSuggestedMixInHomodimers;


        public string TotalDatabaseHitsPdb;
        public string TotalDatabaseSequencesPdb;
        public string TotalDatabaseHitsUniProtKbSwissProt;
        public string TotalDatabaseSequencesUniProtKbSwissProt;
        public string TotalDatabaseHitsUniProtKbTrEmbl;
        public string TotalDatabaseSequencesUniProtKbTrEmbl;
        public string TotalDatabaseHitsOverall;
        public string TotalSequencesOverall;

        //public string TotalMatchesFromMotifSource;
        //public string NumberRandomMatchesExpected;

        public string[] ToStrings()
        {
            var result = new[]
            {
                Motif,
                // MotifTooGeneral,
                TotalTimesSuggestedFwd,
                TotalTimesSuggestedRev,
                TotalTimesSuggestedMix,
                TotalTimesSuggestedOverall,

                //TotalTimesSuggestedFwdInHeterodimers,
                //TotalTimesSuggestedRevInHeterodimers,
                //TotalTimesSuggestedMixInHeterodimers,

                //TotalTimesSuggestedFwdInHomodimers,
                //TotalTimesSuggestedRevInHomodimers,
                //TotalTimesSuggestedMixInHomodimers,

                TotalDatabaseHitsPdb,
                TotalDatabaseSequencesPdb,
                TotalDatabaseHitsUniProtKbSwissProt,
                TotalDatabaseSequencesUniProtKbSwissProt,
                TotalDatabaseHitsUniProtKbTrEmbl,
                TotalDatabaseSequencesUniProtKbTrEmbl,
                TotalDatabaseHitsOverall,
                TotalSequencesOverall,
            };
            return result;
        }

        public static List<MotifHitSpreadsheetRecord> MotifRecordList(Dictionary<string, MotifCounter> motifDistinctWithCount)
        {
            if (motifDistinctWithCount == null) throw new ArgumentNullException(nameof(motifDistinctWithCount));

            var result = new List<MotifHitSpreadsheetRecord>();

            foreach (var kvp in motifDistinctWithCount)
            {
                var motif = kvp.Key;
                var motifCounter = kvp.Value;

                ProproteinInterfaceMatchSet pdb = null;
                ProproteinInterfaceMatchSet sp = null;
                ProproteinInterfaceMatchSet tr = null;

                var tasks = new List<Task>()
                {
                    Task.Run(() => pdb = ProproteinInterfaceServiceClient.LoadProproteinInterfaceResponse(new ScanProproteinInterfaceParameters() {sig = motif, db = ScanProproteinInterfaceParameters.TargetProteinDatabases.ProteinDataBank})),
                    Task.Run(() => sp = ProproteinInterfaceServiceClient.LoadProproteinInterfaceResponse(new ScanProproteinInterfaceParameters() {sig = motif, db = ScanProproteinInterfaceParameters.TargetProteinDatabases.UniProtKbSwissProt})),
                    Task.Run(() => tr = ProproteinInterfaceServiceClient.LoadProproteinInterfaceResponse(new ScanProproteinInterfaceParameters() {sig = motif, db = ScanProproteinInterfaceParameters.TargetProteinDatabases.UniProtKbTrEmbl})),
                };
                Task.WaitAll(tasks.Where(t=>!t.IsCompleted&&!t.IsCanceled&&!t.IsFaulted).ToArray());

                var totalPdbMatch = pdb != null ? ProteinDataBankFileOperations.NullableTryParseInt32(pdb.NMatch) : null;
                if (totalPdbMatch == null) totalPdbMatch = -1;

                var totalSpMatch = sp != null ? ProteinDataBankFileOperations.NullableTryParseInt32(sp.NMatch) : null;
                if (totalSpMatch == null) totalSpMatch = -1;

                var totalTrMatch = tr != null ? ProteinDataBankFileOperations.NullableTryParseInt32(tr.NMatch) : null;
                if (totalTrMatch == null) totalTrMatch = -1;

                var totalMatchOverall = ((totalPdbMatch > -1 ? totalPdbMatch : 0) + (totalSpMatch > -1 ? totalSpMatch : 0) + (totalTrMatch > -1 ? totalTrMatch : 0));
                if (totalPdbMatch == -1 && totalSpMatch == -1 && totalTrMatch == -1) totalMatchOverall = -1;

                var totalPdbSeq = pdb != null ? ProteinDataBankFileOperations.NullableTryParseInt32(pdb.NSeq) : null;
                if (totalPdbSeq == null) totalPdbSeq = -1;

                var totalSpSeq = sp != null ? ProteinDataBankFileOperations.NullableTryParseInt32(sp.NSeq) : null;
                if (totalSpSeq == null) totalSpSeq = -1;

                var totalTrSeq = tr != null ? ProteinDataBankFileOperations.NullableTryParseInt32(tr.NSeq) : null;
                if (totalTrSeq == null) totalTrSeq = -1;

                var totalSeqOverall = ((totalPdbSeq > -1 ? totalPdbSeq : 0) + (totalSpSeq > -1 ? totalSpSeq : 0) + (totalTrSeq > -1 ? totalTrSeq : 0));
                if (totalPdbSeq == -1 && totalSpSeq == -1 && totalTrSeq == -1) totalSeqOverall = -1;

                var totalTimesOverall = (motifCounter.TotalFwd + motifCounter.TotalRev + motifCounter.TotalMix);
                var record = new MotifHitSpreadsheetRecord()
                {
                    Motif = kvp.Key,
                    //MotifTooGeneral = "" + kvp.Value.MotifTooGeneral,
                    TotalTimesSuggestedFwd = "" + motifCounter.TotalFwd,
                    TotalTimesSuggestedRev = "" + motifCounter.TotalRev,
                    TotalTimesSuggestedMix = "" + motifCounter.TotalMix,
                    TotalTimesSuggestedOverall = "" + totalTimesOverall,

                    //TotalTimesSuggestedFwdInHeterodimers = "" + motifCounter.TotalFwdInHeterodimers,
                    //TotalTimesSuggestedRevInHeterodimers = "" + motifCounter.TotalRevInHeterodimers,
                    //TotalTimesSuggestedMixInHeterodimers = "" + motifCounter.TotalMixInHeterodimers,

                    //TotalTimesSuggestedFwdInHomodimers = "" + motifCounter.TotalFwdInHomodimers,
                    //TotalTimesSuggestedRevInHomodimers = "" + motifCounter.TotalRevInHomodimers,
                    //TotalTimesSuggestedMixInHomodimers = "" + motifCounter.TotalMixInHomodimers,

                    TotalDatabaseHitsPdb = "" + totalPdbMatch,
                    TotalDatabaseSequencesPdb = "" + totalPdbSeq,
                    TotalDatabaseHitsUniProtKbSwissProt = "" + totalSpMatch,
                    TotalDatabaseSequencesUniProtKbSwissProt = "" + totalSpSeq,
                    TotalDatabaseHitsUniProtKbTrEmbl = "" + totalTrMatch,
                    TotalDatabaseSequencesUniProtKbTrEmbl = "" + totalTrSeq,

                    TotalDatabaseHitsOverall = "" + totalMatchOverall,
                    TotalSequencesOverall = "" + totalSeqOverall,
                };

                result.Add(record);
            }

            return result;
        }



        public static Dictionary<string, MotifCounter> MotifDistinctWithCount(List<ProproteinInterfaceSpreadsheetRecord> proproteinInterfaceSpreadsheetRecordList)
        {
            if (proproteinInterfaceSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(proproteinInterfaceSpreadsheetRecordList));

            var result = new Dictionary<string, MotifCounter>();

            foreach (var record in proproteinInterfaceSpreadsheetRecordList)
            {
                var motifs = record.Motifs().Distinct().ToArray();

                foreach (var motif in motifs)
                {
                    if (!result.ContainsKey(motif))
                    {
                        var motifCounter = new MotifCounter()
                        {
                            Motif = motif,
                            MotifTooGeneral = ProproteinInterfaceMotif.IsMotifTooGeneral(motif),
                            TotalFwd = 0,
                            TotalRev = 0,
                            TotalMix = 0,
                        };
                        result.Add(motif, motifCounter);
                    }

                    if (record.Direction == "Fwd")
                    {
                        result[motif].TotalFwd++;
                    }
                    else if (record.Direction == "Rev")
                    {
                        result[motif].TotalRev++;
                    }
                    else if (record.Direction == "Mix")
                    {
                        result[motif].TotalMix++;
                    }
                }
            }

            return result;
        }

        public static MotifHitSpreadsheetRecord Header()
        {
            var result = new MotifHitSpreadsheetRecord()
            {
                Motif = "Motif",
                //MotifTooGeneral = "Very General?",
                TotalTimesSuggestedFwd = "Total Fwd",
                TotalTimesSuggestedRev = "Total Rev",
                TotalTimesSuggestedMix = "Total Mix",
                TotalTimesSuggestedOverall = "Total Overall",
                //TotalTimesSuggestedFwdInHeterodimers = "Total Fwd In Heterodimers",
                //TotalTimesSuggestedRevInHeterodimers = "Total Rev In Heterodimers",
                //TotalTimesSuggestedMixInHeterodimers = "Total Mix In Heterodimers",

                //TotalTimesSuggestedFwdInHomodimers = "Total Fwd In Homodimers",
                //TotalTimesSuggestedRevInHomodimers = "Total Rev In Homodimers",
                //TotalTimesSuggestedMixInHomodimers = "Total Mix In Homodimers",


                TotalDatabaseHitsPdb = "PDB Hits",
                TotalDatabaseSequencesPdb = "PDB Seq",
                TotalDatabaseHitsUniProtKbSwissProt = "SwissProt Hits",
                TotalDatabaseSequencesUniProtKbSwissProt = "SwissProt Seq",
                TotalDatabaseHitsUniProtKbTrEmbl = "TrEMBL Hits",
                TotalDatabaseSequencesUniProtKbTrEmbl = "TrEMBL Seq",
                TotalDatabaseHitsOverall = "Overall Hits",
                TotalSequencesOverall = "Overall Seq",
            };

            return result;
        }

        public static string[,] MotifSpreadsheet(List<MotifHitSpreadsheetRecord> motifHitSpreadsheetRecordList)
        {
            if (motifHitSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(motifHitSpreadsheetRecordList));

            var result = new List<string[]>
            {
                Header().ToStrings()
            };

            foreach (var record in motifHitSpreadsheetRecordList
                .OrderByDescending(a => ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalDatabaseHitsPdb) ? "0" : a.TotalDatabaseHitsPdb)
                                        + ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalDatabaseHitsUniProtKbSwissProt) ? "0" : a.TotalDatabaseHitsUniProtKbSwissProt)
                                        + ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalDatabaseHitsUniProtKbTrEmbl) ? "0" : a.TotalDatabaseHitsUniProtKbTrEmbl))

                .ThenByDescending(a => ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalDatabaseSequencesPdb) ? "0" : a.TotalDatabaseSequencesPdb)
                                       + ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalDatabaseSequencesUniProtKbSwissProt) ? "0" : a.TotalDatabaseSequencesUniProtKbSwissProt)
                                       + ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalDatabaseSequencesUniProtKbTrEmbl) ? "0" : a.TotalDatabaseSequencesUniProtKbTrEmbl))

                .ThenByDescending(a => ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalTimesSuggestedFwd) ? "0" : a.TotalTimesSuggestedFwd)
                                       + ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalTimesSuggestedRev) ? "0" : a.TotalTimesSuggestedRev)
                                       + ProteinDataBankFileOperations.NullableTryParseInt32(string.IsNullOrWhiteSpace(a.TotalTimesSuggestedMix) ? "0" : a.TotalTimesSuggestedMix)))
            {
                result.Add(record.ToStrings());
            }

            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }
    }
}
