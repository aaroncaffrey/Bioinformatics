using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.Extensions;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.TypeConversions;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class AminoAcidDistributionSpreadsheetRecord
    {
        public string Pattern;
        public string Type;
        public string Metric;
        public decimal NumberOfSamples;
        //public decimal NumberOfSamplesInHeterodimers;
        //public decimal NumberOfSamplesInHomodimers;
        public decimal TotalAminoAcids;

        public decimal[][] Distribution; // set rule, category number

        public AminoAcidDistributionSpreadsheetRecord()
        {
            Distribution = new decimal[AminoAcidGroups.AminoAcidGroups.GetTotalGroups()][];
            for (var index = 0; index < Distribution.Length; index++)
            {
                Distribution[index] = new decimal[AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(index)];
            }
        }

        public string[] ToStrings()
        {
            var result = new List<string>()
                {
                    Pattern,
                    Type,
                    Metric,
                    $"{NumberOfSamples:0.##}",
                    //$"{NumberOfSamplesInHeterodimers:0.##}",
                    //$"{NumberOfSamplesInHomodimers:0.##}",
                    $"{TotalAminoAcids:0.##}",
                    "-",
                };


            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                result.AddRange(Distribution[(int)enumAminoAcidGroups].Select(a => $"{a:0.00}").ToArray());
                result.Add("-");
            }

            return result.ToArray();
        }

        public static string[] Header()
        {
            var result = new List<string>()
            {
                "Pattern",
                "Type",
                "Metric",
                "Number of Samples",
                //"Number of Samples In Heterodimers",
                //"Number of Samples In Homodimers",
                "Total Amino Acids",
                "-",
            };

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                result.AddRange(AminoAcidGroups.AminoAcidGroups.GetSubgroupDescriptions(enumAminoAcidGroups));
                result.Add("-");
            }

            return result.ToArray();
        }

        public static AminoAcidDistributionSpreadsheetRecord UniProtKb()
        {
            // UniProtKB/TrEMBL composition
            var record = new AminoAcidDistributionSpreadsheetRecord
            {
                Pattern = "UniProtKB/TrEMBL",
                Type = "UniProtKB/TrEMBL",
                Metric = "UniProtKB/TrEMBL",
                TotalAminoAcids = UniProtProteinDatabaseComposition.TotalAminoAcidsInDatabase,
                NumberOfSamples = UniProtProteinDatabaseComposition.TotalSamplesInDatabase,
            };

            var uniprotComposition = UniProtProteinDatabaseComposition.AminoAcidCompositionAsChain();

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                var groupItemsTotal = AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups);

                for (var groupItemIndex = 0; groupItemIndex < groupItemsTotal; groupItemIndex++)
                {
                    record.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = uniprotComposition.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                }
            }

            return record;
        }

        public static List<AminoAcidDistributionSpreadsheetRecord> OverallDistributionRecords(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, List<string> pdbIdList, List<ISequence> seqList/*, int vectorType*/)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (pdbIdList == null) throw new ArgumentNullException(nameof(pdbIdList));
            if (seqList == null) throw new ArgumentNullException(nameof(seqList));

            var result = new List<AminoAcidDistributionSpreadsheetRecord>();

            // over all composition
            var recordAllComposition = new AminoAcidDistributionSpreadsheetRecord();
            var recordProteinInterfaceComposition = new AminoAcidDistributionSpreadsheetRecord();
            var recordProteinInterfaceInteractionComposition = new AminoAcidDistributionSpreadsheetRecord();
            var recordProteinInterfaceNonInteractionComposition = new AminoAcidDistributionSpreadsheetRecord();

            result.Add(recordAllComposition);
            result.Add(recordProteinInterfaceComposition);
            result.Add(recordProteinInterfaceInteractionComposition);
            result.Add(recordProteinInterfaceNonInteractionComposition);

            var allComposition = new AminoAcidChainComposition();
            var proteinInterfaceComposition = new AminoAcidChainComposition();
            var proteinInterfaceInteractionComposition = new AminoAcidChainComposition();
            var proteinInterfaceNonInteractionComposition = new AminoAcidChainComposition();

            foreach (var chain in seqList)
            {
                var seq = chain.ConvertToString();
                foreach (var c in seq)
                {
                    allComposition.IncrementAminoAcidCount(c);
                }
            }

            foreach (var vectorProteinInterfaceWhole in vectorProteinInterfaceWholeList)
            {
                var aminoAcids1L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids1L();
                var interactionBools = vectorProteinInterfaceWhole.InteractionBools();

                for (int index = 0; index < aminoAcids1L.Length; index++)
                {
                    var c = aminoAcids1L[index];

                    proteinInterfaceComposition.IncrementAminoAcidCount(c);

                    if (interactionBools[index])
                    {
                        proteinInterfaceInteractionComposition.IncrementAminoAcidCount(c);
                    }
                    else
                    {
                        proteinInterfaceNonInteractionComposition.IncrementAminoAcidCount(c);
                    }
                }
            }

            recordAllComposition.Pattern = "*";
            recordProteinInterfaceComposition.Pattern = "*";
            recordProteinInterfaceInteractionComposition.Pattern = "*";
            recordProteinInterfaceNonInteractionComposition.Pattern = "*";

            recordAllComposition.Type = "*";
            recordProteinInterfaceComposition.Type = "*";
            recordProteinInterfaceInteractionComposition.Type = "*";
            recordProteinInterfaceNonInteractionComposition.Type = "*";

            recordAllComposition.Metric = "Proteins";
            recordProteinInterfaceComposition.Metric = "ProteinInterfaces";
            recordProteinInterfaceInteractionComposition.Metric = "ProteinInterface interactions";
            recordProteinInterfaceNonInteractionComposition.Metric = "ProteinInterface non interactions";

            // number of samples
            recordAllComposition.NumberOfSamples = seqList.Count;
            recordProteinInterfaceComposition.NumberOfSamples = vectorProteinInterfaceWholeList.Count;
            recordProteinInterfaceInteractionComposition.NumberOfSamples = vectorProteinInterfaceWholeList.Count;
            recordProteinInterfaceNonInteractionComposition.NumberOfSamples = vectorProteinInterfaceWholeList.Count;

            // number of amino acids
            recordAllComposition.TotalAminoAcids = allComposition.AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();
            recordProteinInterfaceComposition.TotalAminoAcids = proteinInterfaceComposition.AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();
            recordProteinInterfaceInteractionComposition.TotalAminoAcids = proteinInterfaceInteractionComposition.AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();
            recordProteinInterfaceNonInteractionComposition.TotalAminoAcids = proteinInterfaceNonInteractionComposition.AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();

            // convert composition to percentage
            var allCompositionPercentage = AminoAcidChainComposition.ConvertToPercentage(allComposition);
            var proteinInterfaceCompositionPercentage = AminoAcidChainComposition.ConvertToPercentage(proteinInterfaceComposition);
            var proteinInterfaceInteractionCompositionPercentage = AminoAcidChainComposition.ConvertToPercentage(proteinInterfaceInteractionComposition);
            var proteinInterfaceNonInteractionCompositionPercentage = AminoAcidChainComposition.ConvertToPercentage(proteinInterfaceNonInteractionComposition);

            // output composition to result spreadsheet matrix

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                var groupItemsTotal = AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups);

                for (var groupItemIndex = 0; groupItemIndex < groupItemsTotal; groupItemIndex++)
                {
                    recordAllComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = allCompositionPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                    recordProteinInterfaceComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = proteinInterfaceCompositionPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                    recordProteinInterfaceInteractionComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = proteinInterfaceInteractionCompositionPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                    recordProteinInterfaceNonInteractionComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = proteinInterfaceNonInteractionCompositionPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                }
            }

            return result;
        }


        public static List<AminoAcidDistributionSpreadsheetRecord> PatternDistributionSpreadsheetRecords(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, List<string> pdbIdList, List<ISequence> seqList, int vectorType)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (pdbIdList == null) throw new ArgumentNullException(nameof(pdbIdList));
            if (seqList == null) throw new ArgumentNullException(nameof(seqList));

            var result = new List<AminoAcidDistributionSpreadsheetRecord>();

            // patterns

            var patternProteinDictionary = new Dictionary<string, AminoAcidChainComposition>();
            var patternProteinInterfaceDictionary = new Dictionary<string, AminoAcidChainComposition>();
            var patternProteinInterfaceInteractionDictionary = new Dictionary<string, AminoAcidChainComposition>();
            var patternProteinInterfaceNonInteractionDictionary = new Dictionary<string, AminoAcidChainComposition>();

            foreach (var vectorProteinInterfaceWhole in vectorProteinInterfaceWholeList)
            {
                var interactionBools = vectorProteinInterfaceWhole.InteractionBools();

                string pattern;
                if (vectorType >= 0 && vectorType <= 3)
                {
                    pattern = vectorProteinInterfaceWhole.VectorString(vectorType);
                }
                else if (vectorType == 4)
                {
                    pattern = vectorProteinInterfaceWhole.SecondaryStructure;
                }
                else if (vectorType == 5)
                {
                    pattern = "ProteinInterface Length " + vectorProteinInterfaceWhole.ProteinInterfaceLength;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(vectorType));
                }

                if (!patternProteinDictionary.ContainsKey(pattern)) { patternProteinDictionary.Add(pattern, new AminoAcidChainComposition()); }
                if (!patternProteinInterfaceDictionary.ContainsKey(pattern)) { patternProteinInterfaceDictionary.Add(pattern, new AminoAcidChainComposition()); }
                if (!patternProteinInterfaceInteractionDictionary.ContainsKey(pattern)) { patternProteinInterfaceInteractionDictionary.Add(pattern, new AminoAcidChainComposition()); }
                if (!patternProteinInterfaceNonInteractionDictionary.ContainsKey(pattern)) { patternProteinInterfaceNonInteractionDictionary.Add(pattern, new AminoAcidChainComposition()); }

                var aminoAcids1L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids1L();

                for (int index = 0; index < aminoAcids1L.Length; index++)
                {
                    var c = aminoAcids1L[index];

                    patternProteinInterfaceDictionary[pattern].IncrementAminoAcidCount(c);

                    if (interactionBools[index])
                    {
                        patternProteinInterfaceInteractionDictionary[pattern].IncrementAminoAcidCount(c);
                    }
                    else
                    {
                        patternProteinInterfaceNonInteractionDictionary[pattern].IncrementAminoAcidCount(c);
                    }
                }

                patternProteinDictionary[pattern].NumberSamples++;
                patternProteinInterfaceDictionary[pattern].NumberSamples++;
                patternProteinInterfaceInteractionDictionary[pattern].NumberSamples++;
                patternProteinInterfaceNonInteractionDictionary[pattern].NumberSamples++;


                var bsSeqList = seqList.Where(a => SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID).PdbId == vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId && SequenceIdSplit.SequenceIdToPdbIdAndChainId(a.ID).ChainId == SpreadsheetFileHandler.AlphabetLetterRollOver(vectorProteinInterfaceWhole.FullProteinInterfaceId.ChainId)).ToList();
                foreach (var chain in bsSeqList)
                {
                    var seq = chain.ConvertToString();
                    foreach (var c in seq)
                    {
                        patternProteinDictionary[pattern].IncrementAminoAcidCount(c);
                    }
                }
            }

            foreach (var kvp in patternProteinDictionary)
            {
                var recordAllComposition = new AminoAcidDistributionSpreadsheetRecord();
                var recordProteinInterfaceComposition = new AminoAcidDistributionSpreadsheetRecord();
                var recordProteinInterfaceInteractionComposition = new AminoAcidDistributionSpreadsheetRecord();
                var recordProteinInterfaceNonInteractionComposition = new AminoAcidDistributionSpreadsheetRecord();

                result.Add(recordAllComposition);
                result.Add(recordProteinInterfaceComposition);
                result.Add(recordProteinInterfaceInteractionComposition);
                result.Add(recordProteinInterfaceNonInteractionComposition);

                recordAllComposition.Pattern = kvp.Key;
                recordProteinInterfaceComposition.Pattern = kvp.Key;
                recordProteinInterfaceInteractionComposition.Pattern = kvp.Key;
                recordProteinInterfaceNonInteractionComposition.Pattern = kvp.Key;

                var vectorTypeStr = VectorProteinInterfaceWhole.VectorStringDescription(vectorType);

                recordAllComposition.Type = vectorTypeStr;
                recordProteinInterfaceComposition.Type = vectorTypeStr;
                recordProteinInterfaceInteractionComposition.Type = vectorTypeStr;
                recordProteinInterfaceNonInteractionComposition.Type = vectorTypeStr;

                recordAllComposition.Metric = "Protein";
                recordProteinInterfaceComposition.Metric = "ProteinInterface";
                recordProteinInterfaceInteractionComposition.Metric = "ProteinInterface interactions";
                recordProteinInterfaceNonInteractionComposition.Metric = "ProteinInterface non interactions";

                // number of samples
                recordAllComposition.NumberOfSamples = kvp.Value.NumberSamples;
                recordProteinInterfaceComposition.NumberOfSamples = patternProteinInterfaceDictionary[kvp.Key].NumberSamples;
                recordProteinInterfaceInteractionComposition.NumberOfSamples = patternProteinInterfaceInteractionDictionary[kvp.Key].NumberSamples;
                recordProteinInterfaceNonInteractionComposition.NumberOfSamples = patternProteinInterfaceNonInteractionDictionary[kvp.Key].NumberSamples;

                // number of amino acids
                recordAllComposition.TotalAminoAcids = kvp.Value.AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();
                recordProteinInterfaceComposition.TotalAminoAcids = patternProteinInterfaceDictionary[kvp.Key].AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();
                recordProteinInterfaceInteractionComposition.TotalAminoAcids = patternProteinInterfaceInteractionDictionary[kvp.Key].AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();
                recordProteinInterfaceNonInteractionComposition.TotalAminoAcids = patternProteinInterfaceNonInteractionDictionary[kvp.Key].AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();

                var allPercentage = AminoAcidChainComposition.ConvertToPercentage(kvp.Value);
                var proteinInterfacePercentage = AminoAcidChainComposition.ConvertToPercentage(patternProteinInterfaceDictionary[kvp.Key]);
                var proteinInterfaceInteractionPercentage = AminoAcidChainComposition.ConvertToPercentage(patternProteinInterfaceInteractionDictionary[kvp.Key]);
                var proteinInterfaceNonInteractionPercentage = AminoAcidChainComposition.ConvertToPercentage(patternProteinInterfaceNonInteractionDictionary[kvp.Key]);

                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    var groupItemsTotal = AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups);

                    for (var groupItemIndex = 0; groupItemIndex < groupItemsTotal; groupItemIndex++)
                    {
                        recordAllComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = allPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                        recordProteinInterfaceComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = proteinInterfacePercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                        recordProteinInterfaceInteractionComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = proteinInterfaceInteractionPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                        recordProteinInterfaceNonInteractionComposition.Distribution[(int)enumAminoAcidGroups][groupItemIndex] = proteinInterfaceNonInteractionPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupItemIndex];
                    }
                }
            }

            return result;
        }

        public static string[,] Spreadsheet(List<AminoAcidDistributionSpreadsheetRecord> aminoAcidDistributionSpreadsheetRecordList)
        {
            if (aminoAcidDistributionSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(aminoAcidDistributionSpreadsheetRecordList));
            // for each short/long vector pattern, show the sd aa for all proteinInterfaces in the pattern, all proteins in the pattern
            // also show for entire set
            // 
            //                                      Num. samples  A B C D E F ... Total AAs
            // Uniprot - whole database
            // All - whole protein
            // All - whole proteinInterface
            // All - proteinInterface interactions
            // pattern 1010101 - whole protein
            // pattern 1010101 - whole proteinInterfaces
            // pattern 1010101 - proteinInterface interactions
            // 
            // 
            // % property for each protein, chain, proteinInterface
            // 
            // 
            // 

            var header = Header();
            //var totalRows = aminoAcidDistributionSpreadsheetRecordList.Count + 1;
            //var totalColumns = header.Length;

            var result = new List<string[]>();

            result.Add(header);

            result.Add(UniProtKb().ToStrings());

            //var overallDistribution = OverallDistributionRecords();
            //var patternDistribution = PatternDistributionSpreadsheetRecords();
            // distribution formatting String.Format("{0:0.00}", dist)

            result.AddRange(aminoAcidDistributionSpreadsheetRecordList.Select(record => record.ToStrings()));

            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }
    }

}
