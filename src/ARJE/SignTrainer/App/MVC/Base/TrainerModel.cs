﻿using System;
using ARJE.Shared.Models;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Threading;
using ARJE.Utils.Video;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App.MVC.Base
{
    public sealed record TrainerModel(
            IAsyncVideoSource<Matrix> VideoSource,
            HandsModel Detector,
            OnDiskModelTrainingConfigCollection ModelTrainingConfigCollection)
    {
        public SingleThreadSynchronizationContext SyncCtx { get; } = new();

        public TrainerModel Validate()
        {
            ArgumentNullException.ThrowIfNull(this.VideoSource);
            ArgumentNullException.ThrowIfNull(this.Detector);
            ArgumentNullException.ThrowIfNull(this.ModelTrainingConfigCollection);

            return this;
        }
    }
}
