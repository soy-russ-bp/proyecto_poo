using System;
using System.IO;
using System.Threading.Tasks;

namespace ARJE.Utils.IO.Pipes
{
    public interface INamedPipe : IDisposable
    {
        public string NameOrPath { get; }

        public Stream Stream { get; }

        public static abstract INamedPipe Create(string pipeName);

        public void Connect();

        public Task ConnectAsync();
    }
}
