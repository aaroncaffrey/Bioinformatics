using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;

namespace ComplexContacts
{
    class ComplexContacts
    {
        static void Main(string[] args)
        {
            var parameters = new string[,]
            {
                { "[pdb_or_atoms_file]", "output from the ComplexAtoms program"},
                { "[max_distance]", "maximum allowed contact distance in angstroms [i.e. 5.0 or 8.0]"},
                { "[[output_file]]", "optional output file.  when ommitted, output to console"},
                { "[[overwrite]]", "overwrite if output file exists"}
            };

            var maxParamLength = parameters.Cast<string>().Where((a, i) => i % 2 == 0).Max(a => a.Length);
            var exeFilename = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            if (args.Length == 0)
            {
                Console.WriteLine(exeFilename + @" is a program to list atomic contacts for a PDB file ATOM records.");
                Console.WriteLine();
                Console.WriteLine(@"Usage:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" " + String.Join(" ", parameters.Cast<string>().Where((a, i) => i % 2 == 0)), maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Example:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" ""c:\pdb_db\pdb1a12.pdb"" 8.0 ""c:\pdb_atoms\atoms1a12.pdb""", maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Arguments:");
                for (var i = 0; i < parameters.GetLength(0); i++) Console.WriteLine(@" " + parameters[i, 0].PadLeft(maxParamLength, ' ') + " " + ProteinBioClass.WrapConsoleText(parameters[i, 1], maxParamLength + 2, 1, false));
                Console.WriteLine();
                //return;
            }

            // load arguments
            var p = 0;
            var atomsFilename = args.Length >p && args[p].Length > 0 ? args[p] : "";
            atomsFilename = atomsFilename.Replace("\"", ""); 
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + atomsFilename);

            p++;
            var maxDistance = args.Length >p && args[p].Length > 0 ? Decimal.Parse(args[p]) : 0.0m;
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + maxDistance);

            p++;
            var outputFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            outputFilename = outputFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + outputFilename);


            p++;
            var overwrite = args.Length > p && args[p].Length > 0 ? args[p] : "";
            overwrite = overwrite.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + overwrite);

            if (!string.IsNullOrWhiteSpace(overwrite) && overwrite.ToUpperInvariant() != "Y" && File.Exists(outputFilename))
            {
                Console.Write("; File exists, skipping.");
                return;
            }

            Console.WriteLine();

            if (!File.Exists(atomsFilename)) { return; }


            var interactions = ProteinBioClass.FindInteractions(CancellationToken.None, maxDistance, atomsFilename, new Dictionary<string, List<char>>());

            if (!string.IsNullOrWhiteSpace(outputFilename))
            {
                ProteinBioClass.AtomPair.SaveAtomPairList(outputFilename, interactions);
            }
            else
            {
                //Console.WriteLine("; Atom pairs with contacts: " + interactions.Count);
                foreach (var a in interactions.Select(a => a.ToString()).ToList())
                {
                    Console.WriteLine(a);
                }
            }

        }
    }
}
