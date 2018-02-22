using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class MTRIX1_Record : ProteinDataBankFileRecord
    {
        [Description("n=1, 2, or 3"), Category("Data")]
        public MTRIX1_Field MTRIX1;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("1 if coordinates for the representations which are approximately related by the transformations of the molecule are contained in the entry. Otherwise, blank."), Category("Data")]
        public iGiven_Field iGiven;

        [Description("Mn1"), Category("Data")]
        public m_1_1_Field m_1_1_;

        [Description("Mn2"), Category("Data")]
        public m_1_2_Field m_1_2_;

        [Description("Mn3"), Category("Data")]
        public m_1_3_Field m_1_3_;
        [Description("Serial number."), Category("Data")]
        public serial_Field serial;

        [Description("Vn"), Category("Data")]
        public v_1_Field v_1_;

        public MTRIX1_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            MTRIX1 = new MTRIX1_Field(columnFormatLine);
            serial = new serial_Field(columnFormatLine);
            m_1_1_ = new m_1_1_Field(columnFormatLine);
            m_1_2_ = new m_1_2_Field(columnFormatLine);
            m_1_3_ = new m_1_3_Field(columnFormatLine);
            v_1_ = new v_1_Field(columnFormatLine);
            iGiven = new iGiven_Field(columnFormatLine);
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
                    MTRIX1.FieldValue,
                    serial.FieldValue,
                    m_1_1_.FieldValue,
                    m_1_2_.FieldValue,
                    m_1_3_.FieldValue,
                    v_1_.FieldValue,
                    iGiven.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description("n=1, 2, or 3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class MTRIX1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "n=1, 2, or 3";
            public new const string FieldName = "MTRIX1";


            public MTRIX1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 61;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("1 if coordinates for the representations which are approximately related by the transformations of the molecule are contained in the entry. Otherwise, blank."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iGiven_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 60;
            public new const int LastColumn = 60;
            public new const string DataType = "Integer";
            public new const string Definition = "1 if coordinates for the representations which are approximately related by the transformations of the molecule are contained in the entry. Otherwise, blank.";
            public new const string FieldName = "iGiven";


            public iGiven_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Mn1"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class m_1_1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 20;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Mn1";
            public new const string FieldName = "m_1_1_";


            public m_1_1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Mn2"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class m_1_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 21;
            public new const int LastColumn = 30;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Mn2";
            public new const string FieldName = "m_1_2_";


            public m_1_2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Mn3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class m_1_3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 31;
            public new const int LastColumn = 40;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Mn3";
            public new const string FieldName = "m_1_3_";


            public m_1_3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Serial number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serial_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number.";
            public new const string FieldName = "serial";


            public serial_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Vn"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class v_1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 46;
            public new const int LastColumn = 55;
            public new const string DataType = "Real(10.5)";
            public new const string Definition = "Vn";
            public new const string FieldName = "v_1_";


            public v_1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
