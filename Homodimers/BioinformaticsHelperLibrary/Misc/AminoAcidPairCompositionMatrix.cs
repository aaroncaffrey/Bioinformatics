//-----------------------------------------------------------------------
// <copyright file="AminoAcidPairCompositionMatrix.cs" company="Aaron Caffrey">
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
    ///     AminoAcidPairCompositionMatrix
    /// </summary>
    public class AminoAcidPairCompositionMatrix
    {
        private readonly object _lockAminoAcidToAminoAcid = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="AminoAcidPairCompositionMatrix" /> class.
        /// </summary>
        public AminoAcidPairCompositionMatrix()
        {
            AminoAcidToAminoAcid = new decimal[AminoAcidGroups.AminoAcidGroups.GetTotalGroups()][,];

            for (var ruleIndex = 0; ruleIndex < AminoAcidToAminoAcid.Length; ruleIndex++)
            {
                AminoAcidToAminoAcid[ruleIndex] = new decimal[AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(ruleIndex), AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(ruleIndex)];
            }

            
        }

        /// <summary>
        ///     Gets AminoAcidToAminoAcid property.  The AminoAcidToAminoAcid property represents the total number of times two
        ///     amino acids are grouped together, for example, in an interaction.
        /// </summary>
        public decimal[][,] AminoAcidToAminoAcid;


        /// <summary>
        ///     CalculatePercentageMatrix method. Converts the values to their mean average percentage values.
        /// </summary>
        /// <param name="dataMatrix"></param>
        /// <param name="factorisation"></param>
        /// <returns></returns>
        public static AminoAcidPairCompositionMatrix CalculatePercentageMatrix(AminoAcidPairCompositionMatrix dataMatrix)
        {
            if (dataMatrix == null)
            {
                return null;
            }

            var result = new AminoAcidPairCompositionMatrix();

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                

                int totalGroups = AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups);

                var countMatrix = dataMatrix.AminoAcidToAminoAcid[(int) enumAminoAcidGroups];
                var resultMatrix = result.AminoAcidToAminoAcid[(int)enumAminoAcidGroups];

                decimal sumTotal = countMatrix.Cast<decimal>().Sum();


                if (sumTotal > 0.0m)
                {
                    for (int x = 0; x < totalGroups; x++)
                    {
                        for (int y = 0; y < totalGroups; y++)
                        {
                            resultMatrix[x, y] = (countMatrix[x, y] / (sumTotal / 2)) * 100.0m;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     NormalizeWithCompositionMatrix method.
        /// </summary>
        /// <param name="dataMatrix"></param>
        /// <param name="normalisationMatrix"></param>
        /// <returns></returns>
        public static AminoAcidPairCompositionMatrix NormalizeWithCompositionMatrix(AminoAcidPairCompositionMatrix dataMatrix, AminoAcidPairCompositionMatrix normalisationMatrix)
        {
            if (dataMatrix == null)
            {
                throw new ArgumentNullException(nameof(dataMatrix));
            }

            if (normalisationMatrix == null)
            {
                throw new ArgumentNullException(nameof(normalisationMatrix));
            }

            var result = new AminoAcidPairCompositionMatrix();

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                int totalGroups = AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups);

                for (int groupIndexX = 0; groupIndexX < totalGroups; groupIndexX++)
                {
                    for (int groupIndexY = 0; groupIndexY < totalGroups; groupIndexY++)
                    {
                        result.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][groupIndexX, groupIndexY] = dataMatrix.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][groupIndexX, groupIndexY] + (dataMatrix.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][groupIndexX, groupIndexY] - normalisationMatrix.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][groupIndexX, groupIndexY]);
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///     DifferenceWithCompositionMatrix
        /// </summary>
        /// <param name="dataMatrix"></param>
        /// <param name="normalisationMatrix"></param>
        /// <returns></returns>
        public static AminoAcidPairCompositionMatrix DifferenceWithCompositionMatrix(AminoAcidPairCompositionMatrix dataMatrix, AminoAcidPairCompositionMatrix normalisationMatrix)
        {
            if (dataMatrix == null)
            {
                throw new ArgumentNullException(nameof(dataMatrix));
            }

            if (normalisationMatrix == null)
            {
                throw new ArgumentNullException(nameof(normalisationMatrix));
            }

            var result = new AminoAcidPairCompositionMatrix();

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {              
                int totalGroups = AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups);

                for (int x = 0; x < totalGroups; x++)
                {
                    for (int y = 0; y < totalGroups; y++)
                    {
                        result.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][x, y] = dataMatrix.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][x, y] - normalisationMatrix.AminoAcidToAminoAcid[(int)enumAminoAcidGroups][x, y];
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     AminoAcidColorGroupsHeatMapTsv
        /// </summary>
        /// <returns></returns>
        public List<SpreadsheetCell[]> SpreadsheetAminoAcidColorGroupsHeatMap(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups)
        {
            lock (_lockAminoAcidToAminoAcid)
            {
                var matrix = AminoAcidToAminoAcid[(int) enumAminoAcidGroups];

                var rows = new List<List<SpreadsheetCell>>();

                var headerRowStrings = new List<SpreadsheetCell> {new SpreadsheetCell()};


                for (var columnIndex = 0; columnIndex < matrix.GetLength(1); columnIndex++)
                {
                    if (enumAminoAcidGroups == AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids)
                    {
                        headerRowStrings.Add(new SpreadsheetCell(AminoAcidConversions.AminoAcidNumberToCode1L(columnIndex + 1)));
                    }
                    else
                    {
                        headerRowStrings.Add(new SpreadsheetCell(columnIndex + 1));    
                    }
                }

                rows.Add(headerRowStrings);

                for (var rowIndex = 0; rowIndex < matrix.GetLength(0); rowIndex++)
                {
                    var row = new List<SpreadsheetCell>();

                    if (enumAminoAcidGroups == AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids)
                    {
                        row.Add(new SpreadsheetCell(AminoAcidConversions.AminoAcidNumberToCode1L(rowIndex + 1)));
                    }
                    else
                    {
                        row.Add(new SpreadsheetCell(rowIndex + 1));
                    }


                    for (var columnIndex = 0; columnIndex < matrix.GetLength(1); columnIndex++)
                    {
                        row.Add(new SpreadsheetCell(matrix[rowIndex, columnIndex]));
                    }

                    rows.Add(row);
                }

                return rows.Select(a => a.ToArray()).ToList();
            }
        }

        /// <summary>
        ///     IncrementAminoAcidCount
        /// </summary>
        /// <param name="aminoAcidCodeA"></param>
        /// <param name="aminoAcidCodeB"></param>
        /// <param name="incrementValue"></param>
        public void IncrementAminoAcidCount(string aminoAcidCodeA, string aminoAcidCodeB, decimal incrementValue = 1.0m)
        {
            IncrementAminoAcidCount(AminoAcidConversions.AminoAcidNameToNumber(aminoAcidCodeA), AminoAcidConversions.AminoAcidNameToNumber(aminoAcidCodeB), incrementValue);
        }

        /// <summary>
        ///     IncrementAminoAcidCount
        /// </summary>
        /// <param name="aminoAcidNumberA"></param>
        /// <param name="aminoAcidNumberB"></param>
        /// <param name="incrementValue"></param>
        public void IncrementAminoAcidCount(int aminoAcidNumberA, int aminoAcidNumberB, decimal incrementValue = 1.0m)
        {
            lock (_lockAminoAcidToAminoAcid)
            {
                foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
                {
                    var aminoAcidGroupsA = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNumberToSubgroupNumbers(enumAminoAcidGroups, aminoAcidNumberA);
                    var aminoAcidGroupsB = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNumberToSubgroupNumbers(enumAminoAcidGroups, aminoAcidNumberB);

                    for (int a = 0; a < aminoAcidGroupsA.Length; a++)
                    {
                        for (int b = 0; b < aminoAcidGroupsB.Length; b++)
                        {
                            AminoAcidToAminoAcid[(int) enumAminoAcidGroups][aminoAcidGroupsA[a], aminoAcidGroupsB[b]] += incrementValue;
                            AminoAcidToAminoAcid[(int) enumAminoAcidGroups][aminoAcidGroupsB[b], aminoAcidGroupsA[a]] += incrementValue;
                        }
                    }
                }
            }
        }
    }
}