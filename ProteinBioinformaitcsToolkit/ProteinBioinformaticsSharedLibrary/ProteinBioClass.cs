using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary.AminoAcids;
using ProteinBioinformaticsSharedLibrary.AminoAcids.Additional;
using ProteinBioinformaticsSharedLibrary.AminoAcids.Ambiguous;
using ProteinBioinformaticsSharedLibrary.AminoAcids.NonStandard;
using ProteinBioinformaticsSharedLibrary.AminoAcids.Standard;
using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;
using ProteinBioinformaticsSharedLibrary.TaskManagement;

namespace ProteinBioinformaticsSharedLibrary
{
    public class ProteinBioClass
    {
        public static class StructureToSequenceAlignment
        {
            public class StructureToSequenceAlignmentResult
            {
                public string FastaSequence;
                public string PdbSequence;
                public string SuperSequence;
                public string FastaSequenceAligned;
                public string PdbSequenceAligned;
                public int[] AlignmentMap;
                public List<int> StructureMissingResidues;
                public List<int> StructureMissingResiduesAligned;
                public List<int> ChainResSeqList;
                public int ChainResSeqMin;
                public int ChainResSeqMax;
                public int ChainResSeqLength;
            }

            public static StructureToSequenceAlignmentResult Align(List<ATOM_Record> atomList, string fastaSequence, string pdbSequence)
            {
                if (atomList == null || atomList.Count == 0 || string.IsNullOrWhiteSpace(fastaSequence) || string.IsNullOrWhiteSpace(pdbSequence)) return null;
                var result = new StructureToSequenceAlignmentResult();
                result.FastaSequence = fastaSequence;
                result.PdbSequence = pdbSequence;

                //var alignment = new NeedlemanWunsch(ProteinBioClass.CleanAminoAcidSequence(sequenceFromSequenceFile), ProteinBioClass.CleanAminoAcidSequence(sequenceFromStructureFile));
                var alignment = new NeedlemanWunsch(Sequence.EscapeAminoAcidSequence(result.FastaSequence, '-', true), Sequence.EscapeAminoAcidSequence(result.PdbSequence, '-', true));
                var aligmentStr = alignment.getAlignment();

                result.FastaSequenceAligned = aligmentStr[0];
                result.PdbSequenceAligned = aligmentStr[1];
                result.AlignmentMap = new int[result.FastaSequenceAligned.Length];

                result.ChainResSeqList = atomList.Select(a => int.Parse(a.resSeq.FieldValue)).Distinct().OrderBy(a => a).ToList();
                result.ChainResSeqMin = result.ChainResSeqList.Min(); // startIndex
                result.ChainResSeqMax = result.ChainResSeqList.Max();
                result.ChainResSeqLength = (result.ChainResSeqMax - result.ChainResSeqMin) + 1;

                result.StructureMissingResidues = new List<int>();
                for (var i = result.ChainResSeqMin; i <= result.ChainResSeqMax; i++)
                {
                    if (!result.ChainResSeqList.Contains(i)) { result.StructureMissingResidues.Add(i); }
                }

                var startIndex = result.PdbSequenceAligned.ToList().FindIndex(a => a != '-');

                result.AlignmentMap[startIndex] = result.ChainResSeqMin;
                for (var i = startIndex - 1; i >= 0; i--) { result.AlignmentMap[i] = result.AlignmentMap[i + 1] - 1; }

                var resSeqListIndex = 0;
                var thisResSeq = result.ChainResSeqList.Count - 1 >= resSeqListIndex ? result.ChainResSeqList[resSeqListIndex] : 1; // : thisResSeq + 1;
                var nextResSeq = result.ChainResSeqList.Count - 1 > resSeqListIndex ? result.ChainResSeqList[resSeqListIndex + 1] : thisResSeq + 1;

                var notReallyMissing = new List<int>();

                for (var i = startIndex; i < result.PdbSequenceAligned.Length; i++)
                {
                    if (result.PdbSequenceAligned[i] == '-')
                    {
                        if (thisResSeq < nextResSeq - 1) { thisResSeq++; }
                        if (nextResSeq <= thisResSeq) { nextResSeq = thisResSeq + 1; }

                        if (result.FastaSequenceAligned[i] == '-')
                        {
                            result.AlignmentMap[i] = int.MinValue;
                            notReallyMissing.Add(thisResSeq); // + 1);
                        }
                        else
                        { result.AlignmentMap[i] = thisResSeq; }

                    }
                    else //if (alignmentPdbSeq[i] != '-')
                    {
                        thisResSeq = result.ChainResSeqList.Count - 1 >= resSeqListIndex ? result.ChainResSeqList[resSeqListIndex] : thisResSeq + 1;
                        nextResSeq = result.ChainResSeqList.Count - 1 > resSeqListIndex ? result.ChainResSeqList[resSeqListIndex + 1] : thisResSeq + 1;
                        if (nextResSeq <= thisResSeq) { nextResSeq = thisResSeq + 1; }

                        result.AlignmentMap[i] = thisResSeq;
                        resSeqListIndex++;
                    }
                }

                result.StructureMissingResiduesAligned = new List<int>();
                for (var i = result.ChainResSeqMin; i <= result.ChainResSeqMax; i++)
                {
                    if (!result.AlignmentMap.Contains(i) && !notReallyMissing.Contains(i)) { result.StructureMissingResiduesAligned.Add(i); }
                }

                result.SuperSequence = new string(result.FastaSequenceAligned.Where((a, i) => result.AlignmentMap[i] >= result.ChainResSeqMin && result.AlignmentMap[i] <= result.ChainResSeqMax).ToArray());

                return result;
            }
        }

        public static int InterfaceOverlap(int x1, int x2, int y1, int y2)
        {
            var xMin = Math.Min(x1, x2);
            var xMax = Math.Max(x1, x2);

            var yMin = Math.Min(y1, y2);
            var yMax = Math.Max(y1, y2);

            return (Math.Min(xMax, yMax) - Math.Max(xMin, yMin)) + 1;
        }

        public static decimal InterfaceOverlapPercentage(int x1, int x2, int y1, int y2)
        {
            var x_min = Math.Min(x1, x2);
            var x_max = Math.Max(x1, x2);

            var y_min = Math.Min(y1, y2);
            var y_max = Math.Max(y1, y2);

            var xy_min = Math.Min(x_min, y_min);
            var xy_max = Math.Max(x_max, y_max);

            var xy_len = Math.Abs(xy_max - xy_min) + 1;

            return ((decimal) InterfaceOverlap(x1, x2, y1, y2)) / ((decimal)xy_len);
        }





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

                    var description = split.Length > 2 ? string.Join(" ",split.Skip(2).ToList()) : "";

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



        public static Tuple<string, string> SimpleAlignmentOffset(string sequenceA, string sequenceB)
        {
            if (String.IsNullOrEmpty(sequenceA) || String.IsNullOrEmpty(sequenceB))
            {
                return new Tuple<string, string>(sequenceA, sequenceB);
            }



            var sA = sequenceA.ToList();
            var sB = sequenceB.ToList();

            var origSaLen = sA.Count;
            var origSbLen = sB.Count;

            // pad longest with length of shortest & pad shortest with length of longest
            sA.InsertRange(0, new List<char>(new string('\0', Math.Abs(sB.Count - sA.Count))));
            sB.InsertRange(0, new List<char>(new string('\0', Math.Abs(sA.Count - sB.Count))));

            var longest = sA.Count > sB.Count ? sA : sB;
            var shortest = longest == sA ? sB : sA;

            shortest.InsertRange(0, new List<char>(new string('\0', origSaLen > origSbLen ? origSaLen : origSbLen)));

            // 0000000000000000AAAAA
            // 00000AAAAAAAAAAA

            var a = String.Join("", sA);
            var b = String.Join("", sB);

            var bestOffset = 0;
            var bestMatch = 0;
            var bestSA = "";
            var bestSB = "";

            while (shortest.Count > 1)
            {
                shortest.RemoveAt(0);

                var match = 0;

                for (var i = 0; i < shortest.Count; i++)
                {
                    if (i > longest.Count - 1) break;

                    if ((shortest[i] != '\0' && shortest[i] == longest[i]) || (shortest[i] == 'X' || longest[i] == 'X')) match++;
                }

                if (bestMatch == 0 || match >= bestMatch)
                {
                    bestMatch = match;
                    bestOffset = shortest.Count;
                    bestSA = String.Join("", sA);
                    bestSB = String.Join("", sB);

                    var trim = 0;
                    for (var j = 0; j < (bestSA.Length > bestSB.Length ? bestSB.Length : bestSA.Length); j++)
                    {
                        if (bestSA[j] == '\0' && bestSB[j] == '\0') trim++;
                        else break;
                    }
                    bestSA = bestSA.Remove(0, trim);
                    bestSB = bestSB.Remove(0, trim);
                }


            }


            return new Tuple<string, string>(bestSA, bestSB);
        }

        /*
        public static Tuple<string, string> SimpleAlignmentOffset(string sequenceA, string sequenceB)
        {
            if (String.IsNullOrEmpty(sequenceA) || String.IsNullOrEmpty(sequenceB))
            {
                return new Tuple<string, string>(sequenceA, sequenceB);
            }

            var sA = sequenceA.ToList();
            var sB = sequenceB.ToList();

            var origSaLen = sA.Count;
            var origSbLen = sB.Count;

            // pad longest with length of shortest & pad shortest with length of longest
            sA.InsertRange(0, new List<char>(new string('-', Math.Abs(sB.Count - sA.Count))));
            sB.InsertRange(0, new List<char>(new string('-', Math.Abs(sA.Count - sB.Count))));

            var longest = sA.Count > sB.Count ? sA : sB;
            var shortest = longest == sA ? sB : sA;

            shortest.InsertRange(0, new List<char>(new string('-', origSaLen > origSbLen ? origSaLen : origSbLen)));

            // 0000000000000000AAAAA
            // 00000AAAAAAAAAAA

            var a = String.Join("", sA);
            var b = String.Join("", sB);

            var bestOffset = 0;
            var bestMatch = 0;
            var bestSA = "";
            var bestSB = "";

            while (shortest.Count > 1)
            {
                shortest.RemoveAt(0);

                var match = 0;

                for (var i = 0; i < shortest.Count; i++)
                {
                    if (i > longest.Count - 1) break;

                    if ((shortest[i] != '-' && shortest[i] == longest[i]) || (shortest[i] == 'X' || longest[i] == 'X')) match++;
                }

                if (bestMatch == 0 || match >= bestMatch)
                {
                    bestMatch = match;
                    bestOffset = shortest.Count;
                    bestSA = String.Join("", sA);
                    bestSB = String.Join("", sB);

                    var trim = 0;
                    for (var j = 0; j < (bestSA.Length > bestSB.Length ? bestSB.Length : bestSA.Length); j++)
                    {
                        if (bestSA[j] == '-' && bestSB[j] == '-') trim++;
                        else break;
                    }
                    bestSA = bestSA.Remove(0, trim);
                    bestSB = bestSB.Remove(0, trim);
                }
            }

            return new Tuple<string, string>(bestSA, bestSB);
        }
*/

        public enum AlignmentType
        {
            NON,
            SIM,
            NMW,
            SWM

        }

        public enum AlignmentIdentityOption
        {
            MinimumSequenceLength,
            MeanAverageSequenceLength,
            MaximumSequenceLength
        }

        public static AlignmentScore AlignedSequenceSimilarityPercentage(string alignableSequenceA,
            string alignableSequenceB,
            AlignmentType alignmentType,
            AlignmentIdentityOption alignmentIdentityOption = AlignmentIdentityOption.MaximumSequenceLength)
        {
            return AlignedSequenceSimilarityPercentage(new Sequence(alignableSequenceA), new Sequence(alignableSequenceB), alignmentType, alignmentIdentityOption);
        }

        public class AlignmentScore
        {
            public decimal Score;
            public decimal ScoreEvo;

            public AlignmentScore(decimal score, decimal scoreEvo)
            {
                Score = score;
                ScoreEvo = scoreEvo;
            }
        }

        public static AlignmentScore AlignedSequenceSimilarityPercentage(Sequence alignableSequenceA, Sequence alignableSequenceB, AlignmentType alignmentType, AlignmentIdentityOption alignmentIdentityOption = AlignmentIdentityOption.MaximumSequenceLength)
        {
            if (alignableSequenceA == null || alignableSequenceA.Count() == 0 || alignableSequenceB == null || alignableSequenceB.Count() == 0)
            {
                return new AlignmentScore(0, 0);
            }

            var firstSequence = alignableSequenceA;
            var secondSequence = alignableSequenceB;


            if (alignmentType == AlignmentType.NMW)
            {
                var nmw =
                    new NeedlemanWunsch(
                        alignableSequenceA.FullSequence, alignableSequenceB.FullSequence);

                var aligned = nmw.getAlignment();
                return SequenceSimilarityPercentage(aligned[0], aligned[1], alignmentIdentityOption);

            }
            /*else if (alignmentType == AlignmentType.SWM)
            {
                var smithWatermanAligner = new SmithWatermanAligner();

                alignedList = smithWatermanAligner.AlignSimple(alignableSequenceA, alignableSequenceB);

                var seqlen = alignableSequenceA.Count > alignableSequenceB.Count
                    ? alignableSequenceA.Count
                    : alignableSequenceB.Count;

                if (alignedList != null && alignedList.Count > 0 &&
                    alignedList[0].PairwiseAlignedSequences.Count > 0)
                {
                    var firstSequences =
                        alignedList[0].PairwiseAlignedSequences.Select(a => a.FirstSequence).ToArray();
                    var secondSequences =
                        alignedList[0].PairwiseAlignedSequences.Select(a => a.SecondSequence).ToArray();

                    return SequenceSimilarityPercentage(seqlen, firstSequences, secondSequences);

                }
            }*/
            else if (alignmentType == AlignmentType.SIM)
            {
                var alignment = ProteinBioClass.SimpleAlignmentOffset(alignableSequenceA.FullSequence, alignableSequenceB.FullSequence);
                return SequenceSimilarityPercentage(alignment.Item1, alignment.Item2, alignmentIdentityOption);
            }
            else if (alignmentType == AlignmentType.NON)
            {
            }
            return SequenceSimilarityPercentage(firstSequence, secondSequence, alignmentIdentityOption);
        }

        public static AlignmentScore SequenceSimilarityPercentage(long seqlen, Sequence[] alignedSequenceA, Sequence[] alignedSequenceB)//, AlignmentIdentityOption alignmentIdentityOption = AlignmentIdentityOption.MaximumSequenceLength)
        {
            var total = alignedSequenceA.Length;

            var scores = new decimal[total];
            var scores_evo = new decimal[total];

            long totalLocalSeqLens = 0;

            for (var i = 0; i < total; i++)
            {
                var localSeqLen = alignedSequenceA[i].Count() > alignedSequenceB[i].Count()
                    ? alignedSequenceA[i].Count()
                    : alignedSequenceB[i].Count();
                totalLocalSeqLens += localSeqLen;


                var x = SequenceSimilarityPercentage(alignedSequenceA[i], alignedSequenceB[i]
                    /*, physicochemicalProperties*/);
                scores[i] = x.Score * localSeqLen;
                scores_evo[i] = x.ScoreEvo * localSeqLen;
            }

            var y = totalLocalSeqLens / seqlen;

            var result = new AlignmentScore(Math.Round(scores.Sum() * y, 2), Math.Round(scores_evo.Sum() * y, 2));

            return result;
        }

        public static AlignmentScore SequenceSimilarityPercentage(Sequence alignedSequenceA, Sequence alignedSequenceB, AlignmentIdentityOption alignmentIdentityOption = AlignmentIdentityOption.MaximumSequenceLength)
        {
            if (alignedSequenceA == null || alignedSequenceA.Count() == 0 || alignedSequenceB == null || alignedSequenceB.Count() == 0)
            {
                return new AlignmentScore(0.00m, 0.00m);
            }

            if (alignedSequenceA.SequenceEqual(alignedSequenceB))
            {
                return new AlignmentScore(1.00m, 1.00m);
            }

            int sequenceEquality = 0;
            int sequenceEqualityEvo = 0;

            var sequenceLengthMin = alignedSequenceA.Count() < alignedSequenceB.Count()
                ? alignedSequenceA.Count()
                : alignedSequenceB.Count();
            var sequenceLengthMax = alignedSequenceA.Count() > alignedSequenceB.Count()
                ? alignedSequenceA.Count()
                : alignedSequenceB.Count();

            long alignmentLengthMax = 0;
            if (alignmentIdentityOption == AlignmentIdentityOption.MinimumSequenceLength) alignmentLengthMax = sequenceLengthMin;
            else if (alignmentIdentityOption == AlignmentIdentityOption.MaximumSequenceLength) alignmentLengthMax = sequenceLengthMax;
            else if (alignmentIdentityOption == AlignmentIdentityOption.MeanAverageSequenceLength) alignmentLengthMax = (sequenceLengthMin + sequenceLengthMax) / 2;


            var groups = AminoAcidGroups.AminoAcidGroups.GetSubgroupAminoAcidsCodesStrings(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb);

            for (var proteinSequenceIndex = 0; proteinSequenceIndex < sequenceLengthMin; proteinSequenceIndex++)
            {
                if (alignedSequenceA[proteinSequenceIndex] == alignedSequenceB[proteinSequenceIndex])
                {
                    sequenceEquality++;
                    sequenceEqualityEvo++;
                    continue;
                }

                var aminoAcidA = Convert.ToChar(alignedSequenceA[proteinSequenceIndex]);
                var aminoAcidB = Convert.ToChar(alignedSequenceB[proteinSequenceIndex]);

                if (groups.Any(t => t.Contains(aminoAcidA) && t.Contains(aminoAcidB)))
                {
                    sequenceEqualityEvo++;
                }
            }

            var result = new AlignmentScore(
                sequenceEquality == 0 ? 0 : Math.Round(((decimal)sequenceEquality / (decimal)alignmentLengthMax), 2),
                sequenceEqualityEvo == 0 ? 0 : Math.Round(((decimal)sequenceEqualityEvo / (decimal)alignmentLengthMax), 2));

            return result;
        }

        public static AlignmentScore SequenceSimilarityPercentage(string alignedSequenceA, string alignedSequenceB, AlignmentIdentityOption alignmentIdentityOption = AlignmentIdentityOption.MaximumSequenceLength)
        {
            if (alignedSequenceA == null || alignedSequenceA.Length == 0 || alignedSequenceB == null || alignedSequenceB.Length == 0)
            {
                return new AlignmentScore(0.00m, 0.00m);
            }

            if (alignedSequenceA == alignedSequenceB)
            {
                return new AlignmentScore(1.00m, 1.00m);
            }

            int sequenceEquality = 0;
            int sequenceEqualityEvo = 0;

            var sequenceLengthMin = alignedSequenceA.Length < alignedSequenceB.Length
                ? alignedSequenceA.Length
                : alignedSequenceB.Length;
            var sequenceLengthMax = alignedSequenceA.Length > alignedSequenceB.Length
                ? alignedSequenceA.Length
                : alignedSequenceB.Length;

            long alignmentLengthMax = 0;
            if (alignmentIdentityOption == AlignmentIdentityOption.MinimumSequenceLength) alignmentLengthMax = sequenceLengthMin;
            else if (alignmentIdentityOption == AlignmentIdentityOption.MaximumSequenceLength) alignmentLengthMax = sequenceLengthMax;
            else if (alignmentIdentityOption == AlignmentIdentityOption.MeanAverageSequenceLength) alignmentLengthMax = (sequenceLengthMin + sequenceLengthMax) / 2;

            var groups = AminoAcidGroups.AminoAcidGroups.GetSubgroupAminoAcidsCodesStrings(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb);

            for (var proteinSequenceIndex = 0; proteinSequenceIndex < sequenceLengthMin; proteinSequenceIndex++)
            {
                if (alignedSequenceA[proteinSequenceIndex] == alignedSequenceB[proteinSequenceIndex])
                {
                    sequenceEquality++;
                    sequenceEqualityEvo++;
                    continue;
                }

                var aminoAcidA = alignedSequenceA[proteinSequenceIndex];
                var aminoAcidB = alignedSequenceB[proteinSequenceIndex];

                if (groups.Any(t => t.Contains(aminoAcidA) && t.Contains(aminoAcidB)))
                {
                    sequenceEqualityEvo++;
                }
            }

            var result = new AlignmentScore(
                sequenceEquality == 0 ? 0 : Math.Round(((decimal)sequenceEquality / (decimal)alignmentLengthMax), 2),
                sequenceEqualityEvo == 0 ? 0 : Math.Round(((decimal)sequenceEqualityEvo / (decimal)alignmentLengthMax), 2));

            return result;
        }


        public static string WrapConsoleText(string text, int indentLeft = 0, int indentRight = 0, bool wrapFirstLine = true)
        {
            var lines = new List<List<char>>();
            var line = new List<char>();
            lines.Add(line);
            if (wrapFirstLine)
            {
                for (var i = 0; i < indentLeft; i++) line.Add(' ');
            }

            var screenWidth = Console.WindowWidth;
            var screenWidthWithoutPadding = screenWidth - (indentLeft + indentRight);

            var lastSpaceIndex = 0;

            var index = 0;
            while (index < text.Length)
            {
                var ch = text[index];

                if (ch == ' ') lastSpaceIndex = index;

                line.Add(ch);

                if (line.Count >= screenWidthWithoutPadding)
                {
                    if (lastSpaceIndex < index)
                    {
                        var remove = (index - lastSpaceIndex);
                        if (remove < screenWidthWithoutPadding)
                        {
                            for (var i = 0; i < remove; i++) line.RemoveAt(line.Count - 1);
                            index = lastSpaceIndex;

                        }
                    }

                    for (var i = 0; i < indentRight; i++) line.Add(' ');

                    line = new List<char>();
                    lines.Add(line);
                    for (var i = 0; i < indentLeft; i++) line.Add(' ');
                }

                index++;
            }

            return String.Join(Environment.NewLine, lines.Select(a => String.Join("", a)).ToList());

        }

        public class FullProteinInterfaceId : IEquatable<FullProteinInterfaceId>
        {
            public string ProteinId = null;
            public int ChainId = -1;
            public int ProteinInterfaceId = -1;
            public int ProteinInterfaceStartIndex = -1;
            public int ProteinInterfaceEndIndex = -1;

            public static string AlphabetLetterRollOver(int alphabetLetterNumberRollOver)
            {
                const int firstLetterInAlphabet = 65;
                const int lettersInAlphabet = 26;

                string columnName = String.Empty;
                int dividend = alphabetLetterNumberRollOver + 1;

                while (dividend > 0)
                {
                    int letterNumber = (dividend - 1) % lettersInAlphabet;
                    var letter = ((char)(firstLetterInAlphabet + letterNumber));
                    columnName = letter + columnName;
                    dividend = ((dividend - letterNumber) / lettersInAlphabet);
                }

                return columnName;
            }

            public FullProteinInterfaceId()
            {

            }

            public FullProteinInterfaceId(string proteinId, int chainId, int proteinInterfaceId, int proteinInterfaceStartIndex, int proteinInterfaceEndIndex)
            {
                ProteinId = proteinId;
                ChainId = chainId;
                ProteinInterfaceId = proteinInterfaceId;
                ProteinInterfaceStartIndex = proteinInterfaceStartIndex;
                ProteinInterfaceEndIndex = proteinInterfaceEndIndex;
            }

            public FullProteinInterfaceId(FullProteinInterfaceId fullProteinInterfaceId)
            {
                ProteinId = fullProteinInterfaceId.ProteinId;
                ChainId = fullProteinInterfaceId.ChainId;
                ProteinInterfaceId = fullProteinInterfaceId.ProteinInterfaceId;
                ProteinInterfaceStartIndex = fullProteinInterfaceId.ProteinInterfaceStartIndex;
                ProteinInterfaceEndIndex = fullProteinInterfaceId.ProteinInterfaceEndIndex;
            }

            public bool Equals(FullProteinInterfaceId other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return ChainId == other.ChainId && String.Equals(ProteinId, other.ProteinId, StringComparison.InvariantCultureIgnoreCase) && ProteinInterfaceId == other.ProteinInterfaceId;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((FullProteinInterfaceId)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = ChainId;
                    hashCode = (hashCode * 397) ^ (ProteinId != null ? ProteinId.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ ProteinInterfaceId;
                    return hashCode;
                }
            }

            public static bool operator ==(FullProteinInterfaceId left, FullProteinInterfaceId right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(FullProteinInterfaceId left, FullProteinInterfaceId right)
            {
                return !Equals(left, right);
            }

            public override string ToString()
            {
                return this.ProteinId + "_" + AlphabetLetterRollOver(this.ChainId) + "_" + AlphabetLetterRollOver(this.ProteinInterfaceId);

            }
        }

        public class AtomPair
        {
            /// <summary>
            ///     Atom1 property.
            /// </summary>
            public ATOM_Record Atom1 = null;

            /// <summary>
            ///     Atom2 property.
            /// </summary>
            public ATOM_Record Atom2 = null;


            public FullProteinInterfaceId Atom1FullProteinInterfaceId = new FullProteinInterfaceId();

            public FullProteinInterfaceId Atom2FullProteinInterfaceId = new FullProteinInterfaceId();


            /// <summary>
            ///     The calculated distance between Atom1 and Atom2.
            /// </summary>
            public decimal Distance;


            public override string ToString()
            {
                return
                    @"; distance-1d " + Math.Abs(Int32.Parse(Atom1.resSeq.FieldValue) - Int32.Parse(Atom2.resSeq.FieldValue)) + Environment.NewLine +
                    @"; distance-3d " + Point3D.Distance3D(new Point3D(Atom1.x.FieldValue, Atom1.y.FieldValue, Atom1.z.FieldValue), new Point3D(Atom2.x.FieldValue, Atom2.y.FieldValue, Atom2.z.FieldValue)) + Environment.NewLine +
                    Atom1.ColumnFormatLine + Environment.NewLine +
                    Atom2.ColumnFormatLine + Environment.NewLine;
            }

            public static List<AtomPair> LoadAtomPairList(string filename)
            {
                if (!File.Exists(filename)) return new List<AtomPair>();

                var fileLines = File.ReadAllLines(filename);

                var atomPairList = new List<AtomPair>();

                var lineNumber = 0;
                while (lineNumber < fileLines.Length - 1)
                {
                    var line1 = fileLines[lineNumber];

                    while (line1.First() == ';')
                    {
                        lineNumber++;
                        line1 = fileLines[lineNumber];
                        continue;
                    }

                    var line2 = fileLines[lineNumber + 1];

                    if (String.IsNullOrWhiteSpace(line1) || String.IsNullOrWhiteSpace(line2) || line2.First() == ';')
                    {
                        throw new ArgumentException("Atom pair input file is corrupt");
                    }



                    var atom1 = new ATOM_Record(line1);
                    var atom2 = new ATOM_Record(line2);

                    var atomPair = new ProteinBioClass.AtomPair(atom1, atom2);

                    atomPairList.Add(atomPair);

                    lineNumber += 3;
                }

                return atomPairList;
            }

            public static void SaveAtomPairList(string filename, List<AtomPair> atomPairList)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                File.WriteAllLines(filename, atomPairList.Select(a => a.ToString()).ToList());
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="AtomPair" /> class.
            /// </summary>
            /// <param name="atom1">The first atom.</param>
            /// <param name="atom2">The second atom.</param>
            /// <param name="distance">
            ///     The distance between atom1 and atom2 (presumed already calculated as it is expensive to
            ///     calculate 3d distance).
            /// </param>
            public AtomPair(ATOM_Record atom1, ATOM_Record atom2, decimal distance = 0)
            {
                //PdbIdAtom1 = null;
                Atom1 = atom1;
                //PdbIdAtom2 = null;
                Atom2 = atom2;
                Distance = distance;
            }

            public AtomPair(string pdbIdAtom1, ATOM_Record atom1, string pdbIdAtom2, ATOM_Record atom2, decimal distance = 0)
            {
                Atom1 = atom1;
                Atom2 = atom2;
                Distance = distance;

                Atom1FullProteinInterfaceId.ProteinId = pdbIdAtom1;

                Atom2FullProteinInterfaceId.ProteinId = pdbIdAtom2;
            }


            public AtomPair(string pdbIdAtom1, ATOM_Record atom1, int chainIndexAtom1, string pdbIdAtom2, int chainIndexAtom2, ATOM_Record atom2, decimal distance = 0)
            {
                Atom1 = atom1;
                Atom2 = atom2;
                Distance = distance;

                Atom1FullProteinInterfaceId.ProteinId = pdbIdAtom1;
                Atom1FullProteinInterfaceId.ChainId = chainIndexAtom1;

                Atom2FullProteinInterfaceId.ProteinId = pdbIdAtom2;
                Atom2FullProteinInterfaceId.ChainId = chainIndexAtom2;
            }

            public AtomPair SwapAtoms()
            {
                var _atom2 = Atom1;
                var _atom2data = Atom1FullProteinInterfaceId;

                var _atom1 = Atom2;
                var _atom1data = Atom2FullProteinInterfaceId;

                Atom1 = _atom1;
                Atom1FullProteinInterfaceId = _atom1data;

                Atom2 = _atom2;
                Atom2FullProteinInterfaceId = _atom2data;

                return this;
            }
        }

        public class Point3D
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="Point3D" /> class.
            /// </summary>
            public Point3D()
                : this(0, 0, 0)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Point3D" /> class.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            public Point3D(decimal x, decimal y, decimal z)
            {
                X = x;
                Y = y;
                Z = z;
                ParseOK = true;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Point3D" /> class.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            public Point3D(string x, string y, string z)
            {
                ParseOK = true;

                decimal localX;
                if (Decimal.TryParse(x, out localX))
                {
                    X = localX;
                }
                else
                {
                    X = 0;
                    ParseOK = false;
                }

                decimal localY;
                if (Decimal.TryParse(y, out localY))
                {
                    Y = localY;
                }
                else
                {
                    Y = 0;
                    ParseOK = false;
                }

                decimal localZ;
                if (Decimal.TryParse(z, out localZ))
                {
                    Z = localZ;
                }
                else
                {
                    Z = 0;
                    ParseOK = false;
                }
            }

            /// <summary>
            ///     Gets or sets X. X is the point's position on the X axis.
            /// </summary>
            public decimal X { get; set; }

            /// <summary>
            ///     Gets or sets Y. Y is the point's position on the Y axis.
            /// </summary>
            public decimal Y { get; set; }

            /// <summary>
            ///     Gets or sets Z.  Z is the point's position on the Z axis.
            /// </summary>
            public decimal Z { get; set; }

            /// <summary>
            ///     Gets a value indicating whether or not the numbers were successfully parsed.
            /// </summary>
            public bool ParseOK { get; private set; }

            /// <summary>
            ///     Calculate the distance between two given points.
            /// </summary>
            /// <param name="pointA"></param>
            /// <param name="pointB"></param>
            /// <param name="abs"></param>
            /// <returns></returns>
            public static decimal Distance3D(Point3D pointA, Point3D pointB, bool abs = true)
            {
                var result = new Point3D(pointA.X - pointB.X, pointA.Y - pointB.Y, pointA.Z - pointB.Z);
                var distance3D = (decimal)Math.Sqrt(((double)((result.X * result.X) + (result.Y * result.Y) + (result.Z * result.Z))));

                if (abs)
                {
                    distance3D = Math.Abs(distance3D);
                }

                return distance3D;
            }
        }

        public static bool IsProteinAtomListContainerNullOrEmpty(AtomListContainer proteinAtomListContainer)
        {
            if (proteinAtomListContainer == null || proteinAtomListContainer.AtomList == null || proteinAtomListContainer.AtomList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public class AtomListContainer
        {
            /// <summary>
            ///     AtomList: A list of atoms which are part of a chain, proteinInterface, group or cluster
            /// </summary>
            public char ChainId = ' ';
            public List<ATOM_Record> AtomList = new List<ATOM_Record>();
        }

        public static List<Point3D> AtomRecordListToPoint3DList(AtomListContainer proteinAtomListContainer)
        {
            if (IsProteinAtomListContainerNullOrEmpty(proteinAtomListContainer))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinAtomListContainer));
            }

            var result = new List<Point3D>();

            result.AddRange(proteinAtomListContainer.AtomList.Select(t => new Point3D(t.x.FieldValue, t.y.FieldValue, t.z.FieldValue)).ToList());

            return result;
        }

        private static string NullEmptyOrWhiteSpaceToString(string nullEmptyOrWhiteSpaceString)
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

        public static string PdbIdFromPdbFilename(string pdbFilename)
        {
            const int pdbIdLen = 4;

            if (String.IsNullOrWhiteSpace(pdbFilename))
            {
                //throw new ArgumentOutOfRangeException(nameof(pdbFilename), pdbFilename, "parameter was " + NullEmptyOrWhiteSpaceToString(pdbFilename));
                return "";
            }

            var pdbId = Path.GetFileNameWithoutExtension(pdbFilename).Trim();
            //var chainId = "";

            if (pdbId.StartsWith("pdb")) pdbId = pdbId.Substring(3);

            var tokens = pdbId.Split(new char[] {'_', ',', '-', '.', ' ', '[', ']', '(', ')', '{', '}' });

            foreach (var t in tokens)
            {
                if (t.Length >= pdbIdLen && t.Substring(0,4).Any(char.IsLetter) && t.Substring(0,4).Any(char.IsDigit))
                {
                    pdbId = t;//.Substring(0, pdbIdLen);
                }

                //if (t.Length > pdbIdLen)
                //{
                //    chainId = t.Substring(pdbIdLen);
                //}
            }
            
            //if (pdbId.Length > pdbIdLen)
            //{
            //    pdbId = pdbId.Substring(pdbId.Length - 4);
            //}

            //if (pdbId.Length != pdbIdLen)
            //{
            //    throw new ArgumentException("PDB ID could not be extracted from parameter pdbFilename", nameof(pdbFilename));
            //}

            return pdbId.ToUpperInvariant();
        }

        private static bool IsLoadFilenameInvalid(string loadFilename)
        {
            if (String.IsNullOrWhiteSpace(loadFilename) || !File.Exists(loadFilename))
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
            if (Enum.IsDefined(typeof(StandardAminoAcids1L), aminoAcidNumber) ||
                Enum.IsDefined(typeof(AdditionalAminoAcids1L), aminoAcidNumber) ||
                Enum.IsDefined(typeof(AmbiguousAminoAcids1L), aminoAcidNumber) ||
                Enum.IsDefined(typeof(NonStandardAminoAcids1L), aminoAcidNumber))
            {
                return true;
            }
            return false;
        }

        public class StructureChainListContainer
        {
            public List<AtomListContainer> ChainList = new List<AtomListContainer>();
        }

        private static int? NullableTryParseInt32(string str)
        {
            int intValue;
            return Int32.TryParse(str, out intValue) ? (int?)intValue : null;
        }

        public static StructureChainListContainer PdbAtomicChains(string pdbFilename, char[] chainIdWhiteList,
            int minimumChains = -1, int maximumChains = -1, bool onlyCarbonAlphas = false)
        {
            if (!File.Exists(pdbFilename))
            {
                //return null;
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            var structureFileLines = File.ReadAllLines(pdbFilename);

            return PdbAtomicChains(structureFileLines, chainIdWhiteList, minimumChains, maximumChains, onlyCarbonAlphas);
        }

        public static StructureChainListContainer PdbAtomicChains(string[] structureFileLines, char[] chainIdWhiteList, int minimumChains = -1, int maximumChains = -1, bool onlyCarbonAlphas = false)
        {



            // Check min chains not more than max chains.
            if (minimumChains > maximumChains)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumChains));
            }

            const string lCarbonAlpha = "CA";

            const int atom_chain = 21;
            const int atom_chain_len = 1;

            if (chainIdWhiteList!=null&& chainIdWhiteList.Length>0) { structureFileLines = structureFileLines.Where(a => chainIdWhiteList.Contains(a[atom_chain])).ToArray();}

            // Load pdb/protein file, excluding all records but ATOM, HETATM and TER.
            var proteinDataBankFile = new ProteinDataBankFile(structureFileLines, new[]
            {
                ATOM_Record.ATOM_Field.FieldName,
                //HETATM_Record.HETATM_Field.FieldName,
                TER_Record.TER_Field.FieldName,
                MODEL_Record.MODEL_Field.FieldName,
                ENDMDL_Record.ENDMDL_Field.FieldName
            });


            // Make new array for atom chain.
            //List<ATOM_Record>[] proteinFileChains = new List<ATOM_Record>[maximumChains];
            var pdbFileChains = new StructureChainListContainer();

            //var fileError = false;
            //var chainCount = 0;
            // Loop through all the previously loaded protein file records to make lists of atoms in each chain.
            // Also make a list of residue numbers (which will be sorted later just in case it is out of order).

            var atomRecordListDictionary = new Dictionary<char, List<ProteinDataBankFileRecord>>();
            var hetAtomRecordListDictionary = new Dictionary<char, List<ProteinDataBankFileRecord>>();
            int terCount = 0;

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



                    if (onlyCarbonAlphas && atom.name.FieldValue.Trim().ToUpperInvariant() != lCarbonAlpha)
                    {
                        continue;
                    }

                    char chainIdKey = (atom.chainID.FieldValue.Trim()+' ')[0];

                    if (chainIdWhiteList != null && chainIdWhiteList.Length > 0 && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    if (!atomRecordListDictionary.ContainsKey(chainIdKey))
                    {
                        atomRecordListDictionary.Add(chainIdKey, new List<ProteinDataBankFileRecord>());
                    }

                    if (IsAminoAcidCodeValid(atom.resName.FieldValue))
                    {
                        atomRecordListDictionary[chainIdKey].Add(atom);
                    }

                }
                /*else if (currentRecord.GetType() == typeof(HETATM_Record))
                {
                    var hetatm = (HETATM_Record)currentRecord;

                    if (onlyCarbonAlphas && hetatm.name.FieldValue.Trim().ToUpperInvariant() != lCarbonAlpha)
                    {
                        continue;
                    }

                    char chainIdKey = (hetatm.chainID.FieldValue.Trim()+' ')[0];

                    if (chainIdWhiteList != null && chainIdWhiteList.Length > 0 && !chainIdWhiteList.Contains(chainIdKey))
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

                    if (IsAminoAcidCodeValid(hetatm.resName.FieldValue))
                    {
                        hetAtomRecordListDictionary[chainIdKey].Add(hetatm);
                    }
                }*/
                else if (currentRecord.GetType() == typeof(TER_Record))
                {
                    var ter = (TER_Record)currentRecord;

                    char chainIdKey = (ter.chainID.FieldValue.Trim()+' ')[0];

                    if (chainIdWhiteList != null && !chainIdWhiteList.Contains(chainIdKey))
                    {
                        continue;
                    }

                    terCount++;

                    if (maximumChains > -1 && terCount >= maximumChains)
                    {
                        break;
                        //return null;
                    }
                }
                else if (currentRecord.GetType() == typeof(ENDMDL_Record))
                {
                    break;
                }
            }

            // file has been parsed so clear used file data from memory as soon as possible
            proteinDataBankFile.UnloadFile();

            int totalChains = atomRecordListDictionary.Count > hetAtomRecordListDictionary.Count ? atomRecordListDictionary.Count : hetAtomRecordListDictionary.Count;

            var atomsChainIdList = atomRecordListDictionary.Select(a => a.Key);
            //var hetsChainIdList = hetAtomRecordListDictionary.Select(a => a.Key);
            var jointChainIdList = new List<char>();
            jointChainIdList.AddRange(atomsChainIdList);
            //jointChainIdList.AddRange(hetsChainIdList);
            jointChainIdList = jointChainIdList.Distinct().ToList();
            jointChainIdList = jointChainIdList.OrderBy(a => a).ToList();

            //for (int chainIndex = 0; chainIndex < totalChains; chainIndex++)
            foreach (var chainId in jointChainIdList)
            {
                pdbFileChains.ChainList.Add(new AtomListContainer()
                {
                    ChainId = chainId
                });
            }

            atomRecordListDictionary = atomRecordListDictionary.OrderBy(a => a.Key).ToDictionary(a => a.Key, a => a.Value);

            //int chainIndex2 = -1;
            foreach (var atomRecordListKvp in atomRecordListDictionary)
            {
                //chainIndex2++;
                char chainName = atomRecordListKvp.Key;
                var chainIndex2 = jointChainIdList.IndexOf(chainName);


                List<ProteinDataBankFileRecord> chainRecords = atomRecordListKvp.Value;

                if (chainRecords == null || chainRecords.Count == 0)
                {
                    continue;
                }

                chainRecords = chainRecords.OrderBy(a => NullableTryParseInt32(((ATOM_Record)a).serial.FieldValue)).ToList();

                pdbFileChains.ChainList[chainIndex2].AtomList = chainRecords.Select(a => (ATOM_Record)a).ToList();
            }

            //hetAtomRecordListDictionary = hetAtomRecordListDictionary.OrderBy(a => a.Key).ToDictionary(a => a.Key, a => a.Value);

            //int chainIndex3 = -1;
            /*foreach (var hetAtomRecordListKvp in hetAtomRecordListDictionary)
            {
                //chainIndex3++;
                char chainName = hetAtomRecordListKvp.Key;
                var chainIndex3 = jointChainIdList.IndexOf(chainName);

                List<ProteinDataBankFileRecord> chainRecords = hetAtomRecordListKvp.Value;

                if (chainRecords == null || chainRecords.Count == 0)
                {
                    continue;
                }

                chainRecords = chainRecords.OrderBy(a => NullableTryParseInt32(((HETATM_Record)a).serial.FieldValue)).ToList();

                foreach (ProteinDataBankFileRecord proteinDataBankFileRecord in chainRecords)
                {
                    var chainRecord = (HETATM_Record)proteinDataBankFileRecord;

                    string residueSequenceToFind = chainRecord.resSeq.FieldValue;
                    char atomChainId = (chainRecord.chainID.FieldValue.Trim()+' ')[0];

                    if (!atomRecordListDictionary.ContainsKey(atomChainId) || atomRecordListDictionary[atomChainId].Count(a => ((ATOM_Record)a).resSeq.FieldValue == residueSequenceToFind) == 0)
                    {
                        ATOM_Record atom = ConvertHetatmRecordToAtomRecord(chainRecord);

                        pdbFileChains.ChainList[chainIndex3].AtomList.Add(atom);
                    }
                }
            }*/

            int nonEmptyChainCount = pdbFileChains.ChainList.Count(a => a != null && a.AtomList != null && a.AtomList.Count > 0);

            if ((minimumChains == -1 || nonEmptyChainCount >= minimumChains) && (maximumChains == -1 || nonEmptyChainCount <= maximumChains))
            {
                return pdbFileChains;
            }

            ////////Console.WriteLine("Too many chains (" + nonEmptyChainCount + "): " + pdbFilename);
            return null;
        }

        public static List<AtomPair> FindInteractions(CancellationToken cancellationToken, decimal maxAtomInterationDistance /*= 8.0m*/, string pdbFilename, Dictionary<string, List<char>> pdbIdChainIdList = null, bool breakWhenFirstInteractionFound = false, int totalThreads = -1, bool sort = true, int requiredChains = -1)
        {
            if (IsLoadFilenameInvalid(pdbFilename)) // && ParameterValidation.IsProteinChainListContainerNullOrEmpty(pdbFileChains))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            var proteinId = PdbIdFromPdbFilename(pdbFilename);

            var chainIdList = pdbIdChainIdList != null && pdbIdChainIdList.Count > 0 ? (proteinId != null && pdbIdChainIdList.ContainsKey(proteinId) ? pdbIdChainIdList[proteinId].ToArray() : null) : null;

            var proteinFileChains = PdbAtomicChains(pdbFilename, chainIdList, requiredChains, requiredChains, false);// true);

            var atomPairList = FindInteractions(cancellationToken, maxAtomInterationDistance, proteinId, proteinFileChains, pdbIdChainIdList, breakWhenFirstInteractionFound, totalThreads, sort, requiredChains);

            if (atomPairList == null)
            {
                // only save if null, otherwise, already saved in other method
                atomPairList = new List<AtomPair>();
            }

            return atomPairList;
        }

        public static List<AtomPair> FindInteractions(CancellationToken cancellationToken, decimal maxAtomInterationDistance /*= 8.0m*/, string proteinId, StructureChainListContainer proteinFileChains, Dictionary<string, List<char>> pdbIdChainIdList = null, bool breakWhenFirstInteractionFound = false, int totalThreads = -1, bool sort = true, int requiredChains = -1)
        {
            // check required number of chains are found
            if (proteinFileChains == null || proteinFileChains.ChainList == null || (requiredChains > -1 && proteinFileChains.ChainList.Count != requiredChains))
            {
                return null;
            }

            // check that all chains have atoms
            if (proteinFileChains.ChainList.Any(chain => chain.AtomList == null || chain.AtomList.Count == 0))
            {
                return null;
            }

            // Make list of 3D positions of atoms.
            var positions = new List<Point3D>[proteinFileChains.ChainList.Count];

            for (int chainIndex = 0; chainIndex < proteinFileChains.ChainList.Count; chainIndex++)
            {
                positions[chainIndex] = AtomRecordListToPoint3DList(proteinFileChains.ChainList[chainIndex]);
            }

            var tasks = new List<Task<List<AtomPair>>>();

            for (int chainIndexA = 0; chainIndexA < proteinFileChains.ChainList.Count; chainIndexA++)
            {
                for (int chainIndexB = 0; chainIndexB < proteinFileChains.ChainList.Count; chainIndexB++)
                {
                    if (chainIndexB == chainIndexA || chainIndexB < chainIndexA)
                    {
                        continue;
                    }

                    WorkDivision<List<AtomPair>> workDivision = new WorkDivision<List<AtomPair>>(proteinFileChains.ChainList[chainIndexA].AtomList.Count, totalThreads);

                    bool breakOut = false;
                    var lockBreakOut = new object();

                    for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
                    {
                        int localThreadIndex = threadIndex;
                        int localChainIndexA = chainIndexA;
                        int localChainIndexB = chainIndexB;
                        WorkDivision<List<AtomPair>> localWorkDivision = workDivision;

                        Task<List<AtomPair>> task = Task.Run(() =>
                        {
                            var taskResult = new List<AtomPair>();

                            for (int atomIndexA = localWorkDivision.ThreadFirstIndex[localThreadIndex]; atomIndexA <= localWorkDivision.ThreadLastIndex[localThreadIndex]; atomIndexA++)
                            {
                                if (breakOut)
                                {
                                    break;
                                }

                                for (int atomIndexB = 0; atomIndexB < proteinFileChains.ChainList[localChainIndexB].AtomList.Count; atomIndexB++)
                                {
                                    if (breakOut || (breakWhenFirstInteractionFound && taskResult.Count > 0))
                                    {
                                        lock (lockBreakOut)
                                        {
                                            breakOut = true;
                                        }

                                        break;
                                    }

                                    if ((!positions[localChainIndexA][atomIndexA].ParseOK) || (!positions[localChainIndexB][atomIndexB].ParseOK)) continue;

                                    decimal atomicDistanceAngstroms3D = Point3D.Distance3D(positions[localChainIndexA][atomIndexA], positions[localChainIndexB][atomIndexB], true);

                                    // Chemical proteinInterface bonds found at 5 angstrom or less.
                                    if (atomicDistanceAngstroms3D <= 0.0m || atomicDistanceAngstroms3D > maxAtomInterationDistance) continue;

                                    var atomPair = new AtomPair(
                                        proteinId,
                                        proteinFileChains.ChainList[localChainIndexA].AtomList[atomIndexA],
                                        localChainIndexA,
                                        proteinId,
                                        localChainIndexB,
                                        proteinFileChains.ChainList[localChainIndexB].AtomList[atomIndexB],
                                        atomicDistanceAngstroms3D);


                                    taskResult.Add(atomPair);
                                }
                            }

                            if (taskResult.Count == 0)
                            {
                                return null;
                            }

                            return taskResult;
                        }, cancellationToken);

                        workDivision.TaskList.Add(task);
                    }

                    tasks.AddRange(workDivision.TaskList);
                }
            }


            try
            {
                Task[] tasksToWait = tasks.Where(task => task != null && !task.IsCompleted).ToArray<Task>();
                if (tasksToWait.Length > 0)
                {
                    Task.WaitAll(tasksToWait);
                }
            }
            catch (AggregateException)
            {
            }

            // merge all results

            var atomPairList = new List<AtomPair>();

            foreach (var task in tasks.Where(t => t != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted && t.Result != null && t.Result.Count > 0))
            {
                atomPairList.AddRange(task.Result);
            }

            if (sort && atomPairList != null && atomPairList.Count > 1)
            {
                atomPairList = atomPairList
                    .OrderBy(i => NullableTryParseInt32(i.Atom1.resSeq.FieldValue))
                    .ThenBy(i => NullableTryParseInt32(i.Atom1.serial.FieldValue))
                    .ThenBy(j => NullableTryParseInt32(j.Atom2.resSeq.FieldValue))
                    .ThenBy(j => NullableTryParseInt32(j.Atom2.serial.FieldValue))
                    .ToList();
            }

            //if (useCache)
            //{
            //    InteractionsCache.SavePdbInteractionCache(proteinId, atomPairList, requiredChains);
            //}

            return atomPairList;
        }
    }
}
