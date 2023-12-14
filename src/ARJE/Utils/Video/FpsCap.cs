using System;

namespace ARJE.Utils.Video
{
    public readonly record struct FpsCap(int? MaxFps)
    {
        public double FpsDelay { get; } = GetFpsDelay(MaxFps);

        public static FpsCap None { get; } = new FpsCap(null);

        private static double GetFpsDelay(int? maxFps)
        {
            if (!maxFps.HasValue)
            {
                return 0;
            }

            int maxFpsValue = maxFps.Value;
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxFpsValue, 0);
            return TimeSpan.FromSeconds(1.0f / maxFpsValue).TotalMilliseconds;
        }
    }
}
