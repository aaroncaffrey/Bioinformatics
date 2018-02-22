using ProteinBioinformaticsSharedLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary.AminoAcids;
using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;

namespace NewDataSet
{
    class Program
    {
        public class MultiBindingInterface
        {
            public string DomainSuperFamily;
            public string InteractionDomainPartnersPartner1;
            public string InteractionDomainPartnersPartner2;
            public string InteractionChainsPdb1;
            public string InteractionChainsPdb1Chain1;
            public string InteractionChainsPdb1Chain2;
            public string InteractionChainsPdb2;
            public string InteractionChainsPdb2Chain1;
            public string InteractionChainsPdb2Chain2;
            public string InterfaceOverlapRatio;

            public MultiBindingInterface(string domainSuperFamily, string interactionDomainPartnersPartner1, string interactionDomainPartnersPartner2, string interactionChainsPdb1, string interactionChainsPdb1Chain1, string interactionChainsPdb1Chain2, string interactionChainsPdb2, string interactionChainsPdb2Chain1, string interactionChainsPdb2Chain2, string interfaceOverlapRatio)
            {
                DomainSuperFamily = domainSuperFamily;
                InteractionDomainPartnersPartner1 = interactionDomainPartnersPartner1;
                InteractionDomainPartnersPartner2 = interactionDomainPartnersPartner2;
                InteractionChainsPdb1 = interactionChainsPdb1;
                InteractionChainsPdb1Chain1 = interactionChainsPdb1Chain1;
                InteractionChainsPdb1Chain2 = interactionChainsPdb1Chain2;
                InteractionChainsPdb2 = interactionChainsPdb2;
                InteractionChainsPdb2Chain1 = interactionChainsPdb2Chain1;
                InteractionChainsPdb2Chain2 = interactionChainsPdb2Chain2;
                InterfaceOverlapRatio = interfaceOverlapRatio;
            }

            public static List<MultiBindingInterface> Load(string file)
            {
                var result = new List<MultiBindingInterface>();

                var fileData = File.ReadAllLines(file);

                var DomainSuperFamily = "";
                for (int index = 0; index < fileData.Length; index++)
                {
                    if (index == 0 || index == 1) continue;

                    var line = fileData[index];
                    var lineSplit = line.Split(',');

                    var lineRecord = new MultiBindingInterface(
                        lineSplit[0],
                        lineSplit[1],
                        lineSplit[2],
                        lineSplit[3],
                        lineSplit[4],
                        lineSplit[5],
                        lineSplit[6],
                        lineSplit[7],
                        lineSplit[8],
                        lineSplit[9]);


                    if (!string.IsNullOrWhiteSpace(lineRecord.DomainSuperFamily))
                        DomainSuperFamily = lineRecord.DomainSuperFamily;
                    else lineRecord.DomainSuperFamily = DomainSuperFamily;

                    result.Add(lineRecord);
                }

                result =
                    result.Where(
                        a =>
                            !string.IsNullOrWhiteSpace(a.InteractionChainsPdb1) &&
                            !string.IsNullOrWhiteSpace(a.InteractionChainsPdb2)).ToList();

                return result;
            }

        }

        static void Main(string[] args)
        {
            // the indexes of data, contacts1 and contacts2 all match

            var data = MultiBindingInterface.Load(@"c:\pdb\new_data_set.csv");

            var contactsPartner1 =
                data.Select(
                    a =>
                    {
                        var x = FindAtomicContacts.AtomPair.LoadAtomPairList(@"C:\pdb\new_data_set\contacts\contacts_" +
                                                                     a.InteractionChainsPdb1.ToLowerInvariant() + ".pdb")

                            .Where(
                                b =>
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb1Chain1.ToUpperInvariant()
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb1Chain2.ToUpperInvariant())
                                    ||
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb1Chain2.ToUpperInvariant()
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb1Chain1.ToUpperInvariant())).ToList();

                        x = x.Select(c =>
                        {
                            if (c.Atom1.chainID.FieldValue.ToUpperInvariant() == a.InteractionChainsPdb1Chain2.ToUpperInvariant())
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
                        var x = FindAtomicContacts.AtomPair.LoadAtomPairList(@"C:\pdb\new_data_set\contacts\contacts_" +
                                                                     a.InteractionChainsPdb2.ToLowerInvariant() + ".pdb")

                            .Where(
                                b =>
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb2Chain1.ToUpperInvariant()
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb2Chain2.ToUpperInvariant())
                                    ||
                                    (b.Atom1.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb2Chain2.ToUpperInvariant()
                                     &&
                                     b.Atom2.chainID.FieldValue.ToUpperInvariant() ==
                                     a.InteractionChainsPdb2Chain1.ToUpperInvariant())).ToList();

                        x = x.Select(c =>
                        {
                            if (c.Atom1.chainID.FieldValue.ToUpperInvariant() == a.InteractionChainsPdb2Chain2.ToUpperInvariant())
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

            var resultData = new List<string>();

            for (int index = 0; index < data.Count; index++)
            {
                var d = data[index];
                var cp1 = contactsPartner1[index];
                var cp2 = contactsPartner2[index];
                var ip1 = interfacePartner1[index];
                var ip2 = interfacePartner2[index];

                if (d == null || cp1 == null || cp2 == null || ip1 == null || ip2 == null) continue;
                if (cp1.Count == 0 || cp2.Count == 0) continue;

                var p1c1_pdb = FindAtomicContacts.PdbAtomicChains(@"c:\pdb\new_data_set\" + d.InteractionChainsPdb1 + ".pdb", new string[] { d.InteractionChainsPdb1Chain1 }, -1, -1, true);
                var p1c2_pdb = FindAtomicContacts.PdbAtomicChains(@"c:\pdb\new_data_set\" + d.InteractionChainsPdb1 + ".pdb", new string[] { d.InteractionChainsPdb1Chain2 }, -1, -1, true);
                var p2c1_pdb = FindAtomicContacts.PdbAtomicChains(@"c:\pdb\new_data_set\" + d.InteractionChainsPdb2 + ".pdb", new string[] { d.InteractionChainsPdb2Chain1 }, -1, -1, true);
                var p2c2_pdb = FindAtomicContacts.PdbAtomicChains(@"c:\pdb\new_data_set\" + d.InteractionChainsPdb2 + ".pdb", new string[] { d.InteractionChainsPdb2Chain2 }, -1, -1, true);

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
                    var middle = cp1a1_best50_middle_finder[cp1a1_best50_middle_finder.Count/2];
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

                    if (x + interface_target_length  >= cp2a1_max) break;
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
                    return l >= cp1a1_best50_min && l <= cp1a1_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp1a2_interface = string.Join("", p1c2_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp1a2_best50_min && l <= cp1a2_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp2a1_interface = string.Join("", p2c1_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp2a1_best50_min && l <= cp2a1_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp2a2_interface = string.Join("", p2c2_pdb.ChainList.First().AtomList.Where(a =>
                {
                    var l = int.Parse(a.resSeq.FieldValue);
                    return l >= cp2a2_best50_min && l <= cp2a2_best50_max;
                }).OrderBy(c => int.Parse(c.resSeq.FieldValue)).Select(b => AminoAcidConversions.AminoAcidNameToCode1L(b.resName.FieldValue)).ToList());

                var cp1a1_interface_interactions = new string('_', cp1a1_interface.Length);
                cp1a1_interface_interactions = string.Join("", cp1a1_interface_interactions.Select((a, i) => cp1a1_res_seq.Contains(i + cp1a1_best50_min) ? "X" : "_").ToList());

                var cp1a2_interface_interactions = new string('_', cp1a2_interface.Length);
                cp1a2_interface_interactions = string.Join("", cp1a2_interface_interactions.Select((a, i) => cp1a2_res_seq.Contains(i + cp1a2_best50_min) ? "X" : "_").ToList());

                var cp2a1_interface_interactions = new string('_', cp2a1_interface.Length);
                cp2a1_interface_interactions = string.Join("", cp2a1_interface_interactions.Select((a, i) => cp2a1_res_seq.Contains(i + cp2a1_best50_min) ? "X" : "_").ToList());

                var cp2a2_interface_interactions = new string('_', cp2a2_interface.Length);
                cp2a2_interface_interactions = string.Join("", cp2a2_interface_interactions.Select((a, i) => cp2a2_res_seq.Contains(i + cp2a2_best50_min) ? "X" : "_").ToList());

                resultData.Add(string.Join(",", new string[] {
                    d.DomainSuperFamily,
                    d.InteractionChainsPdb1,
                    d.InteractionChainsPdb1Chain1,""+ip1.Item1,""+ip1.Item2,""+cp1a1_best50_min,""+cp1a1_best50_max,""+cp1a1_best50_interactions,cp1a1_interface,cp1a1_interface_interactions,
                    d.InteractionChainsPdb1Chain2,""+ip1.Item3,""+ip1.Item4,""+cp1a2_best50_min,""+cp1a2_best50_max,""+cp1a2_best50_interactions,cp1a2_interface,cp1a2_interface_interactions,
                    d.InteractionChainsPdb2,
                    d.InteractionChainsPdb2Chain1,""+ip2.Item1,""+ip2.Item2,""+cp2a1_best50_min,""+cp2a1_best50_max,""+cp2a1_best50_interactions,cp2a1_interface,cp2a1_interface_interactions,
                    d.InteractionChainsPdb2Chain2,""+ip2.Item3,""+ip2.Item4,""+cp2a2_best50_min,""+cp2a2_best50_max,""+cp2a2_best50_interactions,cp2a2_interface,cp2a2_interface_interactions,
                }));

            }

            resultData.Insert(0,string.Join(",", new string[] {
                    "super family",

                    "partner 1 pdb id",
                    "partner 1 chain id 1", "p1c1 interface start", "p1c1 interface end", "p1c1 best 50 start", "p1c1 best 50 end","p1c1 best 50 interactions","p1c1 interface seq","p1c1 interface mask",
                    "partner 1 chain id 2", "p1c2 interface start", "p1c1 interface end", "p1c2 best 50 start", "p1c2 best 50 end","p1c2 best 50 interactions","p1c2 interface seq","p1c2 interface mask",
                    "partner 2 pdb id",
                    "partner 2 chain id 1", "p2c1 interface start", "p2c1 interface end", "p2c1 best 50 start", "p2c1 best 50 end","p2c1 best 50 interactions","p2c1 interface seq","p2c1 interface mask",
                    "partner 2 chain id 2", "p2c2 interface start", "p2c2 interface end", "p2c2 best 50 start", "p2c2 best 50 end","p2c2 best 50 interactions","p2c2 interface seq","p2c2 interface mask",
                    }));

            File.WriteAllLines(@"c:\pdb\new_data_set_results.csv", resultData);
            return;
        }
    }
}
