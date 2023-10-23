using System;
using System.IO;

namespace ARJE.Utils.IO
{
    public sealed class ReusableBinaryBuffer : IReadableBinaryBuffer, IFillableByReader, IDisposable
    {
        public ReusableBinaryBuffer(int capacity = 0)
        {
            this.Stream = new MemoryStream(capacity);
            this.Reader = new BinaryReader(this.Stream);
        }

        public BinaryReader Reader { get; }

        private MemoryStream Stream { get; }

        public void Fill(BinaryReader reader, int length)
        {
            this.Reset(length);
            Span<byte> buffer = this.Stream.GetBuffer().AsSpan()[..length];
            reader.BaseStream.ReadExactly(buffer);
        }

        public void Dispose()
        {
            this.Reader.Close();
        }

        private void Reset(int length)
        {
            this.Stream.Position = 0;
            this.Stream.SetLength(length);
        }
    }
}
