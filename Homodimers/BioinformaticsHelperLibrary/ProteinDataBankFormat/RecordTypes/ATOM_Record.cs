using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{
    [Serializable]
    public class ATOM_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public ATOM_Field ATOM;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;

        [Description("Alternate location indicator."), Category("Data")]
        public altLoc_Field altLoc;

        [Description("Chain identifier."), Category("Data")]
        public chainID_Field chainID;
        [Description("Charge on the atom."), Category("Data")]
        public charge_Field charge;
        [Description("Element symbol, right-justified."), Category("Data")]
        public element_Field element;

        [Description("Code for insertion of residues."), Category("Data")]
        public iCode_Field iCode;
        [Description("Atom name."), Category("Data")]
        public name_Field name;
        [Description("Occupancy."), Category("Data")]
        public occupancy_Field occupancy;
        [Description("Residue name."), Category("Data")]
        public resName_Field resName;
        [Description("Residue sequence number."), Category("Data")]
        public resSeq_Field resSeq;
        [Description("Atom serial number."), Category("Data")]
        public serial_Field serial;
        [Description("Temperature factor."), Category("Data")]
        public tempFactor_Field tempFactor;

        [Description("Orthogonal coordinates for X in Angstroms."), Category("Data")]
        public x_Field x;

        [Description("Orthogonal coordinates for Y in Angstroms."), Category("Data")]
        public y_Field y;

        [Description("Orthogonal coordinates for Z in Angstroms."), Category("Data")]
        public z_Field z;

        public ATOM_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            ATOM = new ATOM_Field(columnFormatLine);
            serial = new serial_Field(columnFormatLine);
            name = new name_Field(columnFormatLine);
            altLoc = new altLoc_Field(columnFormatLine);
            resName = new resName_Field(columnFormatLine);
            chainID = new chainID_Field(columnFormatLine);
            resSeq = new resSeq_Field(columnFormatLine);
            iCode = new iCode_Field(columnFormatLine);
            x = new x_Field(columnFormatLine);
            y = new y_Field(columnFormatLine);
            z = new z_Field(columnFormatLine);
            occupancy = new occupancy_Field(columnFormatLine);
            tempFactor = new tempFactor_Field(columnFormatLine);
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
                    ATOM.FieldValue,
                    serial.FieldValue,
                    name.FieldValue,
                    altLoc.FieldValue,
                    resName.FieldValue,
                    chainID.FieldValue,
                    resSeq.FieldValue,
                    iCode.FieldValue,
                    x.FieldValue,
                    y.FieldValue,
                    z.FieldValue,
                    occupancy.FieldValue,
                    tempFactor.FieldValue,
                    element.FieldValue,
                    charge.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class ATOM_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "ATOM";


            public ATOM_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Alternate location indicator."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class altLoc_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 17;
            public new const string DataType = "Character";
            public new const string Definition = "Alternate location indicator.";
            public new const string FieldName = "altLoc";


            public altLoc_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public chainID_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public charge_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public element_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Code for insertion of residues."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class iCode_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 27;
            public new const int LastColumn = 27;
            public new const string DataType = "AChar";
            public new const string Definition = "Code for insertion of residues.";
            public new const string FieldName = "iCode";


            public iCode_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public name_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Occupancy."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class occupancy_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 55;
            public new const int LastColumn = 60;
            public new const string DataType = "Real(6.2)";
            public new const string Definition = "Occupancy.";
            public new const string FieldName = "occupancy";


            public occupancy_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public resName_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public resSeq_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
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


            public serial_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Temperature factor."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class tempFactor_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 61;
            public new const int LastColumn = 66;
            public new const string DataType = "Real(6.2)";
            public new const string Definition = "Temperature factor.";
            public new const string FieldName = "tempFactor";


            public tempFactor_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Orthogonal coordinates for X in Angstroms."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class x_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 31;
            public new const int LastColumn = 38;
            public new const string DataType = "Real(8.3)";
            public new const string Definition = "Orthogonal coordinates for X in Angstroms.";
            public new const string FieldName = "x";


            public x_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Orthogonal coordinates for Y in Angstroms."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class y_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 39;
            public new const int LastColumn = 46;
            public new const string DataType = "Real(8.3)";
            public new const string Definition = "Orthogonal coordinates for Y in Angstroms.";
            public new const string FieldName = "y";


            public y_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Orthogonal coordinates for Z in Angstroms."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class z_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 47;
            public new const int LastColumn = 54;
            public new const string DataType = "Real(8.3)";
            public new const string Definition = "Orthogonal coordinates for Z in Angstroms.";
            public new const string FieldName = "z";


            public z_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
