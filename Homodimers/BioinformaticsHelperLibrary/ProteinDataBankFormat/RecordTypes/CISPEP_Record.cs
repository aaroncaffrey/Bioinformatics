using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class CISPEP_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public CISPEP_Field CISPEP;
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

        [Description("Angle measurement in degrees."), Category("Data")]
        public measure_Field measure;
        [Description("Identifies the specific model."), Category("Data")]
        public modNum_Field modNum;
        [Description("Residue name."), Category("Data")]
        public pep1_Field pep1;
        [Description("Residue name."), Category("Data")]
        public pep2_Field pep2;
        [Description("Residue sequence number."), Category("Data")]
        public seqNum1_Field seqNum1;
        [Description("Residue sequence number."), Category("Data")]
        public seqNum2_Field seqNum2;
        [Description("Record serial number."), Category("Data")]
        public serNum_Field serNum;

        public CISPEP_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            CISPEP = new CISPEP_Field(columnFormatLine);
            serNum = new serNum_Field(columnFormatLine);
            pep1 = new pep1_Field(columnFormatLine);
            chainID1 = new chainID1_Field(columnFormatLine);
            seqNum1 = new seqNum1_Field(columnFormatLine);
            icode1 = new icode1_Field(columnFormatLine);
            pep2 = new pep2_Field(columnFormatLine);
            chainID2 = new chainID2_Field(columnFormatLine);
            seqNum2 = new seqNum2_Field(columnFormatLine);
            icode2 = new icode2_Field(columnFormatLine);
            modNum = new modNum_Field(columnFormatLine);
            measure = new measure_Field(columnFormatLine);
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
                    CISPEP.FieldValue,
                    serNum.FieldValue,
                    pep1.FieldValue,
                    chainID1.FieldValue,
                    seqNum1.FieldValue,
                    icode1.FieldValue,
                    pep2.FieldValue,
                    chainID2.FieldValue,
                    seqNum2.FieldValue,
                    icode2.FieldValue,
                    modNum.FieldValue,
                    measure.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class CISPEP_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "CISPEP";


            public CISPEP_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 60;
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

        [Description("Angle measurement in degrees."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class measure_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 54;
            public new const int LastColumn = 59;
            public new const string DataType = "Real(6.2)";
            public new const string Definition = "Angle measurement in degrees.";
            public new const string FieldName = "measure";


            public measure_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Identifies the specific model."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class modNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 44;
            public new const int LastColumn = 46;
            public new const string DataType = "Integer";
            public new const string Definition = "Identifies the specific model.";
            public new const string FieldName = "modNum";


            public modNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class pep1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 14;
            public new const string DataType = "LString(3)";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "pep1";


            public pep1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class pep2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 26;
            public new const int LastColumn = 28;
            public new const string DataType = "LString(3)";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "pep2";


            public pep2_Field(string columnFormatLine)
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

        [Description("Record serial number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Record serial number.";
            public new const string FieldName = "serNum";


            public serNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
