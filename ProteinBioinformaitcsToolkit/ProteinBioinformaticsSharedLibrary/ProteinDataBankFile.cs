//-----------------------------------------------------------------------
// <copyright file="ProteinDataBankFile.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;

namespace ProteinBioinformaticsSharedLibrary
{
    /// <summary>
    ///     This class loads and parses a Protein Data Bank file (*.PDB).  The PDB file can be partially loaded with only
    ///     wanted record types, which is useful to save memory when processing extensive data.
    /// </summary>
    [Serializable]
    public class ProteinDataBankFile
    {
        /// <summary>
        ///     Current PDB file record number.
        /// </summary>
        private int _currentFileRecordNumber = -1;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProteinDataBankFile" /> class.
        /// </summary>
        public ProteinDataBankFile()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProteinDataBankFile" /> class.  This constructor also loads a PDB
        ///     file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="targetRecordTypes"></param>
        /// <param name="convertToProteinDataBankFileRecordList"></param>
        public ProteinDataBankFile(string filename, string[] targetRecordTypes = null, bool convertToProteinDataBankFileRecordList = true)
        {
            LoadFile(filename, targetRecordTypes, convertToProteinDataBankFileRecordList);
        }

        public ProteinDataBankFile(string[] structureFileLines, string[] targetRecordTypes = null, bool convertToProteinDataBankFileRecordList = true)
        {
            LoadFile(structureFileLines, targetRecordTypes, convertToProteinDataBankFileRecordList);
        }

        /// <summary>
        ///     Gets FileLinesArray. Used when the user wishes to not parse the data and convert into PDB record classes.
        /// </summary>
        public string[] FileLinesArray { get; private set; }

        /// <summary>
        ///     Gets TargetRecordTypes. The PDB Record Types which should be loaded.
        /// </summary>
        public string[] TargetRecordTypes { get; private set; }

        /// <summary>
        ///     Gets ProteinDataBankFileRecordList. A list of the PDB records which have been loaded.
        /// </summary>
        public List<ProteinDataBankFileRecord> ProteinDataBankFileRecordList { get; private set; }

        /*
        /// <summary>
        ///     Gets the PDB Records as a list of TSV Lines.
        /// </summary>
        public string[][] Lines
        {
            get
            {
                var result = new string[Count][];
                for (int recordIndex = 0; recordIndex < Count; recordIndex++)
                {
                    ProteinDataBankFileRecord proteinDataBankFileRecord = NextRecord();
                    result[recordIndex] = proteinDataBankFileRecord.ToArray();
                }
                return result;
            }
        }
        */

        /// <summary>
        ///     Returns the number of records loaded from the PDB file.
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get
            {
                if (ProteinDataBankFileRecordList != null)
                {
                    return ProteinDataBankFileRecordList.Count;
                }

                if (FileLinesArray != null)
                {
                    return FileLinesArray.Length;
                }

                return 0;
            }

            private set { }
        }

        public void LoadFile(string filename, string[] targetRecordTypes = null, bool parseAll = true)
        {
            UnloadFile();

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("The PDB file specified was not found.", filename);
                //return;
            }

            FileLinesArray = File.ReadAllLines(filename);

            LoadFile(FileLinesArray, targetRecordTypes, parseAll);
        }

        /// <summary>
        ///     Loads a PDB file.  If target record types are specified, it will only load records of those types.
        /// </summary>
        /// <param name="filename">The filename of a PDB file.</param>
        /// <param name="targetRecordTypes">Target PDB file record types.</param>
        /// <param name="parseAll">Parse the whole PDB file into class instances.</param>
        public void LoadFile(string[] structureFileLines, string[] targetRecordTypes = null, bool parseAll = true)
        {
            UnloadFile();

            FileLinesArray = structureFileLines;

            TargetRecordTypes = targetRecordTypes;

            if (parseAll)
            {
                ProteinDataBankFileRecordList = new List<ProteinDataBankFileRecord>();

                for (int lineIndex = 0; lineIndex < FileLinesArray.Length; lineIndex++)
                {
                    ProteinDataBankFileRecord proteinDataBankFileRecord = ProteinDataBaseFileLineRecord(FileLinesArray[lineIndex]);
                    ProteinDataBankFileRecordList.Add(proteinDataBankFileRecord);
                }

                FileLinesArray = null;
            }
        }

        /// <summary>
        ///     Returns the next PDB file record.
        /// </summary>
        /// <returns></returns>
        public ProteinDataBankFileRecord NextRecord()
        {
            _currentFileRecordNumber++;

            if (ProteinDataBankFileRecordList != null && _currentFileRecordNumber < ProteinDataBankFileRecordList.Count)
            {
                return ProteinDataBankFileRecordList[_currentFileRecordNumber];
            }
            if (FileLinesArray != null && _currentFileRecordNumber < FileLinesArray.Length)
            {
                ProteinDataBankFileRecord proteinDataBankFileRecord = ProteinDataBaseFileLineRecord(FileLinesArray[_currentFileRecordNumber]);
                return proteinDataBankFileRecord;
            }
            UnloadFile();
            return null;
        }

        /// <summary>
        ///     Returns a string of the record type for a given line from a PDB file.
        /// </summary>
        /// <param name="columnFormatLine"></param>
        /// <returns></returns>
        public static string ProteinDataBankFileLineRecordType(string columnFormatLine)
        {
            return !string.IsNullOrEmpty(columnFormatLine) ? columnFormatLine.Substring(0, (columnFormatLine.Length >= 6) ? 6 : columnFormatLine.Length).Trim().ToUpperInvariant() : null;
        }

        /// <summary>
        ///     Returns a PDB record class instance of the appropriate type for the given line of PDB file.
        /// </summary>
        /// <param name="columnFormatLine"></param>
        /// <returns></returns>
        public ProteinDataBankFileRecord ProteinDataBaseFileLineRecord(string columnFormatLine)
        {
            ProteinDataBankFileRecord result = null;
            string recordType = ProteinDataBankFileLineRecordType(columnFormatLine);

            if ((TargetRecordTypes != null) && (TargetRecordTypes.Length > 0) && (!TargetRecordTypes.Contains(recordType)))
            {
                return result;
            }

            switch (recordType)
            {
                case "HEADER":
                    result = new HEADER_Record(columnFormatLine);
                    break;
                case "OBSLTE":
                    result = new OBSLTE_Record(columnFormatLine);
                    break;
                case "TITLE":
                    result = new TITLE_Record(columnFormatLine);
                    break;
                case "SPLIT":
                    result = new SPLIT_Record(columnFormatLine);
                    break;
                case "CAVEAT":
                    result = new CAVEAT_Record(columnFormatLine);
                    break;
                case "COMPND":
                    result = new COMPND_Record(columnFormatLine);
                    break;
                case "SOURCE":
                    result = new SOURCE_Record(columnFormatLine);
                    break;
                case "KEYWDS":
                    result = new KEYWDS_Record(columnFormatLine);
                    break;
                case "EXPDTA":
                    result = new EXPDTA_Record(columnFormatLine);
                    break;
                case "NUMMDL":
                    result = new NUMMDL_Record(columnFormatLine);
                    break;
                case "MDLTYP":
                    result = new MDLTYP_Record(columnFormatLine);
                    break;
                case "AUTHOR":
                    result = new AUTHOR_Record(columnFormatLine);
                    break;
                case "REVDAT":
                    result = new REVDAT_Record(columnFormatLine);
                    break;
                case "SPRSDE":
                    result = new SPRSDE_Record(columnFormatLine);
                    break;
                case "JRNL":
                    result = new JRNL_DOI_Record(columnFormatLine);
                    if (((JRNL_DOI_Record) result).DOI.FieldValue == JRNL_DOI_Record.DOI_Field.FieldName) break;

                    result = new JRNL_PMID_Record(columnFormatLine);
                    if (((JRNL_PMID_Record) result).PMID.FieldValue == JRNL_PMID_Record.PMID_Field.FieldName) break;

                    result = new JRNL_PUBL_Record(columnFormatLine);
                    if (((JRNL_PUBL_Record) result).PUBL.FieldValue == JRNL_PUBL_Record.PUBL_Field.FieldName) break;

                    result = new JRNL_REFN_ISSN_or_ESSN_Record(columnFormatLine);
                    if (((JRNL_REFN_ISSN_or_ESSN_Record) result).REFN.FieldValue == JRNL_REFN_ISSN_or_ESSN_Record.REFN_Field.FieldName && (((JRNL_REFN_ISSN_or_ESSN_Record) result).ISSN_or_ESSN.FieldValue.ToUpperInvariant() == "ISSN" || ((JRNL_REFN_ISSN_or_ESSN_Record) result).ISSN_or_ESSN.FieldValue.ToUpperInvariant() == "ESSN")) break;

                    result = new JRNL_REF_V_Record(columnFormatLine);
                    if (((JRNL_REF_V_Record) result).REF.FieldValue == JRNL_REF_V_Record.REF_Field.FieldName && ((JRNL_REF_V_Record) result).V_.FieldValue == JRNL_REF_V_Record.V_Field.FieldName) break;

                    result = new JRNL_REFN_Record(columnFormatLine);
                    if (((JRNL_REFN_Record) result).REFN.FieldValue == JRNL_REFN_Record.REFN_Field.FieldName) break;

                    result = new JRNL_REF_TOBEPUBLISHED_Record(columnFormatLine);
                    if (((JRNL_REF_TOBEPUBLISHED_Record) result).REF.FieldValue == JRNL_REF_TOBEPUBLISHED_Record.REF_Field.FieldName) break;

                    result = new JRNL_Record(columnFormatLine);
                    break;
                case "REMARK":
                    result = new REMARK_1_AUTH_Record(columnFormatLine);
                    if (((REMARK_1_AUTH_Record) result)._1.FieldValue == REMARK_1_AUTH_Record._1_Field.FieldName && ((REMARK_1_AUTH_Record) result).AUTH.FieldValue == REMARK_1_AUTH_Record.AUTH_Field.FieldName) break;

                    result = new REMARK_1_PUBL_Record(columnFormatLine);
                    if (((REMARK_1_PUBL_Record) result)._1.FieldValue == REMARK_1_PUBL_Record._1_Field.FieldName && ((REMARK_1_PUBL_Record) result).PUBL.FieldValue == REMARK_1_PUBL_Record.PUBL_Field.FieldName) break;

                    result = new REMARK_1_REF_V_Record(columnFormatLine);
                    if (((REMARK_1_REF_V_Record) result)._1.FieldValue == REMARK_1_REF_V_Record._1_Field.FieldName && ((REMARK_1_REF_V_Record) result).REF.FieldValue == REMARK_1_REF_V_Record.REF_Field.FieldName && ((REMARK_1_REF_V_Record) result).V_.FieldValue == REMARK_1_REF_V_Record.V_Field.FieldName) break;

                    result = new REMARK_1_REFERENCE_Record(columnFormatLine);
                    if (((REMARK_1_REFERENCE_Record) result)._1.FieldValue == REMARK_1_REFERENCE_Record._1_Field.FieldName && ((REMARK_1_REFERENCE_Record) result).REFERENCE.FieldValue == REMARK_1_REFERENCE_Record.REFERENCE_Field.FieldName) break;

                    result = new REMARK_1_REFN_ISSN_or_ESSN_Record(columnFormatLine);
                    if (((REMARK_1_REFN_ISSN_or_ESSN_Record) result)._1.FieldValue == REMARK_1_REFN_ISSN_or_ESSN_Record._1_Field.FieldName && ((REMARK_1_REFN_ISSN_or_ESSN_Record) result).REFN.FieldValue == REMARK_1_REFN_ISSN_or_ESSN_Record.REFN_Field.FieldName && (((REMARK_1_REFN_ISSN_or_ESSN_Record) result).ISSN_or_ESSN.FieldValue == "ISSN" || ((REMARK_1_REFN_ISSN_or_ESSN_Record) result).ISSN_or_ESSN.FieldValue == "ESSN")) break;

                    result = new REMARK_1_REFN_Record(columnFormatLine);
                    if (((REMARK_1_REFN_Record) result)._1.FieldValue == REMARK_1_REFN_Record._1_Field.FieldName && ((REMARK_1_REFN_Record) result).REFN.FieldValue == REMARK_1_REFN_Record.REFN_Field.FieldName) break;

                    result = new REMARK_1_TITL_Record(columnFormatLine);
                    if (((REMARK_1_TITL_Record) result)._1.FieldValue == REMARK_1_TITL_Record._1_Field.FieldName && ((REMARK_1_TITL_Record) result).TITL.FieldValue == REMARK_1_TITL_Record.TITL_Field.FieldName) break;

                    result = new REMARK_2_RESOLUTION_ANGSTROMS_Record(columnFormatLine);
                    if (((REMARK_2_RESOLUTION_ANGSTROMS_Record) result)._2.FieldValue == REMARK_2_RESOLUTION_ANGSTROMS_Record._2_Field.FieldName && ((REMARK_2_RESOLUTION_ANGSTROMS_Record) result).RESOLUTION_.FieldValue == REMARK_2_RESOLUTION_ANGSTROMS_Record.RESOLUTION_Field.FieldName && ((REMARK_2_RESOLUTION_ANGSTROMS_Record) result).ANGSTROMS_.FieldValue == REMARK_2_RESOLUTION_ANGSTROMS_Record.ANGSTROMS_Field.FieldName) break;

                    result = new REMARK_2_RESOLUTION_NOTAPPLICABLE_Record(columnFormatLine);
                    if (((REMARK_2_RESOLUTION_NOTAPPLICABLE_Record) result)._2.FieldValue == REMARK_2_RESOLUTION_NOTAPPLICABLE_Record._2_Field.FieldName && ((REMARK_2_RESOLUTION_NOTAPPLICABLE_Record) result).RESOLUTION_NOT_APPLICABLE_.FieldValue == REMARK_2_RESOLUTION_NOTAPPLICABLE_Record.RESOLUTION_NOT_APPLICABLE_Field.FieldName) break;

                    result = new REMARK_1_REF_TOBEPUBLISHED_Record(columnFormatLine);
                    if (((REMARK_1_REF_TOBEPUBLISHED_Record) result)._1.FieldValue == REMARK_1_REF_TOBEPUBLISHED_Record._1_Field.FieldName && ((REMARK_1_REF_TOBEPUBLISHED_Record) result).REF.FieldValue == REMARK_1_REF_TOBEPUBLISHED_Record.REF_Field.FieldName && ((REMARK_1_REF_TOBEPUBLISHED_Record) result).TO_BE_PUBLISHED.FieldValue == REMARK_1_REF_TOBEPUBLISHED_Record.TO_BE_PUBLISHED_Field.FieldName) break;

                    result = new REMARK_Record(columnFormatLine);
                    break;
                case "DBREF":
                    result = new DBREF_Record(columnFormatLine);
                    break;
                case "DBREF1":
                    result = new DBREF1_Record(columnFormatLine);
                    break;
                case "DBREF2":
                    result = new DBREF2_Record(columnFormatLine);
                    break;
                case "SEQADV":
                    result = new SEQADV_Record(columnFormatLine);
                    break;
                case "SEQRES":
                    result = new SEQRES_Record(columnFormatLine);
                    break;
                case "MODRES":
                    result = new MODRES_Record(columnFormatLine);
                    break;
                case "HET":
                    result = new HET_Record(columnFormatLine);
                    break;
                case "HETNAM":
                    result = new HETNAM_Record(columnFormatLine);
                    break;
                case "HETSYN":
                    result = new HETSYN_Record(columnFormatLine);
                    break;
                case "FORMUL":
                    result = new FORMUL_Record(columnFormatLine);
                    break;
                case "HELIX":
                    result = new HELIX_Record(columnFormatLine);
                    break;
                case "SHEET":
                    result = new SHEET_Record(columnFormatLine);
                    break;
                case "SSBOND":
                    result = new SSBOND_CYS_CYS_Record(columnFormatLine);
                    break;
                case "LINK":
                    result = new LINK_Record(columnFormatLine);
                    break;
                case "CISPEP":
                    result = new CISPEP_Record(columnFormatLine);
                    break;
                case "SITE":
                    result = new SITE_Record(columnFormatLine);
                    break;
                case "CRYST1":
                    result = new CRYST1_Record(columnFormatLine);
                    break;
                case "ORIGX1":
                    result = new ORIGX1_Record(columnFormatLine);
                    break;
                case "ORIGX2":
                    result = new ORIGX2_Record(columnFormatLine);
                    break;
                case "ORIGX3":
                    result = new ORIGX3_Record(columnFormatLine);
                    break;
                case "SCALE1":
                    result = new SCALE1_Record(columnFormatLine);
                    break;
                case "SCALE2":
                    result = new SCALE2_Record(columnFormatLine);
                    break;
                case "SCALE3":
                    result = new SCALE3_Record(columnFormatLine);
                    break;
                case "MTRIX1":
                    result = new MTRIX1_Record(columnFormatLine);
                    break;
                case "MTRIX2":
                    result = new MTRIX2_Record(columnFormatLine);
                    break;
                case "MTRIX3":
                    result = new MTRIX3_Record(columnFormatLine);
                    break;
                case "MODEL":
                    result = new MODEL_Record(columnFormatLine);
                    break;
                case "ATOM":
                    result = new ATOM_Record(columnFormatLine);
                    break;
                case "ANISOU":
                    result = new ANISOU_Record(columnFormatLine);
                    break;
                case "TER":
                    result = new TER_Record(columnFormatLine);
                    break;
                case "HETATM":
                    result = new HETATM_Record(columnFormatLine);
                    break;
                case "ENDMDL":
                    result = new ENDMDL_Record(columnFormatLine);
                    break;
                case "CONECT":
                    result = new CONECT_Record(columnFormatLine);
                    break;
                case "MASTER":
                    result = new MASTER_0_Record(columnFormatLine);
                    break;
                case "END":
                    result = new END_Record(columnFormatLine);
                    break;
            }
            return result;
        }

        /// <summary>
        ///     ToString override.  Returns a string representation of the currently loaded records of the PDB file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (int recordIndex = 0; recordIndex < Count; recordIndex++)
            {
                ProteinDataBankFileRecord proteinDataBankFileRecord = NextRecord();
                stringBuilder.AppendLine(proteinDataBankFileRecord != null ? proteinDataBankFileRecord.ToString() : string.Empty);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Unloads any loaded PDB file from memory and reset the class instance to its defaults.
        /// </summary>
        public void UnloadFile()
        {
            FileLinesArray = null;
            TargetRecordTypes = null;
            ProteinDataBankFileRecordList = null;
            _currentFileRecordNumber = -1;
        }

    }
}