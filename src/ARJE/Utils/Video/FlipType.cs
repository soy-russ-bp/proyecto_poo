using System;

namespace ARJE.Utils.Video
{
    [Flags]
    public enum FlipType
    {
        None = 0,
        Vertical = 1 << 0,
        Horizontal = 1 << 1,
        Both = Vertical | Horizontal,
    }
}
