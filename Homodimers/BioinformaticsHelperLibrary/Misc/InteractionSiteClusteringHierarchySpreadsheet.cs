//-----------------------------------------------------------------------
// <copyright file="InteractionProteinInterfaceClusteringHierarchySpreadsheet.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using BioinformaticsHelperLibrary.Spreadsheets;

namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    /// </summary>
    public class InteractionProteinInterfaceClusteringHierarchySpreadsheet
    {
        public List<InteractionProteinInterfaceClusteringHierarchyData> InteractionProteinInterfaceDensityDataList = new List<InteractionProteinInterfaceClusteringHierarchyData>();
        private readonly object _lockInteractionProteinInterfaceDensityDataList = new object();


        public void Add(InteractionProteinInterfaceClusteringHierarchyData interactionProteinInterfaceDensityData)
        {
            lock (_lockInteractionProteinInterfaceDensityDataList)
            {
                InteractionProteinInterfaceDensityDataList.Add(interactionProteinInterfaceDensityData);
            }
        }

        public void AddRange(List<InteractionProteinInterfaceClusteringHierarchyData> interactionProteinInterfaceDensityDataList)
        {
            lock (_lockInteractionProteinInterfaceDensityDataList)
            {
                InteractionProteinInterfaceDensityDataList.AddRange(interactionProteinInterfaceDensityDataList);
            }
        }

        public static SpreadsheetCell[] SpreadsheetColumnHeadersRow()
        {
            var spreadsheetColumnHeadersRow = new[]
            {
                new SpreadsheetCell("PDB ID"),
                new SpreadsheetCell("Chain ID"),
                new SpreadsheetCell("Number ProteinInterfaces Found"),
                new SpreadsheetCell("Clustering Hierarchy Stage"),
                new SpreadsheetCell("Total Clustering Hierarchy Stages"), 
            };

            return spreadsheetColumnHeadersRow;
        }

        public List<SpreadsheetCell[]> SpreadsheetDataRowList(int numberProteinInterfacesRequired = -1)
        {
            var rows = new List<SpreadsheetCell[]>();
            var excludeProteinId = new List<string>();

            lock (_lockInteractionProteinInterfaceDensityDataList)
            {
                foreach (InteractionProteinInterfaceClusteringHierarchyData interactionProteinInterfaceDensityData in InteractionProteinInterfaceDensityDataList)
                {
                    if ((numberProteinInterfacesRequired > -1) && (interactionProteinInterfaceDensityData.ProteinInterfaceCount != numberProteinInterfacesRequired))
                    {
                        excludeProteinId.Add(interactionProteinInterfaceDensityData.ProteinId);
                    }
                }

                foreach (InteractionProteinInterfaceClusteringHierarchyData interactionProteinInterfaceDensityData in InteractionProteinInterfaceDensityDataList)
                {
                    if (!excludeProteinId.Contains(interactionProteinInterfaceDensityData.ProteinId))
                    {
                        rows.Add(interactionProteinInterfaceDensityData.SpreadsheetDataRow());
                    }
                }
            }

            //result.Sort();
            rows = rows.OrderBy(a => a[0].CellData).ThenBy(a=>a[1].CellData).ThenBy(a=>a[2].CellData).ToList();

            return rows;
        }

        public void SaveToFile(string saveFilename, int numberProteinInterfacesRequired = -1)
        {
            var rowList = new List<SpreadsheetCell[]> { SpreadsheetColumnHeadersRow() };

            rowList.AddRange(SpreadsheetDataRowList(numberProteinInterfacesRequired));

            SpreadsheetFileHandler.SaveSpreadsheet(saveFilename, null, rowList);
        }
    }
}