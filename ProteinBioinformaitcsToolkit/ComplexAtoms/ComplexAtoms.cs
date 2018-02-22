using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;

namespace ComplexAtoms
{
    class ComplexAtoms
    {
        static void Main(string[] args)
        {

            const int atom_chain = 21;
            const int atom_chain_len = 1;

            const int atom_icode = 26;
            const int atom_icode_len = 1;

            const int atom_type = 14;
            const int atom_type_len = 3;

            const int atom_resseq = 22;
            const int atom_resseq_len = 4;

            var parameters = new string[,]
            {
                { "[pdb_file]", "PDB ~v3.3 Protein Data Bank format file [*.pdb, *.ent]"},
                { "[[subset]]", "-, mc, sc, ca"},
                { "[[chain_ids]]", "molecule chains to output [2 formats: - for all, ABC, or A,1,50,B,2,40,C,5,200]"},
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
            var subset = args.Length > p && args[p].Length > 0 ? args[p].ToUpperInvariant() : "";
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + subset);
            


            p++;
            var chainIds = args.Length > p && args[p].Length > 0 ? args[p] : "";
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + chainIds);

            p++;
            var outputFilename = args.Length > p && args[p].Length > 0 ? args[p] : "";
            outputFilename = outputFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + outputFilename);

            Console.WriteLine();

            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                return;
            }

            if (!File.Exists(pdbFilename)) { return; }

            if (chainIds.Contains('-')) chainIds = null;

            var chainStartEnd = new List<Tuple<char, int, int>>();

            var chainIdsSplit = chainIds?.Split(',').ToList();

            char[] chainIdWhiteList;

            if (chainIdsSplit?.Count > 1)
            {
                if (chainIdsSplit.Count%3 != 0) { return; }
                for (var i = 0; i < chainIdsSplit.Count; i += 3)
                {
                    var id = chainIdsSplit[i + 0][0];

                    var start = chainIdsSplit[i + 1];
                    if (string.IsNullOrWhiteSpace(start)) start = "-1";

                    var end = chainIdsSplit[i + 2];
                    if (string.IsNullOrWhiteSpace(end)) end = "-1";

                    chainStartEnd.Add(new Tuple<char, int, int>(id, int.Parse(start), int.Parse(end)));
                }

                chainIdWhiteList = chainStartEnd.Select(a => a.Item1).Distinct().ToArray();
            }
            else
            {
                chainIdWhiteList = chainIds?.Where(char.IsLetter).Distinct().ToArray();//!string.IsNullOrEmpty(chainIds) ? chainIds.ToUpperInvariant().Split(new char[] { ' ', ',' },StringSplitOptions.RemoveEmptyEntries) : null;    
            }
            

            
            var terminatedChains = new List<char>();

            var pdbfilenameShort = Path.GetFileNameWithoutExtension(pdbFilename);

            var pdbId = pdbfilenameShort.Substring(pdbfilenameShort.Length - 4).ToUpperInvariant();

            var lines = File.ReadAllLines(pdbFilename);

            var result = new List<Tuple<char,string>>();

            string[] ca = new string[] { "CA" };
            string[] bb = new[] { "N", "CA", "C", "O" };

            foreach (var line in lines)
            {
                if (line.Length < 22) continue;

                if (line.Substring(0, 4).ToUpperInvariant() == "TER ")
                {
                    var chainId = line[21];//).ToUpperInvariant();

                    terminatedChains.Add(chainId);
                }


                if (line.Substring(0, 5).ToUpperInvariant() == "ATOM ")
                {
                    var chainId = line[21];//).ToUpperInvariant();

                    if (terminatedChains.Contains(chainId)) continue;

                    if (chainIdWhiteList != null && chainIdWhiteList.Length > 0 && !chainIdWhiteList.Contains(chainId)) continue;
                    
                    //if (subset == "ca" || subset == "sc") && (line[13] != 'C' || line[14] != 'A')) continue;

                    var add = false;

                    var atom_type_s = line.Substring(13, 3).Trim();

                    if (atom_type_s[0] != 'C' && atom_type_s[0] != 'N' && atom_type_s[0] != 'O') continue;

                    // check chainIdsSplit
                    var resId = int.Parse(line.Substring(atom_resseq, atom_resseq_len));

                    var chainStartEndItem = chainStartEnd.FirstOrDefault(a => a.Item1 == chainId);

                    if (chainStartEndItem != null)
                    {
                        if (!((chainStartEndItem.Item2 == -1 || resId >= chainStartEndItem.Item2) && (chainStartEndItem.Item3 == -1 || resId <= chainStartEndItem.Item3)))
                        {
                            continue;
                        }
                    }

                    if (subset == "-") { add = true; }
                    else if (subset == "CA" && ca.Contains(atom_type_s)) { add = true; }
                    else if (subset == "MC" && bb.Contains(atom_type_s)) { add = true; }
                    else if (subset == "SC" && !bb.Contains(atom_type_s)) { add = true; }

                    if (add) { result.Add(new Tuple<char, string>(chainId,line));}
                }
            }

            
            if (!string.IsNullOrWhiteSpace(outputFilename))
            {
                var outputFilename2 = outputFilename.Replace("?", "");
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilename2));
                if (!outputFilename.Contains("?"))
                {
                    File.WriteAllLines(outputFilename, result.Select(a => a.Item2).ToList());
                }
                else if (outputFilename.Contains("??"))
                {
                    var chains = new string(result.Select(a => a.Item1).Where(char.IsLetter).Distinct().OrderBy(a=>a).ToArray());
                    
                    outputFilename2 = Path.GetDirectoryName(outputFilename2) + @"\" + Path.GetFileNameWithoutExtension(outputFilename2) + chains + Path.GetExtension(outputFilename2);

                    File.WriteAllLines(outputFilename2, result.Select(a => a.Item2).ToList());
                }
                else if (outputFilename.Contains("?"))
                {
                    var chains = result.Select(a => a.Item1).Distinct().ToList();

                    foreach (var chain in chains)
                    {
                        outputFilename2 = outputFilename.Replace("?", "");
                        outputFilename2 = Path.GetDirectoryName(outputFilename2) + @"\" + Path.GetFileNameWithoutExtension(outputFilename2) + chain + Path.GetExtension(outputFilename2);
                        File.WriteAllLines(outputFilename2, result.Where(a=>a.Item1==chain).Select(a=>a.Item2).ToList());
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
