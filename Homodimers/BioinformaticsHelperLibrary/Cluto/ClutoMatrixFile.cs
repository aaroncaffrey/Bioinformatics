using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BioinformaticsHelperLibrary.Misc;
using DocumentFormat.OpenXml.InkML;

namespace BioinformaticsHelperLibrary.Cluto
{
    public static class ClutoMatrixFile
    {
        public const string ClutoRowFileExt = ".rlabel";
        public const string ClutoColumnFileExt = ".clabel";
        public const string ClutoMatrixFileExt = ".mat";

        public static string[][] LoadDelimitedMatrix(string filename, char columnDelimiter = ',')
        {
            var data = File.ReadAllLines(filename);

            var rows = data.Select(a => a.Split(columnDelimiter)).ToArray();

            return rows;
        }

        public static string[][] RemoveFirstRow(string[][] matrix)
        {
            return matrix.Where((v, i) => i != 0).ToArray();
        }

        public static string[][] RemoveFirstColumn(string[][] matrix)
        {
            return matrix.Select((v, i) => v.Where((u, j) => j != 0).ToArray()).ToArray();
        }

        public static string[] GetFirstRow(string[][] matrix)
        {
            return matrix[0].Where((v, i) => i != 0).ToArray();
        }

        public static string[] GetFirstColumn(string[][] matrix)
        {
            return matrix.Where((v, i) => i != 0).Select(a => a[0]).ToArray();
        }

        public static decimal[,] DiagonalZeroHalfMatrix(decimal[,] distanceMatrix, bool firstHalf = true)
        {
            var distanceMatrixCopy = new decimal[distanceMatrix.GetLength(0),distanceMatrix.GetLength(1)];
            Array.Copy(distanceMatrix, distanceMatrixCopy, distanceMatrix.Length);

            for (var x = 0; x < distanceMatrix.GetLength(0); x++)
            {
                for (var y = 0; y < distanceMatrix.GetLength(1); y++)
                {
                    if ((y > x && firstHalf) || (y < x && !firstHalf))
                    {
                        distanceMatrixCopy[x, y] = 0;
                    }
                }
            }

            return distanceMatrixCopy;
        }


        /// <summary>
        /// Outputs distance matrix in cluto dense/sparse matrix format (*.mat), row headings in cluto row label file format (*.mat.rlabel) and column headings in cluto column label file format (*.mat.clabel)
        /// </summary>
        /// <param name="distanceMatrix"></param>
        /// <param name="matrixOutputFilename"></param>
        /// <param name="rowLabels"></param>
        /// <param name="rowLabelsOutputFilename"></param>
        /// <param name="columnLabels"></param>
        /// <param name="columnLabelsOutputFilename"></param>
        /// <param name="zeroHalf"></param>
        /// <param name="sprase"></param>
        /// <param name="clutoMatrixFormatTypes"></param>
        /// <param name="fileExistsOptions"></param>
        /// <returns></returns>
        public static string[] ConvertToMatrixFile(decimal[,] distanceMatrix, string matrixOutputFilename, string[] rowLabels = null, string rowLabelsOutputFilename = null, string[] columnLabels = null, string columnLabelsOutputFilename = null, ClutoMatrixFormatTypes clutoMatrixFormatTypes = ClutoMatrixFormatTypes.SparseMatrixTopHalf, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            if (distanceMatrix == null || distanceMatrix.Length == 0)
            {
                throw new ArgumentNullException(nameof(distanceMatrix), "null or empty");
            }

            if (string.IsNullOrWhiteSpace(matrixOutputFilename))
            {
                throw new ArgumentNullException(nameof(matrixOutputFilename), "null or empty");
            }

            if (rowLabels != null && rowLabels.Length != distanceMatrix.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(rowLabels), "length not equal to matrix rows");
            }

            if (columnLabels != null && columnLabels.Length != distanceMatrix.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(columnLabels), "length not equal to matrix columns");
            }

            if (columnLabels != null && string.IsNullOrWhiteSpace(columnLabelsOutputFilename))
            {
                throw new ArgumentNullException(nameof(columnLabelsOutputFilename), "null or empty");
            }

            if (rowLabels != null && string.IsNullOrWhiteSpace(rowLabelsOutputFilename))
            {
                throw new ArgumentNullException(nameof(rowLabelsOutputFilename), "null or empty");
            }

            if (clutoMatrixFormatTypes == ClutoMatrixFormatTypes.DenseMatrixTopHalf || clutoMatrixFormatTypes == ClutoMatrixFormatTypes.SparseMatrixTopHalf)
            {
                distanceMatrix = DiagonalZeroHalfMatrix(distanceMatrix, true);
            }
            else if (clutoMatrixFormatTypes == ClutoMatrixFormatTypes.DenseMatrixBottomHalf || clutoMatrixFormatTypes == ClutoMatrixFormatTypes.SparseMatrixBottomHalf)
            {
                distanceMatrix = DiagonalZeroHalfMatrix(distanceMatrix, false);
            }

            bool sparse = clutoMatrixFormatTypes == ClutoMatrixFormatTypes.SparseMatrixBottomHalf || clutoMatrixFormatTypes == ClutoMatrixFormatTypes.SparseMatrixTopHalf || clutoMatrixFormatTypes == ClutoMatrixFormatTypes.SparseMatrixComplete;

            var result = new List<List<string>>();

            result.Add(new List<string>());

            var rowTotal = distanceMatrix.GetLength(1);
            var columnTotal = distanceMatrix.GetLength(0);
            var nonZeroTotal = distanceMatrix.Cast<decimal>().Count(a => a != 0);

            result[0].Add(rowTotal.ToString());
            result[0].Add(columnTotal.ToString());

            if (sparse)
            {
                result[0].Add(nonZeroTotal.ToString());    
            }

            for (var y = 0; y < rowTotal; y++)
            { 
                result.Add(new List<string>());

                for (var x = 0; x < columnTotal; x++)
                {
                    var value = distanceMatrix[x, y];

                    if (sparse)
                    {
                        if (value == 0) continue;
                        result[result.Count - 1].Add(x.ToString());
                        result[result.Count - 1].Add(value.ToString());
                    }
                    else
                    {
                        result[result.Count - 1].Add(value.ToString());    
                    }
                    
                }
            }

            var lines = result.Select(a => string.Join(" ", a)).ToList();

            var savedFiles = new List<string>();

            if (File.Exists(matrixOutputFilename))
            {
                if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                {
                    matrixOutputFilename = FileExistsHandler.FindNextFreeOutputFilename(matrixOutputFilename);
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                {
                    
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                {
                    return savedFiles.ToArray();
                }
            }


            FileAndPathMethods.CreateDirectory(matrixOutputFilename);
            File.WriteAllLines(matrixOutputFilename, lines);
            savedFiles.Add(matrixOutputFilename);

            if (rowLabels != null && !string.IsNullOrWhiteSpace(rowLabelsOutputFilename))
            {
                var saveRowLabels = true;

                if (File.Exists(rowLabelsOutputFilename))
                {
                    if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                    {
                        rowLabelsOutputFilename = FileExistsHandler.FindNextFreeOutputFilename(rowLabelsOutputFilename);
                    }
                    else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                    {

                    }
                    else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                    {
                        saveRowLabels = false;
                    }
                }

                if (saveRowLabels)
                {
                    FileAndPathMethods.CreateDirectory(rowLabelsOutputFilename);
                    File.WriteAllLines(rowLabelsOutputFilename, rowLabels);
                    savedFiles.Add(rowLabelsOutputFilename);
                }
            }

            if (columnLabels != null && !string.IsNullOrWhiteSpace(columnLabelsOutputFilename))
            {
                var saveColumnLabels = true;

                if (File.Exists(columnLabelsOutputFilename))
                {
                    if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                    {
                        columnLabelsOutputFilename = FileExistsHandler.FindNextFreeOutputFilename(columnLabelsOutputFilename);
                    }
                    else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                    {

                    }
                    else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                    {
                        saveColumnLabels = false;
                    }
                }

                if (saveColumnLabels)
                {
                    FileAndPathMethods.CreateDirectory(columnLabelsOutputFilename);
                    File.WriteAllLines(columnLabelsOutputFilename, columnLabels);
                    savedFiles.Add(columnLabelsOutputFilename);
                }
            }

            return savedFiles.ToArray();
        }

    }
}
