using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    [Serializable]
    public class ANISOU_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")] public ANISOU_Field ANISOU;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")] public additionalData_Field additionalData;

        [Description("Alternate location indicator"), Category("Data")] public altLoc_Field altLoc;

        [Description("Chain identifier."), Category("Data")] public chainID_Field chainID;
        [Description("Charge on the atom."), Category("Data")] public charge_Field charge;
        [Description("Element symbol, right-justified."), Category("Data")] public element_Field element;

        [Description("Insertion code."), Category("Data")] public iCode_Field iCode;
        [Description("Atom name."), Category("Data")] public name_Field name;
        [Description("Residue name."), Category("Data")] public resName_Field resName;
        [Description("Residue sequence number."), Category("Data")] public resSeq_Field resSeq;
        [Description("Atom serial number."), Category("Data")] public serial_Field serial;

        [Description("U(1,1)"), Category("Data")] public u_0_0_Field u_0_0_;

        [Description("U(1,2)"), Category("Data")] public u_0_1_Field u_0_1_;

        [Description("U(1,3)"), Category("Data")] public u_0_2_Field u_0_2_;
        [Description("U(2,2)"), Category("Data")] public u_1_1_Field u_1_1_;

        [Description("U(2,3)"), Category("Data")] public u_1_2_Field u_1_2_;
        [Description("U(3,3)"), Category("Data")] public u_2_2_Field u_2_2_;

        public ANISOU_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            ANISOU = new ANISOU_Field(columnFormatLine);
            serial = new serial_Field(columnFormatLine);
            name = new name_Field(columnFormatLine);
            altLoc = new altLoc_Field(columnFormatLine);
            resName = new resName_Field(columnFormatLine);
            chainID = new chainID_Field(columnFormatLine);
            resSeq = new resSeq_Field(columnFormatLine);
            iCode = new iCode_Field(columnFormatLine);
            u_0_0_ = new u_0_0_Field(columnFormatLine);
            u_1_1_ = new u_1_1_Field(columnFormatLine);
            u_2_2_ = new u_2_2_Field(columnFormatLine);
            u_0_1_ = new u_0_1_Field(columnFormatLine);
            u_0_2_ = new u_0_2_Field(columnFormatLine);
            u_1_2_ = new u_1_2_Field(columnFormatLine);
            element = new element_Field(columnFormatLine);
            charge = new charge_Field(columnFormatLine);
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
                ANISOU.FieldValue,
                serial.FieldValue,
                name.FieldValue,
                altLoc.FieldValue,
                resName.FieldValue,
                chainID.FieldValue,
                resSeq.FieldValue,
                iCode.FieldValue,
                u_0_0_.FieldValue,
                u_1_1_.FieldValue,
                u_2_2_.FieldValue,
                u_0_1_.FieldValue,
                u_0_2_.FieldValue,
                u_1_2_.FieldValue,
                element.FieldValue,
                charge.FieldValue,
                additionalData.FieldValue
            };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class ANISOU_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "ANISOU";


            public ANISOU_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 81;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {
                
            }

        }

        [Description("Alternate location indicator"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class altLoc_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 17;
            public new const string DataType = "Character";
            public new const string Definition = "Alternate location indicator";
            public new const string FieldName = "altLoc";


            public altLoc_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 22;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier.";
            public new const string FieldName = "chainID";


            public chainID_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Charge on the atom."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class charge_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 79;
            public new const int LastColumn = 80;
            public new const string DataType = "LString(2)";
            public new const string Definition = "Charge on the atom.";
            public new const string FieldName = "charge";


            public charge_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Element symbol, right-justified."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class element_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 77;
            public new const int LastColumn = 78;
            public new const string DataType = "LString(2)";
            public new const string Definition = "Element symbol, right-justified.";
            public new const string FieldName = "element";


            public element_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 27;
            public new const int LastColumn = 27;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code.";
            public new const string FieldName = "iCode";


            public iCode_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Atom name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class name_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "Atom";
            public new const string Definition = "Atom name.";
            public new const string FieldName = "name";


            public name_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 18;
            public new const int LastColumn = 20;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName";


            public resName_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resSeq_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 23;
            public new const int LastColumn = 26;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number.";
            public new const string FieldName = "resSeq";


            public resSeq_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Atom serial number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serial_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 7;
            public new const int LastColumn = 11;
            public new const string DataType = "Integer";
            public new const string Definition = "Atom serial number.";
            public new const string FieldName = "serial";


            public serial_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("U(1,1)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class u_0_0_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 29;
            public new const int LastColumn = 35;
            public new const string DataType = "Integer";
            public new const string Definition = "U(1,1)";
            public new const string FieldName = "u_0_0_";


            public u_0_0_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("U(1,2)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class u_0_1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 50;
            public new const int LastColumn = 56;
            public new const string DataType = "Integer";
            public new const string Definition = "U(1,2)";
            public new const string FieldName = "u_0_1_";


            public u_0_1_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("U(1,3)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class u_0_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 63;
            public new const string DataType = "Integer";
            public new const string Definition = "U(1,3)";
            public new const string FieldName = "u_0_2_";


            public u_0_2_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("U(2,2)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class u_1_1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 36;
            public new const int LastColumn = 42;
            public new const string DataType = "Integer";
            public new const string Definition = "U(2,2)";
            public new const string FieldName = "u_1_1_";


            public u_1_1_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("U(2,3)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class u_1_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 64;
            public new const int LastColumn = 70;
            public new const string DataType = "Integer";
            public new const string Definition = "U(2,3)";
            public new const string FieldName = "u_1_2_";


            public u_1_2_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("U(3,3)"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class u_2_2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 43;
            public new const int LastColumn = 49;
            public new const string DataType = "Integer";
            public new const string Definition = "U(3,3)";
            public new const string FieldName = "u_2_2_";


            public u_2_2_Field(string columnFormatLine) : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }
}
