using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    [Serializable]
    public class SPRSDE_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public SPRSDE_Field SPRSDE;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Allows for multiple ID codes."), Category("Data")]
        public continuation_Field continuation;

        [Description("ID code of this entry. This field is not copied on continuations."), Category("Data")]
        public idCode_Field idCode;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode_Field sIdCode;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode2_Field sIdCode2;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode3_Field sIdCode3;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode4_Field sIdCode4;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode5_Field sIdCode5;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode6_Field sIdCode6;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode7_Field sIdCode7;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode8_Field sIdCode8;

        [Description("ID code of a superseded entry."), Category("Data")]
        public sIdCode9_Field sIdCode9;
        [Description("Date this entry superseded the listed entries. This field is not copied on continuations."), Category("Data")]
        public sprsdeDate_Field sprsdeDate;

        public SPRSDE_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SPRSDE = new SPRSDE_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            sprsdeDate = new sprsdeDate_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
            sIdCode = new sIdCode_Field(columnFormatLine);
            sIdCode2 = new sIdCode2_Field(columnFormatLine);
            sIdCode3 = new sIdCode3_Field(columnFormatLine);
            sIdCode4 = new sIdCode4_Field(columnFormatLine);
            sIdCode5 = new sIdCode5_Field(columnFormatLine);
            sIdCode6 = new sIdCode6_Field(columnFormatLine);
            sIdCode7 = new sIdCode7_Field(columnFormatLine);
            sIdCode8 = new sIdCode8_Field(columnFormatLine);
            sIdCode9 = new sIdCode9_Field(columnFormatLine);
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
                    SPRSDE.FieldValue,
                    continuation.FieldValue,
                    sprsdeDate.FieldValue,
                    idCode.FieldValue,
                    sIdCode.FieldValue,
                    sIdCode2.FieldValue,
                    sIdCode3.FieldValue,
                    sIdCode4.FieldValue,
                    sIdCode5.FieldValue,
                    sIdCode6.FieldValue,
                    sIdCode7.FieldValue,
                    sIdCode8.FieldValue,
                    sIdCode9.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SPRSDE_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "SPRSDE";


            public SPRSDE_Field(string columnFormatLine)
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

        [Description("Allows for multiple ID codes."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 9;
            public new const int LastColumn = 10;
            public new const string DataType = "Continuation";
            public new const string Definition = "Allows for multiple ID codes.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of this entry. This field is not copied on continuations."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 25;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of this entry. This field is not copied on continuations.";
            public new const string FieldName = "idCode";


            public idCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 37;
            public new const int LastColumn = 40;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode2";


            public sIdCode2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 42;
            public new const int LastColumn = 45;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode3";


            public sIdCode3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 47;
            public new const int LastColumn = 50;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode4";


            public sIdCode4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode5_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 52;
            public new const int LastColumn = 55;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode5";


            public sIdCode5_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode6_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 60;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode6";


            public sIdCode6_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode7_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 62;
            public new const int LastColumn = 65;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode7";


            public sIdCode7_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode8_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = 70;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode8";


            public sIdCode8_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode9_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 72;
            public new const int LastColumn = 75;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode9";


            public sIdCode9_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of a superseded entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sIdCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = 35;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of a superseded entry.";
            public new const string FieldName = "sIdCode";


            public sIdCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Date this entry superseded the listed entries. This field is not copied on continuations."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sprsdeDate_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 20;
            public new const string DataType = "Date";
            public new const string Definition = "Date this entry superseded the listed entries. This field is not copied on continuations.";
            public new const string FieldName = "sprsdeDate";


            public sprsdeDate_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
