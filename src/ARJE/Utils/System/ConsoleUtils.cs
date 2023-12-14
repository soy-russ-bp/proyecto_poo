using System;

namespace ARJE.Utils.System
{
    public static class ConsoleUtils
    {
        public static void FlushInput()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
