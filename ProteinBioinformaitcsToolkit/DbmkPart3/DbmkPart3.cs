using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmkPart3
{
    class DbmkPart3
    {
        public static void TrimModelInterface(string pdbFile, string outputFile, int start, int end, char chain = ' ')
        {
            if (!File.Exists(pdbFile)) return;
            const int atom_resseq = 22;
            const int atom_resseq_len = 4;

            const int atom_chain = 21;
            const int atom_chain_len = 1;

            var lines = File.ReadAllLines(pdbFile).ToList();

            lines = lines.Where(a =>
                                {
                                    if (!a.StartsWith("ATOM ")) return true;

                                    if (a[atom_chain] != chain) return true;

                                    var resId = int.Parse(a.Substring(atom_resseq, atom_resseq_len));


                                    return resId >= start && resId <= end;
                                }).ToList();

            //var pdbFileInt = Path.GetDirectoryName(pdbFile) + @"\" + Path.GetFileNameWithoutExtension(pdbFile) + "_int" + Path.GetExtension(pdbFile);
            //var pdbFileInt = outfolder + Path.GetFileNameWithoutExtension(pdbFile) + "_int" + Path.GetExtension(pdbFile);
            Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
            File.WriteAllLines(outputFile, lines);
        }
        static void Main(string[] args)
        {
            var d = Directory.GetFiles(@"C:\Users\k1040\Desktop\energies\all\csv2\", "*.csv", SearchOption.TopDirectoryOnly);

            var f = d.Select(a => File.ReadAllLines(a).Select(b=>b.Split(',').ToList()).ToList()).ToList();

            var act = 0;

            if (act == 0)
            {
                /*
                 * read csv files, find line with overlap 100%, get start and end positions
                 * output batch file for cutting those out
                 * 
                 * this is to cut out the interfacial area of the theoretical model (model_monomer.pdb)
                 * */
                for (var index = 0; index < d.Length; index++)
                {
                    var ff = f[index];
                    var dd = d[index];

                    var folder = @"C:\pdbe_split\models\" + Path.GetFileNameWithoutExtension(dd) + @"\pymol\";
                    var start = int.Parse(ff[1][4]);
                    var end = int.Parse(ff[1][5]);

                    var pdb = ff[1][1]; // pdb of the receiving ligand.
                    var chain = ff[1][2][0];
                    var partnerChain = ff[1][20][0];

                    var lig1pdb = ff[0][21].Replace("DOPE ", "");
                    var lig2pdb = ff[0][23].Replace("DOPE ", "");
                    var lig3pdb = ff[0][25].Replace("DOPE ", "");
                    var lig4pdb = ff[0][27].Replace("DOPE ", "");
                    var lig5pdb = ff[0][29].Replace("DOPE ", "");

                    var pdbDimerTemplate = @"C:\pdbe_split\" + pdb + String.Join("", new char[] { chain, partnerChain }.OrderBy(a => a).ToArray()) + ".pdb";
                    var pdbMonomerTemplate = @"C:\pdbe_split\models\" + Path.GetFileNameWithoutExtension(dd) + @"\" + "template_ligand_all.pdb";

                    if (!File.Exists(pdbMonomerTemplate)) continue;

                    TrimModelInterface(pdbDimerTemplate, folder + "if_template_dimer.pdb", start, end, chain);
                    TrimModelInterface(pdbMonomerTemplate, folder + "if_template_monomer.pdb",start, end, chain);
                    File.Copy(pdbDimerTemplate, folder + "all_template_dimer.pdb",true);
                    File.Copy(pdbMonomerTemplate, folder + "all_template_monomer.pdb",true);

                    //var pdbNativeModel = @""; // don't have it - only have the energy value

                    var pdbModelLig1 = string.IsNullOrWhiteSpace(lig1pdb) ? "" : @"C:\pdbe_split\models\" + lig1pdb + @"\model_monomer.pdb";
                    var pdbModelLig2 = string.IsNullOrWhiteSpace(lig2pdb) ? "" : @"C:\pdbe_split\models\" + lig2pdb + @"\model_monomer.pdb";
                    var pdbModelLig3 = string.IsNullOrWhiteSpace(lig3pdb) ? "" : @"C:\pdbe_split\models\" + lig3pdb + @"\model_monomer.pdb";
                    var pdbModelLig4 = string.IsNullOrWhiteSpace(lig4pdb) ? "" : @"C:\pdbe_split\models\" + lig4pdb + @"\model_monomer.pdb";
                    var pdbModelLig5 = string.IsNullOrWhiteSpace(lig5pdb) ? "" : @"C:\pdbe_split\models\" + lig5pdb + @"\model_monomer.pdb";

                    TrimModelInterface(pdbModelLig1,folder + "if_1_" + lig1pdb.Substring(lig1pdb.LastIndexOf(@"\") + 1) + ".pdb", start, end);
                    TrimModelInterface(pdbModelLig2,folder + "if_2_" + lig2pdb.Substring(lig2pdb.LastIndexOf(@"\") + 1) + ".pdb", start, end);
                    TrimModelInterface(pdbModelLig3,folder + "if_3_" + lig3pdb.Substring(lig3pdb.LastIndexOf(@"\") + 1) + ".pdb", start, end);
                    TrimModelInterface(pdbModelLig4,folder + "if_4_" + lig4pdb.Substring(lig4pdb.LastIndexOf(@"\") + 1) + ".pdb", start, end);
                    TrimModelInterface(pdbModelLig5,folder + "if_5_" + lig5pdb.Substring(lig5pdb.LastIndexOf(@"\") + 1) + ".pdb", start, end);

                    if (File.Exists(pdbModelLig1))  File.Copy(pdbModelLig1, folder + "all_1_" + lig1pdb.Substring(lig1pdb.LastIndexOf(@"\") + 1) + ".pdb", true);
                    if (File.Exists(pdbModelLig2))  File.Copy(pdbModelLig2, folder + "all_2_" + lig2pdb.Substring(lig1pdb.LastIndexOf(@"\") + 1) + ".pdb", true);
                    if (File.Exists(pdbModelLig3))  File.Copy(pdbModelLig3, folder + "all_3_" + lig3pdb.Substring(lig1pdb.LastIndexOf(@"\") + 1) + ".pdb", true);
                    if (File.Exists(pdbModelLig4))  File.Copy(pdbModelLig4, folder + "all_4_" + lig4pdb.Substring(lig1pdb.LastIndexOf(@"\") + 1) + ".pdb", true);
                    if (File.Exists(pdbModelLig5))  File.Copy(pdbModelLig5, folder + "all_5_" + lig5pdb.Substring(lig1pdb.LastIndexOf(@"\") + 1) + ".pdb", true);

                    //var text = "ComplexAtoms " + lig1pdb + ".pdb ";
                    //return;
                }
            }

           if (act == 1)
            {
                var x = f.Select(a => !string.IsNullOrWhiteSpace(a[0][21])).ToList();

                for (var index = 0; index < d.Length; index++)
                {
                    var i = d[index];

                    var k = x[index];

                    if (k) Debug.WriteLine(@"move """ + i + @""" with_subs");
                }
            }

            if (act == 2)
            {
                //=ABS(AF2-O2)/ABS(O2-MAX(K:K))
                // file, interface position within energy landscape,
                var data = new List<object>();//List<Tuple<string, decimal>>();

                for (var index = 0; index < d.Length; index++)
                {
                    var ff = f[index];
                    var dd = d[index];

                    dd = dd.Replace(@"C:\Users\k1040\Desktop\energies\all\csv2\", "").Replace(".csv", ".*");

                    var ff_min_dope = ff.Skip(1).Select(a => decimal.Parse(a[10])).Min();
                    var ff_max_dope = ff.Skip(1).Select(a => decimal.Parse(a[10])).Max();

                    var diff = Math.Abs(ff_max_dope - ff_min_dope);

                    // 6 18

                    var len = int.Parse(ff[1][6]);

                    var row = ff.Skip(1).First(a => int.Parse(a[18]) == len + 1);
                    var native_dope = decimal.Parse(row[10]);

                    var lig1_dope = row[21] != "" ? decimal.Parse(row[21]) : ff_max_dope;
                    var lig2_dope = row[23] != "" ? decimal.Parse(row[23]) : ff_max_dope;
                    var lig3_dope = row[25] != "" ? decimal.Parse(row[25]) : ff_max_dope;
                    var lig4_dope = row[27] != "" ? decimal.Parse(row[27]) : ff_max_dope;
                    var lig5_dope = row[29] != "" ? decimal.Parse(row[29]) : ff_max_dope;

                    if (lig1_dope < ff_min_dope || lig1_dope > ff_max_dope) lig1_dope = ff_max_dope;
                    if (lig2_dope < ff_min_dope || lig2_dope > ff_max_dope) lig2_dope = ff_max_dope;
                    if (lig3_dope < ff_min_dope || lig3_dope > ff_max_dope) lig3_dope = ff_max_dope;
                    if (lig4_dope < ff_min_dope || lig4_dope > ff_max_dope) lig4_dope = ff_max_dope;
                    if (lig5_dope < ff_min_dope || lig5_dope > ff_max_dope) lig5_dope = ff_max_dope;

                    var pctNative = Math.Abs(native_dope - ff_min_dope) / Math.Abs(ff_max_dope - ff_min_dope);

                    var pctLig1 = Math.Abs(lig1_dope - ff_min_dope) / Math.Abs(ff_max_dope - ff_min_dope);
                    var pctLig2 = Math.Abs(lig2_dope - ff_min_dope) / Math.Abs(ff_max_dope - ff_min_dope);
                    var pctLig3 = Math.Abs(lig3_dope - ff_min_dope) / Math.Abs(ff_max_dope - ff_min_dope);
                    var pctLig4 = Math.Abs(lig4_dope - ff_min_dope) / Math.Abs(ff_max_dope - ff_min_dope);
                    var pctLig5 = Math.Abs(lig5_dope - ff_min_dope) / Math.Abs(ff_max_dope - ff_min_dope);

                    
                    data.Add(String.Join(",", new object[] { dd,pctNative, pctLig1, pctLig2, pctLig3, pctLig4, pctLig5 }));
                    // 21 .. 30
                    // File.WriteAllLines(@"c:\users\k1040\desktop\percentlist.txt", data.Select(a=>a.Item1+","+a.Item2).ToList());
                    File.WriteAllLines(@"c:\users\k1040\desktop\percentlist.txt", data.Select(a=>a.ToString()).ToList());
                }


            }

            return;
        }
    }
}
