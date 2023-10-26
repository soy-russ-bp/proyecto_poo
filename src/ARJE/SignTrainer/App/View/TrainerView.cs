using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using Emgu.CV;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer.App.View
{
    public sealed class TrainerView
    {
        public void DisplayDetections(HandDetectionCollection detections, ref Matrix frame)
        {
            foreach (HandDetection detection in detections)
            {
                DetectionDrawer.Draw(frame, detection);
            }

            CvInvoke.Imshow("Video test", frame);
            CvInvoke.WaitKey(1);
        }
    }
}
