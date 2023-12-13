using System;
using System.Numerics;
using OpenCvSharp;
using Matrix = OpenCvSharp.Mat;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ARJE.Utils.AI
{
    public static class DetectionDrawer
    {
        public static int NormalizedAxisToPixelCoordinates(float axisPos, int axisSize)
        {
            return (int)MathF.Min(MathF.Floor(axisPos * axisSize), axisSize - 1);
        }

        public static Point NormalizedToPixelCoordinates(Vector2 pos, Size size)
        {
            int xPx = NormalizedAxisToPixelCoordinates(pos.X, size.Width);
            int yPx = NormalizedAxisToPixelCoordinates(pos.Y, size.Height);
            return new Point(xPx, yPx);
        }

        public static void Draw(Matrix image, IDetection detection)
        {
            foreach (Vector3 landmark in detection.Landmarks.Positions)
            {
                Point pos = NormalizedToPixelCoordinates(new Vector2(landmark.X, landmark.Y), image.Size());
                Cv2.Circle(image, pos.X, pos.Y, 2, new Scalar(0, 165, 255), 5);
            }
        }
    }
}
