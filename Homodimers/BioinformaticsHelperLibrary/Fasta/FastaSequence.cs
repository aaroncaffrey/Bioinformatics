using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.Fasta
{
    public class FastaSequence
    {
        public string[] Id;

        public string Sequence;

        public FastaSequence(string[] id, string sequence)
        {
            Id = id;
            Sequence = sequence;
        }

        public static List<FastaSequence> LoadFastaFile(string filename)
        {
            var result = new List<FastaSequence>();

            var data = File.ReadAllLines(filename);

            FastaSequence seq = null;


            for (int index = 0; index < data.Length; index++)
            {
                var line = data[index];

                if (string.IsNullOrEmpty(line)) continue;

                if (line[0] == '>')
                {
                    var idLine = line.Substring(1);
                    var idArray = idLine.Split(new char[] { ':', '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);


                    seq = new FastaSequence(idArray, "");
                    result.Add(seq);
                    continue;

                }

                if (seq == null) continue;

                seq.Sequence += line.Replace("\n", "").Replace("\r", "");

                //if (index % 1000 == 0) Console.WriteLine("index: " + index);
            }

            return result;
        }
    }
}
