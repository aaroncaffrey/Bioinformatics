using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class DBREF1_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public DBREF1_Field DBREF1;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Chain identifier."), Category("Data")]
        public chainID_Field chainID;

        [Description("Sequence database name."), Category("Data")]
        public database_Field database;

        [Description("Sequence database identification code, left justified."), Category("Data")]
        public dbIdCode_Field dbIdCode;
        [Description("ID code of this entry."), Category("Data")]
        public idCode_Field idCode;
        [Description("Initial insertion code of the PDB sequence segment."), Category("Data")]
        public insertBegin_Field insertBegin;
        [Description("Ending insertion code of the PDB sequence segment."), Category("Data")]
        public insertEnd_Field insertEnd;
        [Description("Initial sequence number of the PDB sequence segment, right justified."), Category("Data")]
        public seqBegin_Field seqBegin;
        [Description("Ending sequence number of the PDB sequence segment, right justified."), Category("Data")]
        public seqEnd_Field seqEnd;

        public DBREF1_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            DBREF1 = new DBREF1_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
            chainID = new chainID_Field(columnFormatLine);
            seqBegin = new seqBegin_Field(columnFormatLine);
            insertBegin = new insertBegin_Field(columnFormatLine);
            seqEnd = new seqEnd_Field(columnFormatLine);
            insertEnd = new insertEnd_Field(columnFormatLine);
            database = new database_Field(columnFormatLine);
            dbIdCode = new dbIdCode_Field(columnFormatLine);
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
                    DBREF1.FieldValue,
                    idCode.FieldValue,
                    chainID.FieldValue,
                    seqBegin.FieldValue,
                    insertBegin.FieldValue,
                    seqEnd.FieldValue,
                    insertEnd.FieldValue,
                    database.FieldValue,
                    dbIdCode.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class DBREF1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "DBREF1";


            public DBREF1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 68;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Chain identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 13;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier.";
            public new const string FieldName = "chainID";


            public chainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence database name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class database_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 27;
            public new const int LastColumn = 32;
            public new const string DataType = "LString";
            public new const string Definition = "Sequence database name.";
            public new const string FieldName = "database";


            public database_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence database identification code, left justified."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbIdCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 48;
            public new const int LastColumn = 67;
            public new const string DataType = "LString";
            public new const string Definition = "Sequence database identification code, left justified.";
            public new const string FieldName = "dbIdCode";


            public dbIdCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ID code of this entry."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 11;
            public new const string DataType = "IDcode";
            public new const string Definition = "ID code of this entry.";
            public new const string FieldName = "idCode";


            public idCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Initial insertion code of the PDB sequence segment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class insertBegin_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 19;
            public new const int LastColumn = 19;
            public new const string DataType = "AChar";
            public new const string Definition = "Initial insertion code of the PDB sequence segment.";
            public new const string FieldName = "insertBegin";


            public insertBegin_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Ending insertion code of the PDB sequence segment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class insertEnd_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 25;
            public new const int LastColumn = 25;
            public new const string DataType = "AChar";
            public new const string Definition = "Ending insertion code of the PDB sequence segment.";
            public new const string FieldName = "insertEnd";


            public insertEnd_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Initial sequence number of the PDB sequence segment, right justified."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqBegin_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 15;
            public new const int LastColumn = 18;
            public new const string DataType = "Integer";
            public new const string Definition = "Initial sequence number of the PDB sequence segment, right justified.";
            public new const string FieldName = "seqBegin";


            public seqBegin_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Ending sequence number of the PDB sequence segment, right justified."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqEnd_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 21;
            public new const int LastColumn = 24;
            public new const string DataType = "Integer";
            public new const string Definition = "Ending sequence number of the PDB sequence segment, right justified.";
            public new const string FieldName = "seqEnd";


            public seqEnd_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
