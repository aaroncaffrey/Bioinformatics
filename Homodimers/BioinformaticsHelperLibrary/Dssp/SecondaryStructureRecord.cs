using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.Dssp
{
    [Serializable] public class SecondaryStructureRecord
    {
        public SecondaryStructureRecord.Field_SequentualResidueNumber FieldSequentualResidueNumber;
        public SecondaryStructureRecord.Field_PdbResidueSequenceIndex FieldPdbResidueSequenceIndex;
        public SecondaryStructureRecord.Field_iCode FieldICode;
        public SecondaryStructureRecord.Field_Chain FieldChain;
        public SecondaryStructureRecord.Field_AminoAcid FieldAminoAcid;
        public SecondaryStructureRecord.Field_SecondaryStructure FieldSecondaryStructure;
        public SecondaryStructureRecord.Field_TurnsHelix3 FieldTurnsHelix3;
        public SecondaryStructureRecord.Field_TurnsHelix4 FieldTurnsHelix4;
        public SecondaryStructureRecord.Field_TurnsHelix5 FieldTurnsHelix5;
        public SecondaryStructureRecord.Field_GeometricalBend FieldGeometricalBend;
        public SecondaryStructureRecord.Field_Chirality FieldChirality;
        public SecondaryStructureRecord.Field_BetaBridgeLabel2 FieldBetaBridgeLabel2;
        public SecondaryStructureRecord.Field_BridgePartner1 FieldBridgePartner1;
        public SecondaryStructureRecord.Field_BridgePartner2 FieldBridgePartner2;
        public SecondaryStructureRecord.Field_BetaSheetLabel FieldBetaSheetLabel;
        public SecondaryStructureRecord.Field_Acc FieldAcc;
        public SecondaryStructureRecord.Field_N_H___O1 FieldNHO1;
        public SecondaryStructureRecord.Field_O___H_N1 FieldOHN1;
        public SecondaryStructureRecord.Field_N_H___O2 FieldNHO2;
        public SecondaryStructureRecord.Field_O___H_N2 FieldOHN2;
        public SecondaryStructureRecord.Field_Tco FieldTco;
        public SecondaryStructureRecord.Field_Kappa FieldKappa;
        public SecondaryStructureRecord.Field_Alpha FieldAlpha;
        public SecondaryStructureRecord.Field_PHI FieldPhi;
        public SecondaryStructureRecord.Field_PSI FieldPsi;
        public SecondaryStructureRecord.Field_X_CA FieldXCa;
        public SecondaryStructureRecord.Field_Y_CA FieldYCa;
        public SecondaryStructureRecord.Field_Z_CA FieldZCa;


        public SecondaryStructureRecord(string columnFormatLine)
        {
            FieldSequentualResidueNumber = new SecondaryStructureRecord.Field_SequentualResidueNumber(columnFormatLine);
            FieldPdbResidueSequenceIndex = new SecondaryStructureRecord.Field_PdbResidueSequenceIndex(columnFormatLine);
            FieldICode = new SecondaryStructureRecord.Field_iCode(columnFormatLine);
            FieldChain = new SecondaryStructureRecord.Field_Chain(columnFormatLine);
            FieldAminoAcid = new SecondaryStructureRecord.Field_AminoAcid(columnFormatLine);
            FieldSecondaryStructure = new SecondaryStructureRecord.Field_SecondaryStructure(columnFormatLine);
            FieldTurnsHelix3 = new SecondaryStructureRecord.Field_TurnsHelix3(columnFormatLine);
            FieldTurnsHelix4 = new SecondaryStructureRecord.Field_TurnsHelix4(columnFormatLine);
            FieldTurnsHelix5 = new SecondaryStructureRecord.Field_TurnsHelix5(columnFormatLine);
            FieldGeometricalBend = new SecondaryStructureRecord.Field_GeometricalBend(columnFormatLine);
            FieldChirality = new SecondaryStructureRecord.Field_Chirality(columnFormatLine);
            FieldBetaBridgeLabel2 = new SecondaryStructureRecord.Field_BetaBridgeLabel2(columnFormatLine);
            FieldBridgePartner1 = new SecondaryStructureRecord.Field_BridgePartner1(columnFormatLine);
            FieldBridgePartner2 = new SecondaryStructureRecord.Field_BridgePartner2(columnFormatLine);
            FieldBetaSheetLabel = new SecondaryStructureRecord.Field_BetaSheetLabel(columnFormatLine);
            FieldAcc = new SecondaryStructureRecord.Field_Acc(columnFormatLine);
            FieldNHO1 = new SecondaryStructureRecord.Field_N_H___O1(columnFormatLine);
            FieldOHN1 = new SecondaryStructureRecord.Field_O___H_N1(columnFormatLine);
            FieldNHO2 = new SecondaryStructureRecord.Field_N_H___O2(columnFormatLine);
            FieldOHN2 = new SecondaryStructureRecord.Field_O___H_N2(columnFormatLine);
            FieldTco = new SecondaryStructureRecord.Field_Tco(columnFormatLine);
            FieldKappa = new SecondaryStructureRecord.Field_Kappa(columnFormatLine);
            FieldAlpha = new SecondaryStructureRecord.Field_Alpha(columnFormatLine);
            FieldPhi = new SecondaryStructureRecord.Field_PHI(columnFormatLine);
            FieldPsi = new SecondaryStructureRecord.Field_PSI(columnFormatLine);
            FieldXCa = new SecondaryStructureRecord.Field_X_CA(columnFormatLine);
            FieldYCa = new SecondaryStructureRecord.Field_Y_CA(columnFormatLine);
            FieldZCa = new SecondaryStructureRecord.Field_Z_CA(columnFormatLine);
        }

        [Serializable] public class Field_Acc : DsspField
        {
            public new const string FieldName = "Field_Acc";
            public new const int FirstColumn = 36;
            public new const int LastColumn = 38;


            public Field_Acc(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_Alpha : DsspField
        {
            public new const string FieldName = "Field_Alpha";
            public new const int FirstColumn = 99;
            public new const int LastColumn = 103;


            public Field_Alpha(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_AminoAcid : DsspField
        {
            public new const string FieldName = "Field_AminoAcid";
            public new const int FirstColumn = 14;
            public new const int LastColumn = 14;


            public Field_AminoAcid(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_BetaBridgeLabel2 : DsspField
        {
            public new const string FieldName = "Field_BetaBridgeLabel2";
            public new const int FirstColumn = 24;
            public new const int LastColumn = 25;


            public Field_BetaBridgeLabel2(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_BetaSheetLabel : DsspField
        {
            public new const string FieldName = "Field_BetaSheetLabel";
            public new const int FirstColumn = 34;
            public new const int LastColumn = 34;


            public Field_BetaSheetLabel(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_BridgePartner1 : DsspField
        {
            public new const string FieldName = "Field_BridgePartner1";
            public new const int FirstColumn = 27;
            public new const int LastColumn = 29;


            public Field_BridgePartner1(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_BridgePartner2 : DsspField
        {
            public new const string FieldName = "Field_BridgePartner2";
            public new const int FirstColumn = 31;
            public new const int LastColumn = 33;


            public Field_BridgePartner2(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_Chain : DsspField
        {
            public new const string FieldName = "Field_Chain";
            public new const int FirstColumn = 12;
            public new const int LastColumn = 12;


            public Field_Chain(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_Chirality : DsspField
        {
            public new const string FieldName = "Field_Chirality";
            public new const int FirstColumn = 23;
            public new const int LastColumn = 23;


            public Field_Chirality(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_GeometricalBend : DsspField
        {
            public new const string FieldName = "Field_GeometricalBend";
            public new const int FirstColumn = 22;
            public new const int LastColumn = 22;


            public Field_GeometricalBend(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_Kappa : DsspField
        {
            public new const string FieldName = "Field_Kappa";
            public new const int FirstColumn = 93;
            public new const int LastColumn = 97;


            public Field_Kappa(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_N_H___O1 : DsspField
        {
            public new const string FieldName = "Field_N_H___O1";
            public new const int FirstColumn = 40;
            public new const int LastColumn = 50;


            public Field_N_H___O1(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_N_H___O2 : DsspField
        {
            public new const string FieldName = "Field_N_H___O2";
            public new const int FirstColumn = 64;
            public new const int LastColumn = 74;


            public Field_N_H___O2(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_O___H_N1 : DsspField
        {
            public new const string FieldName = "Field_O___H_N1";
            public new const int FirstColumn = 52;
            public new const int LastColumn = 62;


            public Field_O___H_N1(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_O___H_N2 : DsspField
        {
            public new const string FieldName = "Field_O___H_N2";
            public new const int FirstColumn = 76;
            public new const int LastColumn = 86;


            public Field_O___H_N2(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_PHI : DsspField
        {
            public new const string FieldName = "Field_PHI";
            public new const int FirstColumn = 105;
            public new const int LastColumn = 109;


            public Field_PHI(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_PSI : DsspField
        {
            public new const string FieldName = "Field_PSI";
            public new const int FirstColumn = 111;
            public new const int LastColumn = 115;


            public Field_PSI(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_PdbResidueSequenceIndex : DsspField
        {
            public new const string FieldName = "Field_PdbResidueSequenceIndex";
            public new const int FirstColumn = 6;
            public new const int LastColumn = 10;


            public Field_PdbResidueSequenceIndex(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_SecondaryStructure : DsspField
        {
            public new const string FieldName = "Field_SecondaryStructure";
            public new const int FirstColumn = 17;
            public new const int LastColumn = 17;


            public Field_SecondaryStructure(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_SequentualResidueNumber : DsspField
        {
            public new const string FieldName = "Field_SequentualResidueNumber";
            public new const int FirstColumn = 1;
            public new const int LastColumn = 5;


            public Field_SequentualResidueNumber(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_Tco : DsspField
        {
            public new const string FieldName = "Field_Tco";
            public new const int FirstColumn = 86;
            public new const int LastColumn = 91;


            public Field_Tco(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_TurnsHelix3 : DsspField
        {
            public new const string FieldName = "Field_TurnsHelix3";
            public new const int FirstColumn = 19;
            public new const int LastColumn = 19;


            public Field_TurnsHelix3(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_TurnsHelix4 : DsspField
        {
            public new const string FieldName = "Field_TurnsHelix4";
            public new const int FirstColumn = 20;
            public new const int LastColumn = 20;


            public Field_TurnsHelix4(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_TurnsHelix5 : DsspField
        {
            public new const string FieldName = "Field_TurnsHelix5";
            public new const int FirstColumn = 21;
            public new const int LastColumn = 21;


            public Field_TurnsHelix5(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_X_CA : DsspField
        {
            public new const string FieldName = "Field_X_CA";
            public new const int FirstColumn = 116;
            public new const int LastColumn = 122;


            public Field_X_CA(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_Y_CA : DsspField
        {
            public new const string FieldName = "Field_Y_CA";
            public new const int FirstColumn = 123;
            public new const int LastColumn = 129;


            public Field_Y_CA(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_Z_CA : DsspField
        {
            public new const string FieldName = "Field_Z_CA";
            public new const int FirstColumn = 130;
            public new const int LastColumn = 136;


            public Field_Z_CA(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }

        [Serializable] public class Field_iCode : DsspField
        {
            public new const string FieldName = "Field_iCode";
            public new const int FirstColumn = 11;
            public new const int LastColumn = 11;


            public Field_iCode(string columnFormatLine)
                : base(fieldName: FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {

            }
        }
    }

}
