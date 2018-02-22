//-----------------------------------------------------------------------
// <copyright file="AminoAcidChainComposition.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.Spreadsheets;

namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    ///     AminoAcidChainComposition class.  This class is used to count how many of each amino acid are found in a sequence
    ///     chain.
    /// </summary>
    public class AminoAcidChainComposition
    {
        private readonly object _lockAminoAcidCount = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="AminoAcidChainComposition" /> class.
        /// </summary>
        /// <param name="proteinId"></param>
        /// <param name="chainId"></param>
        public AminoAcidChainComposition(string proteinId, string chainId)
            : this()
        {
            ProteinId = proteinId;
            ChainId = chainId;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AminoAcidChainComposition" /> class.
        /// </summary>
        public AminoAcidChainComposition()
        {
            AminoAcidGroupsCount = new decimal[AminoAcidGroups.AminoAcidGroups.GetTotalGroups()][];

            for (var ruleIndex = 0; ruleIndex < AminoAcidGroupsCount.Length; ruleIndex++)
            {
                AminoAcidGroupsCount[ruleIndex] = new decimal[AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(ruleIndex)];
            }
        }

        public string ProteinId;

        public string ChainId;

        //public decimal[] AminoAcidCount;

        public decimal[][] AminoAcidGroupsCount;

        public long NumberSamples;

        /// <summary>
        ///     TotalFromAminoAcidChainCompositionList method.
        /// </summary>
        /// <param name="aminoAcidChainCompositionList"></param>
        /// <returns></returns>
        public static AminoAcidChainComposition TotalFromAminoAcidChainCompositionList(List<AminoAcidChainComposition> aminoAcidChainCompositionList)
        {
            var aminoAcidChainCompositionTotal = new AminoAcidChainComposition("Total", "-");

            lock (aminoAcidChainCompositionList)
            {
                foreach (AminoAcidChainComposition aminoAcidChainComposition in aminoAcidChainCompositionList)
                {
                    for (int x = 0; x < AminoAcidTotals.TotalAminoAcids(); x++)
                    {
                        // note 1: array is zero based, amino acid numbers are one based
                        // note 2: IncrementAminoAcidCount method also increments all groups so only one call is required
                        aminoAcidChainCompositionTotal.IncrementAminoAcidCount(x + 1, aminoAcidChainComposition.AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids][x]); 
                    }
                }
            }
            return aminoAcidChainCompositionTotal;
        }

        /// <summary>
        ///     ConvertToPercentage method.
        /// </summary>
        /// <param name="aminoAcidChainComposition"></param>
        /// <param name="factorisation"></param>
        /// <returns></returns>
        public static AminoAcidChainComposition ConvertToPercentage(AminoAcidChainComposition aminoAcidChainComposition)
        {
            var aminoAcidChainCompositionAsPercentage = new AminoAcidChainComposition(aminoAcidChainComposition.ProteinId, aminoAcidChainComposition.ChainId);

            
            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                var sumTotal = aminoAcidChainComposition.AminoAcidGroupsCount[(int) enumAminoAcidGroups].Sum();

                if (sumTotal == 0) continue;
                
                for (int groupIndex = 0; groupIndex < AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups); groupIndex++)
                {
                    aminoAcidChainCompositionAsPercentage.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupIndex] = (aminoAcidChainComposition.AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupIndex] / sumTotal) * 100.0m;
                }
            }
            
            return aminoAcidChainCompositionAsPercentage;
        }

        public static AminoAcidPairCompositionMatrix ConvertToMatrix(AminoAcidChainComposition chain)
        {
            if (chain == null)
            {
                return null;
            }

            var result = new AminoAcidPairCompositionMatrix();

            int totalAminoAcids = AminoAcidTotals.TotalAminoAcids();



            for (int x = 0; x < totalAminoAcids; x++)
            {
                for (int y = 0; y < totalAminoAcids; y++)
                {
                    result.IncrementAminoAcidCount(x + 1, y + 1, (chain.AminoAcidGroupsCount[(int) AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids][x]*chain.AminoAcidGroupsCount[(int) AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids][y])/100);
                }
            }


            return result;
        }

        public static SpreadsheetCell[] SpreadsheetTitleRow()
        {
            var titleRow = new List<SpreadsheetCell>();

            titleRow.Add(new SpreadsheetCell("Protein ID"));
            titleRow.Add(new SpreadsheetCell("Chain ID"));

            int totalAminoAcids = AminoAcidTotals.TotalAminoAcids();

            for (int index = 0; index < totalAminoAcids; index++)
            {
                titleRow.Add(new SpreadsheetCell(AminoAcidConversions.AminoAcidNumberToCode1L(index + 1)));
            }

            titleRow.Add(new SpreadsheetCell("Total"));

            return titleRow.ToArray();
        }

        public static SpreadsheetCell[] SpreadsheetGroupsTitleRow(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups)
        {
            var titleRow = new List<SpreadsheetCell>();

            titleRow.Add(new SpreadsheetCell("Protein ID"));
            titleRow.Add(new SpreadsheetCell("Chain ID"));

            for (int index = 0; index < AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups); index++)
            {
                titleRow.Add(new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupDescriptions(enumAminoAcidGroups)[index]));
            }

            titleRow.Add(new SpreadsheetCell("Total"));

            return titleRow.ToArray();
        }

        public SpreadsheetCell[] SpreadsheetDataRow()
        {
            var result = new List<SpreadsheetCell>();

            result.Add(new SpreadsheetCell(ProteinId));
            result.Add(new SpreadsheetCell(ChainId));

            for (int index = 0; index < AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Length; index++)
            {
                result.Add(new SpreadsheetCell(AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids][index]));
            }

            var allAminoAcidsTotal = AminoAcidGroupsCount[(int)AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids].Sum();

            result.Add(new SpreadsheetCell(allAminoAcidsTotal));

            return result.ToArray();
        }

        public SpreadsheetCell[] SpreadsheetGroupsDataRow(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups)
        {
            var dataRow = new List<SpreadsheetCell>();

            dataRow.Add(new SpreadsheetCell(ProteinId));
            dataRow.Add(new SpreadsheetCell(ChainId));
            var allGroupsTotal = AminoAcidGroupsCount[(int) enumAminoAcidGroups].Sum();

            for (int index = 0; index < AminoAcidGroupsCount[(int) enumAminoAcidGroups].Length; index++)
            {
                dataRow.Add(new SpreadsheetCell(AminoAcidGroupsCount[(int) enumAminoAcidGroups][index]));
            }

            dataRow.Add(new SpreadsheetCell(allGroupsTotal));

            return dataRow.ToArray();
        }

        public void IncrementAminoAcidCount(char aminoAcidCode, decimal incrementValue = 1)
        {
            if (!ParameterValidation.IsAminoAcidCodeValid(aminoAcidCode))
            {
                throw new ArgumentOutOfRangeException(nameof(aminoAcidCode));
            }

            IncrementAminoAcidCount(AminoAcidConversions.AminoAcidNameToNumber(aminoAcidCode), incrementValue);
        }

        public void IncrementAminoAcidCount(string aminoAcidCode, decimal incrementValue = 1)
        {
            if (string.IsNullOrEmpty(aminoAcidCode))
            {
                throw new ArgumentOutOfRangeException(nameof(aminoAcidCode));
            }

            IncrementAminoAcidCount(AminoAcidConversions.AminoAcidNameToNumber(aminoAcidCode), incrementValue);
        }

        public void IncrementAminoAcidCount(int aminoAcidNumber, decimal incrementValue = 1)
        {
            lock (_lockAminoAcidCount)
            {
                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    var groupIndexes = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNumberToSubgroupNumbers(enumAminoAcidGroups, aminoAcidNumber);

                    for (int groupIndex = 0; groupIndex < groupIndexes.Length; groupIndex++)
                    {
                        AminoAcidGroupsCount[(int)enumAminoAcidGroups][groupIndexes[groupIndex]] += incrementValue;
                    }
                }
            }
        }
    }
}