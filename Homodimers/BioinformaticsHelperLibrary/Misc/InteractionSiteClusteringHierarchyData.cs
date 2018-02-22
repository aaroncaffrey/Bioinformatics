//-----------------------------------------------------------------------
// <copyright file="____________.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using BioinformaticsHelperLibrary.Spreadsheets;

namespace BioinformaticsHelperLibrary.Misc
{
    public class InteractionProteinInterfaceClusteringHierarchyData
    {
        public string ChainId;
        public int ClusteringStageProteinInterfaceFoundAt;
        public int ClusteringStagesTotal;
        public string ProteinId;
        public int ProteinInterfaceCount;

        public InteractionProteinInterfaceClusteringHierarchyData(string proteinId, string chainId, int proteinInterfaceCount, int clusteringStageProteinInterfaceFoundAt, int clusteringStagesTotal)
        {
            ProteinId = proteinId;
            ChainId = chainId;
            ProteinInterfaceCount = proteinInterfaceCount;
            ClusteringStageProteinInterfaceFoundAt = clusteringStageProteinInterfaceFoundAt;
            ClusteringStagesTotal = clusteringStagesTotal;
        }

        public SpreadsheetCell[] SpreadsheetDataRow()
        {
            var dataRow = new[]
            {
                new SpreadsheetCell(ProteinId),
                new SpreadsheetCell(ChainId),
                new SpreadsheetCell(ProteinInterfaceCount),
                new SpreadsheetCell(ClusteringStageProteinInterfaceFoundAt),
                new SpreadsheetCell(ClusteringStagesTotal), 
            };

            return dataRow;
        }

    }
}