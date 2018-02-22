using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace SlidingWindowThreading
{
    public class SlidingWindowThreading
    {
        public static string Pir(List<string> seqList)
        {
            var p = @">P1;query" + Environment.NewLine + "sequence:query::::::::" + Environment.NewLine;

            for (var index = 0; index < seqList.Count; index++)
            {
                var seq = seqList[index];

                while (seq.Length > 80)
                {
                    p = p + seq.Substring(0, 80) + Environment.NewLine;
                    seq = seq.Substring(80);
                }

                if (seq.Length > 0) p = p + seq;

                if (index == seqList.Count - 1)
                {
                    p = p + "*" + Environment.NewLine;
                }
                else
                {
                    p = p + "/" + Environment.NewLine;
                }
            }

            return p;
        }

        public static class StructureToSequenceAlignment
        {
            public class StructureToSequenceAlignmentResult
            {
                public string FastaSequence;
                public string PdbSequence;
                public string SuperSequence;
                public int SuperSequenceStartIndex;
                public int SuperSequenceStartResSeq;
                public int SuperSequenceLastIndex;
                public int SuperSequenceLastResSeq;
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

            public static StructureToSequenceAlignmentResult Align(List<ATOM_Record> atomList, string fastaSequence, string pdbSequence, int first = -1, int last = -1)
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

                if (first != -1 && last != -1)
                {
                    result.ChainResSeqMin = first;
                    result.ChainResSeqMax = last;
                }

                result.SuperSequence = new string(result.FastaSequenceAligned.Where((a, i) => result.AlignmentMap[i] >= result.ChainResSeqMin && result.AlignmentMap[i] <= result.ChainResSeqMax).ToArray());

                var x = result.FastaSequenceAligned.Select((a, i) => result.AlignmentMap[i] >= result.ChainResSeqMin && result.AlignmentMap[i] <= result.ChainResSeqMax).ToList();
                result.SuperSequenceStartIndex = x.IndexOf(true);
                result.SuperSequenceLastIndex = x.LastIndexOf(true);
                result.SuperSequenceStartResSeq = result.AlignmentMap[result.SuperSequenceStartIndex];
                result.SuperSequenceLastResSeq = result.AlignmentMap[result.SuperSequenceLastIndex];

                return result;
            }
        }

        public class InterfaceData
        {
            public int StartIndex;
            public int Length;
            public string InterfaceSequence;
        }
        public static InterfaceData InterfaceSubsequence(string sequence, int start, int end, int length, bool oneBased = false)
        {
            // returns a subsequence of sequence, from the centre of start and end, of given length, assumes numbers are 1-based
            // start and end are taken from residue sequence indexes in the pdb structure files
            // length may be longer than ((end-start)+1) for getting the flanking residues around the interface for substitution with mismatching sized interfaces (can be larger or smaller)
            string result;

            if (oneBased)
            {
                start--;
                end--;
            }

            var mid = (start + end) / 2;

            var realstart = mid - length / 2;

            if (realstart < 0) realstart = 0;

            var realend = realstart + length - 1;

            if (realend > sequence.Length - 1) realend = sequence.Length - 1;

            var reallength = sequence.Length - realstart > length ? length : sequence.Length - realstart;

            if (reallength < length && realstart > 0)
            {
                var lendiff = Math.Abs(length - reallength);
                realstart = realstart - lendiff;
                if (realstart < 0) realstart = 0;
                reallength = sequence.Length - realstart > length ? length : sequence.Length - realstart;
            }

            //result = sequence.Substring(realstart, reallength); //Math.Abs(realend - realstart) + 1);

            return new InterfaceData { StartIndex = realstart, Length = reallength, InterfaceSequence = sequence.Substring(realstart, reallength) };
            //return result;
        }



        public static StructureToSequenceAlignment.StructureToSequenceAlignmentResult GetSequence(string pdbId, char chainId, int first = -1, int last = -1)
        {
            //var chainId = pdbId[4];
            pdbId = pdbId.Substring(0, 4);
            var pdbFilename = @"c:\pdbe\" + pdbId + ".pdb";
            var fastaFilename = @"c:\pdbe\pdb_seqres.fasta";
            var fastaSequence = Sequence.LoadSequenceFile(fastaFilename, new string[] { null, "", "protein" }).First(a => a.IdSplit.PdbId.ToUpperInvariant() == pdbId.ToUpperInvariant() && a.IdSplit.ChainId == chainId);
            var pdbSequence = Sequence.LoadStructureFile(pdbFilename, new[] { chainId }, true, null, null, '-', '-').First(a => a.IdSplit.PdbId.ToUpperInvariant() == pdbId.ToUpperInvariant() && a.IdSplit.ChainId == chainId);
            var atoms = ProteinBioClass.PdbAtomicChains(pdbFilename, new char[] { chainId }).ChainList.First(a => a.ChainId == chainId).AtomList;
            var align = StructureToSequenceAlignment.Align(atoms, fastaSequence.FullSequence, pdbSequence.FullSequence, first, last);

            return align;

        }

        public static void MonomerTemplate(string pdbFile, string pdbOutputFile, char chain)
        {
            const int atom_chain = 21;

            var pdb = File.ReadAllLines(pdbFile).ToList();

            if (pdb.Any(a => a.StartsWith("MODEL "))) pdb = pdb.Where((a, i) => i < pdb.FindIndex(b => b.StartsWith("ENDMDL "))).ToList();

            pdb = pdb.Where(a => a.StartsWith("ATOM ") && a[atom_chain] == chain).ToList();

            Directory.CreateDirectory(Path.GetDirectoryName(pdbOutputFile));

            File.WriteAllLines(pdbOutputFile, pdb);
        }

        public static void DimerTemplate(string pdbFile, string pdbOutputFile, char recChainId, char ligChainId)
        {
            const int atom_chain = 21;

            var pdb = File.ReadAllLines(pdbFile).ToList();

            if (pdb.Any(a => a.StartsWith("MODEL "))) pdb = pdb.Where((a, i) => i < pdb.FindIndex(b => b.StartsWith("ENDMDL "))).ToList();

            pdb = pdb.Where(a => a.StartsWith("ATOM ") && (a[atom_chain] == recChainId || a[atom_chain] == ligChainId)).ToList();

            var recPdb = pdb.Where(a => a.StartsWith("ATOM ") && a[atom_chain] == recChainId).Select(a => a.Remove(atom_chain, 1).Insert(atom_chain, "A")).ToList();
            var ligPdb = pdb.Where(a => a.StartsWith("ATOM ") && a[atom_chain] == ligChainId).Select(a => a.Remove(atom_chain, 1).Insert(atom_chain, "B")).ToList();

            var recLigPdb = recPdb;
            recLigPdb.AddRange(ligPdb);

            Directory.CreateDirectory(Path.GetDirectoryName(pdbOutputFile));

            File.WriteAllLines(pdbOutputFile, recLigPdb);
        }

        public class RecLigInfo
        {
            public string PdbId;

            public char RecChainId;
            public string RecSequence;
            public string RecInfSequence;
            public int RecInfPosInRecSeq;
            public int RecInfStart;
            public int RecInfEnd;
            public StructureToSequenceAlignment.StructureToSequenceAlignmentResult RecSequenceAlignment;
            public StructureToSequenceAlignment.StructureToSequenceAlignmentResult RecInfSequenceAlignment;

            public char LigChainId;
            public string LigSequence;
            public string LigInfSequence;
            public int LigInfPosInLigSeq;
            public int LigInfStart;
            public int LigInfEnd;
            public StructureToSequenceAlignment.StructureToSequenceAlignmentResult LigSequenceAlignment;
            public StructureToSequenceAlignment.StructureToSequenceAlignmentResult LigInfSequenceAlignment;
        }

        public static void MakeCrystalTemplates(List<Tuple<string, char, char>> pdbIdList)
        {
            foreach (var t in pdbIdList)
            {
                var pdbId = t.Item1;
                var recId = t.Item2;
                var ligId = t.Item3;

                DimerTemplate(@"c:\pdbe\" + pdbId + ".pdb", @"c:\pdb_templates\" + pdbId + recId + ligId + ".pdb", recId, ligId);



                var recSeq = GetSequence(pdbId, recId);
                var ligSeq = GetSequence(pdbId, ligId);


                var pirData = Pir(new List<string>() { recSeq.SuperSequence, ligSeq.SuperSequence });

                var pirFile = @"c:\pdb_templates\" + pdbId + recId + ligId + ".ali";

                File.WriteAllText(pirFile, pirData);
            }
        }


        static void Main(string[] args)
        {

            //var crystals = new List<Tuple<string, char, char>>();
            //crystals.Add(new Tuple<string, char, char>("2SIC", 'E', 'I'));
            //crystals.Add(new Tuple<string, char, char>("3BX1", 'A', 'C'));
            //crystals.Add(new Tuple<string, char, char>("1RGI", 'A', 'G'));


            //MakeCrystalTemplates(crystals);

            //return;
            // uncomment one of the options below or add a new one

            //var pdbSumInterfaceIdFirstLastList = @"
            //3JBIV 990 1001
            //1T44G 95 110
            //4PKHB 114 137
            //5AFUb 52 60
            //1RGIAG 95 110
            //1H1VG 473 488
            //4EAHA 641 653
            //1KXPD 196 210
            //1M8QA 529 544

            //3JBIAV 990 1001
            //1T44AG 95 110
            //4PKHAB 114 137
            //1RGIAG 95 110
            //1H1VAG 473 488
            //4EAHHA 641 653
            //1KXPAD 196 210

            //var pdbSumInterfaceIdFirstLastList = @"
            //4GI3AC 22 29
            //2SICEI 65 74
            //1SBNEI 35 53
            //4LVNAP 207 216
            //1OYVBI 54 64
            //1V5IAB 68 76
            //1R0REI 11 20
            //3BX1AC 84 94
            //";


            // receptor ligand lig-inf-start lig-inf-end
            /*
            var ligandSiblingInterfaceList = @"
            1RGIAG 95 110            
            3JBIAV 990 1001
            4PKHAB 114 137
            1H1VAG 473 488
            4EAHHA 641 653
            1KXPAD 196 210
            ";
            */
            
            var ligandSiblingInterfaceList = @"
            4GI3AC 22 29
            2SICEI 65 74
            1SBNEI 35 53
            4LVNAP 207 216
            1OYVBI 54 64
            1V5IAB 68 76
            1R0REI 11 20
            3BX1AC 84 94
            ";
            

            /*
            2SICEI 65 74
            3BX1AC 84 94
            1RGIAG 95 110
            */

            var recLigInfoList = ligandSiblingInterfaceList.Trim().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(a =>
            {
                var b = a.Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                var pdbId = b[0].Substring(0, 4);

                var recChainId = b[0][4];
                var recInfStart = -1;
                var recInfEnd = -1;
                var recSequenceAlignment = GetSequence(pdbId, recChainId);
                var recSequence = recSequenceAlignment.SuperSequence;
                var recInfSequence = "";
                var recInfPosInRecSeq = -1;


                var ligChainId = b[0][5];
                var ligInfStart = int.Parse(b[1]);
                var ligInfEnd = int.Parse(b[2]);
                var ligSequenceAlignment = GetSequence(pdbId, ligChainId);
                var ligSequence = ligSequenceAlignment.SuperSequence;
                var ligInfSequenceAlignment = GetSequence(pdbId, ligChainId, ligInfStart, ligInfEnd);



                var ligInfSequence = ligInfSequenceAlignment.SuperSequence;
                var ligInfPosInLigSeq = ligSequence.IndexOf(ligInfSequence);

                return new RecLigInfo()
                {
                    PdbId = pdbId,
                    RecChainId = recChainId,
                    RecInfStart = recInfStart,
                    RecInfEnd = recInfEnd,
                    RecSequence = recSequence,
                    RecInfSequence = recInfSequence,
                    RecInfPosInRecSeq = recInfPosInRecSeq,
                    RecSequenceAlignment = recSequenceAlignment,
                    RecInfSequenceAlignment = null,

                    LigChainId = ligChainId,
                    LigInfStart = ligInfStart,
                    LigInfEnd = ligInfEnd,
                    LigSequence = ligSequence,
                    LigInfSequence = ligInfSequence,
                    LigInfPosInLigSeq = ligInfPosInLigSeq,
                    LigSequenceAlignment = ligSequenceAlignment,
                    LigInfSequenceAlignment = ligInfSequenceAlignment,
                };

            }).ToList();

            var allSequenceIds = recLigInfoList.Select(a => a.PdbId + a.RecChainId + a.LigChainId).Distinct().OrderBy(a => a).ToList();
            var rootFolder = @"C:\pdbe_split_4\sw_" + string.Join("_", allSequenceIds) + @"\";
            Directory.CreateDirectory(rootFolder);
            //var rootFolderSubDirs = Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories).ToList();
            //rootFolderSubDirs.Remove(Path.GetDirectoryName(rootFolder));

            if (recLigInfoList.Any(a => string.IsNullOrWhiteSpace(a.LigInfSequence)))
            {
                Console.WriteLine("Error: empty interface");
                return;
            }

            if (recLigInfoList.Any(a => string.IsNullOrWhiteSpace(a.LigSequence) || a.LigSequence.Length < 50))
            {
                Console.WriteLine("Error: empty/short sequence");
                return;
            }

            var fastaSequences = recLigInfoList.Select(a => ">" + a.PdbId + a.LigChainId + "_pdb\r\n" + a.LigSequence).ToList();
            var fastaInfSequences = recLigInfoList.Select(a => ">" + a.PdbId + a.LigChainId + "_interface\r\n" + a.LigInfSequence).ToList();

            File.WriteAllLines(rootFolder + "sequences.fasta", fastaSequences);
            File.WriteAllLines(rootFolder + "interfaces_pdbsum.fasta", fastaInfSequences);


            if (recLigInfoList.Any(a => a.LigSequence.IndexOf(a.LigInfSequence) != a.LigSequence.LastIndexOf(a.LigInfSequence)))
            {
                Console.WriteLine("More than one interface match in the sequence");
                return;
            }



            var folders = new List<string>();

            foreach (var template in recLigInfoList.Where(a => (new List<string>() { "1RGI", "3BX1", "2SIC" }).Contains(a.PdbId)))
            {


                /*
                // replacement with direct overwrite of interface
                foreach (var templateForSiblingInterface in recLigInfoList)
                {
                    var superOffset = templateForSiblingInterface.LigSequenceAlignment.SuperSequenceStartIndex;

                    var siblingInterfaceToFit = InterfaceSubsequence(templateForSiblingInterface.LigSequence, templateForSiblingInterface.LigInfStart - superOffset, templateForSiblingInterface.LigInfEnd - superOffset, templateForModelling.LigInfSequence.Length).InterfaceSequence;

                    var substitutionDescription = "sibling_" + templateForSiblingInterface.PdbId + templateForSiblingInterface.RecChainId + templateForSiblingInterface.LigChainId;

                    var templateFolder = rootFolder + templateForModelling.PdbId + templateForModelling.RecChainId + templateForModelling.LigChainId + @"\" + templateForSiblingInterface.PdbId + templateForSiblingInterface.RecChainId + templateForSiblingInterface.LigChainId + @"\";
                    var currentFolder = rootFolder + templateForModelling.PdbId + templateForModelling.RecChainId + templateForModelling.LigChainId + @"\" + templateForSiblingInterface.PdbId + templateForSiblingInterface.RecChainId + templateForSiblingInterface.LigChainId + @"\" + substitutionDescription + @"\";
                    Directory.CreateDirectory(Path.GetDirectoryName(currentFolder));
                    rootFolderSubDirs.Remove(Path.GetDirectoryName(currentFolder));

                    var ligSeqModified = templateForModelling.LigSequence.Remove(templateForModelling.LigInfPosInLigSeq, siblingInterfaceToFit.Length).Insert(templateForModelling.LigInfPosInLigSeq, siblingInterfaceToFit);

                    var file = currentFolder + "template_ligand_after_substitution.ali";
                    var seqModPir = Pir(new List<string>() { templateForModelling.RecSequence, ligSeqModified });

                    File.WriteAllText(file, seqModPir);
                    folders.Add(currentFolder);

                    //DimerTemplate(, templateForModelling.RecChainId, templateForModelling.LigChainId);

                    var templateSrc = @"c:\pdb_templates\repaired_crystal_and_repaired_model\" + templateForModelling.PdbId + templateForModelling.RecChainId + templateForModelling.LigChainId + ".pdb"; //@"c:\pdbe\" + templateForModelling.PdbId + ".pdb";
                    var templateDest = templateFolder + "template_ligand_all.pdb";
                    File.Copy(templateSrc, templateDest, true);

                    if (templateForModelling.PdbId == templateForSiblingInterface.PdbId)
                    {
                        Debug.WriteLine("");
                    }
                }
                */

                // replacements with sliding window method
                foreach (var sibling in recLigInfoList/*.Where(a => a == templateForModelling).ToList()*/)//.Where(a => a.PdbId == templateForModelling.PdbId && a.RecChainId == templateForModelling.RecChainId))
                {

                    if (template != sibling && (template.LigInfSequence.Contains(sibling.LigInfSequence) || sibling.LigInfSequence.Contains(template.LigInfSequence)))
                    {
                        Debug.WriteLine("template and sibling have matching interface");
                        continue;
                    }

                    var currentFolder = rootFolder + template.PdbId + template.RecChainId + template.LigChainId + @"\" + sibling.PdbId + sibling.RecChainId + sibling.LigChainId + @"\";
                    Directory.CreateDirectory(Path.GetDirectoryName(currentFolder));

                    var templateSrc = @"c:\pdb_templates\repaired_crystal_and_repaired_model\" + template.PdbId + template.RecChainId + template.LigChainId + ".pdb"; //@"c:\pdbe\" + templateForModelling.PdbId + ".pdb";
                    var templateDest = currentFolder + "template_ligand_all.pdb";
                    File.Copy(templateSrc, templateDest, true);

                    // 1. slide sibling interface over template interface (if bigger, try each possible)
                    // 2. best alignments?
                    // 3. 

                    // 1. ignore size difference (sibling can be same size, longer or shorter, still copied as-is)

                    //Debug.WriteLine("Template: " + template.LigInfSequence + " " + template.LigInfSequence.Length);
                    //Debug.WriteLine("Sibling: " + sibling.LigInfSequence + " " + sibling.LigInfSequence.Length);

                    //part 0 - random sequence tests
                    if (template == sibling)
                    {
                        var firstPosition = template.LigInfPosInLigSeq - 50;// (template.LigInfSequence.Length - 1);
                        if (firstPosition < 0) firstPosition = 0;

                        var lastPosition = template.LigInfPosInLigSeq + template.LigInfSequence.Length + 50;

                        if (lastPosition + (template.LigInfSequence.Length - 1) > template.LigSequence.Length) lastPosition = template.LigSequence.Length - sibling.LigInfSequence.Length;

                        var substitutionMode = "0";

                        for (var i = firstPosition; i <= lastPosition; i++)
                        {
                            var overlap = ProteinBioClass.InterfaceOverlapPercentage(i, (i + template.LigInfSequence.Length) - 1, template.LigInfPosInLigSeq, (template.LigInfPosInLigSeq + template.LigInfSequence.Length) - 1); overlap = Math.Round(overlap, 2);

                            var random = template.LigSequence.Substring(i, template.LigInfSequence.Length);

                            var ligSeqMod = template.LigSequence.Remove(template.LigInfPosInLigSeq, template.LigInfSequence.Length).Insert(template.LigInfPosInLigSeq, random);

                            currentFolder = rootFolder + template.PdbId + template.RecChainId + template.LigChainId + @"\" +
                                template.PdbId + template.RecChainId + template.LigChainId + @"\" +
                                substitutionMode + "_" + (i + 1).ToString().PadLeft(4, '0') + "_" + (template.LigInfSequence.Length + 1).ToString().PadLeft(4, '0') + "_" + (sibling.LigInfSequence.Length + 1).ToString().PadLeft(4, '0') + "_" + (overlap) + @"\";

                            

                            Directory.CreateDirectory(Path.GetDirectoryName(currentFolder));
                            var file = currentFolder + "template_ligand_after_substitution.ali";
                            var seqModPir = Pir(new List<string>() { template.RecSequence, ligSeqMod });

                            if (!File.Exists(file))
                            {
                                if (!folders.Contains(currentFolder)) folders.Add(currentFolder);
                                File.WriteAllText(file, seqModPir);
                            }
                        }
                    }
                    continue;

                    {
                        //Debug.WriteLine("Part 1");
                        var firstPosition = template.LigInfPosInLigSeq - (sibling.LigInfSequence.Length - 1);
                        if (firstPosition < 0) firstPosition = 0;

                        var lastPosition = template.LigInfPosInLigSeq + (template.LigInfSequence.Length - 1);
                        if (lastPosition + (sibling.LigInfSequence.Length - 1) > template.LigSequence.Length) lastPosition = template.LigSequence.Length - sibling.LigInfSequence.Length;

                        for (var i = firstPosition; i <= lastPosition; i++)
                        {
                            var substitutionMode = "1";

                            var overlap = ProteinBioClass.InterfaceOverlapPercentage(i, (i + sibling.LigInfSequence.Length) - 1, template.LigInfPosInLigSeq, (template.LigInfPosInLigSeq + template.LigInfSequence.Length) - 1); overlap = Math.Round(overlap, 2);

                            var ligSeqMod = template.LigSequence.Remove(i, sibling.LigInfSequence.Length).Insert(i, sibling.LigInfSequence);


                            currentFolder = rootFolder + template.PdbId + template.RecChainId + template.LigChainId + @"\" +
                                sibling.PdbId + sibling.RecChainId + sibling.LigChainId + @"\" +
                                substitutionMode + "_" + (i + 1).ToString().PadLeft(4, '0') + "_" + (template.LigInfSequence.Length + 1).ToString().PadLeft(4, '0') + "_" + (sibling.LigInfSequence.Length + 1).ToString().PadLeft(4, '0') + "_" + (overlap) + @"\";
                            if (!folders.Contains(currentFolder)) folders.Add(currentFolder);

                            Directory.CreateDirectory(Path.GetDirectoryName(currentFolder));
                            var file = currentFolder + "template_ligand_after_substitution.ali";
                            var seqModPir = Pir(new List<string>() { template.RecSequence, ligSeqMod });
                            File.WriteAllText(file, seqModPir);

                            //                        Debug.WriteLine(io + " " + ligSeqMod);
                            //Debug.WriteLine("");
                        }
                    }

                    
                    // 2. keep longer sibling interface the same size as template interface
                    if (sibling.LigInfSequence.Length > template.LigInfSequence.Length)
                    {
                        //Debug.WriteLine("Part 2");
                        var firstPosition = template.LigInfPosInLigSeq - (template.LigInfSequence.Length - 1);
                        var lastPosition = template.LigInfPosInLigSeq + (template.LigInfSequence.Length - 1);

                        for (var i = firstPosition; i <= lastPosition; i++)
                        {
                            for (var j = 0; j <= sibling.LigInfSequence.Length - template.LigInfSequence.Length; j++)
                            {
                                

                                var ligSeqMod = template.LigSequence.Remove(i, template.LigInfSequence.Length).Insert(i, sibling.LigInfSequence.Substring(j, template.LigInfSequence.Length));


                                var substitutionMode = "2";
                                var overlap = ProteinBioClass.InterfaceOverlapPercentage(i, (i + template.LigInfSequence.Length) - 1, template.LigInfPosInLigSeq, (template.LigInfPosInLigSeq + template.LigInfSequence.Length) - 1);
                                overlap = Math.Round(overlap, 2);

                                currentFolder = rootFolder + template.PdbId + template.RecChainId + template.LigChainId + @"\" +
                                    sibling.PdbId + sibling.RecChainId + sibling.LigChainId + @"\" +
                                    substitutionMode + "_" + (i + 1).ToString().PadLeft(4, '0') + "_" + (j + 1).ToString().PadLeft(4,'0') + "_" + (overlap) + @"\";
                                if (!folders.Contains(currentFolder)) folders.Add(currentFolder);

                                Directory.CreateDirectory(Path.GetDirectoryName(currentFolder));
                                var file = currentFolder + "template_ligand_after_substitution.ali";
                                var seqModPir = Pir(new List<string>() { template.RecSequence, ligSeqMod });
                                File.WriteAllText(file, seqModPir);
                            }
                        }

                        //Debug.WriteLine("");
                    }

                    // 3. delete original template interface, insert sibling interface (will already be in part 1 if size is equal)
                    if (sibling.LigInfSequence.Length != template.LigInfSequence.Length)
                    {
                        Debug.WriteLine("Part 3");

                        var ligSeqMod = template.LigSequence.Remove(template.LigInfPosInLigSeq, template.LigInfSequence.Length).Insert(template.LigInfPosInLigSeq, sibling.LigInfSequence);

                        var substitutionMode = "3";
                        

                        currentFolder = rootFolder + template.PdbId + template.RecChainId + template.LigChainId + @"\" + 
                            sibling.PdbId + sibling.RecChainId + sibling.LigChainId + @"\" + 
                            substitutionMode + "_" + "delete-insert" + @"\";
                        if (!folders.Contains(currentFolder)) folders.Add(currentFolder);

                        Directory.CreateDirectory(Path.GetDirectoryName(currentFolder));
                        var file = currentFolder + "template_ligand_after_substitution.ali";
                        var seqModPir = Pir(new List<string>() { template.RecSequence, ligSeqMod });
                        File.WriteAllText(file, seqModPir);

                        Debug.WriteLine("");
                    }

                    /*    
                    continue;

                    var firstBound = -1;
                    var lastBound = -1;
                    var resolution = -1;
                    var substitutionDescription = "";

                    var interfaceLengthDifference = template.LigInfSequence.Length - sibling.LigInfSequence.Length; // positive=replacement is shorter, negative=replacement is longer, zero=the same

                    const int flank = 10;
                    firstBound = sibling.LigInfPosInLigSeq - flank;
                    lastBound = (firstBound + sibling.LigInfSequence.Length + (flank * 2)) - 1;

                    resolution = 1;
                    substitutionDescription = "inf";


                    if (firstBound < 0) firstBound = 0;
                    if (lastBound > sibling.LigSequence.Length - 1) lastBound = sibling.LigSequence.Length - 1;

                    var totalPossibleInterfaceOverlapPositions = 1;


                    if (interfaceLengthDifference < 0)
                    {
                        totalPossibleInterfaceOverlapPositions = Math.Abs(interfaceLengthDifference) + 1;
                    }
                    


                    var lastPossibleSubPos = (lastBound - (interfaceLengthDifference > 0 ? sibling.LigInfSequence.Length : template.LigInfSequence.Length)) + 1;

                    for (var substitutionSourcePos = firstBound; substitutionSourcePos <= lastBound && substitutionSourcePos <= lastPossibleSubPos; substitutionSourcePos += resolution)
                    {
                        for (var interfaceOverlapPosition = 0; interfaceOverlapPosition < totalPossibleInterfaceOverlapPositions; interfaceOverlapPosition++)
                        {
                            var replacementInterfaceSubsequence = sibling.LigSequence.Substring(substitutionSourcePos, sibling.LigInfSequence.Length);

                            if (sibling.LigInfSequence.Length > template.LigInfSequence.Length)
                            {
                                replacementInterfaceSubsequence = replacementInterfaceSubsequence.Substring(interfaceOverlapPosition, template.LigInfSequence.Length);
                            }

                            var ligSeqModified = template.LigSequence.Remove(template.LigInfPosInLigSeq, replacementInterfaceSubsequence.Length).Insert(template.LigInfPosInLigSeq, replacementInterfaceSubsequence);

                            if (ligSeqModified.Length != template.LigSequence.Length)
                            {
                                throw new Exception("Wrong sub pos or len");
                            }

                            var native = (substitutionSourcePos >= sibling.LigInfPosInLigSeq && substitutionSourcePos + replacementInterfaceSubsequence.Length <= sibling.LigInfPosInLigSeq + sibling.LigInfSequence.Length);
                            if (native)
                            {
                                Console.WriteLine("");
                            }
                            currentFolder = rootFolder + template.PdbId + template.RecChainId + template.LigChainId + @"\" + sibling.PdbId + sibling.RecChainId + sibling.LigChainId + @"\" + substitutionDescription + "_" + (substitutionSourcePos + 1).ToString().PadLeft(4, '0') + "_" + (substitutionSourcePos + replacementInterfaceSubsequence.Length).ToString().PadLeft(4, '0') + "_" + (interfaceOverlapPosition + 1) + "_" + totalPossibleInterfaceOverlapPositions + (native ? "_native" : "") + @"\";
                            Directory.CreateDirectory(Path.GetDirectoryName(currentFolder));
                            //rootFolderSubDirs.Remove(Path.GetDirectoryName(currentFolder));

                            var file = currentFolder + "template_ligand_after_substitution.ali";
                            var seqModPir = Pir(new List<string>() { template.RecSequence, ligSeqModified });
                            File.WriteAllText(file, seqModPir);
                        }
                    }
                    */
                }
            }

            //File.WriteAllLines(rootFolder + "obsolete_dirs.txt", rootFolderSubDirs);

            if (folders.Count > 0)
            {
                var scripts = new List<string> { "modeller_monomer.bat", "foldx_dimer.bat" };//, "pisa_dimer.bat" };

                
                int div = folders.Count / Environment.ProcessorCount;

                
                var batch = new List<string>();
                var c = 0;
                while (folders.Count > 0)
                {
                    c++;
                    batch.Add(@"@echo off");
                    batch.Add(@"set HDF5_DISABLE_VERSION_CHECK=2");
                    batch.Add(@"set THIS_DIR=%cd%");
                    batch.Add(@"set PATH=%PATH%;c:\modeller_scripts;");

                    var t = folders.Count >= div * 2 ? div : folders.Count;

                    folders.Take(t).ToList().ForEach(a =>
                    {
                        batch.Add(@"echo " + a);
                        batch.Add(@"cd " + a);
                        batch.Add(@"CALL %script1%");
                        batch.Add(@"CALL %script2%");
                    });
                    folders = folders.Skip(t).ToList();

                    batch.Add(@"pause");

                    var d = batch/*.Select(a => a.Replace("%script%", script))*/.ToList();
                    var n = "";
                    for (var i = 0; i < scripts.Count; i++)
                    {
                        var script = scripts[i];
                        d = d.Select(a => a.Replace("%script" + (i + 1).ToString() + "%", script)).ToList();
                        n = n + Path.GetFileNameWithoutExtension(script) + "_";
                    }
                    File.WriteAllLines(rootFolder + @"r_" + c + "_" + n + DateTime.Now.Ticks + @".bat", d);

                    batch.Clear();
                }
            }
        }
    }
}
