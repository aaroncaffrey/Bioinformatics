using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{
    [Serializable]
    public class ORIGX3_Record : ProteinDataBankFileRecord
    {
        [Description("n=1, 2, or 3"), Category("Data")]
        public ORIGX3_Field ORIGX3;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("On1"), Category("Data")]
        public o_3_1_Field o_3_1_;

        [Description("On2"), Category("Data")]
        public o_3_2_Field o_3_2_;

        [Description("On3"), Category("Data")]
        public o_3_3_Field o_3_3_;

        [Description("Tn"), Category("Data")]
        public t_3_Field t_3_;

        public ORIGX3_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            ORIGX3 = new ORIGX3_Field(columnFormatLine);
            o_3_1_ = new o_3_1_Field(columnFormatLine);
            o_3_2_ = new o_3_2_Field(columnFormatLine);
            o_3_3_ = new o_3_3_Field(columnFormatLine);
            t_3_ = new t_3_Field(columnFormatLine);
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
                    ORIGX3.FieldValue,
                    o_3_1_.FieldValue,
                    o_3_2_.FieldValue,
                    o_3_3_.FieldValue,
                    t_3_.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description("n=1, 2, or 3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class ORIGX3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "n=1, 2, or 3";
            public new const string FieldName = "ORIGX3";


            public ORIGX3_Field(string columnFormatLine)
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

        [Description("On1"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class o_3_1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 20;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "On1";
            public new const string FieldName = "o_3_1_";


            public o_3_1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("On2"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class o_3_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 21;
            public new const int LastColumn = 30;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "On2";
            public new const string FieldName = "o_3_2_";


            public o_3_2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("On3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class o_3_3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 31;
            public new const int LastColumn = 40;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "On3";
            public new const string FieldName = "o_3_3_";


            public o_3_3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Tn"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class t_3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 46;
            public new const int LastColumn = 55;
            public new const string DataType = "Real(10.5)";
            public new const string Definition = "Tn";
            public new const string FieldName = "t_3_";


            public t_3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
