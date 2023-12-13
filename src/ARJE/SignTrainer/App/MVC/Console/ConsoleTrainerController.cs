using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using ARJE.Shared.Models;
using ARJE.SignTrainer.App.MVC.Base;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Spectre.Console;
using ARJE.Utils.System.Extensions;
using EnumsNET;
using Spectre.Console;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App.MVC.Console
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public class ConsoleTrainerController : BaseTrainerController<ConsoleTrainerView>
    {
        public ConsoleTrainerController(TrainerModel model, ConsoleTrainerView view)
            : base(model, view)
        {
        }

        private enum MainMenuOption
        {
            Select,
            Import,
            Create,
            Exit,
        }

        private Func<MainMenuOption, string> MainMenuStyle { get; } = CreateMainMenuPromptStyledConverter();

        private string? SelectedLabel { get; set; }

        private SampleCollector<HandDetectionCollection, HandDetection, Matrix>? CurrentSampleCollector { get; set; }

        public override void Run()
        {
            Task.Run(this.RunUI);
            this.Model.SyncCtx.RunOnCurrentThread();
        }

        private static void NotifyUserCollectorStart()
        {
            for (int i = 0; i < 3; i++)
            {
                System.Console.Beep();
                Thread.Sleep(TimeSpan.FromSeconds(0.5f));
            }
        }

        private static Func<MainMenuOption, string> CreateMainMenuPromptStyledConverter()
        {
            return SelectionPromptUtils.CreateStyledConverter<MainMenuOption>(
                option => option switch
                {
                    MainMenuOption.Select => new Style(Color.Lime),
                    MainMenuOption.Import => new Style(Color.Orange1),
                    MainMenuOption.Create => new Style(Color.Fuchsia),
                    MainMenuOption.Exit => new Style(Color.Red),
                    _ => throw new ArgumentOutOfRangeException(),
                });
        }

        private void RunUI()
        {
            do
            {
                this.View.Clear();
            }
            while (this.UILoop());

            this.Model.SyncCtx.Complete();
        }

        private bool UILoop()
        {
            MainMenuOption option = this.ReadMainMenuOption();
            switch (option)
            {
                case MainMenuOption.Select:
                    this.OnSelect();
                    break;
                case MainMenuOption.Import:
                    break;
                case MainMenuOption.Create:
                    this.OnCreate();
                    break;
                case MainMenuOption.Exit:
                default:
                    return false;
            }

            return true;
        }

        private MainMenuOption ReadMainMenuOption()
        {
            return this.View.SelectionPrompt("Options:", Enums.GetValues<MainMenuOption>(), this.MainMenuStyle);
        }

        private void OnSelect()
        {
            OnDiskModelTrainingConfigCollection configCollection = this.Model.ModelTrainingConfigCollection;
            configCollection.Update();
            IReadOnlyCollection<IModelTrainingConfig<IModelConfig>> configs = configCollection.Configs;
            if (configs.Count == 0)
            {
                this.View.DisplayErrorMsg("No imported models.");
                return;
            }

            IModelTrainingConfig<IModelConfig> selectedModel = this.View.SelectionPrompt("Models:", configs, m => m.Title);
            this.View.DisplayMsg(selectedModel.InfoPrint());

            ((HandsModelConfig)selectedModel.ModelConfig).CopyTo(this.Model.Detector.ModelConfig);

            var trainingState = new ModelTrainingState(configCollection, selectedModel);
            this.DisplaySamplesState(trainingState);

            do
            {
                if (trainingState.Completed)
                {
                    if (this.View.Confirm("Export"))
                    {
                        const int MinEpochs = 50, MaxEpochs = 500;
                        int epochs = this.View.TextPrompt($"Epochs [{MinEpochs}, {MaxEpochs}]", 200, input => input.InRange(MinEpochs, MaxEpochs));
                        configCollection.Train(selectedModel, trainingState, epochs);
                        configCollection.Export(selectedModel, out string exportPath);
                        this.View.DisplayMsg($"Saved model to: '{exportPath}'");
                    }

                    this.View.WaitKey("Press any key...");
                    break;
                }

                if (!this.View.Confirm("Train"))
                {
                    break;
                }

                this.TrainModel(selectedModel, trainingState);
                this.DisplaySamplesState(trainingState);
            }
            while (true);
        }

        private void OnCreate()
        {
            ModelTrainingConfig<HandsModelConfig> config = new ConsoleModelCreator(this).AskForTrainingConfig();
            this.Model.ModelTrainingConfigCollection.Add(config);
            this.View.Clear();
            this.OnSelect();
        }

        private void TrainModel(IModelTrainingConfig<IModelConfig> modelConfig, ModelTrainingState trainingState)
        {
            string selectedLabel = this.View.SelectionPrompt("Collect samples:", trainingState.EnumerateIncompleteSamples(), s => s.Key).Key;
            this.SelectedLabel = selectedLabel;

            this.Model.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            SampleCollector<HandDetectionCollection, HandDetection, Matrix> sampleCollector
                = new(this.Model.VideoSource, this.Model.Detector, this.Model.SyncCtx, modelConfig.SampleLength, modelConfig.SamplesPerSecond);
            this.CurrentSampleCollector = sampleCollector;

            NotifyUserCollectorStart();
            sampleCollector.Start();

            ReadOnlyCollection<HandDetectionCollection> newSamples = sampleCollector.Wait();
            bool validSamples = trainingState.AddSamples(selectedLabel, newSamples);
            if (!validSamples)
            {
                this.View.DisplayErrorMsg("Invalid samples, keep your hand in sight.");
            }

            this.Model.VideoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            this.SelectedLabel = null;
            this.CurrentSampleCollector = null;
            this.View.CollectionEnded();
            trainingState.Save();
        }

        private void DisplaySamplesState(ModelTrainingState trainingState)
        {
            this.View.DisplayMsg($"Label count: {trainingState.TrainingConfig.Labels.Count}.");
            foreach (var sample in trainingState.EnumerateSamples())
            {
                this.View.DisplayMsg($"{sample.Key} - {sample.Value.Count} / {trainingState.TrainingConfig.SampleCount}");
            }
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            HandDetectionCollection detections = this.Model.Detector.Process(frame);
            this.View.DisplayCollectionState(
                $"Collecting samples: '{this.SelectedLabel}'" +
                $"(${this.CurrentSampleCollector!.CollectedSamplesCount + 1}/{this.CurrentSampleCollector.SampleLength})",
                detections,
                frame);
        }
    }
}
