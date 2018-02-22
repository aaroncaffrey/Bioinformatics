using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class JRNL_DOI_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public DOI_Field DOI;

        [Description(""), Category("Data")]
        public DOI2_Field DOI2;
        [Description(""), Category("Data")]
        public JRNL_Field JRNL;
        [Description(""), Category("Data")]
        public JRNL2_Field JRNL2;

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("Unique DOI assigned to the publication describing the experiment. Allows for a long DOI string."), Category("Data")]
        public continuation_Field continuation;
        [Description("Unique DOI assigned to the Publication describing the experiment. Allows for a long DOI string."), Category("Data")]
        public continuation2_Field continuation2;

        public JRNL_DOI_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            JRNL = new JRNL_Field(columnFormatLine);
            DOI = new DOI_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            JRNL2 = new JRNL2_Field(columnFormatLine);
            DOI2 = new DOI2_Field(columnFormatLine);
            continuation2 = new continuation2_Field(columnFormatLine);
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
                    JRNL.FieldValue,
                    DOI.FieldValue,
                    continuation.FieldValue,
                    JRNL2.FieldValue,
                    DOI2.FieldValue,
                    continuation2.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class DOI2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "";
            public new const string FieldName = "DOI2";


            public DOI2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class DOI_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "";
            public new const string FieldName = "DOI";


            public DOI_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class JRNL2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "JRNL2";


            public JRNL2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class JRNL_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "JRNL";


            public JRNL_Field(string columnFormatLine)
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

        [Description("Unique DOI assigned to the Publication describing the experiment. Allows for a long DOI string."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "LString";
            public new const string Definition = "Unique DOI assigned to the Publication describing the experiment. Allows for a long DOI string.";
            public new const string FieldName = "continuation2";


            public continuation2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Unique DOI assigned to the publication describing the experiment. Allows for a long DOI string."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "LString";
            public new const string Definition = "Unique DOI assigned to the publication describing the experiment. Allows for a long DOI string.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
