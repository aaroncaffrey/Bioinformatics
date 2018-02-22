using ProteinBioinformaticsSharedLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexInterfaceExtraction
{
    public class ComplexInterfaceExtraction
    {
        static void Main(string[] args)
        {
            var parameters = new string[,]
                        {
                { "[pdb_file]", "PDB ~v3.3 Protein Data Bank format file [*.pdb, *.ent]"},
                { "[interface-interface_file]", "interface-interface file"},
                { "[[chain_ids]]", "molecule chains to output [* for all]"},
                { "[[output_file]]", "optional output file. use ? for chain id. when ommitted, output to console"},
                        };

            var maxParamLength = parameters.Cast<string>().Where((a, i) => i % 2 == 0).Max(a => a.Length);
            var exeFilename = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            if (args.Length == 0)
            {
                Console.WriteLine(exeFilename + @" is a program to extract ATOM records from a PDB file.");
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
                return;
            }

            // load and echo arguments
            var p = 0;
            var pdbFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            pdbFilename = pdbFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + pdbFilename);

            p++;
            var interfaceInterfaceFile = args.Length > p && args[p].Length > 0 ? args[p].ToUpperInvariant() : "";
            interfaceInterfaceFile = interfaceInterfaceFile.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + interfaceInterfaceFile);

            p++;
            var chainIds = args.Length > p && args[p].Length > 0 ? args[p] : "";
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + chainIds);

            p++;
            var outputFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            outputFilename = outputFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + outputFilename);

            Console.WriteLine();

            if (!File.Exists(pdbFilename))
            {
                Console.WriteLine("; File not found: " + pdbFilename);
                return;
            }

            if (!File.Exists(interfaceInterfaceFile))
            {
                Console.WriteLine("; File not found: " + interfaceInterfaceFile);
                return;
            }

            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                return;
            }

            if (chainIds.Contains('*')) chainIds = null;

            var chainIdWhiteList = !string.IsNullOrEmpty(chainIds) ? chainIds.ToUpperInvariant().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries) : null;

            var interfaceData = ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load(interfaceInterfaceFile);
            
            var terminatedChains = new List<string>();

            var pdbfilenameShort = Path.GetFileNameWithoutExtension(pdbFilename);

            var pdbId = pdbfilenameShort.Substring(pdbfilenameShort.Length - 4).ToUpperInvariant();

            var lines = File.ReadAllLines(pdbFilename);

            var result = new List<Tuple<string, string>>();

            var interfaceDataChains = interfaceData.Select(a => a.ReceptorChainId).Distinct().ToList();

            var interfaceDataStart = interfaceDataChains.Select(a => interfaceData.Where(b => b.ReceptorChainId == a).Min(b => b.ReceptorInterfaceResSeqStart)).ToList();
            var interfaceDataEnd = interfaceDataChains.Select(a => interfaceData.Where(b => b.ReceptorChainId == a).Max(b => b.ReceptorInterfaceResSeqEnd)).ToList();


            foreach (var line in lines)
            {
                if (line.Length < 22) continue;

                if (line.Substring(0, 4).ToUpperInvariant() == "TER ")
                {
                    var chainId = ("" + line[21]).ToUpperInvariant();

                    terminatedChains.Add(chainId);
                }


                if (line.Substring(0, 5).ToUpperInvariant() == "ATOM ")
                {
                    var chainId = ("" + line[21]).ToUpperInvariant();

                    if (terminatedChains.Contains(chainId)) continue;

                    if (chainIdWhiteList != null && chainIdWhiteList.Length > 0 && !chainIdWhiteList.Contains(chainId)) continue;

                    if (!interfaceDataChains.Contains(chainId[0])) continue;

                    //if (caTraceOnlyBool && (line[13] != 'C' || line[14] != 'A')) continue;

                    var interfaceDataChainIndex = interfaceDataChains.IndexOf(chainId[0]);

                    var resSeq = int.Parse(line.Substring(22, 4).Trim());

                    if (resSeq >= interfaceDataStart[interfaceDataChainIndex] && resSeq <= interfaceDataEnd[interfaceDataChainIndex])

                    result.Add(new Tuple<string, string>(chainId, line));
                }
            }


            if (!string.IsNullOrWhiteSpace(outputFilename))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilename.Replace("?", "")));
                if (!outputFilename.Contains("?"))
                {
                    File.WriteAllLines(outputFilename, result.Select(a => a.Item2).ToList());
                }
                else
                {
                    var chains = result.Select(a => a.Item1).Distinct().ToList();
                    foreach (var chain in chains)
                    {
                        var outputFilename2 = outputFilename.Replace("?", "");
                        outputFilename2 = Path.GetDirectoryName(outputFilename2) + @"\" + Path.GetFileNameWithoutExtension(outputFilename2) + chain + Path.GetExtension(outputFilename2);
                        File.WriteAllLines(outputFilename2, result.Where(a => a.Item1 == chain).Select(a => a.Item2).ToList());
                    }
                }
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
    }
}
