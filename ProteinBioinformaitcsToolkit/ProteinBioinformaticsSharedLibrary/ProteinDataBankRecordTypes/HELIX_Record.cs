using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class HELIX_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public HELIX_Field HELIX;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("Comment about this helix."), Category("Data")]
        public comment_Field comment;

        [Description("Chain identifier for the chain containing this helix."), Category("Data")]
        public endChainID_Field endChainID;

        [Description("Insertion code of the terminal residue."), Category("Data")]
        public endICode_Field endICode;
        [Description("Name of the terminal residue of the helix."), Category("Data")]
        public endResName_Field endResName;
        [Description("Sequence number of the terminal residue."), Category("Data")]
        public endSeqNum_Field endSeqNum;

        [Description("Helix class (see below)."), Category("Data")]
        public helixClass_Field helixClass;
        [Description("Helix identifier. In addition to a serial number, each helix is given an alphanumeric character helix identifier."), Category("Data")]
        public helixID_Field helixID;
        [Description("Chain identifier for the chain containing this helix."), Category("Data")]
        public initChainID_Field initChainID;
        [Description("Insertion code of the initial residue."), Category("Data")]
        public initICode_Field initICode;
        [Description("Name of the initial residue."), Category("Data")]
        public initResName_Field initResName;
        [Description("Sequence number of the initial residue."), Category("Data")]
        public initSeqNum_Field initSeqNum;

        [Description("Length of this helix."), Category("Data")]
        public length_Field length;
        [Description("Serial number of the helix. This starts at 1 and increases incrementally."), Category("Data")]
        public serNum_Field serNum;

        public HELIX_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            HELIX = new HELIX_Field(columnFormatLine);
            serNum = new serNum_Field(columnFormatLine);
            helixID = new helixID_Field(columnFormatLine);
            initResName = new initResName_Field(columnFormatLine);
            initChainID = new initChainID_Field(columnFormatLine);
            initSeqNum = new initSeqNum_Field(columnFormatLine);
            initICode = new initICode_Field(columnFormatLine);
            endResName = new endResName_Field(columnFormatLine);
            endChainID = new endChainID_Field(columnFormatLine);
            endSeqNum = new endSeqNum_Field(columnFormatLine);
            endICode = new endICode_Field(columnFormatLine);
            helixClass = new helixClass_Field(columnFormatLine);
            comment = new comment_Field(columnFormatLine);
            length = new length_Field(columnFormatLine);
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
                    HELIX.FieldValue,
                    serNum.FieldValue,
                    helixID.FieldValue,
                    initResName.FieldValue,
                    initChainID.FieldValue,
                    initSeqNum.FieldValue,
                    initICode.FieldValue,
                    endResName.FieldValue,
                    endChainID.FieldValue,
                    endSeqNum.FieldValue,
                    endICode.FieldValue,
                    helixClass.FieldValue,
                    comment.FieldValue,
                    length.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class HELIX_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "HELIX";


            public HELIX_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 77;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Comment about this helix."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class comment_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 41;
            public new const int LastColumn = 70;
            public new const string DataType = "string";
            public new const string Definition = "Comment about this helix.";
            public new const string FieldName = "comment";


            public comment_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier for the chain containing this helix."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endChainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = 32;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier for the chain containing this helix.";
            public new const string FieldName = "endChainID";


            public endChainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code of the terminal residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endICode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 38;
            public new const int LastColumn = 38;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code of the terminal residue.";
            public new const string FieldName = "endICode";


            public endICode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Name of the terminal residue of the helix."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endResName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 28;
            public new const int LastColumn = 30;
            public new const string DataType = "Residue name";
            public new const string Definition = "Name of the terminal residue of the helix.";
            public new const string FieldName = "endResName";


            public endResName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence number of the terminal residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endSeqNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 34;
            public new const int LastColumn = 37;
            public new const string DataType = "Integer";
            public new const string Definition = "Sequence number of the terminal residue.";
            public new const string FieldName = "endSeqNum";


            public endSeqNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Helix class (see below)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class helixClass_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 39;
            public new const int LastColumn = 40;
            public new const string DataType = "Integer";
            public new const string Definition = "Helix class (see below).";
            public new const string FieldName = "helixClass";


            public helixClass_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Helix identifier. In addition to a serial number, each helix is given an alphanumeric character helix identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class helixID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 14;
            public new const string DataType = "LString(3)";
            public new const string Definition = "Helix identifier. In addition to a serial number, each helix is given an alphanumeric character helix identifier.";
            public new const string FieldName = "helixID";


            public helixID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier for the chain containing this helix."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initChainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 20;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier for the chain containing this helix.";
            public new const string FieldName = "initChainID";


            public initChainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code of the initial residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initICode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 26;
            public new const int LastColumn = 26;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code of the initial residue.";
            public new const string FieldName = "initICode";


            public initICode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Name of the initial residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initResName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 16;
            public new const int LastColumn = 18;
            public new const string DataType = "Residue name";
            public new const string Definition = "Name of the initial residue.";
            public new const string FieldName = "initResName";


            public initResName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence number of the initial residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initSeqNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 25;
            public new const string DataType = "Integer";
            public new const string Definition = "Sequence number of the initial residue.";
            public new const string FieldName = "initSeqNum";


            public initSeqNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Length of this helix."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class length_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 72;
            public new const int LastColumn = 76;
            public new const string DataType = "Integer";
            public new const string Definition = "Length of this helix.";
            public new const string FieldName = "length";


            public length_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Serial number of the helix. This starts at 1 and increases incrementally."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number of the helix. This starts at 1 and increases incrementally.";
            public new const string FieldName = "serNum";


            public serNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
