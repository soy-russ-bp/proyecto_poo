using System;

namespace ARJE.Utils.AI.Solutions.Hands
{
    public class HandDetection : IDetection
    {
        public HandDetection(LandmarkCollection landmarks)
        {
            ArgumentNullException.ThrowIfNull(landmarks);

            this.Landmarks = landmarks;
        }

        public LandmarkCollection Landmarks { get; }
    }
}
