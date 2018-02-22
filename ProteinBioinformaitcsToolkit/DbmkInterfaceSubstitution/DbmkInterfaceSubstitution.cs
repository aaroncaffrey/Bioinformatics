using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.Dssp;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace DbmkInterfaceSubstitution
{
    class DbmkInterfaceSubstitution
    {
        public static List<Sequence> PdbSeqList = Sequence.LoadSequenceFile(@"C:\Users\k1040\Downloads\pdb_seqres.txt\pdb_seqres.txt");

        public static string InterfaceDataFolder = @"C:\ibis_dbmk_multibinding\interfaces_-_6.8_0_5_0.5\";
        public static List<List<string>> LoadIbisPpiData(string interfaceId)
        {
            var pdbId = interfaceId.Substring(0, 4).ToUpperInvariant();
            var chainId = interfaceId.Length > 4 ? interfaceId.Substring(4, 1).ToUpperInvariant()[0] : ' ';
            var interfaceIndex = interfaceId.Length > 5 ? int.Parse(interfaceId.Substring(5)) : -1;

            var ibisFile = @"C:\Users\k1040\Downloads\ibisdown.tar\ibisdown\" + pdbId.Substring(1, 2) + @"\" + pdbId.Substring(0, 4) + @".txt";
            if (!File.Exists(ibisFile)) return null;

            var data = File.ReadAllLines(ibisFile).Select(b => b.Split(':').Select(c => c.Trim()).ToList()).ToList();

            if (chainId != ' ') data = data.Where(a => a[0] == pdbId + chainId).ToList();

            data = data.Where(b => b[1] == "PPI").ToList();

            return data;
        }

        public static Tuple<List<string>, List<string>> LoadIbisPpiPdbEvidenceList(string interfaceId)
        {
            // here, the query is a ligand

            var pdbId = interfaceId.Substring(0, 4);
            var chainId = interfaceId.Substring(4, 1)[0];
            var interfaceIndex = interfaceId.Length>=5? int.Parse(interfaceId.Substring(5)) : -1;

            var data = LoadIbisPpiData(interfaceId);

            var complexes = data.Select(b => b[12]).ToList();
            var receptors = complexes.Select(b => b.Substring(0, b.IndexOf('_'))).ToList();
            var ligands = complexes.Select(b => b.Substring(b.IndexOf('_') + 1)).ToList();

            return new Tuple<List<string>, List<string>>(receptors, ligands);
        }

        /*public static void FindInterfacePartners(string interfaceId)
        {
            // finds a list of proteins which interact with the same receptor interface or ligand interface

            //var ppi = LoadIbisPpiPdbEvidenceList(interfaceId);

            var pdbId = interfaceId.Substring(0, 4);
            var chainId = interfaceId.Substring(4, 1)[0];
            var interfaceIndex = interfaceId.Length >= 5 ? int.Parse(interfaceId.Substring(5)) : -1;

            var ibisData = LoadIbisData(interfaceId);


            //var seq = PdbSeqList.First(a => a.IdSplit.PdbId == pdbId && a.IdSplit.ChainId == chainId);

            //var entries = PdbSeqList.Where(a => a.FullSequence == seq.FullSequence).ToList();

            //Debug.WriteLine("");
            //Debug.WriteLine(pdbId + "_" + chainId + ", " + entries.Count + " matches:");
            //entries.ForEach(a=>Debug.Write(a.IdSplit.PdbId+a.IdSplit.ChainId));


            var receptorLigandLists = LoadIbisPpiPdbEvidenceList(interfaceId);



        }*/
        /*
        public static void AddPartnerChain(string csvFile)
        {
            var data = File.ReadAllLines(csvFile).ToList();

            var line = data[1];

            var lineSplit = line.Split(',');

            var pdbId = lineSplit[1];
            var chainId = lineSplit[2][0];
            var interfaceIndex = int.Parse(lineSplit[3]); // might not match?
            var interfaceStart = int.Parse(lineSplit[4]);
            var interfaceEnd = int.Parse(lineSplit[5]);
            var interfaceLength = int.Parse(lineSplit[6]);

            var interfaceData = ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load(InterfaceDataFolder + @"\interface-interface_" + pdbId + ".pdb.csv");

            var partnerChainId = interfaceData.FirstOrDefault(a => a.ReceptorChainId == chainId && a.ReceptorInterfaceSuperLength==interfaceLength && a.ReceptorInterfaceSuperStart==interfaceStart&&a.ReceptorInterfaceSuperEnd==interfaceEnd&& char.IsLetterOrDigit(a.LigandChainId)).LigandChainId;

            data = data.Select((a,i) => i==0? a + ',' + "Partner Chain Id" : a + ',' + partnerChainId).ToList();

            File.WriteAllLines(csvFile,data);
        }
        */

        public static string InterfaceSubsequence(string sequence, int start, int end, int length)
        {
            // returns a subsequence of sequence, from the centre of start and end, of given length, assumes numbers are 1-based
            // start and end are taken from residue sequence indexes in the pdb structure files
            // length may be longer than ((end-start)+1) for getting the flanking residues around the interface for substitution with mismatching sized interfaces (can be larger or smaller)
            string result;

            //start--;
            //end--;

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

        public static void FindReceptorEntries(string csvFileLigand)
        {
            const int atom_chain = 21;
            const int atom_chain_len = 1;

            const int atom_icode = 26;
            const int atom_icode_len = 1;

            const int atom_type = 14;
            const int atom_type_len = 3;

            const int atom_resseq = 22;
            const int atom_resseq_len = 4;
            //Debug.WriteLine("");

            var data = File.ReadAllLines(csvFileLigand).ToList();

            var line = data[1];

            var lineSplit = line.Split(',');

            var csvPdbId = lineSplit[1];
            var csvChainId = lineSplit[2][0];
            var interfaceIndex = int.Parse(lineSplit[3]); // might not match?
            var interfaceStart = int.Parse(lineSplit[4]);
            var interfaceEnd = int.Parse(lineSplit[5]);
            var interfaceLength = int.Parse(lineSplit[6]);
            var csvPartnerChainId = lineSplit[lineSplit.Length-1][0];
            //var querySeq = PdbSeqList.First(a => a.IdSplit.PdbId == csvPdbId && a.IdSplit.ChainId == csvChainId);

            // match the csv file with the interface entry it was generated from
            var csvInterfaceDataList1 = ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load(InterfaceDataFolder + @"\interface-interface_" + csvPdbId + ".pdb.csv");
            //if (!File.Exists(InterfaceDataFolder + @"\interface-interface_" + csvPdbId + ".pdb.csv")) Debug.WriteLine(csvPdbId);
            //return;
            if (csvInterfaceDataList1 == null) return;
            var csvInterfaceDataReceptor = csvInterfaceDataList1.First(a => a.LigandChainId == csvChainId && a.ReceptorChainId==csvPartnerChainId && a.LigandInterfaceSuperLength == interfaceLength && a.LigandInterfaceSuperStart == interfaceStart && a.LigandInterfaceSuperEnd == interfaceEnd && char.IsLetterOrDigit(a.ReceptorChainId) && char.IsLetterOrDigit(a.LigandChainId));
            var csvInterfaceDataReceptorResidueIds = Enumerable.Range(csvInterfaceDataReceptor.ReceptorInterfaceSuperStart, (csvInterfaceDataReceptor.ReceptorInterfaceSuperEnd - csvInterfaceDataReceptor.ReceptorInterfaceSuperStart)+1).Select(a=>""+a).ToList();

            // load ibis of partner chain (the receptor) to find other ligands
            var ibisReceptorData = LoadIbisPpiData(csvPdbId + csvPartnerChainId);
            if (ibisReceptorData == null) return;
            
            // get ibis entry line for the receptor interface
            var ibisReceptorEntry = ibisReceptorData.FirstOrDefault(a => a[12] == csvPdbId + csvPartnerChainId + "_" + csvPdbId + csvChainId);
            if (ibisReceptorEntry == null) return;

            // get receptor interface residue indexes for overlap comparison (to check if other ligands make use of the same receptor interface)
            var receptorIbisMmResidueIds = ibisReceptorEntry[2].Trim().Split(' ');
            var receptorIbisPdbResidueIds = ibisReceptorEntry[3].Trim().Split(' ');


            
            //Debug.WriteLine("");
            //Debug.WriteLine("");
            //Debug.WriteLine("File: " + csvFileLigand);
            //Debug.WriteLine("");
            //Debug.WriteLine("Receptor: " + csvInterfaceDataReceptor.ReceptorInterfaceAllAminoAcidsSuper);
            //Debug.WriteLine("Ligand (input): " + csvInterfaceDataReceptor.LigandInterfaceAllAminoAcidsSuper);
            //Debug.WriteLine("");
            var receptorIdsWithIntersection = new List<string>();
            var receptorIdsWithoutIntersection = new List<string>();


            // same receptor interface domain?
            ibisReceptorData = ibisReceptorData.Where(a => a[15] == ibisReceptorEntry[15]).ToList();

            // remove main receptor from list, as it is the main one, rather than a sibling
            //ibisReceptorData = ibisReceptorData.Where(a => a[12] != csvPdbId + csvPartnerChainId + "_" + csvPdbId + csvChainId).ToList();
            
            // --- commented out temporarily -  
            ibisReceptorData = ibisReceptorData.Where(a => !a[12].EndsWith("_" + csvPdbId + csvChainId)).ToList();

            // loop through receptor and receptor homologs
            foreach (var ibisPpiEntry in ibisReceptorData)
            {
                // loop through ibis ppi entries to find matching receptor interfaces
                // receptor sequence may mismatch as ibis represents domain interactions

                var dimer = ibisPpiEntry[12];
                var receptorId = dimer.Substring(0, dimer.IndexOf('_'));
                var ligandId = dimer.Substring(dimer.IndexOf('_') + 1);

                if (receptorId == ligandId) continue; // same chain

                var receptorPdbId = receptorId.Substring(0, 4);
                var receptorChainId = receptorId.Substring(4, 1)[0];
                var ligandPdbId = ligandId.Substring(0, 4);
                var ligandChainId = ligandId.Substring(4, 1)[0];


                var receptorMmResiduesIds = ibisPpiEntry[2].Trim().Split(' ');
                var receptorPdbResiduesIds = ibisPpiEntry[3].Trim().Split(' ');


                // does custom criteria interface and this ibis entry intersect?
                var intersectPdbIdsCountPbtk = receptorPdbResiduesIds.Intersect(csvInterfaceDataReceptorResidueIds).Count();

                // does ibis interface entry intersect?
                var intersectMmIdsCountIbis = receptorMmResiduesIds.Intersect(receptorIbisMmResidueIds).Count();

                if (intersectPdbIdsCountPbtk == 0 || intersectMmIdsCountIbis == 0)
                {
                    receptorIdsWithoutIntersection.Add(receptorPdbId + receptorChainId);
                }
                else
                {
                    receptorIdsWithIntersection.Add(receptorPdbId + receptorChainId);
                }
            }

            receptorIdsWithoutIntersection = receptorIdsWithoutIntersection.OrderBy(a => a).Distinct().ToList();
            receptorIdsWithIntersection = receptorIdsWithIntersection.OrderBy(a => a).Distinct().ToList();
            //var receptorInterfaceHomologs = new List<string>();

            var interfaces = new List<Tuple<decimal, string, string, string, string>>();

            foreach (var receptorId in receptorIdsWithIntersection)
            {
                var receptorPdbId = receptorId.Substring(0, 4);
                var receptorChainId = receptorId.Substring(4, 1)[0];
                // does custom criteria interface & custom criteria interface entry intersect?


                var receptorInterfaceDataList = ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load(InterfaceDataFolder + @"\interface-interface_" + receptorPdbId + ".pdb.csv");



                //if (!File.Exists(InterfaceDataFolder + @"\interface-interface_" + receptorPdbId + ".pdb.csv")) Debug.WriteLine(receptorPdbId);
                //continue;



                if (receptorInterfaceDataList == null) continue;

                receptorInterfaceDataList = receptorInterfaceDataList.Where(a => a.ReceptorChainId == receptorChainId && a.ReceptorChainId!=a.LigandChainId && char.IsLetterOrDigit(a.LigandChainId)).ToList();

                if (receptorInterfaceDataList.Count == 0) continue;

                //var interfaceDataList2filtered = new List<ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData>();

                // there may be more than 1 interface, we only want the interfaces which intersect residue ids
                receptorInterfaceDataList = receptorInterfaceDataList.Where(a =>
                                                                            {
                                                                                var receptorInterfaceDataResidueIds = Enumerable.Range(a.ReceptorInterfaceSuperStart, (a.ReceptorInterfaceSuperEnd - a.ReceptorInterfaceSuperStart) + 1).Select(b => "" + b).ToList();

                                                                                var intersectPdbIdsCount = csvInterfaceDataReceptorResidueIds.Intersect(receptorInterfaceDataResidueIds).Count();

                                                                                return intersectPdbIdsCount > 0;
                                                                            }).ToList();

                var alignmentScores = new Dictionary<ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData, Tuple<ProteinBioClass.AlignmentScore, ProteinBioClass.AlignmentScore>>();

                // the receptor interface should have common ancestory
                /*
                receptorInterfaceDataList = receptorInterfaceDataList.Where(a =>
                                                                            {
                                                                                var csvReceptorInterfaceSequence = Sequence.EscapeAminoAcidSequence(csvInterfaceDataReceptor.ReceptorInterfaceAllAminoAcidsSuper).Replace('X', '-');
                                                                                var ibisReceptorInterfaceSequence = Sequence.EscapeAminoAcidSequence(a.ReceptorInterfaceAllAminoAcidsSuper).Replace('X', '-');
                                                                                var requiredLength = csvReceptorInterfaceSequence.Length > ibisReceptorInterfaceSequence.Length ? csvReceptorInterfaceSequence.Length : ibisReceptorInterfaceSequence.Length;

                                                                                var csvReceptorInterfaceSequenceExtended = Sequence.EscapeAminoAcidSequence(InterfaceSubsequence(csvInterfaceDataReceptor.ReceptorSequenceSuper, csvInterfaceDataReceptor.ReceptorInterfaceSuperStart + 1, csvInterfaceDataReceptor.ReceptorInterfaceSuperEnd + 1, requiredLength), '-');
                                                                                var ibisReceptorInterfaceSequenceExtended = Sequence.EscapeAminoAcidSequence(InterfaceSubsequence(a.ReceptorSequenceSuper, a.ReceptorInterfaceSuperStart + 1, a.ReceptorInterfaceSuperEnd + 1, requiredLength), '-');

                                                                                var alignmentScore = ProteinBioClass.AlignedSequenceSimilarityPercentage(csvReceptorInterfaceSequence, ibisReceptorInterfaceSequence, ProteinBioClass.AlignmentType.NMW, ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength);
                                                                                var alignmentScoreExtended = ProteinBioClass.AlignedSequenceSimilarityPercentage(csvReceptorInterfaceSequenceExtended, ibisReceptorInterfaceSequenceExtended, ProteinBioClass.AlignmentType.NMW, ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength);

                                                                                // Debug.WriteLine("score: " + alignmentScore.Score + " " + alignmentScore.ScoreEvo + " " + alignmentScoreExtended.Score + " " + alignmentScoreExtended.ScoreEvo);

                                                                                alignmentScores.Add(a, new Tuple<ProteinBioClass.AlignmentScore, ProteinBioClass.AlignmentScore>(alignmentScore,alignmentScoreExtended));

                                                                                return alignmentScore.Score>0.2m|| alignmentScoreExtended.Score>0.2m;
                                                                            }).ToList();*/


                

                foreach (var receptorInterfaceData in receptorInterfaceDataList)
                {
                    // add source and dest
                    var destination = csvInterfaceDataReceptor;//.LigandInterfaceAllAminoAcidsSuper;
                    var source = receptorInterfaceData;//.LigandInterfaceAllAminoAcidsSuper;

                    // get flanking residues to match size (of larger interface)
                    var bLigandInterface = Sequence.EscapeAminoAcidSequence(source.LigandInterfaceAllAminoAcidsSuper).Replace('X', '-');

                    // get correct size (from A) ligand interface (from B) for substitution
                    //var requiredSubstitutionLength = receptorLigandDimerA.LigandInterfaceResSeqLength;
                    //var requiredSubstitutionLength = source.LigandInterfaceAllAminoAcidsSuper.Length > destination.LigandInterfaceAllAminoAcidsSuper.Length ? source.LigandInterfaceAllAminoAcidsSuper.Length : destination.LigandInterfaceAllAminoAcidsSuper.Length;
                    //if (requiredSubstitutionLength > destination.LigandInterfaceAllAminoAcidsSuper.Length) requiredSubstitutionLength = source.LigandInterfaceAllAminoAcidsSuper.Length;
                    var requiredSubstitutionLength = destination.LigandInterfaceAllAminoAcidsSuper.Length;

                    //var bLigandInterfaceExtended = Sequence.EscapeAminoAcidSequence(InterfaceSubsequence(receptorInterfaceData.LigandSequenceSuper, receptorInterfaceData.LigandInterfaceSuperStart + 1, receptorInterfaceData.LigandInterfaceSuperEnd + 1, requiredSubstitutionLength), '-');
                    var bLigandInterfaceExtended = Sequence.EscapeAminoAcidSequence(InterfaceSubsequence(source.LigandSequenceSuper, source.LigandInterfaceSuperStart + 1, source.LigandInterfaceSuperEnd+ 1, requiredSubstitutionLength), '-');

                    //"GFHFCGGSLINENWVVTAAHCGVTTS"

                    //var score = (alignmentScores[receptorInterfaceData].Item1.Score + alignmentScores[receptorInterfaceData].Item1.ScoreEvo + alignmentScores[receptorInterfaceData].Item2.Score + alignmentScores[receptorInterfaceData].Item2.ScoreEvo) / 4;
                    var score = 0m;

                    interfaces.Add(new Tuple<decimal, string, string, string, string>(
                        score, 
                        receptorId,
                        receptorInterfaceData.LigandPdbId + receptorInterfaceData.LigandChainId,
                        bLigandInterface,//receptorInterfaceData.LigandInterfaceAllAminoAcidsSuper,
                        bLigandInterfaceExtended));

                    //Debug.WriteLine(receptorId + ": " + receptorInterfaceData.LigandPdbId+ receptorInterfaceData.LigandChainId+": " + bLigandInterfaceExtended + ": " + receptorInterfaceData.LigandInterfaceAllAminoAcidsSuper);
                }


                

            }
            const int max_siblings = 5;

            ////interfaces=interfaces.Where(a=>a.)
            //interfaces = interfaces.OrderByDescending(a=>a.Item5.Length).ThenByDescending(a => a.Item1).ToList();
            //interfaces = interfaces.Where(a => a.Item4 != csvInterfaceDataReceptor.LigandInterfaceAllAminoAcidsSuper && a.Item5 != csvInterfaceDataReceptor.LigandInterfaceAllAminoAcidsSuper).ToList();
            //interfaces = interfaces.GroupBy(a => a.Item5).Select(a => a.First()).ToList();
            //interfaces = interfaces.GetRange(0, interfaces.Count >= max_siblings ? max_siblings : interfaces.Count).ToList();

            var pdbAtomsFile = csvInterfaceDataReceptor.ReceptorPdbId + (csvInterfaceDataReceptor.ReceptorChainId < csvInterfaceDataReceptor.LigandChainId ? "" + csvInterfaceDataReceptor.ReceptorChainId + csvInterfaceDataReceptor.LigandChainId : "" + csvInterfaceDataReceptor.LigandChainId + csvInterfaceDataReceptor.ReceptorChainId) + ".pdb";

            var receptorLigandDimerChainOrderA = new List<char> { csvInterfaceDataReceptor.ReceptorChainId, csvInterfaceDataReceptor.LigandChainId };
            var acceptorTemplateAllAtomPdbFileData = File.ReadAllLines(@"c:\pdbe_split\" + pdbAtomsFile).Where(a => a[atom_icode] == ' ' && ((a[atom_chain] == csvInterfaceDataReceptor.ReceptorChainId) || (a[atom_chain] == csvInterfaceDataReceptor.LigandChainId))).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToArray();
            var monomerAcceptorTemplateLigandAllAtomPdbFile = acceptorTemplateAllAtomPdbFileData.Where(a => a[atom_icode] == ' ' && a[atom_chain] == csvInterfaceDataReceptor.LigandChainId).OrderBy(a => receptorLigandDimerChainOrderA.IndexOf(a[atom_chain])).ThenBy(a => int.Parse(a.Substring(atom_resseq, atom_resseq_len))).ToList();
            var ligandHeader = ">" + csvInterfaceDataReceptor.LigandPdbId + ":" + csvInterfaceDataReceptor.LigandChainId;

            interfaces.ForEach(a =>
                               {
                                   //Debug.WriteLine(a.Item1 + ": " + a.Item2 + ": " + a.Item3 + ": " + a.Item4 + ": " + a.Item5)

                                   //Debug.WriteLine("Substitute in " + csvInterfaceDataReceptor.LigandPdbId + csvInterfaceDataReceptor.LigandChainId + " interface " + csvInterfaceDataReceptor.LigandInterfaceAllAminoAcidsSuper + " with sibling " + a.Item3 + " interface " + a.Item5);

                                   var offset = AlignInside(csvInterfaceDataReceptor.LigandInterfaceAllAminoAcidsSuper, a.Item5);

                                   if (csvInterfaceDataReceptor.LigandInterfaceAllAminoAcidsSuper.Length < a.Item5.Length) { offset = -offset; }

                                   var acceptorTemplateSubstitutionStart = csvInterfaceDataReceptor.LigandInterfaceResSeqStart + offset;
                                   var acceptorTemplateSubstitutionEnd = acceptorTemplateSubstitutionStart + a.Item5.Length - 1;

                                   var templateStructureOutputFolder = @"c:\pdbe_split\models3\" + csvInterfaceDataReceptor.LigandPdbId + csvInterfaceDataReceptor.LigandChainId + interfaceIndex + @"\";
                                   var threadingSequenceOutputFolder = @"c:\pdbe_split\models3\" + csvInterfaceDataReceptor.LigandPdbId + csvInterfaceDataReceptor.LigandChainId + interfaceIndex + @"\" + a.Item3 + @"\";

                                   Directory.CreateDirectory(templateStructureOutputFolder);
                                   Directory.CreateDirectory(threadingSequenceOutputFolder);

                                   File.WriteAllLines(templateStructureOutputFolder + "template_ligand_all.pdb", monomerAcceptorTemplateLigandAllAtomPdbFile);

                                   //var mutatedLigandSequenceSubLigand = new Sequence(ligandHeader, csvInterfaceDataReceptor.LigandSequenceSuper.Remove(acceptorTemplateSubstitutionStart, a.Item5.Length).Insert(acceptorTemplateSubstitutionStart, a.Item5));

                                   var mutatedLigandSequenceSubLigand = new Sequence(ligandHeader, csvInterfaceDataReceptor.LigandSequenceSuper.Remove(csvInterfaceDataReceptor.LigandInterfaceSuperStart, a.Item5.Length).Insert(csvInterfaceDataReceptor.LigandInterfaceSuperStart, a.Item5));

                                   File.WriteAllText(threadingSequenceOutputFolder + "template_ligand_after_substitution.ali", Sequence.GetFormattedSequence(mutatedLigandSequenceSubLigand, Sequence.SequenceFormat.Pir));
                               });

            /*
                  
                  // how similar is receptor to query?
                var rSeq = PdbSeqList.First(a => a.IdSplit.PdbId == rPdbId && a.IdSplit.ChainId == rChainId);

                var nmw1 = new NeedlemanWunsch(querySeq.FullSequence, rSeq.FullSequence);
                var aligned1 = nmw1.getAlignment();
                var similarity1 = ProteinBioClass.SequenceSimilarityPercentage(aligned1[0], aligned1[1]);
                var midScore1 = (similarity1.Score + similarity1.ScoreEvo) / 2;

                Debug.WriteLine(pdbId + " " + rPdbId);

                if (midScore1 < 0.9m) continue;

                Debug.WriteLine(pdbId + " " + rPdbId);
                */

            //Debug.WriteLine("Receptor list (take interfaces from ligands): " + csvPdbId+csvChainId+": "+ String.Join(", ",receptorInterfaceHomologs));

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

        public static void s()
        {
            var x = @"c:\pdbe_split\contacts_-_6.8\1A0HBD.pdb
c:\pdbe_split\contacts_-_6.8\1A0JAB.pdb
c:\pdbe_split\contacts_-_6.8\1A0JBC.pdb
c:\pdbe_split\contacts_-_6.8\1A0JCD.pdb
c:\pdbe_split\contacts_-_6.8\1A2KAD.pdb
c:\pdbe_split\contacts_-_6.8\1A2KBC.pdb
c:\pdbe_split\contacts_-_6.8\1A7XAB.pdb
c:\pdbe_split\contacts_-_6.8\1ACBEI.pdb
c:\pdbe_split\contacts_-_6.8\1AK4AD.pdb
c:\pdbe_split\contacts_-_6.8\1AKSAB.pdb
c:\pdbe_split\contacts_-_6.8\1AM4AD.pdb
c:\pdbe_split\contacts_-_6.8\1AMHAB.pdb
c:\pdbe_split\contacts_-_6.8\1AO5AB.pdb
c:\pdbe_split\contacts_-_6.8\1ATHAB.pdb
c:\pdbe_split\contacts_-_6.8\1ATNAD.pdb
c:\pdbe_split\contacts_-_6.8\1AYFAB.pdb
c:\pdbe_split\contacts_-_6.8\1B1ZBC.pdb
c:\pdbe_split\contacts_-_6.8\1B1ZCD.pdb
c:\pdbe_split\contacts_-_6.8\1B6CAB.pdb
c:\pdbe_split\contacts_-_6.8\1B88AB.pdb
c:\pdbe_split\contacts_-_6.8\1BGXHT.pdb
c:\pdbe_split\contacts_-_6.8\1BGXLT.pdb
c:\pdbe_split\contacts_-_6.8\1BI8AB.pdb
c:\pdbe_split\contacts_-_6.8\1BI8CD.pdb
c:\pdbe_split\contacts_-_6.8\1BKDRS.pdb
c:\pdbe_split\contacts_-_6.8\1BMLAC.pdb
c:\pdbe_split\contacts_-_6.8\1BMLBD.pdb
c:\pdbe_split\contacts_-_6.8\1BTHHL.pdb
c:\pdbe_split\contacts_-_6.8\1BU1AD.pdb
c:\pdbe_split\contacts_-_6.8\1BU1CE.pdb
c:\pdbe_split\contacts_-_6.8\1BUHAB.pdb
c:\pdbe_split\contacts_-_6.8\1BUIBC.pdb
c:\pdbe_split\contacts_-_6.8\1BVNPT.pdb
c:\pdbe_split\contacts_-_6.8\1C12AB.pdb
c:\pdbe_split\contacts_-_6.8\1C2OCD.pdb
c:\pdbe_split\contacts_-_6.8\1C8TAB.pdb
c:\pdbe_split\contacts_-_6.8\1C9TAG.pdb
c:\pdbe_split\contacts_-_6.8\1CC0AE.pdb
c:\pdbe_split\contacts_-_6.8\1CHOFI.pdb
c:\pdbe_split\contacts_-_6.8\1CICAD.pdb
c:\pdbe_split\contacts_-_6.8\1CICBC.pdb
c:\pdbe_split\contacts_-_6.8\1CJEAB.pdb
c:\pdbe_split\contacts_-_6.8\1CKSAB.pdb
c:\pdbe_split\contacts_-_6.8\1CKSBC.pdb
c:\pdbe_split\contacts_-_6.8\1CMKEI.pdb
c:\pdbe_split\contacts_-_6.8\1CMXBC.pdb
c:\pdbe_split\contacts_-_6.8\1CSEEI.pdb
c:\pdbe_split\contacts_-_6.8\1CXZAB.pdb
c:\pdbe_split\contacts_-_6.8\1D1JAB.pdb
c:\pdbe_split\contacts_-_6.8\1D1JAC.pdb
c:\pdbe_split\contacts_-_6.8\1D5BAH.pdb
c:\pdbe_split\contacts_-_6.8\1D5MAC.pdb
c:\pdbe_split\contacts_-_6.8\1D6RAI.pdb
c:\pdbe_split\contacts_-_6.8\1DB2AB.pdb
c:\pdbe_split\contacts_-_6.8\1DFJEI.pdb
c:\pdbe_split\contacts_-_6.8\1DKTAB.pdb
c:\pdbe_split\contacts_-_6.8\1DS5BE.pdb
c:\pdbe_split\contacts_-_6.8\1DS5BF.pdb
c:\pdbe_split\contacts_-_6.8\1DX5IM.pdb
c:\pdbe_split\contacts_-_6.8\1E0FCE.pdb
c:\pdbe_split\contacts_-_6.8\1E0FEJ.pdb
c:\pdbe_split\contacts_-_6.8\1E0OAB.pdb
c:\pdbe_split\contacts_-_6.8\1EAIAC.pdb
c:\pdbe_split\contacts_-_6.8\1EAKAC.pdb
c:\pdbe_split\contacts_-_6.8\1EFNAB.pdb
c:\pdbe_split\contacts_-_6.8\1EQYAS.pdb
c:\pdbe_split\contacts_-_6.8\1ESVAS.pdb
c:\pdbe_split\contacts_-_6.8\1EWYAC.pdb
c:\pdbe_split\contacts_-_6.8\1EZUAC.pdb
c:\pdbe_split\contacts_-_6.8\1F5WAB.pdb
c:\pdbe_split\contacts_-_6.8\1F9JAB.pdb
c:\pdbe_split\contacts_-_6.8\1FCCBD.pdb
c:\pdbe_split\contacts_-_6.8\1FE8AL.pdb
c:\pdbe_split\contacts_-_6.8\1FFWAB.pdb
c:\pdbe_split\contacts_-_6.8\1FGLAB.pdb
c:\pdbe_split\contacts_-_6.8\1FI8AC.pdb
c:\pdbe_split\contacts_-_6.8\1FJ1AF.pdb
c:\pdbe_split\contacts_-_6.8\1FJ1BF.pdb
c:\pdbe_split\contacts_-_6.8\1FKIAB.pdb
c:\pdbe_split\contacts_-_6.8\1FOEAB.pdb
c:\pdbe_split\contacts_-_6.8\1FONAB.pdb
c:\pdbe_split\contacts_-_6.8\1FQ1AB.pdb
c:\pdbe_split\contacts_-_6.8\1FQJAB.pdb
c:\pdbe_split\contacts_-_6.8\1FSKAB.pdb
c:\pdbe_split\contacts_-_6.8\1FUJBC.pdb
c:\pdbe_split\contacts_-_6.8\1FUJCD.pdb
c:\pdbe_split\contacts_-_6.8\1FVEAB.pdb
c:\pdbe_split\contacts_-_6.8\1G16CD.pdb
c:\pdbe_split\contacts_-_6.8\1G4URS.pdb
c:\pdbe_split\contacts_-_6.8\1G9IEI.pdb
c:\pdbe_split\contacts_-_6.8\1GGRAB.pdb
c:\pdbe_split\contacts_-_6.8\1GHQAB.pdb
c:\pdbe_split\contacts_-_6.8\1GHQBC.pdb
c:\pdbe_split\contacts_-_6.8\1GIGHL.pdb
c:\pdbe_split\contacts_-_6.8\1GLAFG.pdb
c:\pdbe_split\contacts_-_6.8\1GPWAB.pdb
c:\pdbe_split\contacts_-_6.8\1GRIAB.pdb
c:\pdbe_split\contacts_-_6.8\1GRNAB.pdb
c:\pdbe_split\contacts_-_6.8\1GXDAC.pdb
c:\pdbe_split\contacts_-_6.8\1GZSAB.pdb
c:\pdbe_split\contacts_-_6.8\1H0DAC.pdb
c:\pdbe_split\contacts_-_6.8\1H0DBC.pdb
c:\pdbe_split\contacts_-_6.8\1H1VAG.pdb
c:\pdbe_split\contacts_-_6.8\1H4LAD.pdb
c:\pdbe_split\contacts_-_6.8\1H8PAB.pdb
c:\pdbe_split\contacts_-_6.8\1HE1AC.pdb
c:\pdbe_split\contacts_-_6.8\1HE8AB.pdb
c:\pdbe_split\contacts_-_6.8\1HEZAE.pdb
c:\pdbe_split\contacts_-_6.8\1HH4AD.pdb
c:\pdbe_split\contacts_-_6.8\1I2MAB.pdb
c:\pdbe_split\contacts_-_6.8\1I4DAD.pdb
c:\pdbe_split\contacts_-_6.8\1I4LAD.pdb
c:\pdbe_split\contacts_-_6.8\1I4TAD.pdb
c:\pdbe_split\contacts_-_6.8\1I85AB.pdb
c:\pdbe_split\contacts_-_6.8\1I85BD.pdb
c:\pdbe_split\contacts_-_6.8\1IBRAB.pdb
c:\pdbe_split\contacts_-_6.8\1IGFJL.pdb
c:\pdbe_split\contacts_-_6.8\1ILR12.pdb
c:\pdbe_split\contacts_-_6.8\1IRAXY.pdb
c:\pdbe_split\contacts_-_6.8\1ITVAB.pdb
c:\pdbe_split\contacts_-_6.8\1JEW1R.pdb
c:\pdbe_split\contacts_-_6.8\1JJOBD.pdb
c:\pdbe_split\contacts_-_6.8\1JMJAB.pdb
c:\pdbe_split\contacts_-_6.8\1JN6AB.pdb
c:\pdbe_split\contacts_-_6.8\1JQQAD.pdb
c:\pdbe_split\contacts_-_6.8\1JS0AC.pdb
c:\pdbe_split\contacts_-_6.8\1JSTAD.pdb
c:\pdbe_split\contacts_-_6.8\1K2PAB.pdb
c:\pdbe_split\contacts_-_6.8\1K5DAC.pdb
c:\pdbe_split\contacts_-_6.8\1K5GAB.pdb
c:\pdbe_split\contacts_-_6.8\1K5GAC.pdb
c:\pdbe_split\contacts_-_6.8\1K8KAF.pdb
c:\pdbe_split\contacts_-_6.8\1K8RAB.pdb
c:\pdbe_split\contacts_-_6.8\1KACAB.pdb
c:\pdbe_split\contacts_-_6.8\1KIBAG.pdb
c:\pdbe_split\contacts_-_6.8\1KIPBC.pdb
c:\pdbe_split\contacts_-_6.8\1KTKBF.pdb
c:\pdbe_split\contacts_-_6.8\1KTZAB.pdb
c:\pdbe_split\contacts_-_6.8\1KXPAD.pdb
c:\pdbe_split\contacts_-_6.8\1KXQAH.pdb
c:\pdbe_split\contacts_-_6.8\1KXTAB.pdb
c:\pdbe_split\contacts_-_6.8\1KYOOW.pdb
c:\pdbe_split\contacts_-_6.8\1LB1AB.pdb
c:\pdbe_split\contacts_-_6.8\1LFAAB.pdb
c:\pdbe_split\contacts_-_6.8\1LFDAB.pdb
c:\pdbe_split\contacts_-_6.8\1LHZAB.pdb
c:\pdbe_split\contacts_-_6.8\1LK3AH.pdb
c:\pdbe_split\contacts_-_6.8\1LK3AL.pdb
c:\pdbe_split\contacts_-_6.8\1LMKAC.pdb
c:\pdbe_split\contacts_-_6.8\1LXDAB.pdb
c:\pdbe_split\contacts_-_6.8\1M10AB.pdb
c:\pdbe_split\contacts_-_6.8\1M27AC.pdb
c:\pdbe_split\contacts_-_6.8\1M8Q8A.pdb
c:\pdbe_split\contacts_-_6.8\1M8QAV.pdb
c:\pdbe_split\contacts_-_6.8\1MAHAF.pdb
c:\pdbe_split\contacts_-_6.8\1MCOHL.pdb
c:\pdbe_split\contacts_-_6.8\1MF2HN.pdb
c:\pdbe_split\contacts_-_6.8\1MH5AB.pdb
c:\pdbe_split\contacts_-_6.8\1MHPAH.pdb
c:\pdbe_split\contacts_-_6.8\1MQ8AB.pdb
c:\pdbe_split\contacts_-_6.8\1MVWGX.pdb
c:\pdbe_split\contacts_-_6.8\1MVWMZ.pdb
c:\pdbe_split\contacts_-_6.8\1MZWAB.pdb
c:\pdbe_split\contacts_-_6.8\1N1AAB.pdb
c:\pdbe_split\contacts_-_6.8\1N4OAB.pdb
c:\pdbe_split\contacts_-_6.8\1N8OBE.pdb
c:\pdbe_split\contacts_-_6.8\1N8OCE.pdb
c:\pdbe_split\contacts_-_6.8\1NAMBH.pdb
c:\pdbe_split\contacts_-_6.8\1NCCHN.pdb
c:\pdbe_split\contacts_-_6.8\1NCCLN.pdb
c:\pdbe_split\contacts_-_6.8\1NF3AC.pdb
c:\pdbe_split\contacts_-_6.8\1NFDCD.pdb
c:\pdbe_split\contacts_-_6.8\1NFDDH.pdb
c:\pdbe_split\contacts_-_6.8\1NSGAB.pdb
c:\pdbe_split\contacts_-_6.8\1NSWCD.pdb
c:\pdbe_split\contacts_-_6.8\1NU7BD.pdb
c:\pdbe_split\contacts_-_6.8\1NVVQS.pdb
c:\pdbe_split\contacts_-_6.8\1NVVRS.pdb
c:\pdbe_split\contacts_-_6.8\1NVWQS.pdb
c:\pdbe_split\contacts_-_6.8\1NW2AC.pdb
c:\pdbe_split\contacts_-_6.8\1NW2AD.pdb
c:\pdbe_split\contacts_-_6.8\1NW2CD.pdb
c:\pdbe_split\contacts_-_6.8\1O182M.pdb
c:\pdbe_split\contacts_-_6.8\1O2FAB.pdb
c:\pdbe_split\contacts_-_6.8\1OAKAH.pdb
c:\pdbe_split\contacts_-_6.8\1OAZAL.pdb
c:\pdbe_split\contacts_-_6.8\1OC0AB.pdb
c:\pdbe_split\contacts_-_6.8\1OCWHL.pdb
c:\pdbe_split\contacts_-_6.8\1OHQAB.pdb
c:\pdbe_split\contacts_-_6.8\1OIVAB.pdb
c:\pdbe_split\contacts_-_6.8\1OJWAB.pdb
c:\pdbe_split\contacts_-_6.8\1OK2AB.pdb
c:\pdbe_split\contacts_-_6.8\1OK9AB.pdb
c:\pdbe_split\contacts_-_6.8\1OL5AB.pdb
c:\pdbe_split\contacts_-_6.8\1OM3HL.pdb
c:\pdbe_split\contacts_-_6.8\1OMEAB.pdb
c:\pdbe_split\contacts_-_6.8\1OOKBG.pdb
c:\pdbe_split\contacts_-_6.8\1OP8AB.pdb
c:\pdbe_split\contacts_-_6.8\1OP9AB.pdb
c:\pdbe_split\contacts_-_6.8\1OPHAB.pdb
c:\pdbe_split\contacts_-_6.8\1OVAAB.pdb
c:\pdbe_split\contacts_-_6.8\1OVOAB.pdb
c:\pdbe_split\contacts_-_6.8\1OW0AB.pdb
c:\pdbe_split\contacts_-_6.8\1OYVAI.pdb
c:\pdbe_split\contacts_-_6.8\1OYVBI.pdb
c:\pdbe_split\contacts_-_6.8\1P3QRV.pdb
c:\pdbe_split\contacts_-_6.8\1P57AB.pdb
c:\pdbe_split\contacts_-_6.8\1PBIAB.pdb
c:\pdbe_split\contacts_-_6.8\1PG7LW.pdb
c:\pdbe_split\contacts_-_6.8\1PIOAB.pdb
c:\pdbe_split\contacts_-_6.8\1PVHAB.pdb
c:\pdbe_split\contacts_-_6.8\1QBKBC.pdb
c:\pdbe_split\contacts_-_6.8\1QFWAH.pdb
c:\pdbe_split\contacts_-_6.8\1QKZAH.pdb
c:\pdbe_split\contacts_-_6.8\1QMZBC.pdb
c:\pdbe_split\contacts_-_6.8\1QPLAC.pdb
c:\pdbe_split\contacts_-_6.8\1QSJAB.pdb
c:\pdbe_split\contacts_-_6.8\1R0REI.pdb
c:\pdbe_split\contacts_-_6.8\1RGIAG.pdb
c:\pdbe_split\contacts_-_6.8\1RVF1H.pdb
c:\pdbe_split\contacts_-_6.8\1RVF2H.pdb
c:\pdbe_split\contacts_-_6.8\1S1CAY.pdb
c:\pdbe_split\contacts_-_6.8\1S1CBY.pdb
c:\pdbe_split\contacts_-_6.8\1S78AD.pdb
c:\pdbe_split\contacts_-_6.8\1SBBAC.pdb
c:\pdbe_split\contacts_-_6.8\1SBNEI.pdb
c:\pdbe_split\contacts_-_6.8\1SBWAI.pdb
c:\pdbe_split\contacts_-_6.8\1SCEAB.pdb
c:\pdbe_split\contacts_-_6.8\1SGFAG.pdb
c:\pdbe_split\contacts_-_6.8\1SGFBG.pdb
c:\pdbe_split\contacts_-_6.8\1SHFAB.pdb
c:\pdbe_split\contacts_-_6.8\1SJHBD.pdb
c:\pdbe_split\contacts_-_6.8\1SQ0AB.pdb
c:\pdbe_split\contacts_-_6.8\1SQKAB.pdb
c:\pdbe_split\contacts_-_6.8\1SVZAB.pdb
c:\pdbe_split\contacts_-_6.8\1SYRAB.pdb
c:\pdbe_split\contacts_-_6.8\1T0PAB.pdb
c:\pdbe_split\contacts_-_6.8\1T44AG.pdb
c:\pdbe_split\contacts_-_6.8\1TABEI.pdb
c:\pdbe_split\contacts_-_6.8\1TBRHR.pdb
c:\pdbe_split\contacts_-_6.8\1TCOAC.pdb
c:\pdbe_split\contacts_-_6.8\1TCOBC.pdb
c:\pdbe_split\contacts_-_6.8\1TE1AB.pdb
c:\pdbe_split\contacts_-_6.8\1TGSIZ.pdb
c:\pdbe_split\contacts_-_6.8\1TMQAB.pdb
c:\pdbe_split\contacts_-_6.8\1TPAEI.pdb
c:\pdbe_split\contacts_-_6.8\1TQCAC.pdb
c:\pdbe_split\contacts_-_6.8\1TU3AF.pdb
c:\pdbe_split\contacts_-_6.8\1TU3AG.pdb
c:\pdbe_split\contacts_-_6.8\1TU3BF.pdb
c:\pdbe_split\contacts_-_6.8\1TU3BG.pdb
c:\pdbe_split\contacts_-_6.8\1TZHLV.pdb
c:\pdbe_split\contacts_-_6.8\1U0NAB.pdb
c:\pdbe_split\contacts_-_6.8\1U0NAC.pdb
c:\pdbe_split\contacts_-_6.8\1U5SAB.pdb
c:\pdbe_split\contacts_-_6.8\1UADAC.pdb
c:\pdbe_split\contacts_-_6.8\1UEXBC.pdb
c:\pdbe_split\contacts_-_6.8\1UHBAB.pdb
c:\pdbe_split\contacts_-_6.8\1UKVGY.pdb
c:\pdbe_split\contacts_-_6.8\1UPTCD.pdb
c:\pdbe_split\contacts_-_6.8\1US7AB.pdb
c:\pdbe_split\contacts_-_6.8\1UTZAB.pdb
c:\pdbe_split\contacts_-_6.8\1V1PAB.pdb
c:\pdbe_split\contacts_-_6.8\1V5IAB.pdb
c:\pdbe_split\contacts_-_6.8\1V7MLV.pdb
c:\pdbe_split\contacts_-_6.8\1V7NLV.pdb
c:\pdbe_split\contacts_-_6.8\1V7PAC.pdb
c:\pdbe_split\contacts_-_6.8\1VDGAB.pdb
c:\pdbe_split\contacts_-_6.8\1W98AB.pdb
c:\pdbe_split\contacts_-_6.8\1WA5AC.pdb
c:\pdbe_split\contacts_-_6.8\1WEJFH.pdb
c:\pdbe_split\contacts_-_6.8\1WEJFL.pdb
c:\pdbe_split\contacts_-_6.8\1WRDAB.pdb
c:\pdbe_split\contacts_-_6.8\1WVMAB.pdb
c:\pdbe_split\contacts_-_6.8\1X27BC.pdb
c:\pdbe_split\contacts_-_6.8\1X86AB.pdb
c:\pdbe_split\contacts_-_6.8\1XQSAC.pdb
c:\pdbe_split\contacts_-_6.8\1XT9AB.pdb
c:\pdbe_split\contacts_-_6.8\1Y64AB.pdb
c:\pdbe_split\contacts_-_6.8\1YC8AB.pdb
c:\pdbe_split\contacts_-_6.8\1YHNAB.pdb
c:\pdbe_split\contacts_-_6.8\1YNTDG.pdb
c:\pdbe_split\contacts_-_6.8\1YPHCD.pdb
c:\pdbe_split\contacts_-_6.8\1YPZEG.pdb
c:\pdbe_split\contacts_-_6.8\1YQVHY.pdb
c:\pdbe_split\contacts_-_6.8\1Z0JAB.pdb
c:\pdbe_split\contacts_-_6.8\1Z0KAB.pdb
c:\pdbe_split\contacts_-_6.8\1Z2CAB.pdb
c:\pdbe_split\contacts_-_6.8\1Z7KAB.pdb
c:\pdbe_split\contacts_-_6.8\1Z7Z1I.pdb
c:\pdbe_split\contacts_-_6.8\1Z7Z2I.pdb
c:\pdbe_split\contacts_-_6.8\1ZBDAB.pdb
c:\pdbe_split\contacts_-_6.8\1ZC3AD.pdb
c:\pdbe_split\contacts_-_6.8\1ZCPAB.pdb
c:\pdbe_split\contacts_-_6.8\1ZGLET.pdb
c:\pdbe_split\contacts_-_6.8\1ZHHAB.pdb
c:\pdbe_split\contacts_-_6.8\1ZOPAB.pdb
c:\pdbe_split\contacts_-_6.8\1ZY4AB.pdb
c:\pdbe_split\contacts_-_6.8\2A1AAB.pdb
c:\pdbe_split\contacts_-_6.8\2A3ZAC.pdb
c:\pdbe_split\contacts_-_6.8\2A42AB.pdb
c:\pdbe_split\contacts_-_6.8\2A45BJ.pdb
c:\pdbe_split\contacts_-_6.8\2A5FAB.pdb
c:\pdbe_split\contacts_-_6.8\2A73AB.pdb
c:\pdbe_split\contacts_-_6.8\2A78AB.pdb
c:\pdbe_split\contacts_-_6.8\2A9KAB.pdb
c:\pdbe_split\contacts_-_6.8\2ADFAL.pdb
c:\pdbe_split\contacts_-_6.8\2AEQAL.pdb
c:\pdbe_split\contacts_-_6.8\2AJFAE.pdb
c:\pdbe_split\contacts_-_6.8\2AJQAB.pdb
c:\pdbe_split\contacts_-_6.8\2AQ3AG.pdb
c:\pdbe_split\contacts_-_6.8\2AQ3EG.pdb
c:\pdbe_split\contacts_-_6.8\2ASSBC.pdb
c:\pdbe_split\contacts_-_6.8\2AXHAB.pdb
c:\pdbe_split\contacts_-_6.8\2B0ZAB.pdb
c:\pdbe_split\contacts_-_6.8\2B42AB.pdb
c:\pdbe_split\contacts_-_6.8\2BCGGY.pdb
c:\pdbe_split\contacts_-_6.8\2BCNBC.pdb
c:\pdbe_split\contacts_-_6.8\2BDNAH.pdb
c:\pdbe_split\contacts_-_6.8\2BKUAB.pdb
c:\pdbe_split\contacts_-_6.8\2BMCAB.pdb
c:\pdbe_split\contacts_-_6.8\2BOVAB.pdb
c:\pdbe_split\contacts_-_6.8\2BTCEI.pdb
c:\pdbe_split\contacts_-_6.8\2BTFAP.pdb
c:\pdbe_split\contacts_-_6.8\2BTOAT.pdb
c:\pdbe_split\contacts_-_6.8\2BTOBT.pdb
c:\pdbe_split\contacts_-_6.8\2CCIAF.pdb
c:\pdbe_split\contacts_-_6.8\2CJWAB.pdb
c:\pdbe_split\contacts_-_6.8\2D1XAB.pdb
c:\pdbe_split\contacts_-_6.8\2D26AB.pdb
c:\pdbe_split\contacts_-_6.8\2D26AC.pdb
c:\pdbe_split\contacts_-_6.8\2DCJAB.pdb
c:\pdbe_split\contacts_-_6.8\2DD8HS.pdb
c:\pdbe_split\contacts_-_6.8\2DD8LS.pdb
c:\pdbe_split\contacts_-_6.8\2DGEAD.pdb
c:\pdbe_split\contacts_-_6.8\2E9VAB.pdb
c:\pdbe_split\contacts_-_6.8\2EIZAC.pdb
c:\pdbe_split\contacts_-_6.8\2F2CAB.pdb
c:\pdbe_split\contacts_-_6.8\2F49AC.pdb
c:\pdbe_split\contacts_-_6.8\2FD6HU.pdb
c:\pdbe_split\contacts_-_6.8\2FF6AH.pdb
c:\pdbe_split\contacts_-_6.8\2FJUAB.pdb
c:\pdbe_split\contacts_-_6.8\2FU5AD.pdb
c:\pdbe_split\contacts_-_6.8\2G77AB.pdb
c:\pdbe_split\contacts_-_6.8\2G9HBD.pdb
c:\pdbe_split\contacts_-_6.8\2GAFAD.pdb
c:\pdbe_split\contacts_-_6.8\2GHWAB.pdb
c:\pdbe_split\contacts_-_6.8\2GOMAB.pdb
c:\pdbe_split\contacts_-_6.8\2GOXAB.pdb
c:\pdbe_split\contacts_-_6.8\2GTPAD.pdb
c:\pdbe_split\contacts_-_6.8\2GYUAB.pdb
c:\pdbe_split\contacts_-_6.8\2H7CCE.pdb
c:\pdbe_split\contacts_-_6.8\2H7VAC.pdb
c:\pdbe_split\contacts_-_6.8\2H9ECH.pdb
c:\pdbe_split\contacts_-_6.8\2H9GAR.pdb
c:\pdbe_split\contacts_-_6.8\2H9GBR.pdb
c:\pdbe_split\contacts_-_6.8\2HEIAB.pdb
c:\pdbe_split\contacts_-_6.8\2HI9AB.pdb
c:\pdbe_split\contacts_-_6.8\2HI9AC.pdb
c:\pdbe_split\contacts_-_6.8\2HI9BC.pdb
c:\pdbe_split\contacts_-_6.8\2HJ9AC.pdb
c:\pdbe_split\contacts_-_6.8\2HJ9CD.pdb
c:\pdbe_split\contacts_-_6.8\2HRKAB.pdb
c:\pdbe_split\contacts_-_6.8\2HSNAB.pdb
c:\pdbe_split\contacts_-_6.8\2HV8AD.pdb
c:\pdbe_split\contacts_-_6.8\2I9LBI.pdb
c:\pdbe_split\contacts_-_6.8\2ICWGJ.pdb
c:\pdbe_split\contacts_-_6.8\2ID5AB.pdb
c:\pdbe_split\contacts_-_6.8\2IJ0BC.pdb
c:\pdbe_split\contacts_-_6.8\2IJ0CE.pdb
c:\pdbe_split\contacts_-_6.8\2ILNAB.pdb
c:\pdbe_split\contacts_-_6.8\2IPAAB.pdb
c:\pdbe_split\contacts_-_6.8\2IWTAB.pdb
c:\pdbe_split\contacts_-_6.8\2J06AB.pdb
c:\pdbe_split\contacts_-_6.8\2J0MAB.pdb
c:\pdbe_split\contacts_-_6.8\2J4WDH.pdb
c:\pdbe_split\contacts_-_6.8\2J6EAH.pdb
c:\pdbe_split\contacts_-_6.8\2J7QAB.pdb
c:\pdbe_split\contacts_-_6.8\2J88AL.pdb
c:\pdbe_split\contacts_-_6.8\2JELHP.pdb
c:\pdbe_split\contacts_-_6.8\2JIXCF.pdb
c:\pdbe_split\contacts_-_6.8\2JIXCG.pdb
c:\pdbe_split\contacts_-_6.8\2JIXEG.pdb
c:\pdbe_split\contacts_-_6.8\2JKIAS.pdb
c:\pdbe_split\contacts_-_6.8\2K8BAB.pdb
c:\pdbe_split\contacts_-_6.8\2KDFAC.pdb
c:\pdbe_split\contacts_-_6.8\2KH2AB.pdb
c:\pdbe_split\contacts_-_6.8\2KI6EF.pdb
c:\pdbe_split\contacts_-_6.8\2KWIAB.pdb
c:\pdbe_split\contacts_-_6.8\2L00AB.pdb
c:\pdbe_split\contacts_-_6.8\2L0TAB.pdb
c:\pdbe_split\contacts_-_6.8\2LXCAB.pdb
c:\pdbe_split\contacts_-_6.8\2LXCAC.pdb
c:\pdbe_split\contacts_-_6.8\2M32AB.pdb
c:\pdbe_split\contacts_-_6.8\2M32AC.pdb
c:\pdbe_split\contacts_-_6.8\2MBQAB.pdb
c:\pdbe_split\contacts_-_6.8\2MCNAB.pdb
c:\pdbe_split\contacts_-_6.8\2MP0AB.pdb
c:\pdbe_split\contacts_-_6.8\2MQSAD.pdb
c:\pdbe_split\contacts_-_6.8\2MREAB.pdb
c:\pdbe_split\contacts_-_6.8\2MSEBD.pdb
c:\pdbe_split\contacts_-_6.8\2N0SAB.pdb
c:\pdbe_split\contacts_-_6.8\2N3UBC.pdb
c:\pdbe_split\contacts_-_6.8\2N3WAC.pdb
c:\pdbe_split\contacts_-_6.8\2N8RAB.pdb
c:\pdbe_split\contacts_-_6.8\2N8RAC.pdb
c:\pdbe_split\contacts_-_6.8\2NGRAB.pdb
c:\pdbe_split\contacts_-_6.8\2NOJEF.pdb
c:\pdbe_split\contacts_-_6.8\2NR6AC.pdb
c:\pdbe_split\contacts_-_6.8\2NTYAC.pdb
c:\pdbe_split\contacts_-_6.8\2NTYAD.pdb
c:\pdbe_split\contacts_-_6.8\2NTYBC.pdb
c:\pdbe_split\contacts_-_6.8\2NTYBD.pdb
c:\pdbe_split\contacts_-_6.8\2NZ8AB.pdb
c:\pdbe_split\contacts_-_6.8\2O3BAB.pdb
c:\pdbe_split\contacts_-_6.8\2O6VAC.pdb
c:\pdbe_split\contacts_-_6.8\2O8VAB.pdb
c:\pdbe_split\contacts_-_6.8\2OCYAC.pdb
c:\pdbe_split\contacts_-_6.8\2ODBAB.pdb
c:\pdbe_split\contacts_-_6.8\2ODYBE.pdb
c:\pdbe_split\contacts_-_6.8\2OE1AB.pdb
c:\pdbe_split\contacts_-_6.8\2OFVAB.pdb
c:\pdbe_split\contacts_-_6.8\2OOBAB.pdb
c:\pdbe_split\contacts_-_6.8\2OT3AB.pdb
c:\pdbe_split\contacts_-_6.8\2OZRAB.pdb
c:\pdbe_split\contacts_-_6.8\2OZRBD.pdb
c:\pdbe_split\contacts_-_6.8\2OZRBH.pdb
c:\pdbe_split\contacts_-_6.8\2OZRGH.pdb
c:\pdbe_split\contacts_-_6.8\2P1YAC.pdb
c:\pdbe_split\contacts_-_6.8\2P2LAB.pdb
c:\pdbe_split\contacts_-_6.8\2P2LAC.pdb
c:\pdbe_split\contacts_-_6.8\2P43AB.pdb
c:\pdbe_split\contacts_-_6.8\2P48AB.pdb
c:\pdbe_split\contacts_-_6.8\2P4RAT.pdb
c:\pdbe_split\contacts_-_6.8\2P5SAB.pdb
c:\pdbe_split\contacts_-_6.8\2P6AAD.pdb
c:\pdbe_split\contacts_-_6.8\2PAVAP.pdb
c:\pdbe_split\contacts_-_6.8\2PBDPV.pdb
c:\pdbe_split\contacts_-_6.8\2PBIAB.pdb
c:\pdbe_split\contacts_-_6.8\2PCBAB.pdb
c:\pdbe_split\contacts_-_6.8\2PCCAB.pdb
c:\pdbe_split\contacts_-_6.8\2PE9AB.pdb
c:\pdbe_split\contacts_-_6.8\2PM8AB.pdb
c:\pdbe_split\contacts_-_6.8\2PUKAC.pdb
c:\pdbe_split\contacts_-_6.8\2PVOAD.pdb
c:\pdbe_split\contacts_-_6.8\2Q8BAL.pdb
c:\pdbe_split\contacts_-_6.8\2QEJAD.pdb
c:\pdbe_split\contacts_-_6.8\2QEJBC.pdb
c:\pdbe_split\contacts_-_6.8\2QJ4AB.pdb
c:\pdbe_split\contacts_-_6.8\2QQNAL.pdb
c:\pdbe_split\contacts_-_6.8\2QRZAB.pdb
c:\pdbe_split\contacts_-_6.8\2QXLAB.pdb
c:\pdbe_split\contacts_-_6.8\2QY0AD.pdb
c:\pdbe_split\contacts_-_6.8\2QYIAB.pdb
c:\pdbe_split\contacts_-_6.8\2QZTAB.pdb
c:\pdbe_split\contacts_-_6.8\2R33AB.pdb
c:\pdbe_split\contacts_-_6.8\2R4SAH.pdb
c:\pdbe_split\contacts_-_6.8\2R56AL.pdb
c:\pdbe_split\contacts_-_6.8\2RA3AB.pdb
c:\pdbe_split\contacts_-_6.8\2RFDAC.pdb
c:\pdbe_split\contacts_-_6.8\2RGNBC.pdb
c:\pdbe_split\contacts_-_6.8\2RMAAM.pdb
c:\pdbe_split\contacts_-_6.8\2RMACE.pdb
c:\pdbe_split\contacts_-_6.8\2RMKAB.pdb
c:\pdbe_split\contacts_-_6.8\2SEBAD.pdb
c:\pdbe_split\contacts_-_6.8\2SICEI.pdb
c:\pdbe_split\contacts_-_6.8\2UV3AB.pdb
c:\pdbe_split\contacts_-_6.8\2UWEAF.pdb
c:\pdbe_split\contacts_-_6.8\2UZIHR.pdb
c:\pdbe_split\contacts_-_6.8\2V51BF.pdb
c:\pdbe_split\contacts_-_6.8\2V55AB.pdb
c:\pdbe_split\contacts_-_6.8\2VDBAB.pdb
c:\pdbe_split\contacts_-_6.8\2VH5HR.pdb
c:\pdbe_split\contacts_-_6.8\2VWIAC.pdb
c:\pdbe_split\contacts_-_6.8\2VWICD.pdb
c:\pdbe_split\contacts_-_6.8\2VXQAL.pdb
c:\pdbe_split\contacts_-_6.8\2VXTHI.pdb
c:\pdbe_split\contacts_-_6.8\2VYRBF.pdb
c:\pdbe_split\contacts_-_6.8\2VYRFH.pdb
c:\pdbe_split\contacts_-_6.8\2VYRFI.pdb
c:\pdbe_split\contacts_-_6.8\2W2OAE.pdb
c:\pdbe_split\contacts_-_6.8\2W83AC.pdb
c:\pdbe_split\contacts_-_6.8\2W83AD.pdb
c:\pdbe_split\contacts_-_6.8\2W83BD.pdb
c:\pdbe_split\contacts_-_6.8\2W9EAL.pdb
c:\pdbe_split\contacts_-_6.8\2W9LKX.pdb
c:\pdbe_split\contacts_-_6.8\2WBLAC.pdb
c:\pdbe_split\contacts_-_6.8\2WBLAD.pdb
c:\pdbe_split\contacts_-_6.8\2WBLBC.pdb
c:\pdbe_split\contacts_-_6.8\2WBWAB.pdb
c:\pdbe_split\contacts_-_6.8\2WIHAB.pdb
c:\pdbe_split\contacts_-_6.8\2WP9AB.pdb
c:\pdbe_split\contacts_-_6.8\2WTKDF.pdb
c:\pdbe_split\contacts_-_6.8\2WTKEF.pdb
c:\pdbe_split\contacts_-_6.8\2WUBCQ.pdb
c:\pdbe_split\contacts_-_6.8\2WUBCR.pdb
c:\pdbe_split\contacts_-_6.8\2WUCAB.pdb
c:\pdbe_split\contacts_-_6.8\2WV7AB.pdb
c:\pdbe_split\contacts_-_6.8\2WWTAB.pdb
c:\pdbe_split\contacts_-_6.8\2WY7AQ.pdb
c:\pdbe_split\contacts_-_6.8\2WY8AQ.pdb
c:\pdbe_split\contacts_-_6.8\2WZPDG.pdb
c:\pdbe_split\contacts_-_6.8\2X0GAB.pdb
c:\pdbe_split\contacts_-_6.8\2X19AB.pdb
c:\pdbe_split\contacts_-_6.8\2X2AAB.pdb
c:\pdbe_split\contacts_-_6.8\2XCMAE.pdb
c:\pdbe_split\contacts_-_6.8\2XGGAB.pdb
c:\pdbe_split\contacts_-_6.8\2XQBAL.pdb
c:\pdbe_split\contacts_-_6.8\2XQWAC.pdb
c:\pdbe_split\contacts_-_6.8\2XTJAD.pdb
c:\pdbe_split\contacts_-_6.8\2XTTAB.pdb
c:\pdbe_split\contacts_-_6.8\2XV6CD.pdb
c:\pdbe_split\contacts_-_6.8\2XWBFJ.pdb
c:\pdbe_split\contacts_-_6.8\2XWTAC.pdb
c:\pdbe_split\contacts_-_6.8\2Y4IBC.pdb
c:\pdbe_split\contacts_-_6.8\2Y7JAD.pdb
c:\pdbe_split\contacts_-_6.8\2YC4CD.pdb
c:\pdbe_split\contacts_-_6.8\2YK3AC.pdb
c:\pdbe_split\contacts_-_6.8\2YVJAB.pdb
c:\pdbe_split\contacts_-_6.8\2Z59AB.pdb
c:\pdbe_split\contacts_-_6.8\2Z6WAB.pdb
c:\pdbe_split\contacts_-_6.8\2ZVNAB.pdb
c:\pdbe_split\contacts_-_6.8\2ZVOBG.pdb
c:\pdbe_split\contacts_-_6.8\3A2CJL.pdb
c:\pdbe_split\contacts_-_6.8\3A58AB.pdb
c:\pdbe_split\contacts_-_6.8\3A6PAC.pdb
c:\pdbe_split\contacts_-_6.8\3AB0AB.pdb
c:\pdbe_split\contacts_-_6.8\3AULAB.pdb
c:\pdbe_split\contacts_-_6.8\3B08AB.pdb
c:\pdbe_split\contacts_-_6.8\3B0AAB.pdb
c:\pdbe_split\contacts_-_6.8\3B2TAB.pdb
c:\pdbe_split\contacts_-_6.8\3B2UAH.pdb
c:\pdbe_split\contacts_-_6.8\3B3XAB.pdb
c:\pdbe_split\contacts_-_6.8\3B5UBD.pdb
c:\pdbe_split\contacts_-_6.8\3B5UDF.pdb
c:\pdbe_split\contacts_-_6.8\3B63FH.pdb
c:\pdbe_split\contacts_-_6.8\3BC1AB.pdb
c:\pdbe_split\contacts_-_6.8\3BCMAB.pdb
c:\pdbe_split\contacts_-_6.8\3BG4BD.pdb
c:\pdbe_split\contacts_-_6.8\3BIWAD.pdb
c:\pdbe_split\contacts_-_6.8\3BIWAE.pdb
c:\pdbe_split\contacts_-_6.8\3BJIAD.pdb
c:\pdbe_split\contacts_-_6.8\3BQUBD.pdb
c:\pdbe_split\contacts_-_6.8\3BT2HU.pdb
c:\pdbe_split\contacts_-_6.8\3BX1AC.pdb
c:\pdbe_split\contacts_-_6.8\3BYHAB.pdb
c:\pdbe_split\contacts_-_6.8\3C6LDH.pdb
c:\pdbe_split\contacts_-_6.8\3C6LFG.pdb
c:\pdbe_split\contacts_-_6.8\3CH5AB.pdb
c:\pdbe_split\contacts_-_6.8\3CHNJS.pdb
c:\pdbe_split\contacts_-_6.8\3CPHAG.pdb
c:\pdbe_split\contacts_-_6.8\3CPJBG.pdb
c:\pdbe_split\contacts_-_6.8\3CRKAB.pdb
c:\pdbe_split\contacts_-_6.8\3CUEAF.pdb
c:\pdbe_split\contacts_-_6.8\3CUPAB.pdb
c:\pdbe_split\contacts_-_6.8\3CVHAL.pdb
c:\pdbe_split\contacts_-_6.8\3CWZAB.pdb
c:\pdbe_split\contacts_-_6.8\3CX5PV.pdb
c:\pdbe_split\contacts_-_6.8\3D0EAB.pdb
c:\pdbe_split\contacts_-_6.8\3D5SAC.pdb
c:\pdbe_split\contacts_-_6.8\3D7TAB.pdb
c:\pdbe_split\contacts_-_6.8\3DAKAB.pdb
c:\pdbe_split\contacts_-_6.8\3DPUAB.pdb
c:\pdbe_split\contacts_-_6.8\3DQVAC.pdb
c:\pdbe_split\contacts_-_6.8\3DVGAX.pdb
c:\pdbe_split\contacts_-_6.8\3DVNBY.pdb
c:\pdbe_split\contacts_-_6.8\3DWLAD.pdb
c:\pdbe_split\contacts_-_6.8\3DXBAF.pdb
c:\pdbe_split\contacts_-_6.8\3E5AAB.pdb
c:\pdbe_split\contacts_-_6.8\3EG1AB.pdb
c:\pdbe_split\contacts_-_6.8\3EGSAC.pdb
c:\pdbe_split\contacts_-_6.8\3EHBBD.pdb
c:\pdbe_split\contacts_-_6.8\3EOBHI.pdb
c:\pdbe_split\contacts_-_6.8\3EXUAB.pdb
c:\pdbe_split\contacts_-_6.8\3EYVAL.pdb
c:\pdbe_split\contacts_-_6.8\3F1SAB.pdb
c:\pdbe_split\contacts_-_6.8\3F7VAC.pdb
c:\pdbe_split\contacts_-_6.8\3FMGAH.pdb
c:\pdbe_split\contacts_-_6.8\3FP7EJ.pdb
c:\pdbe_split\contacts_-_6.8\3FZKAB.pdb
c:\pdbe_split\contacts_-_6.8\3G04AC.pdb
c:\pdbe_split\contacts_-_6.8\3G37OQ.pdb
c:\pdbe_split\contacts_-_6.8\3GCGAB.pdb
c:\pdbe_split\contacts_-_6.8\3GCWAE.pdb
c:\pdbe_split\contacts_-_6.8\3GJEBH.pdb
c:\pdbe_split\contacts_-_6.8\3GJFAL.pdb
c:\pdbe_split\contacts_-_6.8\3GJXAC.pdb
c:\pdbe_split\contacts_-_6.8\3GMWAB.pdb
c:\pdbe_split\contacts_-_6.8\3GRWAH.pdb
c:\pdbe_split\contacts_-_6.8\3H9RAB.pdb
c:\pdbe_split\contacts_-_6.8\3HAEAL.pdb
c:\pdbe_split\contacts_-_6.8\3HEKAB.pdb
c:\pdbe_split\contacts_-_6.8\3HI6BX.pdb
c:\pdbe_split\contacts_-_6.8\3HRZBD.pdb
c:\pdbe_split\contacts_-_6.8\3HRZCD.pdb
c:\pdbe_split\contacts_-_6.8\3I6UAB.pdb
c:\pdbe_split\contacts_-_6.8\3IECAE.pdb
c:\pdbe_split\contacts_-_6.8\3IHPAC.pdb
c:\pdbe_split\contacts_-_6.8\3INBBC.pdb
c:\pdbe_split\contacts_-_6.8\3IS5CD.pdb
c:\pdbe_split\contacts_-_6.8\3IS5CE.pdb
c:\pdbe_split\contacts_-_6.8\3IU3BK.pdb
c:\pdbe_split\contacts_-_6.8\3IUCAC.pdb
c:\pdbe_split\contacts_-_6.8\3IYPCF.pdb
c:\pdbe_split\contacts_-_6.8\3J2TAH.pdb
c:\pdbe_split\contacts_-_6.8\3J42HI.pdb
c:\pdbe_split\contacts_-_6.8\3J4KAB.pdb
c:\pdbe_split\contacts_-_6.8\3J70SU.pdb
c:\pdbe_split\contacts_-_6.8\3J81Fj.pdb
c:\pdbe_split\contacts_-_6.8\3J82AB.pdb
c:\pdbe_split\contacts_-_6.8\3J8JFH.pdb
c:\pdbe_split\contacts_-_6.8\3J8WDH.pdb
c:\pdbe_split\contacts_-_6.8\3JBF17.pdb
c:\pdbe_split\contacts_-_6.8\3JBF27.pdb
c:\pdbe_split\contacts_-_6.8\3JBG17.pdb
c:\pdbe_split\contacts_-_6.8\3JBIAV.pdb
c:\pdbe_split\contacts_-_6.8\3JBIBV.pdb
c:\pdbe_split\contacts_-_6.8\3JBTAB.pdb
c:\pdbe_split\contacts_-_6.8\3JS2AB.pdb
c:\pdbe_split\contacts_-_6.8\3JWOAL.pdb
c:\pdbe_split\contacts_-_6.8\3K1KAC.pdb
c:\pdbe_split\contacts_-_6.8\3K65AB.pdb
c:\pdbe_split\contacts_-_6.8\3K75BD.pdb
c:\pdbe_split\contacts_-_6.8\3K9BAC.pdb
c:\pdbe_split\contacts_-_6.8\3KPRDE.pdb
c:\pdbe_split\contacts_-_6.8\3KR3DL.pdb
c:\pdbe_split\contacts_-_6.8\3KZ1AF.pdb
c:\pdbe_split\contacts_-_6.8\3L5XAH.pdb
c:\pdbe_split\contacts_-_6.8\3L7ZCI.pdb
c:\pdbe_split\contacts_-_6.8\3L7ZHI.pdb
c:\pdbe_split\contacts_-_6.8\3L89BN.pdb
c:\pdbe_split\contacts_-_6.8\3LB8AC.pdb
c:\pdbe_split\contacts_-_6.8\3LHPHS.pdb
c:\pdbe_split\contacts_-_6.8\3LUEDO.pdb
c:\pdbe_split\contacts_-_6.8\3LUEHS.pdb
c:\pdbe_split\contacts_-_6.8\3LXFAB.pdb
c:\pdbe_split\contacts_-_6.8\3LXFAD.pdb
c:\pdbe_split\contacts_-_6.8\3M7FAB.pdb
c:\pdbe_split\contacts_-_6.8\3MJ9AH.pdb
c:\pdbe_split\contacts_-_6.8\3MXWAL.pdb
c:\pdbe_split\contacts_-_6.8\3N9YAC.pdb
c:\pdbe_split\contacts_-_6.8\3NBTEF.pdb
c:\pdbe_split\contacts_-_6.8\3NBYAC.pdb
c:\pdbe_split\contacts_-_6.8\3NC0AC.pdb
c:\pdbe_split\contacts_-_6.8\3NFPAK.pdb
c:\pdbe_split\contacts_-_6.8\3NFSHL.pdb
c:\pdbe_split\contacts_-_6.8\3NI6AB.pdb
c:\pdbe_split\contacts_-_6.8\3NS8AB.pdb
c:\pdbe_split\contacts_-_6.8\3O0GAD.pdb
c:\pdbe_split\contacts_-_6.8\3O65AB.pdb
c:\pdbe_split\contacts_-_6.8\3O71AB.pdb
c:\pdbe_split\contacts_-_6.8\3OEDAC.pdb
c:\pdbe_split\contacts_-_6.8\3OJ3AI.pdb
c:\pdbe_split\contacts_-_6.8\3OURAB.pdb
c:\pdbe_split\contacts_-_6.8\3OWEAB.pdb
c:\pdbe_split\contacts_-_6.8\3OZ6AB.pdb
c:\pdbe_split\contacts_-_6.8\3P6ZBC.pdb
c:\pdbe_split\contacts_-_6.8\3PCRAB.pdb
c:\pdbe_split\contacts_-_6.8\3PVBAB.pdb
c:\pdbe_split\contacts_-_6.8\3PXKAB.pdb
c:\pdbe_split\contacts_-_6.8\3Q3GCG.pdb
c:\pdbe_split\contacts_-_6.8\3Q3GDG.pdb
c:\pdbe_split\contacts_-_6.8\3Q4UAB.pdb
c:\pdbe_split\contacts_-_6.8\3Q4ZAB.pdb
c:\pdbe_split\contacts_-_6.8\3QBTAB.pdb
c:\pdbe_split\contacts_-_6.8\3QEQDE.pdb
c:\pdbe_split\contacts_-_6.8\3QMLAC.pdb
c:\pdbe_split\contacts_-_6.8\3QPXHL.pdb
c:\pdbe_split\contacts_-_6.8\3QTLBC.pdb
c:\pdbe_split\contacts_-_6.8\3QUMAP.pdb
c:\pdbe_split\contacts_-_6.8\3QUMBP.pdb
c:\pdbe_split\contacts_-_6.8\3QUMLP.pdb
c:\pdbe_split\contacts_-_6.8\3QWOLP.pdb
c:\pdbe_split\contacts_-_6.8\3QXUBD.pdb
c:\pdbe_split\contacts_-_6.8\3R8BAB.pdb
c:\pdbe_split\contacts_-_6.8\3RAJAL.pdb
c:\pdbe_split\contacts_-_6.8\3RDZAC.pdb
c:\pdbe_split\contacts_-_6.8\3RHWFN.pdb
c:\pdbe_split\contacts_-_6.8\3RI5IO.pdb
c:\pdbe_split\contacts_-_6.8\3RIFIO.pdb
c:\pdbe_split\contacts_-_6.8\3RT3BC.pdb
c:\pdbe_split\contacts_-_6.8\3RYTAC.pdb
c:\pdbe_split\contacts_-_6.8\3S37HX.pdb
c:\pdbe_split\contacts_-_6.8\3S97AC.pdb
c:\pdbe_split\contacts_-_6.8\3SDLAD.pdb
c:\pdbe_split\contacts_-_6.8\3SE8GL.pdb
c:\pdbe_split\contacts_-_6.8\3SFVAB.pdb
c:\pdbe_split\contacts_-_6.8\3SGBEI.pdb
c:\pdbe_split\contacts_-_6.8\3SGQEI.pdb
c:\pdbe_split\contacts_-_6.8\3SJHAB.pdb
c:\pdbe_split\contacts_-_6.8\3SJVAD.pdb
c:\pdbe_split\contacts_-_6.8\3SJVDN.pdb
c:\pdbe_split\contacts_-_6.8\3SJVEF.pdb
c:\pdbe_split\contacts_-_6.8\3SO3AB.pdb
c:\pdbe_split\contacts_-_6.8\3SQOAH.pdb
c:\pdbe_split\contacts_-_6.8\3SQOAL.pdb
c:\pdbe_split\contacts_-_6.8\3STBAD.pdb
c:\pdbe_split\contacts_-_6.8\3STLAC.pdb
c:\pdbe_split\contacts_-_6.8\3SU8AX.pdb
c:\pdbe_split\contacts_-_6.8\3SUAAD.pdb
c:\pdbe_split\contacts_-_6.8\3T2NAH.pdb
c:\pdbe_split\contacts_-_6.8\3T2NAL.pdb
c:\pdbe_split\contacts_-_6.8\3TACAB.pdb
c:\pdbe_split\contacts_-_6.8\3TECEI.pdb
c:\pdbe_split\contacts_-_6.8\3TEIAB.pdb
c:\pdbe_split\contacts_-_6.8\3THMFL.pdb
c:\pdbe_split\contacts_-_6.8\3TNFAB.pdb
c:\pdbe_split\contacts_-_6.8\3TW8AB.pdb
c:\pdbe_split\contacts_-_6.8\3U30AB.pdb
c:\pdbe_split\contacts_-_6.8\3U30AC.pdb
c:\pdbe_split\contacts_-_6.8\3U9PCM.pdb
c:\pdbe_split\contacts_-_6.8\3UAJBC.pdb
c:\pdbe_split\contacts_-_6.8\3ULRAB.pdb
c:\pdbe_split\contacts_-_6.8\3UX9AB.pdb
c:\pdbe_split\contacts_-_6.8\3V0AAC.pdb
c:\pdbe_split\contacts_-_6.8\3V7AAE.pdb
c:\pdbe_split\contacts_-_6.8\3VG9AB.pdb
c:\pdbe_split\contacts_-_6.8\3VGAAB.pdb
c:\pdbe_split\contacts_-_6.8\3VHLAB.pdb
c:\pdbe_split\contacts_-_6.8\3VHTBC.pdb
c:\pdbe_split\contacts_-_6.8\3VI4BF.pdb
c:\pdbe_split\contacts_-_6.8\3VUYAD.pdb
c:\pdbe_split\contacts_-_6.8\3VV3AB.pdb
c:\pdbe_split\contacts_-_6.8\3VW6AB.pdb
c:\pdbe_split\contacts_-_6.8\3W2DAH.pdb
c:\pdbe_split\contacts_-_6.8\3WD5AL.pdb
c:\pdbe_split\contacts_-_6.8\3WEEAB.pdb
c:\pdbe_split\contacts_-_6.8\3WFCCL.pdb
c:\pdbe_split\contacts_-_6.8\3WHE1I.pdb
c:\pdbe_split\contacts_-_6.8\3WWQAH.pdb
c:\pdbe_split\contacts_-_6.8\3WXEAB.pdb
c:\pdbe_split\contacts_-_6.8\3WYFAC.pdb
c:\pdbe_split\contacts_-_6.8\3WYGAC.pdb
c:\pdbe_split\contacts_-_6.8\3ZBWAB.pdb
c:\pdbe_split\contacts_-_6.8\3ZHDAB.pdb
c:\pdbe_split\contacts_-_6.8\3ZHPAD.pdb
c:\pdbe_split\contacts_-_6.8\3ZKXAB.pdb
c:\pdbe_split\contacts_-_6.8\3ZKXAC.pdb
c:\pdbe_split\contacts_-_6.8\3ZLQAC.pdb
c:\pdbe_split\contacts_-_6.8\3ZRLAB.pdb
c:\pdbe_split\contacts_-_6.8\3ZTNHL.pdb
c:\pdbe_split\contacts_-_6.8\3ZXHAB.pdb
c:\pdbe_split\contacts_-_6.8\4A5WAB.pdb
c:\pdbe_split\contacts_-_6.8\4A63AB.pdb
c:\pdbe_split\contacts_-_6.8\4A7FEF.pdb
c:\pdbe_split\contacts_-_6.8\4A7HAI.pdb
c:\pdbe_split\contacts_-_6.8\4A7HCE.pdb
c:\pdbe_split\contacts_-_6.8\4A7LEF.pdb
c:\pdbe_split\contacts_-_6.8\4A7NAB.pdb
c:\pdbe_split\contacts_-_6.8\4A7NAC.pdb
c:\pdbe_split\contacts_-_6.8\4ACQAD.pdb
c:\pdbe_split\contacts_-_6.8\4ACQBC.pdb
c:\pdbe_split\contacts_-_6.8\4AF3AD.pdb
c:\pdbe_split\contacts_-_6.8\4AFSAC.pdb
c:\pdbe_split\contacts_-_6.8\4AG1AC.pdb
c:\pdbe_split\contacts_-_6.8\4APQCD.pdb
c:\pdbe_split\contacts_-_6.8\4AT6AB.pdb
c:\pdbe_split\contacts_-_6.8\4AWOAB.pdb
c:\pdbe_split\contacts_-_6.8\4B1YBM.pdb
c:\pdbe_split\contacts_-_6.8\4BBNAC.pdb
c:\pdbe_split\contacts_-_6.8\4BKLAG.pdb
c:\pdbe_split\contacts_-_6.8\4BOSBC.pdb
c:\pdbe_split\contacts_-_6.8\4BVXAB.pdb
c:\pdbe_split\contacts_-_6.8\4BWGAE.pdb
c:\pdbe_split\contacts_-_6.8\4BXSAV.pdb
c:\pdbe_split\contacts_-_6.8\4BZ1AH.pdb
c:\pdbe_split\contacts_-_6.8\4C2VAD.pdb
c:\pdbe_split\contacts_-_6.8\4C56GK.pdb
c:\pdbe_split\contacts_-_6.8\4C56HI.pdb
c:\pdbe_split\contacts_-_6.8\4C57AC.pdb
c:\pdbe_split\contacts_-_6.8\4C59AB.pdb
c:\pdbe_split\contacts_-_6.8\4CC4AB.pdb
c:\pdbe_split\contacts_-_6.8\4CDGAC.pdb
c:\pdbe_split\contacts_-_6.8\4CHABG.pdb
c:\pdbe_split\contacts_-_6.8\4CKDBK.pdb
c:\pdbe_split\contacts_-_6.8\4CVWAC.pdb
c:\pdbe_split\contacts_-_6.8\4CYMAD.pdb
c:\pdbe_split\contacts_-_6.8\4CZ2AE.pdb
c:\pdbe_split\contacts_-_6.8\4CZ2BD.pdb
c:\pdbe_split\contacts_-_6.8\4CZ2BE.pdb
c:\pdbe_split\contacts_-_6.8\4D0NAB.pdb
c:\pdbe_split\contacts_-_6.8\4D5KAB.pdb
c:\pdbe_split\contacts_-_6.8\4D9QAH.pdb
c:\pdbe_split\contacts_-_6.8\4DC2AZ.pdb
c:\pdbe_split\contacts_-_6.8\4DDGAG.pdb
c:\pdbe_split\contacts_-_6.8\4DEPAC.pdb
c:\pdbe_split\contacts_-_6.8\4DIDAB.pdb
c:\pdbe_split\contacts_-_6.8\4DKEAI.pdb
c:\pdbe_split\contacts_-_6.8\4DN4LM.pdb
c:\pdbe_split\contacts_-_6.8\4DOQAB.pdb
c:\pdbe_split\contacts_-_6.8\4DVRGL.pdb
c:\pdbe_split\contacts_-_6.8\4DW2LU.pdb
c:\pdbe_split\contacts_-_6.8\4E05HI.pdb
c:\pdbe_split\contacts_-_6.8\4E0SAB.pdb
c:\pdbe_split\contacts_-_6.8\4E41BD.pdb
c:\pdbe_split\contacts_-_6.8\4E6DAB.pdb
c:\pdbe_split\contacts_-_6.8\4EAHAD.pdb
c:\pdbe_split\contacts_-_6.8\4EIGAB.pdb
c:\pdbe_split\contacts_-_6.8\4EMJAB.pdb
c:\pdbe_split\contacts_-_6.8\4ETQCL.pdb
c:\pdbe_split\contacts_-_6.8\4F15AB.pdb
c:\pdbe_split\contacts_-_6.8\4F38AB.pdb
c:\pdbe_split\contacts_-_6.8\4F3FAC.pdb
c:\pdbe_split\contacts_-_6.8\4FMEAB.pdb
c:\pdbe_split\contacts_-_6.8\4FMEAC.pdb
c:\pdbe_split\contacts_-_6.8\4FXKBC.pdb
c:\pdbe_split\contacts_-_6.8\4FZAAB.pdb
c:\pdbe_split\contacts_-_6.8\4G01AB.pdb
c:\pdbe_split\contacts_-_6.8\4G0DAW.pdb
c:\pdbe_split\contacts_-_6.8\4G3XAB.pdb
c:\pdbe_split\contacts_-_6.8\4G6JAH.pdb
c:\pdbe_split\contacts_-_6.8\4G6MAH.pdb
c:\pdbe_split\contacts_-_6.8\4G6MAL.pdb
c:\pdbe_split\contacts_-_6.8\4G7VLS.pdb
c:\pdbe_split\contacts_-_6.8\4GG6AB.pdb
c:\pdbe_split\contacts_-_6.8\4GI3AC.pdb
c:\pdbe_split\contacts_-_6.8\4GMSEJ.pdb
c:\pdbe_split\contacts_-_6.8\4GMSIM.pdb
c:\pdbe_split\contacts_-_6.8\4GXUAN.pdb
c:\pdbe_split\contacts_-_6.8\4H1LAJ.pdb
c:\pdbe_split\contacts_-_6.8\4H49AB.pdb
c:\pdbe_split\contacts_-_6.8\4H5SAB.pdb
c:\pdbe_split\contacts_-_6.8\4H8WGH.pdb
c:\pdbe_split\contacts_-_6.8\4H8WGL.pdb
c:\pdbe_split\contacts_-_6.8\4HAVAC.pdb
c:\pdbe_split\contacts_-_6.8\4HAWAC.pdb
c:\pdbe_split\contacts_-_6.8\4HAXAC.pdb
c:\pdbe_split\contacts_-_6.8\4HAZAC.pdb
c:\pdbe_split\contacts_-_6.8\4HG4BL.pdb
c:\pdbe_split\contacts_-_6.8\4HGKAD.pdb
c:\pdbe_split\contacts_-_6.8\4HIXAH.pdb
c:\pdbe_split\contacts_-_6.8\4HJ0AP.pdb
c:\pdbe_split\contacts_-_6.8\4HKZBH.pdb
c:\pdbe_split\contacts_-_6.8\4HLQAB.pdb
c:\pdbe_split\contacts_-_6.8\4HT1HT.pdb
c:\pdbe_split\contacts_-_6.8\4HTCHI.pdb
c:\pdbe_split\contacts_-_6.8\4HWBAH.pdb
c:\pdbe_split\contacts_-_6.8\4HWBAL.pdb
c:\pdbe_split\contacts_-_6.8\4HWUAB.pdb
c:\pdbe_split\contacts_-_6.8\4HX2AB.pdb
c:\pdbe_split\contacts_-_6.8\4HX3AB.pdb
c:\pdbe_split\contacts_-_6.8\4I1OAD.pdb
c:\pdbe_split\contacts_-_6.8\4I2XBE.pdb
c:\pdbe_split\contacts_-_6.8\4IDTAB.pdb
c:\pdbe_split\contacts_-_6.8\4II3AB.pdb
c:\pdbe_split\contacts_-_6.8\4IJ3AC.pdb
c:\pdbe_split\contacts_-_6.8\4ILWAD.pdb
c:\pdbe_split\contacts_-_6.8\4IOIAE.pdb
c:\pdbe_split\contacts_-_6.8\4IOIBH.pdb
c:\pdbe_split\contacts_-_6.8\4IOSAE.pdb
c:\pdbe_split\contacts_-_6.8\4IRUAB.pdb
c:\pdbe_split\contacts_-_6.8\4ITRAD.pdb
c:\pdbe_split\contacts_-_6.8\4ITRBC.pdb
c:\pdbe_split\contacts_-_6.8\4J23AB.pdb
c:\pdbe_split\contacts_-_6.8\4J57BF.pdb
c:\pdbe_split\contacts_-_6.8\4J8NAB.pdb
c:\pdbe_split\contacts_-_6.8\4J8NBD.pdb
c:\pdbe_split\contacts_-_6.8\4JBWAN.pdb
c:\pdbe_split\contacts_-_6.8\4JFDAD.pdb
c:\pdbe_split\contacts_-_6.8\4JHWFL.pdb
c:\pdbe_split\contacts_-_6.8\4JJ5AB.pdb
c:\pdbe_split\contacts_-_6.8\4JPKAH.pdb
c:\pdbe_split\contacts_-_6.8\4JQWAC.pdb
c:\pdbe_split\contacts_-_6.8\4JREDH.pdb
c:\pdbe_split\contacts_-_6.8\4K12AB.pdb
c:\pdbe_split\contacts_-_6.8\4K3GAB.pdb
c:\pdbe_split\contacts_-_6.8\4K3JBH.pdb
c:\pdbe_split\contacts_-_6.8\4K7PHY.pdb
c:\pdbe_split\contacts_-_6.8\4K8AAB.pdb
c:\pdbe_split\contacts_-_6.8\4KFZAC.pdb
c:\pdbe_split\contacts_-_6.8\4KI5EM.pdb
c:\pdbe_split\contacts_-_6.8\4KRLAB.pdb
c:\pdbe_split\contacts_-_6.8\4KSLWX.pdb
c:\pdbe_split\contacts_-_6.8\4KVNAL.pdb
c:\pdbe_split\contacts_-_6.8\4KXZBJ.pdb
c:\pdbe_split\contacts_-_6.8\4LCDAE.pdb
c:\pdbe_split\contacts_-_6.8\4LDTAB.pdb
c:\pdbe_split\contacts_-_6.8\4LGRAB.pdb
c:\pdbe_split\contacts_-_6.8\4LHXAE.pdb
c:\pdbe_split\contacts_-_6.8\4LHYAE.pdb
c:\pdbe_split\contacts_-_6.8\4LI0AF.pdb
c:\pdbe_split\contacts_-_6.8\4LIQEH.pdb
c:\pdbe_split\contacts_-_6.8\4LL0AB.pdb
c:\pdbe_split\contacts_-_6.8\4LL1CD.pdb
c:\pdbe_split\contacts_-_6.8\4LQFAH.pdb
c:\pdbe_split\contacts_-_6.8\4LRWAB.pdb
c:\pdbe_split\contacts_-_6.8\4LUCAB.pdb
c:\pdbe_split\contacts_-_6.8\4LV6AB.pdb
c:\pdbe_split\contacts_-_6.8\4LVNAP.pdb
c:\pdbe_split\contacts_-_6.8\4LVOAP.pdb
c:\pdbe_split\contacts_-_6.8\4LWZAB.pdb
c:\pdbe_split\contacts_-_6.8\4LX0AB.pdb
c:\pdbe_split\contacts_-_6.8\4LYFBC.pdb
c:\pdbe_split\contacts_-_6.8\4LYHBC.pdb
c:\pdbe_split\contacts_-_6.8\4M3KAB.pdb
c:\pdbe_split\contacts_-_6.8\4M63AC.pdb
c:\pdbe_split\contacts_-_6.8\4M76AB.pdb
c:\pdbe_split\contacts_-_6.8\4M8NDH.pdb
c:\pdbe_split\contacts_-_6.8\4MQTAB.pdb
c:\pdbe_split\contacts_-_6.8\4MVBAD.pdb
c:\pdbe_split\contacts_-_6.8\4MXVBE.pdb
c:\pdbe_split\contacts_-_6.8\4MXVBF.pdb
c:\pdbe_split\contacts_-_6.8\4N0UAB.pdb
c:\pdbe_split\contacts_-_6.8\4N1HAB.pdb
c:\pdbe_split\contacts_-_6.8\4N3ZAC.pdb
c:\pdbe_split\contacts_-_6.8\4N9KAB.pdb
c:\pdbe_split\contacts_-_6.8\4N9OAB.pdb
c:\pdbe_split\contacts_-_6.8\4NBDAD.pdb
c:\pdbe_split\contacts_-_6.8\4NC1AE.pdb
c:\pdbe_split\contacts_-_6.8\4NHHHM.pdb
c:\pdbe_split\contacts_-_6.8\4NIFAE.pdb
c:\pdbe_split\contacts_-_6.8\4NIFBD.pdb
c:\pdbe_split\contacts_-_6.8\4NIYAE.pdb
c:\pdbe_split\contacts_-_6.8\4NM8BH.pdb
c:\pdbe_split\contacts_-_6.8\4NNJCE.pdb
c:\pdbe_split\contacts_-_6.8\4NP4AH.pdb
c:\pdbe_split\contacts_-_6.8\4NYIQS.pdb
c:\pdbe_split\contacts_-_6.8\4NYJQS.pdb
c:\pdbe_split\contacts_-_6.8\4NYJRS.pdb
c:\pdbe_split\contacts_-_6.8\4NYMQS.pdb
c:\pdbe_split\contacts_-_6.8\4NZLAB.pdb
c:\pdbe_split\contacts_-_6.8\4NZRLM.pdb
c:\pdbe_split\contacts_-_6.8\4NZTLM.pdb
c:\pdbe_split\contacts_-_6.8\4O02AH.pdb
c:\pdbe_split\contacts_-_6.8\4O9HHL.pdb
c:\pdbe_split\contacts_-_6.8\4OCMBC.pdb
c:\pdbe_split\contacts_-_6.8\4ODXHX.pdb
c:\pdbe_split\contacts_-_6.8\4OGRAC.pdb
c:\pdbe_split\contacts_-_6.8\4OGYAH.pdb
c:\pdbe_split\contacts_-_6.8\4OGYAL.pdb
c:\pdbe_split\contacts_-_6.8\4OKVBF.pdb
c:\pdbe_split\contacts_-_6.8\4OL0AB.pdb
c:\pdbe_split\contacts_-_6.8\4ONTBF.pdb
c:\pdbe_split\contacts_-_6.8\4OQTAH.pdb
c:\pdbe_split\contacts_-_6.8\4ORZBC.pdb
c:\pdbe_split\contacts_-_6.8\4OT1AL.pdb
c:\pdbe_split\contacts_-_6.8\4P1CDH.pdb
c:\pdbe_split\contacts_-_6.8\4P2RBD.pdb
c:\pdbe_split\contacts_-_6.8\4P4HHX.pdb
c:\pdbe_split\contacts_-_6.8\4P4KBC.pdb
c:\pdbe_split\contacts_-_6.8\4P7EAB.pdb
c:\pdbe_split\contacts_-_6.8\4PERAB.pdb
c:\pdbe_split\contacts_-_6.8\4PKGAG.pdb
c:\pdbe_split\contacts_-_6.8\4PKHAB.pdb
c:\pdbe_split\contacts_-_6.8\4PKIAG.pdb
c:\pdbe_split\contacts_-_6.8\4PP2AF.pdb
c:\pdbe_split\contacts_-_6.8\4PUFBC.pdb
c:\pdbe_split\contacts_-_6.8\4PZYAB.pdb
c:\pdbe_split\contacts_-_6.8\4Q5EAB.pdb
c:\pdbe_split\contacts_-_6.8\4Q9UDE.pdb
c:\pdbe_split\contacts_-_6.8\4Q9UEF.pdb
c:\pdbe_split\contacts_-_6.8\4QFGAB.pdb
c:\pdbe_split\contacts_-_6.8\4QFRAB.pdb
c:\pdbe_split\contacts_-_6.8\4QHUBD.pdb
c:\pdbe_split\contacts_-_6.8\4QHUCH.pdb
c:\pdbe_split\contacts_-_6.8\4QO1AB.pdb
c:\pdbe_split\contacts_-_6.8\4QT8AC.pdb
c:\pdbe_split\contacts_-_6.8\4QWWAF.pdb
c:\pdbe_split\contacts_-_6.8\4QWWBC.pdb
c:\pdbe_split\contacts_-_6.8\4QXAAB.pdb
c:\pdbe_split\contacts_-_6.8\4R0LDH.pdb
c:\pdbe_split\contacts_-_6.8\4R4HAL.pdb
c:\pdbe_split\contacts_-_6.8\4R8WAL.pdb
c:\pdbe_split\contacts_-_6.8\4R9YBL.pdb
c:\pdbe_split\contacts_-_6.8\4RDQCK.pdb
c:\pdbe_split\contacts_-_6.8\4REDAB.pdb
c:\pdbe_split\contacts_-_6.8\4RF0AB.pdb
c:\pdbe_split\contacts_-_6.8\4RGNAB.pdb
c:\pdbe_split\contacts_-_6.8\4RGNAC.pdb
c:\pdbe_split\contacts_-_6.8\4RIWAB.pdb
c:\pdbe_split\contacts_-_6.8\4S10AC.pdb
c:\pdbe_split\contacts_-_6.8\4S1ZAG.pdb
c:\pdbe_split\contacts_-_6.8\4S22AB.pdb
c:\pdbe_split\contacts_-_6.8\4TNWEJ.pdb
c:\pdbe_split\contacts_-_6.8\4TSCAL.pdb
c:\pdbe_split\contacts_-_6.8\4TTHAB.pdb
c:\pdbe_split\contacts_-_6.8\4TXVAB.pdb
c:\pdbe_split\contacts_-_6.8\4U0QAB.pdb
c:\pdbe_split\contacts_-_6.8\4U40AB.pdb
c:\pdbe_split\contacts_-_6.8\4U6VAL.pdb
c:\pdbe_split\contacts_-_6.8\4UDUBC.pdb
c:\pdbe_split\contacts_-_6.8\4UJ3AB.pdb
c:\pdbe_split\contacts_-_6.8\4UJ4DE.pdb
c:\pdbe_split\contacts_-_6.8\4UJ4GH.pdb
c:\pdbe_split\contacts_-_6.8\4UJ4GI.pdb
c:\pdbe_split\contacts_-_6.8\4UMPAD.pdb
c:\pdbe_split\contacts_-_6.8\4UNUAB.pdb
c:\pdbe_split\contacts_-_6.8\4URVRS.pdb
c:\pdbe_split\contacts_-_6.8\4URXRS.pdb
c:\pdbe_split\contacts_-_6.8\4US2RS.pdb
c:\pdbe_split\contacts_-_6.8\4UZDAB.pdb
c:\pdbe_split\contacts_-_6.8\4V1DAC.pdb
c:\pdbe_split\contacts_-_6.8\4V3LAD.pdb
c:\pdbe_split\contacts_-_6.8\4W6WAB.pdb
c:\pdbe_split\contacts_-_6.8\4WEMAB.pdb
c:\pdbe_split\contacts_-_6.8\4WEUBD.pdb
c:\pdbe_split\contacts_-_6.8\4WGVBD.pdb
c:\pdbe_split\contacts_-_6.8\4WJGCE.pdb
c:\pdbe_split\contacts_-_6.8\4WJGGH.pdb
c:\pdbe_split\contacts_-_6.8\4WLRAC.pdb
c:\pdbe_split\contacts_-_6.8\4WUUAE.pdb
c:\pdbe_split\contacts_-_6.8\4WUUDE.pdb
c:\pdbe_split\contacts_-_6.8\4WV1DF.pdb
c:\pdbe_split\contacts_-_6.8\4WWIAD.pdb
c:\pdbe_split\contacts_-_6.8\4WWKAC.pdb
c:\pdbe_split\contacts_-_6.8\4WYBXY.pdb
c:\pdbe_split\contacts_-_6.8\4WYPAB.pdb
c:\pdbe_split\contacts_-_6.8\4X4MAE.pdb
c:\pdbe_split\contacts_-_6.8\4X4XAC.pdb
c:\pdbe_split\contacts_-_6.8\4X6RAB.pdb
c:\pdbe_split\contacts_-_6.8\4X7FAC.pdb
c:\pdbe_split\contacts_-_6.8\4XAMAC.pdb
c:\pdbe_split\contacts_-_6.8\4XF2AF.pdb
c:\pdbe_split\contacts_-_6.8\4XH9AB.pdb
c:\pdbe_split\contacts_-_6.8\4XI5BD.pdb
c:\pdbe_split\contacts_-_6.8\4XIIAB.pdb
c:\pdbe_split\contacts_-_6.8\4XKHEF.pdb
c:\pdbe_split\contacts_-_6.8\4XMNHL.pdb
c:\pdbe_split\contacts_-_6.8\4XP1AH.pdb
c:\pdbe_split\contacts_-_6.8\4XPTAL.pdb
c:\pdbe_split\contacts_-_6.8\4XSGAB.pdb
c:\pdbe_split\contacts_-_6.8\4XV2AB.pdb
c:\pdbe_split\contacts_-_6.8\4XVTGL.pdb
c:\pdbe_split\contacts_-_6.8\4Y19AE.pdb
c:\pdbe_split\contacts_-_6.8\4Y1ABE.pdb
c:\pdbe_split\contacts_-_6.8\4YGAAB.pdb
c:\pdbe_split\contacts_-_6.8\4YUECL.pdb
c:\pdbe_split\contacts_-_6.8\4Z6AHT.pdb
c:\pdbe_split\contacts_-_6.8\4Z7WAB.pdb
c:\pdbe_split\contacts_-_6.8\4ZFTAB.pdb
c:\pdbe_split\contacts_-_6.8\4ZPZAB.pdb
c:\pdbe_split\contacts_-_6.8\4ZUXjm.pdb
c:\pdbe_split\contacts_-_6.8\5A5B89.pdb
c:\pdbe_split\contacts_-_6.8\5AAMAC.pdb
c:\pdbe_split\contacts_-_6.8\5ADXJY.pdb
c:\pdbe_split\contacts_-_6.8\5AF6AH.pdb
c:\pdbe_split\contacts_-_6.8\5AFUBK.pdb
c:\pdbe_split\contacts_-_6.8\5AFUGb.pdb
c:\pdbe_split\contacts_-_6.8\5AFUJY.pdb
c:\pdbe_split\contacts_-_6.8\5AIKAD.pdb
c:\pdbe_split\contacts_-_6.8\5AIUAF.pdb
c:\pdbe_split\contacts_-_6.8\5BO1AI.pdb
c:\pdbe_split\contacts_-_6.8\5BO1AM.pdb
c:\pdbe_split\contacts_-_6.8\5BVPIL.pdb
c:\pdbe_split\contacts_-_6.8\5BWMAB.pdb
c:\pdbe_split\contacts_-_6.8\5C0RAL.pdb
c:\pdbe_split\contacts_-_6.8\5C0WGI.pdb
c:\pdbe_split\contacts_-_6.8\5C1MAB.pdb
c:\pdbe_split\contacts_-_6.8\5C6WHJ.pdb
c:\pdbe_split\contacts_-_6.8\5CHABG.pdb
c:\pdbe_split\contacts_-_6.8\5CKWAB.pdb
c:\pdbe_split\contacts_-_6.8\5CLQAB.pdb
c:\pdbe_split\contacts_-_6.8\5CM8AB.pdb
c:\pdbe_split\contacts_-_6.8\5CRAAD.pdb
c:\pdbe_split\contacts_-_6.8\5CT1AB.pdb
c:\pdbe_split\contacts_-_6.8\5CUSAH.pdb
c:\pdbe_split\contacts_-_6.8\5CWSBE.pdb
c:\pdbe_split\contacts_-_6.8\5D72AH.pdb
c:\pdbe_split\contacts_-_6.8\5D93AB.pdb
c:\pdbe_split\contacts_-_6.8\5D93AC.pdb
c:\pdbe_split\contacts_-_6.8\5D96DI.pdb
c:\pdbe_split\contacts_-_6.8\5DA0AB.pdb
c:\pdbe_split\contacts_-_6.8\5DFVBE.pdb
c:\pdbe_split\contacts_-_6.8\5DHAAC.pdb
c:\pdbe_split\contacts_-_6.8\5DIFAB.pdb
c:\pdbe_split\contacts_-_6.8\5DLMHX.pdb
c:\pdbe_split\contacts_-_6.8\5DLMLX.pdb
c:\pdbe_split\contacts_-_6.8\5DURAD.pdb
c:\pdbe_split\contacts_-_6.8\5DZQAO.pdb
c:\pdbe_split\contacts_-_6.8\5E8DAL.pdb
c:\pdbe_split\contacts_-_6.8\5EDVAG.pdb
c:\pdbe_split\contacts_-_6.8\5ESVCH.pdb
c:\pdbe_split\contacts_-_6.8\5F6JBG.pdb
c:\pdbe_split\contacts_-_6.8\5FGBCF.pdb
c:\pdbe_split\contacts_-_6.8\5FR1AB.pdb
c:\pdbe_split\contacts_-_6.8\5FR2AB.pdb
c:\pdbe_split\contacts_-_6.8\5FV2BW.pdb
c:\pdbe_split\contacts_-_6.8\5FV2CV.pdb
c:\pdbe_split\contacts_-_6.8\6CHACG.pdb
c:\pdbe_split\contacts_-_6.8\6EBXAB.pdb
c:\pdbe_split\contacts_-_6.8\6Q21AB.pdb
c:\pdbe_split\contacts_-_6.8\6Q21BD.pdb
c:\pdbe_split\contacts_-_6.8\6Q21CD.pdb
c:\pdbe_split\contacts_-_6.8\7GCHFG.pdb
c:\pdbe_split\contacts_-_6.8\9RSAAB.pdb";
            var y = x.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            y.ForEach(a=>Debug.WriteLine(new FileInfo(a).Length));
        }
        static void Main(string[] args)
        {
            // step 1
            //s();
            //return;
            var folder = @"C:\Users\k1040\Desktop\energies\all\csv\";
            var files = Directory.GetFiles(folder, "*.csv", SearchOption.AllDirectories).ToList();


            files.ForEach(a => FindReceptorEntries(a));
            return;

            //files.ForEach(a =>
            //{
            //    var data = File.ReadAllLines(a).ToList();

            //    var line = data[1];

            //    var lineSplit = line.Split(',');

            //    var csvPdbId = lineSplit[1];
            //    var csvChainId = lineSplit[2][0];
            //    var csvPartnerChainId = lineSplit[lineSplit.Length - 1][0];
            //    var ibisReceptorData = LoadIbisPpiData(csvPdbId + csvPartnerChainId);

            //    if (ibisReceptorData == null) return;

            //    foreach (var ibisPpiEntry in ibisReceptorData)
            //    {
            //        // loop through ibis ppi entries to find matching receptor interfaces
            //        // receptor sequence may mismatch as ibis represents domain interactions

            //        var dimer = ibisPpiEntry[12];
            //        var receptorId = dimer.Substring(0, dimer.IndexOf('_'));
            //        var ligandId = dimer.Substring(dimer.IndexOf('_') + 1);


            //        if (receptorId == ligandId) continue; // same chain

            //        var receptorPdbId = receptorId.Substring(0, 4);
            //        var receptorChainId = receptorId.Substring(4, 1)[0];
            //        var ligandPdbId = ligandId.Substring(0, 4);
            //        var ligandChainId = ligandId.Substring(4, 1)[0];

            //        if (receptorChainId==ligandChainId) continue;

            //        var chainIds = receptorChainId < ligandChainId ? "" + receptorChainId + ligandChainId : "" + ligandChainId + receptorChainId;

            //        //Debug.WriteLine(@"ComplexAtoms c:\pdbe\" + receptorPdbId + ".pdb - " + chainIds + @" c:\pdbe_split\" + receptorPdbId + chainIds + ".pdb");
            //        //Debug.WriteLine(@"ComplexContacts c:\pdbe_split\" + receptorPdbId + chainIds + @".pdb 6.8 c:\pdbe_split\contacts_-_6.8\"+ receptorPdbId + chainIds + @".pdb N");

            //        Debug.WriteLine(@"ComplexInterfaces C:\ibis_dbmk_multibinding\seq.fasta c:\pdbe_split\" + receptorPdbId + chainIds + @".pdb c:\pdbe_split\contacts_-_6.8\" + receptorPdbId + chainIds + @".pdb - - - * 6.8 5 0 0.5 C:\pdbe_split\interfaces_-_6.8_0_5_0.5\interfaces_" + receptorPdbId + chainIds + @".csv C:\pdbe_split\interfaces_-_6.8_0_5_0.5\interface-interface_" + receptorPdbId + chainIds + @".csv N");

            //    }
            //    /*
            //                      { "[pdb_file]", "PDB ~v3.3 Protein Data Bank format file [*.pdb, *.ent]"},
            //    { "[[subset]]", "-, mc, sc, ca"},
            //    { "[[chain_ids]]", "molecule chains to output [- for all]"},
            //    { "[[output_file]]", "optional output file. use ? for chain id. when ommitted, output to console"},

            //    }
            //    */

            //});

            //return;
            //var ligandChartTextList = File.ReadAllLines(@"c:\users\k1040\desktop\energies\all\png\dbmk_subset_list.txt").ToList();

            //ligandChartTextList.ForEach(a=> FindInterfacePartners(a));

            //////return;
            //////var ligandChartList = ligandChartTextList.Select(a => new Tuple<string,char,int>( a.Substring(0, 4) , a[4], int.Parse(a.Substring(5)))).Distinct().ToList();

            //////var ligandPartnerList = ligandChartList.Select(a =>
            //////                                     {
            //////                                         var f = @"C:\Users\k1040\Downloads\ibisdown.tar\ibisdown\" + a.Item1.Substring(1, 2) + @"\" + a.Item1.Substring(0, 4) + @".txt";

            //////                                         if (!File.Exists(f)) return null;

            //////                                         var data = File.ReadAllLines(f).Select(b => b.Split(':').Select(c=>c.Trim()).ToList()).ToList();

            //////                                         data = data.Where(b => b[1] == "PPI").ToList();

            //////                                         var ligandReceptorComplexes = data.Select(b => b[12]).ToList();
            //////                                         var ligandHomologs = ligandReceptorComplexes.Select(b => b.Substring(0, b.IndexOf('_'))).ToList();
            //////                                         var receptors = ligandReceptorComplexes.Select(b => b.Substring(b.IndexOf('_')+1)).ToList();

            //////                                         // for each receptor, load ibis, check if it has ligand as a homolog, add to white list 

            //////                                         return new Tuple<Tuple<string,char,int>, List<List<string>>>(a, data);

            //////                                     }).Where(a=>a!=null).ToList();



            //////foreach (var ligandReceptorEntry in ligandPartnerList)
            //////{
            //////    Debug.WriteLine("");
            //////    Debug.WriteLine("Receptor: " + ligandReceptorEntry.Item1.Item1 + "_" + ligandReceptorEntry.Item1.Item2 + "_" + ligandReceptorEntry.Item1.Item3);

            //////    var entrySeq = pdbSeqList.FirstOrDefault(a => a.IdSplit.PdbId == ligandReceptorEntry.Item1.Item1 && a.IdSplit.ChainId == ligandReceptorEntry.Item1.Item2);

            //////    var entryInterfaceData = ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load(InterfaceDataFolder + @"\interface-interface_" + ligandReceptorEntry.Item1.Item1 + ".pdb.csv");
            //////    entryInterfaceData = entryInterfaceData?.Where(a => 

            //////    a.ReceptorPdbId == ligandReceptorEntry.Item1.Item1 &&
            //////    a.ReceptorChainId == ligandReceptorEntry.Item1.Item2 &&
            //////    a.ReceptorInterfaceIndex==ligandReceptorEntry.Item1.Item3 &&
            //////    a.LigandChainId != '*'
            //////    )?.ToList();

            //////    var x = ligandReceptorEntry.Item2.Select(a=>
            //////        new Tuple<string,char,char,string, string>(
            //////            a[12].Substring(0,4),
            //////            a[12][4],
            //////            a[12][10],
            //////            pdbSeqList?.FirstOrDefault(b => b.IdSplit.PdbId == a[12].Substring(0, 4) && b.IdSplit.ChainId == a[12][4])?.FullSequence,
            //////            pdbSeqList?.FirstOrDefault(b => b.IdSplit.PdbId == a[12].Substring(0, 4) && b.IdSplit.ChainId == a[12][10])?.FullSequence)
            //////    ).ToList();

            //////    // delete null sequences
            //////    x = x.Where(a => !string.IsNullOrWhiteSpace(a.Item4) && !string.IsNullOrWhiteSpace(a.Item5)).ToList();



            //////    var ligands = x.Select(a => ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load(InterfaceDataFolder + @"interface-interface_" + a.Item1 + ".pdb.csv")?.Where(b=>b.ReceptorChainId==a.Item2&&b.LigandChainId==a.Item3).ToList()).ToList();

            //////    ligands = ligands.Where(a => a != null).ToList();
            //////    ligands = ligands.Select(a => a.Where(b => b.LigandInterfaceSuperLength >= 10).ToList()).ToList();
            //////    ligands = ligands.Where(a => a.Count > 0).ToList();




            //////    // make sure receptor is homologous to query (as claimed by ibis)
            //////    foreach (var ligand in ligands)
            //////    {
            //////        // align receptor to query receptor, then check overlap

            //////        var notSimiliarList = new List<ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData>();

            //////        foreach (var @interface in ligand)
            //////        {
            //////            // check receptor and query are the same protein or homolog
            //////            var nmw1 = new NeedlemanWunsch(entrySeq.FullSequence, @interface.ReceptorSequenceSuper);
            //////            var aligned1 = nmw1.getAlignment();
            //////            var zzz1 = ProteinBioClass.SequenceSimilarityPercentage(aligned1[0], aligned1[1]);

            //////            var midScore1 = (zzz1.Score + zzz1.ScoreEvo)/2;

            //////            if (midScore1 < 0.4m)
            //////            {
            //////                notSimiliarList.Add(@interface);
            //////                continue;
            //////            }

            //////            // check receptor-interface and query-interface are the same interface
            //////            /*
            //////            var nmw2 = new NeedlemanWunsch( , );
            //////            var aligned2 = nmw2.getAlignment();
            //////            var zzz2 = ProteinBioClass.SequenceSimilarityPercentage(aligned2[0], aligned2[1]);

            //////            var midScore2 = (zzz2.Score + zzz2.ScoreEvo) / 2;

            //////            if (midScore2 < 0.4m)
            //////            {
            //////                notSimiliarList.Add(@interface);
            //////                continue;
            //////            }
            //////            */

            //////            /*
            //////            Debug.WriteLine(zzz.Score + " : " + zzz.ScoreEvo);
            //////            ///Debug.WriteLine(entrySeq.FullSequence);
            //////            ///Debug.WriteLine(@interface.ReceptorSequenceSuper);
            //////            Debug.WriteLine("P1: " + aligned[0]);
            //////            Debug.WriteLine("P2: " + aligned[1]);
            //////            Debug.WriteLine("");
            //////            Debug.WriteLine("I1: " + @interface.ReceptorInterfaceAllAminoAcidsSuper);
            //////            Debug.WriteLine("I2: " + @interface.LigandInterfaceAllAminoAcidsSuper);
            //////            Debug.WriteLine("");
            //////            */
            //////        }

            //////        foreach (var notSimilar in notSimiliarList) { ligand.Remove(notSimilar); }
            //////    }

            //////    // output list of replacement ligand interface sequences, for the entry ligand sequence interface

            //////    var ligandInterfaces = ligands?.SelectMany(a => a.Select(b => b.LigandInterfaceAllAminoAcids).ToList()).Distinct().ToList();
            //////    entryInterfaceData?.ForEach(a=>ligandInterfaces.Remove(a.LigandInterfaceAllAminoAcids));

            //////    ligandInterfaces = ligandInterfaces.Where(a => !ligandInterfaces.Any(b => b!=a&& b.Contains(a))).ToList();

            //////    ligandInterfaces?.ForEach(a=>Debug.WriteLine("sub " + "" + " with " + a));





            //////    continue;

            //////        }

            //////return;


            // data = data.GroupBy(b=>b)

            // data = data.Where(b => b[7] == "1").ToList(); // singleton (only observed once)

            // data = data.Where(b => b[13] == "1").ToList(); // is observed in query

            // remove duplicate ligand sequences (not receptor sequences - which should be ~identical or homologous)
            //x = x.GroupBy(a => a.Item5).Select(a => a.FirstOrDefault()).ToList();
            // load interface data for the ligands, remove ~duplicates

            // cluster receptor interfaces (by resseq ids and sequence similarity)

            // cluster ligand interfaces (already grouped by receptor interface)

            //foreach (var ligand in ligands)
            //{
            //    
            //    foreach (var @interface in ligand)
            //    {
            //        Debug.WriteLine("Ligand Interface: " + @interface.LigandPdbId + "_" + @interface.LigandChainId + "_" + @interface.LigandInterfaceIndex);
            //
            //        Debug.WriteLine(@interface.LigandInterfaceAllAminoAcids);
            //
            //
            //    }
            //}

            /*var seqPdbIds = ibisList.Select(a =>
                                                   {
                                                      var pdbIds = a?.Select(b => b[12]).ToList();

                                                      return pdbIds;
                                                   }).ToList();*/

            //var pdbIds1 = seqPdbIds.Where(a=>a!=null).SelectMany(a =>  a?.Where(b=>b!=null).Select(b => b?.Substring(0, 4)).ToList()).ToList();

            //File.WriteAllLines(@"c:\users\k1040\desktop\dbmk_ibis_pbdids.txt",pdbIds1);

            /*var seqs = seqPdbIds.Select(a => a != null && a.Any(b=>b!=null)? 

                    a.Select(b => seqList.FirstOrDefault(c => c.IdSplit.PdbId == b.Substring(0,4) && c.IdSplit.ChainId == b[4])?.FullSequence).ToList() 

                    : null).ToList();*/



            // not all are similar enough, so filter them to make sure receptors are all highly homologous
            // 2. load ppi partner (receptor-ligand) list from ibis
            // 3. cluster receptors to main dbmk protein, reject non-similar

            // 4. cluster ligands by receptor binding site

            // 5. reject duplicate ligands and duplicate ligand interfaces


            // 6. substitute ligand interface in the main dbmk receptor-ligand complex with remaining ligands

            // 7. output new sequences to theoretically model

        }
            }
}
