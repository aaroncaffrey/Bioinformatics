using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.TypeConversions;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class PatternStatisticsSpreadsheetRecord
    {
        public string Pattern;
        public int PatternLength;
        public int TotalFound;

        //public int TotalFoundInHomodimers;
        //public int TotalFoundInHeterodimers;

        public double TotalFoundPercentage;

        public int TotalChainA;
        public double TotalChainAPercentage;

        public int TotalChainB;
        public double TotalChainBPercentage;

        public int TotalForward;
        public double TotalForwardPercentage;

        public int TotalReverse;
        public double TotalReversePercentage;

        private double CalculatePercentage(int value, int total)
        {
            return (total > 0 ? ((double)value / (double)total) * 100 : 0);
        }

        public static string[,] CountPatternsSpreadsheet(string patternName, string[,] treeDataSheet, int columnIndex)
        {
            if (patternName == null) throw new ArgumentNullException(nameof(patternName));
            if (treeDataSheet == null) throw new ArgumentNullException(nameof(treeDataSheet));

            var countedPatterns = CountPatterns(treeDataSheet, columnIndex);

            var spreadsheet = SpreadsheetStrings(countedPatterns, patternName);

            return ConvertTypes.StringJagged2DArrayTo2DArray(spreadsheet);
        }

        public string[] ToStrings(PatternStatisticsSpreadsheetRecord totals)
        {
            if (totals == null) throw new ArgumentNullException(nameof(totals));

            TotalFoundPercentage = CalculatePercentage(TotalFound, totals.TotalFound);
            TotalChainAPercentage = CalculatePercentage(TotalChainA, totals.TotalChainA);
            TotalChainBPercentage = CalculatePercentage(TotalChainB, totals.TotalChainB);
            TotalForwardPercentage = CalculatePercentage(TotalForward, totals.TotalForward);
            TotalReversePercentage = CalculatePercentage(TotalReverse, totals.TotalReverse);


            var result = new List<string>();

            result.Add(Pattern);
            result.Add("" + TotalFound);
            result.Add("" + $"{TotalFoundPercentage:00.00}");

            //result.Add("" + TotalFound);
            //result.Add("" + $"{TotalFoundPercentage:00.00}");

            //result.Add("" + TotalFound);
            //result.Add("" + $"{TotalFoundPercentage:00.00}");


            result.Add("" + TotalChainA);
            result.Add("" + $"{TotalChainAPercentage:00.00}");
            result.Add("" + TotalChainB);
            result.Add("" + $"{TotalChainBPercentage:00.00}");
            result.Add("" + TotalForward);
            result.Add("" + $"{TotalForwardPercentage:00.00}");
            result.Add("" + TotalReverse);
            result.Add("" + $"{TotalReversePercentage:00.00}");
            result.Add("" + PatternLength);

            return result.ToArray();
        }

        public static string[] HeaderStrings(string patternName)
        {
            if (String.IsNullOrWhiteSpace(patternName)) throw new ArgumentNullException(nameof(patternName));

            string headerPattern = "Pattern: " + (patternName ?? "");
            const string headerPatternLength = "Length";

            const string headerTotalFound = "Total Found";
            const string headerTotalFoundPercentage = "% Total Found";

            //const string headerTotalFoundInHeterodimers = "Total Found In Heterodiemrs";
            //const string headerTotalFoundInHeterodimersPercentage = "% Total Found In Heterodimers";

            //const string headerTotalFoundInHomodimers = "Total Found In Homodimers";
            //const string headerTotalFoundInHomodimersPercentage = "% Total Found In Homodimers";

            const string headerTotalChainA = "Total Chain A";
            const string headerTotalChainAPercentage = "% Total Chain A";
            const string headerTotalChainB = "Total Chain B";
            const string headerTotalChainBPercentage = "% Total Chain B";
            const string headerTotalForward = "Total Forward";
            const string headerTotalForwardPercentage = "% Total Forward";
            const string headerTotalReverse = "Total Reverse";
            const string headerTotalReversePercentage = "% Total Reverse";

            var result = new List<string>();

            result.Add("" + headerPattern);

            result.Add("" + headerTotalFound);
            result.Add("" + headerTotalFoundPercentage);

            //result.Add("" + headerTotalFoundInHeterodimers);
            //result.Add("" + headerTotalFoundInHeterodimersPercentage);

            //result.Add("" + headerTotalFoundInHomodimers);
            //result.Add("" + headerTotalFoundInHomodimersPercentage);

            result.Add("" + headerTotalChainA);
            result.Add("" + headerTotalChainAPercentage);
            result.Add("" + headerTotalChainB);
            result.Add("" + headerTotalChainBPercentage);
            result.Add("" + headerTotalForward);
            result.Add("" + headerTotalForwardPercentage);
            result.Add("" + headerTotalReverse);
            result.Add("" + headerTotalReversePercentage);
            result.Add("" + headerPatternLength);

            return result.ToArray();
        }

        public static string[][] SpreadsheetStrings(List<PatternStatisticsSpreadsheetRecord> patternStatisticsSpreadsheetRecordList, string patternName)
        {
            //if (patternStatisticsSpreadsheetRecordList == null || patternStatisticsSpreadsheetRecordList.Count == 0) throw new ArgumentNullException(nameof(patternStatisticsSpreadsheetRecordList));
            //if (String.IsNullOrWhiteSpace(patternName)) throw new ArgumentNullException(nameof(patternName));

            var result = new List<string[]>();
            result.Add(HeaderStrings(patternName));

            if (patternStatisticsSpreadsheetRecordList == null || patternStatisticsSpreadsheetRecordList.Count == 0) return result.ToArray();
            if (String.IsNullOrWhiteSpace(patternName)) return result.ToArray();

            

            var totalSums = new PatternStatisticsSpreadsheetRecord
            {
                TotalFound = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalFound).Sum(),
                //TotalFoundInHeterodimers = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalFoundInHeterodimers).Sum(),
                //TotalFoundInHomodimers = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalFoundInHomodimers).Sum(),
                TotalChainA = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalChainA).Sum(),
                TotalChainB = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalChainB).Sum(),
                TotalForward = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalForward).Sum(),
                TotalReverse = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalReverse).Sum(),
                PatternLength = patternStatisticsSpreadsheetRecordList.Select(a => a.PatternLength).Sum(),
            };

            result.AddRange(patternStatisticsSpreadsheetRecordList.OrderByDescending(a => a.TotalFound).Select(data => data.ToStrings(totalSums)));

            totalSums.TotalFoundPercentage = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalFoundPercentage).Sum();
            //totalSums.TotalFoundInHeterodimers = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalFoundInHeterodimers).Sum();
            //totalSums.TotalFoundInHomodimers = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalFoundInHomodimers).Sum();
            totalSums.TotalChainAPercentage = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalChainAPercentage).Sum();
            totalSums.TotalChainBPercentage = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalChainBPercentage).Sum();
            totalSums.TotalForwardPercentage = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalForwardPercentage).Sum();
            totalSums.TotalReversePercentage = patternStatisticsSpreadsheetRecordList.Select(a => a.TotalReversePercentage).Sum();

            var totalsStrings = new string[]
                {
                    "Total",
                    "" + totalSums.TotalFound, $"{totalSums.TotalFoundPercentage:00.00}",
                    //"" + totalSums.TotalFoundInHeterodimers, $"{totalSums.TotalFoundInHeterodimers:00.00}",
                    //"" + totalSums.TotalFoundInHomodimers, $"{totalSums.TotalFoundInHomodimers:00.00}",
                    "" + totalSums.TotalChainA, $"{totalSums.TotalChainAPercentage:00.00}",
                    "" + totalSums.TotalChainB, $"{totalSums.TotalChainBPercentage:00.00}",
                    "" + totalSums.TotalForward, $"{totalSums.TotalForwardPercentage:00.00}",
                    "" + totalSums.TotalReverse, $"{totalSums.TotalReversePercentage:00.00}",
                    "" + totalSums.PatternLength,
                };

            result.Add(totalsStrings);

            return result.ToArray();
        }

        public static List<PatternStatisticsSpreadsheetRecord> CountPatterns(string[,] spreadsheet, int columnIndex, bool skipHeaderRow = true)
        {
            if (spreadsheet == null || spreadsheet.Length == 0) throw new ArgumentNullException(nameof(spreadsheet));

            
            var result = new List<PatternStatisticsSpreadsheetRecord>();

            for (var rowIndex = skipHeaderRow ? 1 : 0; rowIndex < spreadsheet.GetLength(0); rowIndex++)
            {
                var pattern = spreadsheet[rowIndex, columnIndex];
                if (string.IsNullOrWhiteSpace(pattern)) continue;

                PatternStatisticsSpreadsheetRecord record = result.FirstOrDefault(a => a.Pattern == pattern);

                if (record == null)
                {
                    record = new PatternStatisticsSpreadsheetRecord
                    {
                        Pattern = pattern,
                        PatternLength = pattern.Length
                    };

                    result.Add(record);
                }

                record.TotalFound++;

                switch (spreadsheet[rowIndex, 02])
                {
                    case "A":
                        record.TotalChainA++;
                        break;
                    case "B":
                        record.TotalChainB++;
                        break;
                }

                switch (spreadsheet[rowIndex, 04])
                {
                    case "Fwd":
                        record.TotalForward++;
                        break;
                    case "Rev":
                        record.TotalReverse++;
                        break;
                }
            }

            return result;
        }
    }

}
