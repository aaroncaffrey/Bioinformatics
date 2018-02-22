using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class REMARK_2_RESOLUTION_NOTAPPLICABLE_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public REMARK_Field REMARK;

        [Description(""), Category("Data")]
        public RESOLUTION_NOT_APPLICABLE_Field RESOLUTION_NOT_APPLICABLE_;
        [Description(""), Category("Data")]
        public _2_Field _2;

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        public REMARK_2_RESOLUTION_NOTAPPLICABLE_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            REMARK = new REMARK_Field(columnFormatLine);
            _2 = new _2_Field(columnFormatLine);
            RESOLUTION_NOT_APPLICABLE_ = new RESOLUTION_NOT_APPLICABLE_Field(columnFormatLine);
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
                    REMARK.FieldValue,
                    _2.FieldValue,
                    RESOLUTION_NOT_APPLICABLE_.FieldValue,
                    additionalData.FieldValue
                };
            return result;
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
        public class RESOLUTION_NOT_APPLICABLE_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 38;
            public new const string DataType = "LString(28)";
            public new const string Definition = "";
            public new const string FieldName = "RESOLUTION_NOT_APPLICABLE_";


            public RESOLUTION_NOT_APPLICABLE_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class _2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 10;
            public new const int LastColumn = 10;
            public new const string DataType = "LString(1)";
            public new const string Definition = "";
            public new const string FieldName = "2";


            public _2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 39;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }
    }

}
