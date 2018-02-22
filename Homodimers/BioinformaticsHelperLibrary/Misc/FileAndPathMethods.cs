using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class FileAndPathMethods
    {
        private const char BackSlash = '\\';
        private const char ForwardSlash = '/';

        public static void CreateDirectory(string fullPath)
        {
            var fileInfo = new FileInfo(fullPath);
            if (fileInfo.Directory != null) fileInfo.Directory.Create();    
        }

        public static char WhichSlashConvention(string path)
        {
            var totalBackSlash = path.Count(c => c == BackSlash);
            var totalForwardSlash = path.Count(c => c == ForwardSlash);

            return totalBackSlash >= totalForwardSlash ? BackSlash : ForwardSlash;
        }

        public static bool IsDirectoryPathSlashTerminated(string path)
        {
            var lastChar = path.Last();

            if (lastChar == ForwardSlash || lastChar == BackSlash)
            {
                return true;
            }

            return false;
        }

        public static string EnforceSlashConvention(string path)
        {
            var convention = WhichSlashConvention(path);
            if (convention == ForwardSlash)
            {
                path = path.Replace(BackSlash, ForwardSlash);
                while (path.Contains("" + ForwardSlash + ForwardSlash)) path = path.Replace("" + ForwardSlash + ForwardSlash, "" + ForwardSlash);
            }
            else
            {
                path = path.Replace(ForwardSlash, BackSlash);
                while (path.Contains("" + BackSlash + BackSlash)) path = path.Replace("" + BackSlash + BackSlash, "" + BackSlash);
            }
            return path;
        }

        public static string SlashTerminate(string path)
        {
            return path + (IsDirectoryPathSlashTerminated(path) ? "" : "" + WhichSlashConvention(path));
        }

        public static string MergePathAndFilename(string path, string subfolder, string file)
        {
            return MergePathAndFilename(SlashTerminate(path) + SlashTerminate(subfolder), file);
        }
        public static string MergePathAndFilename(string path, string file)
        {
            return EnforceSlashConvention((string.IsNullOrWhiteSpace(path) ? "" : path + (IsDirectoryPathSlashTerminated(path) ? "" : "" + WhichSlashConvention(path))) + file);
        }

        /// <summary>
        ///     Returns the filename part of the full path.  Optionally removes the file type extension.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="removeFileTypeExtension"></param>
        /// <returns></returns>
        public static string FullPathToFilename(string fullPath, bool removeFileTypeExtension = true)
        {
            string result = fullPath.Substring(fullPath.LastIndexOfAny(new[] { '\\', '/' }) + 1);

            if (removeFileTypeExtension && result.LastIndexOf('.') > -1)
            {
                result = result.Substring(0, result.LastIndexOf('.'));
            }

            return result;
        }

        public static string RemoveFileExtension(string fullPath)
        {
            int lastIndexOfPeriod = fullPath.LastIndexOf('.');
            int lastIndexOfSlash = fullPath.LastIndexOfAny(new[] { '\\', '/' });

            if (lastIndexOfSlash < lastIndexOfPeriod)
            {
                return fullPath.Substring(0, lastIndexOfPeriod);
            }

            return fullPath;
        }
    }
}
