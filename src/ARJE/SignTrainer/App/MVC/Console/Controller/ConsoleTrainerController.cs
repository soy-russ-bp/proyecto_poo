using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ARJE.SignTrainer.App.MVC.Base.Controller;
using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.SignTrainer.App.MVC.Console.View;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Spectre.Console;
using EnumsNET;
using Spectre.Console;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App.MVC.Console.Controller
{
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

        private SamplesCollector<HandDetectionCollection, HandDetection, Matrix>? CurrentSamplesCollector { get; set; }

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

            var trainingState = new ModelTrainingState(configCollection, selectedModel);
            this.DisplaySamplesState(trainingState);

            do
            {
                if (trainingState.Completed)
                {
                    if (this.View.Confirm("Export"))
                    {
                        CustomModelCreator.Train(
                            selectedModel,
                            trainingState,
                            configCollection.GetFullPathForFile(selectedModel, $"{selectedModel.Title}-model"));
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
            SamplesCollector<HandDetectionCollection, HandDetection, Matrix> samplesCollector
                = new(this.Model.VideoSource, this.Model.Detector, this.Model.SyncCtx, modelConfig.SampleCount, modelConfig.SamplesPerSecond);
            this.CurrentSamplesCollector = samplesCollector;

            NotifyUserCollectorStart();
            samplesCollector.Start();

            ReadOnlyCollection<HandDetectionCollection> newSamples = samplesCollector.Wait();
            bool validSamples = trainingState.AddSamples(selectedLabel, newSamples);
            if (!validSamples)
            {
                this.View.DisplayErrorMsg("Invalid samples, keep your hand in sight.");
            }

            this.Model.VideoSource.OnFrameGrabbed -= this.OnFrameGrabbed;
            this.SelectedLabel = null;
            this.CurrentSamplesCollector = null;
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
                $"(${this.CurrentSamplesCollector!.CollectedSamplesCount + 1}/{this.CurrentSamplesCollector.SampleCount})",
                detections,
                frame);
        }
    }
}
