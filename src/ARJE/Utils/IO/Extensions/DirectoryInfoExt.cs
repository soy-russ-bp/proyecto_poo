using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ARJE.Utils.IO.Extensions
{
    public static class DirectoryInfoExt
    {
        public static IEnumerable<FileInfo> EnumerateFilesWithExtension(this DirectoryInfo directory, string extension)
        {
            extension = FormatExtension(extension);
            return directory.EnumerateFiles().Where(file => file.Name.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
        }

        private static string FormatExtension(string extension)
        {
            extension = extension.Trim();
            if (!extension.StartsWith('.'))
            {
                extension = "." + extension;
            }

            return extension;
        }
    }
}
