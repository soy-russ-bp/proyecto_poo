using System;
using System.IO;
using System.Threading.Tasks;

namespace ARJE.Utils.IO.Pipes
{
    public interface INamedPipe : IDisposable
    {
        public string NameOrPath { get; }

        public bool Connected { get; }

        public static abstract INamedPipe Create(string pipeName);

        public Stream Connect();

        public Task<Stream> ConnectAsync();
    }
}
