using SConsole = System.Console;

namespace ARJE.Utils.Console
{
    public static class ConsoleUtils
    {
        public static void FlushInput()
        {
            while (SConsole.KeyAvailable)
            {
                SConsole.ReadKey(true);
            }
        }
    }
}
