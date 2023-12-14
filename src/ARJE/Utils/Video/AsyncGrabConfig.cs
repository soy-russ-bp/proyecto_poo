using System.Threading;

namespace ARJE.Utils.Video
{
    public record AsyncGrabConfig(
        FpsCap FpsCap = default,
        SynchronizationContext? SynchronizationContext = null)
    {
        public static AsyncGrabConfig Default { get; } = new();
    }
}
