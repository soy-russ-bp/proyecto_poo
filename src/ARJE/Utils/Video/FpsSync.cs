using System;
using System.Diagnostics;

namespace ARJE.Utils.Video
{
    public sealed record FpsSync(int? MaxFps)
    {
        private Stopwatch? Stopwatch { get; } = MaxFps.HasValue ? Stopwatch.StartNew() : null;

        private double FpsDelay { get; } = GetFpsDelay(MaxFps);

        public bool ShouldGrab()
        {
            if (this.Stopwatch == null)
            {
                return true;
            }

            double elapsed = this.Stopwatch.Elapsed.TotalMilliseconds;
            return elapsed > this.FpsDelay;
        }

        public void NotifyGrabbed()
        {
            this.Stopwatch?.Restart();
        }

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
