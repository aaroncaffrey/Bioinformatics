using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadIbisReceptorLigandList
{
    public class LoadIbisReceptorLigandList
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

        public static string PymolChainSelector(string PdbId, string ChainId, int index) { return @"/" + PdbId + (index!=-1?"_"+index:"") + @"//" + ChainId; }
        public static void PymolAlignmentCmds(string mainReceptorPdbId, List<Tuple<string, List<int>, List<int>>> receptorLigandList, string saveFilename)
        {
            for (var index = 0; index <= 4; index++)
            {
                var mainReceptorIdAlt = mainReceptorPdbId + (index != -1 ? "_" + index : "");

                if (!receptorLigandList.Any(a => a.Item1.StartsWith(mainReceptorPdbId.Substring(0,5)))) { mainReceptorIdAlt = receptorLigandList[0].Item1.Substring(0, 5) + (index != -1 ? "_" + index : ""); }

                var pymol_header = @"
import os
import sys
import pymol
from pymol import cmd
";
                var pymol_setup = @"
cmd.set(""ray_trace_fog"",1)
cmd.set(""ray_shadows"", 1)
cmd.set(""antialias"", 1)
cmd.set(""use_shaders"", 1)

cmd.hide(""all"")
cmd.show(""cartoon"")
cmd.reset()
cmd.set(""cartoon_transparency"", ""0.15"")
cmd.set(""transparency"", ""0.15"")         
";

                var pymol_load_align1 = @"cmd.super(""%homolog_receptor_object%"", ""%main_receptor_object%"")";
                var pymol_load_align2 = @"          
cmd.color(""grey"", ""%homolog_receptor_object%"")
#cmd.show(""surf"", ""%homolog_receptor_object%"")          
cmd.create(""rec_%homolog_receptor%"", ""%homolog_receptor_object%"")
cmd.create(""lig_%homolog_ligand%"", ""%homolog_ligand_object%"")
#cmd.show(""surf"", ""rec_%homolog_receptor% within 5. of lig_%homolog_ligand%"")
#cmd.show(""surf"", ""lig_%homolog_ligand% within 5. of rec_%homolog_receptor%"")
#cmd.color(""red"", ""rec_%homolog_receptor% within 5. of lig_%homolog_ligand%"")
#cmd.color(""red"", ""lig_%homolog_ligand% within 5. of rec_%homolog_receptor%"")
cmd.color(""warmpink"", ""lig_%homolog_ligand% & resi %ibis_interactions%"")
cmd.color(""red"", ""lig_%homolog_ligand% & resi %pdbsum_interactions%"")
";

                var pymol3 = @"
cmd.reset()
cmd.disable(""*"")
cmd.enable(""lig*"")
#cmd.enable(""rec_%homolog_receptor%"")
#cmd.show(""surf"", ""rec_%homolog_receptor%"")
";

                var pymol4 = @"
cmd.turn(""xyz"",90)
cmd.png(""%png%_1.png"")
cmd.turn(""xyz"",90)
cmd.png(""%png%_2.png"")
cmd.turn(""xyz"",90)
cmd.png(""%png%_3.png"")
cmd.turn(""xyz"",90)
cmd.png(""%png%_4.png"")
#cmd.quit()
";

                var result1 = receptorLigandList.Select(a => pymol_load_align1
.Replace("%main_receptor_object%", PymolChainSelector(receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(0, 4) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(10, 1), receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1), index))
.Replace("%homolog_receptor_object%", PymolChainSelector(a.Item1.Substring(0, 4) + a.Item1.Substring(4, 1) + a.Item1.Substring(10, 1), a.Item1.Substring(4, 1), index))
.Replace("%homolog_ligand_object%", PymolChainSelector(a.Item1.Substring(0, 4) + a.Item1.Substring(4, 1) + a.Item1.Substring(10, 1), a.Item1.Substring(10, 1), index))
.Replace("%homolog_receptor%", a.Item1.Substring(0, 4) + a.Item1.Substring(4, 1) + (index != -1 ? "_" + index : ""))
.Replace("%homolog_ligand%", a.Item1.Substring(0, 4) + a.Item1.Substring(10, 1) + (index != -1 ? "_" + index : ""))).ToList();

                var result2 = receptorLigandList.Select(a => pymol_load_align2
.Replace("%main_receptor_object%", PymolChainSelector(receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(0, 4) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(10, 1), receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1), index))
.Replace("%homolog_receptor_object%", PymolChainSelector(a.Item1.Substring(0, 4) + a.Item1.Substring(4, 1) + a.Item1.Substring(10, 1), a.Item1.Substring(4, 1), index))
.Replace("%homolog_ligand_object%", PymolChainSelector(a.Item1.Substring(0, 4) + a.Item1.Substring(4, 1) + a.Item1.Substring(10, 1), a.Item1.Substring(10, 1), index))
.Replace("%pdbsum_interactions%", string.Join("+", a.Item2))
.Replace("%ibis_interactions%", string.Join("+", a.Item3))
.Replace("%homolog_receptor%", a.Item1.Substring(0, 4) + a.Item1.Substring(4, 1) + (index != -1 ? "_" + index : ""))
.Replace("%homolog_ligand%", a.Item1.Substring(0, 4) + a.Item1.Substring(10, 1) + (index != -1 ? "_" + index : ""))).ToList();


                var result = new List<string>();

                result.AddRange(result1);
                result.AddRange(result2);
                result.Add(pymol3
.Replace("%main_receptor_object%", PymolChainSelector(receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(0, 4) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(10, 1), receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1), index))
.Replace("%homolog_receptor%", mainReceptorIdAlt)
.Replace("%png%", @"c:\pdbe_split\receptors\png\"
.Replace(@"\", @"\\") + mainReceptorPdbId + (index != -1 ? "_" + index : "")));

                result.Add(pymol4
.Replace("%main_receptor_object%", PymolChainSelector(receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(0, 4) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1) + receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(10, 1), receptorLigandList.First(b => b.Item1.StartsWith(mainReceptorIdAlt.Substring(0, 5))).Item1.Substring(4, 1), index))
.Replace("%homolog_receptor%", mainReceptorIdAlt)
.Replace("%png%", @"c:\pdbe_split\receptors\png\"
.Replace(@"\", @"\\") + mainReceptorPdbId + (index != -1 ? "_" + index : "")));

                result.Insert(0, pymol_setup);


                result.Insert(0, string.Join("\r\n", receptorLigandList.Select(a => @"cmd.load(""" + PdbRecLigFile(mainReceptorPdbId, a.Item1, index)
.Replace(@"\", @"\\") + @""")").ToList()));

                result.Insert(0, pymol_header);
                File.WriteAllLines(Path.GetDirectoryName(saveFilename) + @"\" + Path.GetFileNameWithoutExtension(saveFilename) + @"_" + index + Path.GetExtension(saveFilename), result);
            }
        }

        public static double Dist(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            var d = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2));
            return d;
        }
        public static List<int> TruncateDimerContacts(string pdbFile, char recChain, char ligChain, string pdbOutputFile, double contactDistance = 4.01)
        {
            const int atom_chain = 21;
            const int atom_resseq = 22;
            const int atom_resseq_len = 4;

            var pdb = File.ReadAllLines(pdbFile).ToList();

            if (pdb.Any(a => a.StartsWith("MODEL "))) pdb = pdb.Where((a, i) => i < pdb.FindIndex(b => b.StartsWith("ENDMDL "))).ToList();

            pdb = pdb.Where(a => a.StartsWith("ATOM ") && (recChain == a[atom_chain] || ligChain == a[atom_chain])).ToList();



            var chainIds = pdb.Select(a => a[atom_chain]).Distinct().ToList();

            if (chainIds.Count != 2) return null;


            var atoms = pdb.Select(a => new Tuple<char, int, double, double, double, string, int>(a[atom_chain], int.Parse(a.Substring(atom_resseq, atom_resseq_len)), double.Parse(a.Substring(31 - 1, (38 - 31) + 1)), double.Parse(a.Substring(39 - 1, (46 - 39) + 1)), double.Parse(a.Substring(47 - 1, (54 - 47) + 1)), a, int.Parse(a.Substring(7 - 1, (11 - 7) + 1)))).ToList();
            atoms = atoms.OrderBy(a => a.Item1).ThenBy(a => a.Item2).ThenBy(a => a.Item7).ToList();

            var ligAaTotal = atoms.Where(a => a.Item1 == ligChain).Select(a => a.Item2).Distinct().Count();

            if (ligAaTotal < 50) { return null; }

            if (atoms.Count == 0) return null;

            //var atomsCNO = atoms.Where(a => a.Item6[13] == 'C' || a.Item6[13] == 'N' || a.Item6[13] == 'O').ToList();
            var atomsCNO = atoms;//atoms.Where(a => a.Item6[13] == 'C' || a.Item6[13] == 'N' || a.Item6[13] == 'O').ToList();

            var pdbsum = new List<Tuple<Tuple<char, int, double, double, double, string, int>, Tuple<char, int, double, double, double, string, int>, double>>();
            var contacts1 = atomsCNO.Where(a => a.Item1 == ligChain).Select(a => atomsCNO.Where(b => b.Item1 == recChain).Select(b =>
            {
                var d = Dist(a.Item3, a.Item4, a.Item5, b.Item3, b.Item4, b.Item5);
                var r = new List<Tuple<char, int, double, double, double, string, int>>();
                if (d < contactDistance)
                {
                    r.Add(a);
                    r.Add(b);
                    pdbsum.Add(new Tuple<Tuple<char, int, double, double, double, string, int>, Tuple<char, int, double, double, double, string, int>, double>(a, b, d));
                }
                return r;
            }).Distinct().ToList()).Distinct().ToList();
            var contacts = contacts1.SelectMany(a => a.SelectMany(b => b)).ToList();

            if (contacts.Count == 0) return null;

            var minMaxFlank = 0;
            var min = chainIds.Select(a => contacts.Where(b => b.Item1 == a).Select(b => b.Item2).Min() - minMaxFlank).ToArray();
            var max = chainIds.Select(a => contacts.Where(b => b.Item1 == a).Select(b => b.Item2).Max() + minMaxFlank).ToArray();

            var flank = 0;
            var interfacesAreasFlankZero = atoms.Where(atom => atom.Item1 == ligChain && contacts.Any(contact => atom.Item1 == contact.Item1 && (atom.Item2 >= contact.Item2 - flank && atom.Item2 <= contact.Item2 + flank))).ToList();
            flank = 1;
            var interfacesAreasFlankOne = atoms.Where(atom => atom.Item1 == ligChain && contacts.Any(contact => atom.Item1 == contact.Item1 && (atom.Item2 >= contact.Item2 - flank && atom.Item2 <= contact.Item2 + flank))).ToList();
            flank = 6;
            var interfacesAreasFlankSix = atoms.Where(atom => atom.Item1 == ligChain && contacts.Any(contact => atom.Item1 == contact.Item1 && (atom.Item2 >= contact.Item2 - flank && atom.Item2 <= contact.Item2 + flank))).ToList();

            var interfacesFirstToLast = atoms.Where(a => a.Item2 >= min[chainIds.IndexOf(a.Item1)] && a.Item2 <= max[chainIds.IndexOf(a.Item1)]).ToList();


            var receptorAtoms = atoms.Where(a => a.Item1 == recChain).ToList();

            Directory.CreateDirectory(Path.GetDirectoryName(pdbOutputFile));

            if (pdbsum.Count > 0) { File.WriteAllLines(Path.GetDirectoryName(pdbOutputFile) + @"\pdbsum_" + Path.GetFileNameWithoutExtension(pdbFile) + recChain + ligChain + ".txt", pdbsum.Select(a => string.Join(" ", new string[] { "" + a.Item1.Item1, "" + a.Item1.Item7, "" + a.Item1.Item2, ":", "" + a.Item2.Item1, "" + a.Item2.Item7, "" + a.Item2.Item2, "=", "" + Math.Round(a.Item3, 2) })).ToList()); }


            if (interfacesAreasFlankZero.Count > 0)
            {
                for (var i = 0; i <= 4; i++)
                {
                    List<Tuple<char, int, double, double, double, string, int>> ligandAtoms = null;

                    if (i == 0) ligandAtoms = interfacesAreasFlankZero.Where(a => a.Item1 == ligChain).ToList();
                    else if (i == 1) ligandAtoms = interfacesAreasFlankOne.Where(a => a.Item1 == ligChain).ToList();
                    else if (i == 2) ligandAtoms = interfacesAreasFlankSix.Where(a => a.Item1 == ligChain).ToList();
                    else if (i == 3) ligandAtoms = interfacesFirstToLast.Where(a => a.Item1 == ligChain).ToList();
                    else if (i == 4) ligandAtoms = atoms.Where(a => a.Item1 == ligChain).ToList();

                    var dimer = new List<Tuple<char, int, double, double, double, string, int>>();
                    dimer.AddRange(receptorAtoms);
                    dimer.AddRange(ligandAtoms);
                    File.WriteAllLines(Path.GetDirectoryName(pdbOutputFile) + @"\" + Path.GetFileNameWithoutExtension(pdbOutputFile) + @"_" + i + ".pdb", dimer.Select(a => a.Item6).ToList());
                }
            }

            var pdbsumLigandInteractionResidues = contacts.Where(a => a.Item1 == ligChain).Select(a => a.Item2).Distinct().ToList();
            return pdbsumLigandInteractionResidues;
        }

        public static string PdbRecLigFile(string mainReceptorPdbId, string pdbEvidence, int index) { return @"c:\pdbe_split\receptors\" + mainReceptorPdbId + @"\" + pdbEvidence.Substring(0, 4) + pdbEvidence.Substring(4, 1) + pdbEvidence.Substring(10, 1) + (index != -1 ? "_" + index : "") + ".pdb"; }
        public static void LoadReceptor(string mainReceptorPdbId)
        {
            Debug.WriteLine(mainReceptorPdbId);
            //var mainReceptorPdbId = "2SICE";
            //if (mainReceptorPdbId != "1KXPA" && mainReceptorPdbId != "2SICE") return;

            if (mainReceptorPdbId.Length != 5) return;
            mainReceptorPdbId = mainReceptorPdbId.ToUpperInvariant();

            var pdbFile = @"c:\pdbe\" + mainReceptorPdbId.Substring(0, 4) + ".pdb";
            var ibisFile = @"c:\phd\ibis\" + mainReceptorPdbId.Substring(1, 2) + @"\" + mainReceptorPdbId.Substring(0, 4) + ".txt";
            var ibisData = IbisData.Load(ibisFile);

            if (ibisData == null || ibisData.Count == 0) return;

            ibisData = ibisData.Where(a => a.Query.ToUpperInvariant() == mainReceptorPdbId.ToUpperInvariant()).ToList();
            ibisData = ibisData.Where(a => a.Interaction_type == "PPI").ToList();
            ibisData = ibisData.Where(a =>
            {
                var pdbEvience = a.PDB_Evidence.Trim().Split('_');
                var rec = pdbEvience[0];
                var lig = pdbEvience[1];
                return /*pdbEvience[0]== mainReceptorPdbId ||*/ rec.Length == 5 && lig.Length == 5 && rec != lig;
            }).ToList();

            // at least one matching resid?
            //var mmResNos = ibisData.Select(a => a.Mmdb_Residue_No.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)).Select(b => b.Select(int.Parse).ToList()).ToList();
            //ibisData = ibisData.Where((a, i) => mmResNos.Any(b => mmResNos.IndexOf(b) != i && b.Intersect(mmResNos[i]).Any())).ToList();

            // at least one matching domain?

            const int minLigands = 2;
            const int maxLigands = 400;

            if (ibisData.Count >= minLigands && ibisData.Count <= maxLigands)
            {
                Directory.CreateDirectory(@"c:\pdbe_split\receptors\" + mainReceptorPdbId + @"\");
                var pdbEvidenceList = ibisData.Select(a => new Tuple<string, List<int>>(a.PDB_Evidence, a.PDB_Residue_No.Trim().Split(' ').Select(b=>int.Parse(string.Join("",b.Where(c=>char.IsNumber(c))))).ToList())).ToList();
                var pdbsumInteractonList = pdbEvidenceList.Select(a =>
                {
                    var origPdbFile = @"c:\pdbe\" + a.Item1.Substring(0, 4) + ".pdb";
                    var recChain = a.Item1[4];
                    var ligChain = a.Item1[10];
                    var outPdb = PdbRecLigFile(mainReceptorPdbId, a.Item1, -1);
                    //ExtractAtoms(origPdbFile, "-", evidenceChains, outPdb);
                    var pdbsumInteractions = TruncateDimerContacts(origPdbFile, recChain, ligChain, outPdb);

                    var ibisInteractions = a.Item2;

                    return new Tuple<string, List<int>, List<int>>(a.Item1, pdbsumInteractions, ibisInteractions);
                }).ToList();

                //pdbEvidenceList = pdbEvidenceList.Where((a, i) => success[i] || a.StartsWith(mainReceptorPdbId)).ToList();
                //pdbEvidenceList = pdbEvidenceList.Where((a, i) => pdbsumInteractonList[i].Item2 != null).ToList();

                pdbsumInteractonList = pdbsumInteractonList.Where(a => a.Item2 != null).ToList();

                File.WriteAllLines(@"c:\pdbe_split\receptors\" + mainReceptorPdbId + @"\rec_lig_list_" + mainReceptorPdbId + ".txt", pdbsumInteractonList.Select(a => string.Join("\t", new List<string>() { a.Item1, string.Join("+", a.Item2), string.Join("+", a.Item3) })));

                PymolAlignmentCmds(mainReceptorPdbId, pdbsumInteractonList, @"c:\pdbe_split\receptors\" + mainReceptorPdbId + @"\pymol_images.py");

                var batFilename = @"c:\pdbe_split\receptors\" + mainReceptorPdbId + @"\pymol_images.bat";


                var batData = File.ReadAllLines(@"c:\phd\modeller_scripts\pymol_images.bat");
                for (var i = 0; i <= 4; i++)
                {
                    File.WriteAllLines(Path.GetDirectoryName(batFilename)+@"\"+Path.GetFileNameWithoutExtension(batFilename) +"_"+i+Path.GetExtension(batFilename),batData.Select(a=>a.Replace("pymol_images.py","pymol_images_"+i+".py").Replace("pymol -e","pymol")).ToList());
                }

                var start = false;
                if (start)
                {
                    var process = new Process() { StartInfo = new ProcessStartInfo(batFilename) };
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(batFilename);
                    process.Start();
                    process.WaitForExit();
                    Thread.Sleep(1000);
                }
            }
        }

        public static void Main(string[] args)
        {


            var rec = receptorList.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).Distinct().ToList();

            //rec = rec.Skip(rec.FindIndex(a=>a=="1GHQA")).ToList();
            rec.ForEach(a => LoadReceptor(a));
        }

        public static string receptorList = @"
1TGSZ
2GTPA
1CGIE
1KIGH
1LDTT
1SGFZ
1ACBE
1FLEE
1BRCE
1FQJA
1LDTT
1F2SE
1GL1A
1TGSZ
4DN4M
1ACBE
1BRCE
1C9PA
1CGIE
1D6RA
1EAIB
1GL0E
1GL1A
1LDTT
1LDTT
1MCVA
1MCVA
1PPFE
1SGFZ
";
    }
}
