using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class FORMUL_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public FORMUL_Field FORMUL;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("* for water."), Category("Data")]
        public asterisk_Field asterisk;

        [Description("Component number."), Category("Data")]
        public compNum_Field compNum;

        [Description("Continuation number."), Category("Data")]
        public continuation_Field continuation;
        [Description("Het identifier."), Category("Data")]
        public hetID_Field hetID;

        [Description("Chemical formula."), Category("Data")]
        public text_Field text;

        public FORMUL_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            FORMUL = new FORMUL_Field(columnFormatLine);
            compNum = new compNum_Field(columnFormatLine);
            hetID = new hetID_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            asterisk = new asterisk_Field(columnFormatLine);
            text = new text_Field(columnFormatLine);
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
                    FORMUL.FieldValue,
                    compNum.FieldValue,
                    hetID.FieldValue,
                    continuation.FieldValue,
                    asterisk.FieldValue,
                    text.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class FORMUL_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "FORMUL";


            public FORMUL_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 71;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("* for water."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class asterisk_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 19;
            public new const int LastColumn = 19;
            public new const string DataType = "Character";
            public new const string Definition = "* for water.";
            public new const string FieldName = "asterisk";


            public asterisk_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Component number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class compNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 9;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Component number.";
            public new const string FieldName = "compNum";


            public compNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Continuation number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Integer";
            public new const string Definition = "Continuation number.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Het identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class hetID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 15;
            public new const string DataType = "LString(3)";
            public new const string Definition = "Het identifier.";
            public new const string FieldName = "hetID";


            public hetID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chemical formula."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class text_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 70;
            public new const string DataType = "string";
            public new const string Definition = "Chemical formula.";
            public new const string FieldName = "text";


            public text_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
