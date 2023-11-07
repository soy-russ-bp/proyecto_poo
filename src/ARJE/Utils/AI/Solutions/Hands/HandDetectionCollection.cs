using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ARJE.Utils.AI.Solutions.Hands
{
    public class HandDetectionCollection : ReadOnlyCollection<HandDetection>, IDetectionCollection<HandDetection>
    {
        public HandDetectionCollection(IList<HandDetection> detections)
            : base(detections)
        {
        }
    }
}
