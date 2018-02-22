using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class OBSLTE_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public OBSLTE_Field OBSLTE;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Allows concatenation of multiple records"), Category("Data")]
        public continuation_Field continuation;

        [Description("ID code of this entry."), Category("Data")]
        public idCode_Field idCode;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode_Field rIdCode;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode2_Field rIdCode2;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode3_Field rIdCode3;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode4_Field rIdCode4;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode5_Field rIdCode5;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode6_Field rIdCode6;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode7_Field rIdCode7;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode8_Field rIdCode8;

        [Description("ID code of entry that replaced this one."), Category("Data")]
        public rIdCode9_Field rIdCode9;
        [Description("Date that this entry was replaced."), Category("Data")]
        public repDate_Field repDate;

        public OBSLTE_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            OBSLTE = new OBSLTE_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            repDate = new repDate_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
            rIdCode = new rIdCode_Field(columnFormatLine);
            rIdCode2 = new rIdCode2_Field(columnFormatLine);
            rIdCode3 = new rIdCode3_Field(columnFormatLine);
            rIdCode4 = new rIdCode4_Field(columnFormatLine);
            rIdCode5 = new rIdCode5_Field(columnFormatLine);
            rIdCode6 = new rIdCode6_Field(columnFormatLine);
            rIdCode7 = new rIdCode7_Field(columnFormatLine);
            rIdCode8 = new rIdCode8_Field(columnFormatLine);
            rIdCode9 = new rIdCode9_Field(columnFormatLine);
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
                    OBSLTE.FieldValue,
                    continuation.FieldValue,
                    repDate.FieldValue,
                    idCode.FieldValue,
                    rIdCode.FieldValue,
                    rIdCode2.FieldValue,
                    rIdCode3.FieldValue,
                    rIdCode4.FieldValue,
                    rIdCode5.FieldValue,
                    rIdCode6.FieldValue,
                    rIdCode7.FieldValue,
                    rIdCode8.FieldValue,
                    rIdCode9.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class OBSLTE_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "OBSLTE";


            public OBSLTE_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 76;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Allows concatenation of multiple records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 9;
            public new const int LastColumn = 10;
            public new const string DataType = "Continuation";
            public new const string Definition = "Allows concatenation of multiple records";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of this entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 25;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of this entry.";
            public new const string FieldName = "idCode";


            public idCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 37;
            public new const int LastColumn = 40;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode2";


            public rIdCode2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 42;
            public new const int LastColumn = 45;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode3";


            public rIdCode3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 47;
            public new const int LastColumn = 50;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode4";


            public rIdCode4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode5_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 52;
            public new const int LastColumn = 55;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode5";


            public rIdCode5_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [Serializable]
        public class rIdCode6_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 60;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode6";


            public rIdCode6_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode7_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 62;
            public new const int LastColumn = 65;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode7";


            public rIdCode7_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode8_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = 70;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode8";


            public rIdCode8_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode9_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 72;
            public new const int LastColumn = 75;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode9";


            public rIdCode9_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of entry that replaced this one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class rIdCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = 35;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of entry that replaced this one.";
            public new const string FieldName = "rIdCode";


            public rIdCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Date that this entry was replaced."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class repDate_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 20;
            public new const string DataType = "Date";
            public new const string Definition = "Date that this entry was replaced.";
            public new const string FieldName = "repDate";


            public repDate_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
