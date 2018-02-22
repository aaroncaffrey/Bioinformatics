using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class MASTER_0_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public MASTER_0_Field MASTER_0;

        [Description(""), Category("Data")]
        public _0_Field _0;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("Number of CONECT records"), Category("Data")]
        public numConect_Field numConect;
        [Description("Number of atomic coordinate records (ATOM+HETATM)"), Category("Data")]
        public numCoord_Field numCoord;

        [Description("Number of HELIX records"), Category("Data")]
        public numHelix_Field numHelix;
        [Description("Number of HET records"), Category("Data")]
        public numHet_Field numHet;
        [Description("Number of REMARK records"), Category("Data")]
        public numRemark_Field numRemark;
        [Description("Number of SEQRES records"), Category("Data")]
        public numSeq_Field numSeq;

        [Description("Number of SHEET records"), Category("Data")]
        public numSheet_Field numSheet;

        [Description("Number of SITE records"), Category("Data")]
        public numProteinInterface_Field numProteinInterface;

        [Description("Number of TER records"), Category("Data")]
        public numTer_Field numTer;
        [Description("deprecated"), Category("Data")]
        public numTurn_Field numTurn;
        [Description("Number of coordinate transformation records (ORIGX+SCALE+MTRIX)"), Category("Data")]
        public numXform_Field numXform;

        public MASTER_0_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            MASTER_0 = new MASTER_0_Field(columnFormatLine);
            numRemark = new numRemark_Field(columnFormatLine);
            _0 = new _0_Field(columnFormatLine);
            numHet = new numHet_Field(columnFormatLine);
            numHelix = new numHelix_Field(columnFormatLine);
            numSheet = new numSheet_Field(columnFormatLine);
            numTurn = new numTurn_Field(columnFormatLine);
            numProteinInterface = new numProteinInterface_Field(columnFormatLine);
            numXform = new numXform_Field(columnFormatLine);
            numCoord = new numCoord_Field(columnFormatLine);
            numTer = new numTer_Field(columnFormatLine);
            numConect = new numConect_Field(columnFormatLine);
            numSeq = new numSeq_Field(columnFormatLine);
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
                    MASTER_0.FieldValue,
                    numRemark.FieldValue,
                    _0.FieldValue,
                    numHet.FieldValue,
                    numHelix.FieldValue,
                    numSheet.FieldValue,
                    numTurn.FieldValue,
                    numProteinInterface.FieldValue,
                    numXform.FieldValue,
                    numCoord.FieldValue,
                    numTer.FieldValue,
                    numConect.FieldValue,
                    numSeq.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class MASTER_0_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "MASTER_0";


            public MASTER_0_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class _0_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 16;
            public new const int LastColumn = 20;
            public new const string DataType = "Integer";
            public new const string Definition = "";
            public new const string FieldName = "_0";


            public _0_Field(string columnFormatLine)
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

        [Description("Number of CONECT records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numConect_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 61;
            public new const int LastColumn = 65;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of CONECT records";
            public new const string FieldName = "numConect";


            public numConect_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of atomic coordinate records (ATOM+HETATM)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numCoord_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 51;
            public new const int LastColumn = 55;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of atomic coordinate records (ATOM+HETATM)";
            public new const string FieldName = "numCoord";


            public numCoord_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of HELIX records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numHelix_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 26;
            public new const int LastColumn = 30;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of HELIX records";
            public new const string FieldName = "numHelix";


            public numHelix_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of HET records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numHet_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 21;
            public new const int LastColumn = 25;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of HET records";
            public new const string FieldName = "numHet";


            public numHet_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of REMARK records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numRemark_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 15;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of REMARK records";
            public new const string FieldName = "numRemark";


            public numRemark_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of SEQRES records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numSeq_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 66;
            public new const int LastColumn = 70;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of SEQRES records";
            public new const string FieldName = "numSeq";


            public numSeq_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of SHEET records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numSheet_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 31;
            public new const int LastColumn = 35;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of SHEET records";
            public new const string FieldName = "numSheet";


            public numSheet_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of SITE records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numProteinInterface_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 41;
            public new const int LastColumn = 45;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of SITE records";
            public new const string FieldName = "numProteinInterface";


            public numProteinInterface_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of TER records"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numTer_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 56;
            public new const int LastColumn = 60;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of TER records";
            public new const string FieldName = "numTer";


            public numTer_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("deprecated"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numTurn_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 36;
            public new const int LastColumn = 40;
            public new const string DataType = "Integer";
            public new const string Definition = "deprecated";
            public new const string FieldName = "numTurn";


            public numTurn_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of coordinate transformation records (ORIGX+SCALE+MTRIX)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numXform_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 46;
            public new const int LastColumn = 50;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of coordinate transformation records (ORIGX+SCALE+MTRIX)";
            public new const string FieldName = "numXform";


            public numXform_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
