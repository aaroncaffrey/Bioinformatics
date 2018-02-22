using System;
using System.IO;
using System.Collections.Generic;

namespace BioinformaticsHelperLibrary.Dssp
{
    public static class DsspFormatFile
    {
        private const string SsDataMarker = "  #  RESIDUE AA STRUCTURE BP1 BP2  ACC     N-H-->O    O-->H-N    N-H-->O    O-->H-N    TCO  KAPPA ALPHA  PHI   PSI    X-CA   Y-CA   Z-CA ";
        public static List<SecondaryStructureRecord> LoadDsspFile(string dsspFilename)
        {
            if (!File.Exists(dsspFilename))
            {
                throw new FileNotFoundException("File not found", dsspFilename);    
            }

            var result = new List<SecondaryStructureRecord>();

            //var data = File.ReadAllText(dsspFilename);

            //var lineArray = data.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            var lineArray = File.ReadAllLines(dsspFilename);

            bool ssData = false;

            foreach (var line in lineArray)
            {
                if (line == SsDataMarker)
                {
                    ssData = true;
                    continue;
                }

                if (!ssData)
                {
                    continue;
                }

                var record = new SecondaryStructureRecord(line);

                result.Add(record);

            }

            return result;
        }

        
    }
}