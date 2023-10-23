using System.IO;

namespace ARJE.Utils.IO
{
    public interface IFillableByReader
    {
        public void Fill(BinaryReader reader, int length);
    }
}
