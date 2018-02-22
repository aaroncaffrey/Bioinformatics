using System;
using System.IO;
//using System.Runtime.InteropServices;
//using Microsoft.Office.Interop.Excel;

namespace BioinformaticsHelperLibrary.Spreadsheets
{/*
    public class TsvToXlConversion
    {
        /// <summary>
        ///     Marshal release com object with checks that the object is not null and the object is indeed a com object.
        /// </summary>
        /// <param name="comObject">The com object to be released.</param>
        public static void MarshalReleaseComObject(object comObject)
        {
            if ((comObject != null) && (Marshal.IsComObject(comObject)))
            {
                Marshal.ReleaseComObject(comObject);
            }
        }

        public static void ConvertTsvToExcel(string inputTsvFullPath, string outputExcelFullPath, bool deleteTsv = false)
        {
            if (string.IsNullOrWhiteSpace(inputTsvFullPath))
            {
                throw new ArgumentOutOfRangeException(nameof(inputTsvFullPath));
            }

            if (string.IsNullOrWhiteSpace(outputExcelFullPath))
            {
                throw new ArgumentOutOfRangeException(nameof(outputExcelFullPath));
            }

            var tsvFilename = new FileInfo(inputTsvFullPath);
            var xlFilename = new FileInfo(outputExcelFullPath);

            const int maxSupportedXlFilenameLength = 218;

            if (xlFilename.FullName.Length > maxSupportedXlFilenameLength)
            {
                throw new ArgumentOutOfRangeException(nameof(outputExcelFullPath), outputExcelFullPath, ("The full path filename (" + xlFilename.FullName.Length + " characters) is longer than Microsoft Excel supports (" + maxSupportedXlFilenameLength + " characters)"));
            }

            var excelApp = new Application();
            Workbooks wbs = excelApp.Workbooks;
            Workbook wb = wbs.Open(tsvFilename.FullName);
            //var sheets = excelApp.Worksheets;

            wb.SaveAs(xlFilename.FullName, XlFileFormat.xlOpenXMLWorkbook);

            try
            {
                wb.Close();
                //excel.Quit();
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //MarshalReleaseComObject(sheets);
                MarshalReleaseComObject(wb);
                MarshalReleaseComObject(wbs);
                MarshalReleaseComObject(excelApp);
            }

            if (deleteTsv)
            {
                File.Delete(tsvFilename.FullName);
            }
        }
    }
    */
}