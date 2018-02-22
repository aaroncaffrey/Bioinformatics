using System;
using System.Collections.Generic;
using System.Linq;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.InteractionDetection
{
    public static class SqrtCache
    {
        private const string DefaultCacheFilename = @"c:\d\cache\SqrtCache.dat";
        public static T LoadSqrtCache<T>(string sqrtCacheFilename = DefaultCacheFilename) where T : class, IDictionary<double, double>, new()
        {
            T sqrtCache = DataCache.LoadCache<T>(sqrtCacheFilename);

            return sqrtCache;
        }

        public static void SaveSqrtCache<T>(T sqrtCache, string sqrtCacheFilename = DefaultCacheFilename) where T : class, IDictionary<double, double>
        {
            DataCache.SaveCache(sqrtCacheFilename, sqrtCache);
        }

        public static T MergeSqrtDictionaries<T>(List<T> sqrtCacheDictionaryList) where T : class, IDictionary<double, double>, new()
        {
            var result = new T();

            foreach (var kvp in sqrtCacheDictionaryList.SelectMany(dict => dict.Where(kvp => !result.ContainsKey(kvp.Key))))
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }

        public static double CachedSqrt(double d, IDictionary<double, double> sqrtCache)
        {
            if (sqrtCache == null)
            {
                throw new ArgumentNullException(nameof(sqrtCache));
            }

            if (sqrtCache.ContainsKey(d))
            {
                return sqrtCache[d];
            }

            double result = Math.Sqrt(d);

            sqrtCache.Add(d, result);

            return result;
        }
    }
}