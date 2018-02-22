using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class JRNL_REF_V_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public JRNL_Field JRNL;

        [Description(""), Category("Data")]
        public REF_Field REF;
        [Description("Appears in the first sub-record only, and only if column 55 is non-blank."), Category("Data")]
        public V_Field V_;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Allows long publication names."), Category("Data")]
        public continuation_Field continuation;
        [Description("First page of the article; appears in the first sub-record only."), Category("Data")]
        public page_Field page;

        [Description("Name of the publication including section or series designation. This is the only field of this sub-record which may be continued on successive sub-records."), Category("Data")]
        public pubName_Field pubName;

        [Description("Right-justified blank-filled volume information; appears in the first sub-record only."), Category("Data")]
        public volume_Field volume;

        [Description("Year of publication; first sub-record only."), Category("Data")]
        public year_Field year;

        public JRNL_REF_V_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            JRNL = new JRNL_Field(columnFormatLine);
            REF = new REF_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            pubName = new pubName_Field(columnFormatLine);
            V_ = new V_Field(columnFormatLine);
            volume = new volume_Field(columnFormatLine);
            page = new page_Field(columnFormatLine);
            year = new year_Field(columnFormatLine);
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
                    JRNL.FieldValue,
                    REF.FieldValue,
                    continuation.FieldValue,
                    pubName.FieldValue,
                    V_.FieldValue,
                    volume.FieldValue,
                    page.FieldValue,
                    year.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class JRNL_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "JRNL";


            public JRNL_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REF_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(3)";
            public new const string Definition = "";
            public new const string FieldName = "REF";


            public REF_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Appears in the first sub-record only, and only if column 55 is non-blank."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class V_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 50;
            public new const int LastColumn = 51;
            public new const string DataType = "LString(2)";
            public new const string Definition = "Appears in the first sub-record only, and only if column 55 is non-blank.";
            public new const string FieldName = "V_";


            public V_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Allows long publication names."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Continuation";
            public new const string Definition = "Allows long publication names.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("First page of the article; appears in the first sub-record only."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class page_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 61;
            public new const string DataType = "string";
            public new const string Definition = "First page of the article; appears in the first sub-record only.";
            public new const string FieldName = "page";


            public page_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Name of the publication including section or series designation. This is the only field of this sub-record which may be continued on successive sub-records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class pubName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 47;
            public new const string DataType = "LString";
            public new const string Definition = "Name of the publication including section or series designation. This is the only field of this sub-record which may be continued on successive sub-records.";
            public new const string FieldName = "pubName";


            public pubName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Right-justified blank-filled volume information; appears in the first sub-record only."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class volume_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 52;
            public new const int LastColumn = 55;
            public new const string DataType = "string";
            public new const string Definition = "Right-justified blank-filled volume information; appears in the first sub-record only.";
            public new const string FieldName = "volume";


            public volume_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Year of publication; first sub-record only."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class year_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 63;
            public new const int LastColumn = 66;
            public new const string DataType = "Integer";
            public new const string Definition = "Year of publication; first sub-record only.";
            public new const string FieldName = "year";


            public year_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
