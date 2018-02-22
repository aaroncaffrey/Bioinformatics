using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.AminoAcids;

namespace NewDataSet
{
    public class MultiBinding
    {
        public class MultiBindingInterface
        {
            public int DomainCluster;

            public string DomainSuperFamily;

            public string InteractionDomainPartnersPartner1;
            public string InteractionDomainPartnersPartner2;

            public string InteractionChainsPdb1;

            public int InteractionChainsPdb1Chain1ClusterIndex;
            public char InteractionChainsPdb1Chain1;

            public int InteractionChainsPdb1Chain2ClusterIndex;
            public char InteractionChainsPdb1Chain2;

            public string InteractionChainsPdb2;

            
            public int InteractionChainsPdb2Chain1ClusterIndex;
            public char InteractionChainsPdb2Chain1;

            
            public int InteractionChainsPdb2Chain2ClusterIndex;
            public char InteractionChainsPdb2Chain2;

            public int InterfaceOverlapRatio;



            public int Pdb1Chain1InterfaceStart;
            public int Pdb1Chain1InterfaceEnd;
            public int Pdb1Chain1TotalInteractions;
            public string Pdb1Chain1InterfaceSequence;
            public string Pdb1Chain1InterfaceMask;

            public int Pdb1Chain1Best50InterfaceStart;
            public int Pdb1Chain1Best50InterfaceEnd;
            public int Pdb1Chain1Best50TotalInteractions;
            public string Pdb1Chain1Best50InterfaceSequence;
            public string Pdb1Chain1Best50InterfaceMask;


            public int Pdb1Chain2InterfaceStart;
            public int Pdb1Chain2InterfaceEnd;
            public int Pdb1Chain2TotalInteractions;
            public string Pdb1Chain2InterfaceSequence;
            public string Pdb1Chain2InterfaceMask;

            public int Pdb1Chain2Best50InterfaceStart;
            public int Pdb1Chain2Best50InterfaceEnd;
            public int Pdb1Chain2Best50TotalInteractions;
            public string Pdb1Chain2Best50InterfaceSequence;
            public string Pdb1Chain2Best50InterfaceMask;


            public int Pdb2Chain1InterfaceStart;
            public int Pdb2Chain1InterfaceEnd;
            public int Pdb2Chain1TotalInteractions;
            public string Pdb2Chain1InterfaceSequence;
            public string Pdb2Chain1InterfaceMask;

            public int Pdb2Chain1Best50InterfaceStart;
            public int Pdb2Chain1Best50InterfaceEnd;
            public int Pdb2Chain1Best50TotalInteractions;
            public string Pdb2Chain1Best50InterfaceSequence;
            public string Pdb2Chain1Best50InterfaceMask;

            public int Pdb2Chain2InterfaceStart;
            public int Pdb2Chain2InterfaceEnd;
            public int Pdb2Chain2TotalInteractions;
            public string Pdb2Chain2InterfaceSequence;
            public string Pdb2Chain2InterfaceMask;

            public int Pdb2Chain2Best50InterfaceStart;
            public int Pdb2Chain2Best50InterfaceEnd;
            public int Pdb2Chain2Best50TotalInteractions;
            public string Pdb2Chain2Best50InterfaceSequence;
            public string Pdb2Chain2Best50InterfaceMask;


            public override string ToString()
            {
                return string.Join(",", new string[]
                {
                    ""+DomainCluster,
                    DomainSuperFamily,

                    InteractionDomainPartnersPartner1,
                    InteractionDomainPartnersPartner2,

                    InteractionChainsPdb1,

                    "" + InteractionChainsPdb1Chain1ClusterIndex,
                    ""+InteractionChainsPdb1Chain1,

                    "" + InteractionChainsPdb1Chain2ClusterIndex,
                    ""+InteractionChainsPdb1Chain2,

                    InteractionChainsPdb2,

                    ""+InteractionChainsPdb2Chain1,
                    "" + InteractionChainsPdb2Chain1ClusterIndex,

                    ""+InteractionChainsPdb2Chain2,
                    "" + InteractionChainsPdb2Chain2ClusterIndex,

                    "" + InterfaceOverlapRatio,



                    "" + Pdb1Chain1InterfaceStart,
                    "" + Pdb1Chain1InterfaceEnd,
                    "" + Pdb1Chain1TotalInteractions,
                    Pdb1Chain1InterfaceSequence,
                    Pdb1Chain1InterfaceMask,

                    "" + Pdb1Chain1Best50InterfaceStart,
                    "" + Pdb1Chain1Best50InterfaceEnd,
                    "" + Pdb1Chain1Best50TotalInteractions,
                    Pdb1Chain1Best50InterfaceSequence,
                    Pdb1Chain1Best50InterfaceMask,


                    "" + Pdb1Chain2InterfaceStart,
                    "" + Pdb1Chain2InterfaceEnd,
                    "" + Pdb1Chain2TotalInteractions,
                    Pdb1Chain2InterfaceSequence,
                    Pdb1Chain2InterfaceMask,

                    "" + Pdb1Chain2Best50InterfaceStart,
                    "" + Pdb1Chain2Best50InterfaceEnd,
                    "" + Pdb1Chain2Best50TotalInteractions,
                    Pdb1Chain2Best50InterfaceSequence,
                    Pdb1Chain2Best50InterfaceMask,


                    "" + Pdb2Chain1InterfaceStart,
                    "" + Pdb2Chain1InterfaceEnd,
                    "" + Pdb2Chain1TotalInteractions,
                    Pdb2Chain1InterfaceSequence,
                    Pdb2Chain1InterfaceMask,

                    "" + Pdb2Chain1Best50InterfaceStart,
                    "" + Pdb2Chain1Best50InterfaceEnd,
                    "" + Pdb2Chain1Best50TotalInteractions,
                    Pdb2Chain1Best50InterfaceSequence,
                    Pdb2Chain1Best50InterfaceMask,

                    "" + Pdb2Chain2InterfaceStart,
                    "" + Pdb2Chain2InterfaceEnd,
                    "" + Pdb2Chain2TotalInteractions,
                    Pdb2Chain2InterfaceSequence,
                    Pdb2Chain2InterfaceMask,

                    "" + Pdb2Chain2Best50InterfaceStart,
                    "" + Pdb2Chain2Best50InterfaceEnd,
                    "" + Pdb2Chain2Best50TotalInteractions,
                    Pdb2Chain2Best50InterfaceSequence,
                    Pdb2Chain2Best50InterfaceMask,


                });
            }

            public static string Header()
            {
                return string.Join(",", new string[]
                {
                   "Domain Cluster",
                   "DomainSuperFamily",
                   
                   "InteractionDomainPartnersPartner1",
                   "InteractionDomainPartnersPartner2",
                   
                   "InteractionChainsPdb1",
                   
                   "InteractionChainsPdb1Chain1ClusterIndex",
                   "InteractionChainsPdb1Chain1",
                   
                   "InteractionChainsPdb1Chain2ClusterIndex",
                   "InteractionChainsPdb1Chain2",
                   
                   "InteractionChainsPdb2",
                   
                   "InteractionChainsPdb2Chain1",
                   "InteractionChainsPdb2Chain1ClusterIndex",
                   
                   "InteractionChainsPdb2Chain2",
                   "InteractionChainsPdb2Chain2ClusterIndex",
                   
                   "InterfaceOverlapRatio",
                   
                   
                   
                   "Pdb1Chain1InterfaceStart",
                   "Pdb1Chain1InterfaceEnd",
                   "Pdb1Chain1TotalInteractions",
                   "Pdb1Chain1InterfaceSequence",
                   "Pdb1Chain1InterfaceMask",
                   
                   "Pdb1Chain1Best50InterfaceStart",
                   "Pdb1Chain1Best50InterfaceEnd",
                   "Pdb1Chain1Best50TotalInteractions",
                   "Pdb1Chain1Best50InterfaceSequence",
                   "Pdb1Chain1Best50InterfaceMask",
                   
                   
                   "Pdb1Chain2InterfaceStart",
                   "Pdb1Chain2InterfaceEnd",
                   "Pdb1Chain2TotalInteractions",
                   "Pdb1Chain2InterfaceSequence",
                   "Pdb1Chain2InterfaceMask",
                   
                   "Pdb1Chain2Best50InterfaceStart",
                   "Pdb1Chain2Best50InterfaceEnd",
                   "Pdb1Chain2Best50TotalInteractions",
                   "Pdb1Chain2Best50InterfaceSequence",
                   "Pdb1Chain2Best50InterfaceMask",
                   
                   
                   "Pdb2Chain1InterfaceStart",
                   "Pdb2Chain1InterfaceEnd",
                   "Pdb2Chain1TotalInteractions",
                   "Pdb2Chain1InterfaceSequence",
                   "Pdb2Chain1InterfaceMask",
                   
                   "Pdb2Chain1Best50InterfaceStart",
                   "Pdb2Chain1Best50InterfaceEnd",
                   "Pdb2Chain1Best50TotalInteractions",
                   "Pdb2Chain1Best50InterfaceSequence",
                   "Pdb2Chain1Best50InterfaceMask",
                   
                   "Pdb2Chain2InterfaceStart",
                   "Pdb2Chain2InterfaceEnd",
                   "Pdb2Chain2TotalInteractions",
                   "Pdb2Chain2InterfaceSequence",
                   "Pdb2Chain2InterfaceMask",
                   
                   "Pdb2Chain2Best50InterfaceStart",
                   "Pdb2Chain2Best50InterfaceEnd",
                   "Pdb2Chain2Best50TotalInteractions",
                   "Pdb2Chain2Best50InterfaceSequence",
                   "Pdb2Chain2Best50InterfaceMask",


                });
            }

            public MultiBindingInterface(int domainCluster, string domainSuperFamily, string interactionDomainPartnersPartner1, string interactionDomainPartnersPartner2, string interactionChainsPdb1, int interactionChainsPdb1Chain1ClusterIndex, char interactionChainsPdb1Chain1, int interactionChainsPdb1Chain2ClusterIndex, char interactionChainsPdb1Chain2, string interactionChainsPdb2, char interactionChainsPdb2Chain1, int interactionChainsPdb2Chain1ClusterIndex, char interactionChainsPdb2Chain2, int interactionChainsPdb2Chain2ClusterIndex, int interfaceOverlapRatio)
            {
                DomainCluster = domainCluster;
                DomainSuperFamily = domainSuperFamily;
                InteractionDomainPartnersPartner1 = interactionDomainPartnersPartner1;
                InteractionDomainPartnersPartner2 = interactionDomainPartnersPartner2;
                InteractionChainsPdb1 = interactionChainsPdb1;
                InteractionChainsPdb1Chain1ClusterIndex = interactionChainsPdb1Chain1ClusterIndex;
                InteractionChainsPdb1Chain1 = interactionChainsPdb1Chain1;
                InteractionChainsPdb1Chain2ClusterIndex = interactionChainsPdb1Chain2ClusterIndex;
                InteractionChainsPdb1Chain2 = interactionChainsPdb1Chain2;
                InteractionChainsPdb2 = interactionChainsPdb2;
                InteractionChainsPdb2Chain1 = interactionChainsPdb2Chain1;
                InteractionChainsPdb2Chain1ClusterIndex = interactionChainsPdb2Chain1ClusterIndex;
                InteractionChainsPdb2Chain2 = interactionChainsPdb2Chain2;
                InteractionChainsPdb2Chain2ClusterIndex = interactionChainsPdb2Chain2ClusterIndex;
                InterfaceOverlapRatio = interfaceOverlapRatio;
            }

            public static List<MultiBindingInterface> LoadAuthorData(string multibinding_filename, string pdb_clusters_filename)
            {
                var result = new List<MultiBindingInterface>();

                var fileData = File.ReadAllLines(multibinding_filename);

                var clusterData =
                    SequenceIdentityClustering.SequenceIdentityClustering.SequenceIdentityClusterMember.Load(pdb_clusters_filename);

                //var DomainSuperFamily = "";
                for (int index = 0; index < fileData.Length; index++)
                {
                    if (index == 0/* || index == 1*/) continue;

                    var line = fileData[index];
                    var lineSplit = line.Split(',');

                    var lineRecord = new MultiBindingInterface(
                        int.Parse(lineSplit[0]),
                        lineSplit[1],
                        lineSplit[2],
                        lineSplit[3],
                        lineSplit[4],
                        -1,
                        lineSplit[5][0],
                        -1,
                        lineSplit[6][0],
                        lineSplit[7],
                        lineSplit[8][0],
                        -1,
                        lineSplit[9][0],
                        -1,
                        int.Parse(lineSplit[10]));

                    lineRecord.InteractionChainsPdb1Chain1ClusterIndex =
                        clusterData.FirstOrDefault(
                            a => a.PdbId.ToUpperInvariant() == lineRecord.InteractionChainsPdb1.ToUpperInvariant() &&
                                 a.ChainId == lineRecord.InteractionChainsPdb1Chain1).ClusterIndex;

                    lineRecord.InteractionChainsPdb1Chain2ClusterIndex =
                        clusterData.FirstOrDefault(
                            a => a.PdbId.ToUpperInvariant() == lineRecord.InteractionChainsPdb1.ToUpperInvariant() &&
                                 a.ChainId == lineRecord.InteractionChainsPdb1Chain2).ClusterIndex;

                    lineRecord.InteractionChainsPdb2Chain1ClusterIndex =
                        clusterData.FirstOrDefault(
                            a => a.PdbId.ToUpperInvariant() == lineRecord.InteractionChainsPdb2.ToUpperInvariant() &&
                                 a.ChainId == lineRecord.InteractionChainsPdb2Chain1).ClusterIndex;

                    lineRecord.InteractionChainsPdb2Chain2ClusterIndex =
                        clusterData.FirstOrDefault(
                            a => a.PdbId.ToUpperInvariant() == lineRecord.InteractionChainsPdb2.ToUpperInvariant() &&
                                 a.ChainId == lineRecord.InteractionChainsPdb2Chain2).ClusterIndex;
                    /*
                    if (!string.IsNullOrWhiteSpace(lineRecord.DomainSuperFamily))
                        DomainSuperFamily = lineRecord.DomainSuperFamily;
                    else lineRecord.DomainSuperFamily = DomainSuperFamily;
                    */
                    result.Add(lineRecord);
                }
                /*
                result =
                    result.Where(
                        a =>
                            !string.IsNullOrWhiteSpace(a.InteractionChainsPdb1) &&
                            !string.IsNullOrWhiteSpace(a.InteractionChainsPdb2)).ToList();
*/
                return result;
            }

        }

        static void Main(string[] args)
        {
            // the indexes of data, contacts1 and contacts2 all match

            var data = MultiBindingInterface.LoadAuthorData(@"c:\multibinding\multibinding.csv", @"c:\multibinding\multibinding_homolog_clusters.csv");

            var contactsPartner1 =
                data.Select(
                    a =>
                    {
                        var x = ProteinBioClass.AtomPair.LoadAtomPairList(@"C:\multibinding\contacts\contacts_pdb" +
                                                                     a.InteractionChainsPdb1.ToUpperInvariant() + ".pdb")

                            .Where(
                                b =>
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb1Chain1
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb1Chain2)
                                    ||
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb1Chain2
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb1Chain1)).ToList();

                        x = x.Select(c =>
                        {
                            if (c.Atom1.chainID.FieldValue.ToUpperInvariant()[0] == a.InteractionChainsPdb1Chain2)
                            {
                                c.SwapAtoms();
                            }

                            return c;
                        }).ToList();

                        return x;
                    }).ToList();

            var contactsPartner2 =
                data.Select(
                    a =>
                    {
                        var x = ProteinBioClass.AtomPair.LoadAtomPairList(@"C:\multibinding\contacts\contacts_pdb" +
                                                                     a.InteractionChainsPdb2.ToUpperInvariant() + ".pdb")

                            .Where(
                                b =>
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb2Chain1
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb2Chain2)
                                    ||
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb2Chain2
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant()[0] ==
                                     a.InteractionChainsPdb2Chain1)).ToList();

                        x = x.Select(c =>
                        {
                            if (c.Atom1.chainID.FieldValue.ToUpperInvariant()[0] == a.InteractionChainsPdb2Chain2)
                            {
                                c.SwapAtoms();
                            }

                            return c;
                        }).ToList();

                        return x;
                    }).ToList();

            var interfacePartner1 = contactsPartner1.Select(a =>
            {
                var resSeqChain1 = a.Select(b => int.Parse(b.Atom1.resSeq.FieldValue)).ToList();
                var resSeqChain2 = a.Select(b => int.Parse(b.Atom2.resSeq.FieldValue)).ToList();

                if (resSeqChain1.Count > 0 && resSeqChain2.Count > 0)
                    return new Tuple<int, int, int, int>(resSeqChain1.Min(), resSeqChain1.Max(), resSeqChain2.Min(),
                        resSeqChain2.Max());
                else return null;
            }).ToList();


            var interfacePartner2 = contactsPartner2.Select(a =>
            {
                var resSeqChain1 = a.Select(b => int.Parse(b.Atom1.resSeq.FieldValue)).ToList();
                var resSeqChain2 = a.Select(b => int.Parse(b.Atom2.resSeq.FieldValue)).ToList();

                if (resSeqChain1.Count > 0 && resSeqChain2.Count > 0)
                    return new Tuple<int, int, int, int>(resSeqChain1.Min(), resSeqChain1.Max(), resSeqChain2.Min(),
                        resSeqChain2.Max());
                else return null;
            }).ToList();

           // var resultData = new List<MultiBindingInterface>();

            for (int index = 0; index < data.Count; index++)
            {
                var d = data[index];
                var cp1 = contactsPartner1[index];
                var cp2 = contactsPartner2[index];
                var ip1 = interfacePartner1[index];
                var ip2 = interfacePartner2[index];

                if (d == null || cp1 == null || cp2 == null || ip1 == null || ip2 == null) continue;
                if (cp1.Count == 0 || cp2.Count == 0) continue;

                var p1c1_pdb = ProteinBioClass.PdbAtomicChains(@"c:\multibinding\pdb" + d.InteractionChainsPdb1 + ".pdb", new char[] { d.InteractionChainsPdb1Chain1 }, -1, -1, true);
                var p1c2_pdb = ProteinBioClass.PdbAtomicChains(@"c:\multibinding\pdb" + d.InteractionChainsPdb1 + ".pdb", new char[] { d.InteractionChainsPdb1Chain2 }, -1, -1, true);
                var p2c1_pdb = ProteinBioClass.PdbAtomicChains(@"c:\multibinding\pdb" + d.InteractionChainsPdb2 + ".pdb", new char[] { d.InteractionChainsPdb2Chain1 }, -1, -1, true);
                var p2c2_pdb = ProteinBioClass.PdbAtomicChains(@"c:\multibinding\pdb" + d.InteractionChainsPdb2 + ".pdb", new char[] { d.InteractionChainsPdb2Chain2 }, -1, -1, true);

                var p1c1_res_seq = p1c1_pdb.ChainList.First().AtomList.Select(a => int.Parse(a.resSeq.FieldValue)).ToList();
                var p1c2_res_seq = p1c2_pdb.ChainList.First().AtomList.Select(a => int.Parse(a.resSeq.FieldValue)).ToList();
                var p2c1_res_seq = p2c1_pdb.ChainList.First().AtomList.Select(a => int.Parse(a.resSeq.FieldValue)).ToList();
                var p2c2_res_seq = p2c2_pdb.ChainList.First().AtomList.Select(a => int.Parse(a.resSeq.FieldValue)).ToList();

                var cp1a1_res_seq = cp1.Select(a => int.Parse(a.Atom1.resSeq.FieldValue)).ToList();
                var cp1a2_res_seq = cp1.Select(a => int.Parse(a.Atom2.resSeq.FieldValue)).ToList();
                var cp2a1_res_seq = cp2.Select(a => int.Parse(a.Atom1.resSeq.FieldValue)).ToList();
                var cp2a2_res_seq = cp2.Select(a => int.Parse(a.Atom2.resSeq.FieldValue)).ToList();


                var cp1a1_min = cp1a1_res_seq.Min();
                var cp1a1_max = cp1a1_res_seq.Max();
                var cp1a2_min = cp1a2_res_seq.Min();
                var cp1a2_max = cp1a2_res_seq.Max();

                var cp2a1_min = cp2a1_res_seq.Min();
                var cp2a1_max = cp2a1_res_seq.Max();
                var cp2a2_min = cp2a2_res_seq.Min();
                var cp2a2_max = cp2a2_res_seq.Max();

                var cp1a1_best50_min = int.MinValue;
                var cp1a1_best50_max = int.MinValue;
                var cp1a1_best50_interactions = int.MinValue;
                var cp1a1_best50_middle_finder = new List<Tuple<int, int, int>>();

                var interface_target_length = 50;

                for (var x = cp1a1_min - interface_target_length; x <= cp1a1_max; x++)
                {
                    if (Math.Abs(cp1a1_max - cp1a1_min) <= interface_target_length)
                    {
                        cp1a1_best50_min = cp1a1_min;
                        cp1a1_best50_max = cp1a1_max;
                        cp1a1_best50_interactions = cp1a1_res_seq.Count;
                        break;
                    }

                    var min = x;
                    var max = x + interface_target_length > cp1a1_max ? cp1a1_max : x + interface_target_length;

                    var best50 = cp1a1_res_seq.Count(a => a >= cp1a1_best50_min && a <= cp1a1_best50_max);

                    if (best50 == cp1a1_best50_interactions)
                    {
                        cp1a1_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                    }

                    if (cp1a1_best50_interactions == int.MinValue || best50 > cp1a1_best50_interactions)
                    {
                        cp1a1_best50_middle_finder.Clear();
                        cp1a1_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                        cp1a1_best50_min = min;
                        cp1a1_best50_max = max;
                        cp1a1_best50_interactions = best50;
                    }

                    if (x + interface_target_length >= cp1a1_max) break;
                }

                if (cp1a1_best50_middle_finder.Count > 2)
                {
                    var middle = cp1a1_best50_middle_finder[cp1a1_best50_middle_finder.Count / 2];
                    cp1a1_best50_min = middle.Item1;
                    cp1a1_best50_max = middle.Item2;
                    cp1a1_best50_interactions = middle.Item3;
                }

                var cp1a2_best50_min = int.MinValue;
                var cp1a2_best50_max = int.MinValue;
                var cp1a2_best50_interactions = int.MinValue;
                var cp1a2_best50_middle_finder = new List<Tuple<int, int, int>>();
                for (var x = cp1a2_min - interface_target_length; x <= cp1a2_max; x++)
                {
                    if (Math.Abs(cp1a2_max - cp1a2_min) <= interface_target_length)
                    {
                        cp1a2_best50_min = cp1a2_min;
                        cp1a2_best50_max = cp1a2_max;
                        cp1a2_best50_interactions = cp1a2_res_seq.Count;
                        break;
                    }

                    var min = x;
                    var max = x + interface_target_length > cp1a2_max ? cp1a2_max : x + interface_target_length;

                    var best50 = cp1a2_res_seq.Count(a => a >= cp1a2_best50_min && a <= cp1a2_best50_max);

                    if (best50 == cp1a2_best50_interactions)
                    {
                        cp1a2_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                    }

                    if (cp1a2_best50_interactions == int.MinValue || best50 > cp1a2_best50_interactions)
                    {
                        cp1a2_best50_middle_finder.Clear();
                        cp1a2_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                        cp1a2_best50_min = min;
                        cp1a2_best50_max = max;
                        cp1a2_best50_interactions = best50;
                    }

                    if (x + interface_target_length >= cp1a2_max) break;
                }

                if (cp1a2_best50_middle_finder.Count > 2)
                {
                    var middle = cp1a2_best50_middle_finder[cp1a2_best50_middle_finder.Count / 2];
                    cp1a2_best50_min = middle.Item1;
                    cp1a2_best50_max = middle.Item2;
                    cp1a2_best50_interactions = middle.Item3;
                }


                var cp2a1_best50_min = int.MinValue;
                var cp2a1_best50_max = int.MinValue;
                var cp2a1_best50_interactions = int.MinValue;
                var cp2a1_best50_middle_finder = new List<Tuple<int, int, int>>();
                for (var x = cp2a1_min - interface_target_length; x <= cp2a1_max; x++)
                {
                    if (Math.Abs(cp2a1_max - cp2a1_min) <= interface_target_length)
                    {
                        cp2a1_best50_min = cp2a1_min;
                        cp2a1_best50_max = cp2a1_max;
                        cp2a1_best50_interactions = cp2a1_res_seq.Count;
                        break;
                    }
                    var min = x;
                    var max = x + interface_target_length > cp2a1_max ? cp2a1_max : x + interface_target_length;

                    var best50 = cp2a1_res_seq.Count(a => a >= cp2a1_best50_min && a <= cp2a1_best50_max);

                    if (best50 == cp2a1_best50_interactions)
                    {
                        cp2a1_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                    }

                    if (cp2a1_best50_interactions == int.MinValue || best50 > cp2a1_best50_interactions)
                    {
                        cp2a1_best50_middle_finder.Clear();
                        cp2a1_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                        cp2a1_best50_min = min;
                        cp2a1_best50_max = max;
                        cp2a1_best50_interactions = best50;
                    }

                    if (x + interface_target_length >= cp2a1_max) break;
                }

                if (cp2a1_best50_middle_finder.Count > 2)
                {
                    var middle = cp2a1_best50_middle_finder[cp2a1_best50_middle_finder.Count / 2];
                    cp2a1_best50_min = middle.Item1;
                    cp2a1_best50_max = middle.Item2;
                    cp2a1_best50_interactions = middle.Item3;
                }

                var cp2a2_best50_min = int.MinValue;
                var cp2a2_best50_max = int.MinValue;
                var cp2a2_best50_interactions = int.MinValue;
                var cp2a2_best50_middle_finder = new List<Tuple<int, int, int>>();
                for (var x = cp2a2_min - interface_target_length; x <= cp2a2_max; x++)
                {
                    if (Math.Abs(cp2a2_max - cp2a2_min) <= interface_target_length)
                    {
                        cp2a2_best50_min = cp2a2_min;
                        cp2a2_best50_max = cp2a2_max;
                        cp2a2_best50_interactions = cp2a2_res_seq.Count;
                        break;
                    }
                    var min = x;
                    var max = x + interface_target_length > cp2a2_max ? cp2a2_max : x + interface_target_length;

                    var best50 = cp2a2_res_seq.Count(a => a >= cp2a2_best50_min && a <= cp2a2_best50_max);

                    if (best50 == cp2a2_best50_interactions)
                    {
                        cp2a2_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                    }

                    if (cp2a2_best50_interactions == int.MinValue || best50 > cp2a2_best50_interactions)
                    {
                        cp2a2_best50_middle_finder.Clear();
                        cp2a2_best50_middle_finder.Add(new Tuple<int, int, int>(min, max, best50));
                        cp2a2_best50_min = min;
                        cp2a2_best50_max = max;
                        cp2a2_best50_interactions = best50;
                    }

                    if (x + interface_target_length >= cp2a2_max) break;
                }

                if (cp2a2_best50_middle_finder.Count > 2)
                {
                    var middle = cp2a2_best50_middle_finder[cp2a2_best50_middle_finder.Count / 2];
                    cp2a2_best50_min = middle.Item1;
                    cp2a2_best50_max = middle.Item2;
                    cp2a2_best50_interactions = middle.Item3;
                }

                var cp1a1_interface = string.Join("", p1c1_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp1a1_min && l <= cp1a1_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp1a2_interface = string.Join("", p1c2_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp1a2_min && l <= cp1a2_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp2a1_interface = string.Join("", p2c1_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp2a1_min && l <= cp2a1_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp2a2_interface = string.Join("", p2c2_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp2a2_min && l <= cp2a2_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp1a1_interface_interactions = new string('_', cp1a1_interface.Length);
                cp1a1_interface_interactions = string.Join("", cp1a1_interface_interactions.Select((a, i) => cp1a1_res_seq.Contains(i + cp1a1_min) ? "X" : "_").ToList());

                var cp1a2_interface_interactions = new string('_', cp1a2_interface.Length);
                cp1a2_interface_interactions = string.Join("", cp1a2_interface_interactions.Select((a, i) => cp1a2_res_seq.Contains(i + cp1a2_min) ? "X" : "_").ToList());

                var cp2a1_interface_interactions = new string('_', cp2a1_interface.Length);
                cp2a1_interface_interactions = string.Join("", cp2a1_interface_interactions.Select((a, i) => cp2a1_res_seq.Contains(i + cp2a1_min) ? "X" : "_").ToList());

                var cp2a2_interface_interactions = new string('_', cp2a2_interface.Length);
                cp2a2_interface_interactions = string.Join("", cp2a2_interface_interactions.Select((a, i) => cp2a2_res_seq.Contains(i + cp2a2_min) ? "X" : "_").ToList());

                var cp1a1_interactions = cp1a1_interface_interactions.Count(a => a == 'X');
                var cp1a2_interactions = cp1a2_interface_interactions.Count(a => a == 'X');
                var cp2a1_interactions = cp2a1_interface_interactions.Count(a => a == 'X');
                var cp2a2_interactions = cp2a2_interface_interactions.Count(a => a == 'X');

                var cp1a1_best50_interface = string.Join("", p1c1_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp1a1_best50_min && l <= cp1a1_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp1a2_best50_interface = string.Join("", p1c2_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp1a2_best50_min && l <= cp1a2_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp2a1_best50_interface = string.Join("", p2c1_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp2a1_best50_min && l <= cp2a1_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp2a2_best50_interface = string.Join("", p2c2_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp2a2_best50_min && l <= cp2a2_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp1a1_best50_interface_interactions = new string('_', cp1a1_best50_interface.Length);
                cp1a1_best50_interface_interactions = string.Join("", cp1a1_best50_interface_interactions.Select((a, i) => cp1a1_res_seq.Contains(i + cp1a1_best50_min) ? "X" : "_").ToList());

                var cp1a2_best50_interface_interactions = new string('_', cp1a2_best50_interface.Length);
                cp1a2_best50_interface_interactions = string.Join("", cp1a2_best50_interface_interactions.Select((a, i) => cp1a2_res_seq.Contains(i + cp1a2_best50_min) ? "X" : "_").ToList());

                var cp2a1_best50_interface_interactions = new string('_', cp2a1_best50_interface.Length);
                cp2a1_best50_interface_interactions = string.Join("", cp2a1_best50_interface_interactions.Select((a, i) => cp2a1_res_seq.Contains(i + cp2a1_best50_min) ? "X" : "_").ToList());

                var cp2a2_best50_interface_interactions = new string('_', cp2a2_best50_interface.Length);
                cp2a2_best50_interface_interactions = string.Join("", cp2a2_best50_interface_interactions.Select((a, i) => cp2a2_res_seq.Contains(i + cp2a2_best50_min) ? "X" : "_").ToList());

                d.Pdb1Chain1InterfaceStart = ip1.Item1;
                d.Pdb1Chain1InterfaceEnd = ip1.Item2;
                d.Pdb1Chain1TotalInteractions = cp1a1_interactions;
                d.Pdb1Chain1InterfaceSequence = cp1a1_interface;
                d.Pdb1Chain1InterfaceMask = cp1a1_interface_interactions;

                d.Pdb1Chain1Best50InterfaceStart = cp1a1_best50_min;
                d.Pdb1Chain1Best50InterfaceEnd = cp1a1_best50_max;
                d.Pdb1Chain1Best50TotalInteractions = cp1a1_best50_interactions;
                d.Pdb1Chain1Best50InterfaceSequence = cp1a1_best50_interface;
                d.Pdb1Chain1Best50InterfaceMask = cp1a1_best50_interface_interactions;

                d.Pdb1Chain2InterfaceStart = ip1.Item3;
                d.Pdb1Chain2InterfaceEnd = ip1.Item4;
                d.Pdb1Chain2TotalInteractions = cp1a2_interactions;
                d.Pdb1Chain2InterfaceSequence = cp1a2_interface;
                d.Pdb1Chain2InterfaceMask = cp1a2_interface_interactions;

                d.Pdb1Chain2Best50InterfaceStart = cp1a2_best50_min;
                d.Pdb1Chain2Best50InterfaceEnd = cp1a2_best50_max;
                d.Pdb1Chain2Best50TotalInteractions = cp1a2_best50_interactions;
                d.Pdb1Chain2Best50InterfaceSequence = cp1a2_best50_interface;
                d.Pdb1Chain2Best50InterfaceMask = cp1a2_best50_interface_interactions;

                d.Pdb2Chain1InterfaceStart = ip2.Item1;
                d.Pdb2Chain1InterfaceEnd = ip2.Item2;
                d.Pdb2Chain1TotalInteractions = cp2a1_interactions;
                d.Pdb2Chain1InterfaceSequence = cp2a1_interface;
                d.Pdb2Chain1InterfaceMask = cp2a1_interface_interactions;

                d.Pdb2Chain1Best50InterfaceStart = cp2a1_best50_min;
                d.Pdb2Chain1Best50InterfaceEnd = cp2a1_best50_max;
                d.Pdb2Chain1Best50TotalInteractions = cp2a1_best50_interactions;
                d.Pdb2Chain1Best50InterfaceSequence = cp2a1_best50_interface;
                d.Pdb2Chain1Best50InterfaceMask = cp2a1_best50_interface_interactions;

                d.Pdb2Chain2InterfaceStart = ip2.Item3;
                d.Pdb2Chain2InterfaceEnd = ip2.Item4;
                d.Pdb2Chain2TotalInteractions = cp2a2_interactions;
                d.Pdb2Chain2InterfaceSequence = cp2a2_interface;
                d.Pdb2Chain2InterfaceMask = cp2a2_interface_interactions;

                d.Pdb2Chain2Best50InterfaceStart = cp2a2_best50_min;
                d.Pdb2Chain2Best50InterfaceEnd = cp2a2_best50_max;
                d.Pdb2Chain2Best50TotalInteractions = cp2a2_best50_interactions;
                d.Pdb2Chain2Best50InterfaceSequence = cp2a2_best50_interface;
                d.Pdb2Chain2Best50InterfaceMask = cp2a2_best50_interface_interactions;
            }

            var output = data.Select(a => a.ToString()).ToList();
            output.Insert(0,MultiBindingInterface.Header());
            File.WriteAllLines(@"c:\multibinding\MultiBinding_parsed_results.csv", output);
            return;
        }
    }
}
