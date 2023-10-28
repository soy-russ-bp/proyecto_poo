namespace ARJE.Utils.Video
{
    public interface ISyncVideoSource<TMatrix> : IBaseVideoSource<TMatrix>
    {
        public TMatrix Read();
    }
}
