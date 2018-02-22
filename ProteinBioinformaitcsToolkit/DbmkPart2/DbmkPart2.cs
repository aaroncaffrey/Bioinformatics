using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmkPart2
{
    class DbmkPart2
    {

        public static void process(string csvFileLigand, List<List<string>> energyScores)
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
            var datasplit = data.Select(a => a.Split(',').ToList()).ToList();
            datasplit.ForEach(a =>
                                              {
                                                  while (a.Count<21) a.Add("");
                                              });

            var firstdataline = data[1];

            var firstdatalineSplit = firstdataline.Split(',');

            var csvPdbId = firstdatalineSplit[1];
            var csvChainId = firstdatalineSplit[2][0];
            var interfaceIndex = int.Parse(firstdatalineSplit[3]); // might not match?
            var interfaceStart = int.Parse(firstdatalineSplit[4]);
            var interfaceEnd = int.Parse(firstdatalineSplit[5]);
            var interfaceLength = int.Parse(firstdatalineSplit[6]);
            var csvPartnerChainId = firstdatalineSplit[firstdatalineSplit.Length - 1][0];

            var startCol = 21;

            var scores = energyScores.Where(a => a[0].StartsWith(@"c:\pdbe_split\models\" + csvPdbId + csvChainId + interfaceIndex + @"\")).ToList();



            for (int index = 0; index < datasplit.Count; index++)
            {
                var a = datasplit[index];
                foreach (var s in scores)
                {
                    if (index == 0)
                    {
                        var x = s[0].Replace(@"c:\pdbe_split\models\", "");
                        a.Add("DOPE " + x);
                        a.Add("DOPE-HR " + x);
                    }
                    else
                    {


                        a.Add(s[2]);
                        a.Add(s[3]);
                    }
                }
            }

            datasplit.ForEach(a =>
            {
                while (a.Count < 21 + 5 + 5) a.Add("");
            });

            File.WriteAllLines(@"C:\Users\k1040\Desktop\energies\all\csv2\" + Path.GetFileName(csvFileLigand),datasplit.Select(a=>string.Join(",",a)).ToList());
            return;
        }

        static void Main(string[] args)
        {
            var energyScores = File.ReadAllLines(@"C:\pdbe_split\models\PRMB2066-AA_energy_model_monomer.pdb.csv").Select(a=>a.Split(',').ToList()).ToList();

            var folder = @"C:\Users\k1040\Desktop\energies\all\csv\";
            var files = Directory.GetFiles(folder, "*.csv", SearchOption.AllDirectories).ToList();

            files.ForEach(a => process(a, energyScores));

            //var datas = files.Select(a => File.ReadAllLines(a).Select(b=>b.Split(',')).ToList()).ToList();

            return;
            //files.ForEach(a => FindReceptorEntries(a));

        }
    }
}
