using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.Dssp
{
    public static class ProteinInterfaceSecondaryStructureLoader
    {
        public static string ProteinInterfaceSecondaryStructure(string pdbFilename, string chainId = null, int startResidueSequenceIndex = -1, int endResidueSequenceIndex = -1, bool reversedSequence = false)
        {
            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                return "";
            }

            var pdbId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            var dsspFilename = pdbFilename + ".dssp";

            if (!File.Exists(dsspFilename))
            {
                return "";
            }

            var secondaryStructure = DsspFormatFile.LoadDsspFile(dsspFilename);

            if (chainId != null && secondaryStructure.FirstOrDefault(a=>a.FieldChain.FieldValue==chainId) == null)
            {
                return "";
            }

            if (startResidueSequenceIndex == -1) startResidueSequenceIndex = secondaryStructure.Where(a => chainId == null || a.FieldChain.FieldValue == chainId).Min(a => int.Parse(a.FieldPdbResidueSequenceIndex.FieldValue));
            if (endResidueSequenceIndex == -1) endResidueSequenceIndex = secondaryStructure.Where(a => chainId == null || a.FieldChain.FieldValue == chainId).Max(a => int.Parse(a.FieldPdbResidueSequenceIndex.FieldValue));
            

            // dssp specification says order may not be correct
            secondaryStructure = secondaryStructure.Where(a => !string.IsNullOrWhiteSpace(a.FieldChain.FieldValue) && !string.IsNullOrWhiteSpace(a.FieldPdbResidueSequenceIndex.FieldValue)).OrderBy(a => a.FieldChain.FieldValue).ThenBy(a => ProteinDataBankFileOperations.NullableTryParseInt32(a.FieldPdbResidueSequenceIndex.FieldValue)).ToList();

            var proteinInterfaceLen = ProteinInterfaceDetection.CalculateProteinInterfaceLength(startResidueSequenceIndex, endResidueSequenceIndex);

            char[] result = new char[proteinInterfaceLen];
            for (int index = 0; index < result.Length; index++)
            {
                result[index] = '_';
            }

            foreach (var record in secondaryStructure.Where(a => chainId == null || a.FieldChain.FieldValue == chainId))
            {
                var resSeq = ProteinDataBankFileOperations.NullableTryParseInt32(record.FieldPdbResidueSequenceIndex.FieldValue);

                if (resSeq == null || resSeq < startResidueSequenceIndex || resSeq > endResidueSequenceIndex) continue;

                var position = resSeq - startResidueSequenceIndex;

                if (record.FieldSecondaryStructure.FieldValue.Length == 0) continue;

                result[position.Value] = record.FieldSecondaryStructure.FieldValue[0];
            }

            if (reversedSequence)
            {
                Array.Reverse(result);
            }

            return new string(result);
        }
    }
}
