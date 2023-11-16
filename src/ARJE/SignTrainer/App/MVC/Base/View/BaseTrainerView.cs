﻿using ARJE.Utils.AI.Solutions.Hands;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App.MVC.Base.View
{
    public abstract class BaseTrainerView
    {
        public abstract void DisplayDetections(HandDetectionCollection detections, Matrix frame);
    }
}
