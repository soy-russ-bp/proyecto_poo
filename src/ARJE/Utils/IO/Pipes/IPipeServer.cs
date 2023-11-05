using System;
using System.IO;

namespace ARJE.Utils.IO.Pipes
{
    public interface IPipeServer : IDisposable
    {
        public string Identifier { get; }

        public bool IsConnected { get; }

        public Stream WaitForConnection();
    }
}
