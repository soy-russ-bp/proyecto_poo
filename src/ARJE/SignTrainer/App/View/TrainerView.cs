using System;
using ARJE.SignTrainer.App.Model;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using Emgu.CV;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer.App.View
{
    public sealed class TrainerView
    {
        public TrainerView(TrainerModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            this.Model = model;
        }

        private TrainerModel Model { get; }

        public void Run()
        {
            using Matrix videoFrame = new();

            while (this.Model.VideoSource.IsOpen)
            {
                this.Model.VideoSource.Read(videoFrame);

                HandsDetectionResult detections = this.Model.Detector.Process(videoFrame);
                foreach (HandDetection detection in detections)
                {
                    DetectionDrawer.Draw(videoFrame, detection);
                }

                CvInvoke.Imshow("Video test", videoFrame);
                CvInvoke.WaitKey(1);
            }
        }
    }
}
