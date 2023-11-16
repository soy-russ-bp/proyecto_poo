using System.Threading;

namespace ARJE.Utils.Video
{
    public record AsyncGrabConfig(
        int? MaxFps = null,
        SynchronizationContext? SynchronizationContext = null)
    {
        public static AsyncGrabConfig Default { get; } = new();
    }
}
