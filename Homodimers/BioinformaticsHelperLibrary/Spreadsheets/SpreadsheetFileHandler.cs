//-----------------------------------------------------------------------
// <copyright file="SpreadsheetFileHandler.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.TypeConversions;
using BioinformaticsHelperLibrary.UserProteinInterface;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BioinformaticsHelperLibrary.Spreadsheets
{
    public class SpreadsheetFileHandler
    {
        public static string AlphabetLetterRollOver(int alphabetLetterNumberRollOver)
        {
            const int firstLetterInAlphabet = 65;
            const int lettersInAlphabet = 26;

            string columnName = string.Empty;
            int dividend = alphabetLetterNumberRollOver + 1;

            while (dividend > 0)
            {
                int letterNumber = (dividend - 1) % lettersInAlphabet;
                var letter = ((char)(firstLetterInAlphabet + letterNumber));
                columnName = letter + columnName;
                dividend = ((dividend - letterNumber) / lettersInAlphabet);
            }

            return columnName;
        }

        public static string[] SaveSpreadsheet(string saveFilename, string[] sheetNames, List<List<SpreadsheetCell>> spreadsheet, ProgressActionSet progressActionSet = null, bool tsvFormat = false, bool xlsxFormat = true, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            return SaveSpreadsheet(saveFilename, sheetNames, spreadsheet.Select(a => a.ToArray()).ToList(), progressActionSet, tsvFormat, xlsxFormat, fileExistsOptions);
        }

        public static string[] SaveSpreadsheet(string saveFilename, string[] sheetNames, SpreadsheetCell[][] spreadsheet, ProgressActionSet progressActionSet = null, bool tsvFormat = false, bool xlsxFormat = true, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            return SaveSpreadsheet(saveFilename, sheetNames, spreadsheet.Select(a => a.ToArray()).ToList(), progressActionSet, tsvFormat, xlsxFormat, fileExistsOptions);
        }

        public static string[] SaveSpreadsheet(string saveFilename, string[] sheetNames, List<SpreadsheetCell[,]> spreadsheet, ProgressActionSet progressActionSet = null, bool tsvFormat = false, bool xlsxFormat = true, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            var convertedSpreadsheet = new List<List<SpreadsheetCell[]>>();

            foreach (var sheet in spreadsheet)
            {
                var convertedSheet = ConvertTypes.SpreadsheetCell2DArrayToJaggedArray(sheet).ToList();
                convertedSpreadsheet.Add(convertedSheet);
            }

            return SaveSpreadsheet(saveFilename, sheetNames, convertedSpreadsheet, progressActionSet, tsvFormat, xlsxFormat, fileExistsOptions);
        }



        public static string[] SaveSpreadsheet(string saveFilename, string[] sheetNames, SpreadsheetCell[,] spreadsheet, ProgressActionSet progressActionSet = null, bool tsvFormat = false, bool xlsxFormat = true, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            var convertedSpreadsheet = ConvertTypes.SpreadsheetCell2DArrayToJaggedArray(spreadsheet);

            return SaveSpreadsheet(saveFilename, sheetNames, convertedSpreadsheet.ToList(), progressActionSet, tsvFormat, xlsxFormat, fileExistsOptions);
        }

        public static string[] SaveSpreadsheet(string saveFilename, string[] sheetNames, List<SpreadsheetCell[]> spreadsheet, ProgressActionSet progressActionSet = null, bool tsvFormat = false, bool xlsxFormat = true, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            return SaveSpreadsheet(saveFilename, sheetNames, new List<List<SpreadsheetCell[]>>() { spreadsheet }, progressActionSet, tsvFormat, xlsxFormat, fileExistsOptions);
        }

        /// <summary>
        /// </summary>
        /// <returns>The filenames of the newly saved files</returns>
        public static string[] SaveSpreadsheet(string saveFilename, string[] sheetNames, List<List<SpreadsheetCell[]>> spreadsheet, ProgressActionSet progressActionSet = null, bool tsvFormat = false, bool xlsxFormat = true, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            if (!tsvFormat && !xlsxFormat)
            {
                throw new ArgumentException("Spreadsheet must be either TSV and/or XLSX format");
            }

            var result = new List<string>();

            if (tsvFormat)
            {
                string[] tsvSavedFiles = SaveSpreadsheetTsv(saveFilename, sheetNames, spreadsheet, progressActionSet, fileExistsOptions);
                result.AddRange(tsvSavedFiles);
            }

            if (xlsxFormat)
            {
                string[] xlSavedFiles = SaveSpreadsheetXl(saveFilename, sheetNames, spreadsheet, progressActionSet, fileExistsOptions);
                result.AddRange(xlSavedFiles);
            }

            return result.ToArray();
        }

        public static string[] SaveSpreadsheetTsv(string saveFilename, string[] sheetNames, List<List<SpreadsheetCell[]>> spreadsheet, ProgressActionSet progressActionSet = null, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            var resultList = new List<string>();
            for (int sheetIndex = 0; sheetIndex < spreadsheet.Count; sheetIndex++)
            {
                var padLen = ("" + spreadsheet.Count).Length;
                if (padLen < 2) padLen = 2;
                var paddedSheetIndex = ("" + (sheetIndex+1)).PadLeft(padLen, '0');

                var sheetName = "";
                if (sheetNames != null && sheetNames.Length > sheetIndex)
                {
                    sheetName = sheetNames[sheetIndex];
                }

                if (string.IsNullOrWhiteSpace(sheetName))
                {
                    sheetName = Path.GetFileNameWithoutExtension(saveFilename) + " [sheet " + paddedSheetIndex + "]";
                }

                var saveSheetFilename = FileAndPathMethods.MergePathAndFilename(Path.GetDirectoryName(saveFilename), sheetName + Path.GetExtension(saveFilename));
                var result = SaveSpreadsheetTsv(saveSheetFilename, spreadsheet[sheetIndex], progressActionSet, fileExistsOptions);
                resultList.AddRange(result);
            }
            return resultList.ToArray();
        }

        public static string[] SaveSpreadsheetTsv(string saveFilename, List<SpreadsheetCell[]> spreadsheet, ProgressActionSet progressActionSet = null, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            var result = new List<string>();
            var tsvFilename = new FileInfo(FileAndPathMethods.RemoveFileExtension(saveFilename) + ".tsv");

            if (tsvFilename.Exists)
            {
                if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                {
                    tsvFilename = new FileInfo(FileExistsHandler.FindNextFreeOutputFilename(tsvFilename.FullName));
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                {
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                {
                    return result.ToArray();
                }
            }

            var stringBuilder = new StringBuilder();
            for (int rowIndex = 0; rowIndex < spreadsheet.Count; rowIndex++)
            {
                var rowDataArray = spreadsheet[rowIndex];

                if (rowDataArray == null) continue;

                stringBuilder.AppendLine(string.Join("\t", rowDataArray.Select(row => row.CellData)));
            }

            if (tsvFilename.Directory != null) tsvFilename.Directory.Create();

            File.WriteAllText(tsvFilename.FullName, stringBuilder.ToString());
            result.Add(tsvFilename.FullName);

            return result.ToArray();
        }

        private static string XlSheetNameVerification(string sheetName)
        {
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new ArgumentOutOfRangeException(nameof(sheetName), sheetName, "A excel workbook sheet name could not be taken from the file name or given source.");
            }
            
            if (sheetName.Length > 0)
            {
                sheetName = sheetName.Substring(0, sheetName.Length >= 31 ? 31 : sheetName.Length);

                sheetName = sheetName.Replace(@":", @"_");
                sheetName = sheetName.Replace(@"\", @"_");
                sheetName = sheetName.Replace(@"/", @"_");
                sheetName = sheetName.Replace(@"?", @"_");
                sheetName = sheetName.Replace(@"*", @"_");
                sheetName = sheetName.Replace(@"[", @"_");
                sheetName = sheetName.Replace(@"]", @"_");
            }

            return sheetName;
        }

        public static string[] SaveSpreadsheetXl(string saveFilename, string[] sheetNames, List<SpreadsheetCell[]> spreadsheet, ProgressActionSet progressActionSet = null, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            return SaveSpreadsheetXl(saveFilename, sheetNames, new List<List<SpreadsheetCell[]>>() {spreadsheet}, progressActionSet, fileExistsOptions);
        }

        public static RunProperties[][] GetAminoAcidRunProperties()
        {
            var runPropertiesArray = new RunProperties[AminoAcidGroups.AminoAcidGroups.GetTotalGroups()][];

            foreach (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups in Enum.GetValues(typeof (AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups)))
            {
                runPropertiesArray[(int) enumAminoAcidGroups] = new RunProperties[AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups)];

                for (var index = 0; index < runPropertiesArray[(int) enumAminoAcidGroups].Length; index++)
                {
                    RunProperties runProperties = new RunProperties();
                    FontSize fontSize = new FontSize() {Val = 10D};
                    Color color = new Color() {Rgb = "FF000000"};
                    RunFont runFont = new RunFont() {Val = "Consolas"};
                    FontFamily fontFamily = new FontFamily() {Val = 3};

                    runProperties.Append(fontSize);
                    runProperties.Append(color);
                    runProperties.Append(runFont);
                    runProperties.Append(fontFamily);

                    runPropertiesArray[(int) enumAminoAcidGroups][index] = runProperties;
                }
            }

            return runPropertiesArray;
        }

        public static string[] SaveSpreadsheetXl(string saveFilename, string[] sheetNames, List<List<SpreadsheetCell[]>> spreadsheet, ProgressActionSet progressActionSet = null, FileExistsHandler.FileExistsOptions fileExistsOptions = FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
        {
            var result = new List<string>();

            var xlFilename = new FileInfo(FileAndPathMethods.RemoveFileExtension(saveFilename) + ".xlsx");

            if (xlFilename.Exists)
            {
                if (fileExistsOptions == FileExistsHandler.FileExistsOptions.AppendNumberToFilename)
                {
                    xlFilename = new FileInfo(FileExistsHandler.FindNextFreeOutputFilename(xlFilename.FullName));
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.OverwriteFile)
                {
                }
                else if (fileExistsOptions == FileExistsHandler.FileExistsOptions.SkipFile)
                {
                    return result.ToArray();
                }
            }

            if (xlFilename.Directory != null) xlFilename.Directory.Create();

            var totalSheets = spreadsheet.Count;


            string sheetName = XlSheetNameVerification(Path.GetFileNameWithoutExtension(xlFilename.Name));

            if (sheetNames == null || sheetNames.Length < spreadsheet.Count)
            {
                var extraSheetNames = new string[totalSheets];

                if (sheetNames != null && sheetNames.Length > 0)
                {
                    Array.Copy(sheetNames, extraSheetNames, sheetNames.Length);
                }

                for (var sheetIndex = sheetNames != null ? sheetNames.Length : 0; sheetIndex < extraSheetNames.Length; sheetIndex++)
                {
                    extraSheetNames[sheetIndex] = sheetName + (sheetIndex + 1);
                }

                sheetNames = extraSheetNames;
            }

            for (int index = 0; index < sheetNames.Length; index++)
            {
                if (string.IsNullOrWhiteSpace(sheetNames[index])) sheetNames[index] = sheetName + (index + 1);
                 
                sheetNames[index]= XlSheetNameVerification(sheetNames[index]);
            }


            XlSpreadsheetDocumentContainer xlSpreadsheet = ExcelCreateSpreadsheet.CreateSpreadsheetWorkbook(xlFilename.FullName, sheetNames);


            //Worksheet worksheet1 = xlSpreadsheet.WorksheetPart.Worksheet;//new Worksheet();

            var runPropertiesArray = GetAminoAcidRunProperties();

            for (uint sheetIndex = 0; sheetIndex < spreadsheet.Count; sheetIndex++)
            {
                var worksheet1 = new Worksheet();

                var sheetData1 = new SheetData();


                for (uint rowIndex = 0; rowIndex < spreadsheet[(int) sheetIndex].Count; rowIndex++)
                {
                    var rowDataArray = spreadsheet[(int)sheetIndex][(int)rowIndex];

                    if (rowDataArray == null) continue;

                    var row1 = new Row {RowIndex = (UInt32Value) rowIndex + 1 /*, Spans = new ListValue<StringValue>() { InnerText = "1:3" }, DyDescent = 0.25D*/};
                    //var row4 = new Row(){ RowIndex = (UInt32Value)4U, Spans = new ListValue<StringValue>() { InnerText = "1:2" }, DyDescent = 0.25D };

                    for (uint columnIndex = 0; columnIndex < rowDataArray.Length; columnIndex++)
                    {
                        string columnValue = rowDataArray[columnIndex].CellData;

                        if (string.IsNullOrWhiteSpace(columnValue))
                        {
                            continue;
                        }

                        string columnName = AlphabetLetterRollOver((int) columnIndex);
                        string cellRef = columnName + (rowIndex + 1);

                        var cell1 = new Cell {CellReference = cellRef, StyleIndex = 1U};

                        switch (rowDataArray[columnIndex].SpreadsheetDataType)
                        {
                            case SpreadsheetDataTypes.String:
                                cell1.DataType = CellValues.String;
                                break;
                            case SpreadsheetDataTypes.Integer:
                                cell1.DataType = CellValues.Number;
                                break;
                            case SpreadsheetDataTypes.Double:
                                cell1.DataType = CellValues.Number;
                                break;
                            case SpreadsheetDataTypes.Decimal:
                                cell1.DataType = CellValues.Number;
                                break;
                        }

                        //InlineString inlineString1 = new InlineString();
                        //Text text1 = new Text();
                        //text1.Text = columnValue;

                        //inlineString1.Append(text1);

                        //cell1.Append(inlineString1);


                        //if (rowDataArray[columnIndex].CellColourScheme == SpreadsheetCellColourScheme.Default)
                        //{
                            var cellValue1 = new CellValue();
                            cellValue1.Text = columnValue;
                            cell1.Append(cellValue1);
                            row1.Append(cell1);
                        //}
                        //else if (rowDataArray[columnIndex].CellColourScheme == SpreadsheetCellColourScheme.AminoAcidsUniProtKb)
                        //{

                        //    foreach (var ch in rowDataArray[columnIndex].CellData)
                        //    {
                        //        var subgroups = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToGroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb, ch);
                        //        var firstGroup = subgroups[0];
                        //        //var groupColours = AminoAcidGroups.AminoAcidGroups.GetGroupColors(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb);
                        //        //var firstGroupColour = groupColours[firstGroup];

                        //        var runProperties = runPropertiesArray[(int) AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb][firstGroup];

                        //        Run run = new Run();
                        //        Text text = new Text();
                        //        text.Text = ""+ch;

                        //        run.Append(runProperties);
                        //        run.Append(text);

                        //        cell1.Append(run);
                        //        row1.Append(cell1);
                        //    }                         
                        //}

                        //worksheetPart1.Worksheet = worksheet1;


                        //var cell = ExcelSheets.InsertCellInWorksheet(ProteinInterfaceDetection.AlphabetLetterRollOver(columnIndex), rowIndex + 1, xlSpreadsheet.WorksheetPart);
                        //ExcelSheets.InsertText(xlSpreadsheet.SpreadsheetDocument, xlSpreadsheet.WorksheetPart, cell, "Test");
                        //cell.DataType = new EnumValue<CellValues>(CellValues.String);//.SharedString);
                        //cell.CellValue = new CellValue("test");

                        //xlSpreadsheet.WorksheetPart.Worksheet.Save();
                    }
                    sheetData1.Append(row1);
                }

                worksheet1.Append(sheetData1);

                //xlSpreadsheet.WorksheetPart.Worksheet = worksheet1;
                xlSpreadsheet.WorkbookPartObject.WorksheetParts.ToList()[(int) sheetIndex].Worksheet = worksheet1;
            }

            //xlSpreadsheet.WorksheetPart.Worksheet.Save();
            //xlSpreadsheet.WorkbookPart.Workbook.Save();
            //xlSpreadsheet.SpreadsheetDocument.WorkbookPart.Workbook.Save();

            xlSpreadsheet.SpreadsheetDocumentObject.Close();

            result.Add(xlFilename.FullName);
            return result.ToArray();
        }
    }
}