using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class EXPDTA_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public EXPDTA_Field EXPDTA;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Allows concatenation of multiple records."), Category("Data")]
        public continuation_Field continuation;

        [Description("The experimental technique(s) with optional comment describing the sample or experiment."), Category("Data")]
        public technique_Field technique;

        public EXPDTA_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            EXPDTA = new EXPDTA_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            technique = new technique_Field(columnFormatLine);
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
                    EXPDTA.FieldValue,
                    continuation.FieldValue,
                    technique.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class EXPDTA_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "EXPDTA";


            public EXPDTA_Field(string columnFormatLine)
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

        [Description("The experimental technique(s) with optional comment describing the sample or experiment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class technique_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 79;
            public new const string DataType = "SList";
            public new const string Definition = "The experimental technique(s) with optional comment describing the sample or experiment.";
            public new const string FieldName = "technique";


            public technique_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
