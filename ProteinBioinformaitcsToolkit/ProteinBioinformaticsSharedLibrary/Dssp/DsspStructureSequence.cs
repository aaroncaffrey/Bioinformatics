using System;
using System.IO;
using System.Linq;

namespace ProteinBioinformaticsSharedLibrary.Dssp
{
    public static class DsspStructureSequence
    {
        public static int CalculateProteinInterfaceLength(int minResidueSequenceIndex, int maxResidueSequenceIndex)
        {
            return (maxResidueSequenceIndex - minResidueSequenceIndex) + 1;
        }

        public static int? NullableTryParseInt32(string str)
        {
            int intValue;
            return int.TryParse(str, out intValue) ? (int?)intValue : null;
        }

        public static string LoadDsspStructureSequence(string pdbFilename, string chainId = null, int startResidueSequenceIndex = -1, int endResidueSequenceIndex = -1, bool reversedSequence = false)
        {
            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                return "";
            }

            var pdbId = ProteinBioClass.PdbIdFromPdbFilename(pdbFilename);

            var dsspFilename = pdbFilename;

            if (Path.GetExtension(dsspFilename) != ".dssp")
            {
                dsspFilename += ".dssp";
            }

            if (!File.Exists(dsspFilename))
            {
                return "";
            }

            var secondaryStructure = DsspFormatFile.LoadDsspFile(dsspFilename);

            if (chainId != null && secondaryStructure.FirstOrDefault(a=>a.FieldChain.FieldValue.ToUpperInvariant() == chainId.ToUpperInvariant()) == null)
            {
                return "";
            }

            if (startResidueSequenceIndex == -1) startResidueSequenceIndex = secondaryStructure.Where(a => chainId == null || a.FieldChain.FieldValue.ToUpperInvariant() == chainId.ToUpperInvariant()).Min(a => int.Parse(a.FieldPdbResidueSequenceIndex.FieldValue));
            if (endResidueSequenceIndex == -1) endResidueSequenceIndex = secondaryStructure.Where(a => chainId == null || a.FieldChain.FieldValue.ToUpperInvariant() == chainId.ToUpperInvariant()).Max(a => int.Parse(a.FieldPdbResidueSequenceIndex.FieldValue));
            

            // dssp specification says order may not be correct
            secondaryStructure = secondaryStructure.Where(a => !string.IsNullOrWhiteSpace(a.FieldChain.FieldValue) && !string.IsNullOrWhiteSpace(a.FieldPdbResidueSequenceIndex.FieldValue)).OrderBy(a => a.FieldChain.FieldValue).ThenBy(a => NullableTryParseInt32(a.FieldPdbResidueSequenceIndex.FieldValue)).ToList();

            var proteinInterfaceLen = CalculateProteinInterfaceLength(startResidueSequenceIndex, endResidueSequenceIndex);

            char[] result = new char[proteinInterfaceLen];
            for (int index = 0; index < result.Length; index++)
            {
                result[index] = '_';
            }

            foreach (var record in secondaryStructure.Where(a => chainId == null || a.FieldChain.FieldValue.ToUpperInvariant() == chainId.ToUpperInvariant()))
            {
                var resSeq = NullableTryParseInt32(record.FieldPdbResidueSequenceIndex.FieldValue);

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
