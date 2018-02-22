using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FetchPdbFile
{
    public class FetchPdbFile
    {
        public static void Main(string[] args)
        {
            //var pdbIdList = File.ReadAllLines(@"c:\ds96ub\pdbid_list.txt");

            var x = @"1acb
1avw
1azz
1b3a
1brc
1c9p
1c9t
1cgi
1d6r
1eai
1ezu
1f2s
1fak
1fle
1gl0
1gl1
1k9o
1kig
1ldt
1lw6
1mct
1mcv
1ml0
1ppf
1scj
1sgf
1tab
1tgs
2btc
5sic
";
            var pdbIdList = x.Split(new char[] {'\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(a => a.Trim()).ToList();

            foreach (var pdbId in pdbIdList)
            {
                //var file = "pdb" + pdbId.ToUpperInvariant() + ".ent.gz";
                //var link = "ftp://ftp.wwpdb.org/pub/pdb/data/structures/all/pdb/" + file;
                var link = @"http://webclu.bio.wzw.tum.de/cgi-bin/stride/stridecgi.py?pdbid=" + pdbId + "&action=compute";

                var save = @"c:\CategoryM\stride\" + pdbId + ".stride";

                WebClient webClient = new WebClient();
                webClient.DownloadFile(link, save);

                Console.WriteLine("Saved: " + save);
            }

            Console.ReadLine();
        }
    }
}
