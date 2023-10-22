using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using ARJE.Utils.Collections;

namespace ARJE.Utils.Diagnostics.CommandLine
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public static class CLI
    {
        public static string PlatformExecutablePath
        {
            get
            {
                if (OperatingSystem.IsWindows())
                {
                    return "cmd.exe";
                }
                else if (OperatingSystem.IsMacOS())
                {
                    return "/bin/bash";
                }

                throw new PlatformNotSupportedException();
            }
        }

        public static string GetTitleCommand(string title)
        {
            if (OperatingSystem.IsWindows())
            {
                return $"title {title}";
            }
            else if (OperatingSystem.IsMacOS())
            {
                return $"echo -n -e \"\\033]0;{title}\\007\"\r\n";
            }

            throw new PlatformNotSupportedException();
        }

        public static void Execute(DirectoryInfo workingDirectory, params string?[]? commands)
        {
            Execute(workingDirectory.FullName, commands);
        }

        public static void Execute(string? workingDirectory, params string?[]? commands)
        {
            commands = ArrayUtils.FilterNull(commands);
            if (commands.Length == 0)
            {
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = PlatformExecutablePath,
                UseShellExecute = true,
                WorkingDirectory = workingDirectory,
                Arguments = "/K " + string.Join(" && ", commands),
            };
            Process.Start(startInfo)!.Dispose();
        }
    }
}
