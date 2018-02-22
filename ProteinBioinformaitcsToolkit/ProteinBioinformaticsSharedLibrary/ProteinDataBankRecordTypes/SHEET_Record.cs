using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    [Serializable]
    public class SHEET_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public SHEET_Field SHEET;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Registration. Atom name in current strand."), Category("Data")]
        public curAtom_Field curAtom;

        [Description("Registration. Chain identifier in current strand."), Category("Data")]
        public curChainId_Field curChainId;

        [Description("Registration. Insertion code in current strand."), Category("Data")]
        public curICode_Field curICode;
        [Description("Registration. Residue name in current strand."), Category("Data")]
        public curResName_Field curResName;
        [Description("Registration. Residue sequence number in current strand."), Category("Data")]
        public curResSeq_Field curResSeq;
        [Description("Chain identifier of terminal residue."), Category("Data")]
        public endChainID_Field endChainID;
        [Description("Insertion code of terminal residue."), Category("Data")]
        public endICode_Field endICode;
        [Description("Residue name of terminal residue."), Category("Data")]
        public endResName_Field endResName;
        [Description("Sequence number of terminal residue."), Category("Data")]
        public endSeqNum_Field endSeqNum;
        [Description("Chain identifier of initial residue in strand."), Category("Data")]
        public initChainID_Field initChainID;
        [Description("Insertion code of initial residue in strand."), Category("Data")]
        public initICode_Field initICode;
        [Description("Residue name of initial residue."), Category("Data")]
        public initResName_Field initResName;
        [Description("Sequence number of initial residue in strand."), Category("Data")]
        public initSeqNum_Field initSeqNum;
        [Description("Number of strands in sheet."), Category("Data")]
        public numStrands_Field numStrands;

        [Description("Registration. Atom name in previous strand."), Category("Data")]
        public prevAtom_Field prevAtom;

        [Description("Registration. Chain identifier in previous strand."), Category("Data")]
        public prevChainId_Field prevChainId;

        [Description("Registration. Insertion code in previous strand."), Category("Data")]
        public prevICode_Field prevICode;
        [Description("Registration. Residue name in previous strand."), Category("Data")]
        public prevResName_Field prevResName;
        [Description("Registration. Residue sequence number in previous strand."), Category("Data")]
        public prevResSeq_Field prevResSeq;
        [Description("Sense of strand with respect to previous strand in the sheet. 0 if first strand, 1 if parallel,and -1 if anti-parallel."), Category("Data")]
        public sense_Field sense;
        [Description("Sheet identifier."), Category("Data")]
        public sheetID_Field sheetID;
        [Description("Strand number which starts at 1 for each strand within a sheet and increases by one."), Category("Data")]
        public strand_Field strand;

        public SHEET_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SHEET = new SHEET_Field(columnFormatLine);
            strand = new strand_Field(columnFormatLine);
            sheetID = new sheetID_Field(columnFormatLine);
            numStrands = new numStrands_Field(columnFormatLine);
            initResName = new initResName_Field(columnFormatLine);
            initChainID = new initChainID_Field(columnFormatLine);
            initSeqNum = new initSeqNum_Field(columnFormatLine);
            initICode = new initICode_Field(columnFormatLine);
            endResName = new endResName_Field(columnFormatLine);
            endChainID = new endChainID_Field(columnFormatLine);
            endSeqNum = new endSeqNum_Field(columnFormatLine);
            endICode = new endICode_Field(columnFormatLine);
            sense = new sense_Field(columnFormatLine);
            curAtom = new curAtom_Field(columnFormatLine);
            curResName = new curResName_Field(columnFormatLine);
            curChainId = new curChainId_Field(columnFormatLine);
            curResSeq = new curResSeq_Field(columnFormatLine);
            curICode = new curICode_Field(columnFormatLine);
            prevAtom = new prevAtom_Field(columnFormatLine);
            prevResName = new prevResName_Field(columnFormatLine);
            prevChainId = new prevChainId_Field(columnFormatLine);
            prevResSeq = new prevResSeq_Field(columnFormatLine);
            prevICode = new prevICode_Field(columnFormatLine);
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
                    SHEET.FieldValue,
                    strand.FieldValue,
                    sheetID.FieldValue,
                    numStrands.FieldValue,
                    initResName.FieldValue,
                    initChainID.FieldValue,
                    initSeqNum.FieldValue,
                    initICode.FieldValue,
                    endResName.FieldValue,
                    endChainID.FieldValue,
                    endSeqNum.FieldValue,
                    endICode.FieldValue,
                    sense.FieldValue,
                    curAtom.FieldValue,
                    curResName.FieldValue,
                    curChainId.FieldValue,
                    curResSeq.FieldValue,
                    curICode.FieldValue,
                    prevAtom.FieldValue,
                    prevResName.FieldValue,
                    prevChainId.FieldValue,
                    prevResSeq.FieldValue,
                    prevICode.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SHEET_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "SHEET";


            public SHEET_Field(string columnFormatLine)
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

        [Description("Registration. Atom name in current strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class curAtom_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 42;
            public new const int LastColumn = 45;
            public new const string DataType = "Atom";
            public new const string Definition = "Registration. Atom name in current strand.";
            public new const string FieldName = "curAtom";


            public curAtom_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Chain identifier in current strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class curChainId_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 50;
            public new const int LastColumn = 50;
            public new const string DataType = "Character";
            public new const string Definition = "Registration. Chain identifier in current strand.";
            public new const string FieldName = "curChainId";


            public curChainId_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Insertion code in current strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class curICode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 55;
            public new const int LastColumn = 55;
            public new const string DataType = "AChar";
            public new const string Definition = "Registration. Insertion code in current strand.";
            public new const string FieldName = "curICode";


            public curICode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Residue name in current strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class curResName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 46;
            public new const int LastColumn = 48;
            public new const string DataType = "Residue name";
            public new const string Definition = "Registration. Residue name in current strand.";
            public new const string FieldName = "curResName";


            public curResName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Residue sequence number in current strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class curResSeq_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 51;
            public new const int LastColumn = 54;
            public new const string DataType = "Integer";
            public new const string Definition = "Registration. Residue sequence number in current strand.";
            public new const string FieldName = "curResSeq";


            public curResSeq_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier of terminal residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endChainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 33;
            public new const int LastColumn = 33;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier of terminal residue.";
            public new const string FieldName = "endChainID";


            public endChainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code of terminal residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endICode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 38;
            public new const int LastColumn = 38;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code of terminal residue.";
            public new const string FieldName = "endICode";


            public endICode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name of terminal residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endResName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 29;
            public new const int LastColumn = 31;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name of terminal residue.";
            public new const string FieldName = "endResName";


            public endResName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence number of terminal residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class endSeqNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 34;
            public new const int LastColumn = 37;
            public new const string DataType = "Integer";
            public new const string Definition = "Sequence number of terminal residue.";
            public new const string FieldName = "endSeqNum";


            public endSeqNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier of initial residue in strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initChainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 22;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier of initial residue in strand.";
            public new const string FieldName = "initChainID";


            public initChainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code of initial residue in strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initICode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 27;
            public new const int LastColumn = 27;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code of initial residue in strand.";
            public new const string FieldName = "initICode";


            public initICode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name of initial residue."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initResName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 18;
            public new const int LastColumn = 20;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name of initial residue.";
            public new const string FieldName = "initResName";


            public initResName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence number of initial residue in strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class initSeqNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 23;
            public new const int LastColumn = 26;
            public new const string DataType = "Integer";
            public new const string Definition = "Sequence number of initial residue in strand.";
            public new const string FieldName = "initSeqNum";


            public initSeqNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of strands in sheet."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numStrands_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 15;
            public new const int LastColumn = 16;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of strands in sheet.";
            public new const string FieldName = "numStrands";


            public numStrands_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Atom name in previous strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class prevAtom_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 60;
            public new const string DataType = "Atom";
            public new const string Definition = "Registration. Atom name in previous strand.";
            public new const string FieldName = "prevAtom";


            public prevAtom_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Chain identifier in previous strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class prevChainId_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 65;
            public new const int LastColumn = 65;
            public new const string DataType = "Character";
            public new const string Definition = "Registration. Chain identifier in previous strand.";
            public new const string FieldName = "prevChainId";


            public prevChainId_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Insertion code in previous strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class prevICode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 70;
            public new const int LastColumn = 70;
            public new const string DataType = "AChar";
            public new const string Definition = "Registration. Insertion code in previous strand.";
            public new const string FieldName = "prevICode";


            public prevICode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Residue name in previous strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class prevResName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 61;
            public new const int LastColumn = 63;
            public new const string DataType = "Residue name";
            public new const string Definition = "Registration. Residue name in previous strand.";
            public new const string FieldName = "prevResName";


            public prevResName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Registration. Residue sequence number in previous strand."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class prevResSeq_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 66;
            public new const int LastColumn = 69;
            public new const string DataType = "Integer";
            public new const string Definition = "Registration. Residue sequence number in previous strand.";
            public new const string FieldName = "prevResSeq";


            public prevResSeq_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sense of strand with respect to previous strand in the sheet. 0 if first strand, 1 if parallel,and -1 if anti-parallel."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sense_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 39;
            public new const int LastColumn = 40;
            public new const string DataType = "Integer";
            public new const string Definition = "Sense of strand with respect to previous strand in the sheet. 0 if first strand, 1 if parallel,and -1 if anti-parallel.";
            public new const string FieldName = "sense";


            public sense_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sheet identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sheetID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 14;
            public new const string DataType = "LString(3)";
            public new const string Definition = "Sheet identifier.";
            public new const string FieldName = "sheetID";


            public sheetID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Strand number which starts at 1 for each strand within a sheet and increases by one."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class strand_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Strand number which starts at 1 for each strand within a sheet and increases by one.";
            public new const string FieldName = "strand";


            public strand_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
