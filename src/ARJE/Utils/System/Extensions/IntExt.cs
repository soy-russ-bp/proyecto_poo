namespace ARJE.Utils.System.Extensions
{
    public static class IntExt
    {
        public static bool InRange(this int num, int min, int max)
        {
            return num >= min && num <= max;
        }
    }
}
