using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class SCALE3_Record : ProteinDataBankFileRecord
    {
        [Description("n=1, 2, or 3"), Category("Data")]
        public SCALE3_Field SCALE3;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Sn1"), Category("Data")]
        public s_3_1_Field s_3_1_;

        [Description("Sn2"), Category("Data")]
        public s_3_2_Field s_3_2_;

        [Description("Sn3"), Category("Data")]
        public s_3_3_Field s_3_3_;

        [Description("Un"), Category("Data")]
        public u_3_Field u_3_;

        public SCALE3_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SCALE3 = new SCALE3_Field(columnFormatLine);
            s_3_1_ = new s_3_1_Field(columnFormatLine);
            s_3_2_ = new s_3_2_Field(columnFormatLine);
            s_3_3_ = new s_3_3_Field(columnFormatLine);
            u_3_ = new u_3_Field(columnFormatLine);
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
                    SCALE3.FieldValue,
                    s_3_1_.FieldValue,
                    s_3_2_.FieldValue,
                    s_3_3_.FieldValue,
                    u_3_.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description("n=1, 2, or 3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SCALE3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "n=1, 2, or 3";
            public new const string FieldName = "SCALE3";


            public SCALE3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 56;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Sn1"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class s_3_1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 20;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Sn1";
            public new const string FieldName = "s_3_1_";


            public s_3_1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sn2"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class s_3_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 21;
            public new const int LastColumn = 30;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Sn2";
            public new const string FieldName = "s_3_2_";


            public s_3_2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sn3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class s_3_3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 31;
            public new const int LastColumn = 40;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Sn3";
            public new const string FieldName = "s_3_3_";


            public s_3_3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Un"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class u_3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 46;
            public new const int LastColumn = 55;
            public new const string DataType = "Real(10.5)";
            public new const string Definition = "Un";
            public new const string FieldName = "u_3_";


            public u_3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
