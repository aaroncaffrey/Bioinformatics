using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    [Serializable]
    public class SSBOND_CYS_CYS_Record : ProteinDataBankFileRecord
    {
        [Description("Residue name."), Category("Data")]
        public CYS_Field CYS;

        [Description("Residue name."), Category("Data")]
        public CYS2_Field CYS2;
        [Description("Disulfide bond distance"), Category("Data")]
        public Length_Field Length;
        [Description(""), Category("Data")]
        public SSBOND_CYS_CYS_Field SSBOND_CYS_CYS;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("Chain identifier."), Category("Data")]
        public chainID1_Field chainID1;

        [Description("Chain identifier."), Category("Data")]
        public chainID2_Field chainID2;
        [Description("Insertion code."), Category("Data")]
        public icode1_Field icode1;

        [Description("Insertion code."), Category("Data")]
        public icode2_Field icode2;
        [Description("Residue sequence number."), Category("Data")]
        public seqNum1_Field seqNum1;
        [Description("Residue sequence number."), Category("Data")]
        public seqNum2_Field seqNum2;
        [Description("Serial number."), Category("Data")]
        public serNum_Field serNum;

        [Description("Symmetry operator for residue 1."), Category("Data")]
        public sym1_Field sym1;

        [Description("Symmetry operator for residue 2."), Category("Data")]
        public sym2_Field sym2;

        public SSBOND_CYS_CYS_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SSBOND_CYS_CYS = new SSBOND_CYS_CYS_Field(columnFormatLine);
            serNum = new serNum_Field(columnFormatLine);
            CYS = new CYS_Field(columnFormatLine);
            chainID1 = new chainID1_Field(columnFormatLine);
            seqNum1 = new seqNum1_Field(columnFormatLine);
            icode1 = new icode1_Field(columnFormatLine);
            CYS2 = new CYS2_Field(columnFormatLine);
            chainID2 = new chainID2_Field(columnFormatLine);
            seqNum2 = new seqNum2_Field(columnFormatLine);
            icode2 = new icode2_Field(columnFormatLine);
            sym1 = new sym1_Field(columnFormatLine);
            sym2 = new sym2_Field(columnFormatLine);
            Length = new Length_Field(columnFormatLine);
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
                    SSBOND_CYS_CYS.FieldValue,
                    serNum.FieldValue,
                    CYS.FieldValue,
                    chainID1.FieldValue,
                    seqNum1.FieldValue,
                    icode1.FieldValue,
                    CYS2.FieldValue,
                    chainID2.FieldValue,
                    seqNum2.FieldValue,
                    icode2.FieldValue,
                    sym1.FieldValue,
                    sym2.FieldValue,
                    Length.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class CYS2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 26;
            public new const int LastColumn = 28;
            public new const string DataType = "LString(3)";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "CYS2";


            public CYS2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class CYS_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 14;
            public new const string DataType = "LString(3)";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "CYS";


            public CYS_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Disulfide bond distance"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class Length_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 74;
            public new const int LastColumn = 78;
            public new const string DataType = "Real(5.2)";
            public new const string Definition = "Disulfide bond distance";
            public new const string FieldName = "Length";


            public Length_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SSBOND_CYS_CYS_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "SSBOND_CYS_CYS";


            public SSBOND_CYS_CYS_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 79;
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
        public class chainID1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 16;
            public new const int LastColumn = 16;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier.";
            public new const string FieldName = "chainID1";


            public chainID1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 30;
            public new const int LastColumn = 30;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier.";
            public new const string FieldName = "chainID2";


            public chainID2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class icode1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 22;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code.";
            public new const string FieldName = "icode1";


            public icode1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class icode2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 36;
            public new const int LastColumn = 36;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code.";
            public new const string FieldName = "icode2";


            public icode2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqNum1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 18;
            public new const int LastColumn = 21;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number.";
            public new const string FieldName = "seqNum1";


            public seqNum1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqNum2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = 35;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number.";
            public new const string FieldName = "seqNum2";


            public seqNum2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Serial number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number.";
            public new const string FieldName = "serNum";


            public serNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Symmetry operator for residue 1."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sym1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 60;
            public new const int LastColumn = 65;
            public new const string DataType = "SymOP";
            public new const string Definition = "Symmetry operator for residue 1.";
            public new const string FieldName = "sym1";


            public sym1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Symmetry operator for residue 2."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sym2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = 72;
            public new const string DataType = "SymOP";
            public new const string Definition = "Symmetry operator for residue 2.";
            public new const string FieldName = "sym2";


            public sym2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
