using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ProteinBioinformaticsSharedLibrary;
using static ComplexInterfaces.ComplexInterfaces;

namespace BatchSubstitutePartnerInterfaces
{
    public class BatchSubstitutePartnerInterfaces
    {
        public static string blosum62sub(string sequence, bool similar)
        {
            var matrixData = @"
   A  R  N  D  C  Q  E  G  H  I  L  K  M  F  P  S  T  W  Y  V  B  Z  X  *
A  4 -1 -2 -2  0 -1 -1  0 -2 -1 -1 -1 -1 -2 -1  1  0 -3 -2  0 -2 -1  0 -4 
R -1  5  0 -2 -3  1  0 -2  0 -3 -2  2 -1 -3 -2 -1 -1 -3 -2 -3 -1  0 -1 -4 
N -2  0  6  1 -3  0  0  0  1 -3 -3  0 -2 -3 -2  1  0 -4 -2 -3  3  0 -1 -4 
D -2 -2  1  6 -3  0  2 -1 -1 -3 -4 -1 -3 -3 -1  0 -1 -4 -3 -3  4  1 -1 -4 
C  0 -3 -3 -3  9 -3 -4 -3 -3 -1 -1 -3 -1 -2 -3 -1 -1 -2 -2 -1 -3 -3 -2 -4 
Q -1  1  0  0 -3  5  2 -2  0 -3 -2  1  0 -3 -1  0 -1 -2 -1 -2  0  3 -1 -4 
E -1  0  0  2 -4  2  5 -2  0 -3 -3  1 -2 -3 -1  0 -1 -3 -2 -2  1  4 -1 -4 
G  0 -2  0 -1 -3 -2 -2  6 -2 -4 -4 -2 -3 -3 -2  0 -2 -2 -3 -3 -1 -2 -1 -4 
H -2  0  1 -1 -3  0  0 -2  8 -3 -3 -1 -2 -1 -2 -1 -2 -2  2 -3  0  0 -1 -4 
I -1 -3 -3 -3 -1 -3 -3 -4 -3  4  2 -3  1  0 -3 -2 -1 -3 -1  3 -3 -3 -1 -4 
L -1 -2 -3 -4 -1 -2 -3 -4 -3  2  4 -2  2  0 -3 -2 -1 -2 -1  1 -4 -3 -1 -4 
K -1  2  0 -1 -3  1  1 -2 -1 -3 -2  5 -1 -3 -1  0 -1 -3 -2 -2  0  1 -1 -4 
M -1 -1 -2 -3 -1  0 -2 -3 -2  1  2 -1  5  0 -2 -1 -1 -1 -1  1 -3 -1 -1 -4 
F -2 -3 -3 -3 -2 -3 -3 -3 -1  0  0 -3  0  6 -4 -2 -2  1  3 -1 -3 -3 -1 -4 
P -1 -2 -2 -1 -3 -1 -1 -2 -2 -3 -3 -1 -2 -4  7 -1 -1 -4 -3 -2 -2 -1 -2 -4 
S  1 -1  1  0 -1  0  0  0 -1 -2 -2  0 -1 -2 -1  4  1 -3 -2 -2  0  0  0 -4 
T  0 -1  0 -1 -1 -1 -1 -2 -2 -1 -1 -1 -1 -2 -1  1  5 -2 -2  0 -1 -1  0 -4 
W -3 -3 -4 -4 -2 -2 -3 -2 -2 -3 -2 -3 -1  1 -4 -3 -2 11  2 -3 -4 -3 -2 -4 
Y -2 -2 -2 -3 -2 -1 -2 -3  2 -1 -1 -2 -1  3 -3 -2 -2  2  7 -1 -3 -2 -1 -4 
V  0 -3 -3 -3 -1 -2 -2 -3 -3  3  1 -2  1 -1 -2 -2  0 -3 -1  4 -3 -2 -1 -4 
B -2 -1  3  4 -3  0  1 -1  0 -3 -4  0 -3 -3 -2  0 -1 -4 -3 -3  4  1 -1 -4 
Z -1  0  0  1 -3  3  4 -2  0 -3 -3  1 -1 -3 -1  0 -1 -3 -2 -2  1  4 -1 -4 
X  0 -1 -1 -1 -2 -1 -1 -1 -1 -1 -1 -1 -1 -1 -2  0  0 -2 -1 -1 -1 -1 -1 -4 
* -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4 -4  1 ";

            var matrixLines = matrixData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var matrixColHeader = matrixLines.First().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var matrixRowHeader = matrixLines.Skip(1).Select(a => a.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).First()).ToList();

            var matrix = new List<Tuple<char, char, int>>();

            foreach (var line in matrixLines.Skip(1))
            {
                var split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var aaRow = split[0];

                for (int index = 1; index < split.Length; index++)
                {
                    var s = split[index];
                    var aaCol = matrixColHeader[index - 1];

                    matrix.Add(new Tuple<char, char, int>(aaCol[0], aaRow[0], int.Parse(s)));
                }
            }

            var result = "";

            foreach (var aa in sequence)
            {
                if (!matrixColHeader.Contains("" + aa) || !matrixRowHeader.Contains("" + aa))
                {
                    result += aa;
                    continue;
                }

                //var col = matrixColHeader.FindIndex(a=>a == "" + aa);

                var m = matrix.Where(a => a.Item1 == aa && a.Item2 != 'X' && a.Item2 != '*' && a.Item2 != aa).ToList();

                if (similar)
                {
                    var max = m.Max(a => a.Item3);

                    var item = m.First(a => a.Item3 == max);

                    result += item.Item2;
                }
                else //dissimilar
                {
                    var min = m.Min(a => a.Item3);

                    var item = m.First(a => a.Item3 == min);

                    result += item.Item2;
                }
            }

            return result;
        }

        public static PhysicochemicalPropertyMutationResult PhysicochemicalPropertyMutation(string sequence)
        {
            var result = new PhysicochemicalPropertyMutationResult();

            var aminoAcids = "ARNDCEQGHILKMFPSTWYVUO";
            var SimilarPhysiochemical = "GKQEADNAKLIRIWNNNYWICK";
            var SimilarPropensity = "IEQNKDHFWGVCPTRYSCMACK";
            var DissimilarPhysiochemical = "HVHFHVVHSSSSSSHHHSSHHH";
            var DissimilarPropensity = "KKKKKKKKKKKKKKKKKKKKKK";

            foreach (var aa in sequence)
            {
                if (!aminoAcids.Contains(aa))
                {
                    result.SimilarPhysiochemical += aa;
                    result.SimilarPropensity += aa;
                    result.DissimilarPhysiochemical += aa;
                    result.DissimilarPropensity += aa;
                    continue;
                }

                var matrixIndex = aminoAcids.IndexOf(aa);

                result.SimilarPhysiochemical += SimilarPhysiochemical[matrixIndex];
                result.SimilarPropensity += SimilarPropensity[matrixIndex];
                result.DissimilarPhysiochemical += DissimilarPhysiochemical[matrixIndex];
                result.DissimilarPropensity += DissimilarPropensity[matrixIndex];
            }

            result.SimilarBlosum62 = blosum62sub(sequence, true);
            result.DissimilarBlosum62 = blosum62sub(sequence, false);
            return result;
        }

        public static int AlignInside(string a, string b)
        {
            var longer = a.Length > b.Length ? a : b;
            var shorter = longer == a ? b : a;
            var bestScore = 0;
            var bestOffset = 0;
            var bestDistCentre = 0;

            if (a.Length == b.Length) { return 0; }

            var longerCentre = longer.Length / 2;

            for (var i = 0; i <= longer.Length - shorter.Length; i++)
            {
                var scoreForIndex = 0;

                var distCentre = Math.Abs(longerCentre - i);

                for (var j = 0; j < shorter.Length; j++)
                {
                    if (longer[i + j] == shorter[j]) scoreForIndex++;
                }
                if (scoreForIndex > bestScore || (scoreForIndex == bestScore && distCentre < bestDistCentre))
                {
                    bestScore = scoreForIndex;
                    bestOffset = i;
                    bestDistCentre = distCentre;
                }
            }

            var scorePct = longer.Length > 0 ? bestScore / (decimal)longer.Length : 0;

            // if score is too low, centre the shorter string in the longer string
            if (scorePct <= 0.2m) { bestOffset = (longer.Length / 2) - (shorter.Length / 2); }


            return bestOffset;
        }

        public static string InterfaceSubsequence(string sequence, int start, int end, int length)
        {
            // returns a subsequence of sequence, from the centre of start and end, of given length, assumes numbers are 1-based
            // start and end are taken from residue sequence indexes in the pdb structure files
            // length may be longer than ((end-start)+1) for getting the flanking residues around the interface for substitution with mismatching sized interfaces (can be larger or smaller)
            string result;

            start--;
            end--;

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

            result = sequence.Substring(realstart, reallength); //Math.Abs(realend - realstart) + 1);

            return result;
        }

        public static void Main(string[] args)
        {
            var resultsFolder = @"C:\r\";
            Directory.CreateDirectory(resultsFolder);

            //var clustersCsvFile = @"C:\CategoryM\catm_combined_data.csv";

            //var atomsCaTraceFolder = @"C:\dbmk\atoms_ca\";
            //var atomsBackBoneFolder = @"C:\dbmk\atoms_mc\";
            //var atomsSideChainFolder = @"C:\dbmk\atoms_sc\";
            var atomsFullFolder = @"C:\dbmk\atoms_all\";

            const int atom_chain = 21;
            const int atom_chain_len = 1;

            const int atom_icode = 26;
            const int atom_icode_len = 1;

            const int atom_type = 14;
            const int atom_type_len = 3;

            const int atom_resseq = 22;
            const int atom_resseq_len = 4;

            /*
            var data = InterfaceInterfaceData.Load(clustersCsvFile);

            // interfaces with length at least five
            data = data.Where(a => a.ReceptorInterfaceResSeqLength >= 5 && a.LigandInterfaceResSeqLength >= 5).ToList();

            data = data.Where(a => char.IsLetterOrDigit(a.ReceptorChainId) && char.IsLetterOrDigit(a.LigandChainId)).ToList();

            // ensure pdbids are uppercase
            foreach (var d in data)
            {
                d.ReceptorPdbId = d.ReceptorPdbId.ToUpperInvariant();
                d.LigandPdbId = d.LigandPdbId.ToUpperInvariant();
            }
            */
            var scripts = new List<string>();
            //scripts.AddRange(Directory.GetFiles(@"c:\modeller_scripts\", "*.py*", SearchOption.AllDirectories));
            //scripts.AddRange(Directory.GetFiles(@"c:\modeller_scripts\", "*.bat", SearchOption.AllDirectories));
            //scripts.AddRange(Directory.GetFiles(@"c:\modeller_scripts\", "*.html*", SearchOption.AllDirectories));



            var dbmkList = File.ReadAllLines(@"c:\dbmk\dbmk-1to1-list.txt").ToList();
            dbmkList = dbmkList.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
                

            var scriptsText = scripts.Select(a => new Tuple<string, string>(a, File.ReadAllText(a))).ToList();
            /*
            var dataGroupedByReceptorClusterIndex = data.GroupBy(a => a.ReceptorConsensusClusterIndex).Select(a => a.ToList()).ToList();
            var dataGroupedByReceptorClusterIndexAndInterfaceOverlap = new List<List<InterfaceInterfaceData>>();

            foreach (var l in dataGroupedByReceptorClusterIndex)
            {
                var overlapClusters = l.Select(a => new List<InterfaceInterfaceData> { a }).ToList();

                foreach (var a in l)
                {
                    foreach (var b in l)
                    {
                        if (a == b) continue;

                        var seqLenDiff = Math.Abs(Sequence.TrimSequence(a.ReceptorSequenceFromSequenceFile).Length - Sequence.TrimSequence(b.ReceptorSequenceFromSequenceFile).Length);


                        var maxMultiBindDist = -seqLenDiff;
                        maxMultiBindDist--;

                        var o = ProteinBioClass.InterfaceOverlap(a.ReceptorInterfaceResSeqStart, a.ReceptorInterfaceResSeqEnd, b.ReceptorInterfaceResSeqStart, b.ReceptorInterfaceResSeqEnd);

                        if (o >= maxMultiBindDist || a.ReceptorInterfaceSequenceClusterIndex == b.ReceptorInterfaceSequenceClusterIndex)
                        {
                            var aCluster = overlapClusters.First(c => c.Contains(a));
                            var bCluster = overlapClusters.First(c => c.Contains(b));
                            if (aCluster == bCluster) continue;
                            aCluster.AddRange(bCluster);
                            overlapClusters.Remove(bCluster);
                        }
                    }
                }

                dataGroupedByReceptorClusterIndexAndInterfaceOverlap.AddRange(overlapClusters);
            }

            // make sure cluster has more than 1 pdb id
            dataGroupedByReceptorClusterIndexAndInterfaceOverlap = dataGroupedByReceptorClusterIndexAndInterfaceOverlap.Where(a => a.Select(b => b.ReceptorPdbId).Distinct().Count() > 1).ToList();
            */

            var structureOutputTypes = new List<string>() { "all" };
            var sequenceOutputTypes = new List<string> { "pir" };
            //var calculateReversed = true;

            var list = new List<string>();

            foreach (var dbmkProtein in dbmkList)
            {
                var pdbId = dbmkProtein.Substring(0, 4);
                var pdbChains = dbmkProtein.Substring(4);

                var pdbInterfaceInterfaceData = InterfaceInterfaceData.Load(@"c:\dbmk\interfaces\interface-interface_" + pdbId + ".pdb.csv");

                pdbInterfaceInterfaceData = pdbInterfaceInterfaceData.Where(a => !string.IsNullOrWhiteSpace(a.ReceptorPdbId) && !string.IsNullOrWhiteSpace(a.LigandPdbId)).ToList();
                pdbInterfaceInterfaceData = pdbInterfaceInterfaceData.Where(a => pdbChains.Contains(a.ReceptorChainId) && pdbChains.Contains(a.LigandChainId)).ToList();
                pdbInterfaceInterfaceData = pdbInterfaceInterfaceData.Where(a => ((double) a.LigandInterfaceAllAminoAcidsSuper.Length / (double) a.LigandSequenceSuper.Length) < 0.5).ToList();

                
                for (int index = 0; index < pdbInterfaceInterfaceData.Count; index++)
                {
                    var receptorLigandDimerA = pdbInterfaceInterfaceData[index];
                    var outputFolder = @"c:\r\" + receptorLigandDimerA.LigandPdbId + "_" + receptorLigandDimerA.LigandChainId + "_" + (index+1) + "_" + receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length + "_" + receptorLigandDimerA.LigandSequenceSuper.Length + "_" + receptorLigandDimerA.LigandInterfaceSuperStart + "_" + receptorLigandDimerA.LigandInterfaceSuperEnd + @"\";

                    //list.Add(outputFolder);
                    //continue;

                    var substitutionList = new List<DonatedSubstitutionInfo>();

                    var receptorLigandDimerChainOrderA = new List<char> {receptorLigandDimerA.ReceptorChainId, receptorLigandDimerA.LigandChainId};


                    for (var interfacePositionIndex = 0; interfacePositionIndex <= receptorLigandDimerA.LigandSequenceSuper.Length - receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length; interfacePositionIndex++)
                    {
                        var subInterface = receptorLigandDimerA.LigandSequenceSuper.Substring(interfacePositionIndex, receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length);
                        var description = "Interface Sliding Window " + interfacePositionIndex + " " + (interfacePositionIndex + subInterface.Length);
                        substitutionList.Add(new DonatedSubstitutionInfo(description, description.Replace(" ", "_"), subInterface, subInterface, receptorLigandDimerA));
                    }


                    var receptorLigandFilename = @"" + receptorLigandDimerA.LigandPdbId + ".pdb";

                    var acceptorTemplateAllAtomPdbFileData = !structureOutputTypes.Contains("all") ? null : File.ReadAllLines(atomsFullFolder + receptorLigandFilename).Where(a => a[atom_icode] == ' ' && ((a[atom_chain] == receptorLigandDimerA.ReceptorChainId) || (a[atom_chain] == receptorLigandDimerA.LigandChainId))).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToArray();

                    // monomer
                    //var monomerAcceptorTemplateReceptorAllAtomPdbFile = !structureOutputTypes.Contains("all") ? null : acceptorTemplateAllAtomPdbFileData.Where(a => a[atom_icode] == ' ' && a[atom_chain] == receptorLigandDimerA.ReceptorChainId).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToList();

                    var monomerAcceptorTemplateLigandAllAtomPdbFile = !structureOutputTypes.Contains("all") ? null : acceptorTemplateAllAtomPdbFileData.Where(a => a[atom_icode] == ' ' && a[atom_chain] == receptorLigandDimerA.LigandChainId).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToList();


                    // dimer
                    //var dimerAcceptorTemplateDimerAllAtomPdbFile = !structureOutputTypes.Contains("all") ? null : acceptorTemplateAllAtomPdbFileData.Where(a => a[atom_icode] == ' ' && (a[atom_chain] == receptorLigandDimerA.ReceptorChainId || a[atom_chain] == receptorLigandDimerA.LigandChainId)).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToList();

                    var receptorHeader = ">" + receptorLigandDimerA.ReceptorPdbId + ":" + receptorLigandDimerA.ReceptorChainId;
                    var ligandHeader = ">" + receptorLigandDimerA.LigandPdbId + ":" + receptorLigandDimerA.LigandChainId;

                    //var monomerAcceptorTemplateLigandSequenceSuper = new Sequence(ligandHeader, receptorLigandDimerA.LigandSequenceSuper);

                    var dimerAcceptorTemplateDimerSequenceSuper = new List<Sequence> {new Sequence(receptorHeader, receptorLigandDimerA.ReceptorSequenceSuper), new Sequence(ligandHeader, receptorLigandDimerA.LigandSequenceSuper)};


                    /*
                    var dimerAcceptorTemplateDimerSequenceSuperLocked = new List<Sequence>
                                                                  {
                                                                      new Sequence(receptorHeader, new string('-',receptorLigandDimerA.ReceptorSequenceSuper.Length)),
                                                                      new Sequence(ligandHeader, new string('-',receptorLigandDimerA.LigandSequenceSuper.Length))
                                                                  };
                                                                  */

                    foreach (var sub in substitutionList)
                    {
                        // align ligand interface sequence and substitution sequence
                        var offset = AlignInside(receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper, sub.InterfacePadded);

                        if (receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length < sub.InterfacePadded.Length) { offset = -offset; }

                        var acceptorTemplateSubstitutionStart = receptorLigandDimerA.LigandInterfaceSuperStart + offset;
                        //var acceptorTemplateSubstitutionEnd = (acceptorTemplateSubstitutionStart + sub.InterfacePadded.Length) - 1;

                        var info = string.Join(Environment.NewLine, new string[] {"" + receptorLigandDimerA.ReceptorSequenceClusterIndex, "" + receptorLigandDimerA.ReceptorInterfaceSequenceClusterIndex, "" + receptorLigandDimerA.LigandSequenceClusterIndex, "" + receptorLigandDimerA.LigandInterfaceSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorInterfaceSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandInterfaceSequenceClusterIndex, receptorLigandDimerA.ReceptorPdbId, "" + receptorLigandDimerA.ReceptorChainId, receptorLigandDimerA.LigandPdbId, "" + receptorLigandDimerA.LigandChainId, sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorPdbId, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorChainId, sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandPdbId, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandChainId, sub.Description, receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper, sub.Interface, sub.InterfacePadded});


                        //var monomerAcceptorTemplateFragmentLigandCaTracePdbFile = !structureOutputTypes.Contains("ca") ? null : monomerAcceptorTemplateLigandCaTracePdbFile.Where(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd).ToList();
                        //var monomerAcceptorTemplateFragmentLigandBackBonePdbFile = !structureOutputTypes.Contains("mc") ? null : monomerAcceptorTemplateLigandBackBonePdbFile.Where(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd).ToList();
                        //var monomerAcceptorTemplateFragmentLigandSideChainPdbFile = !structureOutputTypes.Contains("sc") ? null : monomerAcceptorTemplateLigandSideChainPdbFile.Where(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd).ToList();
                        //var monomerAcceptorTemplateFragmentLigandAllAtomPdbFile = !structureOutputTypes.Contains("all") ? null : monomerAcceptorTemplateLigandAllAtomPdbFile.Where(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd).ToList();

                        //var dimerAcceptorTemplateFragmentDimerCaTracePdbFile = !structureOutputTypes.Contains("ca") ? null : dimerAcceptorTemplateDimerCaTracePdbFile.Where(a => (a[atom_chain] == receptorLigandDimerA.ReceptorChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= receptorLigandDimerA.ReceptorInterfaceResSeqStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= receptorLigandDimerA.ReceptorInterfaceResSeqEnd) || (a[atom_chain] == receptorLigandDimerA.LigandChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd)).ToList();
                        //var dimerAcceptorTemplateFragmentDimerBackBonePdbFile = !structureOutputTypes.Contains("mc") ? null : dimerAcceptorTemplateDimerBackBonePdbFile.Where(a => (a[atom_chain] == receptorLigandDimerA.ReceptorChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= receptorLigandDimerA.ReceptorInterfaceResSeqStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= receptorLigandDimerA.ReceptorInterfaceResSeqEnd) || (a[atom_chain] == receptorLigandDimerA.LigandChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd)).ToList();
                        //var dimerAcceptorTemplateFragmentDimerSideChainPdbFile = !structureOutputTypes.Contains("sc") ? null : dimerAcceptorTemplateDimerSideChainPdbFile.Where(a => (a[atom_chain] == receptorLigandDimerA.ReceptorChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= receptorLigandDimerA.ReceptorInterfaceResSeqStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= receptorLigandDimerA.ReceptorInterfaceResSeqEnd) || (a[atom_chain] == receptorLigandDimerA.LigandChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd)).ToList();
                        //var dimerAcceptorTemplateFragmentDimerAllAtomPdbFile = !structureOutputTypes.Contains("all") ? null : dimerAcceptorTemplateDimerAllAtomPdbFile.Where(a => (a[atom_chain] == receptorLigandDimerA.ReceptorChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= receptorLigandDimerA.ReceptorInterfaceResSeqStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= receptorLigandDimerA.ReceptorInterfaceResSeqEnd) || (a[atom_chain] == receptorLigandDimerA.LigandChainId && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) >= acceptorTemplateSubstitutionStart && int.Parse(a.Substring(atom_resseq, atom_resseq_len)) <= acceptorTemplateSubstitutionEnd)).ToList();

                        var modelOutputFolder = outputFolder + sub.Folder + @"\";
                        Directory.CreateDirectory(modelOutputFolder);
                        //scriptsText.ForEach(a => File.WriteAllText(modelOutputFolder + Path.GetFileName(a.Item1), a.Item2.Replace("%%chain%%", "" + receptorLigandDimerA.LigandChainId)));

                        File.WriteAllText(modelOutputFolder + "substitution.log", info);


                        if (structureOutputTypes.Contains("all")) File.WriteAllLines(modelOutputFolder + "template_ligand_all.pdb", monomerAcceptorTemplateLigandAllAtomPdbFile);

                        //if (structureOutputTypes.Contains("all")) File.WriteAllLines(modelOutputFolder + "template_dimer_all.pdb", dimerAcceptorTemplateDimerAllAtomPdbFile);


                        //if (structureOutputTypes.Contains("all")) File.WriteAllLines(modelOutputFolder + "template_locked_ligand_all.pdb", monomerAcceptorTemplateLigandAllAtomPdbFile);

                        //if (structureOutputTypes.Contains("all")) File.WriteAllLines(modelOutputFolder + "template_locked_dimer_all.pdb", dimerAcceptorTemplateDimerAllAtomPdbFile);


                        //if (structureOutputTypes.Contains("all")) File.WriteAllLines(modelOutputFolder + "template_fragment_ligand_all.pdb", monomerAcceptorTemplateFragmentLigandAllAtomPdbFile);

                        //if (structureOutputTypes.Contains("all")) File.WriteAllLines(modelOutputFolder + "template_fragment_dimer_all.pdb", dimerAcceptorTemplateFragmentDimerAllAtomPdbFile);

                        /*
                        var mutatedLigandSequenceLocked =
                            new Sequence(ligandHeader,

                            new string('-', receptorLigandDimerA.LigandSequenceSuper.Length)
                            .Remove(acceptorTemplateSubstitutionStart, sub.InterfacePadded.Length)
                            .Insert(acceptorTemplateSubstitutionStart, sub.InterfacePadded));
                            */

                         var mutatedLigandSequenceSubLigand = new Sequence(ligandHeader, receptorLigandDimerA.LigandSequenceSuper.Remove(acceptorTemplateSubstitutionStart, sub.InterfacePadded.Length).Insert(acceptorTemplateSubstitutionStart, sub.InterfacePadded));


                         var dimerSequenceAfterSubstitution = dimerAcceptorTemplateDimerSequenceSuper.Select(a => a.Id == ligandHeader ? mutatedLigandSequenceSubLigand : a).ToList();
                        //var dimerSequenceAfterSubstitutionLocked = dimerAcceptorTemplateDimerSequenceSuperLocked.Select(a => a.Id == ligandHeader ? mutatedLigandSequenceLocked : a).ToList();

                        /*
                        var mutatedMonomerAcceptorTemplateFragmentLigandSequenceSuper = new List<Sequence>
                                                                  {
                                                                      new Sequence(ligandHeader, sub.InterfacePadded)
                                                                  };
                                                                  */
                        /*
                        var mutatedDimerAcceptorTemplateFragmentDimerSequenceSuper = new List<Sequence>
                                                                  {
                                                                      new Sequence(receptorHeader, receptorLigandDimerA.ReceptorInterfaceAllAminoAcidsSuper),
                                                                      new Sequence(ligandHeader, sub.InterfacePadded)
                                                                  };
                                                                  */

                        // save before
                        //if (sequenceOutputTypes.Contains("pir"))   File.WriteAllText(modelOutputFolder + "template_ligand_before_substitution.ali", Sequence.GetFormattedSequence(monomerAcceptorTemplateLigandSequenceSuper, Sequence.SequenceFormat.Pir));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_ligand_before_substitution.fasta", Sequence.GetFormattedSequence(monomerAcceptorTemplateLigandSequenceSuper, Sequence.SequenceFormat.Fasta));
                         if (sequenceOutputTypes.Contains("pir")) File.WriteAllText(modelOutputFolder + "template_ligand_after_substitution.ali", Sequence.GetFormattedSequence(mutatedLigandSequenceSubLigand, Sequence.SequenceFormat.Pir));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_ligand_after_substitution.fasta", Sequence.GetFormattedSequence(mutatedLigandSequenceSubLigand, Sequence.SequenceFormat.Fasta));

                        //if (sequenceOutputTypes.Contains("pir")) File.WriteAllText(modelOutputFolder +   "template_dimer_before_substitution.ali", Sequence.GetFormattedSequence(dimerAcceptorTemplateDimerSequenceSuper, Sequence.SequenceFormat.Pir, ligandHeader));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_dimer_before_substitution.fasta", Sequence.GetFormattedSequence(dimerAcceptorTemplateDimerSequenceSuper, Sequence.SequenceFormat.Fasta, ligandHeader));
                        //if (sequenceOutputTypes.Contains("pir")) File.WriteAllText(modelOutputFolder +   "template_dimer_after_substitution.ali", Sequence.GetFormattedSequence(dimerSequenceAfterSubstitution, Sequence.SequenceFormat.Pir, ligandHeader));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_dimer_after_substitution.fasta", Sequence.GetFormattedSequence(dimerSequenceAfterSubstitution, Sequence.SequenceFormat.Fasta));

                        //if (sequenceOutputTypes.Contains("pir"))   File.WriteAllText(modelOutputFolder + "template_fragment_ligand_after_substitution.ali", Sequence.GetFormattedSequence(mutatedMonomerAcceptorTemplateFragmentLigandSequenceSuper, Sequence.SequenceFormat.Pir, ligandHeader));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_fragment_ligand_after_substitution.fasta", Sequence.GetFormattedSequence(mutatedMonomerAcceptorTemplateFragmentLigandSequenceSuper, Sequence.SequenceFormat.Fasta));
                        //if (sequenceOutputTypes.Contains("pir"))   File.WriteAllText(modelOutputFolder + "template_fragment_dimer_after_substitution.ali", Sequence.GetFormattedSequence(mutatedDimerAcceptorTemplateFragmentDimerSequenceSuper, Sequence.SequenceFormat.Pir, ligandHeader));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_fragment_dimer_after_substitution.fasta", Sequence.GetFormattedSequence(mutatedDimerAcceptorTemplateFragmentDimerSequenceSuper, Sequence.SequenceFormat.Fasta));

                        //if (sequenceOutputTypes.Contains("pir"))   File.WriteAllText(modelOutputFolder + "template_locked_ligand_after_substitution.ali", Sequence.GetFormattedSequence(mutatedLigandSequenceLocked, Sequence.SequenceFormat.Pir));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_locked_ligand_after_substitution.fasta", Sequence.GetFormattedSequence(mutatedLigandSequenceLocked, Sequence.SequenceFormat.Fasta));
                        //if (sequenceOutputTypes.Contains("pir"))   File.WriteAllText(modelOutputFolder + "template_locked_dimer_after_substitution.ali", Sequence.GetFormattedSequence(dimerSequenceAfterSubstitutionLocked, Sequence.SequenceFormat.Pir, ligandHeader));
                        //if (sequenceOutputTypes.Contains("fasta")) File.WriteAllText(modelOutputFolder + "template_locked_dimer_after_substitution.fasta", Sequence.GetFormattedSequence(dimerSequenceAfterSubstitutionLocked, Sequence.SequenceFormat.Fasta));
                    }
                }
                
            }
            //File.WriteAllLines(@"c:\r\list.csv", list);
            /*
            var batches = new Dictionary<string, string[]>
                          {
                //{ resultsFolder + @"call_fs_modeller_monomer_full.bat", Directory.GetFiles(resultsFolder, @"modeller_monomer_full.bat", SearchOption.AllDirectories).Where(a => Path.GetDirectoryName(a).Contains("Full Structure")).ToArray()},
                //{ resultsFolder + @"call_fs_modeller_dimer_full.bat", Directory.GetFiles(resultsFolder, @"modeller_dimer_full.bat", SearchOption.AllDirectories).Where(a => Path.GetDirectoryName(a).Contains("Full Structure")).ToArray()},
                { resultsFolder + @"call_fs_modeller_monomer_and_dimer_full.bat", Directory.GetFiles(resultsFolder, @"modeller_monomer_and_dimer_full.bat", SearchOption.AllDirectories).Where(a => Path.GetDirectoryName(a).Contains("Full Structure")).ToArray()},
                { resultsFolder + @"call_fs_pymol_images.bat", Directory.GetFiles(resultsFolder, @"pymol_images.bat", SearchOption.AllDirectories).Where(a => Path.GetDirectoryName(a).Contains("Full Structure")).ToArray()}
                          };

            foreach (var batch in batches)
            {
                var outputCmds = new List<string>();

                outputCmds.Add(@"echo Running """ + Path.GetFileName(batch.Key) + @"""");

                for (var index = 0; index < batch.Value.Length; index++)
                {
                    var file = batch.Value[index];
                    var pct = (index + 1) / (decimal)batch.Value.Length;

                    outputCmds.Add(@"echo Processing """ + Path.GetDirectoryName(file) + @""" " + (index + 1) + "/" + batch.Value.Length + " (" + Math.Round(pct * 100, 2) + "%)");
                    outputCmds.Add(@"cd """ + Path.GetDirectoryName(file) + @"""");
                    outputCmds.Add(@"echo Calling """ + Path.GetFileName(file) + @"""");
                    outputCmds.Add(@"call """ + file + @"""");
                    outputCmds.Add(@"");
                }
                outputCmds.Add(@"pause");
                outputCmds.Add(@"");

                File.WriteAllLines(batch.Key, outputCmds);
            }
            */
        }

        public class PhysicochemicalPropertyMutationResult
        {
            public string DissimilarPhysiochemical;
            public string DissimilarPropensity;
            public string DissimilarBlosum62;
            public string SimilarPhysiochemical;
            public string SimilarPropensity;
            public string SimilarBlosum62;
        }

        public class DonatedSubstitutionInfo
        {
            public string Description;
            public string Folder;

            public string Interface;

            public InterfaceInterfaceData InterfaceInterfaceSequenceIdentityClusteringItem;
            public string InterfacePadded;

            public DonatedSubstitutionInfo(string description, string folder, string @interface, string interfacePadded, InterfaceInterfaceData interfaceInterfaceSequenceIdentityClusteringItem)
            {
                Description = description;
                Folder = folder;
                Interface = @interface;
                InterfacePadded = interfacePadded;
                InterfaceInterfaceSequenceIdentityClusteringItem = interfaceInterfaceSequenceIdentityClusteringItem;
            }
        }

    }
}
