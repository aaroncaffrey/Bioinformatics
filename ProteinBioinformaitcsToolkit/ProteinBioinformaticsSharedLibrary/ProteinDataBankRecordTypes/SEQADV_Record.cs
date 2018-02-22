using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    [Serializable]
    public class SEQADV_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public SEQADV_Field SEQADV;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("PDB chain identifier."), Category("Data")]
        public chainID_Field chainID;
        [Description("Conflict comment."), Category("Data")]
        public conflict_Field conflict;

        [Description(""), Category("Data")]
        public database_Field database;

        [Description("Sequence database accession number."), Category("Data")]
        public dbAccession_Field dbAccession;

        [Description("Sequence database residue name."), Category("Data")]
        public dbRes_Field dbRes;

        [Description("Sequence database sequence number."), Category("Data")]
        public dbSeq_Field dbSeq;
        [Description("PDB insertion code."), Category("Data")]
        public iCode_Field iCode;
        [Description("ID code of this entry."), Category("Data")]
        public idCode_Field idCode;
        [Description("Name of the PDB residue in conflict."), Category("Data")]
        public resName_Field resName;
        [Description("PDB sequence number."), Category("Data")]
        public seqNum_Field seqNum;

        public SEQADV_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SEQADV = new SEQADV_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
            resName = new resName_Field(columnFormatLine);
            chainID = new chainID_Field(columnFormatLine);
            seqNum = new seqNum_Field(columnFormatLine);
            iCode = new iCode_Field(columnFormatLine);
            database = new database_Field(columnFormatLine);
            dbAccession = new dbAccession_Field(columnFormatLine);
            dbRes = new dbRes_Field(columnFormatLine);
            dbSeq = new dbSeq_Field(columnFormatLine);
            conflict = new conflict_Field(columnFormatLine);
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
                    SEQADV.FieldValue,
                    idCode.FieldValue,
                    resName.FieldValue,
                    chainID.FieldValue,
                    seqNum.FieldValue,
                    iCode.FieldValue,
                    database.FieldValue,
                    dbAccession.FieldValue,
                    dbRes.FieldValue,
                    dbSeq.FieldValue,
                    conflict.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SEQADV_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "SEQADV";


            public SEQADV_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 71;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("PDB chain identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 17;
            public new const string DataType = "Character";
            public new const string Definition = "PDB chain identifier.";
            public new const string FieldName = "chainID";


            public chainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Conflict comment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class conflict_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 50;
            public new const int LastColumn = 70;
            public new const string DataType = "LString";
            public new const string Definition = "Conflict comment.";
            public new const string FieldName = "conflict";


            public conflict_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class database_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 25;
            public new const int LastColumn = 28;
            public new const string DataType = "LString";
            public new const string Definition = "";
            public new const string FieldName = "database";


            public database_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence database accession number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbAccession_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 30;
            public new const int LastColumn = 38;
            public new const string DataType = "LString";
            public new const string Definition = "Sequence database accession number.";
            public new const string FieldName = "dbAccession";


            public dbAccession_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence database residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbRes_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 40;
            public new const int LastColumn = 42;
            public new const string DataType = "Residue name";
            public new const string Definition = "Sequence database residue name.";
            public new const string FieldName = "dbRes";


            public dbRes_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence database sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbSeq_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 44;
            public new const int LastColumn = 48;
            public new const string DataType = "Integer";
            public new const string Definition = "Sequence database sequence number.";
            public new const string FieldName = "dbSeq";


            public dbSeq_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("PDB insertion code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 23;
            public new const int LastColumn = 23;
            public new const string DataType = "AChar";
            public new const string Definition = "PDB insertion code.";
            public new const string FieldName = "iCode";


            public iCode_Field(string columnFormatLine)
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

        [Description("Name of the PDB residue in conflict."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 15;
            public new const string DataType = "Residue name";
            public new const string Definition = "Name of the PDB residue in conflict.";
            public new const string FieldName = "resName";


            public resName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("PDB sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 19;
            public new const int LastColumn = 22;
            public new const string DataType = "Integer";
            public new const string Definition = "PDB sequence number.";
            public new const string FieldName = "seqNum";


            public seqNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
