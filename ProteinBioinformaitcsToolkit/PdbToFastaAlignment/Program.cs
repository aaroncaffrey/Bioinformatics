using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace PdbToFastaAlignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var ids = @"3JBIV
1T44G
1RGIG
1H1VG
5AFUb
1KXPD
4EAHA
4PKHB

4GI3C
1SBNI
1V5IB
4LVNP
2SICI
3BX1C
1R0RI
1OYVI
";

            var ids2 = ids.Split(new char[] {'\r','\n'},StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToList();
            ids2 = ids2.Where(a=>!string.IsNullOrWhiteSpace(a)).ToList();

            var seqList = Sequence.LoadSequenceFile(@"c:\pdbe\pdb_seqres.fasta");

            Debug.WriteLine("");
            foreach (var id in ids2)
            {
                var seq = seqList.First(a => a.IdSplit.PdbId == id.Substring(0, 4) && a.IdSplit.ChainId == id[4]);
                var pdb = Sequence.LoadStructureFile(@"c:\pdbe\" + id.Substring(0, 4) + ".pdb", new char[] {id[4]},false);
                var al = new NeedlemanWunsch(seq.FullSequence, pdb.First().FullSequence);

                var al2 = al.getAlignment();

                var seqA = al2[0];
                var pdbA = al2[1];

                var seqB = string.Join("", seqA.Where((a, i) => !((a == '-' || a == 'X') && (pdbA[i] == '-' || pdbA[i] == 'X'))).ToList());
                var pdbB = string.Join("", pdbA.Where((a, i) => !((a == '-' || a == 'X') && (seqA[i] == '-' || seqA[i] == 'X'))).ToList());

                seqB = seqB.Replace('-', 'X');
                pdbB = pdbB.Replace('-', 'X');

                var match = seqB.Where((a, i) => a == pdbB[i]).Count();


                var len = seqB.Length > pdbB.Length ? seqB.Length : pdbB.Length;


                var score = (match <= 0 || len <= 0) ? 0 : Math.Round(((decimal)match/(decimal)len),2);

                Debug.WriteLine(id + "\t" + string.Format("{0:0.00}", score));
                Debug.WriteLine(seqB);
                Debug.WriteLine(pdbB);
                Debug.WriteLine("");

            }

        }
    }
}
