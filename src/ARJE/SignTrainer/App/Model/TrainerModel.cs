﻿using System;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Video;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer.App.Model
{
    public sealed class TrainerModel
    {
        public TrainerModel(IAsyncVideoSource<Matrix> videoSource, IDetectionModel<HandDetectionCollection, HandDetection, Matrix> detectionModel)
        {
            ArgumentNullException.ThrowIfNull(videoSource);
            ArgumentNullException.ThrowIfNull(detectionModel);

            this.VideoSource = videoSource;
            this.Detector = detectionModel;
        }

        public IAsyncVideoSource<Matrix> VideoSource { get; }

        public IDetectionModel<HandDetectionCollection, HandDetection, Matrix> Detector { get; }
    }
}
