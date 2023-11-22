using ARJE.Utils.Video;
using OpenCvSharp;

namespace ARJE.Utils.OpenCvSharp
{
    public static class FlipConverter
    {
        public static bool TryConvertToFlipMode(FlipType flipType, out FlipMode flipMode)
        {
            const int NoValue = int.MinValue;
            flipMode = flipType switch
            {
                FlipType.Vertical => FlipMode.X,
                FlipType.Horizontal => FlipMode.Y,
                FlipType.Both => FlipMode.XY,
                _ => (FlipMode)NoValue,
            };

            return (int)flipMode != NoValue;
        }
    }
}
