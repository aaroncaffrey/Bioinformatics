using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;

namespace Ds96Ub
{
    class Program
    {
        static void Main(string[] args)
        {
            var pdbFolder = @"C:\ds96ub_homologs\";

            var pdbFiles = Directory.GetFiles(pdbFolder, "*.pdb",SearchOption.TopDirectoryOnly);

            var pdbIdList = pdbFiles.Select(FindAtomicContacts.PdbIdFromPdbFilename).ToList();

            // only ca-atoms, ters and endmdls
            var pdbAtoms =
                pdbFiles.Select(
                    a =>
                        File.ReadAllLines(a)
                            .Where(b => (b.StartsWith("ATOM ") && b[13] == 'C' && b[14] == 'A') || b.StartsWith("TER ") || b.StartsWith("ENDMDL "))
                            .ToList()).ToList();

            // only first nmr model
            pdbAtoms = pdbAtoms.Select(a =>
            {
                var x = a.FindIndex(b => b.StartsWith("ENDMDL "));
                return x == -1 ? a : a.GetRange(0, x - 1);
            }).ToList();

            // get list of unique chain ids 
            var pdbChainIds = pdbAtoms.Select((a, i) => a.Select(b => char.ToUpperInvariant(b[21]))).Distinct().ToList();

            var pdbIdChainIdList = new List<Tuple<string, char>>();
            for (var i = 0; i < pdbIdList.Count; i++)
            {
                pdbIdChainIdList.AddRange(pdbChainIds[i].Select(chainId => new Tuple<string, char>(pdbIdList[i], chainId)));
            }


            var pdbContacts =
                pdbIdChainIdList.Select(a => 
                    {
                        var x =
                            FindAtomicContacts.AtomPair.LoadAtomPairList(@"C:\pdb\new_data_set\contacts\contacts_" + a.Item1.ToLowerInvariant() + ".pdb")
                            .Where(b => char.ToUpperInvariant(b.Atom1.chainID.FieldValue[0]) == a.Item2 || char.ToUpperInvariant(b.Atom2.chainID.FieldValue[0]) == a.Item2)
                            .Select(c =>
                            {
                                if (char.ToUpperInvariant(c.Atom1.chainID.FieldValue[0]) != a.Item2)
                                {
                                    c.SwapAtoms();
                                }

                                return c;
                        }).ToList();

                        return x;
                    }).ToList();
            return;


        }
    }
}
