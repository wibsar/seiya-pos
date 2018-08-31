using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Seiya
{
    public class FileIO
    {
        /// <summary>
        /// Remove commas from string input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            //TODO: What happens if there is no ,
            return input.Replace(",", "");
        }

        /// <summary>
        /// Get the file name with no extension
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileNameFromPathWithoutExtension(string filePath)
        {
            //If there is no \ in the file path just return the original string
            if (!filePath.Contains(@"\"))
                return filePath;

            //Grab the part of the string after the last \
            var fileNameWithExtension = filePath.Split('\\').Last();

            //If there is no period (and thus no file extension) return the string
            if (!fileNameWithExtension.Contains(@"."))
                return fileNameWithExtension;

            //Split the file name and extension on the . character
            var periodSplit = fileNameWithExtension.Split('.');

            //Ideally the file name shouldn't have any periods in it except for the file extension one.
            if (periodSplit.Length == 2)
                return periodSplit.First();

            var fileNameBuilder = new StringBuilder();

            //However, handle stupid file paths here
            for (var i = 0; i < periodSplit.Length - 1; ++i)
            {
                fileNameBuilder.Append(periodSplit[i]);
            }

            return fileNameBuilder.ToString();
        }

        public static string GetFolderFromFilePathString(string filePath)
        {
            try
            {
                return GetFolderFromFilePathString(filePath);
            }
            catch (Exception e)
            {
                //TODO: Add exception
            }

            return string.Empty;
        }

        public static bool MoveFile(string sourceFolderPath, string fileName, string targetFolderPath)
        {
            try
            {
                return MoveFile(sourceFolderPath, fileName, targetFolderPath);
            }
            catch (Exception e)
            {

            }
            return false;
        }
    }
}
