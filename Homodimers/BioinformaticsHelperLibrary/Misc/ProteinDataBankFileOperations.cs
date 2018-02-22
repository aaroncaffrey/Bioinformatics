//-----------------------------------------------------------------------
// <copyright file="ProteinDataBankFileOperations.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using BioinformaticsHelperLibrary.AminoAcids.Ambiguous.AmbiguousTypes;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.UserProteinInterface;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class ProteinDataBankFileOperations
    {

        public static Dictionary<string, List<string>> PdbIdChainIdList(List<ISequence> sequenceList)
        {
            var result = new Dictionary<string, List<string>>();
            
            foreach (var seq in sequenceList)
            {
                var seqId = SequenceIdSplit.SequenceIdToPdbIdAndChainId(seq.ID);

                if (!result.ContainsKey(seqId.PdbId)) result.Add(seqId.PdbId, new List<string>());

                result[seqId.PdbId].Add(seqId.ChainId);
            }

            return result;
        }


        public static string[] ExpectedPdbFilenames(string folder, string pdbId)
        {
            return new[]
            {
                FileAndPathMethods.MergePathAndFilename(folder, pdbId + ".pdb"),
                FileAndPathMethods.MergePathAndFilename(folder, "pdb" + pdbId + ".ent")
            };
        }

        public static int FindChainByLetter(ProteinChainListContainer pdb, string chainIdLetter)
        {
            for (int index = 0; index < pdb.ChainList.Count; index++)
            {
                var chain = pdb.ChainList[index];

                if (chain.AtomList.Count == 0) continue;

                if (chain.AtomList[0].chainID.FieldValue == chainIdLetter)
                {
                    return index;
                }
            }

            return -1;
        }

        public static ProteinChainListContainer AtomPairListToUnpairedAtomLists(List<AtomPair> atomPairList, bool distinct = false)
        {
            var proteinChainSequenceListContainer = new ProteinChainListContainer {ChainList = new List<ProteinAtomListContainer>()};

            ProteinAtomListContainer atoms1 = GetListOfAtomFromAtomPair(atomPairList, 1, distinct);
            proteinChainSequenceListContainer.ChainList.Add(atoms1);

            ProteinAtomListContainer atoms2 = GetListOfAtomFromAtomPair(atomPairList, 2, distinct);
            proteinChainSequenceListContainer.ChainList.Add(atoms2);

            return proteinChainSequenceListContainer;
        }

        /// <summary>
        ///     This method splits a List of AtomPair into separate chains.
        /// </summary>
        /// <param name="atomPairList"></param>
        /// <param name="atomNumberInPair"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        private static ProteinAtomListContainer GetListOfAtomFromAtomPair(List<AtomPair> atomPairList, int atomNumberInPair, bool distinct = false)
        {
            if (atomNumberInPair != 1 && atomNumberInPair != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(atomNumberInPair));
            }

            if (atomPairList == null || atomPairList.Count <= 0)
            {
                //return null;
                throw new ArgumentOutOfRangeException(nameof(atomPairList));
            }

            var result = new ProteinAtomListContainer();

            for (int index = 0; index < atomPairList.Count; index++)
            {
                AtomPair atomPair = atomPairList[index];

                if (atomNumberInPair == 1)
                {
                    result.AtomList.Add(atomPair.Atom1);
                }
                else if (atomNumberInPair == 2)
                {
                    result.AtomList.Add(atomPair.Atom2);
                }
            }

            if (distinct)
            {
                result.AtomList = result.AtomList.Distinct().ToList();
            }

            return result;
        }

        public static ATOM_Record ConvertHetatmRecordToAtomRecord(HETATM_Record hetatm)
        {
            var atom = new ATOM_Record("ATOM  " + hetatm.ColumnFormatLine.Substring(6));

            return atom;
        }


        public static int PdbModelCount(string pdbFilename, int maximumToFind = -1)
        {
            if (!File.Exists(pdbFilename))
            {
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            // Load pdb/protein file, excluding all records but ATOM, HETATM and TER.
            var proteinDataBankFile = new ProteinDataBankFile(pdbFilename, new[]
            {
                MODEL_Record.MODEL_Field.FieldName
                //ProteinDataBankFile.ENDMDL_Record.ENDMDL_Field.FieldName,
            });

            int modelCount = 0;
            //var endModelCount = 0;

            for (int proteinDataBankFileRecordIndex = 0; proteinDataBankFileRecordIndex < proteinDataBankFile.Count; proteinDataBankFileRecordIndex++)
            {
                ProteinDataBankFileRecord currentRecord = proteinDataBankFile.NextRecord();

                if (currentRecord == null)
                {
                    continue;
                }

                if (currentRecord.GetType() == typeof (MODEL_Record))
                {
                    var model = (MODEL_Record) currentRecord;
                    modelCount++;

                    if (maximumToFind > -1 && modelCount > maximumToFind)
                    {
                        break;
                    }
                }
                //else if (currentRecord.GetType() == typeof(ProteinDataBankFile.ENDMDL_Record))
                //{
                //    var endModel = (ProteinDataBankFile.ENDMDL_Record)currentRecord;
                //    endModelCount++;
                //}
            }

            return modelCount;
        }

        public static List<string> PdbAtomAcidList(string pdbFilename, string[] chainIdWhiteList=null, bool onlyCarbonAlphas =true, bool distinct = true)
        {
            if (!File.Exists(pdbFilename))
            {
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            // Load pdb/protein file, excluding all records but ATOM, HETATM and TER.
            var proteinDataBankFile = new ProteinDataBankFile(pdbFilename, new[]
            {
                ATOM_Record.ATOM_Field.FieldName,
                HETATM_Record.HETATM_Field.FieldName,
                //TER_Record.TER_Field.FieldName
            });

            var atomAcidList = new List<string>();

            for (int proteinDataBankFileRecordIndex = 0; proteinDataBankFileRecordIndex < proteinDataBankFile.Count; proteinDataBankFileRecordIndex++)
            {
                ProteinDataBankFileRecord currentRecord = proteinDataBankFile.NextRecord();

                if (currentRecord == null)
                {
                    continue;
                }

                if (currentRecord.GetType() == typeof(ATOM_Record))
                {
                    var atom = (ATOM_Record)currentRecord;

                    string chainIdKey = atom.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    if (onlyCarbonAlphas && atom.name.FieldValue.Trim().ToUpperInvariant() != StaticValues.CarbonAlpha)
                    {
                        continue;
                    }

                    if (!distinct || !atomAcidList.Contains(atom.resName.FieldValue))
                    {
                        atomAcidList.Add(atom.resName.FieldValue);
                    }
                }
                else if (currentRecord.GetType() == typeof(HETATM_Record))
                {
                    var hetatm = (HETATM_Record)currentRecord;

                    string chainIdKey = hetatm.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    if (onlyCarbonAlphas && hetatm.name.FieldValue.Trim().ToUpperInvariant() != StaticValues.CarbonAlpha)
                    {
                        continue;
                    }

                    if (!distinct || !atomAcidList.Contains(hetatm.resName.FieldValue))
                    {
                        atomAcidList.Add(hetatm.resName.FieldValue);
                    }
                }
                //else if (currentRecord.GetType() == typeof (HETATM_Record))
                //{
                //    var ter = (HETATM_Record)currentRecord;

                //    string chainIdKey = ter.chainID.FieldValue.Trim().ToUpperInvariant();

                //    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                //    {
                //        continue;
                //    }
                //}
            }


            return atomAcidList;
        }

        public static int PdbAtomicChainsCount(string pdbFilename, string[] chainIdWhiteList=null, int maximumToFind = -1)
        {
            if (!File.Exists(pdbFilename))
            {
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            // Load pdb/protein file, excluding all records but ATOM, HETATM and TER.
            var proteinDataBankFile = new ProteinDataBankFile(pdbFilename, new[]
            {
                ATOM_Record.ATOM_Field.FieldName,
                HETATM_Record.HETATM_Field.FieldName,
                //TER_Record.TER_Field.FieldName
                MODEL_Record.MODEL_Field.FieldName,
                ENDMDL_Record.ENDMDL_Field.FieldName
            });


            int atomCount = 0;
            int hetAtomCount = 0;
            var terCount = 0;

            var chainNames = new List<string>();

            for (int proteinDataBankFileRecordIndex = 0; proteinDataBankFileRecordIndex < proteinDataBankFile.Count; proteinDataBankFileRecordIndex++)
            {
                ProteinDataBankFileRecord currentRecord = proteinDataBankFile.NextRecord();

                if (currentRecord == null)
                {
                    continue;
                }

                if (currentRecord.GetType() == typeof (ATOM_Record))
                {
                    var atom = (ATOM_Record) currentRecord;

                    string chainIdKey = atom.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    atomCount++;

                    if (!string.IsNullOrWhiteSpace(atom.chainID.FieldValue) && !chainNames.Contains(atom.chainID.FieldValue))
                    {
                        chainNames.Add(atom.chainID.FieldValue);

                        if (maximumToFind > -1 && chainNames.Count > maximumToFind)
                        {
                            break;
                        }
                    }
                }
                else if (currentRecord.GetType() == typeof(HETATM_Record))
                {
                    var hetatm = (HETATM_Record) currentRecord;

                    string chainIdKey = hetatm.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    hetAtomCount++;

                    if (!string.IsNullOrWhiteSpace(hetatm.chainID.FieldValue) && !chainNames.Contains(hetatm.chainID.FieldValue))
                    {
                        chainNames.Add(hetatm.chainID.FieldValue);

                        if (maximumToFind > -1 && chainNames.Count > maximumToFind)
                        {
                            break;
                        }
                    }
                }
                else if (currentRecord.GetType() == typeof (ENDMDL_Record))
                {
                    break;
                }
                else if (currentRecord.GetType() == typeof (TER_Record))
                {
                    var ter = (TER_Record) currentRecord;

                    string chainIdKey = ter.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    terCount++;

                    if (!string.IsNullOrWhiteSpace(ter.chainID.FieldValue) && !chainNames.Contains(ter.chainID.FieldValue))
                    {
                        chainNames.Add(ter.chainID.FieldValue);
                    }
                }
            }

            int chainNamesCount = chainNames.Distinct().Count();

            //var chainCount = chainNamesCount > terCount ? chainNamesCount : terCount;

            return chainNamesCount;
        }

        /// <summary>
        ///     This method loads 1 pdb file and returns the atoms contained in the different chains.
        /// </summary>
        /// <param name="pdbFilename"></param>
        /// <param name="chainIdWhiteList"></param>
        /// <param name="minimumChains"></param>
        /// <param name="maximumChains"></param>
        /// <returns></returns>
        public static ProteinChainListContainer PdbAtomicChains(string pdbFilename, string[] chainIdWhiteList, int minimumChains = 2, int maximumChains = 2, bool onlyCarbonAlphas = false)
        {            
            ////////Console.WriteLine(pdbFilename);
            // Check file exists.
            if (!File.Exists(pdbFilename))
            {
                //return null;
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            // Check min chains not more than max chains.
            if (minimumChains > maximumChains)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumChains));
            }

            // Load pdb/protein file, excluding all records but ATOM, HETATM and TER.
            var proteinDataBankFile = new ProteinDataBankFile(pdbFilename, new[]
            {
                ATOM_Record.ATOM_Field.FieldName,
                HETATM_Record.HETATM_Field.FieldName,
                TER_Record.TER_Field.FieldName,
                MODEL_Record.MODEL_Field.FieldName,
                ENDMDL_Record.ENDMDL_Field.FieldName
            });


            // Make new array for atom chain.
            //List<ATOM_Record>[] proteinFileChains = new List<ATOM_Record>[maximumChains];
            var pdbFileChains = new ProteinChainListContainer();

            //var fileError = false;
            //var chainCount = 0;
            // Loop through all the previously loaded protein file records to make lists of atoms in each chain.
            // Also make a list of residue numbers (which will be sorted later just in case it is out of order).

            var atomRecordListDictionary = new Dictionary<string, List<ProteinDataBankFileRecord>>();
            var hetAtomRecordListDictionary = new Dictionary<string, List<ProteinDataBankFileRecord>>();
            int terCount = 0;

            for (int proteinDataBankFileRecordIndex = 0; proteinDataBankFileRecordIndex < proteinDataBankFile.Count; proteinDataBankFileRecordIndex++)
            {
                ProteinDataBankFileRecord currentRecord = proteinDataBankFile.NextRecord();

                if (currentRecord == null)
                {
                    continue;
                }

                if (currentRecord.GetType() == typeof (ATOM_Record))
                {
                    var atom = (ATOM_Record) currentRecord;

                    if (onlyCarbonAlphas && atom.name.FieldValue.Trim().ToUpperInvariant() != StaticValues.CarbonAlpha)
                    {
                        continue;
                    }

                    string chainIdKey = atom.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    if (!atomRecordListDictionary.ContainsKey(chainIdKey))
                    {
                        atomRecordListDictionary.Add(chainIdKey, new List<ProteinDataBankFileRecord>());
                    }

                    if (ParameterValidation.IsAminoAcidCodeValid(atom.resName.FieldValue))
                    {
                        atomRecordListDictionary[chainIdKey].Add(atom);
                    }
                    
                }
                else if (currentRecord.GetType() == typeof (HETATM_Record))
                {
                    var hetatm = (HETATM_Record) currentRecord;

                    if (onlyCarbonAlphas && hetatm.name.FieldValue.Trim().ToUpperInvariant() != StaticValues.CarbonAlpha)
                    {
                        continue;
                    }

                    string chainIdKey = hetatm.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    if (!hetAtomRecordListDictionary.ContainsKey(chainIdKey))
                    {
                        hetAtomRecordListDictionary.Add(chainIdKey, new List<ProteinDataBankFileRecord>());
                    }

                    //if (!ParameterValidation.IsAminoAcidCodeValid(hetatm.resName.FieldValue))
                    //{
                    //    ////////Console.WriteLine(hetatm.columnFormatLine);
                    //    hetatm.resName.FieldValue = UnspecifiedOrUnknownAminoAcid.Code3L;
                    //    hetatm.columnFormatLine = hetatm.columnFormatLine.Remove(ProteinDataBankFile.HETATM_Record.resName_Field.FirstColumn - 1, (ProteinDataBankFile.HETATM_Record.resName_Field.LastColumn - ProteinDataBankFile.HETATM_Record.resName_Field.FirstColumn) + 1);
                    //    hetatm.columnFormatLine = hetatm.columnFormatLine.Insert(ProteinDataBankFile.HETATM_Record.resName_Field.FirstColumn - 1, UnspecifiedOrUnknownAminoAcid.Code3L);
                    //    ////////Console.WriteLine(hetatm.columnFormatLine);
                    //}

                    if (ParameterValidation.IsAminoAcidCodeValid(hetatm.resName.FieldValue))
                    {
                        hetAtomRecordListDictionary[chainIdKey].Add(hetatm);
                    }
                }
                else if (currentRecord.GetType() == typeof(TER_Record))
                {
                    var ter = (TER_Record) currentRecord;

                    string chainIdKey = ter.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    terCount++;

                    if (terCount >= maximumChains)
                    {
                        break;
                        //return null;
                    }
                }
                else if (currentRecord.GetType() == typeof (ENDMDL_Record))
                {
                    break;
                }
            }

            // file has been parsed so clear used file data from memory as soon as possible
            proteinDataBankFile.UnloadFile();

            int totalChains = atomRecordListDictionary.Count > hetAtomRecordListDictionary.Count ? atomRecordListDictionary.Count : hetAtomRecordListDictionary.Count;

            for (int chainIndex = 0; chainIndex < totalChains; chainIndex++)
            {
                pdbFileChains.ChainList.Add(new ProteinAtomListContainer());
            }

            atomRecordListDictionary = atomRecordListDictionary.OrderBy(a => a.Key).ToDictionary(a => a.Key, a => a.Value);

            int chainIndex2 = -1;
            foreach (var atomRecordListKvp in atomRecordListDictionary)
            {
                chainIndex2++;

                string chainName = atomRecordListKvp.Key;
                List<ProteinDataBankFileRecord> chainRecords = atomRecordListKvp.Value;

                if (chainRecords == null || chainRecords.Count == 0)
                {
                    continue;
                }

                chainRecords = chainRecords.OrderBy(a => NullableTryParseInt32(((ATOM_Record) a).serial.FieldValue)).ToList();

                pdbFileChains.ChainList[chainIndex2].AtomList = chainRecords.Select(a => (ATOM_Record) a).ToList();
            }

            hetAtomRecordListDictionary = hetAtomRecordListDictionary.OrderBy(a => a.Key).ToDictionary(a => a.Key, a => a.Value);

            int chainIndex3 = -1;
            foreach (var hetAtomRecordListKvp in hetAtomRecordListDictionary)
            {
                chainIndex3++;
                string chainName = hetAtomRecordListKvp.Key;
                List<ProteinDataBankFileRecord> chainRecords = hetAtomRecordListKvp.Value;

                if (chainRecords == null || chainRecords.Count == 0)
                {
                    continue;
                }

                chainRecords = chainRecords.OrderBy(a => NullableTryParseInt32(((HETATM_Record) a).serial.FieldValue)).ToList();

                foreach (ProteinDataBankFileRecord proteinDataBankFileRecord in chainRecords)
                {
                    var chainRecord = (HETATM_Record) proteinDataBankFileRecord;

                    string residueSequenceToFind = chainRecord.resSeq.FieldValue;
                    string atomChainId = chainRecord.chainID.FieldValue.Trim().ToUpperInvariant();

                    if (!atomRecordListDictionary.ContainsKey(atomChainId) || atomRecordListDictionary[atomChainId].Count(a => ((ATOM_Record) a).resSeq.FieldValue == residueSequenceToFind) == 0)
                    {
                        ATOM_Record atom = ConvertHetatmRecordToAtomRecord(chainRecord);

                        pdbFileChains.ChainList[chainIndex3].AtomList.Add(atom);
                    }
                }
            }

            int nonEmptyChainCount = pdbFileChains.ChainList.Count(a => a != null && a.AtomList != null && a.AtomList.Count > 0);

            if (nonEmptyChainCount >= minimumChains && nonEmptyChainCount <= maximumChains)
            {
                return pdbFileChains;
            }

            ////////Console.WriteLine("Too many chains (" + nonEmptyChainCount + "): " + pdbFilename);
            return null;
        }

        public static int? NullableTryParseInt32(string str)
        {
            int intValue;
            return int.TryParse(str, out intValue) ? (int?) intValue : null;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static string[] RemoveNonWhiteListedPdbIdFromPdbFilesArray(List<string> pdbIdWhiteList,
            string[] pdbFilesArray)
        {
            if ((pdbIdWhiteList == null) || (pdbFilesArray == null))
            {
                return null;
            }

            return pdbFilesArray.Where(s => pdbIdWhiteList.Contains(PdbIdFromPdbFilename(s))).ToArray();
        }

        public static string[] PdbIdBadList = new string[]
        {
                 "1GTV", "2XGC", "2XGE", "1CWQ"
        };

        /// <summary>
        ///     This method returns a list of *.PDB/*.ENT files found in the specified folders.
        /// </summary>
        /// <param name="pdbFilesFolders"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static string[] GetPdbFilesArray(string[] pdbFilesFolders, bool distinct = true)
        {
            var result = new List<string>();

            foreach (string pdbFilesFolder in pdbFilesFolders)
            {
                var filesArrayPdb = Directory.GetFiles(pdbFilesFolder, "*.pdb", SearchOption.AllDirectories);
                var filesArrayEnt = Directory.GetFiles(pdbFilesFolder, "*.ent", SearchOption.AllDirectories);
                var filesArrayBrk = Directory.GetFiles(pdbFilesFolder, "*.brk", SearchOption.AllDirectories);

                result.AddRange(filesArrayPdb);
                result.AddRange(filesArrayEnt);
                result.AddRange(filesArrayBrk);
            }

            result = result.Select(FileAndPathMethods.EnforceSlashConvention).ToList();

            result = result.Where(a => !PdbIdBadList.Contains(ProteinDataBankFileOperations.PdbIdFromPdbFilename(a).ToUpperInvariant())).ToList();

            if (distinct)
            {
                result = result.Distinct().ToList();
            }

            return result.ToArray();
        }


        /// <summary>
        ///     This method finds the PDB ID from a PDB file's filename.
        /// </summary>
        /// <param name="pdbFilename"></param>
        /// <returns></returns>
        public static string PdbIdFromPdbFilename(string pdbFilename)
        {
            const int proteinIdLength = 4;

            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename), pdbFilename, "parameter was " + ParameterValidation.NullEmptyOrWhiteSpaceToString(pdbFilename));
            }

            var proteinId = Path.GetFileNameWithoutExtension(pdbFilename).Trim(); 

            if (proteinId.Length > proteinIdLength)
            {
                proteinId = proteinId.Substring(3);//.Replace("pdb", "");
            }

            if (proteinId.Length != proteinIdLength)
            {
                throw new ArgumentException("PDB ID could not be extracted from parameter pdbFilename", nameof(pdbFilename));
            }

            return proteinId.ToUpperInvariant();
        }


        /// <summary>
        ///     Returns a list of PDB ID's for which the corresponding .pdb files were not found
        /// </summary>
        /// <param name="pdbFilesArray">A list of PDB files (*.pdb)</param>
        /// <param name="pdbIdList">A list of PDB Unique ID's</param>
        /// <returns></returns>
        public static List<string> CheckForMissingPdbFiles(string[] pdbFilesArray, List<string> pdbIdList)
        {
            if (pdbFilesArray == null || pdbFilesArray.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilesArray));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            // Check if any required pdb files are missing.
            var pdbIdListCheck = new List<string>(pdbIdList);

            foreach (string proteinId in pdbFilesArray.Select(PdbIdFromPdbFilename))
            {
                pdbIdListCheck.Remove(proteinId);
            }

            return pdbIdListCheck;
        }


        public static List<string> CheckForUnusedPdbFiles(string[] pdbFilesArray, List<string> pdbIdList)
        {
            if (pdbFilesArray == null || pdbFilesArray.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilesArray));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            return pdbFilesArray.Select(PdbIdFromPdbFilename).Where(proteinId => !pdbIdList.Contains(proteinId)).ToList();
        }

        /// <summary>
        ///     Informs the user about any pdb files which are required but missing from the locations given.
        /// </summary>
        /// <param name="pdbFilesArray">A list of pdb files found.</param>
        /// <param name="pdbIdList">A list of required PDB IDs.</param>
        /// <param name="consoleTextBox">The user proteinInterface textbox for user feedback.</param>
        public static void ShowMissingPdbFiles(string[] pdbFilesArray, List<string> pdbIdList, ProgressActionSet progressActionSet, bool showMissing=true, bool showUnused=true)
        {
            if (pdbFilesArray == null || pdbFilesArray.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilesArray));
            }

            if (pdbIdList == null || pdbIdList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pdbIdList));
            }

            if (progressActionSet == null)
            {
                throw new ArgumentNullException(nameof(progressActionSet));
            }

            if (showMissing)
            {
                List<string> pdbIdListCheck = CheckForMissingPdbFiles(pdbFilesArray, pdbIdList);

                if (pdbIdListCheck.Count > 0)
                {
                    ProgressActionSet.Report("There are " + pdbIdListCheck.Count + " PDB files missing:", progressActionSet);
                    ProgressActionSet.Report(string.Join(", ", pdbIdListCheck), progressActionSet);
                }
                else
                {
                    ProgressActionSet.Report("There are not any PDB files missing - all are accounted for.", progressActionSet);
                }
            }

            if (showUnused)
            {
                List<string> pdbIdUnused = CheckForUnusedPdbFiles(pdbFilesArray, pdbIdList);

                if (pdbIdUnused.Count > 0)
                {
                    ProgressActionSet.Report("There are " + pdbIdUnused.Count + " unused PDB files found:", progressActionSet);
                    ProgressActionSet.Report(string.Join(", ", pdbIdUnused), progressActionSet);
                }
                else
                {
                    ProgressActionSet.Report("There are not any unused PDB files - all are loaded.", progressActionSet);
                }
            }
        }
    }
}