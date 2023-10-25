using System;
using System.Drawing;
using System.Numerics;
using Emgu.CV;
using Emgu.CV.Structure;
using Matrix = Emgu.CV.Mat;

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
                Point pos = NormalizedToPixelCoordinates(new Vector2(landmark.X, landmark.Y), image.Size);
                CvInvoke.Circle(image, pos, 2, new MCvScalar(0, 165, 255), 5);
            }
        }
    }
}
