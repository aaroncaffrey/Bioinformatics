using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class CONECT_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public CONECT_Field CONECT;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Atom serial number"), Category("Data")]
        public serial_Field serial;

        [Description("Serial number of bonded atom"), Category("Data")]
        public serial2_Field serial2;

        [Description("Serial number of bonded atom"), Category("Data")]
        public serial3_Field serial3;

        [Description("Serial number of bonded atom"), Category("Data")]
        public serial4_Field serial4;

        [Description("Serial number of bonded atom"), Category("Data")]
        public serial5_Field serial5;

        public CONECT_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            CONECT = new CONECT_Field(columnFormatLine);
            serial = new serial_Field(columnFormatLine);
            serial2 = new serial2_Field(columnFormatLine);
            serial3 = new serial3_Field(columnFormatLine);
            serial4 = new serial4_Field(columnFormatLine);
            serial5 = new serial5_Field(columnFormatLine);
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
                    CONECT.FieldValue,
                    serial.FieldValue,
                    serial2.FieldValue,
                    serial3.FieldValue,
                    serial4.FieldValue,
                    serial5.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class CONECT_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "CONECT";


            public CONECT_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 32;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Serial number of bonded atom"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serial2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 12;
            public new const int LastColumn = 16;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number of bonded atom";
            public new const string FieldName = "serial2";


            public serial2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Serial number of bonded atom"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serial3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 21;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number of bonded atom";
            public new const string FieldName = "serial3";


            public serial3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Serial number of bonded atom"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serial4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 22;
            public new const int LastColumn = 26;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number of bonded atom";
            public new const string FieldName = "serial4";


            public serial4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Serial number of bonded atom"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serial5_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 27;
            public new const int LastColumn = 31;
            public new const string DataType = "Integer";
            public new const string Definition = "Serial number of bonded atom";
            public new const string FieldName = "serial5";


            public serial5_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Atom serial number"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class serial_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 7;
            public new const int LastColumn = 11;
            public new const string DataType = "Integer";
            public new const string Definition = "Atom serial number";
            public new const string FieldName = "serial";


            public serial_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
