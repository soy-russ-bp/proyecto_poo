using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ARJE.Utils.AI.Solutions.Hands
{
    public class HandsDetectionResult : IDetectionCollection<HandDetection>
    {
        public HandsDetectionResult(IList<HandDetection> detections)
        {
            ArgumentNullException.ThrowIfNull(detections);

            this.Detections = detections.AsReadOnly();
        }

        public ReadOnlyCollection<HandDetection> Detections { get; }

        public IEnumerator<HandDetection> GetEnumerator()
        {
            return this.Detections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Detections.GetEnumerator();
        }
    }
}
