//-----------------------------------------------------------------------
// <copyright file="InteractionMatchPercentage.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using BioinformaticsHelperLibrary.Spreadsheets;

namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    ///     InteractionMatchPercentage class.
    /// </summary>
    public class InteractionMatchPercentage
    {
        public CalculatePercentageResult Result;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InteractionMatchPercentage" /> class.
        /// </summary>
        /// <param name="proteinId"></param>
        public InteractionMatchPercentage(string proteinId)
        {
            ProteinId = proteinId;

            TotalInteractions = 0;

            ChainInteractionResidueIndexList = new List<int>[2];

            for (int index = 0; index < ChainInteractionResidueIndexList.Length; index++)
            {
                ChainInteractionResidueIndexList[index] = new List<int>();
            }
        }

        /// <summary>
        ///     Gets ProteinId property.
        /// </summary>
        public string ProteinId { get; }

        /// <summary>
        ///     Gets ChainInteractionResidueIndexList property.
        /// </summary>
        public List<int>[] ChainInteractionResidueIndexList { get; }

        /// <summary>
        ///     Gets CalculatedInteractionMatchPercentage property.
        /// </summary>
        /// <summary>
        ///     Gets TotalInteractions property.
        /// </summary>
        public int TotalInteractions { get; private set; }

        /// <summary>
        ///     SpreadsheetColumnHeadersRow method.
        /// </summary>
        /// <returns></returns>
        public static SpreadsheetCell[] SpreadsheetColumnHeadersRow()
        {
            var row = new[]
            {
                new SpreadsheetCell("Protein Id"),
                new SpreadsheetCell("A: Distinct atoms with interaction"),
                new SpreadsheetCell("B: Distinct atoms with interaction"),
                new SpreadsheetCell("A: Interactions"),
                new SpreadsheetCell("B: Interactions"),
                new SpreadsheetCell("Overall: Distinct Interactions"),
                new SpreadsheetCell("A: % Interactions matching B"),
                new SpreadsheetCell("B: % Interactions matching A"),
                new SpreadsheetCell("Overall: % average")
            };

            return row;
        }

        /// <summary>
        ///     AddResidueSequenceIndex method.
        /// </summary>
        /// <param name="chainId"></param>
        /// <param name="residueSequenceIndex"></param>
        public void AddResidueSequenceIndex(int chainId, string residueSequenceIndex)
        {
            int residueSequenceIndexNumber;
            if (int.TryParse(residueSequenceIndex, out residueSequenceIndexNumber))
            {
                AddResidueSequenceIndex(chainId, residueSequenceIndexNumber);
            }
        }

        /// <summary>
        ///     AddResidueSequenceIndex method.
        /// </summary>
        /// <param name="chainId"></param>
        /// <param name="residueSequenceIndex"></param>
        public void AddResidueSequenceIndex(int chainId, int residueSequenceIndex)
        {
            ChainInteractionResidueIndexList[chainId].Add(residueSequenceIndex);
        }

        /// <summary>
        ///     IncrementTotalInteractions method.
        /// </summary>
        /// <param name="incrementValue"></param>
        public void IncrementTotalInteractions(int incrementValue = 1)
        {
            TotalInteractions += incrementValue;
        }

        /// <summary>
        ///     CalculatePercentage method.
        /// </summary>
        /// <returns></returns>
        public CalculatePercentageResult CalculatePercentage()
        {
            int interactionMatchAB = 0;
            int interactionMatchBA = 0;

            // Which percentage of interactions in A appear in B?
            foreach (int residueSequenceIndex in ChainInteractionResidueIndexList[StaticValues.ChainA])
            {
                if (ChainInteractionResidueIndexList[StaticValues.ChainB].Contains(residueSequenceIndex))
                {
                    interactionMatchAB++;
                }
            }

            // Which percentage of interactions in B appear in A?
            foreach (int residueSequenceIndex in ChainInteractionResidueIndexList[StaticValues.ChainB])
            {
                if (ChainInteractionResidueIndexList[StaticValues.ChainA].Contains(residueSequenceIndex))
                {
                    interactionMatchBA++;
                }
            }

            // Calculate percentage from totals.
            decimal interactionMatchPercentageA = 0.0m;
            decimal interactionMatchPercentageB = 0.0m;
            decimal interactionMatchPercentageAverage = 0.0m;

            interactionMatchPercentageA = ((interactionMatchAB > 0) && (ChainInteractionResidueIndexList[0].Count > 0)) ? (interactionMatchAB/(decimal) ChainInteractionResidueIndexList[0].Count)*100.00m : 0.00m;
            interactionMatchPercentageB = ((interactionMatchBA > 0) && (ChainInteractionResidueIndexList[1].Count > 0)) ? (interactionMatchBA/(decimal) ChainInteractionResidueIndexList[1].Count)*100.00m : 0.00m;
            interactionMatchPercentageAverage = (interactionMatchPercentageA/2.00m) + (interactionMatchPercentageB/2.00m);

            // Round percentage to 2 decimal points.
            interactionMatchPercentageA = Math.Round(interactionMatchPercentageA, 2);
            interactionMatchPercentageB = Math.Round(interactionMatchPercentageB, 2);
            interactionMatchPercentageAverage = Math.Round(interactionMatchPercentageAverage, 2);

            //this.CalculatedInteractionMatchPercentage = new decimal[] { interactionMatchPercentageA, interactionMatchPercentageB, interactionMatchPercentageAverage };
            Result = new CalculatePercentageResult(interactionMatchPercentageA, interactionMatchPercentageB, interactionMatchPercentageAverage);

            //return this.CalculatedInteractionMatchPercentage;
            return Result;
        }

        /// <summary>
        ///     SpreadsheetDataRow method.
        /// </summary>
        /// <returns></returns>
        public SpreadsheetCell[] SpreadsheetDataRow()
        {
            var dataRow = new[]
            {
                new SpreadsheetCell(ProteinId),
                new SpreadsheetCell(ChainInteractionResidueIndexList[StaticValues.ChainA].Distinct().Count()),
                new SpreadsheetCell(ChainInteractionResidueIndexList[StaticValues.ChainB].Distinct().Count()),
                new SpreadsheetCell(ChainInteractionResidueIndexList[StaticValues.ChainA].Count),
                new SpreadsheetCell(ChainInteractionResidueIndexList[StaticValues.ChainB].Count),
                new SpreadsheetCell(TotalInteractions),
                new SpreadsheetCell(Result.InteractionMatchPercentageA),
                new SpreadsheetCell(Result.InteractionMatchPercentageB),
                new SpreadsheetCell(Result.InteractionMatchPercentageAverage)
            };

            return dataRow;
        }

        public class CalculatePercentageResult
        {
            public decimal InteractionMatchPercentageA;
            public decimal InteractionMatchPercentageAverage;
            public decimal InteractionMatchPercentageB;

            public CalculatePercentageResult(decimal interactionMatchPercentageA, decimal interactionMatchPercentageB, decimal interactionMatchPercentageAverage)
            {
                InteractionMatchPercentageA = interactionMatchPercentageA;
                InteractionMatchPercentageB = interactionMatchPercentageB;
                InteractionMatchPercentageAverage = interactionMatchPercentageAverage;
            }
        }
    }
}