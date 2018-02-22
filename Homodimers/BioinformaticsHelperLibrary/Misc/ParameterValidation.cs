//-----------------------------------------------------------------------
// <copyright file="Spreadsheets.cs" company="Aaron Caffrey">
//     Copyright (c) 2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioinformaticsHelperLibrary.AminoAcids;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class ParameterValidation
    {
        public static bool IsProteinChainListContainerNullOrEmpty(ProteinChainListContainer pdbFileChains)
        {
            if (pdbFileChains == null || pdbFileChains.ChainList == null || pdbFileChains.ChainList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsClusteringFullResultListContainerNullOrEmpty(ClusteringFullResultListContainer clusteringFullResultListContainer)
        {
            if (clusteringFullResultListContainer == null || clusteringFullResultListContainer.ChainList == null || clusteringFullResultListContainer.ChainList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsIntArrayNullOrEmpty(int[] intArray)
        {
            if (intArray == null || intArray.Length == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsIntArrayNullOrEmpty(int[,] intArray)
        {
            if (intArray == null || intArray.Length == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsDecimalArrayNullOrEmpty(decimal[,] decimalArray)
        {
            if (decimalArray == null || decimalArray.Length == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsDecimalArrayNullOrEmpty(decimal[][,] decimalArray)
        {
            if (decimalArray == null || decimalArray.Length == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsClusterNullOrEmpty(ClusteringFullResultListContainer.Chain.Stage.Cluster cluster)
        {
            if (cluster == null || cluster.AtomIndexList == null || cluster.AtomIndexList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsProteinInterfaceNullOrEmpty(ClusteringResultProteinInterfaceListContainer.Chain.ProteinInterface proteinInterface)
        {
            if (proteinInterface == null || proteinInterface.AtomList == null || proteinInterface.AtomList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsClusteringResultProteinInterfaceListContainerNullOrEmpty(ClusteringResultProteinInterfaceListContainer clusteringResultProteinInterfaceListContainer)
        {
            if (clusteringResultProteinInterfaceListContainer == null || clusteringResultProteinInterfaceListContainer.ChainList == null || clusteringResultProteinInterfaceListContainer.ChainList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsChainIndexInvalid(int chainIndex, bool allowNegativeValues = false)
        {
            if (chainIndex < 0 && !allowNegativeValues)
            {
                return true;
            }
            return false;
        }

        public static bool IsProteinInterfaceIndexInvalid(int proteinInterfaceIndex, bool allowNegativeValues = false)
        {
            if (proteinInterfaceIndex < 0 && !allowNegativeValues)
            {
                return true;
            }
            return false;
        }

        public static bool IsResidueSequenceIndexInvalid(int residueSequenceIndex, bool allowNegativeValues = false)
        {
            if (residueSequenceIndex < 0 && !allowNegativeValues)
            {
                return true;
            }
            return false;
        }

        public static bool IsAtomNullOrEmpty(ATOM_Record atom)
        {
            if (atom == null || string.IsNullOrWhiteSpace(atom.resName.FieldValue))
            {
                return true;
            }
            return false;
        }

        public static bool IsSaveAsFilenameInvalid(string saveAsFilename)
        {
            if (string.IsNullOrWhiteSpace(saveAsFilename))
            {
                return true;
            }
            return false;
        }

        public static bool IsLoadFilenameInvalid(string loadFilename)
        {
            if (string.IsNullOrWhiteSpace(loadFilename) || !File.Exists(loadFilename))
            {
                return true;
            }
            return false;
        }

        public static bool IsFastaFilenamesArrayInvalid(string[] fastaFiles)
        {
            if (fastaFiles == null || fastaFiles.Length == 0 || fastaFiles.Count(a => !string.IsNullOrWhiteSpace(a)) == 0)
            {
                return true;
            }
            foreach (string fastaFile in fastaFiles.Where(a => !string.IsNullOrWhiteSpace(a)))
            {
                if (!File.Exists(fastaFile))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPdbFilesFoldersArrayInvalid(string[] pdbFilesFolders)
        {
            if (pdbFilesFolders == null || pdbFilesFolders.Length == 0 || pdbFilesFolders.Count(a => !string.IsNullOrWhiteSpace(a)) == 0)
            {
                return true;
            }
            foreach (string pdbFilesFolder in pdbFilesFolders.Where(a => !string.IsNullOrWhiteSpace(a)))
            {
                if (!Directory.Exists(pdbFilesFolder))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsProteinAtomListContainerNullOrEmpty(ProteinAtomListContainer proteinAtomListContainer)
        {
            if (proteinAtomListContainer == null || proteinAtomListContainer.AtomList == null || proteinAtomListContainer.AtomList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsListNullOrEmpty<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsDictionaryNullOrEmpty<T, Y>(Dictionary<T, Y> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsInteractionBetweenProteinInterfacesListContainerNullOrEmpty(InteractionBetweenProteinInterfacesListContainer interactionBetweenProteinInterfacesListContainer)
        {
            if (interactionBetweenProteinInterfacesListContainer == null)
            {
                return true;
            }

            if ((interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList == null || interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList.Count == 0) &&
                (interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList == null || interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList.Count == 0))
            {
                return true;
            }

            return false;
        }

        public static bool IsAminoAcidCodeValid(char aminoAcidNameOrCode)
        {
            int aminoAcidNumber = AminoAcidConversions.AminoAcidNameToNumber(aminoAcidNameOrCode);

            return IsAminoAcidNumberValid(aminoAcidNumber);
        }

        public static bool IsAminoAcidCodeValid(string aminoAcidNameOrCode)
        {
            int aminoAcidNumber = AminoAcidConversions.AminoAcidNameToNumber(aminoAcidNameOrCode);

            return IsAminoAcidNumberValid(aminoAcidNumber);
        }

        public static bool IsAminoAcidNumberValid(int aminoAcidNumber)
        {
            if (Enum.IsDefined(typeof(AminoAcids.Standard.StandardAminoAcids1L),aminoAcidNumber) || 
                Enum.IsDefined(typeof(AminoAcids.Additional.AdditionalAminoAcids1L),aminoAcidNumber) || 
                Enum.IsDefined(typeof(AminoAcids.Ambiguous.AmbiguousAminoAcids1L),aminoAcidNumber) || 
                Enum.IsDefined(typeof(AminoAcids.NonStandard.NonStandardAminoAcids1L),aminoAcidNumber))
            {
                return true;
            }
            return false;
        }

        public static bool IsStringNullOrEmpty(string str, bool allowWhiteSpace = false)
        {
            return (!allowWhiteSpace && string.IsNullOrWhiteSpace(str)) || (string.IsNullOrEmpty(str));
        }

        public static string NullEmptyOrWhiteSpaceToString(string nullEmptyOrWhiteSpaceString)
        {
            if (nullEmptyOrWhiteSpaceString == null)
            {
                return "null";
            }

            if (nullEmptyOrWhiteSpaceString.Length == 0)
            {
                return "empty";
            }

            if (nullEmptyOrWhiteSpaceString.Trim().Length == 0)
            {
                return "white space";
            }

            throw new ArgumentOutOfRangeException(nameof(nullEmptyOrWhiteSpaceString), "string was not null, empty or white space");
        }
    }
}