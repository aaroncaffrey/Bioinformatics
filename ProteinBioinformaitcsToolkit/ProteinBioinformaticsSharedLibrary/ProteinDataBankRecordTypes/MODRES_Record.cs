using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class MODRES_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public MODRES_Field MODRES;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("Chain identifier."), Category("Data")]
        public chainID_Field chainID;
        [Description("Description of the residue modification."), Category("Data")]
        public comment_Field comment;
        [Description("Insertion code."), Category("Data")]
        public iCode_Field iCode;

        [Description("ID code of this entry."), Category("Data")]
        public idCode_Field idCode;

        [Description("Residue name used in this entry."), Category("Data")]
        public resName_Field resName;

        [Description("Sequence number."), Category("Data")]
        public seqNum_Field seqNum;

        [Description("Standard residue name."), Category("Data")]
        public stdRes_Field stdRes;

        public MODRES_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            MODRES = new MODRES_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
            resName = new resName_Field(columnFormatLine);
            chainID = new chainID_Field(columnFormatLine);
            seqNum = new seqNum_Field(columnFormatLine);
            iCode = new iCode_Field(columnFormatLine);
            stdRes = new stdRes_Field(columnFormatLine);
            comment = new comment_Field(columnFormatLine);
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
                    MODRES.FieldValue,
                    idCode.FieldValue,
                    resName.FieldValue,
                    chainID.FieldValue,
                    seqNum.FieldValue,
                    iCode.FieldValue,
                    stdRes.FieldValue,
                    comment.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class MODRES_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "MODRES";


            public MODRES_Field(string columnFormatLine)
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

        [Description("Chain identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 17;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier.";
            public new const string FieldName = "chainID";


            public chainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Description of the residue modification."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class comment_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 30;
            public new const int LastColumn = 70;
            public new const string DataType = "string";
            public new const string Definition = "Description of the residue modification.";
            public new const string FieldName = "comment";


            public comment_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 23;
            public new const int LastColumn = 23;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code.";
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

        [Description("Residue name used in this entry."), Category("Data")]
        [Serializable]
        public class resName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 15;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name used in this entry.";
            public new const string FieldName = "resName";


            public resName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 19;
            public new const int LastColumn = 22;
            public new const string DataType = "Integer";
            public new const string Definition = "Sequence number.";
            public new const string FieldName = "seqNum";


            public seqNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Standard residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class stdRes_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 25;
            public new const int LastColumn = 27;
            public new const string DataType = "Residue name";
            public new const string Definition = "Standard residue name.";
            public new const string FieldName = "stdRes";


            public stdRes_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
