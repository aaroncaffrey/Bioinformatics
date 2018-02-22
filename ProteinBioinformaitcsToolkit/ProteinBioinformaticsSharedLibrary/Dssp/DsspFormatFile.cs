using System.Collections.Generic;
using System.IO;

namespace ProteinBioinformaticsSharedLibrary.Dssp
{
    public static class DsspFormatFile
    {
        private const string SsDataMarker = "  #  RESIDUE AA STRUCTURE BP1 BP2  ACC     N-H-->O    O-->H-N    N-H-->O    O-->H-N    TCO  KAPPA ALPHA  PHI   PSI    X-CA   Y-CA   Z-CA ";
                                            //"  #  RESIDUE AA STRUCTURE BP1 BP2  ACC     N-H-->O    O-->H-N    N-H-->O    O-->H-N    TCO  KAPPA ALPHA  PHI   PSI    X-CA   Y-CA   Z-CA            CHAIN"

        public static List<DsspRecord> LoadDsspFile(string dsspFilename)
        {
            if (!File.Exists(dsspFilename))
            {
                throw new FileNotFoundException("File not found", dsspFilename);    
            }

            var result = new List<DsspRecord>();

            var lineArray = File.ReadAllLines(dsspFilename);

            var ssData = false;

            foreach (var line in lineArray)
            {
                if (line.StartsWith(SsDataMarker))
                {
                    ssData = true;
                    continue;
                }

                if (!ssData)
                {
                    continue;
                }

                var record = new DsspRecord(line);

                result.Add(record);

            }

            return result;
        }

        
    }
}