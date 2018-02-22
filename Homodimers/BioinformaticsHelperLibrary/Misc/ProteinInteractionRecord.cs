//-----------------------------------------------------------------------
// <copyright file="ProteinInteractionRecord.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.Measurements;
using BioinformaticsHelperLibrary.Spreadsheets;

namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    ///     This class stores information about one interaction, and allows the information to be output in tsv/csv format.
    /// </summary>
    public class ProteinInteractionRecord
    {
        public AtomPair InteractingAtomPair = null;
        public string ProteinId = "";
        public int ProteinInteractionCount = 0;
        public string ProteinInterfaceIdA = "";
        public string ProteinInterfaceIdB = "";

        public ProteinInteractionRecord()
        {
        }

        public ProteinInteractionRecord(string proteinId, int proteinInteractionCount, AtomPair atomPair)
        {
            ProteinId = proteinId;
            ProteinInteractionCount = proteinInteractionCount;
            InteractingAtomPair = atomPair;
        }

        public static SpreadsheetCell[] TsvColumnHeadersRow()
        {
            var columnHeadersRow = new[]
            {
                new SpreadsheetCell("Protein ID"),
                new SpreadsheetCell("Atomic Distance (Angstroms)"),
                new SpreadsheetCell("-"),
                new SpreadsheetCell("A: Chain ID"),
                new SpreadsheetCell("A: ProteinInterface ID"),
                new SpreadsheetCell("A: Residue Sequence Index"),
                new SpreadsheetCell("A: Atom"),
                new SpreadsheetCell("A: Amino Acid"),
                new SpreadsheetCell("A: AA Physicochemical Group"),
                new SpreadsheetCell("B: AA Hydrophobicity Group"),
                new SpreadsheetCell("B: AA PdbSum Group"),
                new SpreadsheetCell("B: AA UniProtKb Group"),
                new SpreadsheetCell("A: Atom X Position"),
                new SpreadsheetCell("A: Atom Y Position"),
                new SpreadsheetCell("A: Atom Z Position"),
                new SpreadsheetCell("<-->"),
                new SpreadsheetCell("B: Chain ID"),
                new SpreadsheetCell("B: ProteinInterface ID"),
                new SpreadsheetCell("B: Residue Sequence Index"),
                new SpreadsheetCell("B: Atom"),
                new SpreadsheetCell("B: Amino Acid"),
                new SpreadsheetCell("B: AA Physicochemical Group"),
                new SpreadsheetCell("B: AA Hydrophobicity Group"),
                new SpreadsheetCell("B: AA PdbSum Group"),
                new SpreadsheetCell("B: AA UniProtKb Group"),
                new SpreadsheetCell("B: Atom X Position"),
                new SpreadsheetCell("B: Atom Y Position"),
                new SpreadsheetCell("B: Atom Z Position"),

            };

            return columnHeadersRow;
        }

        public SpreadsheetCell[] SpreadsheetDataRow()
        {

            var atomPoint1 = PointConversions.AtomPoint3D(InteractingAtomPair.Atom1);
            var atomPoint2 = PointConversions.AtomPoint3D(InteractingAtomPair.Atom2);

            var row = new[]
            {
                new SpreadsheetCell(ProteinId),
                new SpreadsheetCell(InteractingAtomPair.Distance), 
                new SpreadsheetCell("-"), 
                new SpreadsheetCell(InteractingAtomPair.Atom1.chainID.FieldValue), 
                new SpreadsheetCell(ProteinInterfaceIdA), 
                new SpreadsheetCell(InteractingAtomPair.Atom1.resSeq.FieldValue), 
                new SpreadsheetCell(InteractingAtomPair.Atom1.name.FieldValue), 
                new SpreadsheetCell(AminoAcidConversions.AminoAcidNameToCode1L(InteractingAtomPair.Atom1.resName.FieldValue)), 
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical, InteractingAtomPair.Atom1.resName.FieldValue)[0]]),
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity, InteractingAtomPair.Atom1.resName.FieldValue)[0]]),
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum, InteractingAtomPair.Atom1.resName.FieldValue)[0]]),
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb, InteractingAtomPair.Atom1.resName.FieldValue)[0]]),
                new SpreadsheetCell(atomPoint1.X), 
                new SpreadsheetCell(atomPoint1.Y), 
                new SpreadsheetCell(atomPoint1.Z), 
                new SpreadsheetCell("<-->"), 
                new SpreadsheetCell(InteractingAtomPair.Atom2.chainID.FieldValue), 
                new SpreadsheetCell(ProteinInterfaceIdB), 
                new SpreadsheetCell(InteractingAtomPair.Atom2.resSeq.FieldValue), 
                new SpreadsheetCell(InteractingAtomPair.Atom2.name.FieldValue), 
                new SpreadsheetCell(AminoAcidConversions.AminoAcidNameToCode1L(InteractingAtomPair.Atom2.resName.FieldValue)), 
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical, InteractingAtomPair.Atom2.resName.FieldValue)[0]]),
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity, InteractingAtomPair.Atom2.resName.FieldValue)[0]]),
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum, InteractingAtomPair.Atom2.resName.FieldValue)[0]]),
                new SpreadsheetCell(AminoAcidGroups.AminoAcidGroups.GetSubgroupColorNames(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb)[AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb, InteractingAtomPair.Atom2.resName.FieldValue)[0]]),
                new SpreadsheetCell(atomPoint2.X), 
                new SpreadsheetCell(atomPoint2.Y), 
                new SpreadsheetCell(atomPoint2.Z)
            };

            return row;
        }
    }
}