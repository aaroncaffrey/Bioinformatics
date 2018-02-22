using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class REVDAT_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public REVDAT_Field REVDAT;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Allows concatenation of multiple records."), Category("Data")]
        public continuation_Field continuation;

        [Description("Date of modification (or release for new entries) in DD-MMM-YY format. This is not repeated on continued lines."), Category("Data")]
        public modDate_Field modDate;

        [Description("ID code of this entry. This is not repeated on continuation lines."), Category("Data")]
        public modId_Field modId;
        [Description("Modification number."), Category("Data")]
        public modNum_Field modNum;

        [Description("An integer identifying the type of modification. For all revisions, the modification type is listed as 1."), Category("Data")]
        public modType_Field modType;

        [Description("Modification detail."), Category("Data")]
        public record_Field record;

        [Description("Modification detail."), Category("Data")]
        public record2_Field record2;

        [Description("Modification detail."), Category("Data")]
        public record3_Field record3;

        [Description("Modification detail."), Category("Data")]
        public record4_Field record4;

        public REVDAT_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            REVDAT = new REVDAT_Field(columnFormatLine);
            modNum = new modNum_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            modDate = new modDate_Field(columnFormatLine);
            modId = new modId_Field(columnFormatLine);
            modType = new modType_Field(columnFormatLine);
            record = new record_Field(columnFormatLine);
            record2 = new record2_Field(columnFormatLine);
            record3 = new record3_Field(columnFormatLine);
            record4 = new record4_Field(columnFormatLine);
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
                    REVDAT.FieldValue,
                    modNum.FieldValue,
                    continuation.FieldValue,
                    modDate.FieldValue,
                    modId.FieldValue,
                    modType.FieldValue,
                    record.FieldValue,
                    record2.FieldValue,
                    record3.FieldValue,
                    record4.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REVDAT_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "REVDAT";


            public REVDAT_Field(string columnFormatLine)
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

        [Description("Allows concatenation of multiple records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 12;
            public new const string DataType = "Continuation";
            public new const string Definition = "Allows concatenation of multiple records.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Date of modification (or release for new entries) in DD-MMM-YY format. This is not repeated on continued lines."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class modDate_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 14;
            public new const int LastColumn = 22;
            public new const string DataType = "Date";
            public new const string Definition = "Date of modification (or release for new entries) in DD-MMM-YY format. This is not repeated on continued lines.";
            public new const string FieldName = "modDate";


            public modDate_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of this entry. This is not repeated on continuation lines."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class modId_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 24;
            public new const int LastColumn = 27;
            public new const string DataType = "IDCode";
            public new const string Definition = "ID code of this entry. This is not repeated on continuation lines.";
            public new const string FieldName = "modId";


            public modId_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Modification number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class modNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Modification number.";
            public new const string FieldName = "modNum";


            public modNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("An integer identifying the type of modification. For all revisions, the modification type is listed as 1."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class modType_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = 32;
            public new const string DataType = "Integer";
            public new const string Definition = "An integer identifying the type of modification. For all revisions, the modification type is listed as 1.";
            public new const string FieldName = "modType";


            public modType_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Modification detail."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class record2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 47;
            public new const int LastColumn = 52;
            public new const string DataType = "LString(6)";
            public new const string Definition = "Modification detail.";
            public new const string FieldName = "record2";


            public record2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Modification detail."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class record3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 54;
            public new const int LastColumn = 59;
            public new const string DataType = "LString(6)";
            public new const string Definition = "Modification detail.";
            public new const string FieldName = "record3";


            public record3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Modification detail."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class record4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 61;
            public new const int LastColumn = 66;
            public new const string DataType = "LString(6)";
            public new const string Definition = "Modification detail.";
            public new const string FieldName = "record4";


            public record4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Modification detail."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class record_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 40;
            public new const int LastColumn = 45;
            public new const string DataType = "LString(6)";
            public new const string Definition = "Modification detail.";
            public new const string FieldName = "record";


            public record_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
