using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.AminoAcids;
using ProteinBioinformaticsSharedLibrary.Dssp;
using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;

namespace Ds93Ub
{
    public class Ds93Ub
    {
        public class Ds93UbInterface
        {
            public string PdbId;
            public char ChainId1;
            public char ChainId2;
            public int MinResSeq;
            public int MaxResSeq;
            public int BestMinResSeq;
            public int BestMaxResSeq;
            public int BestInteractionsCount;

            public string InterfaceAminoAcids;
            public string InterfaceInteractionsMask;

            public int Partner1ClusterIndex;
            public string Partner1InterfaceAminoAcids;
            public string Partner1InterfaceInteractionsMask;
            public int Partner1InterfaceOverlap;

            public int Partner2ClusterIndex;
            public string Partner2InterfaceAminoAcids;
            public string Partner2InterfaceInteractionsMask;
            public int Partner2InterfaceOverlap;

            public override string ToString()
            {
                return string.Join(",", new string[]
                {
                    PdbId,
                    ""+ChainId1,
                    ""+ChainId2,
                    ""+MinResSeq,
                    ""+MaxResSeq,
                    ""+BestMinResSeq,
                    ""+BestMaxResSeq,
                    ""+BestInteractionsCount,

                    InterfaceAminoAcids,
                    InterfaceInteractionsMask,

                    "" +Partner1ClusterIndex,
                    Partner1InterfaceAminoAcids,
                    Partner1InterfaceInteractionsMask,
                    ""+Partner1InterfaceOverlap,

                    "" +Partner2ClusterIndex,
                    Partner2InterfaceAminoAcids,
                    Partner2InterfaceInteractionsMask,
                    ""+Partner2InterfaceOverlap
                });
            }

            public Ds93UbInterface(string pdbId, char chainId1, char chainId2, int minResSeq, int maxResSeq, int bestMinResSeq, int bestMaxResSeq, int bestInteractionsCount, string interfaceAminoAcids, string interfaceInteractionsMask, int partner1ClusterIndex, string partner1InterfaceAminoAcids, string partner1InterfaceInteractionsMask, int partner1InterfaceOverlap, int partner2ClusterIndex, string partner2InterfaceAminoAcids, string partner2InterfaceInteractionsMask, int partner2InterfaceOverlap)
            {
                PdbId = pdbId;
                ChainId1 = chainId1;
                ChainId2 = chainId2;
                MinResSeq = minResSeq;
                MaxResSeq = maxResSeq;
                BestMinResSeq = bestMinResSeq;
                BestMaxResSeq = bestMaxResSeq;
                BestInteractionsCount = bestInteractionsCount;
                InterfaceAminoAcids = interfaceAminoAcids;
                InterfaceInteractionsMask = interfaceInteractionsMask;
                Partner1ClusterIndex = partner1ClusterIndex;
                Partner1InterfaceAminoAcids = partner1InterfaceAminoAcids;
                Partner1InterfaceInteractionsMask = partner1InterfaceInteractionsMask;
                Partner1InterfaceOverlap = partner1InterfaceOverlap;
                Partner2ClusterIndex = partner2ClusterIndex;
                Partner2InterfaceAminoAcids = partner2InterfaceAminoAcids;
                Partner2InterfaceInteractionsMask = partner2InterfaceInteractionsMask;
                Partner2InterfaceOverlap = partner2InterfaceOverlap;
            }

        }

        static void Main(string[] args)
        {
            var pdbFolder = @"C:\ds96ub_homologs\";

            var homologClusterData =FindHomologsCluster.FindHomologsCluster.HomologClusterData.Load(@"c:\ds96ub_homologs\ds96ub_homologs_0.7.csv");

            var pdbFiles = Directory.GetFiles(pdbFolder, "*.pdb",SearchOption.TopDirectoryOnly);

            var pdbIdList = pdbFiles.Select(ProteinBioClass.PdbIdFromPdbFilename).ToList();

            // only ca-atoms, ters and endmdls
            var pdbAtomsText =
                pdbFiles.Select(
                    a =>
                        File.ReadAllLines(a)
                            .Where(b => (b.StartsWith("ATOM ") && b[13] == 'C' && b[14] == 'A') || /*b.StartsWith("TER ") ||*/ b.StartsWith("ENDMDL "))
                            .ToList()).ToList();

            // only first nmr model
            pdbAtomsText = pdbAtomsText.Select(a =>
            {
                var x = a.FindIndex(b => b.StartsWith("ENDMDL "));
                return x == -1 ? a : a.GetRange(0, x - 1);
            }).ToList();

            var pdbAtoms = pdbAtomsText.Select(a => a.Select(b => new ATOM_Record(b)).ToList()).ToList();

            // get list of unique chain ids 
            var pdbChainIds = pdbAtoms.Select((a, i) => a.Select(b => char.ToUpperInvariant(b.chainID.FieldValue[0])).ToList()).Distinct().ToList();

            var pdbIdChainIdList = new List<Tuple<string, char>>();
            for (var i = 0; i < pdbIdList.Count; i++)
            {
                pdbIdChainIdList.AddRange(pdbChainIds[i].Select(chainId => new Tuple<string, char>(pdbIdList[i], chainId)));
            }
            pdbIdChainIdList = pdbIdChainIdList.Distinct().ToList();

            // for each chain
            var pdbContacts =
                pdbIdChainIdList.Select(a => 
                    {
                        var x =
                            ProteinBioClass.AtomPair.LoadAtomPairList(@"C:\ds96ub_homologs\contacts\contacts_pdb" + a.Item1.ToUpperInvariant() + ".pdb")
                            .Where(b => char.ToUpperInvariant(b.Atom1.chainID.FieldValue[0]) == a.Item2 || char.ToUpperInvariant(b.Atom2.chainID.FieldValue[0]) == a.Item2)
                            .Select(c =>
                            {
                                if (char.ToUpperInvariant(c.Atom1.chainID.FieldValue[0]) != a.Item2)
                                {
                                    c.SwapAtoms();
                                }

                                return c;
                        }).ToList();

                        return x;
                    }).ToList();


            

            // res min, res max, best min, best max, interface aa, interface mask
            var pdbInterfaces = new List<Ds93UbInterface>();

            var interface_target_length = 50;

            
            for (int index = 0; index < pdbContacts.Count; index++)
            {
                var pdbId = pdbIdChainIdList[index].Item1;
                var chainId = pdbIdChainIdList[index].Item2;

                var pdbContact = pdbContacts[index];

                if (pdbContact.Count==0) continue;
                
                var contactChains = pdbContact.Where(a => char.ToUpperInvariant(a.Atom2.chainID.FieldValue[0]) != chainId).Select(a => char.ToUpperInvariant(a.Atom2.chainID.FieldValue[0])).Distinct().ToList();

                foreach (var contactChain in contactChains)
                {
                    var pdbContactsResSeqIds =
                        pdbContact.Where(a => char.ToUpperInvariant(a.Atom1.chainID.FieldValue[0]) == chainId
                        && char.ToUpperInvariant(a.Atom2.chainID.FieldValue[0])==contactChain)
                            .Select(a => int.Parse(a.Atom1.resSeq.FieldValue))
                            .ToList();
                    

                    var res_seq = pdbContactsResSeqIds;
                    var min_res_seq = pdbContactsResSeqIds.Min();
                    var max_res_seq = pdbContactsResSeqIds.Max();

                    var best50_min = int.MinValue;
                    var best50_max = int.MinValue;
                    var best50_interactions = int.MinValue;
                    var best50_middle_finder = new List<Tuple<int, int, int>>();
                    for (var x = min_res_seq - interface_target_length; x <= max_res_seq; x++)
                    {
                        if (Math.Abs(max_res_seq - min_res_seq) <= interface_target_length)
                        {
                            best50_min = min_res_seq;
                            best50_max = max_res_seq;
                            best50_interactions = res_seq.Count;
                            break;
                        }

                        var min = x;
                        var max = x + interface_target_length > max_res_seq ? max_res_seq : x + interface_target_length;

                        var best50 = res_seq.Count(a => a >= best50_min && a <= best50_max);

                        if (best50 == best50_interactions)
                        {
                            best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                        }

                        if (best50_interactions == int.MinValue || best50 > best50_interactions)
                        {
                            best50_middle_finder.Clear();
                            best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                            best50_min = min;
                            best50_max = max;
                            best50_interactions = best50;
                        }

                        if (x + interface_target_length >= max) break;
                    }

                    if (best50_middle_finder.Count > 2)
                    {
                        var middle = best50_middle_finder[best50_middle_finder.Count/2];
                        best50_min = middle.Item1;
                        best50_max = middle.Item2;
                        best50_interactions = middle.Item3;
                    }

                    var best50_interface_atoms = pdbAtoms[pdbIdList.IndexOf(pdbId)].Where(a =>
                    {
                        var l = int.Parse(a.resSeq.FieldValue);
                        var c = char.ToUpperInvariant(a.chainID.FieldValue[0]);
                        return c == chainId && l >= best50_min && l <= best50_max;
                    }).ToList();

                    best50_interface_atoms= best50_interface_atoms.OrderBy(c => int.Parse(c.resSeq.FieldValue)).ToList();

                    var best50_interface = string.Join("",best50_interface_atoms.Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                    var best50_mask = new string('_', best50_interface.Length);
                    best50_mask = string.Join("",
                        best50_mask.Select((a, i) => res_seq.Contains(i + best50_min) ? "X" : "_").ToList());

                    pdbInterfaces.Add(new Ds93UbInterface(pdbId, chainId, contactChain, min_res_seq, max_res_seq, best50_min,
                        best50_max, best50_interactions, best50_interface, best50_mask, -1, "", "",0, -1, "","",0));
                }
            }

            var homologClusterIndexes = homologClusterData.Select(a => a.ClusterIndex).Distinct().ToList();

            var homologClusters = homologClusterIndexes.Select(a => homologClusterData.Where(b => b.ClusterIndex == a).ToList()).ToList();

            var pdbInterfacesSorted = homologClusters.Select(a => pdbInterfaces.Where(b=>a.Any(c=>c.PdbId==b.PdbId && (char.ToUpperInvariant(c.ChainId) == b.ChainId1 || char.ToUpperInvariant(c.ChainId) == b.ChainId2))).ToList()).ToList();

            var outputData = new List<string>();

            
            foreach (var clusterIndex in homologClusterIndexes)
            {
                var cluster = pdbInterfacesSorted[clusterIndex - 1];

                // currently, cluster is a list of chain1-->chain2 interfaces ... so the 'chain2' interface needs adding to the record

                



                foreach (var inf1 in cluster)
                {
                    var partner =
                        cluster.Where(a => a!=inf1 && a.PdbId == inf1.PdbId && inf1.ChainId2 == a.ChainId1)
                            .OrderByDescending(
                                a => InterfaceOverlap(inf1.MinResSeq, inf1.MaxResSeq, a.MinResSeq, a.MaxResSeq))
                            .ToList();

                    var first = partner.FirstOrDefault();
                    if (first != null)
                    {
                        inf1.Partner1InterfaceAminoAcids = first.InterfaceAminoAcids;
                        inf1.Partner1InterfaceInteractionsMask = first.InterfaceInteractionsMask;
                        inf1.Partner1InterfaceOverlap = InterfaceOverlap(inf1.MinResSeq, inf1.MaxResSeq, first.MinResSeq, first.MaxResSeq);
                    }

                    var second = partner.ElementAtOrDefault(1);
                    if (second != null)
                    {
                        inf1.Partner2InterfaceAminoAcids = second.InterfaceAminoAcids;
                        inf1.Partner2InterfaceInteractionsMask = second.InterfaceInteractionsMask;
                        inf1.Partner2InterfaceOverlap = InterfaceOverlap(inf1.MinResSeq, inf1.MaxResSeq, second.MinResSeq,second.MaxResSeq);
                    }

                }

                cluster = cluster.Where(a => a.Partner1InterfaceOverlap > 0 || a.Partner2InterfaceOverlap > 0).ToList();
                /*
                var partners = 
                    foreach (var inf2 in cluster)
                    {
                        if (inf1.PdbId!=inf2.PdbId) continue;

                        if (inf1==inf2) continue;

                        if (!(inf1.ChainId1==inf2.ChainId2 || inf1.ChainId2==inf2.ChainId1)) continue;

                        // 
                        var overlap = InterfaceOverlap(inf1.MinResSeq, inf1.MaxResSeq, inf2.MinResSeq, inf2.MaxResSeq);

                        if (overlap > 0)
                        {
                            if (overlap > inf1.Partner1InterfaceOverlap)
                            {
                                inf1.Partner1InterfaceOverlap = overlap;
                                inf1.Partner1InterfaceAminoAcids = inf2.InterfaceAminoAcids;
                                inf1.Partner1InterfaceInteractionsMask = inf2.InterfaceInteractionsMask;
                            }

                            if (overlap > inf2.Partner1InterfaceOverlap)
                            {
                                inf2.Partner1InterfaceOverlap = overlap;
                                inf2.Partner1InterfaceAminoAcids = inf1.InterfaceAminoAcids;
                                inf2.Partner1InterfaceInteractionsMask = inf1.InterfaceInteractionsMask;
                            }
                        }
                    }
                }
                */

                //var interfaces = cluster.Select(a => a.InterfaceAminoAcids).ToList();
                //interfaces = interfaces.Where(a => interfaces.Count(b => b == a) > 1).ToList();

                //cluster = cluster.Where(a => a.InterfaceAminoAcids.Length >= 5 && cluster.Count(b => b.InterfaceAminoAcids == a.InterfaceAminoAcids) > 1).ToList();
                cluster = cluster.Where(a => a.InterfaceAminoAcids.Length >= 5).ToList();

                var clusterInterfaces = cluster.Select(a => a.InterfaceAminoAcids).ToList();

                var homologInterfaces = new List<List<string>>();
                foreach (var inf1 in clusterInterfaces)
                {
                    var highest_score = decimal.MinValue;
                    string highest_inf = null;

                    foreach (var inf2 in clusterInterfaces)
                    {
                        if (inf1 == inf2) continue;

                        var score = ProteinBioClass.AlignedSequenceSimilarityPercentage(inf1, inf2, ProteinBioClass.AlignmentType.NMW);
                        if (score.Score > highest_score)
                        {
                            highest_score = score.Score;
                            highest_inf = inf2;
                        }

                    }
                    var y = homologInterfaces.FirstOrDefault(a => a.Contains(inf1) || a.Contains(highest_inf));
                    if (y != null)
                    {
                        if (!y.Contains(inf1)) y.Add(inf1);
                        if (!y.Contains(highest_inf)) y.Add(highest_inf);
                    }
                    else
                    {
                        var z = new List<string>();
                        z.Add(inf1);
                        z.Add(highest_inf);
                        homologInterfaces.Add(z);
                    }
                }

                foreach (var c in cluster)
                {
                    c.Partner1ClusterIndex = homologInterfaces.FindIndex(b => b.Contains(c.Partner1InterfaceAminoAcids));
                    c.Partner2ClusterIndex = homologInterfaces.FindIndex(b => b.Contains(c.Partner2InterfaceAminoAcids));
                }

                for (int index = 0; index < homologInterfaces.Count; index++)
                {
                    var homologInterface = homologInterfaces[index];



                    var cluster2 =
                        cluster.Where(a=> homologInterface.Contains(a.InterfaceAminoAcids)
                        )
                        .OrderBy(a => a.Partner1ClusterIndex)
                            .ThenBy(a => a.Partner2ClusterIndex)
                            .ThenBy(a => a.InterfaceAminoAcids)
                            .ThenBy(a => a.Partner1InterfaceAminoAcids)
                            .ThenBy(a => a.Partner2InterfaceAminoAcids)
                            .ToList();

                    var partners =
                        cluster2.Select(
                            a =>
                                new Tuple<string, string, string>(a.InterfaceAminoAcids, a.Partner1InterfaceAminoAcids,
                                    a.Partner2InterfaceAminoAcids)).Distinct();

                    cluster2 =
                        partners.Select(
                            a =>
                                cluster2.FirstOrDefault(
                                    b =>
                                        b.InterfaceAminoAcids == a.Item1 && b.Partner1InterfaceAminoAcids == a.Item2 &&
                                        b.Partner2InterfaceAminoAcids == a.Item3)).ToList();

                    outputData.Add("cluster " + clusterIndex + "." + index);
                    outputData.AddRange(cluster2.Select(a => a.ToString()).ToList());
                    outputData.Add("");
                }
            }

            File.WriteAllLines(@"c:\ds96ub_homologs\ds96ub_homologs_interfaces.csv", outputData);//pdbInterfaces.Select(a=>a.ToString()).ToList());

        }

        public static int InterfaceOverlap(int x1, int x2, int y1, int y2)
        {
            var x_min = Math.Min(x1, x2);
            var x_max = Math.Max(x1, x2);

            var y_min = Math.Min(y1, y2);
            var y_max = Math.Max(y1, y2);

            return (Math.Min(x_max, y_max) - Math.Max(x_min, y_min)) + 1;
        }

    }
}
