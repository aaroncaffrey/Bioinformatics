using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Spreadsheets;

namespace BioinformaticsHelperLibrary.TypeConversions
{
    public static class ConvertTypes

    {

        public static decimal[,] ListDecimalListToDecimal2DArray(List<List<decimal>> list)
        {
            var result = new decimal[list.Count, list.Select(a => a.Count).Max()];

            for (var x = 0; x < result.GetLength(0); x++)
                for (var y = 0; y < result.GetLength(1); y++)

                    result[x, y] = list[x][y];

            return result;
        }

        public static List<List<decimal>> Decimal2DArrayToDecimalListList(decimal[,] array)
        {
            var result = new List<List<decimal>>();

            for (var x = 0; x < array.GetLength(0); x++)
            {
                result.Add(new List<decimal>());
            }

            for (var x = 0; x < array.GetLength(0); x++)
                for (var y = 0; y < array.GetLength(1); y++)
                    result[x].Add(array[x, y]);

            return result;
        }

        public static decimal[,] Int2DArrayToDecimal2DArray(int[,] array)
        {
            var result = new decimal[array.GetLength(0), array.GetLength(1)];
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    result[x, y] = array[x, y];
            return result;
        }

        public static decimal[,] Double2DArrayToDecimal2DArray(double[,] array)
        {
            var result = new decimal[array.GetLength(0), array.GetLength(1)];
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    result[x, y] = Convert.ToDecimal(array[x, y]);
            return result;
        }

        public static SpreadsheetCell[,] String2DArrayToSpreadsheetCell2DArray(string[,] array)
        {
            var result = new SpreadsheetCell[array.GetLength(0), array.GetLength(1)];
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    result[x, y] = new SpreadsheetCell(array[x, y]);
            return result;
        }

        public static string[,] StringJagged2DArrayTo2DArray(string[][] array)
        {
            var rows = array.Length;
            var cols = array.Select(a => a.Length).Max();

            var result = new string[rows, cols];

            for (var r = 0; r < array.Length; r++)
            {
                for (var c = 0; c < array[r].Length; c++)
                {
                    result[r, c] = array[r][c];
                }
            }

            return result;
        }

        public static SpreadsheetCell[][] SpreadsheetCell2DArrayToJaggedArray(SpreadsheetCell[,] spreadsheet)
        {
            var convertedSpreadsheet = new SpreadsheetCell[spreadsheet.GetLength(0)][];

            for (var indexX = 0; indexX < spreadsheet.GetLength(0); indexX++)
            {
                convertedSpreadsheet[indexX] = new SpreadsheetCell[spreadsheet.GetLength(1)];

                for (var indexY = 0; indexY < spreadsheet.GetLength(1); indexY++)
                {
                    convertedSpreadsheet[indexX][indexY] = spreadsheet[indexX, indexY];
                }
            }

            return convertedSpreadsheet;
        }
    }
}
