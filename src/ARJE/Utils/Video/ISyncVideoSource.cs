namespace ARJE.Utils.Video
{
    public interface ISyncVideoSource<TMatrix> : IVideoSource<TMatrix>
    {
        public TMatrix Read();
    }
}
