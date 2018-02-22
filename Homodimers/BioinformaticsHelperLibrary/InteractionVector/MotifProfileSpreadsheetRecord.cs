using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio.Util;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.TypeConversions;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class MotifProfileSpreadsheetRecord
    {
        public string MotifName;
        public string MotifSource;
        public string Direction;
        public string TotalFound;
        //public string TotalFoundInHeterodimers;
        //public string TotalFoundInHomodimers;
        public decimal[][] AminoAcidProfile;
        public decimal[] AverageProfile;

        public static string[,] Spreadsheet(List<MotifProfileSpreadsheetRecord> motifProfileSpreadsheetRecordList)
        {
            if (motifProfileSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(motifProfileSpreadsheetRecordList));

            var result = new List<string[]>();

            var totalAminoAcids = AminoAcidTotals.TotalAminoAcids();

            var sheetHeader = new List<string>()
            {
                "Motif Name",
                "Motif Source",
                "Direction",
                "Total Found",
                //"Total Found In Heterodimers",
                //"Total Found In Homodimers",
                "Profile Position",
            };

            sheetHeader.AddRange(AminoAcidConversions.AminoAcidCodeArray1L());

            result.Add(sheetHeader.ToArray());

            foreach (var record in motifProfileSpreadsheetRecordList.OrderByDescending(a=> ProteinDataBankFileOperations.NullableTryParseInt32(a.TotalFound)))
            {
                result.Add(new string[] { });

                var recordHeader = new List<string>()
                {
                    record.MotifName,
                    record.MotifSource,
                    record.Direction,
                    record.TotalFound,
                    //record.TotalFoundInHeterodimers,
                    //record.TotalFoundInHomodimers,
                    "",
                };

                recordHeader.AddRange(AminoAcidConversions.AminoAcidCodeArray1L());

                result.Add(recordHeader.ToArray());

                for (var positionIndex = 0; positionIndex < record.AminoAcidProfile.Length; positionIndex++)
                {
                    var row = new string[sheetHeader.Count];

                    row[sheetHeader.IndexOf("Profile Position")] = ""  + (positionIndex + 1);

                    for (var aaIndex = 0; aaIndex < record.AminoAcidProfile[positionIndex].Length; aaIndex++)
                    {
                        row[aaIndex + sheetHeader.IndexOf("Profile Position") + 1] = $"{record.AminoAcidProfile[positionIndex][aaIndex]:0.00}";
                    }

                    result.Add(row);
                }

                var rowAverage = new string[sheetHeader.Count];

                rowAverage[sheetHeader.IndexOf("Profile Position")] = "Average";

                for (var aaIndex = 0; aaIndex < record.AverageProfile.Length; aaIndex++)
                {
                    rowAverage[aaIndex + sheetHeader.IndexOf("Profile Position") + 1] = $"{record.AverageProfile[aaIndex]:0.00}";
                }

                result.Add(rowAverage);

            }


            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }

        public static MotifProfileSpreadsheetRecord Record(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList)
        {
            if (vectorProteinInterfaceWholeList == null || vectorProteinInterfaceWholeList.Count == 0) { return null; }
        
            var maxProteinInterfaceLength = vectorProteinInterfaceWholeList.Select(a => a.ProteinInterfaceAminoAcids1L().Length).Max();

            var totalAminoAcids = AminoAcidTotals.TotalAminoAcids();

            var result = new MotifProfileSpreadsheetRecord
            {
                AminoAcidProfile = new decimal[maxProteinInterfaceLength][],
                AverageProfile = new decimal[totalAminoAcids],
                TotalFound = "" + vectorProteinInterfaceWholeList.Count,
            };

            var directionFwd = vectorProteinInterfaceWholeList.Count(a => !a.ReversedSequence);
            var directionRev = vectorProteinInterfaceWholeList.Count - directionFwd;

            if (directionFwd > 0 && directionRev == 0)
            {
                result.Direction = "Fwd";
            }
            else if (directionFwd == 0 && directionRev > 0)
            {
                result.Direction = "Rev";
            }
            else
            {
                result.Direction = "Mix";
            }

            for (var positionIndex = 0; positionIndex < result.AminoAcidProfile.Length; positionIndex++)
            {
                result.AminoAcidProfile[positionIndex] = new decimal[totalAminoAcids];
            }

            foreach (var record in vectorProteinInterfaceWholeList)
            {
                var aminoAcidCode1L = record.ProteinInterfaceAminoAcids1L();

                for (int positionIndex = 0; positionIndex < aminoAcidCode1L.Length; positionIndex++)
                {
                    var aa = aminoAcidCode1L[positionIndex];

                    var aaIndex = AminoAcidConversions.AminoAcidNameToNumber(aa) - 1;

                    result.AminoAcidProfile[positionIndex][aaIndex]++;
                    result.AverageProfile[aaIndex]++;

                }
            }

            for (var positionIndex = 0; positionIndex < result.AminoAcidProfile.Length; positionIndex++)
            {
                var positionTotal = result.AminoAcidProfile[positionIndex].Sum();

                for (var aaIndex = 0; aaIndex < totalAminoAcids; aaIndex++)
                {
                    result.AminoAcidProfile[positionIndex][aaIndex] = (result.AminoAcidProfile[positionIndex][aaIndex]/positionTotal)*100;
                }
            }

            var averageTotal = result.AverageProfile.Sum();

            for (var aaIndex = 0; aaIndex < totalAminoAcids; aaIndex++)
            {
                result.AverageProfile[aaIndex] = averageTotal != 0 ? (result.AverageProfile[aaIndex] / averageTotal) * 100 : 0;
            }

            return result;
        }


        public static List<MotifProfileSpreadsheetRecord> MotifSpreadsheetData(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));

            var result = new List<MotifProfileSpreadsheetRecord>();

            // motif by proteinInterface length
            var distinctProteinInterfaceLengths = vectorProteinInterfaceWholeList.Select(a => a.ProteinInterfaceLength).Distinct().ToArray();
            foreach (var proteinInterfaceLength in distinctProteinInterfaceLengths)
            {
                for (var index = 0; index < 3; index++)
                {
                    MotifProfileSpreadsheetRecord record;
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
                    MotifProfileSpreadsheetRecord record;
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
                        MotifProfileSpreadsheetRecord record;
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
    }
}
