using System.IO;
using System.Threading.Tasks;

namespace ARJE.Utils.IO.Pipes
{
    public static class PipeExt
    {
        public static Task<Stream> WaitForConnectionAsync(this IPipeServer pipe)
        {
            return Task.Run(pipe.WaitForConnection);
        }
    }
}
