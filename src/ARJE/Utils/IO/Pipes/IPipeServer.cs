using System;
using System.IO;

namespace ARJE.Utils.IO.Pipes
{
    public interface IPipeServer : IDisposable
    {
        public string Identifier { get; }

        public PipeState State { get; }

        public Stream WaitForConnection();
    }
}
