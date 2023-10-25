using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ARJE.Utils.AI
{
    public interface IDetectionCollection<TDetection> : IEnumerable<TDetection>
        where TDetection : IDetection
    {
        ReadOnlyCollection<TDetection> Detections { get; }
    }
}
