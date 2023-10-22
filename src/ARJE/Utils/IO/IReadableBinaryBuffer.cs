using System.IO;

namespace ARJE.Utils.IO
{
    public interface IReadableBinaryBuffer
    {
        public BinaryReader Reader { get; }

        public void Fill(BinaryReader reader, int length);
    }
}
