using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class DBREF_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public DBREF_Field DBREF;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Chain identifier."), Category("Data")]
        public chainID_Field chainID;

        [Description("Sequence database name."), Category("Data")]
        public database_Field database;

        [Description("Sequence database accession code."), Category("Data")]
        public dbAccession_Field dbAccession;

        [Description("Sequence database identification code."), Category("Data")]
        public dbIdCode_Field dbIdCode;
        [Description("Insertion code of the ending residue of the segment, if PDB is the reference."), Category("Data")]
        public dbinsEnd_Field dbinsEnd;

        [Description("Initial sequence number of the database seqment."), Category("Data")]
        public dbseqBegin_Field dbseqBegin;

        [Description("Ending sequence number of the database segment."), Category("Data")]
        public dbseqEnd_Field dbseqEnd;
        [Description("ID code of this entry."), Category("Data")]
        public idCode_Field idCode;
        [Description("Insertion code of initial residue of the segment, if PDB is the reference."), Category("Data")]
        public idbnsBeg_Field idbnsBeg;
        [Description("Initial insertion code of the PDB sequence segment."), Category("Data")]
        public insertBegin_Field insertBegin;
        [Description("Ending insertion code of the PDB sequence segment."), Category("Data")]
        public insertEnd_Field insertEnd;
        [Description("Initial sequence number of the PDB sequence segment."), Category("Data")]
        public seqBegin_Field seqBegin;
        [Description("Ending sequence number of the PDB sequence segment."), Category("Data")]
        public seqEnd_Field seqEnd;

        public DBREF_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            DBREF = new DBREF_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
            chainID = new chainID_Field(columnFormatLine);
            seqBegin = new seqBegin_Field(columnFormatLine);
            insertBegin = new insertBegin_Field(columnFormatLine);
            seqEnd = new seqEnd_Field(columnFormatLine);
            insertEnd = new insertEnd_Field(columnFormatLine);
            database = new database_Field(columnFormatLine);
            dbAccession = new dbAccession_Field(columnFormatLine);
            dbIdCode = new dbIdCode_Field(columnFormatLine);
            dbseqBegin = new dbseqBegin_Field(columnFormatLine);
            idbnsBeg = new idbnsBeg_Field(columnFormatLine);
            dbseqEnd = new dbseqEnd_Field(columnFormatLine);
            dbinsEnd = new dbinsEnd_Field(columnFormatLine);
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
                    DBREF.FieldValue,
                    idCode.FieldValue,
                    chainID.FieldValue,
                    seqBegin.FieldValue,
                    insertBegin.FieldValue,
                    seqEnd.FieldValue,
                    insertEnd.FieldValue,
                    database.FieldValue,
                    dbAccession.FieldValue,
                    dbIdCode.FieldValue,
                    dbseqBegin.FieldValue,
                    idbnsBeg.FieldValue,
                    dbseqEnd.FieldValue,
                    dbinsEnd.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class DBREF_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "DBREF";


            public DBREF_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 69;
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

        [Description("Sequence database accession code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbAccession_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 34;
            public new const int LastColumn = 41;
            public new const string DataType = "LString";
            public new const string Definition = "Sequence database accession code.";
            public new const string FieldName = "dbAccession";


            public dbAccession_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence database identification code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbIdCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 43;
            public new const int LastColumn = 54;
            public new const string DataType = "LString";
            public new const string Definition = "Sequence database identification code.";
            public new const string FieldName = "dbIdCode";


            public dbIdCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code of the ending residue of the segment, if PDB is the reference."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbinsEnd_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 68;
            public new const int LastColumn = 68;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code of the ending residue of the segment, if PDB is the reference.";
            public new const string FieldName = "dbinsEnd";


            public dbinsEnd_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Initial sequence number of the database seqment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbseqBegin_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 56;
            public new const int LastColumn = 60;
            public new const string DataType = "Integer";
            public new const string Definition = "Initial sequence number of the database seqment.";
            public new const string FieldName = "dbseqBegin";


            public dbseqBegin_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Ending sequence number of the database segment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class dbseqEnd_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 63;
            public new const int LastColumn = 67;
            public new const string DataType = "Integer";
            public new const string Definition = "Ending sequence number of the database segment.";
            public new const string FieldName = "dbseqEnd";


            public dbseqEnd_Field(string columnFormatLine)
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

        [Description("Insertion code of initial residue of the segment, if PDB is the reference."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idbnsBeg_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 61;
            public new const int LastColumn = 61;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code of initial residue of the segment, if PDB is the reference.";
            public new const string FieldName = "idbnsBeg";


            public idbnsBeg_Field(string columnFormatLine)
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

        [Description("Initial sequence number of the PDB sequence segment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqBegin_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 15;
            public new const int LastColumn = 18;
            public new const string DataType = "Integer";
            public new const string Definition = "Initial sequence number of the PDB sequence segment.";
            public new const string FieldName = "seqBegin";


            public seqBegin_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Ending sequence number of the PDB sequence segment."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqEnd_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 21;
            public new const int LastColumn = 24;
            public new const string DataType = "Integer";
            public new const string Definition = "Ending sequence number of the PDB sequence segment.";
            public new const string FieldName = "seqEnd";


            public seqEnd_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
