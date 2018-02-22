using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class REMARK_1_AUTH_Record : ProteinDataBankFileRecord
    {
        [Description("Appears on all continuation records."), Category("Data")]
        public AUTH_Field AUTH;
        [Description("Appears on all continuation records."), Category("Data")]
        public AUTH2_Field AUTH2;

        [Description(""), Category("Data")]
        public REMARK_Field REMARK;
        [Description(""), Category("Data")]
        public REMARK_1_AUTH_Field REMARK_1_AUTH;
        [Description(""), Category("Data")]
        public _1_Field _1;

        [Description(""), Category("Data")]
        public _12_Field _12;

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("List of the authors."), Category("Data")]
        public authorList_Field authorList;
        [Description("List of the authors."), Category("Data")]
        public authorList2_Field authorList2;
        [Description("Allows a long list of authors."), Category("Data")]
        public continuation_Field continuation;
        [Description("Allows a long list of authors."), Category("Data")]
        public continuation2_Field continuation2;

        public REMARK_1_AUTH_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            REMARK_1_AUTH = new REMARK_1_AUTH_Field(columnFormatLine);
            _1 = new _1_Field(columnFormatLine);
            AUTH = new AUTH_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            authorList = new authorList_Field(columnFormatLine);
            REMARK = new REMARK_Field(columnFormatLine);
            _12 = new _12_Field(columnFormatLine);
            AUTH2 = new AUTH2_Field(columnFormatLine);
            continuation2 = new continuation2_Field(columnFormatLine);
            authorList2 = new authorList2_Field(columnFormatLine);
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
                    REMARK_1_AUTH.FieldValue,
                    _1.FieldValue,
                    AUTH.FieldValue,
                    continuation.FieldValue,
                    authorList.FieldValue,
                    REMARK.FieldValue,
                    _12.FieldValue,
                    AUTH2.FieldValue,
                    continuation2.FieldValue,
                    authorList2.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description("Appears on all continuation records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class AUTH2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "Appears on all continuation records.";
            public new const string FieldName = "AUTH2";

            public AUTH2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }


        }

        [Description("Appears on all continuation records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class AUTH_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "Appears on all continuation records.";
            public new const string FieldName = "AUTH";


            public AUTH_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REMARK_1_AUTH_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "REMARK_1_AUTH";


            public REMARK_1_AUTH_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REMARK_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "REMARK";


            public REMARK_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class _12_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 10;
            public new const int LastColumn = 10;
            public new const string DataType = "LString(1)";
            public new const string Definition = "";
            public new const string FieldName = "_12";


            public _12_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class _1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 10;
            public new const int LastColumn = 10;
            public new const string DataType = "LString(1)";
            public new const string Definition = "";
            public new const string FieldName = "1";


            public _1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 80;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("List of the authors."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class authorList2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "List";
            public new const string Definition = "List of the authors.";
            public new const string FieldName = "authorList2";


            public authorList2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("List of the authors."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class authorList_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "List";
            public new const string Definition = "List of the authors.";
            public new const string FieldName = "authorList";


            public authorList_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Allows a long list of authors."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Continuation";
            public new const string Definition = "Allows a long list of authors.";
            public new const string FieldName = "continuation2";


            public continuation2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Allows a long list of authors."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Continuation";
            public new const string Definition = "Allows a long list of authors.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
