using System.Collections.Generic;

namespace ARJE.Utils.AI
{
    public interface IDetectionCollection<TDetection> : IReadOnlyList<TDetection>
        where TDetection : IDetection
    {
    }
}
