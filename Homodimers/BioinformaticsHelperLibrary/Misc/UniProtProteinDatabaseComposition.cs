//-----------------------------------------------------------------------
// <copyright file="UniProtProteinDatabaseComposition.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.AminoAcids.Ambiguous;
using BioinformaticsHelperLibrary.AminoAcids.Standard;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.UserProteinInterface;
using System;

namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    ///     This class stores the percentage values of Amino Acids in the entire UniProt database.  It has methods to represent
    ///     this data in different formats.
    /// </summary>
    public static class UniProtProteinDatabaseComposition
    {                                                
        public static long TotalSamplesInDatabase = 48744721;
        public static long TotalAminoAcidsInDatabase = 16239187733;

        public static void SaveUniProtSpreadsheet(string saveFilename, ProgressActionSet progressActionSet)
        {
            if (saveFilename == null) throw new ArgumentNullException(nameof(saveFilename));
            List<SpreadsheetCell[]> spreadsheet = UniProtHeatMapSpreadsheet();

            string[] savedFiles = SpreadsheetFileHandler.SaveSpreadsheet(saveFilename, null, spreadsheet);

            foreach (string savedFile in savedFiles)
            {
                ProgressActionSet.Report("Saved: " + savedFile, progressActionSet);
            }
        }

        /// <summary>
        ///     This method returns a spreadsheet heat map of the entire UniProt sequence database.
        /// </summary>
        /// <returns></returns>
        public static List<SpreadsheetCell[]> UniProtHeatMapSpreadsheet()
        {
            var spreadsheet = new List<SpreadsheetCell[]>();
            //spreadsheet.Add("HM Entire UniProt Database"); // filename / worksheet name

            spreadsheet.Add(new[] {new SpreadsheetCell("Amino Acid Heat Map - Entire UniProt Database Composition (Converted To Matrix)") }); // spreadsheet title

            //spreadsheet.Add(new[] {new SpreadsheetCell(string.Empty), });
            //spreadsheet.Add(new[] {new SpreadsheetCell("Amino Acid Heat Map - Entire UniProt Database - A to Z") }); // section title
            //spreadsheet.AddRange(AminoAcidCompositionAsMatrix().SpreadsheetAminoAcidColorGroupsHeatMap());

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                

                spreadsheet.Add(new[] {new SpreadsheetCell(string.Empty),});
                spreadsheet.Add(new[] {new SpreadsheetCell("Amino Acid Heat Map - Entire UniProt Database - Acid Groups " + enumAminoAcidGroups)}); // section title
                spreadsheet.AddRange(AminoAcidCompositionAsMatrix().SpreadsheetAminoAcidColorGroupsHeatMap(enumAminoAcidGroups));
            }

            spreadsheet.Add(new[] {new SpreadsheetCell(string.Empty), });
            spreadsheet.Add(new[] {new SpreadsheetCell("Amino Acid Heat Map - Entire UniProt Database - Original Data - A to Z") }); // section title

            AminoAcidChainComposition c = AminoAcidCompositionAsChain();

            var chain = new List<SpreadsheetCell>();
            chain.Add(new SpreadsheetCell(string.Empty));
            for (int i = 0; i < AminoAcidTotals.TotalAminoAcids(); i++)
            {
                chain.Add(new SpreadsheetCell(AminoAcidConversions.AminoAcidNumberToCode1L(i + 1)));
            }
            spreadsheet.Add(chain.ToArray());

            chain = new List<SpreadsheetCell>();
            chain.Add(new SpreadsheetCell(string.Empty));
            for (int i = 0; i < AminoAcidTotals.TotalAminoAcids(); i++)
            {
                chain.Add(new SpreadsheetCell(c.AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids][i]));
            }
            spreadsheet.Add(chain.ToArray());

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                

                spreadsheet.Add(new[] {new SpreadsheetCell(string.Empty),});
                spreadsheet.Add(new[] {new SpreadsheetCell("Amino Acid Heat Map - Entire UniProt Database - Original Data - Acid Groups " + enumAminoAcidGroups)}); // section title

                var chainGroups = new List<SpreadsheetCell>();
                chainGroups.Add(new SpreadsheetCell(string.Empty));
                for (int i = 0; i < AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups); i++)
                {
                    chainGroups.Add(new SpreadsheetCell((i + 1)));
                }
                spreadsheet.Add(chainGroups.ToArray());

                chainGroups = new List<SpreadsheetCell>();
                chainGroups.Add(new SpreadsheetCell(string.Empty));
                for (int i = 0; i < AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups); i++)
                {
                    chainGroups.Add(new SpreadsheetCell(c.AminoAcidGroupsCount[(int)enumAminoAcidGroups][i]));
                }
                spreadsheet.Add(chainGroups.ToArray());
            }

            return spreadsheet;
        }

        /// <summary>
        ///     This method returns the percentage of the given Amino Acid in the entire UniProt database.
        /// </summary>
        /// <param name="aminoAcidCode"></param>
        /// <returns></returns>
        public static decimal AminoAcidCompositionInPercent(string aminoAcidCode)
        {
            if (aminoAcidCode == null) throw new ArgumentNullException(nameof(aminoAcidCode));
            return AminoAcidCompositionInPercent(AminoAcidConversions.AminoAcidNameToNumber(aminoAcidCode));
        }

        /// <summary>
        ///     This method returns the percentage of the given Amino Acid in the entire UniProt database.
        /// </summary>
        /// <param name="aminoAcidNumber"></param>
        /// <returns></returns>
        public static decimal AminoAcidCompositionInPercent(int aminoAcidNumber)
        {
            switch (aminoAcidNumber)
            {
                case (int) StandardAminoAcids3L.ALA:
                    return 8.59m;
                case (int) StandardAminoAcids3L.ARG:
                    return 5.52m;
                case (int) StandardAminoAcids3L.ASN:
                    return 4.09m;
                case (int) StandardAminoAcids3L.ASP:
                    return 5.38m;
                case (int) StandardAminoAcids3L.CYS:
                    return 1.29m;
                case (int) StandardAminoAcids3L.GLN:
                    return 3.92m;
                case (int) StandardAminoAcids3L.GLU:
                    return 6.23m;
                case (int) StandardAminoAcids3L.GLY:
                    return 7.02m;
                case (int) StandardAminoAcids3L.HIS:
                    return 2.22m;
                case (int) StandardAminoAcids3L.ILE:
                    return 5.83m;
                case (int) StandardAminoAcids3L.LEU:
                    return 9.82m;
                case (int) StandardAminoAcids3L.LYS:
                    return 5.29m;
                case (int) StandardAminoAcids3L.MET:
                    return 2.42m;
                case (int) StandardAminoAcids3L.PHE:
                    return 3.99m;
                case (int) StandardAminoAcids3L.PRO:
                    return 4.79m;
                case (int) StandardAminoAcids3L.SER:
                    return 6.89m;
                case (int) StandardAminoAcids3L.THR:
                    return 5.57m;
                case (int) StandardAminoAcids3L.TRP:
                    return 1.29m;
                case (int) StandardAminoAcids3L.TYR:
                    return 3.01m;
                case (int) StandardAminoAcids3L.VAL:
                    return 6.71m;
                case (int) AmbiguousAminoAcids3L.ASX:
                    return 0.00m;
                case (int) AmbiguousAminoAcids3L.GLX:
                    return 0.00m;
                case (int) AmbiguousAminoAcids3L.XAA:
                    return 0.03m;
                default:
                    return 0.00m;
            }
        }

        /// <summary>
        ///     This method returns the entire UniProt database composition as a chain.
        /// </summary>
        /// <returns></returns>
        public static AminoAcidChainComposition AminoAcidCompositionAsChain()
        {
            var result = new AminoAcidChainComposition();

            int totalAminoAcids = AminoAcidTotals.TotalAminoAcids();

            for (int x = 0; x < totalAminoAcids; x++)
            {
                var aaPercentage = AminoAcidCompositionInPercent(x + 1);
                var aaOriginal = (aaPercentage/100) * TotalAminoAcidsInDatabase;

                result.IncrementAminoAcidCount(x + 1, aaOriginal);
            }
            
            result.NumberSamples = TotalSamplesInDatabase;
            
            result = AminoAcidChainComposition.ConvertToPercentage(result);
            
            return result;
        }

        /// <summary>
        ///     This method returns the entire UniProt database composition as a 2d matrix.
        /// </summary>
        /// <returns></returns>
        public static AminoAcidPairCompositionMatrix AminoAcidCompositionAsMatrix()
        {
            return AminoAcidChainComposition.ConvertToMatrix(AminoAcidCompositionAsChain());
        }
    }
}