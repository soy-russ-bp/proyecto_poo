using System;
using System.Runtime.Versioning;
using ARJE.SignTrainer.App;

namespace ARJE.SignTrainer
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal class Program
    {
        private static void Main()
        {
            try
            {
                TrainerApp.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex);
                Console.ReadKey();
                throw;
            }
        }
    }
}
