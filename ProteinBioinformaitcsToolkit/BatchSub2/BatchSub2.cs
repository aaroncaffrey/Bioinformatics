using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ProteinBioinformaticsSharedLibrary;
using static ComplexInterfaces.ComplexInterfaces;

namespace BatchSub2
{
    public class BatchSub2
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

            var atomsFullFolder = @"C:\dbmk\atoms_all\";

            const int atom_chain = 21;
            const int atom_chain_len = 1;

            const int atom_icode = 26;
            const int atom_icode_len = 1;

            const int atom_type = 14;
            const int atom_type_len = 3;

            const int atom_resseq = 22;
            const int atom_resseq_len = 4;

            var dbmkList = File.ReadAllLines(@"c:\dbmk\dbmk-1to1-list.txt").ToList();
            dbmkList = dbmkList.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

            var structureOutputTypes = new List<string>() { "all" };
            var sequenceOutputTypes = new List<string> { "pir" };

            var list = new List<string>();

            foreach (var dbmkProtein in dbmkList)
            {
                var pdbId = dbmkProtein.Substring(0, 4);
                var pdbChains = dbmkProtein.Substring(4);

                var pdbInterfaceInterfaceData = InterfaceInterfaceData.Load(@"c:\dbmk\interfaces\interface-interface_" + pdbId + ".pdb.csv");

                pdbInterfaceInterfaceData = pdbInterfaceInterfaceData.Where(a => !string.IsNullOrWhiteSpace(a.ReceptorPdbId) && !string.IsNullOrWhiteSpace(a.LigandPdbId)).ToList();
                pdbInterfaceInterfaceData = pdbInterfaceInterfaceData.Where(a => pdbChains.Contains(a.ReceptorChainId) && pdbChains.Contains(a.LigandChainId)).ToList();
                pdbInterfaceInterfaceData = pdbInterfaceInterfaceData.Where(a => ((double)a.LigandInterfaceAllAminoAcidsSuper.Length / (double)a.LigandSequenceSuper.Length) < 0.5).ToList();


                for (int index = 0; index < pdbInterfaceInterfaceData.Count; index++)
                {
                    var receptorLigandDimerA = pdbInterfaceInterfaceData[index];
                    var outputFolder = @"c:\r\" + receptorLigandDimerA.LigandPdbId + "_" + receptorLigandDimerA.LigandChainId + "_" + (index + 1) + "_" + receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length + "_" + receptorLigandDimerA.LigandSequenceSuper.Length + "_" + receptorLigandDimerA.LigandInterfaceSuperStart + "_" + receptorLigandDimerA.LigandInterfaceSuperEnd + @"\";

                    var substitutionList = new List<DonatedSubstitutionInfo>();

                    var receptorLigandDimerChainOrderA = new List<char> { receptorLigandDimerA.ReceptorChainId, receptorLigandDimerA.LigandChainId };


                    for (var interfacePositionIndex = 0; interfacePositionIndex <= receptorLigandDimerA.LigandSequenceSuper.Length - receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length; interfacePositionIndex++)
                    {
                        var subInterface = receptorLigandDimerA.LigandSequenceSuper.Substring(interfacePositionIndex, receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length);
                        var description = "Interface Sliding Window " + interfacePositionIndex + " " + (interfacePositionIndex + subInterface.Length);
                        substitutionList.Add(new DonatedSubstitutionInfo(description, description.Replace(" ", "_"), subInterface, subInterface, receptorLigandDimerA));
                    }

                    var receptorLigandFilename = @"" + receptorLigandDimerA.LigandPdbId + ".pdb";

                    var acceptorTemplateAllAtomPdbFileData = !structureOutputTypes.Contains("all") ? null : File.ReadAllLines(atomsFullFolder + receptorLigandFilename).Where(a => a[atom_icode] == ' ' && ((a[atom_chain] == receptorLigandDimerA.ReceptorChainId) || (a[atom_chain] == receptorLigandDimerA.LigandChainId))).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToArray();

                    var monomerAcceptorTemplateLigandAllAtomPdbFile = !structureOutputTypes.Contains("all") ? null : acceptorTemplateAllAtomPdbFileData.Where(a => a[atom_icode] == ' ' && a[atom_chain] == receptorLigandDimerA.LigandChainId).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToList();

                    var receptorHeader = ">" + receptorLigandDimerA.ReceptorPdbId + ":" + receptorLigandDimerA.ReceptorChainId;
                    var ligandHeader = ">" + receptorLigandDimerA.LigandPdbId + ":" + receptorLigandDimerA.LigandChainId;

                    var dimerAcceptorTemplateDimerSequenceSuper = new List<Sequence> { new Sequence(receptorHeader, receptorLigandDimerA.ReceptorSequenceSuper), new Sequence(ligandHeader, receptorLigandDimerA.LigandSequenceSuper) };

                    foreach (var sub in substitutionList)
                    {
                        // align ligand interface sequence and substitution sequence
                        var offset = AlignInside(receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper, sub.InterfacePadded);

                        if (receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper.Length < sub.InterfacePadded.Length) { offset = -offset; }

                        var acceptorTemplateSubstitutionStart = receptorLigandDimerA.LigandInterfaceSuperStart + offset;

                        //var info = string.Join(Environment.NewLine, new string[] { "" + receptorLigandDimerA.ReceptorSequenceClusterIndex, "" + receptorLigandDimerA.ReceptorInterfaceSequenceClusterIndex, "" + receptorLigandDimerA.LigandSequenceClusterIndex, "" + receptorLigandDimerA.LigandInterfaceSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorInterfaceSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandSequenceClusterIndex, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandInterfaceSequenceClusterIndex, receptorLigandDimerA.ReceptorPdbId, "" + receptorLigandDimerA.ReceptorChainId, receptorLigandDimerA.LigandPdbId, "" + receptorLigandDimerA.LigandChainId, sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorPdbId, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.ReceptorChainId, sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandPdbId, "" + sub.InterfaceInterfaceSequenceIdentityClusteringItem.LigandChainId, sub.Description, receptorLigandDimerA.LigandInterfaceAllAminoAcidsSuper, sub.Interface, sub.InterfacePadded });

                        var modelOutputFolder = outputFolder + sub.Folder + @"\";
                        Directory.CreateDirectory(modelOutputFolder);

                        //File.WriteAllText(modelOutputFolder + "substitution.log", info);

                        if (structureOutputTypes.Contains("all")) File.WriteAllLines(modelOutputFolder + "template_ligand_all.pdb", monomerAcceptorTemplateLigandAllAtomPdbFile);

                        var mutatedLigandSequenceSubLigand = new Sequence(ligandHeader, receptorLigandDimerA.LigandSequenceSuper.Remove(acceptorTemplateSubstitutionStart, sub.InterfacePadded.Length).Insert(acceptorTemplateSubstitutionStart, sub.InterfacePadded));


                        var dimerSequenceAfterSubstitution = dimerAcceptorTemplateDimerSequenceSuper.Select(a => a.Id == ligandHeader ? mutatedLigandSequenceSubLigand : a).ToList();

                        if (sequenceOutputTypes.Contains("pir")) File.WriteAllText(modelOutputFolder + "template_ligand_after_substitution.ali", Sequence.GetFormattedSequence(mutatedLigandSequenceSubLigand, Sequence.SequenceFormat.Pir));
                    }
                }

            }
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
