using System;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Video;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer.App.Model
{
    public sealed class TrainerModel
    {
        public TrainerModel(IVideoSource<Matrix> videoSource, IDetectionModel<HandsDetectionResult, HandDetection, Matrix> detectionModel)
        {
            ArgumentNullException.ThrowIfNull(videoSource);
            ArgumentNullException.ThrowIfNull(detectionModel);

            this.VideoSource = videoSource;
            this.Detector = detectionModel;
        }

        public IVideoSource<Matrix> VideoSource { get; }

        public IDetectionModel<HandsDetectionResult, HandDetection, Matrix> Detector { get; }
    }
}
