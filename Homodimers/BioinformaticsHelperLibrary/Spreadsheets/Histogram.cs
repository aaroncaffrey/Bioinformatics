using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using DocumentFormat.OpenXml.Validation;

namespace BioinformaticsHelperLibrary.Spreadsheets
{
    public static class Histogram
    {

        /*
         * 
         *         
         * Sample usage: 
         * 
            string[] lineArray = File.ReadAllLines(@"c:\d\s\e\1 - Homodimers - List - Protein Interaction Record.tsv");

            var rowList = new SpreadsheetCell[lineArray.Length][];

            int rowIndex = 0;

            foreach (string line in lineArray)
            {
                rowList[rowIndex] = line.Split(new[] {"\t"}, StringSplitOptions.None).Select(a => new SpreadsheetCell(a.Trim())).ToArray();

                rowIndex++;
            }


            SpreadsheetCell[][] histogram3 = Histogram.MatrixToHistogram(rowList, Histogram.MakeBinDecimals(0m, 5m, 0.00100m), new[] {1}, -1, -1);

            rowList = Histogram.InsertMatrixOverwrite(rowList, histogram3, 1, Histogram.MaxColumns(rowList) + 1);

            textBoxUserFeedback.Lines = rowList.Select(a => String.Join("", a.Select(b => b.CellData.Substring(0, b.CellData.Length < 6 ? b.CellData.Length : 6).PadRight(10)).ToArray())).ToArray();

         * 
         */

        public static int MaxColumns(SpreadsheetCell[][] matrix)
        {
            return matrix.Where(x => x != null).Select(x => x.Length).Concat(new[] {0}).Max();
        }

        public static int MaxRows(SpreadsheetCell[][] matrix)
        {
            return matrix.Length;
        }

        public static SpreadsheetCell[][] InsertMatrixOverwrite(SpreadsheetCell[][] matrix, SpreadsheetCell[][] matrixToInsert, int insertAtRow, int insertAtColumn)
        {
            int matrixMaxColumns = MaxColumns(matrix);
            int matrixMaxRows = MaxRows(matrix);

            int matrixToInsertMaxColumns = MaxColumns(matrixToInsert);
            int matrixToInsertMaxRows = MaxRows(matrixToInsert);

            int columnsRequired = (insertAtColumn + matrixToInsertMaxColumns); // - 1;

            if (matrixMaxColumns > columnsRequired)
            {
                columnsRequired = matrixMaxColumns;
            }

            int rowsRequired = (insertAtRow + matrixToInsertMaxRows); // - 1;

            if (matrixMaxRows > rowsRequired)
            {
                rowsRequired = matrixMaxRows;
            }

            var combinedMatrix = new SpreadsheetCell[rowsRequired][];
            for (int rowIndex = 0; rowIndex < combinedMatrix.Length; rowIndex++)
            {
                combinedMatrix[rowIndex] = new SpreadsheetCell[columnsRequired];

                for (int columnIndex = 0; columnIndex < combinedMatrix[rowIndex].Length; columnIndex++)
                {
                    combinedMatrix[rowIndex][columnIndex] = new SpreadsheetCell();
                }
            }

            for (int rowIndex = 0; rowIndex < matrix.Length; rowIndex++)
            {
                var row = matrix[rowIndex];

                if (row == null)
                {
                    continue;
                }

                for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    var cell = row[columnIndex];

                    if (cell == null)
                    {
                        cell = new SpreadsheetCell();
                    }

                    combinedMatrix[rowIndex][columnIndex] = cell;
                }
            }

            for (int rowIndex = 0; rowIndex < matrixToInsert.Length; rowIndex++)
            {
                var row = matrixToInsert[rowIndex];

                if (row == null)
                {
                    continue;
                }

                for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    var cell = row[columnIndex];

                    if (cell == null)
                    {
                        cell = new SpreadsheetCell();
                    }

                    combinedMatrix[insertAtRow + rowIndex][insertAtColumn + columnIndex] = cell;
                }
            }

            return combinedMatrix;
        }

        public static decimal[] MakeBinDecimals(decimal minValue = 0, decimal maxValue = 100, decimal stepStartPosition = 0, decimal binStep = 10)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }

            if (binStep == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(binStep));
            }

            if (stepStartPosition < minValue)
            {
                throw new ArgumentOutOfRangeException(nameof(stepStartPosition));
            }

            var result = new List<decimal>();

            

            if (stepStartPosition > minValue)
            {
                result.Add(minValue);
            }

            decimal value;
            for (value = stepStartPosition; value <= maxValue + binStep; value += binStep)
            {
                if (value >= maxValue)
                {
                    result.Add(maxValue);
                    break;
                }

                result.Add(value);                
            }

            return result.ToArray();
        }

        public static SpreadsheetCell[][] MatrixToHistogram(SpreadsheetCell[][] spreadsheetRowList, decimal[] binsDecimals, int[] columnNumberArray, int startRow = 1, int endRow = -1, bool firstRowHeaders = true)
        {
            if (spreadsheetRowList == null || spreadsheetRowList.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(spreadsheetRowList));
            }

            if (binsDecimals == null || binsDecimals.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(binsDecimals));
            }

            if (columnNumberArray == null || columnNumberArray.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnNumberArray));
            }

            decimal[] binsDecimalsSorted = binsDecimals.ToList().Distinct().OrderBy(d => d).ToArray();


            //var binMin = binsDecimals.Min();
            decimal binMax = binsDecimals.Max();

            int firstRow = startRow > -1 ? startRow : 0;

            var headers = new string[columnNumberArray.Length];

            if (firstRowHeaders)
            {
                
                
                for (var columnIndex = columnNumberArray.Min(); columnIndex <= columnNumberArray.Max(); columnIndex++)
                {
                    headers[columnIndex - columnNumberArray.Min()] = spreadsheetRowList[firstRow][columnIndex].CellData;
                }

                firstRow++;
            }


            int lastRow = endRow > -1 && endRow <= spreadsheetRowList.Length ? endRow : spreadsheetRowList.Length;

            //var lessBin = new int[columnNumberArray.Length];
            var moreBin = new int[columnNumberArray.Length];

            var totals = new int[columnNumberArray.Length];

            var binsDictionary = new Dictionary<decimal, int[]>();
            foreach (decimal d in binsDecimalsSorted)
            {
                binsDictionary.Add(d, new int[columnNumberArray.Length]);
            }

            for (int rowIndex = firstRow; rowIndex < lastRow; rowIndex++)
            {
                var row = spreadsheetRowList[rowIndex];

                for (int columnNumberArrayIndex = 0; columnNumberArrayIndex < columnNumberArray.Length; columnNumberArrayIndex++)
                {
                    int columnNumber = columnNumberArray[columnNumberArrayIndex];

                    if (columnNumber >= row.Length) 
                    {
                        continue;
                    }

                    var column = row[columnNumber];

                    if (column == null)
                    {
                        continue;
                    }

                    decimal columnDecimal;
                    if (!decimal.TryParse(column.ToString(), out columnDecimal))
                    {
                        continue;
                    }

                    if (columnDecimal > binMax)
                    {
                        moreBin[columnNumberArrayIndex]++;
                        totals[columnNumberArrayIndex]++;
                    }
                    else if (binsDecimalsSorted.Length > 1)
                    {
                        for (int index = 0; index < binsDecimalsSorted.Length - 1; index++)
                        {
                            decimal d1 = binsDecimalsSorted[index];
                            decimal d2 = binsDecimalsSorted[index + 1];

                            if (columnDecimal > d1 && columnDecimal <= d2)
                            {
                                binsDictionary[d2][columnNumberArrayIndex]++;
                                totals[columnNumberArrayIndex]++;
                                break;
                            }
                            if (index == 0 && columnDecimal <= d1)
                            {
                                binsDictionary[d1][columnNumberArrayIndex]++;
                                totals[columnNumberArrayIndex]++;
                                break;
                            }
                        }
                    }
                    else if (binsDecimalsSorted.Length == 1)
                    {
                        decimal d1 = binsDecimalsSorted[0];
                        if (columnDecimal <= d1)
                        {
                            binsDictionary[d1][columnNumberArrayIndex]++;
                            totals[columnNumberArrayIndex]++;
                        }
                    }
                }
            }

            var headerColumnList = new List<SpreadsheetCell> {new SpreadsheetCell("Bin")};
            headerColumnList.AddRange(columnNumberArray.Select(columnNumber => new SpreadsheetCell("Total: column " + SpreadsheetFileHandler.AlphabetLetterRollOver(columnNumber))));

            if (firstRowHeaders && headers.Length > 0)
            {
                for (int index = 0; index < headers.Length; index++)
                {
                    var header = headers[index];
                    if (!string.IsNullOrWhiteSpace(header))
                    {
                        headerColumnList[index + 1] = new SpreadsheetCell(header);
                    }
                }
            }

            var result = new SpreadsheetCell[binsDecimalsSorted.Length + 3][];

            int resultRow = 0;
            result[resultRow] = headerColumnList.ToArray();

            foreach (var kvp in binsDictionary)
            {
                resultRow++;
                var x = new List<SpreadsheetCell>
                {
                    new SpreadsheetCell(kvp.Key.ToString(CultureInfo.InvariantCulture))
                };
                x.AddRange(kvp.Value.Select(a => new SpreadsheetCell(a.ToString(CultureInfo.InvariantCulture))).ToList());
                result[resultRow] = x.ToArray();
            }

            if (moreBin.Count(a => a > 0) > 0)
            {
                resultRow++;
                result[resultRow] = (new[] {new SpreadsheetCell("More")}).Concat(moreBin.Select(a => new SpreadsheetCell(a.ToString(CultureInfo.InvariantCulture)))).ToArray();
            }

            resultRow++;
            result[resultRow] = (new[] {new SpreadsheetCell("Total")}).Concat(totals.Select(a => new SpreadsheetCell(a.ToString(CultureInfo.InvariantCulture)))).ToArray();

            return result;
        }
    }
}