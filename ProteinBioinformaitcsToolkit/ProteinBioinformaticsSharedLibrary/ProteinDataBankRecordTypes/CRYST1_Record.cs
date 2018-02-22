using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{

    [Serializable]
    public class CRYST1_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public CRYST1_Field CRYST1;

        [Description("a (Angstroms)."), Category("Data")]
        public a_Field a;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("alpha (degrees)."), Category("Data")]
        public alpha_Field alpha;
        [Description("b (Angstroms)."), Category("Data")]
        public b_Field b;

        [Description("beta (degrees)."), Category("Data")]
        public beta_Field beta;
        [Description("c (Angstroms)."), Category("Data")]
        public c_Field c;

        [Description("gamma (degrees)."), Category("Data")]
        public gamma_Field gamma;

        [Description("Space group."), Category("Data")]
        public sGroup_Field sGroup;

        [Description("Z value."), Category("Data")]
        public z_Field z;

        public CRYST1_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            CRYST1 = new CRYST1_Field(columnFormatLine);
            a = new a_Field(columnFormatLine);
            b = new b_Field(columnFormatLine);
            c = new c_Field(columnFormatLine);
            alpha = new alpha_Field(columnFormatLine);
            beta = new beta_Field(columnFormatLine);
            gamma = new gamma_Field(columnFormatLine);
            sGroup = new sGroup_Field(columnFormatLine);
            z = new z_Field(columnFormatLine);
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
                    CRYST1.FieldValue,
                    a.FieldValue,
                    b.FieldValue,
                    c.FieldValue,
                    alpha.FieldValue,
                    beta.FieldValue,
                    gamma.FieldValue,
                    sGroup.FieldValue,
                    z.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class CRYST1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "CRYST1";


            public CRYST1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("a (Angstroms)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class a_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 7;
            public new const int LastColumn = 15;
            public new const string DataType = "Real(9.3)";
            public new const string Definition = "a (Angstroms).";
            public new const string FieldName = "a";


            public a_Field(string columnFormatLine)
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

        [Description("alpha (degrees)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class alpha_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 34;
            public new const int LastColumn = 40;
            public new const string DataType = "Real(7.2)";
            public new const string Definition = "alpha (degrees).";
            public new const string FieldName = "alpha";


            public alpha_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("b (Angstroms)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class b_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 16;
            public new const int LastColumn = 24;
            public new const string DataType = "Real(9.3)";
            public new const string Definition = "b (Angstroms).";
            public new const string FieldName = "b";


            public b_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("beta (degrees)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class beta_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 41;
            public new const int LastColumn = 47;
            public new const string DataType = "Real(7.2)";
            public new const string Definition = "beta (degrees).";
            public new const string FieldName = "beta";


            public beta_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("c (Angstroms)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class c_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 25;
            public new const int LastColumn = 33;
            public new const string DataType = "Real(9.3)";
            public new const string Definition = "c (Angstroms).";
            public new const string FieldName = "c";


            public c_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("gamma (degrees)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class gamma_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 48;
            public new const int LastColumn = 54;
            public new const string DataType = "Real(7.2)";
            public new const string Definition = "gamma (degrees).";
            public new const string FieldName = "gamma";


            public gamma_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Space group."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class sGroup_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 56;
            public new const int LastColumn = 66;
            public new const string DataType = "LString";
            public new const string Definition = "Space group.";
            public new const string FieldName = "sGroup";


            public sGroup_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Z value."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class z_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = 70;
            public new const string DataType = "Integer";
            public new const string Definition = "Z value.";
            public new const string FieldName = "z";


            public z_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
