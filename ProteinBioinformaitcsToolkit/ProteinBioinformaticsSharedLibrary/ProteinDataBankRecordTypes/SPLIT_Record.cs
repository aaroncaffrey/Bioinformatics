using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    [Serializable]
    public class SPLIT_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public SPLIT_Field SPLIT;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Allows concatenation of multiple records."), Category("Data")]
        public continuation_Field continuation;

        [Description("ID code of related entry."), Category("Data")]
        public idCode_Field idCode;
        [Description("ID code of related entry."), Category("Data")]
        public idCode10_Field idCode10;
        [Description("ID code of related entry."), Category("Data")]
        public idCode11_Field idCode11;
        [Description("ID code of related entry."), Category("Data")]
        public idCode12_Field idCode12;
        [Description("ID code of related entry."), Category("Data")]
        public idCode13_Field idCode13;
        [Description("ID code of related entry."), Category("Data")]
        public idCode14_Field idCode14;

        [Description("ID code of related entry."), Category("Data")]
        public idCode2_Field idCode2;

        [Description("ID code of related entry."), Category("Data")]
        public idCode3_Field idCode3;

        [Description("ID code of related entry."), Category("Data")]
        public idCode4_Field idCode4;

        [Description("ID code of related entry."), Category("Data")]
        public idCode5_Field idCode5;

        [Description("ID code of related entry."), Category("Data")]
        public idCode6_Field idCode6;

        [Description("ID code of related entry."), Category("Data")]
        public idCode7_Field idCode7;

        [Description("ID code of related entry."), Category("Data")]
        public idCode8_Field idCode8;

        [Description("ID code of related entry."), Category("Data")]
        public idCode9_Field idCode9;

        public SPLIT_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SPLIT = new SPLIT_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
            idCode2 = new idCode2_Field(columnFormatLine);
            idCode3 = new idCode3_Field(columnFormatLine);
            idCode4 = new idCode4_Field(columnFormatLine);
            idCode5 = new idCode5_Field(columnFormatLine);
            idCode6 = new idCode6_Field(columnFormatLine);
            idCode7 = new idCode7_Field(columnFormatLine);
            idCode8 = new idCode8_Field(columnFormatLine);
            idCode9 = new idCode9_Field(columnFormatLine);
            idCode10 = new idCode10_Field(columnFormatLine);
            idCode11 = new idCode11_Field(columnFormatLine);
            idCode12 = new idCode12_Field(columnFormatLine);
            idCode13 = new idCode13_Field(columnFormatLine);
            idCode14 = new idCode14_Field(columnFormatLine);
            additionalData = new additionalData_Field(columnFormatLine);
        }

        public override string ToString()
        {
            return string.Join("\t", ToArray());
        }

        public override string[] ToArray()
        {
            var result = new[]
                {
                    SPLIT.FieldValue,
                    continuation.FieldValue,
                    idCode.FieldValue,
                    idCode2.FieldValue,
                    idCode3.FieldValue,
                    idCode4.FieldValue,
                    idCode5.FieldValue,
                    idCode6.FieldValue,
                    idCode7.FieldValue,
                    idCode8.FieldValue,
                    idCode9.FieldValue,
                    idCode10.FieldValue,
                    idCode11.FieldValue,
                    idCode12.FieldValue,
                    idCode13.FieldValue,
                    idCode14.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SPLIT_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "SPLIT";


            public SPLIT_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 81;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Allows concatenation of multiple records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 9;
            public new const int LastColumn = 10;
            public new const string DataType = "Continuation";
            public new const string Definition = "Allows concatenation of multiple records.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode10_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 60;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode10";


            public idCode10_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode11_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 62;
            public new const int LastColumn = 65;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode11";


            public idCode11_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode12_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = 70;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode12";


            public idCode12_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode13_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 72;
            public new const int LastColumn = 75;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode13";


            public idCode13_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode14_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 77;
            public new const int LastColumn = 80;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode14";


            public idCode14_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 20;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode2";


            public idCode2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 25;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode3";


            public idCode3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 27;
            public new const int LastColumn = 30;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode4";


            public idCode4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode5_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = 35;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode5";


            public idCode5_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode6_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 37;
            public new const int LastColumn = 40;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode6";


            public idCode6_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode7_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 42;
            public new const int LastColumn = 45;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode7";


            public idCode7_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode8_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 47;
            public new const int LastColumn = 50;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode8";


            public idCode8_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode9_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 52;
            public new const int LastColumn = 55;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode9";


            public idCode9_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of related entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 15;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of related entry.";
            public new const string FieldName = "idCode";


            public idCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
