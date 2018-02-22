using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class HEADER_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public HEADER_Field HEADER;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Classifies the molecule(s)."), Category("Data")]
        public classification_Field classification;

        [Description("Deposition date. This is the date the coordinates were received at the PDB."), Category("Data")]
        public depDate_Field depDate;

        [Description("This identifier is unique within the PDB."), Category("Data")]
        public idCode_Field idCode;

        public HEADER_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            HEADER = new HEADER_Field(columnFormatLine);
            classification = new classification_Field(columnFormatLine);
            depDate = new depDate_Field(columnFormatLine);
            idCode = new idCode_Field(columnFormatLine);
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
                    HEADER.FieldValue,
                    classification.FieldValue,
                    depDate.FieldValue,
                    idCode.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class HEADER_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "HEADER";


            public HEADER_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 67;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Classifies the molecule(s)."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class classification_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 11;
            public new const int LastColumn = 50;
            public new const string DataType = "string(40)";
            public new const string Definition = "Classifies the molecule(s).";
            public new const string FieldName = "classification";


            public classification_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Deposition date. This is the date the coordinates were received at the PDB."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class depDate_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 51;
            public new const int LastColumn = 59;
            public new const string DataType = "Date";
            public new const string Definition = "Deposition date. This is the date the coordinates were received at the PDB.";
            public new const string FieldName = "depDate";


            public depDate_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("This identifier is unique within the PDB."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class idCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 63;
            public new const int LastColumn = 66;
            public new const string DataType = "IDcode";
            public new const string Definition = "This identifier is unique within the PDB.";
            public new const string FieldName = "idCode";


            public idCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
