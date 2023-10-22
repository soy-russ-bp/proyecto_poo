using System.Collections.ObjectModel;

namespace ARJE.Utils.AI
{
    public interface IPredictionModel<TMatrix>
    {
        public ReadOnlyCollection<Detection> Process(TMatrix image);
    }
}
