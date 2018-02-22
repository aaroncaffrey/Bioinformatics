using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;

namespace MutateSequence
{
    public class MutateSequence
    {

        static void Main(string[] args)
        {
            //var s1 =
            //    @"XXXXXXXXXXXXXXXXXXXXKKVKVSHRSHSTEPGLVLTLGQGDVGQLGLGENVMERKKPALVSIPEDVVQAEAGGMHTVCLSKSGQVYSFGCNDEGALGRDTSVEGSEMVPGKVELQEKVVQVSAGDSHTAALTDDGRVFLWGSFRDNNGVIGLLEPMKKSMVPVQVQLDVPVVKVASGNDHLVMLTADGDLYTLGCGEQGQLGRVPELFANRGGRQGLERLLVPKCVMLKSRGSRGHVRFQDAFCGAYFTFAISHEGHVYGFGLSNYHQLGTPGTESCFIPQNLTSFKNSTKSWVGFSGGQHHTVCMDSEGKAYSLGRAEYGRLGLGEGAEEKSIPTLISRLPAVSSVACGASVGYAVTKDGRVFAWGMGTNYQLGTGQDEDAWSPVEMMGKQLENRVVLSVSSGGQHTVLLVKDKEQS";

            //var s2 = @"RRSPPADAIPKSKKVKVSHRSHSTEPGLVLTLGQGDVGQLGLGENVMERKKPALVSIPEDVVQAEAGGMHTVCLSKSGQVYSFGCNDEGALGRDTSVEGSEMVPGKVELQEKVVQVSAGDSHTAALTDDGRVFLWGSFRDNNGVIGLLEPMKKSMVPVQVQLDVPVVKVASGNDHLVMLTADGDLYTLGCGEQGQLGRVPELFANRGGRQGLERLLVPKCVMLKSRGSRGHVRFQDAFCGAYFTFAISHEGHVYGFGLSNYHQLGTPGTESCFIPQNLTSFKNSTKSWVGFSGGQHHTVCMDSEGKAYSLGRAEYGRLGLGEGAEEKSIPTLISRLPAVSSVACGASVGYAVTKDGRVFAWGMGTNYQLGTGQDEDAWSPVEMMGKQLENRVVLSVSSGGQHTVLLVKDKEQS";

            //var x = SimpleAlignmentOffset(s1,s2);

            //Console.WriteLine();
            //Console.WriteLine(x.Item1);
            //Console.WriteLine(x.Item2);
            //Console.WriteLine();
            //Console.ReadLine();
            //return;

            // MutateSequence example.fasta start end mutation original (will find closest to start/end in case of sequence/structure index misalignment)

            var parameters = new string[,]
            {
                {"[input_fasta_file]", "fasta file with sequence to mutate"},
                {"[chain_ids]", "chain ids to mutate"},
                {"[start_positions]", "mutation start position (one based)"},
                {"[end_positions]", "mutation end position (one based)"},
                {"[offsets]", "offsets (for where pdb index doesn't match fasta sequence index) (one based)"},
                {"[mutation_sequence]", "new amino acids to overwrite with"},
                {"[[output_fasta_file]]", "optional output fasta file.  when ommitted, output to console"},
            };

            var maxParamLength = parameters.Cast<string>().Where((a, i) => i%2 == 0).Max(a => a.Length);
            var exeFilename = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            if (args.Length < 5)
            {
                Console.WriteLine(exeFilename +
                                  @" is a program to mutate (substitute) a subsequence of a protein amino acid sequence within a fasta file.");
                Console.WriteLine();
                Console.WriteLine(@"Usage:");
                Console.WriteLine(
                    ProteinBioClass.WrapConsoleText(
                        exeFilename + @" " + String.Join(" ", parameters.Cast<string>().Where((a, i) => i%2 == 0)),
                        maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Example:");
                Console.WriteLine(
                    ProteinBioClass.WrapConsoleText(
                        exeFilename +
                        @" ""c:\pdb_db\fasta\fasta_pdb1a12.pdb.fasta"" A,B,C 10,76,100 15,77,102 GBVBGA,AA,GHG ""c:\pdb_db\fasta_mutated\mutated_pdb1a12.pdb.fasta""",
                        maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Arguments:");
                for (var i = 0; i < parameters.GetLength(0); i++)
                    Console.WriteLine(@" " + parameters[i, 0].PadLeft(maxParamLength, ' ') + " " +
                                      ProteinBioClass.WrapConsoleText(parameters[i, 1], maxParamLength + 2, 1, false));
                Console.WriteLine();
                return;
            }

            var p = 0;
            var input_fasta_file = args.Length > p && args[p].Length > 0 ? args[p] : "";
            input_fasta_file = input_fasta_file.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + input_fasta_file);

            p++;
            var chain_ids = args.Length > p && args[p].Length > 0 ? args[p] : "";
            chain_ids = chain_ids.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + chain_ids);
            var chain_ids_split = chain_ids.ToUpperInvariant().Split(',');

            p++;
            var start_position = args.Length > p && args[p].Length > 0 ? args[p] : "";
            start_position = start_position.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + start_position);
            var start_position_split = start_position.Split(',').Select(int.Parse).ToArray();

            p++;
            var end_position = args.Length > p && args[p].Length > 0 ? args[p] : "";
            end_position = end_position.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + end_position);
            var end_position_split = end_position.Split(',').Select(int.Parse).ToArray();

            p++;
            var offset_position = args.Length > p && args[p].Length > 0 ? args[p] : "";
            offset_position = offset_position.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + offset_position);
            var offset_position_split = offset_position.Split(',').Select(int.Parse).ToArray();

            p++;
            var mutation_sequence = args.Length > p && args[p].Length > 0 ? args[p] : "";
            mutation_sequence = mutation_sequence.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + mutation_sequence);
            var mutation_sequence_split = mutation_sequence.Split(',');

            p++;
            var output_fasta_file = args.Length > p && args[p].Length > 0 ? args[p] : "";
            output_fasta_file = output_fasta_file.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + output_fasta_file);

            Console.WriteLine();

            MutateFastaSequenceSave(input_fasta_file, chain_ids_split, start_position_split, end_position_split,
                offset_position_split, mutation_sequence_split, output_fasta_file);
        }

        public static void MutateFastaSequenceSave(string input_fasta_file, string chain_id,
            int start_position, int end_position, int offset_position,
            string mutation_sequence, string output_fasta_file)
        {
            MutateFastaSequenceSave(input_fasta_file,new []{chain_id},new []{start_position}, new []{end_position},new []{offset_position},new []{mutation_sequence},output_fasta_file);
        }

        public static void MutateFastaSequenceSave(string input_fasta_file, string[] chain_ids_split,
            int[] start_position_split, int[] end_position_split, int[] offset_position_split,
            string[] mutation_sequence_split, string output_fasta_file)
        {
            var result = MutateFastaSequenceReturnFormattedData(input_fasta_file, chain_ids_split, start_position_split, end_position_split,
                offset_position_split, mutation_sequence_split);

            MutateFastaSequenceSave(result, output_fasta_file);
        }

        public static void MutateFastaSequenceSave(List<string> result, string output_fasta_file)//, SequenceFormat sequenceFormat = SequenceFormat.Fasta)
        {
            if (!string.IsNullOrWhiteSpace(output_fasta_file))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(output_fasta_file));
                File.WriteAllLines(output_fasta_file, result);
            }
            else
            {
                foreach (var line in result)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine();
            }
        }







        public static List<Tuple<string, string>> MutateFastaSequenceReturnList(string[] fastaFormattedSequence, string[] chain_ids_split, int[] start_position_split, int[] end_position_split, int[] offset_position_split, string[] mutation_sequence_split)
        {
            var sequenceList = Sequence.LoadSequenceFile(fastaFormattedSequence);
            var fastaInput = Sequence.GetAsTuple(sequenceList);

            var fastaOutput = new List<Tuple<string, string>>();

            var headerSplitChars = new char[] { '>', ':', '|', ',', ';', '\t', ' ', };
            headerSplitChars = headerSplitChars.Where(a => !chain_ids_split.Contains(""+a)).ToArray();

            var chainIds = fastaInput.Select(a => a.Item1.ToUpperInvariant().Split(headerSplitChars).FirstOrDefault(b=> chain_ids_split.Contains(b)) ?? a.Item1).ToList();

            foreach (var chainId in chain_ids_split)
            {
                if (!chainIds.Contains(chainId))
                {
                    Console.WriteLine("Warning: Chain '" + chainId + "' not found in sequence file.");
                }
            }

            var parameterIndex = -1;

            for (int fastaIndex = 0; fastaIndex < fastaInput.Count; fastaIndex++)
            {
                var chainId = chainIds[fastaIndex];

                if (!chain_ids_split.Contains(chainId))
                {
                    fastaOutput.Add(new Tuple<string, string>(fastaInput[fastaIndex].Item1, fastaInput[fastaIndex].Item2));
                    continue;
                }

                parameterIndex++;
                //var fastaHeader = fastaInput[parameterIndex].Item1;
                var fastaSeq = fastaInput[parameterIndex].Item2.ToCharArray();

                
                var offset = offset_position_split[parameterIndex];
                var start = (start_position_split[parameterIndex] - 1) + offset; // one based to zero based
                var end = (end_position_split[parameterIndex] - 1) + offset;
                var len = (end - start) + 1;

                if (start < 0)
                {
                    end = end + Math.Abs(start);
                    start = 0;
                }

                var aa = mutation_sequence_split[parameterIndex].ToCharArray();

                var pos = 0;
                for (var i = start; i <= end && i < fastaSeq.Length; i++)
                {
                    fastaSeq[i] = aa[pos];
                    pos++;
                    if (pos > aa.Length - 1) pos = 0;
                }
                fastaOutput.Add(new Tuple<string, string>(fastaInput[fastaIndex].Item1, new string(fastaSeq)));
            }

            return fastaOutput;
        }
    }
}
