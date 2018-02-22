using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.TypeConversions;
using BioinformaticsHelperLibrary.UserProteinInterface;
using BioinformaticsHelperLibrary.Dssp;

namespace BioinformaticsHelperLibrary.InteractionVector
{

    public class TreeDataSpreadsheetRecord
    {
        public string TreeId;
        public string PdbId;
        public string ChainId;
        public string ProteinInterfaceId;
        public string Direction;

        public string SequenceLength;
        public string BindingProteinInterfaceLength;
        public string BindingProteinInterfaceStart;
        public string BindingProteinInterfaceEnd;

        public string BindingProteinInterfaceSecondaryStructure;
        public string BindingProteinInterfaceInteractionSecondaryStructure;
        public string BindingProteinInterfaceNonInteractionSecondaryStructure;

        public string BindingProteinInterfaceVectorShort;
        public string BindingProteinInterfaceVectorLong;

        public string BindingProteinInterfaceSequence1L;
        public string BindingProteinInterfaceInteractionSequence1L;
        public string BindingProteinInterfaceNonInteractionSequence1L;

        public string BindingProteinInterfaceSequence3L;
        public string BindingProteinInterfaceInteractionSequence3L;
        public string BindingProteinInterfaceNonInteractionSequence3L;

        public string PhysicochemicalGroupNumbers;
        public string PhysicochemicalInteractionGroupNumbers;
        public string PhysicochemicalNonInteractionGroupNumbers;

        public string HydrophobicityGroupNumbers;
        public string HydrophobicityInteractionGroupNumbers;
        public string HydrophobicityNonInteractionGroupNumbers;

        public string PdbSumGroupNumbers;
        public string PdbSumInteractionGroupNumbers;
        public string PdbSumNonInteractionGroupNumbers;

        public string UniProtKbGroupNumbers;
        public string UniProtKbInteractionGroupNumbers;
        public string UniProtKbNonInteractionGroupNumbers;

        public string BindingProteinInterfaceVectorShortMirrorSymmetryPercentage;
        public string BindingProteinInterfaceVectorLongMirrorSymmetryPercentage;
        public string BindingProteinInterfaceVectorShortCloneSymmetryPercentage;
        public string BindingProteinInterfaceVectorLongCloneSymmetryPercentage;

        public string BindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage;
        public string BindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage;
        public string BindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage;

        public string BindingProteinInterfaceSequence1LMirrorSymmetryPercentage;
        public string BindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage;
        public string BindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage;

        public string PhysicochemicalGroupNumbersMirrorSymmetryPercentage;
        public string PhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage;
        public string PhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage;

        public string HydrophobicityGroupNumbersMirrorSymmetryPercentage;
        public string HydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage;
        public string HydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage;

        public string PdbSumGroupNumbersMirrorSymmetryPercentage;
        public string PdbSumInteractionGroupNumbersMirrorSymmetryPercentage;
        public string PdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage;

        public string UniProtKbGroupNumbersMirrorSymmetryPercentage;
        public string UniProtKbInteractionGroupNumbersMirrorSymmetryPercentage;
        public string UniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage;

        public string BindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage;
        public string BindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage;
        public string BindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage;

        public string BindingProteinInterfaceSequence1LCloneSymmetryPercentage;
        public string BindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage;
        public string BindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage;

        public string PhysicochemicalGroupNumbersCloneSymmetryPercentage;
        public string PhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage;
        public string PhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage;

        public string HydrophobicityGroupNumbersCloneSymmetryPercentage;
        public string HydrophobicityInteractionGroupNumbersCloneSymmetryPercentage;
        public string HydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage;

        public string PdbSumGroupNumbersCloneSymmetryPercentage;
        public string PdbSumInteractionGroupNumbersCloneSymmetryPercentage;
        public string PdbSumNonInteractionGroupNumbersCloneSymmetryPercentage;

        public string UniProtKbGroupNumbersCloneSymmetryPercentage;
        public string UniProtKbInteractionGroupNumbersCloneSymmetryPercentage;
        public string UniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage;

        public string BindingProteinInterfaceSequence1LPhysicochemicalColorisation;
        public string BindingProteinInterfaceSequence1LHydrophobicityColorisation;
        public string BindingProteinInterfaceSequence1LPdbSumColorisation;
        public string BindingProteinInterfaceSequence1LUniProtKbColorisation;

        public string BindingProteinInterfaceInteractionSequence1LPhysicochemicalColorisation;
        public string BindingProteinInterfaceInteractionSequence1LHydrophobicityColorisation;
        public string BindingProteinInterfaceInteractionSequence1LPdbSumColorisation;
        public string BindingProteinInterfaceInteractionSequence1LUniProtKbColorisation;

        public string BindingProteinInterfaceNonInteractionSequence1LPhysicochemicalColorisation;
        public string BindingProteinInterfaceNonInteractionSequence1LHydrophobicityColorisation;
        public string BindingProteinInterfaceNonInteractionSequence1LPdbSumColorisation;
        public string BindingProteinInterfaceNonInteractionSequence1LUniProtKbColorisation;

        //public string Acidic;
        //public string AcidicPercentage;
        public string Aliphatic;
        public string AliphaticPercentage;
        public string Aromatic;
        public string AromaticPercentage;
        public string Charged;
        public string ChargedPercentage;
        public string Hydrophobic;
        public string HydrophobicPercentage;
        public string Hydroxylic;
        public string HydroxylicPercentage;
        public string Negative;
        public string NegativePercentage;
        public string Polar;
        public string PolarPercentage;
        public string Positive;
        public string PositivePercentage;
        public string Small;
        public string SmallPercentage;
        public string Sulphur;
        public string SulphurPercentage;
        public string Tiny;
        public string TinyPercentage;

        //public string InteractionAcidic;
        //public string InteractionAcidicPercentage;
        public string InteractionAliphatic;
        public string InteractionAliphaticPercentage;
        public string InteractionAromatic;
        public string InteractionAromaticPercentage;
        public string InteractionCharged;
        public string InteractionChargedPercentage;
        public string InteractionHydrophobic;
        public string InteractionHydrophobicPercentage;
        public string InteractionHydroxylic;
        public string InteractionHydroxylicPercentage;
        public string InteractionNegative;
        public string InteractionNegativePercentage;
        public string InteractionPolar;
        public string InteractionPolarPercentage;
        public string InteractionPositive;
        public string InteractionPositivePercentage;
        public string InteractionSmall;
        public string InteractionSmallPercentage;
        public string InteractionSulphur;
        public string InteractionSulphurPercentage;
        public string InteractionTiny;
        public string InteractionTinyPercentage;

        //public string NonInteractionAcidic;
        //public string NonInteractionAcidicPercentage;
        public string NonInteractionAliphatic;
        public string NonInteractionAliphaticPercentage;
        public string NonInteractionAromatic;
        public string NonInteractionAromaticPercentage;
        public string NonInteractionCharged;
        public string NonInteractionChargedPercentage;
        public string NonInteractionHydrophobic;
        public string NonInteractionHydrophobicPercentage;
        public string NonInteractionHydroxylic;
        public string NonInteractionHydroxylicPercentage;
        public string NonInteractionNegative;
        public string NonInteractionNegativePercentage;
        public string NonInteractionPolar;
        public string NonInteractionPolarPercentage;
        public string NonInteractionPositive;
        public string NonInteractionPositivePercentage;
        public string NonInteractionSmall;
        public string NonInteractionSmallPercentage;
        public string NonInteractionSulphur;
        public string NonInteractionSulphurPercentage;
        public string NonInteractionTiny;
        public string NonInteractionTinyPercentage;

        public string[] ToStringsTreeData()
        {
            var result = new string[]
                {
                     TreeId,
                     PdbId,
                     ChainId,
                     ProteinInterfaceId,
                     Direction,

                     SequenceLength,
                     BindingProteinInterfaceLength,
                     BindingProteinInterfaceStart,
                     BindingProteinInterfaceEnd,

                     BindingProteinInterfaceSecondaryStructure,
                     BindingProteinInterfaceInteractionSecondaryStructure,
                     BindingProteinInterfaceNonInteractionSecondaryStructure,

                     BindingProteinInterfaceSequence1L,
                     BindingProteinInterfaceInteractionSequence1L,
                     BindingProteinInterfaceNonInteractionSequence1L,

                     BindingProteinInterfaceSequence3L,
                     BindingProteinInterfaceInteractionSequence3L,
                     BindingProteinInterfaceNonInteractionSequence3L,

                     BindingProteinInterfaceVectorShort,
                     BindingProteinInterfaceVectorLong,

                     PhysicochemicalGroupNumbers,
                     PhysicochemicalInteractionGroupNumbers,
                     PhysicochemicalNonInteractionGroupNumbers,

                     HydrophobicityGroupNumbers,
                     HydrophobicityInteractionGroupNumbers,
                     HydrophobicityNonInteractionGroupNumbers,

                     PdbSumGroupNumbers,
                     PdbSumInteractionGroupNumbers,
                     PdbSumNonInteractionGroupNumbers,

                     UniProtKbGroupNumbers,
                     UniProtKbInteractionGroupNumbers,
                     UniProtKbNonInteractionGroupNumbers,
                };
            return result;
        }

        public string[] ToStringsSymmetryData()
        {
            var result = new string[]
                {
                     TreeId,
                     PdbId,
                     ChainId,
                     ProteinInterfaceId,
                     Direction,
                     BindingProteinInterfaceLength,

                     BindingProteinInterfaceVectorShortMirrorSymmetryPercentage,
                     BindingProteinInterfaceVectorLongMirrorSymmetryPercentage,

                     BindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage,
                     BindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage,
                     BindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage,

                     BindingProteinInterfaceSequence1LMirrorSymmetryPercentage,
                     BindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage,
                     BindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage,

                     PhysicochemicalGroupNumbersMirrorSymmetryPercentage,
                     PhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage,
                     PhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage,

                     HydrophobicityGroupNumbersMirrorSymmetryPercentage,
                     HydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage,
                     HydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage,

                     PdbSumGroupNumbersMirrorSymmetryPercentage,
                     PdbSumInteractionGroupNumbersMirrorSymmetryPercentage,
                     PdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage,

                     UniProtKbGroupNumbersMirrorSymmetryPercentage,
                     UniProtKbInteractionGroupNumbersMirrorSymmetryPercentage,
                     UniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage,

                     BindingProteinInterfaceVectorShortCloneSymmetryPercentage,
                     BindingProteinInterfaceVectorLongCloneSymmetryPercentage,

                     BindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage,
                     BindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage,
                     BindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage,

                     BindingProteinInterfaceSequence1LCloneSymmetryPercentage,
                     BindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage,
                     BindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage,

                     PhysicochemicalGroupNumbersCloneSymmetryPercentage,
                     PhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage,
                     PhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage,

                     HydrophobicityGroupNumbersCloneSymmetryPercentage,
                     HydrophobicityInteractionGroupNumbersCloneSymmetryPercentage,
                     HydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage,

                     PdbSumGroupNumbersCloneSymmetryPercentage,
                     PdbSumInteractionGroupNumbersCloneSymmetryPercentage,
                     PdbSumNonInteractionGroupNumbersCloneSymmetryPercentage,

                     UniProtKbGroupNumbersCloneSymmetryPercentage,
                     UniProtKbInteractionGroupNumbersCloneSymmetryPercentage,
                     UniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage,
                };
            return result;
        }

        public string[] ToStringsAminoAcidData()
        {
            var result = new string[]
                {
                    TreeId,
                    PdbId,
                    ChainId,
                    ProteinInterfaceId,
                    Direction,
                    BindingProteinInterfaceVectorShort,
                    BindingProteinInterfaceSequence1LPhysicochemicalColorisation,
                    BindingProteinInterfaceSequence1LHydrophobicityColorisation,
                    BindingProteinInterfaceSequence1LPdbSumColorisation,
                    BindingProteinInterfaceSequence1LUniProtKbColorisation,
                    //Acidic,
                    //AcidicPercentage,
                    Aliphatic,
                    AliphaticPercentage,
                    Aromatic,
                    AromaticPercentage,
                    Charged,
                    ChargedPercentage,
                    Hydrophobic,
                    HydrophobicPercentage,
                    Hydroxylic,
                    HydroxylicPercentage,
                    Negative,
                    NegativePercentage,
                    Polar,
                    PolarPercentage,
                    Positive,
                    PositivePercentage,
                    Small,
                    SmallPercentage,
                    Sulphur,
                    SulphurPercentage,
                    Tiny,
                    TinyPercentage,
                };
            return result;
        }

        public string[] ToStringsInteractionAminoAcidData()
        {
            var result = new string[]
                {
                    TreeId,
                    PdbId,
                    ChainId,
                    ProteinInterfaceId,
                    Direction,
                    BindingProteinInterfaceVectorShort,
                    BindingProteinInterfaceInteractionSequence1LPhysicochemicalColorisation,
                    BindingProteinInterfaceInteractionSequence1LHydrophobicityColorisation,
                    BindingProteinInterfaceInteractionSequence1LPdbSumColorisation,
                    BindingProteinInterfaceInteractionSequence1LUniProtKbColorisation,
                    //InteractionAcidic,
                    //InteractionAcidicPercentage,
                    InteractionAliphatic,
                    InteractionAliphaticPercentage,
                    InteractionAromatic,
                    InteractionAromaticPercentage,
                    InteractionCharged,
                    InteractionChargedPercentage,
                    InteractionHydrophobic,
                    InteractionHydrophobicPercentage,
                    InteractionHydroxylic,
                    InteractionHydroxylicPercentage,
                    InteractionNegative,
                    InteractionNegativePercentage,
                    InteractionPolar,
                    InteractionPolarPercentage,
                    InteractionPositive,
                    InteractionPositivePercentage,
                    InteractionSmall,
                    InteractionSmallPercentage,
                    InteractionSulphur,
                    InteractionSulphurPercentage,
                    InteractionTiny,
                    InteractionTinyPercentage,
                };
            return result;
        }

        public string[] ToStringsNonInteractionAminoAcidData()
        {
            var result = new string[]
                {
                    TreeId,
                    PdbId,
                    ChainId,
                    ProteinInterfaceId,
                    Direction,
                    BindingProteinInterfaceVectorShort,
                    BindingProteinInterfaceNonInteractionSequence1LPhysicochemicalColorisation,
                    BindingProteinInterfaceNonInteractionSequence1LHydrophobicityColorisation,
                    BindingProteinInterfaceNonInteractionSequence1LPdbSumColorisation,
                    BindingProteinInterfaceNonInteractionSequence1LUniProtKbColorisation,
                    //NonInteractionAcidic,
                    //NonInteractionAcidicPercentage,
                    NonInteractionAliphatic,
                    NonInteractionAliphaticPercentage,
                    NonInteractionAromatic,
                    NonInteractionAromaticPercentage,
                    NonInteractionCharged,
                    NonInteractionChargedPercentage,
                    NonInteractionHydrophobic,
                    NonInteractionHydrophobicPercentage,
                    NonInteractionHydroxylic,
                    NonInteractionHydroxylicPercentage,
                    NonInteractionNegative,
                    NonInteractionNegativePercentage,
                    NonInteractionPolar,
                    NonInteractionPolarPercentage,
                    NonInteractionPositive,
                    NonInteractionPositivePercentage,
                    NonInteractionSmall,
                    NonInteractionSmallPercentage,
                    NonInteractionSulphur,
                    NonInteractionSulphurPercentage,
                    NonInteractionTiny,
                    NonInteractionTinyPercentage,
                };
            return result;
        }

        public static string[,] SpreadsheetTreeData(List<TreeDataSpreadsheetRecord> treeDataSpreadsheetRecordList)
        {
            if (treeDataSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(treeDataSpreadsheetRecordList));

            var result = new List<string[]>();
            result.Add(Header().ToStringsTreeData());
            foreach (var record in treeDataSpreadsheetRecordList)
            {
                result.Add(record.ToStringsTreeData());
            }
            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }

        public static string[,] SpreadsheetAminoAcidData(List<TreeDataSpreadsheetRecord> treeDataSpreadsheetRecordList)
        {
            if (treeDataSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(treeDataSpreadsheetRecordList));

            var result = new List<string[]>();
            result.Add(Header().ToStringsAminoAcidData());
            foreach (var record in treeDataSpreadsheetRecordList)
            {
                result.Add(record.ToStringsAminoAcidData());
            }
            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }

        public static string[,] SpreadsheetInteractionAminoAcidData(List<TreeDataSpreadsheetRecord> treeDataSpreadsheetRecordList)
        {
            if (treeDataSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(treeDataSpreadsheetRecordList));

            var result = new List<string[]>();
            result.Add(Header().ToStringsInteractionAminoAcidData());
            foreach (var record in treeDataSpreadsheetRecordList)
            {
                result.Add(record.ToStringsInteractionAminoAcidData());
            }
            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }


        public static string[,] SpreadsheetNonInteractionAminoAcidData(List<TreeDataSpreadsheetRecord> treeDataSpreadsheetRecordList)
        {
            if (treeDataSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(treeDataSpreadsheetRecordList));

            var result = new List<string[]>();
            result.Add(Header().ToStringsNonInteractionAminoAcidData());
            foreach (var record in treeDataSpreadsheetRecordList)
            {
                result.Add(record.ToStringsNonInteractionAminoAcidData());
            }
            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }

        public static string[,] SpreadsheetSymmetryData(List<TreeDataSpreadsheetRecord> treeDataSpreadsheetRecordList)
        {
            if (treeDataSpreadsheetRecordList == null) throw new ArgumentNullException(nameof(treeDataSpreadsheetRecordList));

            var result = new List<string[]>();
            result.Add(Header().ToStringsSymmetryData());
            foreach (var record in treeDataSpreadsheetRecordList)
            {
                result.Add(record.ToStringsSymmetryData());
            }
            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }

        public static TreeDataSpreadsheetRecord Header()
        {
            var header = new TreeDataSpreadsheetRecord()
            {
                TreeId = "Tree Id",
                PdbId = "Pdb Id", //1
                ChainId = "Chain Id", //2
                ProteinInterfaceId = "ProteinInterface Id", //3
                Direction = "Dir", //4

                SequenceLength = "Seq Len",
                BindingProteinInterfaceLength = "BS Len", //5
                BindingProteinInterfaceStart = "BS Start", //6
                BindingProteinInterfaceEnd = "BS End", //7

                BindingProteinInterfaceSecondaryStructure = "BS All SS", //8
                BindingProteinInterfaceInteractionSecondaryStructure = "BS Interactions SS", //9
                BindingProteinInterfaceNonInteractionSecondaryStructure = "BS Non Interactions SS", //9

                BindingProteinInterfaceSequence1L = "BS Seq 1L", //10
                BindingProteinInterfaceInteractionSequence1L = "BS Seq Interactions 1L", //10
                BindingProteinInterfaceNonInteractionSequence1L = "BS Seq Non Interactions 1L", //10

                BindingProteinInterfaceSequence3L = "BS Seq 3L", //11
                BindingProteinInterfaceInteractionSequence3L = "BS Seq Interactions 3L", //11
                BindingProteinInterfaceNonInteractionSequence3L = "BS Seq Non Interactions 3L", //11

                BindingProteinInterfaceVectorShort = "BS Vector Short", //12
                BindingProteinInterfaceVectorLong = "BS Vector Long", //13

                BindingProteinInterfaceVectorShortMirrorSymmetryPercentage = "%  Mirror Symmetry: BS Vector Short", //14
                BindingProteinInterfaceVectorLongMirrorSymmetryPercentage = "%  Mirror Symmetry: BS Vector Long", //15

                BindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage = "%  Mirror Symmetry: SS",
                BindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage = "%  Mirror Symmetry: SS Interaction",
                BindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage = "%  Mirror Symmetry: SS Non Interaction",

                BindingProteinInterfaceSequence1LMirrorSymmetryPercentage = "%  Mirror Symmetry: BS Seq 1L",
                BindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage = "%  Mirror Symmetry: BS Seq Interactions 1L",
                BindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage = "%  Mirror Symmetry: BS Seq Non Interactions 1L",

                PhysicochemicalGroupNumbersMirrorSymmetryPercentage = "% Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Group Numbers",//16
                PhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Interactions Group Numbers",//17
                PhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Non Interactions Group Numbers",//17

                HydrophobicityGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Group Numbers",//18
                HydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Interactions Group Numbers",//19
                HydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Non Interactions Group Numbers",//19

                PdbSumGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Group Numbers",//20
                PdbSumInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Interactions Group Numbers",//21
                PdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Non Interactions Group Numbers",//21

                UniProtKbGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Group Numbers",//20
                UniProtKbInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Interactions Group Numbers",//21
                UniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage = "%  Mirror Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Non Interactions Group Numbers",//21

                BindingProteinInterfaceVectorShortCloneSymmetryPercentage = "%  Clone Symmetry: BS Vector Short", //14
                BindingProteinInterfaceVectorLongCloneSymmetryPercentage = "%  Clone Symmetry: BS Vector Long", //15

                BindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage = "%  Clone Symmetry: SS",
                BindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage = "%  Clone Symmetry: SS Interaction",
                BindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage = "%  Clone Symmetry: SS Non Interaction",

                BindingProteinInterfaceSequence1LCloneSymmetryPercentage = "%  Clone Symmetry: BS Seq 1L",
                BindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage = "%  Clone Symmetry: BS Seq Interactions 1L",
                BindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage = "%  Clone Symmetry: BS Seq Non Interactions 1L",

                PhysicochemicalGroupNumbersCloneSymmetryPercentage = "% Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Group Numbers",//16
                PhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Interactions Group Numbers",//17
                PhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Non Interactions Group Numbers",//17

                HydrophobicityGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Group Numbers",//18
                HydrophobicityInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Interactions Group Numbers",//19
                HydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Non Interactions Group Numbers",//19

                PdbSumGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Group Numbers",//20
                PdbSumInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Interactions Group Numbers",//21
                PdbSumNonInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Non Interactions Group Numbers",//21

                UniProtKbGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Group Numbers",//20
                UniProtKbInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Interactions Group Numbers",//21
                UniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage = "%  Clone Symmetry: " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Non Interactions Group Numbers",//21


                PhysicochemicalGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Group Numbers",//16
                PhysicochemicalInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Interactions Group Numbers",//17
                PhysicochemicalNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " Non Interactions Group Numbers",//17

                HydrophobicityGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Group Numbers",//18
                HydrophobicityInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Interactions Group Numbers",//19
                HydrophobicityNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " Non Interactions Group Numbers",//19

                PdbSumGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Group Numbers",//20
                PdbSumInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Interactions Group Numbers",//21
                PdbSumNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " Non Interactions Group Numbers",//21

                UniProtKbGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Group Numbers",//20
                UniProtKbInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Interactions Group Numbers",//21
                UniProtKbNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " Non Interactions Group Numbers",//21

                BindingProteinInterfaceSequence1LPhysicochemicalColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical),//6
                BindingProteinInterfaceSequence1LHydrophobicityColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity),//7
                BindingProteinInterfaceSequence1LPdbSumColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum),//8
                BindingProteinInterfaceSequence1LUniProtKbColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb),//8

                BindingProteinInterfaceInteractionSequence1LPhysicochemicalColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical),//6
                BindingProteinInterfaceInteractionSequence1LHydrophobicityColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity),//7
                BindingProteinInterfaceInteractionSequence1LPdbSumColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum),//8
                BindingProteinInterfaceInteractionSequence1LUniProtKbColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb),//8

                BindingProteinInterfaceNonInteractionSequence1LPhysicochemicalColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical),//6
                BindingProteinInterfaceNonInteractionSequence1LHydrophobicityColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity),//7
                BindingProteinInterfaceNonInteractionSequence1LPdbSumColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum),//8
                BindingProteinInterfaceNonInteractionSequence1LUniProtKbColorisation = AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb),//8


                //Acidic = "Acidic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Acidic = true }) + "]",//9
                //AcidicPercentage = "% Acidic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Acidic = true }) + "]",

                Aliphatic = "Aliphatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aliphatic = true }) + "]",
                AliphaticPercentage = "% Aliphatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aliphatic = true }) + "]",

                Aromatic = "Aromatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aromatic = true }) + "]",
                AromaticPercentage = "% Aromatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aromatic = true }) + "]",

                Charged = "Charged [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Charged = true }) + "]",
                ChargedPercentage = "% Charged [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Charged = true }) + "]",

                Hydrophobic = "Hydrophobic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydrophobic = true }) + "]",
                HydrophobicPercentage = "% Hydrophobic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydrophobic = true }) + "]",

                Hydroxylic = "Hydroxylic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydroxylic = true }) + "]",
                HydroxylicPercentage = "% Hydroxylic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydroxylic = true }) + "]",

                Negative = "Negative [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Negative = true }) + "]",
                NegativePercentage = "% Negative [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Negative = true }) + "]",

                Polar = "Polar [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Polar = true }) + "]",
                PolarPercentage = "% Polar [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Polar = true }) + "]",

                Positive = "Positive [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Positive = true }) + "]",
                PositivePercentage = "% Positive [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Positive = true }) + "]",

                Small = "Small [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Small = true }) + "]",
                SmallPercentage = "% Small [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Small = true }) + "]",

                Sulphur = "Sulphur [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Sulphur = true }) + "]",
                SulphurPercentage = "% Sulphur [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Sulphur = true }) + "]",

                Tiny = "Tiny [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Tiny = true }) + "]",
                TinyPercentage = "% Tiny [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Tiny = true }) + "]",


                //InteractionAcidic = "Acidic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Acidic = true }) + "]",//9
                //InteractionAcidicPercentage = "% Acidic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Acidic = true }) + "]",

                InteractionAliphatic = "Aliphatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aliphatic = true }) + "]",
                InteractionAliphaticPercentage = "% Aliphatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aliphatic = true }) + "]",

                InteractionAromatic = "Aromatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aromatic = true }) + "]",
                InteractionAromaticPercentage = "% Aromatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aromatic = true }) + "]",

                InteractionCharged = "Charged [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Charged = true }) + "]",
                InteractionChargedPercentage = "% Charged [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Charged = true }) + "]",

                InteractionHydrophobic = "Hydrophobic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydrophobic = true }) + "]",
                InteractionHydrophobicPercentage = "% Hydrophobic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydrophobic = true }) + "]",

                InteractionHydroxylic = "Hydroxylic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydroxylic = true }) + "]",
                InteractionHydroxylicPercentage = "% Hydroxylic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydroxylic = true }) + "]",

                InteractionNegative = "Negative [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Negative = true }) + "]",
                InteractionNegativePercentage = "% Negative [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Negative = true }) + "]",

                InteractionPolar = "Polar [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Polar = true }) + "]",
                InteractionPolarPercentage = "% Polar [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Polar = true }) + "]",

                InteractionPositive = "Positive [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Positive = true }) + "]",
                InteractionPositivePercentage = "% Positive [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Positive = true }) + "]",

                InteractionSmall = "Small [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Small = true }) + "]",
                InteractionSmallPercentage = "% Small [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Small = true }) + "]",

                InteractionSulphur = "Sulphur [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Sulphur = true }) + "]",
                InteractionSulphurPercentage = "% Sulphur [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Sulphur = true }) + "]",

                InteractionTiny = "Tiny [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Tiny = true }) + "]",
                InteractionTinyPercentage = "% Tiny [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Tiny = true }) + "]",


                //NonInteractionAcidic = "Acidic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Acidic = true }) + "]",//9
                //NonInteractionAcidicPercentage = "% Acidic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Acidic = true }) + "]",

                NonInteractionAliphatic = "Aliphatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aliphatic = true }) + "]",
                NonInteractionAliphaticPercentage = "% Aliphatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aliphatic = true }) + "]",

                NonInteractionAromatic = "Aromatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aromatic = true }) + "]",
                NonInteractionAromaticPercentage = "% Aromatic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Aromatic = true }) + "]",

                NonInteractionCharged = "Charged [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Charged = true }) + "]",
                NonInteractionChargedPercentage = "% Charged [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Charged = true }) + "]",

                NonInteractionHydrophobic = "Hydrophobic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydrophobic = true }) + "]",
                NonInteractionHydrophobicPercentage = "% Hydrophobic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydrophobic = true }) + "]",

                NonInteractionHydroxylic = "Hydroxylic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydroxylic = true }) + "]",
                NonInteractionHydroxylicPercentage = "% Hydroxylic [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Hydroxylic = true }) + "]",

                NonInteractionNegative = "Negative [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Negative = true }) + "]",
                NonInteractionNegativePercentage = "% Negative [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Negative = true }) + "]",

                NonInteractionPolar = "Polar [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Polar = true }) + "]",
                NonInteractionPolarPercentage = "% Polar [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Polar = true }) + "]",

                NonInteractionPositive = "Positive [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Positive = true }) + "]",
                NonInteractionPositivePercentage = "% Positive [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Positive = true }) + "]",

                NonInteractionSmall = "Small [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Small = true }) + "]",
                NonInteractionSmallPercentage = "% Small [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Small = true }) + "]",

                NonInteractionSulphur = "Sulphur [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Sulphur = true }) + "]",
                NonInteractionSulphurPercentage = "% Sulphur [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Sulphur = true }) + "]",

                NonInteractionTiny = "Tiny [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Tiny = true }) + "]",
                NonInteractionTinyPercentage = "% Tiny [" + AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Tiny = true }) + "]",
            };

            if (header.ToStringsTreeData().Length != header.ToStringsTreeData().Distinct().Count())
            {
                throw new ArgumentOutOfRangeException(nameof(header));
            }

            if (header.ToStringsSymmetryData().Length != header.ToStringsSymmetryData().Distinct().Count())
            {
                throw new ArgumentOutOfRangeException(nameof(header));
            }

            if (header.ToStringsAminoAcidData().Length != header.ToStringsAminoAcidData().Distinct().Count())
            {
                throw new ArgumentOutOfRangeException(nameof(header));
            }

            if (header.ToStringsInteractionAminoAcidData().Length != header.ToStringsInteractionAminoAcidData().Distinct().Count())
            {
                throw new ArgumentOutOfRangeException(nameof(header));
            }

            if (header.ToStringsNonInteractionAminoAcidData().Length != header.ToStringsNonInteractionAminoAcidData().Distinct().Count())
            {
                throw new ArgumentOutOfRangeException(nameof(header));
            }

            return header;
        }


        public static List<TreeDataSpreadsheetRecord> TreeDataSpreadsheetRecords(string[] pdbFilesArray, List<string> pdbIdList, List<ISequence> seqList, List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, string outputFolderName, string spreadsheetName, List<string> finalTreeLeafOrderList, FileExistsHandler.FileExistsOptions fileExistsOptions, ProgressActionSet progressActionSet)
        {
            if (pdbFilesArray == null || pdbFilesArray.Length == 0) throw new ArgumentNullException(nameof(pdbFilesArray));
            if (pdbIdList == null || pdbIdList.Count == 0) throw new ArgumentNullException(nameof(pdbIdList));
            if (seqList == null || seqList.Count == 0) throw new ArgumentNullException(nameof(seqList));
            if (vectorProteinInterfaceWholeList == null || vectorProteinInterfaceWholeList.Count == 0) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (String.IsNullOrWhiteSpace(outputFolderName)) throw new ArgumentNullException(nameof(outputFolderName));
            if (String.IsNullOrWhiteSpace(spreadsheetName)) throw new ArgumentNullException(nameof(spreadsheetName));
            if (finalTreeLeafOrderList == null || finalTreeLeafOrderList.Count == 0) throw new ArgumentNullException(nameof(finalTreeLeafOrderList));

            var result = new List<TreeDataSpreadsheetRecord>();

            var itemsCompleted = 0;
            var itemsTotal = vectorProteinInterfaceWholeList.Count;
            var startTicks = DateTime.Now.Ticks;

            for (var index = 0; index < vectorProteinInterfaceWholeList.Count; index++)
            {
                var vectorProteinInterfaceWhole = vectorProteinInterfaceWholeList[index];

                //var pdbFilename = pdbFilesArray.FirstOrDefault(a => a != null && Path.GetFileNameWithoutExtension(a).IndexOf(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId, StringComparison.OrdinalIgnoreCase) > -1);

                var aminoAcids1L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids1L();
                var aminoAcids3L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids3L();

                double totalProteinInterfaceLen = aminoAcids1L.Length;

                var interactionAminoAcids1L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids1L(VectorProteinInterfaceWhole.ProteinInterfaceReadOption.Interactions);
                var interactionAminoAcids3L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids3L(VectorProteinInterfaceWhole.ProteinInterfaceReadOption.Interactions);

                var nonInteractionAminoAcids1L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids1L(VectorProteinInterfaceWhole.ProteinInterfaceReadOption.NonInteractions);
                var nonInteractionAminoAcids3L = vectorProteinInterfaceWhole.ProteinInterfaceAminoAcids3L(VectorProteinInterfaceWhole.ProteinInterfaceReadOption.NonInteractions);

                var interactionBools = vectorProteinInterfaceWhole.InteractionBools();

                var bsVectorShort = string.Join("", interactionBools.Select(Convert.ToInt32)) + "+" + (vectorProteinInterfaceWhole.VectorProteinInterfacePartList.Count(a => a.InteractionToNonProteinInterface) > 0 ? 1 : 0);
                var bsVectorLong = string.Join(" ", vectorProteinInterfaceWhole.VectorProteinInterfacePartList.Select(a => string.Join("", a.InteractionFlagBools.Select(Convert.ToInt32)) + "+" + Convert.ToInt32(a.InteractionToNonProteinInterface)).ToList());

                double totalProteinInterfaceInteractions = interactionAminoAcids1L.Length;
                double totalProteinInterfaceNonInteractions = nonInteractionAminoAcids1L.Length;


                // aminoAcids1L.Where((a, i) => interactionBools[i]).Count();
                //string interactionAminoAcids = String.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).ToList());

                //var secStruct = ProteinInterfaceSecondaryStructureLoader.ProteinInterfaceSecondaryStructure(pdbFilename, SpreadsheetFileHandler.AlphabetLetterRollOver(vectorProteinInterfaceWhole.FullProteinInterfaceId.ChainId), vectorProteinInterfaceWhole.FirstResidueSequenceIndex, vectorProteinInterfaceWhole.LastResidueSequenceIndex, vectorProteinInterfaceWhole.ReversedSequence);
                var secondaryStructure = vectorProteinInterfaceWhole.SecondaryStructure;

                string interactionSecondaryStructure = "";
                string nonInteractionSecondaryStructure = "";

                for (var i = 0; i < secondaryStructure.Length; i++)
                {
                    var isInteraction = interactionBools[i];

                    if (isInteraction)
                    {
                        interactionSecondaryStructure += secondaryStructure[i];
                    }
                    else
                    {
                        nonInteractionSecondaryStructure += secondaryStructure[i];
                    }
                }

                var strVector = new AminoAcidProperties<string>()
                {
                    //Acidic = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Acidic ? 1 : 0).ToList()),
                    Aliphatic = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aliphatic ? 1 : 0).ToList()),
                    Aromatic = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aromatic ? 1 : 0).ToList()),
                    Charged = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Charged ? 1 : 0).ToList()),
                    Hydrophobic = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydrophobic ? 1 : 0).ToList()),
                    Hydroxylic = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydroxylic ? 1 : 0).ToList()),
                    Negative = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Negative ? 1 : 0).ToList()),
                    Polar = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Polar ? 1 : 0).ToList()),
                    Positive = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Positive ? 1 : 0).ToList()),
                    Small = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Small ? 1 : 0).ToList()),
                    Sulphur = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Sulphur ? 1 : 0).ToList()),
                    Tiny = string.Join("", aminoAcids1L.Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Tiny ? 1 : 0).ToList())
                };

                var strInteractionVector = new AminoAcidProperties<string>()
                {
                    //Acidic = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Acidic ? 1 : 0).ToList()),
                    Aliphatic = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aliphatic ? 1 : 0).ToList()),
                    Aromatic = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aromatic ? 1 : 0).ToList()),
                    Charged = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Charged ? 1 : 0).ToList()),
                    Hydrophobic = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydrophobic ? 1 : 0).ToList()),
                    Hydroxylic = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydroxylic ? 1 : 0).ToList()),
                    Negative = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Negative ? 1 : 0).ToList()),
                    Polar = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Polar ? 1 : 0).ToList()),
                    Positive = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Positive ? 1 : 0).ToList()),
                    Small = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Small ? 1 : 0).ToList()),
                    Sulphur = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Sulphur ? 1 : 0).ToList()),
                    Tiny = string.Join("", aminoAcids1L.Where((a, i) => interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Tiny ? 1 : 0).ToList()),
                };


                var strNonInteractionVector = new AminoAcidProperties<string>()
                {
                    //Acidic = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Acidic ? 1 : 0).ToList()),
                    Aliphatic = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aliphatic ? 1 : 0).ToList()),
                    Aromatic = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aromatic ? 1 : 0).ToList()),
                    Charged = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Charged ? 1 : 0).ToList()),
                    Hydrophobic = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydrophobic ? 1 : 0).ToList()),
                    Hydroxylic = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydroxylic ? 1 : 0).ToList()),
                    Negative = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Negative ? 1 : 0).ToList()),
                    Polar = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Polar ? 1 : 0).ToList()),
                    Positive = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Positive ? 1 : 0).ToList()),
                    Small = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Small ? 1 : 0).ToList()),
                    Sulphur = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Sulphur ? 1 : 0).ToList()),
                    Tiny = string.Join("", aminoAcids1L.Where((a, i) => !interactionBools[i]).Select(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Tiny ? 1 : 0).ToList()),
                };

                var dblPercentage = new AminoAcidProperties<double>()
                {
                    //Acidic = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Acidic) / totalProteinInterfaceLen) * 100,
                    Aliphatic = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aliphatic) / totalProteinInterfaceLen) * 100,
                    Aromatic = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aromatic) / totalProteinInterfaceLen) * 100,
                    Charged = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Charged) / totalProteinInterfaceLen) * 100,
                    Hydrophobic = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydrophobic) / totalProteinInterfaceLen) * 100,
                    Hydroxylic = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydroxylic) / totalProteinInterfaceLen) * 100,
                    Negative = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Negative) / totalProteinInterfaceLen) * 100,
                    Polar = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Polar) / totalProteinInterfaceLen) * 100,
                    Positive = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Positive) / totalProteinInterfaceLen) * 100,
                    Small = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Small) / totalProteinInterfaceLen) * 100,
                    Sulphur = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Sulphur) / totalProteinInterfaceLen) * 100,
                    Tiny = ((double)aminoAcids1L.Count(a => AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Tiny) / totalProteinInterfaceLen) * 100,
                };


                var dblPercentageInteraction = new AminoAcidProperties<double>()
                {
                    //Acidic = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Acidic).Count() / totalProteinInterfaceInteractions) * 100,
                    Aliphatic = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aliphatic).Count() / totalProteinInterfaceInteractions) * 100,
                    Aromatic = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aromatic).Count() / totalProteinInterfaceInteractions) * 100,
                    Charged = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Charged).Count() / totalProteinInterfaceInteractions) * 100,
                    Hydrophobic = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydrophobic).Count() / totalProteinInterfaceInteractions) * 100,
                    Hydroxylic = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydroxylic).Count() / totalProteinInterfaceInteractions) * 100,
                    Negative = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Negative).Count() / totalProteinInterfaceInteractions) * 100,
                    Polar = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Polar).Count() / totalProteinInterfaceInteractions) * 100,
                    Positive = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Positive).Count() / totalProteinInterfaceInteractions) * 100,
                    Small = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Small).Count() / totalProteinInterfaceInteractions) * 100,
                    Sulphur = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Sulphur).Count() / totalProteinInterfaceInteractions) * 100,
                    Tiny = ((double)aminoAcids1L.Where((a, i) => interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Tiny).Count() / totalProteinInterfaceInteractions) * 100,
                };

                var dblPercentageNonInteraction = new AminoAcidProperties<double>()
                {
                    //Acidic = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Acidic).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Aliphatic = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aliphatic).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Aromatic = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Aromatic).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Charged = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Charged).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Hydrophobic = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydrophobic).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Hydroxylic = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Hydroxylic).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Negative = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Negative).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Polar = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Polar).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Positive = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Positive).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Small = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Small).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Sulphur = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Sulphur).Count() / totalProteinInterfaceNonInteractions) * 100,
                    Tiny = ((double)aminoAcids1L.Where((a, i) => !interactionBools[i] && AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject(a).Tiny).Count() / totalProteinInterfaceNonInteractions) * 100,
                };

                var physicochemicalGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(aminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical);
                var physicochemicalInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(interactionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical);
                var physicochemicalNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(nonInteractionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical);

                var hydrophobicityGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(aminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity);
                var hydrophobicityInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(interactionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity);
                var hydrophobicityNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(nonInteractionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity);

                var pdbSumGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(aminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum);
                var pdbSumInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(interactionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum);
                var pdbSumNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(nonInteractionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum);

                var uniProtKbGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(aminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb);
                var uniProtKbInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(interactionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb);
                var uniProtKbNonInteractionGroupNumbers = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidStringToSubgroupNumberString(nonInteractionAminoAcids1L, AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb);

                var record = new TreeDataSpreadsheetRecord()
                {
                    TreeId = VectorController.VectorProteinInterfaceWholeTreeHeader(vectorProteinInterfaceWhole),
                    PdbId = vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId,
                    ChainId = SpreadsheetFileHandler.AlphabetLetterRollOver(vectorProteinInterfaceWhole.FullProteinInterfaceId.ChainId),
                    ProteinInterfaceId = SpreadsheetFileHandler.AlphabetLetterRollOver(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinInterfaceId),
                    Direction = vectorProteinInterfaceWhole.ReversedSequence ? "Rev" : "Fwd",
                    SequenceLength = "" + vectorProteinInterfaceWhole.FullSequenceLength,
                    BindingProteinInterfaceLength = "" + vectorProteinInterfaceWhole.ProteinInterfaceLength,
                    BindingProteinInterfaceStart = "" + vectorProteinInterfaceWhole.FirstResidueSequenceIndex,
                    BindingProteinInterfaceEnd = "" + vectorProteinInterfaceWhole.LastResidueSequenceIndex,
                    BindingProteinInterfaceSecondaryStructure = secondaryStructure,
                    BindingProteinInterfaceInteractionSecondaryStructure = interactionSecondaryStructure,
                    BindingProteinInterfaceNonInteractionSecondaryStructure = nonInteractionSecondaryStructure,

                    BindingProteinInterfaceSequence1L = aminoAcids1L,
                    BindingProteinInterfaceSequence3L = aminoAcids3L,

                    BindingProteinInterfaceInteractionSequence1L = interactionAminoAcids1L,
                    BindingProteinInterfaceInteractionSequence3L = interactionAminoAcids3L,

                    BindingProteinInterfaceNonInteractionSequence1L = nonInteractionAminoAcids1L,
                    BindingProteinInterfaceNonInteractionSequence3L = nonInteractionAminoAcids3L,

                    BindingProteinInterfaceVectorShort = bsVectorShort,
                    BindingProteinInterfaceVectorLong = bsVectorLong,

                    PhysicochemicalGroupNumbers = physicochemicalGroupNumbers,
                    PhysicochemicalInteractionGroupNumbers = physicochemicalInteractionGroupNumbers,
                    PhysicochemicalNonInteractionGroupNumbers = physicochemicalNonInteractionGroupNumbers,

                    HydrophobicityGroupNumbers = hydrophobicityGroupNumbers,
                    HydrophobicityInteractionGroupNumbers = hydrophobicityInteractionGroupNumbers,
                    HydrophobicityNonInteractionGroupNumbers = hydrophobicityNonInteractionGroupNumbers,

                    PdbSumGroupNumbers = pdbSumGroupNumbers,
                    PdbSumInteractionGroupNumbers = pdbSumInteractionGroupNumbers,
                    PdbSumNonInteractionGroupNumbers = pdbSumNonInteractionGroupNumbers,

                    UniProtKbGroupNumbers = uniProtKbGroupNumbers,
                    UniProtKbInteractionGroupNumbers = uniProtKbInteractionGroupNumbers,
                    UniProtKbNonInteractionGroupNumbers = uniProtKbNonInteractionGroupNumbers,

                    BindingProteinInterfaceVectorShortMirrorSymmetryPercentage = "" + InteractionVectorSymmetry.VectorSymmetryPercentage(bsVectorShort),
                    BindingProteinInterfaceVectorLongMirrorSymmetryPercentage = "" + InteractionVectorSymmetry.VectorSymmetryPercentage(bsVectorLong),
                    
                    BindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(secondaryStructure),
                    BindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(interactionSecondaryStructure),
                    BindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(nonInteractionSecondaryStructure),

                    BindingProteinInterfaceSequence1LMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(aminoAcids1L),
                    BindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(interactionAminoAcids1L),
                    BindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(nonInteractionAminoAcids1L),

                    PhysicochemicalGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(physicochemicalGroupNumbers),
                    PhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(physicochemicalInteractionGroupNumbers),
                    PhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(physicochemicalNonInteractionGroupNumbers),

                    HydrophobicityGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(hydrophobicityGroupNumbers),
                    HydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(hydrophobicityInteractionGroupNumbers),
                    HydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(hydrophobicityNonInteractionGroupNumbers),

                    PdbSumGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(pdbSumGroupNumbers),
                    PdbSumInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(pdbSumInteractionGroupNumbers),
                    PdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(pdbSumNonInteractionGroupNumbers),

                    UniProtKbGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(uniProtKbGroupNumbers),
                    UniProtKbInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(uniProtKbInteractionGroupNumbers),
                    UniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(uniProtKbNonInteractionGroupNumbers),

                    BindingProteinInterfaceVectorShortCloneSymmetryPercentage = "" + InteractionVectorSymmetry.VectorSymmetryPercentage(bsVectorShort, false),
                    BindingProteinInterfaceVectorLongCloneSymmetryPercentage = "" + InteractionVectorSymmetry.VectorSymmetryPercentage(bsVectorLong, false),

                    BindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(secondaryStructure, false),
                    BindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(interactionSecondaryStructure, false),
                    BindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(nonInteractionSecondaryStructure, false),

                    BindingProteinInterfaceSequence1LCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(aminoAcids1L, false),
                    BindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(interactionAminoAcids1L, false),
                    BindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(nonInteractionAminoAcids1L, false),

                    PhysicochemicalGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(physicochemicalGroupNumbers, false),
                    PhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(physicochemicalInteractionGroupNumbers, false),
                    PhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(physicochemicalNonInteractionGroupNumbers, false),

                    HydrophobicityGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(hydrophobicityGroupNumbers, false),
                    HydrophobicityInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(hydrophobicityInteractionGroupNumbers, false),
                    HydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(hydrophobicityNonInteractionGroupNumbers, false),

                    PdbSumGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(pdbSumGroupNumbers, false),
                    PdbSumInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(pdbSumInteractionGroupNumbers, false),
                    PdbSumNonInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(pdbSumNonInteractionGroupNumbers, false),

                    UniProtKbGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(uniProtKbGroupNumbers, false),
                    UniProtKbInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(uniProtKbInteractionGroupNumbers, false),
                    UniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage = "" + StringSymmetry.StringSymmetryPercentage(uniProtKbNonInteractionGroupNumbers, false),

                    BindingProteinInterfaceSequence1LPhysicochemicalColorisation = aminoAcids1L,
                    BindingProteinInterfaceSequence1LHydrophobicityColorisation = aminoAcids1L,
                    BindingProteinInterfaceSequence1LPdbSumColorisation = aminoAcids1L,
                    BindingProteinInterfaceSequence1LUniProtKbColorisation = aminoAcids1L,

                    BindingProteinInterfaceInteractionSequence1LPhysicochemicalColorisation = interactionAminoAcids1L,
                    BindingProteinInterfaceInteractionSequence1LHydrophobicityColorisation = interactionAminoAcids1L,
                    BindingProteinInterfaceInteractionSequence1LPdbSumColorisation = interactionAminoAcids1L,
                    BindingProteinInterfaceInteractionSequence1LUniProtKbColorisation = interactionAminoAcids1L,

                    BindingProteinInterfaceNonInteractionSequence1LPhysicochemicalColorisation = nonInteractionAminoAcids1L,
                    BindingProteinInterfaceNonInteractionSequence1LHydrophobicityColorisation = nonInteractionAminoAcids1L,
                    BindingProteinInterfaceNonInteractionSequence1LPdbSumColorisation = nonInteractionAminoAcids1L,
                    BindingProteinInterfaceNonInteractionSequence1LUniProtKbColorisation = nonInteractionAminoAcids1L,

                    //Acidic = strVector.Acidic,
                    //AcidicPercentage = $"{dblPercentage.Acidic:00.00}",
                    Aliphatic = strVector.Aliphatic,
                    AliphaticPercentage = $"{dblPercentage.Aliphatic:00.00}",
                    Aromatic = strVector.Aromatic,
                    AromaticPercentage = $"{dblPercentage.Aromatic:00.00}",
                    Charged = strVector.Charged,
                    ChargedPercentage = $"{dblPercentage.Charged:00.00}",
                    Hydrophobic = strVector.Hydrophobic,
                    HydrophobicPercentage = $"{dblPercentage.Hydrophobic:00.00}",
                    Hydroxylic = strVector.Hydroxylic,
                    HydroxylicPercentage = $"{dblPercentage.Hydroxylic:00.00}",
                    Negative = strVector.Negative,
                    NegativePercentage = $"{dblPercentage.Negative:00.00}",
                    Polar = strVector.Polar,
                    PolarPercentage = $"{dblPercentage.Polar:00.00}",
                    Positive = strVector.Positive,
                    PositivePercentage = $"{dblPercentage.Positive:00.00}",
                    Small = strVector.Small,
                    SmallPercentage = $"{dblPercentage.Small:00.00}",
                    Sulphur = strVector.Sulphur,
                    SulphurPercentage = $"{dblPercentage.Sulphur:00.00}",
                    Tiny = strVector.Tiny,
                    TinyPercentage = $"{dblPercentage.Tiny:00.00}",

                    //InteractionAcidic = strInteractionVector.Acidic,
                    //InteractionAcidicPercentage = $"{dblPercentageInteraction.Acidic:00.00}",
                    InteractionAliphatic = strInteractionVector.Aliphatic,
                    InteractionAliphaticPercentage = $"{dblPercentageInteraction.Aliphatic:00.00}",
                    InteractionAromatic = strInteractionVector.Aromatic,
                    InteractionAromaticPercentage = $"{dblPercentageInteraction.Aromatic:00.00}",
                    InteractionCharged = strInteractionVector.Charged,
                    InteractionChargedPercentage = $"{dblPercentageInteraction.Charged:00.00}",
                    InteractionHydrophobic = strInteractionVector.Hydrophobic,
                    InteractionHydrophobicPercentage = $"{dblPercentageInteraction.Hydrophobic:00.00}",
                    InteractionHydroxylic = strInteractionVector.Hydroxylic,
                    InteractionHydroxylicPercentage = $"{dblPercentageInteraction.Hydroxylic:00.00}",
                    InteractionNegative = strInteractionVector.Negative,
                    InteractionNegativePercentage = $"{dblPercentageInteraction.Negative:00.00}",
                    InteractionPolar = strInteractionVector.Polar,
                    InteractionPolarPercentage = $"{dblPercentageInteraction.Polar:00.00}",
                    InteractionPositive = strInteractionVector.Positive,
                    InteractionPositivePercentage = $"{dblPercentageInteraction.Positive:00.00}",
                    InteractionSmall = strInteractionVector.Small,
                    InteractionSmallPercentage = $"{dblPercentageInteraction.Small:00.00}",
                    InteractionSulphur = strInteractionVector.Sulphur,
                    InteractionSulphurPercentage = $"{dblPercentageInteraction.Sulphur:00.00}",
                    InteractionTiny = strInteractionVector.Tiny,
                    InteractionTinyPercentage = $"{dblPercentageInteraction.Tiny:00.00}",

                    //NonInteractionAcidic = strNonInteractionVector.Acidic,
                    //NonInteractionAcidicPercentage = $"{dblPercentageNonInteraction.Acidic:00.00}",
                    NonInteractionAliphatic = strNonInteractionVector.Aliphatic,
                    NonInteractionAliphaticPercentage = $"{dblPercentageNonInteraction.Aliphatic:00.00}",
                    NonInteractionAromatic = strNonInteractionVector.Aromatic,
                    NonInteractionAromaticPercentage = $"{dblPercentageNonInteraction.Aromatic:00.00}",
                    NonInteractionCharged = strNonInteractionVector.Charged,
                    NonInteractionChargedPercentage = $"{dblPercentageNonInteraction.Charged:00.00}",
                    NonInteractionHydrophobic = strNonInteractionVector.Hydrophobic,
                    NonInteractionHydrophobicPercentage = $"{dblPercentageNonInteraction.Hydrophobic:00.00}",
                    NonInteractionHydroxylic = strNonInteractionVector.Hydroxylic,
                    NonInteractionHydroxylicPercentage = $"{dblPercentageNonInteraction.Hydroxylic:00.00}",
                    NonInteractionNegative = strNonInteractionVector.Negative,
                    NonInteractionNegativePercentage = $"{dblPercentageNonInteraction.Negative:00.00}",
                    NonInteractionPolar = strNonInteractionVector.Polar,
                    NonInteractionPolarPercentage = $"{dblPercentageNonInteraction.Polar:00.00}",
                    NonInteractionPositive = strNonInteractionVector.Positive,
                    NonInteractionPositivePercentage = $"{dblPercentageNonInteraction.Positive:00.00}",
                    NonInteractionSmall = strNonInteractionVector.Small,
                    NonInteractionSmallPercentage = $"{dblPercentageNonInteraction.Small:00.00}",
                    NonInteractionSulphur = strNonInteractionVector.Sulphur,
                    NonInteractionSulphurPercentage = $"{dblPercentageNonInteraction.Sulphur:00.00}",
                    NonInteractionTiny = strNonInteractionVector.Tiny,
                    NonInteractionTinyPercentage = $"{dblPercentageNonInteraction.Tiny:00.00}",

                };
                result.Add(record);

                itemsCompleted++;
                ProgressActionSet.ProgressAction(1, progressActionSet);
                ProgressActionSet.EstimatedTimeRemainingAction(startTicks, itemsCompleted, itemsTotal, progressActionSet);
            }

            return result;
        }

    }

}
