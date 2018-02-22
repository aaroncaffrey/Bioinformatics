using System;
using System.IO;

namespace BioinformaticsHelperLibrary.Misc
{
    public static class FileExistsHandler
    {
        public enum FileExistsOptions
        {
            OverwriteFile,
            AppendNumberToFilename,
            SkipFile
        }

        public static FileExistsOptions SelectFileExistsOption(bool overwriteFile, bool appendNumberToFilename, bool skipFile)
        {
            const FileExistsOptions defaultReturnValue = FileExistsOptions.SkipFile;

            int trueCount = (overwriteFile ? 1 : 0) + (appendNumberToFilename ? 1 : 0) + (skipFile ? 1 : 0);

            if (trueCount != 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (overwriteFile)
            {
                return FileExistsOptions.OverwriteFile;
            }

            if (appendNumberToFilename)
            {
                return FileExistsOptions.AppendNumberToFilename;
            }

            if (skipFile)
            {
                return FileExistsOptions.SkipFile;
            }

            return defaultReturnValue;
        }


        /// <summary>
        ///     This method checks if a file exists, and if it does, it appends an unused number to the end of the filename,
        ///     replacing the number if there is already a number present.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string FindNextFreeOutputFilename(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentOutOfRangeException(nameof(filename));
            }

            if (!File.Exists(filename))
            {
                return filename;
            }

            int indexOfLastDot = filename.LastIndexOf('.');
            int indexOfForwardSlash = filename.LastIndexOf('/');
            int indexOfBackSlash = filename.LastIndexOf('\\');
            int indexOfLastSlash = indexOfForwardSlash > indexOfBackSlash ? indexOfForwardSlash : indexOfBackSlash;

            string foldername = filename.Substring(0, indexOfLastSlash + 1);
            string filenameWithoutFiletype = string.Empty;
            string filetypeSuffix = string.Empty;
            string filenameWithoutBrackets = string.Empty;
            long bracketedNumber = 0;

            // Filename might not have a dot.
            filenameWithoutFiletype = filename.Substring(foldername.Length);
            if (indexOfLastSlash < indexOfLastDot)
            {
                // Dot in filename, extract suffix.
                filetypeSuffix = filename.Substring(indexOfLastDot + 1);
                filenameWithoutFiletype = filenameWithoutFiletype.Substring(0, (filenameWithoutFiletype.Length - filetypeSuffix.Length) - 1);
            }

            int indexOfLastOpeningBracket = filenameWithoutFiletype.LastIndexOf("(");
            int indexOfLastClosingBracket = filenameWithoutFiletype.LastIndexOf(")");

            filenameWithoutBrackets = filenameWithoutFiletype;

            if ((indexOfLastOpeningBracket > -1) && (indexOfLastClosingBracket == filenameWithoutFiletype.Length - 1))
            {
                // Bracketed text appears to be last part of filename - check if it is a number.
                string bracketedText = string.Empty;
                bracketedText = filenameWithoutFiletype.Substring(indexOfLastOpeningBracket + 1);
                bracketedText = bracketedText.Substring(0, bracketedText.Length - 1);

                bool isNumberInBrackets = long.TryParse(bracketedText, out bracketedNumber);

                if ((isNumberInBrackets) && (bracketedNumber >= 0))
                {
                    filenameWithoutBrackets = filenameWithoutFiletype.Substring(0, indexOfLastOpeningBracket);
                }
            }

            string nextFilename = filename;

            while (File.Exists(nextFilename))
            {
                bracketedNumber++;
                nextFilename = foldername + filenameWithoutBrackets + "(" + bracketedNumber + ")" + (filetypeSuffix.Length > 0 ? "." : string.Empty) + filetypeSuffix;
            }

            return nextFilename;
        }
    }
}