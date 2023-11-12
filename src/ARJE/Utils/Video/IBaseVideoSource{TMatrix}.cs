using System;

namespace ARJE.Utils.Video
{
    public interface IBaseVideoSource<TMatrix> : IDisposable
    {
        public bool IsOpen { get; }

        public FlipType OutputFlipType { get; set; }
    }
}
