namespace ARJE.Utils.AI
{
    public interface IDetectionModel<TDetectionCollection, TDetection, TMatrix>
        where TDetectionCollection : IDetectionCollection<TDetection>
        where TDetection : IDetection
    {
        public TDetectionCollection Process(TMatrix image);
    }
}
