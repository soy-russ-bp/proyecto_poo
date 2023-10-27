using System;
using System.Runtime.Versioning;
using ARJE.SignTrainer.App;
using Spectre.Console;

namespace ARJE.SignTrainer
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal class Program
    {
        private static void Main()
        {
            AnsiConsole.WriteLine("- START -");

            try
            {
                TrainerApp.Run();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine(ex.ToString());
#if !DEBUG
                Console.ReadKey();
#endif
                throw;
            }

            AnsiConsole.WriteLine("- END -");
        }
    }
}
