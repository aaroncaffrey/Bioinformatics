using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.AminoAcids;
using DocumentFormat.OpenXml.Presentation;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public static class ProproteinInterfaceMotif
    {
        private static readonly int TotalAminoAcids = AminoAcids.AminoAcidTotals.TotalAminoAcids();
        private static readonly string NonStandardAminoAcids = string.Join("", "BJOUXZ".OrderBy(a => a).ToArray());
        private static readonly string StandardAminoAcids = string.Join("", AminoAcids.AminoAcidConversions.StandardAminoAcidsString().OrderBy(a => a).ToArray());

        public static bool IsMotifTooGeneral(string motifPattern)
        {
            if (motifPattern == null) throw new ArgumentNullException(nameof(motifPattern));

            var motif = motifPattern.Split('-');

            var totalAny = motif.Count(a => a == "X");

            var totalAmbiguousMustHave = motif.Count(a => a.StartsWith("[") && a.EndsWith("]") && a.Length >= 8);

            var totalAmbiguousMustNotHave = motif.Count(a => a.StartsWith("{") && a.EndsWith("}") && a.Length <= 6);

            if (totalAny + totalAmbiguousMustHave + totalAmbiguousMustNotHave >= motif.Length - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string MotifFormatter(List<string> motif)
        {
            if (motif == null) throw new ArgumentNullException(nameof(motif));

            for (var index = 0; index < motif.Count; index++)
            {
                if (string.IsNullOrEmpty(motif[index]) || motif[index].Length >= 20)
                {
                    motif[index] = "X";
                }
                else if (motif[index].Length > 1 && motif[index].Length <= 10)
                {
                    motif[index] = "[" + string.Join("", motif[index].OrderBy(a => a).ToArray()) + "]";
                }
                else if (motif[index].Length > 10)
                {
                    motif[index] = "{" + string.Join("", StandardAminoAcids.Where(a => !motif[index].Contains(a)).ToList()) + "}";
                }
            }

            var result = string.Join("-", motif);

            return result;
        }

        public static string FindProteinInterfaceAminoAcidCommonPropertiesMotif(List<string> proteinInterfaceSequenceList, AminoAcidPropertyMatchType aminoAcidPropertyMatchType, double matchValue = 0)
        {
            if (proteinInterfaceSequenceList == null) throw new ArgumentNullException(nameof(proteinInterfaceSequenceList));

            var totalProteinInterfaces = proteinInterfaceSequenceList.Count;

            var proteinInterfaceLength = proteinInterfaceSequenceList.Select(a => a.Length).Max();

            var aminoAcidObjects = new AminoAcidProperties<bool>[TotalAminoAcids];

            for (var aaIndex = 0; aaIndex < aminoAcidObjects.Length; aaIndex++)
            {
                aminoAcidObjects[aaIndex] = AminoAcidConversions.AminoAcidNumberToAminoAcidObject(aaIndex + 1);
            }

            var aminoAcidPositionCount = new AminoAcidProperties<int>[proteinInterfaceLength];

            for (var position = 0; position < aminoAcidPositionCount.Length; position++)
            {
                aminoAcidPositionCount[position] = new AminoAcidProperties<int>();
            }

            var motifs = new List<string>();

            for (int positionIndex = 0; positionIndex < proteinInterfaceLength; positionIndex++)
            {
                string positionMotif = "";

                foreach (var aaCode in proteinInterfaceSequenceList.Where(a => a.Length > positionIndex).Select(a => a[positionIndex]).Distinct().ToArray())
                {
                    if (!char.IsLetterOrDigit(aaCode) || NonStandardAminoAcids.Contains(aaCode)) continue;

                    var aminoAcid = aminoAcidObjects[AminoAcidConversions.AminoAcidNameToNumber(aaCode) - 1];

                    var matches = AminoAcidConversions.ListAminoAcidsByProperty(aminoAcid, aminoAcidPropertyMatchType, matchValue);

                    positionMotif += string.Join("", matches.Where(a => !positionMotif.Contains(a) && !NonStandardAminoAcids.Contains(a)).ToArray());
                }

                motifs.Add(positionMotif);
            }

            return MotifFormatter(motifs);
        }

        public static string FindProteinInterfaceAminoAcidGroupMotif(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups enumAminoAcidGroups, List<string> proteinInterfaceSequenceList)
        {
            if (proteinInterfaceSequenceList == null) throw new ArgumentNullException(nameof(proteinInterfaceSequenceList));

            var proteinInterfaceLength = proteinInterfaceSequenceList.Select(a => a.Length).Max();

            // initalise the profile matrix
            var aminoAcidPositionCount = new int[proteinInterfaceLength][];

            for (var position = 0; position < aminoAcidPositionCount.Length; position++)
            {
                aminoAcidPositionCount[position] = new int[AminoAcidGroups.AminoAcidGroups.GetTotalSubgroups(enumAminoAcidGroups)];
            }

            foreach (var proteinInterface in proteinInterfaceSequenceList)
            {
                for (int positionIndex = 0; positionIndex < proteinInterface.Length; positionIndex++)
                {
                    var aa = proteinInterface[positionIndex];

                    if (!char.IsLetterOrDigit(aa) || NonStandardAminoAcids.Contains(aa)) continue;

                    var aaGroupIndexes = AminoAcidGroups.AminoAcidGroups.ConvertAminoAcidNameCodeToSubgroupNumbers(enumAminoAcidGroups, aa);

                    foreach (var aaGroupIndex in aaGroupIndexes)
                    {
                        aminoAcidPositionCount[positionIndex][aaGroupIndex]++;
                    }
                }

                //if (proteinInterface.Length < proteinInterfaceLength)
                //{
                //    // fill remaining positions with X?
                //}
            }

            var motifs = new List<string>();

            for (var position = 0; position < aminoAcidPositionCount.Length; position++)
            {
                var positionMotif = "";
                for (var aaGroupIndex = 0; aaGroupIndex < aminoAcidPositionCount[position].Length; aaGroupIndex++)
                {
                    if (aminoAcidPositionCount[position][aaGroupIndex] > 0)
                    {
                        var aaGroup = AminoAcidGroups.AminoAcidGroups.GetSubgroupAminoAcidsCodesStrings(enumAminoAcidGroups)[aaGroupIndex];
                        foreach (var aa in aaGroup)
                        {
                            if (!NonStandardAminoAcids.Contains(aa) && !positionMotif.Contains(aa))
                            {
                                positionMotif += aa;
                            }
                        }

                    }
                }

                positionMotif = string.Join("", positionMotif.Distinct().ToArray());

                motifs.Add(positionMotif);
            }

            return MotifFormatter(motifs);

        }

        public static string FindProteinInterfaceAminoAcidMotif(List<string> proteinInterfaceSequenceList)
        {
            if (proteinInterfaceSequenceList == null) throw new ArgumentNullException(nameof(proteinInterfaceSequenceList));

            var proteinInterfaceLength = proteinInterfaceSequenceList.Select(a => a.Length).Max();

            // initalise the profile matrix
            var aminoAcidPositionCount = new int[proteinInterfaceLength][];

            for (var position = 0; position < aminoAcidPositionCount.Length; position++)
            {
                aminoAcidPositionCount[position] = new int[TotalAminoAcids];
            }

            foreach (var proteinInterface in proteinInterfaceSequenceList)
            {
                for (int positionIndex = 0; positionIndex < proteinInterface.Length; positionIndex++)
                {
                    var aa = proteinInterface[positionIndex];

                    if (!char.IsLetterOrDigit(aa) || NonStandardAminoAcids.Contains(aa)) continue;

                    var aaIndex = AminoAcids.AminoAcidConversions.AminoAcidNameToNumber(aa) - 1;

                    aminoAcidPositionCount[positionIndex][aaIndex]++;
                }

                if (proteinInterface.Length < proteinInterfaceLength)
                {
                    // fill remaining positions with X?
                }
            }

            var motifs = new List<string>();

            for (var position = 0; position < aminoAcidPositionCount.Length; position++)
            {
                var positionMotif = "";
                for (var aaIndex = 0; aaIndex < aminoAcidPositionCount[position].Length; aaIndex++)
                {
                    if (aminoAcidPositionCount[position][aaIndex] > 0)
                    {
                        positionMotif += AminoAcids.AminoAcidConversions.AminoAcidNumberToCode1L(aaIndex + 1);
                    }
                }

                positionMotif = string.Join("", positionMotif.Distinct().ToArray());

                motifs.Add(positionMotif);
            }

            return MotifFormatter(motifs);
        }
    }
}
