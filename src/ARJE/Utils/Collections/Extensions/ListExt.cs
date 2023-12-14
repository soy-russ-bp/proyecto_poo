using System.Collections.Generic;
using System.Numerics;

namespace ARJE.Utils.Collections.Extensions
{
    public static class ListExt
    {
        public static void AddXYZ(this IList<float> list, Vector3 vector)
        {
            list.Add(vector.X);
            list.Add(vector.Y);
            list.Add(vector.Z);
        }
    }
}
