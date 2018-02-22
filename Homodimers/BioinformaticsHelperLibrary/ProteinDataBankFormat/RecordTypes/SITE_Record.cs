using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class SITE_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public SITE_Field SITE;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Chain identifier for first residue of proteinInterface."), Category("Data")]
        public chainID1_Field chainID1;

        [Description("Chain identifier for second residue of the proteinInterface."), Category("Data")]
        public chainID2_Field chainID2;

        [Description("Chain identifier for third residue of the proteinInterface."), Category("Data")]
        public chainID3_Field chainID3;
        [Description("Chain identifier for fourth residue of the proteinInterface."), Category("Data")]
        public chainID4_Field chainID4;
        [Description("Insertion code for first residue of the proteinInterface."), Category("Data")]
        public iCode1_Field iCode1;
        [Description("Insertion code for second residue of the proteinInterface."), Category("Data")]
        public iCode2_Field iCode2;

        [Description("Insertion code for third residue of the proteinInterface."), Category("Data")]
        public iCode3_Field iCode3;
        [Description("Insertion code for fourth residue of the proteinInterface."), Category("Data")]
        public iCode4_Field iCode4;
        [Description("Number of residues that compose the proteinInterface."), Category("Data")]
        public numRes_Field numRes;
        [Description("Residue name for first residue that creates the proteinInterface."), Category("Data")]
        public resName1_Field resName1;
        [Description("Residue name for second residue that creates the proteinInterface."), Category("Data")]
        public resName2_Field resName2;
        [Description("Residue name for third residue that creates the proteinInterface."), Category("Data")]
        public resName3_Field resName3;

        [Description("Residue name for fourth residue that creates the proteinInterface."), Category("Data")]
        public resName4_Field resName4;
        [Description("Residue sequence number for first residue of the proteinInterface."), Category("Data")]
        public seq1_Field seq1;
        [Description("Residue sequence number for second residue of the proteinInterface."), Category("Data")]
        public seq2_Field seq2;
        [Description("Residue sequence number for third residue of the proteinInterface."), Category("Data")]
        public seq3_Field seq3;

        [Description("Residue sequence number for fourth residue of the proteinInterface."), Category("Data")]
        public seq4_Field seq4;
        [Description("Sequence number."), Category("Data")]
        public seqNum_Field seqNum;
        [Description("ProteinInterface name."), Category("Data")]
        public proteinInterfaceID_Field proteinInterfaceID;

        public SITE_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SITE = new SITE_Field(columnFormatLine);
            seqNum = new seqNum_Field(columnFormatLine);
            proteinInterfaceID = new proteinInterfaceID_Field(columnFormatLine);
            numRes = new numRes_Field(columnFormatLine);
            resName1 = new resName1_Field(columnFormatLine);
            chainID1 = new chainID1_Field(columnFormatLine);
            seq1 = new seq1_Field(columnFormatLine);
            iCode1 = new iCode1_Field(columnFormatLine);
            resName2 = new resName2_Field(columnFormatLine);
            chainID2 = new chainID2_Field(columnFormatLine);
            seq2 = new seq2_Field(columnFormatLine);
            iCode2 = new iCode2_Field(columnFormatLine);
            resName3 = new resName3_Field(columnFormatLine);
            chainID3 = new chainID3_Field(columnFormatLine);
            seq3 = new seq3_Field(columnFormatLine);
            iCode3 = new iCode3_Field(columnFormatLine);
            resName4 = new resName4_Field(columnFormatLine);
            chainID4 = new chainID4_Field(columnFormatLine);
            seq4 = new seq4_Field(columnFormatLine);
            iCode4 = new iCode4_Field(columnFormatLine);
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
                    SITE.FieldValue,
                    seqNum.FieldValue,
                    proteinInterfaceID.FieldValue,
                    numRes.FieldValue,
                    resName1.FieldValue,
                    chainID1.FieldValue,
                    seq1.FieldValue,
                    iCode1.FieldValue,
                    resName2.FieldValue,
                    chainID2.FieldValue,
                    seq2.FieldValue,
                    iCode2.FieldValue,
                    resName3.FieldValue,
                    chainID3.FieldValue,
                    seq3.FieldValue,
                    iCode3.FieldValue,
                    resName4.FieldValue,
                    chainID4.FieldValue,
                    seq4.FieldValue,
                    iCode4.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SITE_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "SITE";


            public SITE_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 62;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Chain identifier for first residue of proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 23;
            public new const int LastColumn = 23;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier for first residue of proteinInterface.";
            public new const string FieldName = "chainID1";


            public chainID1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier for second residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 34;
            public new const int LastColumn = 34;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier for second residue of the proteinInterface.";
            public new const string FieldName = "chainID2";


            public chainID2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier for third residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 45;
            public new const int LastColumn = 45;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier for third residue of the proteinInterface.";
            public new const string FieldName = "chainID3";


            public chainID3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Chain identifier for fourth residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 56;
            public new const int LastColumn = 56;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier for fourth residue of the proteinInterface.";
            public new const string FieldName = "chainID4";


            public chainID4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code for first residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 28;
            public new const int LastColumn = 28;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code for first residue of the proteinInterface.";
            public new const string FieldName = "iCode1";


            public iCode1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code for second residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 39;
            public new const int LastColumn = 39;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code for second residue of the proteinInterface.";
            public new const string FieldName = "iCode2";


            public iCode2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code for third residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 50;
            public new const int LastColumn = 50;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code for third residue of the proteinInterface.";
            public new const string FieldName = "iCode3";


            public iCode3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Insertion code for fourth residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 61;
            public new const int LastColumn = 61;
            public new const string DataType = "AChar";
            public new const string Definition = "Insertion code for fourth residue of the proteinInterface.";
            public new const string FieldName = "iCode4";


            public iCode4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of residues that compose the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numRes_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 16;
            public new const int LastColumn = 17;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of residues that compose the proteinInterface.";
            public new const string FieldName = "numRes";


            public numRes_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name for first residue that creates the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 19;
            public new const int LastColumn = 21;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name for first residue that creates the proteinInterface.";
            public new const string FieldName = "resName1";


            public resName1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name for second residue that creates the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 30;
            public new const int LastColumn = 32;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name for second residue that creates the proteinInterface.";
            public new const string FieldName = "resName2";


            public resName2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name for third residue that creates the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 41;
            public new const int LastColumn = 43;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name for third residue that creates the proteinInterface.";
            public new const string FieldName = "resName3";


            public resName3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name for fourth residue that creates the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 52;
            public new const int LastColumn = 54;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name for fourth residue that creates the proteinInterface.";
            public new const string FieldName = "resName4";


            public resName4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number for first residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seq1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 24;
            public new const int LastColumn = 27;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number for first residue of the proteinInterface.";
            public new const string FieldName = "seq1";


            public seq1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number for second residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seq2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 35;
            public new const int LastColumn = 38;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number for second residue of the proteinInterface.";
            public new const string FieldName = "seq2";


            public seq2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number for third residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seq3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 46;
            public new const int LastColumn = 49;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number for third residue of the proteinInterface.";
            public new const string FieldName = "seq3";


            public seq3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue sequence number for fourth residue of the proteinInterface."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seq4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 57;
            public new const int LastColumn = 60;
            public new const string DataType = "Integer";
            public new const string Definition = "Residue sequence number for fourth residue of the proteinInterface.";
            public new const string FieldName = "seq4";


            public seq4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Sequence number."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class seqNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Sequence number.";
            public new const string FieldName = "seqNum";


            public seqNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("ProteinInterface name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class proteinInterfaceID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 14;
            public new const string DataType = "LString(3)";
            public new const string Definition = "ProteinInterface name.";
            public new const string FieldName = "proteinInterfaceID";


            public proteinInterfaceID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
