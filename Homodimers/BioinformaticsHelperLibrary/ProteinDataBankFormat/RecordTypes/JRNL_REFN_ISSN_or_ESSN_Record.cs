using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class JRNL_REFN_ISSN_or_ESSN_Record : ProteinDataBankFileRecord
    {
        [Description("International Standard Serial Number or Electronic Standard Serial Number."), Category("Data")]
        public ISSN_or_ESSN_Field ISSN_or_ESSN;
        [Description(""), Category("Data")]
        public JRNL_Field JRNL;

        [Description(""), Category("Data")]
        public REFN_Field REFN;

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("ISSN number (final digit may be a letter and may contain one or more dashes)."), Category("Data")]
        public issn_Field issn;

        public JRNL_REFN_ISSN_or_ESSN_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            JRNL = new JRNL_Field(columnFormatLine);
            REFN = new REFN_Field(columnFormatLine);
            ISSN_or_ESSN = new ISSN_or_ESSN_Field(columnFormatLine);
            issn = new issn_Field(columnFormatLine);
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
                    REFN.FieldValue,
                    ISSN_or_ESSN.FieldValue,
                    issn.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description("International Standard Serial Number or Electronic Standard Serial Number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class ISSN_or_ESSN_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 36;
            public new const int LastColumn = 39;
            public new const string DataType = "LString(4)";
            public new const string Definition = "International Standard Serial Number or Electronic Standard Serial Number.";
            public new const string FieldName = "ISSN_or_ESSN";


            public ISSN_or_ESSN_Field(string columnFormatLine)
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

        [Description(""), Category("Data")]
        [Serializable]
        public class REFN_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "";
            public new const string FieldName = "REFN";


            public REFN_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 66;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("ISSN number (final digit may be a letter and may contain one or more dashes)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class issn_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 41;
            public new const int LastColumn = 65;
            public new const string DataType = "LString";
            public new const string Definition = "ISSN number (final digit may be a letter and may contain one or more dashes).";
            public new const string FieldName = "issn";


            public issn_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
