using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ProteinBioinformaticsSharedLibrary.AminoAcids;

namespace ProteinBioinformaticsSharedLibrary.AminoAcidGroups
{
    public static class AminoAcidGroups
    {
        public enum EnumAminoAcidGroups
        {
            AminoAcids = 0,
            Physicochemical = 1,
            Hydrophobicity = 2,
            UniProtKb = 3,
            PdbSum = 4,
            Properties = 5
        }

        public static AminoAcidProperties<bool>[][] GetGroupsAndAminoAcidCodes(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            return GetSubgroupAminoAcidsCodesStrings(enumAminoAcidGroups).Select(g => g.Select(AminoAcidConversions.AminoAcidNameCodeToAminoAcidObject).ToArray()).ToArray();
        }

        public static Color[] GetSubgroupColors(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            switch (enumAminoAcidGroups)
            {
                case EnumAminoAcidGroups.AminoAcids:
                    return new[]
                    {
                        Color.FromArgb(0, 0, 180),
                        Color.FromArgb(175, 13, 102),
                        Color.FromArgb(146,248,70),
                        Color.FromArgb(255, 200, 47),
                        Color.FromArgb(255,118,0),
                        Color.FromArgb(185,185,185),
                        Color.FromArgb(235,235,222),
                        Color.FromArgb(100,100,100),
                        Color.FromArgb(255,255,0),
                        Color.FromArgb(55,19,112),
                        Color.FromArgb(255,255,150),
                        Color.FromArgb(202,62,94),
                        Color.FromArgb(205,145,63),
                        Color.FromArgb(12,75,100),
                        Color.FromArgb(255,0,0),
                        Color.FromArgb(175,155,50),
                        Color.FromArgb(0,0,0),
                        Color.FromArgb(37,70,25),
                        Color.FromArgb(121,33,135),
                        Color.FromArgb(83,140,208),
                        Color.FromArgb(0,154,37),
                        Color.FromArgb(178,220,205),
                        Color.FromArgb(255,152,213),
                        Color.FromArgb(0,0,74),
                        Color.FromArgb(175,200,74),
                        Color.FromArgb(63,25,12),
                    };
                case EnumAminoAcidGroups.Hydrophobicity:
                    return new[] {
                        Color.Blue,
                        Color.Red,
                        Color.Green,
                        Color.Yellow,
                        Color.Gray
                    };
                case EnumAminoAcidGroups.Physicochemical:
                    return new[] {
                        Color.Red,
                        Color.Blue,
                        Color.Magenta,
                        Color.Green,
                        Color.Gray
                    };
                case EnumAminoAcidGroups.UniProtKb:
                    return new[]
                    {
                        Color.Gray,
                        Color.Red,
                        Color.Green,
                        Color.Blue,
                        Color.Black,
                        Color.White,
                        Color.Yellow,
                        Color.LightGray
                    };
                case EnumAminoAcidGroups.PdbSum:
                    return new[]
                    {
                        Color.DodgerBlue,
                        Color.Red,
                        Color.Green,
                        Color.Gray,
                        Color.MediumPurple,
                        Color.DarkGoldenrod,
                        Color.Yellow,
                        Color.DarkSlateGray
                    };
                case EnumAminoAcidGroups.Properties:
                    return new[]
                    {
                        //Color.FromArgb(12,75,100), 
                        Color.FromArgb(255, 0, 0),
                        Color.FromArgb(175, 155, 50),
                        Color.FromArgb(0, 0, 0),
                        Color.FromArgb(37, 70, 25),
                        Color.FromArgb(121, 33, 135),
                        Color.FromArgb(83, 140, 208),
                        Color.FromArgb(0, 154, 37),
                        Color.FromArgb(178, 220, 205),
                        Color.FromArgb(255, 152, 213),
                        Color.FromArgb(0, 0, 74),
                        Color.FromArgb(175, 200, 74),
                        Color.FromArgb(63, 25, 12),
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string[] GetSubgroupColorNames(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            return GetSubgroupColors(enumAminoAcidGroups).Select(a => a.Name).ToArray();
        }

        public static string ConvertAminoAcidStringToSubgroupNumberString(string aminoAcidString, EnumAminoAcidGroups enumAminoAcidGroups)
        {
            var aminoAcidGroupNumbers = aminoAcidString.Select(a => ConvertAminoAcidNameCodeToSubgroupNumbers(enumAminoAcidGroups, a)).ToList();

            if (aminoAcidGroupNumbers.Count(a => a.Length > 1) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(enumAminoAcidGroups), "Cannot convert to groups with multiple possibilities");
            }

            var aminoAcidGroupNumbersFlattened = aminoAcidGroupNumbers.Select(a => a[0] + 1).ToList();

            var aminoAcidGroupNumbersString = string.Join("", aminoAcidGroupNumbersFlattened);

            return aminoAcidGroupNumbersString;
        }

        public static string[] GetSubgroupDescriptions(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            switch (enumAminoAcidGroups)
            {
                case EnumAminoAcidGroups.AminoAcids:
                    return AminoAcidStrings;
                case EnumAminoAcidGroups.Hydrophobicity:
                    return new[] {
                        "PC: Aliphatic / Non-polar",
                        "PC: Aromatic",
                        "PC: Polar",
                        "PC: Sulfur",
                        "PC: Unusual",
                    };
                case EnumAminoAcidGroups.Physicochemical:
                    return new[] {
                        "CW: Small + hydrophobic + Aromatic - Y",
                        "CW: Acidic",
                        "CW: Basic - H",
                        "CW: Hydroxyl + sulfhydryl + amine + G",
                        "CW: Unusual",
                    };
                case EnumAminoAcidGroups.UniProtKb:
                    return new[]
                    {
                        "UP: Aliphatic",
                        "UP: Acidic",
                        "UP: Small Hydroxyl",
                        "UP: Basic",
                        "UP: Aromatic",
                        "UP: Amide",
                        "UP: Sulfur",
                        "UP: Unusual"
                    };
                case EnumAminoAcidGroups.PdbSum:
                    return new[]
                    {
                        "PS: Positive",
                        "PS: Negative",
                        "PS: Neutral",
                        "PS: Aliphatic",
                        "PS: Aromatic",
                        "PS: P/G",
                        "PS: C",
                        "PS: Unusual"
                    };
                case EnumAminoAcidGroups.Properties:
                    return new[]
                    {
                        //"Acidic",
                        "Aliphatic",
                        "Aromatic",
                        "Charged",
                        "Hydrophobic",
                        "Hydroxylic",
                        "Negative",
                        "Polar",
                        "Positive",
                        "Small",
                        "Sulphur",
                        "Tiny",
                        "None"
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static readonly string[] AminoAcidStrings = (new string[AminoAcidTotals.TotalAminoAcids()]).Select((a, i) => AminoAcidConversions.AminoAcidNumberToCode1L(i + 1)).ToArray();

        public static readonly string[] PropertyStrings = new[]
        {
            //AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Acidic = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Aliphatic = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Aromatic = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Charged = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Hydrophobic = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Hydroxylic = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Negative = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Polar = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Positive = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Small = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Sulphur = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Tiny = true}, AminoAcidPropertyMatchType.AnyTrueMatch),
            AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {}, AminoAcidPropertyMatchType.AllMatch),
        };

        public static string[] GetSubgroupAminoAcidsCodesStrings(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            switch (enumAminoAcidGroups)
            {
                case EnumAminoAcidGroups.AminoAcids:
                    return AminoAcidStrings;
                case EnumAminoAcidGroups.Physicochemical:
                    return new[] {
                        "AVFPMILW",
                        "DE",
                        "RK",
                        "STYHCNGQ",
                        "BJOUXZ"
                    };
                case EnumAminoAcidGroups.Hydrophobicity:
                    return new[] {
                        "AGILPV",
                        "FYW",
                        "DENQRHSTK",
                        "CM",
                        "BJOUXZ"
                    };
                case EnumAminoAcidGroups.UniProtKb:
                    return new[]
                    {
                        "LAGVIP",
                        "DE",
                        "ST",
                        "RKH",
                        "FYW",
                        "NQ",
                        "CM",
                        "BJOUXZ"
                    };
                case EnumAminoAcidGroups.PdbSum:
                    return new[]
                    {
                        "HKR",
                        "DE",
                        "STNQ",
                        "AVLIM",
                        "FYW",
                        "PG",
                        "C",
                        "BJOUXZ"
                    };
                case EnumAminoAcidGroups.Properties:
                    return PropertyStrings;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int[][] GetAllSubgroupAminoAcidsNumbers(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            return GetSubgroupAminoAcidsCodesStrings(enumAminoAcidGroups).Select(g => g.Select(AminoAcidConversions.AminoAcidNameToNumber).ToArray()).ToArray();
        }

        public static string GetGroupRuleName(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            switch (enumAminoAcidGroups)
            {
                case EnumAminoAcidGroups.AminoAcids:
                    return "Amino Acids";
                case EnumAminoAcidGroups.Physicochemical:
                    return "Physicochemical [ClustalW2]";
                case EnumAminoAcidGroups.Hydrophobicity:
                    return "Hydrophobicity [Protein Colourer]";
                case EnumAminoAcidGroups.UniProtKb:
                    return "Physicochemical [UniProtKB/TrEMBL]";
                case EnumAminoAcidGroups.PdbSum:
                    return "Physicochemical [PDBsum]";
                case EnumAminoAcidGroups.Properties:
                    return "Properties [Venn diagram]";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int GetTotalSubgroups(EnumAminoAcidGroups enumAminoAcidGroups)
        {
            return GetSubgroupColors(enumAminoAcidGroups).Length;
        }

        public static int GetTotalSubgroups(int enumAminoAcidGroupIndex)
        {
            return GetSubgroupColors((EnumAminoAcidGroups)enumAminoAcidGroupIndex).Length;
        }

        public static int GetTotalGroups()
        {
            return Enum.GetNames(typeof(EnumAminoAcidGroups)).Length;
        }

        public static int[] ConvertAminoAcidNumberToSubgroupNumbers(EnumAminoAcidGroups enumAminoAcidGroups, int aminoAcidNumber)
        {
            var result = new List<int>();
            var groupNumbers = GetAllSubgroupAminoAcidsNumbers(enumAminoAcidGroups);

            for (var groupIndex = 0; groupIndex < groupNumbers.GetLength(0); groupIndex++)
            {
                if (groupNumbers[groupIndex].Contains(aminoAcidNumber))
                {
                    result.Add(groupIndex);
                }
            }

            if (result.Count > 0)
            {
                return result.ToArray();
            }

            throw new ArgumentOutOfRangeException(nameof(aminoAcidNumber));
        }

        public static int[] ConvertAminoAcidNameCodeToSubgroupNumbers(EnumAminoAcidGroups enumAminoAcidGroups, string aminoAcidNameCode)
        {
            return ConvertAminoAcidNumberToSubgroupNumbers(enumAminoAcidGroups, AminoAcidConversions.AminoAcidNameToNumber(aminoAcidNameCode));
        }

        public static int[] ConvertAminoAcidNameCodeToSubgroupNumbers(EnumAminoAcidGroups enumAminoAcidGroups, char aminoAcidNameCode)
        {
            return ConvertAminoAcidNumberToSubgroupNumbers(enumAminoAcidGroups, AminoAcidConversions.AminoAcidNameToNumber(aminoAcidNameCode));
        }

    }
}
