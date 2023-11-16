using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ARJE.SignTrainer.App.MVC.Base.Controller;
using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.SignTrainer.App.MVC.Console.View;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Spectre.Console;
using ARJE.Utils.Threading;
using ARJE.Utils.Video;
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

        public override void Run()
        {
            var syncCtx = new SingleThreadSynchronizationContext();
            this.Model.VideoSource.StartGrab(new AsyncGrabConfig(SynchronizationContext: syncCtx));
            this.Model.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            Task.Run(this.RunUI);
            syncCtx.RunOnCurrentThread();
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
            while (this.UILoop())
            {
                this.View.Clear();
            }
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
            IEnumerable<IModelTrainingConfig<IModelConfig>> configs = configCollection.Configs;
            IModelTrainingConfig<IModelConfig> selectedModel = this.View.SelectionPrompt("Models:", configs, m => m.Title);
        }

        private void OnCreate()
        {
            ModelTrainingConfig<HandsModelConfig> config = new ConsoleModelCreator(this).AskForTrainingConfig();
            this.Model.ModelTrainingConfigCollection.Add(config);
            this.View.Clear();
            this.OnSelect();
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            HandDetectionCollection detections = this.Model.Detector.Process(frame);
            this.View.DisplayDetections(detections, frame);
        }
    }
}
