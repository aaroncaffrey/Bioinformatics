using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary.AminoAcids;
using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;

namespace ProteinBioinformaticsSharedLibrary
{
    public class Sequence
    {
        public class SequenceId
        {
            public string PdbId;
            public char ChainId;
            public string Mol;
            public string Len;
            public string Description;

            public SequenceId(string sequenceId)
            {
                if (String.IsNullOrWhiteSpace(sequenceId))
                {
                    return;
                }

                if (sequenceId != null && sequenceId.FirstOrDefault() == '>') sequenceId = sequenceId.Substring(1);

                const string molMarker = "mol:";
                const string lenMarker = "length:";

                if (sequenceId.Contains(" " + molMarker) && sequenceId.Contains(" " + lenMarker))
                {
                    var idStrings = sequenceId.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    var pdbId = idStrings[0][0] == '>' ? idStrings[0].Substring(1) : idStrings[0];
                    var chainId = "";
                    var mol = "";
                    var len = "";

                    foreach (var token in idStrings)
                    {
                        if (!String.IsNullOrWhiteSpace(mol) && !String.IsNullOrWhiteSpace(len))
                        {
                            break;
                        }

                        if (String.IsNullOrWhiteSpace(mol) && token.Length >= molMarker.Length && token.Substring(0, molMarker.Length) == molMarker)
                        {
                            mol = token.Replace(molMarker, "");
                            continue;
                        }

                        if (String.IsNullOrWhiteSpace(len) && token.Length >= lenMarker.Length && token.Substring(0, lenMarker.Length) == lenMarker)
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

                    Init(pdbId.ToUpperInvariant(), chainId/*.ToUpperInvariant()*/[0], mol, len, description);
                }
                else
                {

                    var split = sequenceId.Split(new char[] { '_', '-', ':', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (split.Length < 2) return;// Init("", ' ', "", "", "");


                    var pdbId = split[0].ToUpperInvariant();
                    var chainId = split[1]/*.ToUpperInvariant()*/;

                    var description = split.Length > 2 ? string.Join(" ", split.Skip(2).ToList()) : "";

                    Init(pdbId, chainId[0], null, null, description);
                }

            }

            public void Init(string pdbId, char chainId, string mol, string len, string description)
            {
                ChainId = chainId;
                PdbId = pdbId;
                Mol = mol;
                Len = len;
                Description = description;
            }

            protected bool Equals(SequenceId other)
            {
                return String.Equals(PdbId, other.PdbId, StringComparison.InvariantCultureIgnoreCase) && String.Equals("" + ChainId, "" + other.ChainId, StringComparison.InvariantCultureIgnoreCase);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((SequenceId)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((PdbId != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(PdbId) : 0) * 397) ^ (StringComparer.InvariantCultureIgnoreCase.GetHashCode(ChainId));
                }
            }

            public static bool operator ==(SequenceId left, SequenceId right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(SequenceId left, SequenceId right)
            {
                return !Equals(left, right);
            }
        }
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { _Id = value;
                IdSplit = new SequenceId(this._Id);
            }
        }
        public SequenceId IdSplit;
        public string FullSequence;

        public static string EscapeAminoAcidSequence(string sequence, char escape = 'X', bool allowX = false)
        {
            //return String.IsNullOrEmpty(sequence) ? sequence : sequence.All(Char.IsLetter) ? sequence : String.Join("", sequence.Select(b => Char.IsLetter(b) ? b : escape).ToList());

            if (String.IsNullOrEmpty(sequence)) return "";

            var escaped = new string(sequence.Select(a => Char.IsLetter(a) && (allowX || a != 'X') ? a : escape).ToArray());

            return escaped;
        }

        public static string CleanAminoAcidSequence(string sequence, bool allowX = false)
        {
            //return String.IsNullOrEmpty(sequence) ? sequence : sequence.All(Char.IsLetter) ? sequence : String.Join("", sequence.Select(b => Char.IsLetter(b) ? b : escape).ToList());

            if (String.IsNullOrEmpty(sequence)) return "";

            var escaped = new string(sequence.Where(a => Char.IsLetter(a) && (a != 'X' || allowX)).ToArray());

            return escaped;
        }

        public static string TrimSequence(string sequence)
        {
            return sequence.Trim(new char[] { '-', '_', 'X', ' ', '\r', '\n', '\t', '\0' });
        }

        public string GetTrimmedSequence()
        {
            return TrimSequence(FullSequence);    
        }

        public string GetEscapedSequence(char escape = 'X', bool allowX = false) { return EscapeAminoAcidSequence(FullSequence, escape, allowX); }

        public string GetCleanedSequence(bool allowX = false) { return CleanAminoAcidSequence(FullSequence, allowX); }

        public static string WrapLines(string data, int lineLength = 80)
        {
            var result = new List<string>();
            var offset = 0;
            while (data.Length - offset > lineLength)
            {
                result.Add(data.Substring(0 + offset, lineLength));
                offset += lineLength;
            }
            if (data.Length - offset > 0) result.Add(data.Substring(offset));
            return String.Join(Environment.NewLine,result);
        }
        public string GetAsFasta()
        {
            var result = (!String.IsNullOrEmpty(Id) && Id[0]=='>') ? Id : ('>' + Id + Environment.NewLine) + WrapLines(FullSequence,80) + Environment.NewLine;
            return result;
        }

        public Tuple<string, string> GetAsTuple() { return new Tuple<string, string>(Id, FullSequence); }

        public static List<Tuple<string, string>> GetAsTuple(List<Sequence> sequenceList)
        {
            var result = new List<Tuple<string, string>>();

            foreach (var s in sequenceList)
            {
                result.Add(s.GetAsTuple());
            }

            return result;
        }

        public static string GetAsFasta(List<Sequence> sequenceList)
        {
            var result = "";
            foreach (var s in sequenceList) { result += s.GetAsFasta(); }
            return result;
        }
        
        public static string GetAsPir(List<Sequence> sequenceList)
        {
            var outputStr = new List<string>();

            foreach (var seq in sequenceList)
            {
                outputStr.Add(">P1;query");
                outputStr.Add("sequence:query::::::::");

                var chainName = seq.IdSplit.ChainId;
                var chainAminoAcids = seq.FullSequence;

                var offset = 0;
                while (chainAminoAcids.Length - offset > 80)
                {
                    outputStr.Add(chainAminoAcids.Substring(offset, 80));
                    //chainAaStr = chainAaStr.Remove(0, 80);
                    offset += 80;
                }
                if (chainAminoAcids.Length - offset > 0) outputStr.Add(chainAminoAcids.Substring(offset));
                outputStr[outputStr.Count - 1] = outputStr[outputStr.Count - 1] + "*";

            }

            var result = String.Join(Environment.NewLine, outputStr) + Environment.NewLine;

            return result;
        }

        public enum SequenceFormat
        {
            Fasta,
            Pir
        }

        public static string GetFormattedSequence(Sequence sequence, SequenceFormat sequenceFormat, string mergeMaster = null) { return GetFormattedSequence(new List<Sequence>() {sequence}, sequenceFormat, mergeMaster); }

        public static string GetFormattedSequence(List<Sequence> sequenceList, SequenceFormat sequenceFormat, string mergeMaster = null)
        {
            var result = new List<string>();

            if (sequenceFormat == SequenceFormat.Pir)
            {
                if (!String.IsNullOrWhiteSpace(mergeMaster))
                {
                    var id = sequenceList.FirstOrDefault(a => a.Id == mergeMaster);
                    //result.Add(id.Item1.Replace(":", "_").Insert(1, "P1;"));
                    //result.Add("sequence:" + id.Item1.Replace(">", "").Replace(":", "_") + "::::::::");
                    result.Add(">P1;query");
                    result.Add("sequence:query::::::::");
                }
            }

            for (int index = 0; index < sequenceList.Count; index++)
            {
                var id = sequenceList[index];
                if (sequenceFormat == SequenceFormat.Fasta)
                {
                    result.Add(id.Id);
                }
                else if (sequenceFormat == SequenceFormat.Pir)
                {
                    if (String.IsNullOrWhiteSpace(mergeMaster))
                    {
                        //result.Add(id.Item1.Replace(":", "_").Insert(1, "P1;"));
                        //result.Add("sequence:" + id.Item1.Replace(">", "").Replace(":", "_") + "::::::::");
                        result.Add(">P1;query");
                        result.Add("sequence:query::::::::");
                    }
                }


                var s = id.FullSequence;
                while (s.Length > 80)
                {
                    result.Add(s.Substring(0, 80));
                    s = s.Remove(0, 80);
                }
                if (s.Length > 0) result.Add(s);

                if (sequenceFormat == SequenceFormat.Pir)
                {
                    if (String.IsNullOrWhiteSpace(mergeMaster) || index == sequenceList.Count - 1)
                    {
                        result[result.Count - 1] = result[result.Count - 1] + "*";
                    }
                    else
                    {
                        result[result.Count - 1] = result[result.Count - 1] + "/";
                    }
                }
            }

            return string.Join(Environment.NewLine, result) + Environment.NewLine;
        }

        public char this[int index]
        {
            get { return FullSequence[index]; }
            set { FullSequence = FullSequence.Substring(0, index) + value + FullSequence.Substring(index + 1); }
        }
        public int Count()
        {
            return FullSequence.Length;
        }

        public bool SequenceEqual(Sequence sequence)
        {
            return this.FullSequence == sequence.FullSequence;
        }

        public static bool SequenceEqual(Sequence sequenceA, Sequence sequenceB)
        {
            return sequenceA == sequenceB;
        }

        public Sequence(string id, string fullSequence)
        {
            Id = id;
            FullSequence = fullSequence;
        }

        public Sequence(string fullSequence)
        {
            var seq = Sequence.LoadSequenceFile(new string[ ] {  fullSequence });
            if (seq.Count > 0)
            {
                this.FullSequence = seq[0].FullSequence;
                this.Id = seq[0].FullSequence;
            }
        }

        public static List<Sequence> LoadStructureFile(string atomsFilename, char[] chainIdWhiteList = null, bool padMissingBool = true, int[] startResSeq = null, int[] endResSeq = null, char outsidePaddingChar = ' ', char insidePaddingChar = 'X')
        {
            var pdb = ProteinBioClass.PdbAtomicChains(atomsFilename, chainIdWhiteList, -1, -1, true);
            var pdbId = ProteinBioClass.PdbIdFromPdbFilename(atomsFilename);
            return LoadStructureFile(pdb, pdbId, chainIdWhiteList, padMissingBool, startResSeq, endResSeq,
                outsidePaddingChar, insidePaddingChar);
        }

        public static List<Sequence> LoadStructureFile(string[] structureFileLines, string pdbId = "", char[] chainIdWhiteList = null, bool padMissingBool = true, int[] startResSeq = null, int[] endResSeq = null, char outsidePaddingChar = ' ', char insidePaddingChar = 'X')
        {
            var pdb = ProteinBioClass.PdbAtomicChains(structureFileLines, chainIdWhiteList, -1, -1, false);

            foreach (var c in pdb.ChainList) { c.AtomList = c.AtomList.GroupBy(a => a.resSeq.FieldValue).OrderBy(g=>int.Parse(g.Key)).Select(g => g.First()).ToList(); }

            return LoadStructureFile(pdb, pdbId, chainIdWhiteList, padMissingBool, startResSeq, endResSeq, outsidePaddingChar, insidePaddingChar);
        }

        public static List<Sequence> LoadStructureFile(ProteinBioClass.StructureChainListContainer pdb, string pdbId = "", char[] chainIdWhiteList = null, bool padMissingBool = true, int[] startResSeq = null, int[] endResSeq = null, char outsidePaddingChar = ' ', char insidePaddingChar = 'X')
        {
            var result = new List<Sequence>();
            
            List<ProteinBioClass.AtomListContainer> x = pdb.ChainList;
            var zeroBased = pdb.ChainList.Any(a => a.AtomList.Any(b => Int32.Parse(b.resSeq.FieldValue) == 0));

            if ((chainIdWhiteList != null && chainIdWhiteList.Length > 0) && (startResSeq != null && startResSeq.Length > 0) && (endResSeq != null && endResSeq.Length > 0))
            {
                for (int chainIndex = 0; chainIndex < pdb.ChainList.Count; chainIndex++)
                {
                    var chainId = pdb.ChainList[chainIndex].ChainId;

                    var chainStartResSeq = startResSeq[chainIdWhiteList.ToList().FindIndex(a => a == chainId)];
                    var chainEndResSeq =     endResSeq[chainIdWhiteList.ToList().FindIndex(a => a == chainId)];

                    pdb.ChainList[chainIndex].AtomList = pdb.ChainList[chainIndex].AtomList.Where(a => Int32.Parse(a.resSeq.FieldValue) >= chainStartResSeq && Int32.Parse(a.resSeq.FieldValue) <= chainEndResSeq).ToList();
                }
            }

            for (int index = 0; index < pdb.ChainList.Count; index++)
            {
                var suplAtoms = new List<ATOM_Record>();
                foreach (var atom in pdb.ChainList[index].AtomList)
                {
                    if (String.IsNullOrWhiteSpace(atom.iCode.FieldValue)) continue;

                    var resSeq = atom.resSeq.FieldValue;
                    var aa = atom.resName.FieldValue;
                    var atomName = atom.name.FieldValue;

                    var matches = pdb.ChainList[index].AtomList.Where(a => a.resSeq.FieldValue == atom.resSeq.FieldValue && a.resName.FieldValue == atom.resName.FieldValue && a.name.FieldValue == atom.name.FieldValue).ToList();

                    if (matches.Count <= 1) continue;

                    var noIcode = matches.FindIndex(a => string.IsNullOrWhiteSpace(a.iCode.FieldValue));

                    if (noIcode > -1)
                    {
                        suplAtoms.AddRange(matches.Where((a, i) => i != noIcode).ToList());
                    }
                    else
                    {
                        suplAtoms.AddRange(matches.Skip(1).ToList());
                    }

                }
                //pdb.ChainList[index].AtomList = pdb.ChainList[index].AtomList.Where(a => String.IsNullOrWhiteSpace(a.iCode.FieldValue)).ToList();
                pdb.ChainList[index].AtomList.RemoveAll(a => suplAtoms.Contains(a));

                var chain = pdb.ChainList[index];
                var first = chain.AtomList.Min(a => Int32.Parse(a.resSeq.FieldValue));
                var last = chain.AtomList.Max(a => Int32.Parse(a.resSeq.FieldValue));

                var resSeqOffset = first < 0 ? Math.Abs(first) + 1 : 0;

                var chainAa = new char[last + (zeroBased ? 1 : 0) + resSeqOffset];
                var chainAaMask = new bool[last + (zeroBased ? 1 : 0) + resSeqOffset];

                for (var i = 0; i < chainAa.Length; i++)
                {
                    chainAa[i] = outsidePaddingChar;
                    chainAaMask[i] = false;
                }

                foreach (var atom in chain.AtomList)
                {
                    var aa = atom.resName.FieldValue;
                    if (String.IsNullOrWhiteSpace(aa)) aa = "" + insidePaddingChar;

                    var res = Int32.Parse(atom.resSeq.FieldValue);

                    if (!zeroBased) res = res - 1;

                    res += resSeqOffset;

                    var aa1l = AminoAcidConversions.AminoAcidNameToCode1L(aa);
                    chainAa[res] = aa1l[0];
                    chainAaMask[res] = true;
                }

                for (var i = first; i <= last; i++)
                {
                    var res = i;

                    res += resSeqOffset;

                    if (!zeroBased) res = res - 1;

                    if (!chainAaMask[res]) chainAa[res] = insidePaddingChar;
                }

                var chainAaStr = String.Join("", chainAa);
                if (!padMissingBool) chainAaStr = chainAaStr.Trim(outsidePaddingChar);

                result.Add(new Sequence(">" + pdbId + ":" + chain.ChainId, chainAaStr));
            }

            return result;
        }

        public static List<Sequence> LoadSequenceFile(string sequenceFilename, string[] molNames = null)
        {
            if (String.IsNullOrWhiteSpace(sequenceFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceFilename));
            }

            if (!File.Exists(sequenceFilename))
            {
                throw new FileNotFoundException(sequenceFilename);
            }

            var lines = File.ReadAllLines(sequenceFilename);

            return LoadSequenceFile(lines, molNames);
        }

        public static List<Sequence> LoadSequenceFile(string[] sequenceFileData, string[] molNames = null)
        {
            var sequenceList = new List<Sequence>();

            var id = "";
            var seq = "";

            foreach (var line in sequenceFileData)
            {
                if (String.IsNullOrEmpty(line)) continue;

                if (line[0] == '>')
                {
                    if (!String.IsNullOrEmpty(id) || !String.IsNullOrEmpty(seq))
                    {
                        sequenceList.Add(new Sequence(id, seq));
                    }
                    id = line;
                    seq = "";
                    continue;
                }

                seq += line;
            }

            if (!String.IsNullOrEmpty(id) || !String.IsNullOrEmpty(seq))
            {
                sequenceList.Add(new Sequence(id, seq));
            }

            if (sequenceList != null && sequenceList.Count > 0 && molNames != null && molNames.Length > 0)
            {
                sequenceList = sequenceList.Where(a => molNames.Contains(a.IdSplit.Mol)).ToList();
            }

            return sequenceList;
        }

        public static void Save(string outputFastaFile, List<Sequence> sequenceList)
        {
            if (String.IsNullOrWhiteSpace(outputFastaFile)) return;
            
            Directory.CreateDirectory(Path.GetDirectoryName(outputFastaFile));
            //File.WriteAllLines(outputFastaFile, sequenceList.Select(a=>a.));
        }
    }
}
