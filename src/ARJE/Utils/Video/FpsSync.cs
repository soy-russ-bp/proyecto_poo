using System.Diagnostics;

namespace ARJE.Utils.Video
{
    public sealed record FpsSync(FpsCap FpsCap)
    {
        private Stopwatch? Stopwatch { get; } = FpsCap.MaxFps.HasValue ? Stopwatch.StartNew() : null;

        public bool ShouldGrab()
        {
            if (this.Stopwatch == null)
            {
                return true;
            }

            double elapsed = this.Stopwatch.Elapsed.TotalMilliseconds;
            return elapsed > this.FpsCap.FpsDelay;
        }

        public void NotifyGrabbed()
        {
            this.Stopwatch?.Restart();
        }
    }
}
