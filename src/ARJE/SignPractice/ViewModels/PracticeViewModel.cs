using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Avalonia.OpenCvSharp.Extensions;
using ARJE.Utils.Video;
using Avalonia.Threading;
using OpenCvSharp.Internal.Vectors;
using Python.Runtime;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.ViewModels
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal sealed class PracticeViewModel : ViewModelBase, IDisposable
    {
        private readonly IAsyncVideoSource<Matrix> videoSource;
        private readonly HandsModel handsModel;
        private readonly ModelTrainingConfig<HandsModelConfig> trainingConfig;
        private readonly ModelTrainingState trainingState;
        private readonly OnDiskModelTrainingConfigCollection modelTrainingConfigCollection;

        private readonly AsyncGrabConfig grabConfig = new(
            SynchronizationContext: new AvaloniaSynchronizationContext());

        private readonly VectorOfByte frameEncodeBuffer = new();

        private Bitmap? frame;
        private string stateMsg = WaitingMsg;
        private string sampleCountState;
        
        private async Task onALLSamplesReady()
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(()=> StateMsg = "Training...");
            await TrainModel();
            MainWindowViewModel.Instance.GoToHome();
        }
        public string SampleCountState
        {
            get => this.sampleCountState;
            private set => this.RaiseAndSetIfChanged(ref this.sampleCountState, value);

        }
        public string StateMsg
        {
            get => this.stateMsg;
            private set => this.RaiseAndSetIfChanged(ref this.stateMsg, value);

        }

        public string Label
        {
            get => this.label;
            private set => this.RaiseAndSetIfChanged(ref this.label, value);

        }

        private Task TrainModel()
        {
            using (Py.GIL()) ;
            CustomModelCreator.Train(trainingConfig, trainingState, modelTrainingConfigCollection.GetFullPathForFile(trainingConfig, $"{trainingConfig.Title}-model.h5"));
            return Task.CompletedTask;
        }

        private const string WaitingMsg = "Waiting...";
        private int currentLabelIndex;
        private string label;

        public PracticeViewModel(IAsyncVideoSource<Matrix> videoSource, HandsModel handsModel, ModelTrainingConfig<HandsModelConfig> trainingConfig, OnDiskModelTrainingConfigCollection modelTrainingConfigCollection)
        {
            modelTrainingConfigCollection.Add(trainingConfig);
            trainingState = new ModelTrainingState(modelTrainingConfigCollection, trainingConfig);
            this.videoSource = videoSource;
            this.handsModel = handsModel;
            this.trainingConfig = trainingConfig;
            this.modelTrainingConfigCollection = modelTrainingConfigCollection;
            videoSource.StartGrab(this.grabConfig);
            videoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            ManageState();
        }
        private void ManageState()
        {
           
            if (string.IsNullOrEmpty(Label))
            {
                UpdateLabel();
            }
            int sampleCount = trainingState.GetSamples(Label).Count;
            UpdateSampleCount(sampleCount);
            if (sampleCount == trainingConfig.SampleCount)
            {
                currentLabelIndex++;
                
                if (currentLabelIndex == trainingConfig.Labels.Count)
                {
                    Task.Run(onALLSamplesReady);
                    return;

                }
                UpdateLabel();
            }
        }

        private void UpdateLabel()
        {

            Label = trainingConfig.Labels[currentLabelIndex];
        }
        private void UpdateSampleCount(int sampleCount)
        {   

            SampleCountState = $"{sampleCount} / {trainingConfig.SampleCount} ";

        }
        public Bitmap? Frame
        {
            get => this.frame;
            private set
            {
                this.frame?.Dispose();
                this.RaiseAndSetIfChanged(ref this.frame, value);
            }
        }

        public void Dispose()
        {
            this.videoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            this.videoSource.StopGrab();
        }

        private void OnFrameGrabbed(Matrix frame)
        {

            this.Frame = frame.ToAvaloniaBitmap(buffer: this.frameEncodeBuffer);

        }

        public void Onstart()
        {
            var sampleCollector = new SamplesCollector<HandDetectionCollection, HandDetection, Matrix>(videoSource, handsModel, grabConfig.SynchronizationContext, trainingConfig.SampleLength, trainingConfig.SamplesPerSecond);
            sampleCollector.OnSamplesReady += SampleCollector_OnSamplesReady;
            sampleCollector.Start();
            StateMsg = "Collecting";
        }


        private void SampleCollector_OnSamplesReady(ReadOnlyCollection<HandDetectionCollection> samples)
        {
            bool validSamples = trainingState.AddSamples(Label, samples);

            if (validSamples)
            {
                trainingState.Save();
            }
            StateMsg = validSamples ? "sample accepted" : "Invalid sample";
            ManageState();
        }
    }
}
