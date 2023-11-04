using System;

namespace ARJE.Utils.IO.Pipes
{
    public static class PipeUtils
    {
        public static void AssertNotConnectingOrConnected<T>(T pipe)
            where T : IPipeServer
        {
            if (pipe.State is PipeState.Connecting or PipeState.Connected)
            {
                throw new InvalidOperationException("Pipe already connected.");
            }
        }
    }
}
