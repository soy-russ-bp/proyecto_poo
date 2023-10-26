using System;
using System.Threading;
using ARJE.SignTrainer.App.Model;
using ARJE.SignTrainer.App.View;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer.App.Controller
{
    public sealed class TrainerController
    {
        public TrainerController(TrainerModel model, TrainerView view)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(view);

            this.Model = model;
            this.View = view;
        }

        private TrainerModel Model { get; }

        private TrainerView View { get; }

        public void Run()
        {
            this.Model.VideoSource.Start();
            this.Model.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            Thread.Sleep(Timeout.Infinite);
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            var detections = this.Model.Detector.Process(frame);
            this.View.DisplayDetections(detections, ref frame);
        }
    }
}
