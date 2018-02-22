using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;

namespace SequenceStructureOffset
{
    class SequenceStructureOffset
    {


        static void Main(string[] args)
        {
            var parameters = new string[,]
            {
                {"[pdb_or_atoms_file]", "input structure for sequence"},
                {"[fasta_file]", "input sequence for structure"},
                {"[[output_file]]", "optional output file"},
            };

            var maxParamLength = parameters.Cast<string>().Where((a, i) => i % 2 == 0).Max(a => a.Length);
            var exeFilename = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            if (args.Length < 1)
            {
                Console.WriteLine(exeFilename + @" is a program to calculate offset between the sequence and structure.");
                Console.WriteLine();
                Console.WriteLine(@"Usage:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" " + String.Join(" ", parameters.Cast<string>().Where((a, i) => i % 2 == 0)), maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Example:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" ""c:\pdb_db\atoms\atoms1a12.pdb"" ""c:\pdb_db\fasta\atoms1a12.fasta""", maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Arguments:");
                for (var i = 0; i < parameters.GetLength(0); i++) Console.WriteLine(@" " + parameters[i, 0].PadLeft(maxParamLength, ' ') + " " + ProteinBioClass.WrapConsoleText(parameters[i, 1], maxParamLength + 2, 1, false));
                Console.WriteLine();

                return;
            }

            // load arguments
            var p = 0;
            var atomsFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            atomsFilename = atomsFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + atomsFilename);

            p++;
            var inputFastaFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            inputFastaFilename = inputFastaFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + inputFastaFilename);

            p++;
            var outputDataFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            outputDataFilename = outputDataFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + outputDataFilename);

            Console.WriteLine();

            var struct_seq = ProteinBioinformaticsSharedLibrary.ProteinBioClass.StructureFileToAaSequence(atomsFilename, null,
                false);

            
            //foreach (var s in struct_seq)
            //Console.WriteLine(s);

            //var fasta = File.ReadAllLines(inputFastaFilename);
            //foreach (var line in fasta)
            //{
            //    if (string.IsNullOrWhiteSpace(line))continue;
            //    if (line[0] == '>')
            //    {
            //        if (line.Contains())
            //    }

            //}

            /// not finished!
        }
    }
}
