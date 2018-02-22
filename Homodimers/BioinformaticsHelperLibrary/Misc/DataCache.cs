using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class DataCache
    {
        public const string CacheLocation = @"c:\d\cache\";

        public static T LoadCache<T>(string cacheFilename) where T : class
        {
            if (cacheFilename == null) throw new ArgumentNullException(nameof(cacheFilename));

            var fileInfo = new FileInfo(cacheFilename);

            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                return null;
            }

            T cache = null;

            var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                var formatter = new BinaryFormatter();

                cache = (T)formatter.Deserialize(fs);
            }
            catch (SerializationException)
            {
                throw;
            }
            finally
            {
                fs.Close();
            }

            return cache;
        }

        public static void SaveCache<T>(string cacheFilename, T cache) where T : class
        {
            if (cacheFilename == null) throw new ArgumentNullException(nameof(cacheFilename));
            var fileInfo = new FileInfo(cacheFilename);
            fileInfo.Directory?.Create();

            var fs = new FileStream(cacheFilename, FileMode.Create);

            var formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, cache);
            }
            catch (SerializationException)
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
