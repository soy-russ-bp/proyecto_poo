﻿using System;

namespace ARJE.Utils.Video
{
    public interface IVideoSource<TMatrix> : IDisposable
    {
        public bool IsOpen { get; }

        public void Read(TMatrix buffer);
    }
}