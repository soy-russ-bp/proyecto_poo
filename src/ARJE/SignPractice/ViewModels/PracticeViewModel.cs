﻿using ARJE.SignPractice.ViewModels;
using ReactiveUI;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace ARJE.SignPractice.ViewModels
{
    public class PracticeViewModel : ViewModelBase
    {
        Bitmap? frame;

        public Bitmap? Frame
        {
            get => frame;
            private set => this.RaiseAndSetIfChanged(ref frame, value);
        }

        public void SetFrame(Bitmap bm)
        {
            Frame = bm;
        }
    }
}
