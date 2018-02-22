using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.AminoAcids;

namespace StructureToSequence
{
    class StructureToSequence
    {


        public static void Main(string[] args)
        {


            var parameters = new string[,]
            {
                {"[pdb_or_atoms_file]", "standard crystal pdb file or output from the ComplexAtoms program"},
                {"[[pad_missing]]", "Y or N (default: Y)"},
                {"[[output_fasta_file]]", "optional output fasta file.  when ommitted, output to console"},
                {"[[append_or_overwrite]]", "optional (A) append or (O) overwrite (default: overwrite)"},
            };

            var maxParamLength = parameters.Cast<string>().Where((a, i) => i % 2 == 0).Max(a => a.Length);
            var exeFilename = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            if (args.Length < 1)
            {
                Console.WriteLine(exeFilename + @" is a program to extract the protein amino acid fasta sequence from protein structure pdb file.");
                Console.WriteLine();
                Console.WriteLine(@"Usage:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" " + String.Join(" ", parameters.Cast<string>().Where((a, i) => i % 2 == 0)), maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Example:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" ""c:\pdb_db\atoms\atoms1a12.pdb"" ""c:\pdb_db\fasta_from_pdb\atoms1a12.pdb.fasta""", maxParamLength + 2, 1));
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
            var padMissing = args.Length > p && args[p].Length > 0 ? args[p] : "Y";
            padMissing = padMissing.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + padMissing);
            if (padMissing != "Y" && padMissing != "N") padMissing = "Y";
            var padMissingBool = padMissing == "Y";

            p++;
            var outputFastaFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            outputFastaFilename = outputFastaFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + outputFastaFilename);

            p++;
            var appendOrOverwrite = args.Length > p && args[p].Length > 0 ? args[p] : "";
            appendOrOverwrite = appendOrOverwrite.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + appendOrOverwrite);

            if (!(string.IsNullOrWhiteSpace(appendOrOverwrite) || appendOrOverwrite == "O" || appendOrOverwrite == "A"))
            {
                return;
            }

            Console.WriteLine();

            var sequenceList = Sequence.LoadStructureFile(atomsFilename, null, padMissingBool);// ProteinBioClass.StructureFileToAaFastaSequence(atomsFilename, null, padMissingBool);

            var output = Sequence.GetAsFasta(sequenceList);

            if (string.IsNullOrWhiteSpace(outputFastaFilename))
            {
                Console.WriteLine();
                
                Console.WriteLine(output);
                
                Console.WriteLine();
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFastaFilename));
                if (appendOrOverwrite == "A" && File.Exists(outputFastaFilename))
                {
                    var data = File.ReadAllText(outputFastaFilename);
                    if (!data.EndsWith(Environment.NewLine)) data = data + Environment.NewLine;
                    output = data + output;
                }

                File.WriteAllText(outputFastaFilename, output);
            }
        }
    }
}
