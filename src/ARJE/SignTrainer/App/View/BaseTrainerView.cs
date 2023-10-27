﻿using ARJE.Utils.AI.Solutions.Hands;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer.App.View
{
    public abstract class BaseTrainerView
    {
        public abstract void DisplayDetections(HandDetectionCollection detections, Matrix frame);
    }
}
