using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplacePdbChain
{
    public class ReplacePdbChain
    {
        static void Main(string[] args)
        {

            // modeller does not have the chain id on outputs, and it also only works with a single chain
            // however the xyz coordinates are preserved, so it can be reintegrated with the originating complex
            /*
            var complexFilename = @"complex.pdb";
            var modelFilename = @"model.pdb";
            var chainToReplace = "A";

            var complexLines = File.ReadAllLines(complexFilename);//.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var new_lines = new List<string>();
            foreach (var line in lines)
            {
                var a = line.ToCharArray();
                a[21] = 'A';
                var line2 = new string(a);

                new_lines.Add(line2);
            }

            File.WriteAllLines(@"c:\users\aaron\desktop\atoms with chain.txt", new_lines);
            */
        }

    }
    
}
