using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    [Serializable]
    public class SEQRES_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public SEQRES_Field SEQRES;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Chain identifier. This may be any single legal character, including a blank which is used if there is only one chain."), Category("Data")]
        public chainID_Field chainID;

        [Description("Number of residues in the chain. This value is repeated on every record."), Category("Data")]
        public numRes_Field numRes;

        [Description("Residue name."), Category("Data")]
        public resName_Field resName;
        [Description("Residue name."), Category("Data")]
        public resName10_Field resName10;
        [Description("Residue name."), Category("Data")]
        public resName11_Field resName11;
        [Description("Residue name."), Category("Data")]
        public resName12_Field resName12;
        [Description("Residue name."), Category("Data")]
        public resName13_Field resName13;

        [Description("Residue name."), Category("Data")]
        public resName2_Field resName2;

        [Description("Residue name."), Category("Data")]
        public resName3_Field resName3;

        [Description("Residue name."), Category("Data")]
        public resName4_Field resName4;

        [Description("Residue name."), Category("Data")]
        public resName5_Field resName5;

        [Description("Residue name."), Category("Data")]
        public resName6_Field resName6;

        [Description("Residue name."), Category("Data")]
        public resName7_Field resName7;

        [Description("Residue name."), Category("Data")]
        public resName8_Field resName8;

        [Description("Residue name."), Category("Data")]
        public resName9_Field resName9;
        [Description("Serial number of the SEQRES record for the current chain. Starts at 1 and increments by one each line. Reset to 1 for each chain."), Category("Data")]
        public serNum_Field serNum;

        public SEQRES_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            SEQRES = new SEQRES_Field(columnFormatLine);
            serNum = new serNum_Field(columnFormatLine);
            chainID = new chainID_Field(columnFormatLine);
            numRes = new numRes_Field(columnFormatLine);
            resName = new resName_Field(columnFormatLine);
            resName2 = new resName2_Field(columnFormatLine);
            resName3 = new resName3_Field(columnFormatLine);
            resName4 = new resName4_Field(columnFormatLine);
            resName5 = new resName5_Field(columnFormatLine);
            resName6 = new resName6_Field(columnFormatLine);
            resName7 = new resName7_Field(columnFormatLine);
            resName8 = new resName8_Field(columnFormatLine);
            resName9 = new resName9_Field(columnFormatLine);
            resName10 = new resName10_Field(columnFormatLine);
            resName11 = new resName11_Field(columnFormatLine);
            resName12 = new resName12_Field(columnFormatLine);
            resName13 = new resName13_Field(columnFormatLine);
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
                    SEQRES.FieldValue,
                    serNum.FieldValue,
                    chainID.FieldValue,
                    numRes.FieldValue,
                    resName.FieldValue,
                    resName2.FieldValue,
                    resName3.FieldValue,
                    resName4.FieldValue,
                    resName5.FieldValue,
                    resName6.FieldValue,
                    resName7.FieldValue,
                    resName8.FieldValue,
                    resName9.FieldValue,
                    resName10.FieldValue,
                    resName11.FieldValue,
                    resName12.FieldValue,
                    resName13.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class SEQRES_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "SEQRES";


            public SEQRES_Field(string columnFormatLine)
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

        [Description("Chain identifier. This may be any single legal character, including a blank which is used if there is only one chain."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class chainID_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 12;
            public new const string DataType = "Character";
            public new const string Definition = "Chain identifier. This may be any single legal character, including a blank which is used if there is only one chain.";
            public new const string FieldName = "chainID";


            public chainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Number of residues in the chain. This value is repeated on every record."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class numRes_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 14;
            public new const int LastColumn = 17;
            public new const string DataType = "Integer";
            public new const string Definition = "Number of residues in the chain. This value is repeated on every record.";
            public new const string FieldName = "numRes";


            public numRes_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName10_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 56;
            public new const int LastColumn = 58;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName10";


            public resName10_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName11_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 60;
            public new const int LastColumn = 62;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName11";


            public resName11_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName12_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 64;
            public new const int LastColumn = 66;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName12";


            public resName12_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName13_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 68;
            public new const int LastColumn = 70;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName13";


            public resName13_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 24;
            public new const int LastColumn = 26;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName2";


            public resName2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 28;
            public new const int LastColumn = 30;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName3";


            public resName3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = 34;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName4";


            public resName4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName5_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 36;
            public new const int LastColumn = 38;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName5";


            public resName5_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName6_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 40;
            public new const int LastColumn = 42;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName6";


            public resName6_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName7_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 44;
            public new const int LastColumn = 46;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName7";


            public resName7_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName8_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 48;
            public new const int LastColumn = 50;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName8";


            public resName8_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName9_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 52;
            public new const int LastColumn = 54;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName9";


            public resName9_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Residue name."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class resName_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 22;
            public new const string DataType = "Residue name";
            public new const string Definition = "Residue name.";
            public new const string FieldName = "resName";


            public resName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Serial number of the SEQRES record for the current chain. Starts at 1 and increments by one each line. Reset to 1 for each chain."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serNum_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 8;
            public new const int LastColumn = 10;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number of the SEQRES record for the current chain. Starts at 1 and increments by one each line. Reset to 1 for each chain.";
            public new const string FieldName = "serNum";


            public serNum_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
