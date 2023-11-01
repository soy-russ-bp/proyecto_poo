using System;

namespace ARJE.Utils.IO.Pipes
{
    public static class PipeUtils
    {
        public static void AssertNotConnected<T>(T pipe)
            where T : INamedPipe
        {
            if (pipe.Connected)
            {
                throw new InvalidOperationException("Pipe already connected.");
            }
        }
    }
}
