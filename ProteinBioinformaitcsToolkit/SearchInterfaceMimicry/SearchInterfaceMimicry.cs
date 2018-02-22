using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace SearchInterfaceMimicry
{
    public class SearchInterfaceMimicry
    {
        public class IbisData
        {
            public string Query;
            public string Interaction_type;
            public string Mmdb_Residue_No;
            public string PDB_Residue_No;
            public string Binding_Site_Residues;
            public string Binding_Site_Conservation;
            public string Avg_PercentID;
            public string Singleton;
            public string PISA_validation;
            public string Biol_Chemical_validation;
            public string Site_CDD_Annotation;
            public string Interaction_Partner;
            public string PDB_Evidence;
            public string Is_Observed;
            public string Ranking_Score;
            public string Query_Domain;
            public string PubChem_CID;

            public IbisData(string line)
            {
                var split = line.Split(':').Select(a => a.Trim()).ToList();
                if (split.Count <= 1) return;
                var p = -1;
                Query = split[++p];
                Interaction_type = split[++p];
                Mmdb_Residue_No = split[++p];
                PDB_Residue_No = split[++p];
                Binding_Site_Residues = split[++p];
                Binding_Site_Conservation = split[++p];
                Avg_PercentID = split[++p];
                Singleton = split[++p];
                PISA_validation = split[++p];
                Biol_Chemical_validation = split[++p];
                Site_CDD_Annotation = split[++p];
                Interaction_Partner = split[++p];
                PDB_Evidence = split[++p];
                Is_Observed = split[++p];
                Ranking_Score = split[++p];
                Query_Domain = split[++p];
                PubChem_CID = split[++p];
            }


            public static List<IbisData> Load(string filename)
            {
                if (!File.Exists(filename)) { return null; }

                var lines = File.ReadAllLines(filename);

                return lines.Select(a => new IbisData(a)).ToList();
            }
        }

        public static List<Sequence> seqList = Sequence.LoadSequenceFile(@"c:\pdbe\pdb_seqres.fasta");

        public static decimal PdbStructureQuality(string id)
        {
            if (id.Any(a => !char.IsLetterOrDigit(a))) return -1;

            var seq = seqList.FirstOrDefault(a => a.IdSplit.PdbId == id.Substring(0, 4) && a.IdSplit.ChainId == id[4]);

            if (seq == null) return -1;
            var pdb = Sequence.LoadStructureFile(@"c:\pdbe\" + id.Substring(0, 4) + ".pdb", new char[] { id[4] }, false);
            var al = new NeedlemanWunsch(seq.FullSequence, pdb.First().FullSequence);

            var al2 = al.getAlignment();

            var seqA = al2[0];
            var pdbA = al2[1];

            var seqB = string.Join("", seqA.Where((a, i) => !((a == '-' || a == 'X') && (pdbA[i] == '-' || pdbA[i] == 'X'))).ToList());
            var pdbB = string.Join("", pdbA.Where((a, i) => !((a == '-' || a == 'X') && (seqA[i] == '-' || seqA[i] == 'X'))).ToList());

            seqB = seqB.Replace('-', 'X');
            pdbB = pdbB.Replace('-', 'X');

            var match = seqB.Where((a, i) => a == pdbB[i]).Count();

            var len = seqB.Length > pdbB.Length ? seqB.Length : pdbB.Length;

            if (match <= 0 || len <= 0) return 0;
            var score = Math.Round(((decimal)match / (decimal)len), 2);

            return score;
        }

        public static decimal Homology(string pdbId1, string pdbId2)
        {
            if (pdbId1.Any(a => !char.IsLetterOrDigit(a))) return -1;
            if (pdbId2.Any(a => !char.IsLetterOrDigit(a))) return -1;

            var seq1 = seqList.FirstOrDefault(a => a.IdSplit.PdbId == pdbId1.Substring(0, 4) && a.IdSplit.ChainId == pdbId1[4]);
            var seq2 = seqList.FirstOrDefault(a => a.IdSplit.PdbId == pdbId2.Substring(0, 4) && a.IdSplit.ChainId == pdbId2[4]);

            if (seq1 == null) return -1;
            if (seq2 == null) return -1;

            var al = new NeedlemanWunsch(seq1.FullSequence, seq2.FullSequence);

            var al2 = al.getAlignment();

            var seqA = al2[0];
            var pdbA = al2[1];

            var seqB = string.Join("", seqA.Where((a, i) => !((a == '-' || a == 'X') && (pdbA[i] == '-' || pdbA[i] == 'X'))).ToList());
            var pdbB = string.Join("", pdbA.Where((a, i) => !((a == '-' || a == 'X') && (seqA[i] == '-' || seqA[i] == 'X'))).ToList());

            seqB = seqB.Replace('-', 'X');
            pdbB = pdbB.Replace('-', 'X');

            var match = seqB.Where((a, i) => a == pdbB[i]).Count();

            var len = seqB.Length > pdbB.Length ? seqB.Length : pdbB.Length;

            if (match <= 0 || len <= 0) return 0;
            var score = Math.Round(((decimal)match / (decimal)len), 2);

            return score;
        }

        public class LigandInterface
        {
            public string MasterReceptor;
            public string Receptor;
            public string Ligand;
            public int ReceptorInterfaceFirst;
            public int ReceptorInterfaceLast;
            public List<int> ReceptorInterfacePdbResSeqIndexes;
            public string ReceptorInterfaceSecStruct;
            public char ReceptorInterfaceMainSecStruct;
            public int InterfaceIndex;
            public int ContactCluster;

            public static void Save(List<LigandInterface> ligandInterfaceList)
            {
                var data = ligandInterfaceList.Select(a => string.Join(",", new string[] {a.MasterReceptor, a.Receptor, a.Ligand, ""+a.ReceptorInterfaceFirst, ""+a.ReceptorInterfaceLast, string.Join("+",a.ReceptorInterfacePdbResSeqIndexes), a.ReceptorInterfaceSecStruct, ""+a.ReceptorInterfaceMainSecStruct, ""+a.InterfaceIndex, ""+a.ContactCluster})).ToList();
                File.WriteAllLines(@"c:\phd\mimicry.csv",data);
            }
        }

        public static List<string> FilterLigandHomology(List<string> pdbIdList, List<Tuple<string, decimal>> qualities, List<Tuple<string, string, decimal>> homologies)
        {
            var result = new List<string>();

            foreach (var id in pdbIdList)
            {
                var homologs = homologies.Where(a => (a.Item1 == id || a.Item2 == id) && (a.Item1 != a.Item2) && (a.Item3 > 0.30m)).OrderBy(a=>a.Item3).ToList();

                var h = homologs.SelectMany(a => new List<string> {a.Item1, a.Item2}).Distinct().OrderByDescending(a=>a).ToList();
                if (!h.Contains(id))h.Add(id);

                // find which homolog has best quality
                var q = h.Select(a => qualities.First(b => b.Item1==a)).OrderByDescending(b=>b.Item2).ThenByDescending(b=>b.Item1).ToList();

                var best = q.First().Item1;

                if (!result.Contains(best)) result.Add(best);
            }

            return result;
        }

        public static void Main(string[] args)
        {
            var ligandInterfaceList = new List<LigandInterface>();

            var proteinList = File.ReadAllLines(@"c:\phd\search_mimicry_pdb_list.txt");

            if (proteinList.Any(a => a.Length != 5)) throw new Exception("Wrong pdb code length");

            List<Tuple<string, decimal>> crystalToSeqQuality = new List<Tuple<string, decimal>>();
            List<Tuple<string, string, decimal>> homology = new List<Tuple<string, string, decimal>>();


            if (File.Exists(@"c:\phd\pdb_quality.csv"))
            {
                var q = File.ReadAllLines(@"c:\phd\pdb_quality.csv");
                crystalToSeqQuality = q.Select(a => new Tuple<string, decimal>(a.Split(' ')[0], decimal.Parse(a.Split(' ')[1]))).ToList();
            }

            if (File.Exists(@"c:\phd\pdb_homology.csv"))
            {
                var h = File.ReadAllLines(@"c:\phd\pdb_homology.csv");
                homology = h.Select(a => new Tuple<string, string, decimal>(a.Split(' ')[0], a.Split(' ')[1], decimal.Parse(a.Split(' ')[2]))).ToList();
            }

            foreach (var mainReceptorPdbId in proteinList)
            {
                Debug.WriteLine(mainReceptorPdbId);

                var chain = mainReceptorPdbId[4];
                var pdbFile = @"c:\pdbe\" + mainReceptorPdbId.Substring(0, 4) + ".pdb";
                var ibisFile = @"c:\phd\ibis\" + mainReceptorPdbId.Substring(1, 2) + @"\" + mainReceptorPdbId.Substring(0, 4) + ".txt";
                var ibisRecordList = IbisData.Load(ibisFile);
                ibisRecordList = ibisRecordList.Where(a => a.Interaction_type == "PPI").ToList();
                ibisRecordList = ibisRecordList.Where(a => a.Query == mainReceptorPdbId && /*a.PDB_Evidence.StartsWith(mainReceptorPdbId) &&*/ !a.PDB_Evidence.EndsWith(mainReceptorPdbId)).ToList();
                ibisRecordList = ibisRecordList.Where(a => a.PDB_Evidence.Substring(0, 5) != a.PDB_Evidence.Substring(6, 5)).ToList();



                var ligandPdbIds = ibisRecordList.Select(a => a.PDB_Evidence.Substring(6, 5)).Distinct().ToList();
                Debug.WriteLine("B: " + string.Join(", ", ligandPdbIds));
                ligandPdbIds =FilterLigandHomology(ligandPdbIds, crystalToSeqQuality, homology);
                Debug.WriteLine("A: " + string.Join(", ", ligandPdbIds));

                ibisRecordList = ibisRecordList.Where(a => ligandPdbIds.Any(b => a.PDB_Evidence.EndsWith(b))).ToList();

                var currentLigandInterfaceList = new List<LigandInterface>();

                foreach (var ibisRecord in ibisRecordList)
                {

                    var ligandPdbId = ibisRecord.PDB_Evidence.Substring(6, 5);

                    // check quality
                    var ligandQuality = crystalToSeqQuality.FirstOrDefault(a => a.Item1 == ligandPdbId);
                    if (ligandQuality == null)
                    {
                        ligandQuality = new Tuple<string, decimal>(ligandPdbId, PdbStructureQuality(ligandPdbId));
                        crystalToSeqQuality.Add(ligandQuality);
                        File.WriteAllLines(@"c:\phd\pdb_quality.csv", crystalToSeqQuality.Select(a => a.Item1 + " " + a.Item2).ToList());
                        
                    }

                    var dsspFilename = ligandPdbId.Substring(0, 4).ToLowerInvariant() + ".dssp";
                    var dsspFullFilename = @"c:\dssp\" + dsspFilename;
                    var downloadDssp = false;
                    if (!File.Exists(dsspFullFilename)&& downloadDssp)
                    {
                        var ftpDsspFile = @"ftp://ftp.cmbi.ru.nl/pub/molbio/data/dssp/" + dsspFilename;
                        var client = new WebClient();
                        try { client.DownloadFile(ftpDsspFile, dsspFullFilename); }
                        catch (Exception)
                        {
                            
                           // throw;
                        }

                        //if (File.Exists(dsspFullFilename))
                        {
                            //var data = File.ReadAllText(dsspFullFilename);
                            //if (!data.Contains("\r\n") && data.Contains("\n")) { data = data.Replace("\n", "\r\n"); }
                            //if (!data.Contains("\r\n") && data.Contains("\r")) { data = data.Replace("\r", "\r\n"); }
                        }
                    }

                    foreach (var ibisRecord2 in ibisRecordList)
                    {
                        if (ibisRecord == ibisRecord2) continue;

                        // check homology

                        var ligandPdbId2 = ibisRecord2.PDB_Evidence.Substring(6, 5);

                        var homologyPct = homology.FirstOrDefault(a => a.Item1 == ligandPdbId && a.Item2 == ligandPdbId2);

                        if (homologyPct == null)
                        {
                            homologyPct = new Tuple<string, string, decimal>(ligandPdbId, ligandPdbId2, Homology(ligandPdbId, ligandPdbId2));
                            homology.Add(homologyPct);
                            
                            File.WriteAllLines(@"c:\phd\pdb_homology.csv", homology.Select(a => a.Item1 + " " + a.Item2 + " " + a.Item3).ToList());
                        }
                    }
                    var contacts = ibisRecord.PDB_Residue_No.Trim().Split(' ').Select(a => int.Parse(string.Join("", a.Where(b => char.IsNumber(b)).ToList()))).ToList();
                    var min = contacts.Min();
                    var max = contacts.Max();

                    var groups = new List<List<int>>();

                    foreach (var c in contacts)
                    {
                        var g = groups.FirstOrDefault(a => a.Contains(c) || a.Any(b => Math.Abs(c - b) <= 5));
                        if (g == null)
                        {
                            groups.Add(new List<int>() { c });
                            continue;
                        }
                        g.Add(c);

                    }

                    groups = groups.Where(g => g.Count >= 5 && g.Count <= 20).ToList();

                    var secStructs = groups.Select(g => ProteinBioinformaticsSharedLibrary.Dssp.DsspStructureSequence.LoadDsspStructureSequence(dsspFullFilename, "" + chain, g.Min(), g.Max())).ToList();

                    var mainSecStructs = secStructs.Select(a => a.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).FirstOrDefault()).ToList();

                    foreach (var @group in groups)
                    {
                        var index = groups.IndexOf(@group);

                        var r = new LigandInterface() {MasterReceptor = mainReceptorPdbId, Receptor = ibisRecord.PDB_Evidence.Substring(0, 5), Ligand = ibisRecord.PDB_Evidence.Substring(6, 5), ReceptorInterfaceSecStruct = secStructs[index], ReceptorInterfaceMainSecStruct = mainSecStructs[index], ReceptorInterfacePdbResSeqIndexes = @group, ReceptorInterfaceFirst = @group.Min(), ReceptorInterfaceLast = @group.Max(), InterfaceIndex = groups.IndexOf(@group) + 1,};
                        currentLigandInterfaceList.Add(r);
                        ligandInterfaceList.Add(r);
                    }


                }

                var resSeqIndexes = currentLigandInterfaceList.SelectMany(a => a.ReceptorInterfacePdbResSeqIndexes).Distinct().OrderBy(a=>a).ToList();

                var currentLigandInterfaceListGroups = currentLigandInterfaceList.Select(a => new List<LigandInterface>() {a}).ToList();

                foreach (var index in resSeqIndexes)
                {
                    var lists = currentLigandInterfaceListGroups.Where(a => a.Any(b => b.ReceptorInterfacePdbResSeqIndexes.Contains(index))).ToList();
                    currentLigandInterfaceListGroups = currentLigandInterfaceListGroups.Except(lists).ToList();

                    var list = lists.SelectMany(a => a).ToList();
                    var sslist = list.GroupBy(a => a.ReceptorInterfaceMainSecStruct).ToList();
                    
                    sslist.ForEach(list2=>currentLigandInterfaceListGroups.Add(list2.ToList()));
                }

                foreach (var g in currentLigandInterfaceListGroups)
                {
                    g.ForEach(a=>a.ContactCluster=currentLigandInterfaceListGroups.IndexOf(g)+1);
                }

                //return;
            }


            //File.WriteAllLines(@"c:\phd\pdb_quality.csv", crystalToSeqQuality.Select(a => a.Item1 + " " + a.Item2).ToList());
            //File.WriteAllLines(@"c:\phd\pdb_homology.csv", homology.Select(a => a.Item1 + " " + a.Item2 + " " + a.Item3).ToList());
            LigandInterface.Save(ligandInterfaceList);
        }
    }
}
