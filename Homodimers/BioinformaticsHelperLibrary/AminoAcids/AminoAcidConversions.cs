//-----------------------------------------------------------------------
// <copyright file="AminoAcidTypes.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using BioinformaticsHelperLibrary.AminoAcids.Additional;
using BioinformaticsHelperLibrary.AminoAcids.Additional.AdditionalTypes;
using BioinformaticsHelperLibrary.AminoAcids.Ambiguous;
using BioinformaticsHelperLibrary.AminoAcids.Ambiguous.AmbiguousTypes;
using BioinformaticsHelperLibrary.AminoAcids.NonStandard;
using BioinformaticsHelperLibrary.AminoAcids.NonStandard.NonStandardTypes;
using BioinformaticsHelperLibrary.AminoAcids.Standard;
using BioinformaticsHelperLibrary.AminoAcids.Standard.StandardTypes;

namespace BioinformaticsHelperLibrary.AminoAcids
{
    /// <summary>
    ///     This class has various information and representations of amino acids and can perform conversions between those
    ///     representations.
    /// </summary>
    public static class AminoAcidConversions
    {
        public static string[] AminoAcidCodeArray1L()
        {
            var totalAminoAcids = AminoAcidTotals.TotalAminoAcids();

            var result = new string[totalAminoAcids];

            for (var aaIndex = 0; aaIndex < totalAminoAcids; aaIndex++)
            {
                result[aaIndex] = AminoAcidNumberToCode1L(aaIndex + 1);
            }

            return result;
        }

        public static string StandardAminoAcidsString()
        {
            var result = "";
            foreach (StandardAminoAcids1L aminoAcid1L in Enum.GetValues(typeof(StandardAminoAcids1L)))
            {
                result += aminoAcid1L;
            }
            return result;
        }

        public static string AdditionalAminoAcidsString()
        {
            var result = "";
            foreach (AdditionalAminoAcids1L aminoAcid1L in Enum.GetValues(typeof(AdditionalAminoAcids1L)))
            {
                result += aminoAcid1L;
            }
            return result;
        }

        public static string AmbiguousAminoAcidsString()
        {
            var result = "";
            foreach (AmbiguousAminoAcids1L aminoAcid1L in Enum.GetValues(typeof(AmbiguousAminoAcids1L)))
            {
                result += aminoAcid1L;
            }
            return result;
        }

        public static string NonStandardAminoAcidsString()
        {
            var result = "";
            foreach (NonStandardAminoAcids1L aminoAcid1L in Enum.GetValues(typeof(NonStandardAminoAcids1L)))
            {
                result += aminoAcid1L;
            }
            return result;
        }

        /// <summary>
        /// This method returns a list of amino acid codes that match any of the specified true properties.  If all parameter properties are false, it returns a list of 
        /// </summary>
        /// <param name="aminoAcidData"></param>
        /// <param name="aminoAcidPropertyMatchType"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static string ListAminoAcidsByProperty(AminoAcidProperties<bool> aminoAcidData, AminoAcidPropertyMatchType aminoAcidPropertyMatchType = AminoAcidPropertyMatchType.AnyTrueMatch, double matchValue = 0)
        {
            var result = new List<string>();

            var aminoAcidDataPropertiesArray = aminoAcidData.PropertiesArray();

            for (var index = 0; index < AminoAcidTotals.TotalAminoAcids(); index++)
            {
                var aa = AminoAcidConversions.AminoAcidNumberToAminoAcidObject(index + 1);
                //aminoAcidDataPropertiesArray = aa.PropertiesArray();

                var propertyMatches = new AminoAcidProperties<bool>()
                {
                    //Acidic = 		aminoAcidData.Acidic 		== aa.Acidic,
                    Aliphatic = 	aminoAcidData.Aliphatic 	== aa.Aliphatic,
                    Aromatic = 		aminoAcidData.Aromatic 		== aa.Aromatic,
                    Charged = 		aminoAcidData.Charged 		== aa.Charged,
                    Hydrophobic = 	aminoAcidData.Hydrophobic 	== aa.Hydrophobic,
                    Hydroxylic = 	aminoAcidData.Hydroxylic 	== aa.Hydroxylic,
                    Negative = 		aminoAcidData.Negative 		== aa.Negative,
                    Polar = 		aminoAcidData.Polar 		== aa.Polar,
                    Positive = 		aminoAcidData.Positive 		== aa.Positive,
                    Small = 		aminoAcidData.Small 		== aa.Small,
                    Sulphur =       aminoAcidData.Sulphur 		== aa.Sulphur,
                    Tiny = 		 	aminoAcidData.Tiny 		    == aa.Tiny,
                };

                var matchedPropertiesArray = propertyMatches.PropertiesArray();

                // number of equal boolean values in aminoAcidData and aa
                var totalMatchingProperties = matchedPropertiesArray.Where(a=>a).Count();

                var totalTrueMatchingProperties = aminoAcidDataPropertiesArray.Where((a, i) => a && matchedPropertiesArray[i]).Count();
                var totalFalseMatchingProperties = aminoAcidDataPropertiesArray.Where((a, i) => !a && matchedPropertiesArray[i]).Count();

                var totalTrueParameterProperties = aminoAcidDataPropertiesArray.Count(a => a);
                var totalFalseParameterProperties = aminoAcidDataPropertiesArray.Count(a => !a);

                var percentageMatchingProperties = totalMatchingProperties / matchedPropertiesArray.Length;
                var percentageTrueMatchingProperties = totalTrueParameterProperties > 0 ? totalTrueMatchingProperties/totalTrueParameterProperties : 0;
                var percentageFalseMatchingProperties = totalFalseParameterProperties > 0 ? totalFalseMatchingProperties/totalFalseParameterProperties : 0;

                var noPropertyMatches = totalMatchingProperties == 0;
                var anyPropertyMatches = totalMatchingProperties > 0;
                var allPropertiesMatch = matchedPropertiesArray.Length == totalMatchingProperties;

                var anyTrueMatch = aminoAcidDataPropertiesArray.Where((a, i) => a && matchedPropertiesArray[i]).Any();
                var anyFalseMatch = aminoAcidDataPropertiesArray.Where((a, i) => !a && matchedPropertiesArray[i]).Any();

                var allTrueMatch = aminoAcidDataPropertiesArray.Where((a, i) => a && matchedPropertiesArray[i]).Count() == aminoAcidDataPropertiesArray.Length;
                var allFalseMatch = aminoAcidDataPropertiesArray.Where((a, i) => !a && matchedPropertiesArray[i]).Count() == aminoAcidDataPropertiesArray.Length;

                if ((aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.AllMatch && allPropertiesMatch) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.AllTrueMatch && allTrueMatch) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.AllFalseMatch && allFalseMatch) ||

                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.AnyMatch && anyPropertyMatches) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.AnyTrueMatch && anyTrueMatch) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.AnyFalseMatch && anyFalseMatch) ||

                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.NoneMatch && noPropertyMatches) ||

                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.MininumMatch && totalMatchingProperties >= matchValue) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.MininumTrueMatch && totalTrueMatchingProperties >= matchValue) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.MininumFalseMatch && totalFalseMatchingProperties >= matchValue) ||

                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.PercentageMatch && percentageMatchingProperties >= matchValue) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.PercentageTrueMatch && percentageTrueMatchingProperties >= matchValue) ||
                    (aminoAcidPropertyMatchType == AminoAcidPropertyMatchType.PercentageFalseMatch && percentageFalseMatchingProperties >= matchValue))
                {
                    result.Add(aa.Code1L);
                }
            }

            var resultStr = string.Join("", result.OrderBy(a => a).ToArray());

            return resultStr;
        }

        public static AminoAcidProperties<bool> AminoAcidNameCodeToAminoAcidObject(char aminoAcidNameCode)
        {
            return AminoAcidNameCodeToAminoAcidObject("" + aminoAcidNameCode);
        }

        public static AminoAcidProperties<bool> AminoAcidNameCodeToAminoAcidObject(string aminoAcidNameCode)
        {
            return AminoAcidNumberToAminoAcidObject(AminoAcidNameToNumber(aminoAcidNameCode));
        }

        /// <summary>
        ///     Converts an Amino Acid number to an object
        /// </summary>
        /// <param name="aminoAcidNumber"></param>
        /// <returns></returns>
        public static AminoAcidProperties<bool> AminoAcidNumberToAminoAcidObject(int aminoAcidNumber)
        {
            switch (aminoAcidNumber)
            {
                case (int)StandardAminoAcids.Alanine:
                    return new Alanine();
                case (int)StandardAminoAcids.Arginine:
                    return new Arginine();
                case (int)StandardAminoAcids.Asparagine:
                    return new Asparagine();
                case (int)StandardAminoAcids.AsparticAcid:
                    return new AsparticAcid();
                case (int)StandardAminoAcids.Cysteine:
                    return new Cysteine();
                case (int)StandardAminoAcids.GlutamicAcid:
                    return new GlutamicAcid();
                case (int)StandardAminoAcids.Glutamine:
                    return new Glutamine();
                case (int)StandardAminoAcids.Glycine:
                    return new Glycine();
                case (int)StandardAminoAcids.Histidine:
                    return new Histidine();
                case (int)StandardAminoAcids.Isoleucine:
                    return new Isoleucine();
                case (int)StandardAminoAcids.Leucine:
                    return new Leucine();
                case (int)StandardAminoAcids.Lysine:
                    return new Lysine();
                case (int)StandardAminoAcids.Methionine:
                    return new Methionine();
                case (int)StandardAminoAcids.Phenylalanine:
                    return new Phenylalanine();
                case (int)StandardAminoAcids.Proline:
                    return new Proline();
                case (int)StandardAminoAcids.Serine:
                    return new Serine();
                case (int)StandardAminoAcids.Threonine:
                    return new Threonine();
                case (int)StandardAminoAcids.Tryptophan:
                    return new Tryptophan();
                case (int)StandardAminoAcids.Tyrosine:
                    return new Tyrosine();
                case (int)StandardAminoAcids.Valine:
                    return new Valine();
                case (int)AdditionalAminoAcids.Pyrrolysine:
                    return new Pyrrolysine();
                case (int)AdditionalAminoAcids.Selenocysteine:
                    return new Selenocysteine();
                case (int)AmbiguousAminoAcids.AsparagineOrAsparticAcid:
                    return new AsparagineOrAsparticAcid();
                case (int)AmbiguousAminoAcids.GlutamineOrGlutamicAcid:
                    return new GlutamineOrGlutamicAcid();
                case (int)AmbiguousAminoAcids.LeucineOrIsoleucine:
                    return new LeucineOrIsoleucine();
                case (int)AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid:
                    return new UnspecifiedOrUnknownAminoAcid();

                default:
                    //return UnspecifiedOrUnknownAminoAcid.Code3L;
                    throw new ArgumentOutOfRangeException(nameof(aminoAcidNumber));
            }
        }

        /// <summary>
        ///     Converts an amino acid code to the respective english name.
        /// </summary>
        /// <param name="aminoAcidCode"></param>
        /// <returns></returns>
        public static string AminoAcidCodeToName(string aminoAcidCode)
        {
            if (aminoAcidCode == null || aminoAcidCode.Length <= 0)
            {
                return null;
            }
            if (aminoAcidCode.Length == 1 || aminoAcidCode.Length == 3)
            {
                aminoAcidCode = aminoAcidCode.ToUpperInvariant();
            }
            else
            {
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                aminoAcidCode = textInfo.ToTitleCase(aminoAcidCode);
            }

            return AminoAcidNumberToName(AminoAcidNameToNumber(aminoAcidCode));
        }



        /// <summary>
        ///     Converts an Amino Acid name or code to the 1 letter code form.
        /// </summary>
        /// <param name="aminoAcidCode"></param>
        /// <returns></returns>
        public static string AminoAcidNameToCode1L(string aminoAcidCode)
        {
            if (aminoAcidCode == null || aminoAcidCode.Length <= 0)
            {
                return null;
            }
            if (aminoAcidCode.Length == 1 || aminoAcidCode.Length == 3)
            {
                aminoAcidCode = aminoAcidCode.ToUpperInvariant();
            }
            else
            {
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                aminoAcidCode = textInfo.ToTitleCase(aminoAcidCode);
            }

            return AminoAcidNumberToCode1L(AminoAcidNameToNumber(aminoAcidCode));
        }


        /// <summary>
        ///     Converts an Amino Acid name or code to the 3 letter code form.
        /// </summary>
        /// <param name="aminoAcidCode"></param>
        /// <returns></returns>
        public static string AminoAcidNameToCode3L(string aminoAcidCode)
        {
            if (aminoAcidCode == null || aminoAcidCode.Length <= 0)
            {
                return null;
            }
            if (aminoAcidCode.Length == 1 || aminoAcidCode.Length == 3)
            {
                aminoAcidCode = aminoAcidCode.ToUpperInvariant();
            }
            else
            {
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                aminoAcidCode = textInfo.ToTitleCase(aminoAcidCode);
            }

            return AminoAcidNumberToCode3L(AminoAcidNameToNumber(aminoAcidCode));
        }

        /// <summary>
        ///     Converts an Amino Acid number to the english name.
        /// </summary>
        /// <param name="aminoAcidNumber"></param>
        /// <returns></returns>
        public static string AminoAcidNumberToName(int aminoAcidNumber)
        {
            switch (aminoAcidNumber)
            {
                case (int) StandardAminoAcids.Alanine:
                    return Alanine.Name;
                case (int) StandardAminoAcids.Arginine:
                    return Arginine.Name;
                case (int) StandardAminoAcids.Asparagine:
                    return Asparagine.Name;
                case (int) StandardAminoAcids.AsparticAcid:
                    return AsparticAcid.Name;
                case (int) StandardAminoAcids.Cysteine:
                    return Cysteine.Name;
                case (int) StandardAminoAcids.GlutamicAcid:
                    return GlutamicAcid.Name;
                case (int) StandardAminoAcids.Glutamine:
                    return Glutamine.Name;
                case (int) StandardAminoAcids.Glycine:
                    return Glycine.Name;
                case (int) StandardAminoAcids.Histidine:
                    return Histidine.Name;
                case (int) StandardAminoAcids.Isoleucine:
                    return Isoleucine.Name;
                case (int) StandardAminoAcids.Leucine:
                    return Leucine.Name;
                case (int) StandardAminoAcids.Lysine:
                    return Lysine.Name;
                case (int) StandardAminoAcids.Methionine:
                    return Methionine.Name;
                case (int) StandardAminoAcids.Phenylalanine:
                    return Phenylalanine.Name;
                case (int) StandardAminoAcids.Proline:
                    return Proline.Name;
                case (int) StandardAminoAcids.Serine:
                    return Serine.Name;
                case (int) StandardAminoAcids.Threonine:
                    return Threonine.Name;
                case (int) StandardAminoAcids.Tryptophan:
                    return Tryptophan.Name;
                case (int) StandardAminoAcids.Tyrosine:
                    return Tyrosine.Name;
                case (int) StandardAminoAcids.Valine:
                    return Valine.Name;
                case (int) AdditionalAminoAcids.Pyrrolysine:
                    return Pyrrolysine.Name;
                case (int) AdditionalAminoAcids.Selenocysteine:
                    return Selenocysteine.Name;
                case (int) AmbiguousAminoAcids.AsparagineOrAsparticAcid:
                    return AsparagineOrAsparticAcid.Name;
                case (int) AmbiguousAminoAcids.GlutamineOrGlutamicAcid:
                    return GlutamineOrGlutamicAcid.Name;
                case (int) AmbiguousAminoAcids.LeucineOrIsoleucine:
                    return LeucineOrIsoleucine.Name;
                case (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid:
                    return UnspecifiedOrUnknownAminoAcid.Name;
                default:
                    //return UnspecifiedOrUnknownAminoAcid.Name;
                    throw new ArgumentOutOfRangeException(nameof(aminoAcidNumber));
            }
        }

        /// <summary>
        ///     Converts an Amino Acid number to the 1 letter code.
        /// </summary>
        /// <param name="aminoAcidNumber"></param>
        /// <returns></returns>
        public static string AminoAcidNumberToCode1L(int aminoAcidNumber)
        {
            switch (aminoAcidNumber)
            {
                case (int) StandardAminoAcids.Alanine:
                    return Alanine.Code1L;
                case (int) StandardAminoAcids.Arginine:
                    return Arginine.Code1L;
                case (int) StandardAminoAcids.Asparagine:
                    return Asparagine.Code1L;
                case (int) StandardAminoAcids.AsparticAcid:
                    return AsparticAcid.Code1L;
                case (int) StandardAminoAcids.Cysteine:
                    return Cysteine.Code1L;
                case (int) StandardAminoAcids.GlutamicAcid:
                    return GlutamicAcid.Code1L;
                case (int) StandardAminoAcids.Glutamine:
                    return Glutamine.Code1L;
                case (int) StandardAminoAcids.Glycine:
                    return Glycine.Code1L;
                case (int) StandardAminoAcids.Histidine:
                    return Histidine.Code1L;
                case (int) StandardAminoAcids.Isoleucine:
                    return Isoleucine.Code1L;
                case (int) StandardAminoAcids.Leucine:
                    return Leucine.Code1L;
                case (int) StandardAminoAcids.Lysine:
                    return Lysine.Code1L;
                case (int) StandardAminoAcids.Methionine:
                    return Methionine.Code1L;
                case (int) StandardAminoAcids.Phenylalanine:
                    return Phenylalanine.Code1L;
                case (int) StandardAminoAcids.Proline:
                    return Proline.Code1L;
                case (int) StandardAminoAcids.Serine:
                    return Serine.Code1L;
                case (int) StandardAminoAcids.Threonine:
                    return Threonine.Code1L;
                case (int) StandardAminoAcids.Tryptophan:
                    return Tryptophan.Code1L;
                case (int) StandardAminoAcids.Tyrosine:
                    return Tyrosine.Code1L;
                case (int) StandardAminoAcids.Valine:
                    return Valine.Code1L;
                case (int) AdditionalAminoAcids.Pyrrolysine:
                    return Pyrrolysine.Code1L;
                case (int) AdditionalAminoAcids.Selenocysteine:
                    return Selenocysteine.Code1L;
                case (int) AmbiguousAminoAcids.AsparagineOrAsparticAcid:
                    return AsparagineOrAsparticAcid.Code1L;
                case (int) AmbiguousAminoAcids.GlutamineOrGlutamicAcid:
                    return GlutamineOrGlutamicAcid.Code1L;
                case (int) AmbiguousAminoAcids.LeucineOrIsoleucine:
                    return LeucineOrIsoleucine.Code1L;
                case (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid:
                    return UnspecifiedOrUnknownAminoAcid.Code1L;
                default:
                    //return UnspecifiedOrUnknownAminoAcid.Code1L;
                    throw new ArgumentOutOfRangeException(nameof(aminoAcidNumber));
            }
        }

        /// <summary>
        ///     Converts an Amino Acid number to the 3 letter code.
        /// </summary>
        /// <param name="aminoAcidNumber"></param>
        /// <returns></returns>
        public static string AminoAcidNumberToCode3L(int aminoAcidNumber)
        {
            switch (aminoAcidNumber)
            {
                case (int) StandardAminoAcids.Alanine:
                    return Alanine.Code3L;
                case (int) StandardAminoAcids.Arginine:
                    return Arginine.Code3L;
                case (int) StandardAminoAcids.Asparagine:
                    return Asparagine.Code3L;
                case (int) StandardAminoAcids.AsparticAcid:
                    return AsparticAcid.Code3L;
                case (int) StandardAminoAcids.Cysteine:
                    return Cysteine.Code3L;
                case (int) StandardAminoAcids.GlutamicAcid:
                    return GlutamicAcid.Code3L;
                case (int) StandardAminoAcids.Glutamine:
                    return Glutamine.Code3L;
                case (int) StandardAminoAcids.Glycine:
                    return Glycine.Code3L;
                case (int) StandardAminoAcids.Histidine:
                    return Histidine.Code3L;
                case (int) StandardAminoAcids.Isoleucine:
                    return Isoleucine.Code3L;
                case (int) StandardAminoAcids.Leucine:
                    return Leucine.Code3L;
                case (int) StandardAminoAcids.Lysine:
                    return Lysine.Code3L;
                case (int) StandardAminoAcids.Methionine:
                    return Methionine.Code3L;
                case (int) StandardAminoAcids.Phenylalanine:
                    return Phenylalanine.Code3L;
                case (int) StandardAminoAcids.Proline:
                    return Proline.Code3L;
                case (int) StandardAminoAcids.Serine:
                    return Serine.Code3L;
                case (int) StandardAminoAcids.Threonine:
                    return Threonine.Code3L;
                case (int) StandardAminoAcids.Tryptophan:
                    return Tryptophan.Code3L;
                case (int) StandardAminoAcids.Tyrosine:
                    return Tyrosine.Code3L;
                case (int) StandardAminoAcids.Valine:
                    return Valine.Code3L;
                case (int) AdditionalAminoAcids.Pyrrolysine:
                    return Pyrrolysine.Code3L;
                case (int) AdditionalAminoAcids.Selenocysteine:
                    return Selenocysteine.Code3L;
                case (int) AmbiguousAminoAcids.AsparagineOrAsparticAcid:
                    return AsparagineOrAsparticAcid.Code3L;
                case (int) AmbiguousAminoAcids.GlutamineOrGlutamicAcid:
                    return GlutamineOrGlutamicAcid.Code3L;
                case (int) AmbiguousAminoAcids.LeucineOrIsoleucine:
                    return LeucineOrIsoleucine.Code3L;
                case (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid:
                    return UnspecifiedOrUnknownAminoAcid.Code3L;

                default:
                    //return UnspecifiedOrUnknownAminoAcid.Code3L;
                    throw new ArgumentOutOfRangeException(nameof(aminoAcidNumber));
            }
        }

        public static int AminoAcidNameToNumber(char aminoAcidCode)
        {
            return AminoAcidNameToNumber(""+aminoAcidCode);
        }

        /// <summary>
        ///     Converts an Amino Acid name or code to the number.
        /// </summary>
        /// <param name="aminoAcidCode"></param>
        /// <returns></returns>
        public static int AminoAcidNameToNumber(string aminoAcidCode)
        {
            if (string.IsNullOrWhiteSpace(aminoAcidCode))
            {
                return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
            }

            if (aminoAcidCode.Length == 1)
            {
                switch (aminoAcidCode)
                {
                    case Alanine.Code1L:
                        return (int) StandardAminoAcids.Alanine;
                    case Arginine.Code1L:
                        return (int) StandardAminoAcids.Arginine;
                    case Asparagine.Code1L:
                        return (int) StandardAminoAcids.Asparagine;
                    case AsparticAcid.Code1L:
                        return (int) StandardAminoAcids.AsparticAcid;
                    case Cysteine.Code1L:
                        return (int) StandardAminoAcids.Cysteine;
                    case GlutamicAcid.Code1L:
                        return (int) StandardAminoAcids.GlutamicAcid;
                    case Glutamine.Code1L:
                        return (int) StandardAminoAcids.Glutamine;
                    case Glycine.Code1L:
                        return (int) StandardAminoAcids.Glycine;
                    case Histidine.Code1L:
                        return (int) StandardAminoAcids.Histidine;
                    case Isoleucine.Code1L:
                        return (int) StandardAminoAcids.Isoleucine;
                    case Leucine.Code1L:
                        return (int) StandardAminoAcids.Leucine;
                    case Lysine.Code1L:
                        return (int) StandardAminoAcids.Lysine;
                    case Methionine.Code1L:
                        return (int) StandardAminoAcids.Methionine;
                    case Phenylalanine.Code1L:
                        return (int) StandardAminoAcids.Phenylalanine;
                    case Proline.Code1L:
                        return (int) StandardAminoAcids.Proline;
                    case Serine.Code1L:
                        return (int) StandardAminoAcids.Serine;
                    case Threonine.Code1L:
                        return (int) StandardAminoAcids.Threonine;
                    case Tryptophan.Code1L:
                        return (int) StandardAminoAcids.Tryptophan;
                    case Tyrosine.Code1L:
                        return (int) StandardAminoAcids.Tyrosine;
                    case Valine.Code1L:
                        return (int) StandardAminoAcids.Valine;
                    case Selenocysteine.Code1L:
                        return (int) AdditionalAminoAcids.Selenocysteine;
                    case Pyrrolysine.Code1L:
                        return (int) AdditionalAminoAcids.Pyrrolysine;
                    case AsparagineOrAsparticAcid.Code1L:
                        return (int) AmbiguousAminoAcids.AsparagineOrAsparticAcid;
                    case GlutamineOrGlutamicAcid.Code1L:
                        return (int) AmbiguousAminoAcids.GlutamineOrGlutamicAcid;
                    case LeucineOrIsoleucine.Code1L:
                        return (int) AmbiguousAminoAcids.LeucineOrIsoleucine;
                    case UnspecifiedOrUnknownAminoAcid.Code1L:
                        return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
                    default:
                        return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
                }
            }

            if (aminoAcidCode.Length == 3)
            {
                switch (aminoAcidCode)
                {
                    case Alanine.Code3L:
                        return (int) StandardAminoAcids.Alanine;
                    case Arginine.Code3L:
                        return (int) StandardAminoAcids.Arginine;
                    case Asparagine.Code3L:
                        return (int) StandardAminoAcids.Asparagine;
                    case AsparticAcid.Code3L:
                        return (int) StandardAminoAcids.AsparticAcid;
                    case Cysteine.Code3L:
                        return (int) StandardAminoAcids.Cysteine;
                    case GlutamicAcid.Code3L:
                        return (int) StandardAminoAcids.GlutamicAcid;
                    case Glutamine.Code3L:
                        return (int) StandardAminoAcids.Glutamine;
                    case Glycine.Code3L:
                        return (int) StandardAminoAcids.Glycine;
                    case Histidine.Code3L:
                        return (int) StandardAminoAcids.Histidine;
                    case Isoleucine.Code3L:
                        return (int) StandardAminoAcids.Isoleucine;
                    case Leucine.Code3L:
                        return (int) StandardAminoAcids.Leucine;
                    case Lysine.Code3L:
                        return (int) StandardAminoAcids.Lysine;
                    case Methionine.Code3L:
                        return (int) StandardAminoAcids.Methionine;
                    case Phenylalanine.Code3L:
                        return (int) StandardAminoAcids.Phenylalanine;
                    case Proline.Code3L:
                        return (int) StandardAminoAcids.Proline;
                    case Serine.Code3L:
                        return (int) StandardAminoAcids.Serine;
                    case Threonine.Code3L:
                        return (int) StandardAminoAcids.Threonine;
                    case Tryptophan.Code3L:
                        return (int) StandardAminoAcids.Tryptophan;
                    case Tyrosine.Code3L:
                        return (int) StandardAminoAcids.Tyrosine;
                    case Valine.Code3L:
                        return (int) StandardAminoAcids.Valine;
                    case Selenocysteine.Code3L:
                        return (int) AdditionalAminoAcids.Selenocysteine;
                    case Pyrrolysine.Code3L:
                        return (int) AdditionalAminoAcids.Pyrrolysine;
                    case AsparagineOrAsparticAcid.Code3L:
                        return (int) AmbiguousAminoAcids.AsparagineOrAsparticAcid;
                    case GlutamineOrGlutamicAcid.Code3L:
                        return (int) AmbiguousAminoAcids.GlutamineOrGlutamicAcid;
                    case LeucineOrIsoleucine.Code3L:
                        return (int) AmbiguousAminoAcids.LeucineOrIsoleucine;
                    case UnspecifiedOrUnknownAminoAcid.Code3L:
                        return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
                    case UnspecifiedOrUnknownAminoAcid.Code3LAlt:
                        return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
                    case PyroglutamicAcid.Code3L:
                        return (int)NonStandardAminoAcids.PyroglutamicAcid;
                    case Selenomethionine.Code3L:
                        return (int) NonStandardAminoAcids.Selenomethionine;

                    default:
                        return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
                }
            }
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            aminoAcidCode = textInfo.ToTitleCase(aminoAcidCode);

            switch (aminoAcidCode)
            {
                case Alanine.Name:
                    return (int) StandardAminoAcids.Alanine;
                case Arginine.Name:
                    return (int) StandardAminoAcids.Arginine;
                case Asparagine.Name:
                    return (int) StandardAminoAcids.Asparagine;
                case AsparticAcid.Name:
                    return (int) StandardAminoAcids.AsparticAcid;
                case Cysteine.Name:
                    return (int) StandardAminoAcids.Cysteine;
                case GlutamicAcid.Name:
                    return (int) StandardAminoAcids.GlutamicAcid;
                case Glutamine.Name:
                    return (int) StandardAminoAcids.Glutamine;
                case Glycine.Name:
                    return (int) StandardAminoAcids.Glycine;
                case Histidine.Name:
                    return (int) StandardAminoAcids.Histidine;
                case Isoleucine.Name:
                    return (int) StandardAminoAcids.Isoleucine;
                case Leucine.Name:
                    return (int) StandardAminoAcids.Leucine;
                case Lysine.Name:
                    return (int) StandardAminoAcids.Lysine;
                case Methionine.Name:
                    return (int) StandardAminoAcids.Methionine;
                case Phenylalanine.Name:
                    return (int) StandardAminoAcids.Phenylalanine;
                case Proline.Name:
                    return (int) StandardAminoAcids.Proline;
                case Serine.Name:
                    return (int) StandardAminoAcids.Serine;
                case Threonine.Name:
                    return (int) StandardAminoAcids.Threonine;
                case Tryptophan.Name:
                    return (int) StandardAminoAcids.Tryptophan;
                case Tyrosine.Name:
                    return (int) StandardAminoAcids.Tyrosine;
                case Valine.Name:
                    return (int) StandardAminoAcids.Valine;
                case Selenocysteine.Name:
                    return (int) AdditionalAminoAcids.Selenocysteine;
                case Pyrrolysine.Name:
                    return (int) AdditionalAminoAcids.Pyrrolysine;
                case AsparagineOrAsparticAcid.Name:
                    return (int) AmbiguousAminoAcids.AsparagineOrAsparticAcid;
                case GlutamineOrGlutamicAcid.Name:
                    return (int) AmbiguousAminoAcids.GlutamineOrGlutamicAcid;
                case LeucineOrIsoleucine.Name:
                    return (int) AmbiguousAminoAcids.LeucineOrIsoleucine;
                case UnspecifiedOrUnknownAminoAcid.Name:
                    return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
                case PyroglutamicAcid.Name:
                    return (int)NonStandardAminoAcids.PyroglutamicAcid;
                case Selenomethionine.Name:
                    return (int)NonStandardAminoAcids.Selenomethionine;
                default:
                    return (int) AmbiguousAminoAcids.UnspecifiedOrUnknownAminoAcid;
            }
        }
    }
}