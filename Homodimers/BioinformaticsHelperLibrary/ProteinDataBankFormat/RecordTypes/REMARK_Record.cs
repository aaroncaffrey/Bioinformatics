using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{
    [Serializable]
    public class REMARK_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public REMARK_Field REMARK;

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("Left as white space in first line of each new remark."), Category("Data")]
        public empty_Field empty;
        [Description("Remark number. It is not an error for remark n to exist in an entry when remark n-1 does not."), Category("Data")]
        public remarkNum_Field remarkNum;

        public REMARK_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            REMARK = new REMARK_Field(columnFormatLine);
            remarkNum = new remarkNum_Field(columnFormatLine);
            empty = new empty_Field(columnFormatLine);
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
                    remarkNum.FieldValue,
                    empty.FieldValue,
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

        [Description("Left as white space in first line of each new remark."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class empty_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 79;
            public new const string DataType = "LString";
            public new const string Definition = "Left as white space in first line of each new remark.";
            public new const string FieldName = "empty";


            public empty_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Remark number. It is not an error for remark n to exist in an entry when remark n-1 does not."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class remarkNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Remark number. It is not an error for remark n to exist in an entry when remark n-1 does not.";
            public new const string FieldName = "remarkNum";


            public remarkNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
