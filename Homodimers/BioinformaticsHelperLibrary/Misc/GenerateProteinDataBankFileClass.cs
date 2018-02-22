//-----------------------------------------------------------------------
// <copyright file="GenerateProteinDataBankFileClass.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    ///     This class generates the beginnings of a c# class which parses PDB format files.  ProteinDataBankFile.cs was
    ///     originally created with this class but has since been heavily modified.
    ///     <para>ample usage: string result = GenerateProteinDataBankFileClass.Generate("c:/pdbfileformatv33.txt");</para>
    /// </summary>
    public static class GenerateProteinDataBankFileClass
    {
        /// <summary>
        ///     MainNameSpaceAndClass
        /// </summary>
        private const string MainNameSpaceAndClass = @"
        namespace GenerateProteinDataBankFileParser
        {
            using System;
            using System.Collections.Generic;
            using System.ComponentModel;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.IO;

            public class ProteinDataBankFile
            {
                %data%
            }
        }
        ";

        /// <summary>
        ///     ProteinDataBankFileConstructors
        /// </summary>
        private const string ProteinDataBankFileConstructors = @"
        public string[] FileLinesArray;
        public List<ProteinDataBankFileRecord> ProteinDataBankFileRecordList;
        private int currentFileRecordNumber = -1;

        public ProteinDataBankFile()
        {

        }

        public ProteinDataBankFile( string filename, Boolean convertToProteinDataBankFileRecordList = true)
        {
            LoadFile( filename, convertToProteinDataBankFileRecordList);
        }
        ";

        /// <summary>
        ///     ProteinDataBankFileRecordFieldClass
        /// </summary>
        private const string ProteinDataBankFileRecordFieldClass = @"
        public abstract class ProteinDataBankFileRecordField
        {
            [Description(string.EmptyThe field's first column on the linestring.Empty), Category(string.Emptystring.Empty)]
            protected int FirstColumn;

            [Description(string.EmptyThe field's last column on the linestring.Empty), Category(string.Emptystring.Empty)]
            protected int LastColumn;

            [Description(string.EmptyThe field's DataType as stated in the specificationstring.Empty), Category(string.Emptystring.Empty)]
            protected string DataType;

            [Description(string.EmptyThe field's name as stated in the specificationstring.Empty), Category(string.Emptystring.Empty)]
            protected string FieldValue;

            [Description(string.EmptyThe field's definition as stated in the specificationstring.Empty), Category(string.Emptystring.Empty)]
            protected string Definition;

            [Description(string.EmptyThe field's value as found in the line between FirstColumn and LastColumnstring.Empty), Category(string.Emptystring.Empty)]
            protected string FieldValue;

            public ProteinDataBankFileRecordField() : base()
            {

            }

            public abstract override string ToString();
        }
        ";

        /// <summary>
        ///     proteinDataBankFileRecordClass
        /// </summary>
        private const string ProteinDataBankFileRecordClass = @"
        public abstract class ProteinDataBankFileRecord
        {
            public ProteinDataBankFileRecord( string columnFormatLine)
            : base()
            {

            }

            public abstract override string ToString();
        }
        ";

        /// <summary>
        ///     PDBRecordTypeMethod
        /// </summary>
        private const string PDBRecordTypeMethod = @"
        public static string ProteinDataBankFileLineRecordType( string columnFormatLine)
        {
            return (columnFormatLine != null && columnFormatLine.Length > 0) ? columnFormatLine.Substring(0, (columnFormatLine.Length >= 6) ? 6 : columnFormatLine.Length).Trim().ToUpperInvariant() : null;
        }
        ";

        /// <summary>
        ///     ToStringMethod
        /// </summary>
        private const string ToStringMethod = @"
        public override string ToString()
        {
            string result = string.Emptystring.Empty;
            for (var recordIndex = 0; recordIndex < this.Count(); recordIndex++)
            {
                ProteinDataBankFileRecord proteinDataBankFileRecord = this.NextRecord();
                result += (proteinDataBankFileRecord != null ? proteinDataBankFileRecord.ToString() : System.Environment.NewLine) + System.Environment.NewLine;
            }
            return result;
        }
        ";

        /// <summary>
        ///     NextRecordMethod
        /// </summary>
        private const string NextRecordMethod = @"
        public ProteinDataBankFileRecord NextRecord()
        {
            currentFileRecordNumber++;

            if (ProteinDataBankFileRecordList != null && currentFileRecordNumber < ProteinDataBankFileRecordList.Count)
            {
                return ProteinDataBankFileRecordList[currentFileRecordNumber];
            }
            else if (FileLinesArray != null && currentFileRecordNumber < FileLinesArray.Length)
            {
                ProteinDataBankFileRecord proteinDataBankFileRecord = ProteinDataBaseFileLineRecord( FileLinesArray[currentFileRecordNumber]);
                return proteinDataBankFileRecord;
            }
            else
            {
                UnloadFile();
                return null;
            }
        }
        ";

        /// <summary>
        ///     CountMethod
        /// </summary>
        private const string CountMethod = @"
        public int Count()
        {
            if (ProteinDataBankFileRecordList != null)
            {
                return ProteinDataBankFileRecordList.Count;
            }
            else if (FileLinesArray != null)
            {
                return FileLinesArray.Length;
            }
            else
            {
                return 0;
            }
        }
        ";

        /// <summary>
        ///     LoadFileMethod
        /// </summary>
        private const string LoadFileMethod = @"
        public void LoadFile( string filename, Boolean parseAll = true)
        {
            UnloadFile();

            if (!File.Exists(filename)) return;

            FileLinesArray = File.ReadAllLines(filename);

            if (parseAll)
            {
                ProteinDataBankFileRecordList = new List<ProteinDataBankFileRecord>();

                for (var line = 0; line < FileLinesArray.Length; line++)
                {
                    ProteinDataBankFileRecord proteinDataBankFileRecord = ProteinDataBaseFileLineRecord( FileLinesArray[line]);
                    ProteinDataBankFileRecordList.Add(proteinDataBankFileRecord);
                }
                FileLinesArray = null;
            }

        }
        ";

        /// <summary>
        ///     UnloadFileMethod
        /// </summary>
        private const string UnloadFileMethod = @"
        public void UnloadFile()
        {
            FileLinesArray = null;
            ProteinDataBankFileRecordList = null;
            currentFileRecordNumber = -1;
        }
        ";

        /// <summary>
        ///     ProteinDataBaseFileLineRecordMethod
        /// </summary>
        private const string ProteinDataBaseFileLineRecordMethod = @"
        public ProteinDataBankFileRecord ProteinDataBaseFileLineRecord( string columnFormatLine)
        {

            ProteinDataBankFileRecord result = null;
            string recordType = ProteinDataBankFileLineRecordType( columnFormatLine);

            switch (recordType)
            {
                %case statements%
            }
            return result;
        }
        ";

        /// <summary>
        ///     AreQuotesOpenAt
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool AreQuotesOpenAt(string str, int index)
        {
            bool result = false;

            if (index < str.Length)
            {
                for (int i = 0; i <= index; i++)
                {
                    if (str[i] == '"' && (i <= 0 || str[i - 1] != '\\'))
                    {
                        result = !result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Generate
        /// </summary>
        /// <param name="fileFormatSpecificationFilename"></param>
        /// <returns></returns>
        public static string Generate(string fileFormatSpecificationFilename)
        {
            var recordFormatList = new List<RecordFormat>();

            string[] linesArrayNotTrimmed = File.ReadAllLines(fileFormatSpecificationFilename);

            string section = string.Empty;
            bool parseRecordFormat = false;

            string tableHeader = string.Empty;
            string className = string.Empty;
            string finalClassName = string.Empty;

            int lineNumber = -1;

            foreach (string columnFormatLine in linesArrayNotTrimmed)
            {
                lineNumber++;
                string lineTrimmed = columnFormatLine.Trim();
                string[] split = columnFormatLine.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length <= 0)
                {
                    continue;
                }

                if (split[split.Length - 1] == "Section")
                {
                    section = columnFormatLine;
                }

                if (lineTrimmed == "Record Format")
                {
                    parseRecordFormat = false;
                }
                else if (lineTrimmed == "Details")
                {
                    parseRecordFormat = false;
                }
                else if (lineTrimmed == "Example")
                {
                    parseRecordFormat = false;
                }
                else if (lineTrimmed == "Verification/Validation/Value Authority Control")
                {
                    parseRecordFormat = false;
                }
                else if (lineTrimmed == "Relationships to Other Record Types")
                {
                    parseRecordFormat = false;
                }

                if (columnFormatLine.Replace(" ", string.Empty) == "COLUMNS" + "DataType" + "FIELD" + "DEFINITION")
                {
                    tableHeader = linesArrayNotTrimmed[lineNumber];
                    parseRecordFormat = true;

                    if (className != string.Empty && finalClassName != string.Empty)
                    {
                        // Change className to finalClassName.
                        foreach (RecordFormat recordFormat2 in recordFormatList)
                        {
                            if (className != null && recordFormat2.ClassName == className && className.Length > 0)
                            {
                                recordFormat2.ClassName = finalClassName;
                            }

                            if (className != null && recordFormat2.FieldValue == className && className.Length > 0)
                            {
                                recordFormat2.FieldValue = finalClassName;
                            }
                        }

                        className = string.Empty;
                        finalClassName = string.Empty;
                    }
                }

                if (parseRecordFormat)
                {
                    // Find if the first index of a non-space is a definition.
                    int nonspace;
                    for (nonspace = 0; nonspace < linesArrayNotTrimmed[lineNumber].Length; nonspace++)
                    {
                        if (linesArrayNotTrimmed[lineNumber][nonspace] != ' ')
                        {
                            break;
                        }
                    }

                    if (nonspace == tableHeader.IndexOf("DEFINITION"))
                    {
                        RecordFormat definitionRecord = recordFormatList[recordFormatList.Count - 1];
                        definitionRecord.Definition = definitionRecord.Definition.Trim() + " " + columnFormatLine.Trim();
                    }

                    var recordFormat = new RecordFormat();

                    int fieldFirstColumn = -1;
                    int fieldLastColumn = -1;

                    fieldFirstColumn = tableHeader.IndexOf("COLUMNS") + 1;
                    fieldLastColumn = fieldFirstColumn + 2;
                    string FirstColumnString = (fieldFirstColumn > 0 && fieldLastColumn > 0 && columnFormatLine.Length >= fieldFirstColumn) ? columnFormatLine.Substring(fieldFirstColumn - 1, (columnFormatLine.Length >= fieldLastColumn ? ((fieldLastColumn - fieldFirstColumn) + 1) : columnFormatLine.Length - (fieldFirstColumn - 1))).Trim() : string.Empty;

                    fieldFirstColumn = fieldLastColumn + 1;
                    fieldLastColumn = fieldFirstColumn;
                    string columnSplitter = (fieldFirstColumn > 0 && fieldLastColumn > 0 && columnFormatLine.Length >= fieldFirstColumn) ? columnFormatLine.Substring(fieldFirstColumn - 1, (columnFormatLine.Length >= fieldLastColumn ? ((fieldLastColumn - fieldFirstColumn) + 1) : columnFormatLine.Length - (fieldFirstColumn - 1))).Trim() : string.Empty;

                    fieldFirstColumn = fieldLastColumn + 1;
                    fieldLastColumn = fieldFirstColumn + 2;
                    string LastColumnString = (fieldFirstColumn > 0 && fieldLastColumn > 0 && columnFormatLine.Length >= fieldFirstColumn) ? columnFormatLine.Substring(fieldFirstColumn - 1, (columnFormatLine.Length >= fieldLastColumn ? ((fieldLastColumn - fieldFirstColumn) + 1) : columnFormatLine.Length - (fieldFirstColumn - 1))).Trim() : string.Empty;

                    bool columnsFound = false;

                    int x;
                    if (int.TryParse(FirstColumnString, out x))
                    {
                        recordFormat.FirstColumn = x;
                        int t;
                        if (!int.TryParse(columnSplitter, out t))
                        {
                            columnsFound = true;
                        }
                    }

                    int y;
                    if (!int.TryParse(LastColumnString, out y))
                    {
                        recordFormat.LastColumn = recordFormat.FirstColumn;
                    }
                    else
                    {
                        recordFormat.LastColumn = y;
                    }

                    if (columnsFound)
                    {
                        // first column 1..3
                        // second column 5..7
                        // data type 15..28
                        // field name 29..47
                        // definition 48..end
                        int DataTypeStart = tableHeader.IndexOf("DATA") + 1;

                        fieldFirstColumn = tableHeader.IndexOf("DATA") + 1;
                        fieldLastColumn = tableHeader.IndexOf("FIELD");
                        recordFormat.DataType = (fieldFirstColumn > 0 && fieldLastColumn > 0 && columnFormatLine.Length >= fieldFirstColumn) ? columnFormatLine.Substring(fieldFirstColumn - 1, (columnFormatLine.Length >= fieldLastColumn ? ((fieldLastColumn - fieldFirstColumn) + 1) : columnFormatLine.Length - (fieldFirstColumn - 1))).Trim() : string.Empty;

                        fieldFirstColumn = tableHeader.IndexOf("FIELD") + 1;
                        fieldLastColumn = AreQuotesOpenAt(columnFormatLine, tableHeader.IndexOf("DEFINITION") - 1) ? columnFormatLine.IndexOf('"', fieldFirstColumn) : tableHeader.IndexOf("DEFINITION");
                        recordFormat.FieldValue = (fieldFirstColumn > 0 && fieldLastColumn > 0 && columnFormatLine.Length >= fieldFirstColumn) ? columnFormatLine.Substring(fieldFirstColumn - 1, (columnFormatLine.Length >= fieldLastColumn ? ((fieldLastColumn - fieldFirstColumn) + 1) : columnFormatLine.Length - (fieldFirstColumn - 1))).Trim() : string.Empty;

                        string FieldValueNotChanged = recordFormat.FieldValue;

                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        string testname = recordFormat.FieldValue.Trim();

                        if (testname.Length > 0 && testname[0] == '"')
                        {
                            bool q = false;

                            for (int i = 0; i < testname.Length; i++)
                            {
                                if (testname[i] == '"')
                                {
                                    q = !q;
                                    finalClassName += "_";
                                }
                                else
                                {
                                    finalClassName += (string.Empty + testname[i]).Replace(" ", string.Empty).Replace("[", "_").Replace("]", "_").Replace("{", "_").Replace("}", "_").Replace(".", "_").Replace("__", "_").Replace("__", "_");
                                }
                            }

                            finalClassName = finalClassName.Trim(new[] {'_', ' ', '.', ','});
                        }

                        recordFormat.FieldValue = recordFormat.FieldValue.Replace("\"", string.Empty);
                        recordFormat.FieldValue = recordFormat.FieldValue.Replace(" ", "_");
                        recordFormat.FieldValue = recordFormat.FieldValue.Replace("__", "_");
                        recordFormat.FieldValue = recordFormat.FieldValue.Replace("__", "_");
                        recordFormat.FieldValue = recordFormat.FieldValue.Trim(new[] {'_'});

                        int t;
                        if (int.TryParse(recordFormat.FieldValue.Substring(0, 1), out t))
                        {
                            recordFormat.FieldValue = "_" + recordFormat.FieldValue;
                        }

                        fieldFirstColumn = tableHeader.IndexOf("DEFINITION") + 1;
                        fieldLastColumn = columnFormatLine.Length;
                        recordFormat.Definition = (fieldFirstColumn > 0 && fieldLastColumn > 0 && columnFormatLine.Length >= fieldFirstColumn) ? columnFormatLine.Substring(fieldFirstColumn - 1, (columnFormatLine.Length >= fieldLastColumn ? ((fieldLastColumn - fieldFirstColumn) + 1) : columnFormatLine.Length - (fieldFirstColumn - 1))).Trim() : string.Empty;
                        recordFormat.Definition = recordFormat.Definition.Replace("\"", string.Empty);

                        if (recordFormat.DataType == "Record name")
                        {
                            className = recordFormat.FieldValue;

                            int a = 1;
                            bool found = true;
                            string proposedName = string.Empty;
                            while (found)
                            {
                                found = false;
                                proposedName = className + (a > 1 ? string.Empty + a : string.Empty);

                                foreach (RecordFormat recordFormat2 in recordFormatList)
                                {
                                    if (recordFormat2.ClassName == proposedName)
                                    {
                                        a++;
                                        found = true;
                                        break;
                                    }
                                }
                            }

                            className = proposedName;
                        }

                        recordFormat.ClassName = className;

                        recordFormatList.Add(recordFormat);
                    }
                }
            }

            // Make list of classes.
            var classList = new List<string>();

            foreach (RecordFormat recordFormat in recordFormatList)
            {
                if (!classList.Contains(recordFormat.ClassName))
                {
                    classList.Add(recordFormat.ClassName);
                }
            }

            // Add additionaldata column.
            foreach (string name in classList)
            {
                int highestLastColumn = 1;
                foreach (RecordFormat recordFormat in recordFormatList)
                {
                    if (recordFormat.ClassName == name)
                    {
                        if (recordFormat.LastColumn > highestLastColumn)
                        {
                            highestLastColumn = recordFormat.LastColumn;
                        }
                    }
                }

                var additionalDataRecordFormat = new RecordFormat();
                additionalDataRecordFormat.ClassName = name;
                additionalDataRecordFormat.FieldValue = "additionalData";
                additionalDataRecordFormat.FirstColumn = highestLastColumn + 1;
                additionalDataRecordFormat.LastColumn = -1;
                additionalDataRecordFormat.Definition = "Any data found which is additional to the fields listed in the specification";

                recordFormatList.Add(additionalDataRecordFormat);
            }

            foreach (RecordFormat recordFormat in recordFormatList)
            {
                if (recordFormat.ClassName.Length > 1 && (string.Empty + recordFormat.ClassName[1]).IndexOfAny(new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'}) < 0)
                {
                    recordFormat.ClassName = recordFormat.ClassName.Trim(new[] {'_'});
                }

                if (recordFormat.FieldValue.Length > 1 && (string.Empty + recordFormat.FieldValue[1]).IndexOfAny(new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'}) < 0)
                {
                    recordFormat.FieldValue = recordFormat.FieldValue.Trim(new[] {'_'});
                }
            }

            // Ensure all field names are unique.
            foreach (string name in classList)
            {
                var FieldValueList = new List<string>();
                var fieldsToRename = new List<string>();

                foreach (RecordFormat recordFormat in recordFormatList)
                {
                    if (recordFormat.ClassName.ToLowerInvariant() == name.ToLowerInvariant())
                    {
                        if (!FieldValueList.Contains(recordFormat.FieldValue))
                        {
                            FieldValueList.Add(recordFormat.FieldValue);
                        }
                        else
                        {
                            // Add it to be renamed.
                            if (!fieldsToRename.Contains(recordFormat.FieldValue))
                            {
                                fieldsToRename.Add(recordFormat.FieldValue);
                            }
                        }
                    }
                }

                foreach (string FieldValue in fieldsToRename)
                {
                    int i = 0;
                    foreach (RecordFormat recordFormat in recordFormatList)
                    {
                        if (recordFormat.ClassName == name && recordFormat.FieldValue == FieldValue)
                        {
                            i++;
                            if (i > 1)
                            {
                                recordFormat.FieldValue = recordFormat.FieldValue + i;
                            }
                        }
                    }
                }
            }

            string mainClassCode = string.Empty;

            string cases = string.Empty;

            // Iterate through list adding each field's code.
            foreach (string name in classList)
            {
                int repeat = 1;
                if (name.Substring(name.Length - 1, 1) == "n")
                {
                    repeat = 3;
                }

                for (int n = 1; n <= repeat; n++)
                {
                    string childClassCode = string.Empty;
                    string constructor = string.Empty;
                    string tostring = string.Empty;

                    string nameWithN = (name.Substring(name.Length - 1, 1) == "n") ? name.Substring(0, name.Length - 1) + n : name;

                    tostring += "public override string ToString()" + Environment.NewLine;
                    tostring += "{" + Environment.NewLine;

                    tostring += "string columnDelimiter = \"\\t\";" + Environment.NewLine;
                    tostring += "string result = \"\";" + Environment.NewLine;

                    childClassCode += "public class " + nameWithN + "_Record : ProteinDataBankFileRecord" + Environment.NewLine;
                    childClassCode += "{" + Environment.NewLine;

                    constructor += "public " + nameWithN + "_Record( string columnFormatLine) : base( columnFormatLine)" + Environment.NewLine;
                    constructor += "{" + Environment.NewLine;

                    // Iterate through format records finding classname, adding format to class.
                    foreach (RecordFormat recordFormat in recordFormatList)
                    {
                        if (recordFormat.ClassName == nameWithN || recordFormat.ClassName == name)
                        {
                            var recordFormatCopy = new RecordFormat();
                            recordFormatCopy.ClassName = recordFormat.ClassName;
                            recordFormatCopy.FieldValue = recordFormat.FieldValue;
                            recordFormatCopy.Definition = recordFormat.Definition;
                            recordFormatCopy.DataType = recordFormat.DataType;
                            recordFormatCopy.FirstColumn = recordFormat.FirstColumn;
                            recordFormatCopy.LastColumn = recordFormat.LastColumn;
                            recordFormatCopy.FieldValue = recordFormatCopy.FieldValue.Replace("[N]", "_" + n + "_").Replace("[n]", "_" + n + "_");
                            recordFormatCopy.FieldValue = recordFormatCopy.FieldValue.Replace("[", "_").Replace("]", "_");
                            recordFormatCopy.FieldValue = recordFormatCopy.FieldValue.Replace(".", "_");

                            if (recordFormat.ClassName.ToLowerInvariant() == recordFormat.FieldValue.ToLowerInvariant())
                            {
                                recordFormatCopy.ClassName = recordFormatCopy.FieldValue;
                            }

                            if (name != nameWithN)
                            {
                                recordFormatCopy.ClassName = nameWithN;
                            }

                            if (recordFormat.ClassName.Substring(recordFormat.ClassName.Length - 1) == "n" && recordFormat.ClassName == recordFormat.FieldValue)
                            {
                                recordFormatCopy.FieldValue = recordFormatCopy.ClassName;
                            }

                            childClassCode += recordFormatCopy.ToString(1);

                            constructor += recordFormatCopy.ToString(2);

                            tostring += "result += " + recordFormatCopy.FieldValue + ".FieldValue + columnDelimiter;" + Environment.NewLine;

                            if (recordFormatCopy.ClassName == recordFormatCopy.FieldValue || recordFormatCopy.ClassName.Substring(0, recordFormatCopy.ClassName.Length - 1) == recordFormatCopy.FieldValue.Substring(0, recordFormatCopy.FieldValue.Length - 1))
                            {
                                if (recordFormatCopy.FieldValue.Substring(0, recordFormatCopy.FieldValue.Length - 1) == recordFormatCopy.FieldValue.Substring(0, recordFormatCopy.FieldValue.Length - 1).ToUpperInvariant())
                                {
                                    cases += "case \"" + recordFormatCopy.ClassName + "\":" + Environment.NewLine;
                                    cases += "result = new " + recordFormatCopy.ClassName + "_Record( columnFormatLine);" + Environment.NewLine;
                                    cases += "break;" + Environment.NewLine;
                                }
                            }
                        }
                    }

                    tostring += "return result;" + Environment.NewLine;
                    tostring += "}" + Environment.NewLine;

                    constructor += "}" + Environment.NewLine;

                    childClassCode += constructor + Environment.NewLine;
                    childClassCode += tostring + Environment.NewLine;
                    childClassCode += "}" + Environment.NewLine;

                    mainClassCode += childClassCode + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                }
            }

            mainClassCode = ProteinDataBankFileConstructors + ToStringMethod + ProteinDataBaseFileLineRecordMethod.Replace("%case statements%", cases) + PDBRecordTypeMethod + LoadFileMethod + UnloadFileMethod + CountMethod + NextRecordMethod + ProteinDataBankFileRecordFieldClass + ProteinDataBankFileRecordClass + mainClassCode;

            mainClassCode = MainNameSpaceAndClass.Replace("%data%", mainClassCode);
            while (mainClassCode.IndexOf("__") > -1)
            {
                mainClassCode = mainClassCode.Replace("__", "_");
            }

            return mainClassCode;
        }

        /// <summary>
        ///     RecordFormat
        /// </summary>
        private class RecordFormat
        {
            /// <summary>
            ///     Gets or sets ClassName
            /// </summary>
            public string ClassName { get; set; }

            /// <summary>
            ///     Gets or sets FirstColumn
            /// </summary>
            public int FirstColumn { get; set; }

            /// <summary>
            ///     Gets or sets LastColumn
            /// </summary>
            public int LastColumn { get; set; }

            /// <summary>
            ///     Gets or sets DataType
            /// </summary>
            public string DataType { get; set; }

            /// <summary>
            ///     Gets or sets FieldValue
            /// </summary>
            public string FieldValue { get; set; }

            /// <summary>
            ///     Gets or sets Definition
            /// </summary>
            public string Definition { get; set; }

            /// <summary>
            ///     ToString
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return ToString(1) + ToString(2);
            }

            /// <summary>
            ///     ToString
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public string ToString(int scope, int n = -1)
            {
                string result = string.Empty;

                string FieldValue = this.FieldValue;
                FieldValue = n > -1 && FieldValue.Substring(FieldValue.Length - 1) == "n" ? FieldValue + n : FieldValue;

                if (scope == 1)
                {
                    result += "[Description(\"" + Definition + "\"),Category(\"Data\")]" + Environment.NewLine;
                    result += "public " + this.FieldValue + "_Field " + this.FieldValue + ";" + Environment.NewLine;
                    result += Environment.NewLine;

                    result += "[Description(\"" + Definition + "\"),Category(\"Data\")]" + Environment.NewLine;
                    result += "public class " + this.FieldValue + "_Field" + " : ProteinDataBankFileRecordField" + Environment.NewLine;
                    result += "{" + Environment.NewLine;
                    result += " new public const int FirstColumn = " + FirstColumn + ";" + Environment.NewLine;
                    result += " new public const int LastColumn = " + LastColumn + ";" + Environment.NewLine;
                    result += " new public const string DataType = \"" + DataType + "\";" + Environment.NewLine;
                    result += " new public const string Definition = \"" + Definition + "\";" + Environment.NewLine;
                    result += " new public const string FieldValue = \"" + this.FieldValue + "\";" + Environment.NewLine;
                    result += " new public string FieldValue;" + Environment.NewLine;
                    result += Environment.NewLine;

                    result += " public " + this.FieldValue + "_Field" + "( string columnFormatLine) : base()" + Environment.NewLine;
                    result += " {" + Environment.NewLine;
                    result += "  FieldValue = (" + this.FieldValue + "_Field" + ".FirstColumn > 0 && " + this.FieldValue + "_Field" + ".LastColumn > 0 && columnFormatLine.Length >= " + this.FieldValue + "_Field" + ".FirstColumn) ? columnFormatLine.Substring(" + this.FieldValue + "_Field" + ".FirstColumn - 1, (columnFormatLine.Length >= " + this.FieldValue + "_Field" + ".LastColumn ? ((" + this.FieldValue + "_Field" + ".LastColumn - " + this.FieldValue + "_Field" + ".FirstColumn) + 1) : columnFormatLine.Length - (" + this.FieldValue + "_Field" + ".FirstColumn - 1))).Trim() : \"\";" + Environment.NewLine;
                    result += " }" + Environment.NewLine;
                    result += Environment.NewLine;

                    result += "public override string ToString()" + Environment.NewLine;
                    result += "{" + Environment.NewLine;
                    result += " return this.FieldValue;" + Environment.NewLine;
                    result += "}" + Environment.NewLine;
                    result += "}" + Environment.NewLine;
                    result += Environment.NewLine;
                }
                else if (scope == 2)
                {
                    result += this.FieldValue + "  = new " + this.FieldValue + "_Field ( columnFormatLine);" + Environment.NewLine;
                }

                return result;
            }
        }
    }
}