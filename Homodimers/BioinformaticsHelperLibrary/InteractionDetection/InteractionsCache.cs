using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.InteractionDetection
{
    public static class InteractionsCache
    {
        public const string InteractionsCacheLocation = @"c:\d\cache\interactions\";

        public static string CachedPdbInteractionsFilename(string pdbId, int requiredChains)
        {
            if (string.IsNullOrWhiteSpace(pdbId))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbId));
            }

            string cachedPdbInteractionsFilename = $"{InteractionsCacheLocation}pdb{pdbId}-{requiredChains}chains.dat";
            return cachedPdbInteractionsFilename;
        }

        public static List<AtomPair> LoadPdbInteractionCache(string pdbId, int requiredChains)
        {
            string cachedPdbInteractionsFilename = CachedPdbInteractionsFilename(pdbId, requiredChains);
            var fileInfo = new FileInfo(cachedPdbInteractionsFilename);



            if (!File.Exists(cachedPdbInteractionsFilename) || fileInfo.Length == 0)
            {
                return null;
            }

            List<AtomPair> cachedAtomPairList = null;

            var fs = new FileStream(cachedPdbInteractionsFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                var formatter = new BinaryFormatter();

                cachedAtomPairList = (List<AtomPair>)formatter.Deserialize(fs);
            }
            catch (SerializationException)
            {
                //throw;
                cachedAtomPairList = null;
            }
            finally
            {
                fs.Close();
            }

            return cachedAtomPairList;
        }

        public static void SavePdbInteractionCache(string pdbId, List<AtomPair> pdbInteractionsList, int requiredChains)
        {
            string cachedPdbInteractionsFilename = CachedPdbInteractionsFilename(pdbId, requiredChains);

            var fileInfo = new FileInfo(cachedPdbInteractionsFilename);
            if (fileInfo.Directory != null) fileInfo.Directory.Create();

            var fs = new FileStream(cachedPdbInteractionsFilename, FileMode.Create);

            var formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, pdbInteractionsList);
            }
            catch (SerializationException)
            {
                fs.Close();
                fileInfo.Delete();
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
