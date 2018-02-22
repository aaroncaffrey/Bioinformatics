using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.Spreadsheets
{
    public enum SpreadsheetCellColourScheme
    {
        Default = 0,
        AminoAcidsIndividualAminoAcids = 1000 + AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.AminoAcids,
        AminoAcidsHydrophobicity = 1000 + AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity,
        AminoAcidsPhysicochemical = 1000 + AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical,
        AminoAcidsPdbSum = 1000 + AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum,
        AminoAcidsUniProtKb = 1000 + AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb,
        BooleanVector = 2000
    }
}
