using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class SequenceIdSplit
    {
        /// <summary>
        ///     This method returns the PDB ID and Chain ID from the Metadata lines of FASTA files. FASTA is the preferred format.
        ///     <para>For example: A FASTA file containing "proteinId:chainId|useless|info" would return { "proteinId", "chainId" }</para>
        /// </summary>
        /// <param name="sequenceId"></param>
        /// <returns></returns>
        public static SequenceIdToPdbIdAndChainIdResult SequenceIdToPdbIdAndChainId(string sequenceId)
        {
            if (string.IsNullOrWhiteSpace(sequenceId))
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceId));
            }

            const string molMarker = "mol:";
            const string lenMarker = "length:";

            if (sequenceId.Contains(" " + molMarker) && sequenceId.Contains(" " + lenMarker))
            {
                var idStrings = sequenceId.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                var pdbId = idStrings[0][0] == '>' ? idStrings[0].Substring(1) : idStrings[0];
                var chainId = "";
                var mol = "";
                var len = "";

                foreach (var token in idStrings)
                {
                    if (!string.IsNullOrWhiteSpace(mol) && !string.IsNullOrWhiteSpace(len))
                    {
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(mol) && token.Length >= molMarker.Length && token.Substring(0, molMarker.Length) == molMarker)
                    {
                        mol = token.Replace(molMarker, "");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(len) && token.Length >= lenMarker.Length && token.Substring(0, lenMarker.Length) == lenMarker)
                    {
                        len = token.Replace(lenMarker, "");
                        continue;
                    }

                }

                if (pdbId != null && pdbId.Contains("_")) //(mol == "protein")
                {
                    chainId = pdbId.Substring(pdbId.IndexOf('_') + 1);
                    pdbId = pdbId.Substring(0, pdbId.IndexOf('_'));
                }

                var description = sequenceId.Substring(pdbId.Length + 1 + chainId.Length + 1 + mol.Length + 1 + len.Length + 1);

                if (description.Length > 0 && description.IndexOf(' ') > -1)
                {
                    description = description.Substring(description.IndexOf(' ') + 1);
                }

                var result = new SequenceIdToPdbIdAndChainIdResult(pdbId.ToUpperInvariant(), chainId.ToUpperInvariant(), mol, len, description);
                return result;

            }
            else
            {

                var pdbId = sequenceId.Substring(0, 4).ToUpperInvariant();
                var chainId = sequenceId.Substring(5, 1).ToUpperInvariant();
                var description = sequenceId.Substring(7);
                //string key;
                
                //var indexOfColon1 = sequenceId.IndexOf(':');
                //var indexOfSpace1 = sequenceId.IndexOf(' ');
                //var indexOfPipe1 = sequenceId.IndexOf('|');

                //int finish = indexOfSpace1 > -1 && indexOfSpace1 <= indexOfColon1 ? indexOfSpace1 : indexOfColon1;

                //string description = "";
                //if (finish > -1)
                //{
                //    key = sequenceId.Substring(0, finish);
                //    description = sequenceId.Substring(finish + 1);
                //}
                //else
                //{
                //    key = sequenceId;
                //}

                //var indexOfColon2 = key.IndexOf(':');
                //var indexOfSpace2 = key.IndexOf(' ');
                //var indexOfPipe2 = key.IndexOf('|');

                //if (indexOfColon2 != -1 || indexOfSpace2 != -1 || indexOfPipe2 != -1)
                //{
                //    finish = new List<int> {indexOfColon2, indexOfSpace2, indexOfPipe2}.Where(a => a != -1).Min();


                //    if (finish < 0)
                //    {
                //        finish = key.Length;
                //    }
                //}


                //if (!String.IsNullOrEmpty(pdbId))
                //{
                    //string pdbId = key.Substring(0, finish).Trim().ToUpperInvariant();

                    //string chainId = key.Substring(finish + 1).Trim().ToUpperInvariant();

                    var result = new SequenceIdToPdbIdAndChainIdResult(pdbId, chainId, null, null, description);

                    return result;
                //}
                //else
                //{
                //    var result = new SequenceIdToPdbIdAndChainIdResult(pdbId, pdbId, null, null, description);

                //    return result;
                //}
            }
            
        }

        public class SequenceIdToPdbIdAndChainIdResult
        {
            public readonly string PdbId;
            public readonly string ChainId;
            public readonly string Mol;
            public readonly string Len;
            public readonly string Description;

            public SequenceIdToPdbIdAndChainIdResult(string pdbId, string chainId, string mol, string len, string description)
            {
                ChainId = chainId;
                PdbId = pdbId;
                Mol = mol;
                Len = len;
                Description = description;
            }
        }
    }
}