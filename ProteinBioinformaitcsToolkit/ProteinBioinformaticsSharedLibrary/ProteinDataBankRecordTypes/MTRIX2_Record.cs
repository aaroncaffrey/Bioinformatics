using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class MTRIX2_Record : ProteinDataBankFileRecord
    {
        [Description("n=1, 2, or 3"), Category("Data")]
        public MTRIX2_Field MTRIX2;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("1 if coordinates for the representations which are approximately related by the transformations of the molecule are contained in the entry. Otherwise, blank."), Category("Data")]
        public iGiven_Field iGiven;

        [Description("Mn1"), Category("Data")]
        public m_2_1_Field m_2_1_;

        [Description("Mn2"), Category("Data")]
        public m_2_2_Field m_2_2_;

        [Description("Mn3"), Category("Data")]
        public m_2_3_Field m_2_3_;
        [Description("Serial number."), Category("Data")]
        public serial_Field serial;

        [Description("Vn"), Category("Data")]
        public v_2_Field v_2_;

        public MTRIX2_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            MTRIX2 = new MTRIX2_Field(columnFormatLine);
            serial = new serial_Field(columnFormatLine);
            m_2_1_ = new m_2_1_Field(columnFormatLine);
            m_2_2_ = new m_2_2_Field(columnFormatLine);
            m_2_3_ = new m_2_3_Field(columnFormatLine);
            v_2_ = new v_2_Field(columnFormatLine);
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
                    MTRIX2.FieldValue,
                    serial.FieldValue,
                    m_2_1_.FieldValue,
                    m_2_2_.FieldValue,
                    m_2_3_.FieldValue,
                    v_2_.FieldValue,
                    iGiven.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description("n=1, 2, or 3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class MTRIX2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "n=1, 2, or 3";
            public new const string FieldName = "MTRIX2";


            public MTRIX2_Field(string columnFormatLine)
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
        public class m_2_1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 20;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Mn1";
            public new const string FieldName = "m_2_1_";


            public m_2_1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Mn2"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class m_2_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 21;
            public new const int LastColumn = 30;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Mn2";
            public new const string FieldName = "m_2_2_";


            public m_2_2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Mn3"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class m_2_3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 31;
            public new const int LastColumn = 40;
            public new const string DataType = "Real(10.6)";
            public new const string Definition = "Mn3";
            public new const string FieldName = "m_2_3_";


            public m_2_3_Field(string columnFormatLine)
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
        public class v_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 46;
            public new const int LastColumn = 55;
            public new const string DataType = "Real(10.5)";
            public new const string Definition = "Vn";
            public new const string FieldName = "v_2_";


            public v_2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
