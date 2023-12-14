using System;
using System.IO;

namespace ARJE.Utils.IO
{
    public static class PathUtils
    {
        private static char[] DirectorySeparatorChars { get; } = new[] { '\\', '/' };

        public static string GoUpToFolder(string path, string folderName)
        {
            path = path.Trim();
            folderName = folderName.Trim();

            ArgumentException.ThrowIfNullOrEmpty(path);
            ArgumentException.ThrowIfNullOrEmpty(folderName);

            string[] pathParts = path.Split(DirectorySeparatorChars, StringSplitOptions.RemoveEmptyEntries);
            int matchI = Array.LastIndexOf(pathParts, folderName);
            if (matchI == -1)
            {
                throw new InvalidOperationException($"The path \"{path}\" does not contain a folder with the name \"{folderName}\".");
            }

            string newPath = string.Join(Path.DirectorySeparatorChar, pathParts, 0, matchI + 1);
            return newPath;
        }

        public static string TempPathJoin(string path)
        {
            path = path.Trim();
            ArgumentException.ThrowIfNullOrEmpty(path);

            string tempPath = Path.GetTempPath();
            return Path.Join(tempPath, path);
        }
    }
}
