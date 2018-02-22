using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.TypeConversions;

namespace BioinformaticsHelperLibrary.InteractionVector
{

    public class ProproteinInterfaceSpreadsheetRecord
    {
        public string MotifName;
        public string MotifSource;
        public string TotalFound;
        public string ProteinInterfaceLength;
        public string Direction;

        public string MotifAminoAcids;
        public string MotifInteractionAminoAcids;
        public string MotifNonInteractionAminoAcids;

        public string MotifPhysicochemical;
        public string MotifInteractionPhysicochemical;
        public string MotifNonInteractionPhysicochemical;

        public string MotifHydrophobicity;
        public string MotifInteractionHydrophobicity;
        public string MotifNonInteractionHydrophobicity;

        public string MotifPdbSum;
        public string MotifInteractionPdbSum;
        public string MotifNonInteractionPdbSum;

        public string MotifUniProtKb;
        public string MotifInteractionUniProtKb;
        public string MotifNonInteractionUniProtKb;

        public string[] MotifCommonProperties = new string[12];
        public string[] MotifInteractionsCommonProperties = new string[12];
        public string[] MotifNonInteractionsCommonProperties = new string[12];

        public static ProproteinInterfaceSpreadsheetRecord Header()
        {
            var header = new ProproteinInterfaceSpreadsheetRecord
            {
                MotifName = "Motif Name",
                MotifSource = "Motif Source",
                TotalFound = "Total Found",
                Direction = "Direction",
                ProteinInterfaceLength = "ProteinInterface Length",

                MotifAminoAcids = "Amino Acids Motif",
                MotifInteractionAminoAcids = "Interaction Amino Acids Motif",
                MotifNonInteractionAminoAcids = "Non Interaction Amino Acids Motif",

                MotifPhysicochemical = "Physicochemical Motif",
                MotifInteractionPhysicochemical = "Interaction Physicochemical Motif",
                MotifNonInteractionPhysicochemical = "Non Interaction Physicochemical Motif",

                MotifHydrophobicity = "Hydrophobicity Motif",
                MotifInteractionHydrophobicity = "Interaction Hydrophobicity Motif",
                MotifNonInteractionHydrophobicity = "Non Interaction Hydrophobicity Motif",

                MotifPdbSum = "PDBsum Motif",
                MotifInteractionPdbSum = "Interaction PDBsum Motif",
                MotifNonInteractionPdbSum = "Non Interaction PDBsum Motif",

                MotifUniProtKb = "UniProtKb Motif",
                MotifInteractionUniProtKb = "Interaction UniProtKb Motif",
                MotifNonInteractionUniProtKb = "Non Interaction UniProtKb Motif",


            };

            for (var index = header.MotifCommonProperties.Length - 1; index >= 6; index--)
            {
                header.MotifCommonProperties[index] = "Common Properties " + (index + 1) + " / " + header.MotifCommonProperties.Length;
            }

            for (var index = header.MotifInteractionsCommonProperties.Length - 1; index >= 6; index--)
            {
                header.MotifInteractionsCommonProperties[index] = "Interaction Common Properties " + (index + 1) + " / " + header.MotifInteractionsCommonProperties.Length;
            }

            for (var index = header.MotifNonInteractionsCommonProperties.Length - 1; index >= 6; index--)
            {
                header.MotifNonInteractionsCommonProperties[index] = "Non Interaction Common Properties " + (index + 1) + " / " + header.MotifNonInteractionsCommonProperties.Length;
            }

            return header;
        }

        public string[] ToStrings()
        {
            var result = new List<string>()
            {
                MotifName,
                MotifSource,
                TotalFound,
                Direction,
                ProteinInterfaceLength,

                MotifAminoAcids,
                MotifInteractionAminoAcids,
                MotifNonInteractionAminoAcids,

                MotifPhysicochemical,
                MotifInteractionPhysicochemical,
                MotifNonInteractionPhysicochemical,

                MotifHydrophobicity,
                MotifInteractionHydrophobicity,
                MotifNonInteractionHydrophobicity,

                MotifPdbSum,
                MotifInteractionPdbSum,
                MotifNonInteractionPdbSum,

                MotifUniProtKb,
                MotifInteractionUniProtKb,
                MotifNonInteractionUniProtKb,
            };

            for (var index = MotifCommonProperties.Length - 1; index >= 6; index--)
            {
                result.Add(MotifCommonProperties[index]);
                result.Add(MotifInteractionsCommonProperties[index]);
                result.Add(MotifNonInteractionsCommonProperties[index]);
            }

            return result.ToArray();

        }

        public string[] Motifs()
        {
            var result = new List<string>()
            {
                MotifAminoAcids,
                MotifInteractionAminoAcids,
                MotifNonInteractionAminoAcids,

                MotifPhysicochemical,
                MotifInteractionPhysicochemical,
                MotifNonInteractionPhysicochemical,

                MotifHydrophobicity,
                MotifInteractionHydrophobicity,
                MotifNonInteractionHydrophobicity,

                MotifPdbSum,
                MotifInteractionPdbSum,
                MotifNonInteractionPdbSum,

                MotifUniProtKb,
                MotifInteractionUniProtKb,
                MotifNonInteractionUniProtKb,
            };

            for (var index = MotifCommonProperties.Length - 1; index >= 6; index--)
            {
                result.Add(MotifCommonProperties[index]);
                result.Add(MotifInteractionsCommonProperties[index]);
                result.Add(MotifNonInteractionsCommonProperties[index]);
            }

            return result.Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();

        }

        public static ProproteinInterfaceSpreadsheetRecord Record(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList)
        {
            if (vectorProteinInterfaceWholeList == null || vectorProteinInterfaceWholeList.Count == 0)
            {
                return null;
            }

            var proteinInterfaceSequenceList = vectorProteinInterfaceWholeList.Select(a => a.ProteinInterfaceAminoAcids1L()).ToList();
            var proteinInterfaceInteractionSequenceList = vectorProteinInterfaceWholeList.Select(a => string.Join("", a.ProteinInterfaceAminoAcids1L().Select((b, i) => a.InteractionBools()[i] ? b : ' ').ToList())).ToList();
            var proteinInterfaceNonInteractionSequenceList = vectorProteinInterfaceWholeList.Select(a => string.Join("", a.ProteinInterfaceAminoAcids1L().Select((b, i) => !a.InteractionBools()[i] ? b : ' ').ToList())).ToList();


            var distinctProteinInterfaceLengths = vectorProteinInterfaceWholeList.Select(a => a.ProteinInterfaceLength).Distinct().ToArray();

            var directionFwd = vectorProteinInterfaceWholeList.Count(a => !a.ReversedSequence);
            var directionRev = vectorProteinInterfaceWholeList.Count - directionFwd;

            string direction = "";

            if (directionFwd > 0 && directionRev == 0)
            {
                direction = "Fwd";
            }
            else if (directionFwd == 0 && directionRev > 0)
            {
                direction = "Rev";
            }
            else
            {
                direction = "Mix";
            }

            var record = new ProproteinInterfaceSpreadsheetRecord
            {
                MotifName = "",
                MotifSource = "",
                TotalFound = "" + vectorProteinInterfaceWholeList.Count,
                Direction = direction,
                ProteinInterfaceLength = "" + string.Join(", ", distinctProteinInterfaceLengths),

                MotifAminoAcids = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidMotif(proteinInterfaceSequenceList),
                MotifInteractionAminoAcids = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidMotif(proteinInterfaceInteractionSequenceList),
                MotifNonInteractionAminoAcids = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidMotif(proteinInterfaceNonInteractionSequenceList),

                MotifPhysicochemical = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical, proteinInterfaceSequenceList),
                MotifInteractionPhysicochemical = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical, proteinInterfaceInteractionSequenceList),
                MotifNonInteractionPhysicochemical = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical, proteinInterfaceNonInteractionSequenceList),

                MotifHydrophobicity = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity, proteinInterfaceSequenceList),
                MotifInteractionHydrophobicity = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity, proteinInterfaceInteractionSequenceList),
                MotifNonInteractionHydrophobicity = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity, proteinInterfaceNonInteractionSequenceList),

                MotifPdbSum = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum, proteinInterfaceSequenceList),
                MotifInteractionPdbSum = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum, proteinInterfaceInteractionSequenceList),
                MotifNonInteractionPdbSum = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum, proteinInterfaceNonInteractionSequenceList),

                MotifUniProtKb = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb, proteinInterfaceSequenceList),
                MotifInteractionUniProtKb = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb, proteinInterfaceInteractionSequenceList),
                MotifNonInteractionUniProtKb = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb, proteinInterfaceNonInteractionSequenceList),
            };


            for (var index = record.MotifCommonProperties.Length - 1; index >= 6; index--)
            {
                record.MotifCommonProperties[index] = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidCommonPropertiesMotif(proteinInterfaceSequenceList, AminoAcidPropertyMatchType.MininumMatch, index + 1);
                record.MotifInteractionsCommonProperties[index] = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidCommonPropertiesMotif(proteinInterfaceInteractionSequenceList, AminoAcidPropertyMatchType.MininumMatch, index + 1);
                record.MotifNonInteractionsCommonProperties[index] = ProproteinInterfaceMotif.FindProteinInterfaceAminoAcidCommonPropertiesMotif(proteinInterfaceNonInteractionSequenceList, AminoAcidPropertyMatchType.MininumMatch, index + 1);
            }

            return record;
        }

        public static List<ProproteinInterfaceSpreadsheetRecord> MotifSpreadsheetData(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));

            var result = new List<ProproteinInterfaceSpreadsheetRecord>();

            // motif by proteinInterface length
            var distinctProteinInterfaceLengths = vectorProteinInterfaceWholeList.Select(a => a.ProteinInterfaceLength).Distinct().ToArray();
            foreach (var proteinInterfaceLength in distinctProteinInterfaceLengths)
            {
                for (var index = 0; index < 3; index++)
                {
                    ProproteinInterfaceSpreadsheetRecord record;
                    if (index == 0)
                    {
                        record = Record(vectorProteinInterfaceWholeList.Where(b => b.ProteinInterfaceLength == proteinInterfaceLength).ToList());
                    }
                    else if (index == 1)
                    {
                        record = Record(vectorProteinInterfaceWholeList.Where(b => b.ProteinInterfaceLength == proteinInterfaceLength && !b.ReversedSequence).ToList());
                    }
                    else if (index == 2)
                    {
                        record = Record(vectorProteinInterfaceWholeList.Where(b => b.ProteinInterfaceLength == proteinInterfaceLength && b.ReversedSequence).ToList());
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }

                    if (record == null) continue;
                    
                    record.MotifName = "ProteinInterface Length " + proteinInterfaceLength;
                    record.MotifSource = "Length";
                    result.Add(record);
                }
            }

            // motif by common secondary structure pattern
            var distinctSecondaryStructures = vectorProteinInterfaceWholeList.Select(a => a.SecondaryStructure).Distinct().ToList();
            foreach (var secondaryStructure in distinctSecondaryStructures)
            {
                for (var index = 0; index < 3; index++)
                {
                    ProproteinInterfaceSpreadsheetRecord record;
                    if (index == 0)
                    {
                        record = Record(vectorProteinInterfaceWholeList.Where(b => b.SecondaryStructure == secondaryStructure).ToList());
                    }
                    else if (index == 1)
                    {
                        record = Record(vectorProteinInterfaceWholeList.Where(b => b.SecondaryStructure == secondaryStructure && !b.ReversedSequence).ToList());
                    }
                    else if (index == 2)
                    {
                        record = Record(vectorProteinInterfaceWholeList.Where(b => b.SecondaryStructure == secondaryStructure && b.ReversedSequence).ToList());
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }

                    if (record == null) continue;

                    record.MotifName = secondaryStructure;
                    record.MotifSource = "Secondary Structure";
                    result.Add(record);
                }
            }

            // motif by common interaction vector pattern
            for (var vectorType = 0; vectorType < 4; vectorType++)
            {
                var distinctVectors = vectorProteinInterfaceWholeList.Select(a => a.VectorString(vectorType)).Distinct().ToList();
                foreach (var vector in distinctVectors)
                {
                    for (var index = 0; index < 3; index++)
                    {
                        ProproteinInterfaceSpreadsheetRecord record;
                        if (index == 0)
                        {
                            record = Record(vectorProteinInterfaceWholeList.Where(b => b.VectorString(vectorType) == vector).ToList());
                        }
                        else if (index == 1)
                        {
                            record = Record(vectorProteinInterfaceWholeList.Where(b => b.VectorString(vectorType) == vector && !b.ReversedSequence).ToList());
                        }
                        else if (index == 2)
                        {
                            record = Record(vectorProteinInterfaceWholeList.Where(b => b.VectorString(vectorType) == vector && b.ReversedSequence).ToList());
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }

                        if (record == null) continue;

                        record.MotifName = vector;
                        record.MotifSource = VectorProteinInterfaceWhole.VectorStringDescription(vectorType);
                        result.Add(record);
                    }
                }
            }

            return result;
        }

        public static string[,] MotifSpreadsheet(List<ProproteinInterfaceSpreadsheetRecord> proproteinInterfaceSpreadsheetRecordList)
        {
            if (proproteinInterfaceSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(proproteinInterfaceSpreadsheetRecordList));

            var result = new List<string[]>();

            result.Add(Header().ToStrings());

            result.AddRange(proproteinInterfaceSpreadsheetRecordList.OrderByDescending(a=> ProteinDataBankFileOperations.NullableTryParseInt32(a.TotalFound)).Select(record => record.ToStrings()));

            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }
    }

}
