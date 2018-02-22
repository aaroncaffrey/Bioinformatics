using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class LINK_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public LINK_Field LINK;
        [Description("Link distance"), Category("Data")]
        public Length_Field Length;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Alternate location indicator."), Category("Data")]
        public altLoc1_Field altLoc1;
        [Description("Alternate location indicator."), Category("Data")]
        public altLoc2_Field altLoc2;

        [Description("Chain identifier."), Category("Data")]
        public chainID1_Field chainID1;
        [Description("Chain identifier."), Category("Data")]
        public chainID2_Field chainID2;

        [Description("Insertion code."), Category("Data")]
        public iCode1_Field iCode1;
        [Description("Insertion code."), Category("Data")]
        public iCode2_Field iCode2;
        [Description("Atom name."), Category("Data")]
        public name1_Field name1;

        [Description("Atom name."), Category("Data")]
        public name2_Field name2;
        [Description("Residue name."), Category("Data")]
        public resName1_Field resName1;

        [Description("Residue name."), Category("Data")]
        public resName2_Field resName2;
        [Description("Residue sequence number."), Category("Data")]
        public resSeq1_Field resSeq1;

        [Description("Residue sequence number."), Category("Data")]
        public resSeq2_Field resSeq2;

        [Description("Symmetry operator atom 1."), Category("Data")]
        public sym1_Field sym1;

        [Description("Symmetry operator atom 2."), Category("Data")]
        public sym2_Field sym2;

        public LINK_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            LINK = new LINK_Field(columnFormatLine);
            name1 = new name1_Field(columnFormatLine);
            altLoc1 = new altLoc1_Field(columnFormatLine);
            resName1 = new resName1_Field(columnFormatLine);
            chainID1 = new chainID1_Field(columnFormatLine);
            resSeq1 = new resSeq1_Field(columnFormatLine);
            iCode1 = new iCode1_Field(columnFormatLine);
            name2 = new name2_Field(columnFormatLine);
            altLoc2 = new altLoc2_Field(columnFormatLine);
            resName2 = new resName2_Field(columnFormatLine);
            chainID2 = new chainID2_Field(columnFormatLine);
            resSeq2 = new resSeq2_Field(columnFormatLine);
            iCode2 = new iCode2_Field(columnFormatLine);
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
                    LINK.FieldValue,
                    name1.FieldValue,
                    altLoc1.FieldValue,
                    resName1.FieldValue,
                    chainID1.FieldValue,
                    resSeq1.FieldValue,
                    iCode1.FieldValue,
                    name2.FieldValue,
                    altLoc2.FieldValue,
                    resName2.FieldValue,
                    chainID2.FieldValue,
                    resSeq2.FieldValue,
                    iCode2.FieldValue,
                    sym1.FieldValue,
                    sym2.FieldValue,
                    Length.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class LINK_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "LINK";


            public LINK_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Link distance"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class Length_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 74;
            public new const int LastColumn = 78;
            public new const string DataType = "Real(5.2)";
            public new const string Definition = "Link distance";
            public new const string FieldName = "Length";


            public Length_Field(string columnFormatLine)
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

        [Description("Alternate location indicator."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class altLoc1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 17;
            public new const string DataType = "Character";
            public new const string Definition = "Alternate location indicator.";
            public new const string FieldName = "altLoc1";


            public altLoc1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Alternate location indicator."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class altLoc2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 47;
            public new const int LastColumn = 47;
            public new const string DataType = "Character";
            public new const string Definition = "Alternate location indicator.";
            public new const string FieldName = "altLoc2";


            public altLoc2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 22;
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
            public new const int FirstColumn = 52;
            public new const int LastColumn = 52;
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
        public class iCode1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 27;
            public new const int LastColumn = 27;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code.";
            public new const string FieldName = "iCode1";


            public iCode1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 57;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code.";
            public new const string FieldName = "iCode2";


            public iCode2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Atom name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class name1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "Atom";
            public new const string Definition = "Atom name.";
            public new const string FieldName = "name1";


            public name1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Atom name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class name2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 43;
            public new const int LastColumn = 46;
            public new const string DataType = "Atom";
            public new const string Definition = "Atom name.";
            public new const string FieldName = "name2";


            public name2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 18;
            public new const int LastColumn = 20;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName1";


            public resName1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 48;
            public new const int LastColumn = 50;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName2";


            public resName2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resSeq1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 23;
            public new const int LastColumn = 26;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number.";
            public new const string FieldName = "resSeq1";


            public resSeq1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resSeq2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 53;
            public new const int LastColumn = 56;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number.";
            public new const string FieldName = "resSeq2";


            public resSeq2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Symmetry operator atom 1."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sym1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 60;
            public new const int LastColumn = 65;
            public new const string DataType = "SymOP";
            public new const string Definition = "Symmetry operator atom 1.";
            public new const string FieldName = "sym1";


            public sym1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Symmetry operator atom 2."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sym2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = 72;
            public new const string DataType = "SymOP";
            public new const string Definition = "Symmetry operator atom 2.";
            public new const string FieldName = "sym2";


            public sym2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
