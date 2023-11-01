using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
#if OS_WINDOWS
                return "cmd.exe";
#elif OS_MAC
                return "/bin/bash";
#else
#warning PlatformNotSupported: CLI.PlatformExecutablePath
                throw new System.PlatformNotSupportedException();
#endif
            }
        }

        public static string GetTitleCommand(string title)
        {
#if OS_WINDOWS
            return $"title {title}";
#elif OS_MAC
            return $"echo -n -e \"\\033]0;{title}\\007\"\r\n";
#else
#warning PlatformNotSupported: CLI.GetTitleCommand(string)
            throw new System.PlatformNotSupportedException();
#endif
        }

        public static string JoinArgs(params string?[]? args)
        {
            args = ArrayUtils.FilterNull(args);
            if (args.Length == 0)
            {
                return string.Empty;
            }

            IEnumerable<string> wrappedArgs = args.Select(arg => $"\"{arg}\"");
            return string.Join(" ", wrappedArgs);
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
