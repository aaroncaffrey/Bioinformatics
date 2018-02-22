using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;
using BioinformaticsHelperLibrary.Spreadsheets;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public static class IsolateInteractionStructure
    {
        public class ProteinInterfaceId
        {
            public int ChainId;
            public int FirstPosition;
            public int LastPosition;

            public ProteinInterfaceId()
            {
                
            }

            public ProteinInterfaceId(int chainId, int firstPosition, int lastPosition)
            {
                ChainId = chainId;
                FirstPosition = firstPosition;
                LastPosition = lastPosition;
            }
        }

        public static List<string> RemoveNonProteinInterfaceRecords(string pdbFilename, List<ProteinInterfaceId> proteinInterfaceIdList)
        {
            var proteinDataBankFile = new ProteinDataBankFormat.ProteinDataBankFile(pdbFilename);

            var result = new List<string>();

            var foundEndModel = false;
            var levelModel = 0;

            for (var proteinDataBankFileRecordIndex = 0; proteinDataBankFileRecordIndex < proteinDataBankFile.Count; proteinDataBankFileRecordIndex++)
            {
                var record = proteinDataBankFile.NextRecord();

                if (record == null)
                {
                    continue;
                }

                if (record.GetType() == typeof(MODEL_Record))
                {
                    levelModel++;
                }
                else if (record.GetType() == typeof(ENDMDL_Record))
                {
                    foundEndModel = true;
                    levelModel--;
                }
                else if (record.GetType() == typeof (ATOM_Record))
                {
                    var atom = (ATOM_Record) record;

                    var atomResSeq = ProteinDataBankFileOperations.NullableTryParseInt32(atom.resSeq.FieldValue);

                    if (atomResSeq != null)
                    {
                        var atomChain = atom.chainID.FieldValue;

                        if (!foundEndModel && proteinInterfaceIdList.Any(s => SpreadsheetFileHandler.AlphabetLetterRollOver(s.ChainId) == atomChain && atomResSeq >= s.FirstPosition && atomResSeq <= s.LastPosition))
                        {
                            result.Add(record.ColumnFormatLine);
                        }
                    }
                }
                else if (record.GetType() == typeof(HETATM_Record))
                {
                    var hetatm = (HETATM_Record)record;

                    var atomResSeq = ProteinDataBankFileOperations.NullableTryParseInt32(hetatm.resSeq.FieldValue);

                    if (atomResSeq == null) continue;
                    var atomChain = hetatm.chainID.FieldValue;

                    if (!foundEndModel && proteinInterfaceIdList.Any(s => SpreadsheetFileHandler.AlphabetLetterRollOver(s.ChainId) == atomChain && atomResSeq >= s.FirstPosition && atomResSeq <= s.LastPosition))
                    {
                        result.Add(record.ColumnFormatLine);
                    }
                }
                else if (record.GetType() == typeof(LINK_Record))
                {
                    var link = (LINK_Record)record;

                    var atomResSeq1 = ProteinDataBankFileOperations.NullableTryParseInt32(link.resSeq1.FieldValue);
                    var atomResSeq2 = ProteinDataBankFileOperations.NullableTryParseInt32(link.resSeq2.FieldValue);

                    if (atomResSeq1 == null || atomResSeq2 == null) continue;

                    if (!foundEndModel && proteinInterfaceIdList.Any(s => ((atomResSeq1 >= s.FirstPosition && atomResSeq1 <= s.LastPosition) && (atomResSeq2 >= s.FirstPosition && atomResSeq2 <= s.LastPosition))
                    || ((atomResSeq2 >= s.FirstPosition && atomResSeq2 <= s.LastPosition) && (atomResSeq1 >= s.FirstPosition && atomResSeq1 <= s.LastPosition))))
                    {
                        result.Add(record.ColumnFormatLine);
                    }
                }
                else if (record.GetType() == typeof(ANISOU_Record))
                {
                    var anisou = (ANISOU_Record)record;

                    var atomResSeq = ProteinDataBankFileOperations.NullableTryParseInt32(anisou.resSeq.FieldValue);

                    if (atomResSeq == null) continue;

                    var atomChain = anisou.chainID.FieldValue;

                    if (!foundEndModel && proteinInterfaceIdList.Any(s => SpreadsheetFileHandler.AlphabetLetterRollOver(s.ChainId) == atomChain && atomResSeq >= s.FirstPosition && atomResSeq <= s.LastPosition))
                    {
                        result.Add(record.ColumnFormatLine);
                    }
                }
                else if (record.GetType() == typeof (TER_Record))
                {
                    var ter = (TER_Record) record;

                    var atomChain = ter.chainID.FieldValue;

                    if (!foundEndModel && proteinInterfaceIdList.Any(s => SpreadsheetFileHandler.AlphabetLetterRollOver(s.ChainId) == atomChain))
                    {
                        result.Add(record.ColumnFormatLine);
                    }
                }
                else
                {
                    if (!foundEndModel || levelModel == 0)
                    {
                        result.Add(record.ColumnFormatLine);
                    }
                }
            }

            return result;
        }
    }
}
