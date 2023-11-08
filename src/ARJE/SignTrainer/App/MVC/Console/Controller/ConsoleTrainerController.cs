using System;
using System.Threading.Tasks;
using ARJE.SignTrainer.App.MVC.Base.Controller;
using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.SignTrainer.App.MVC.Console.View;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Spectre.Console.Extensions;
using ARJE.Utils.Threading;
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

        private SelectionPrompt<MainMenuOption> MainMenuPrompt { get; } = CreateMainMenuPrompt();

        public override void Run()
        {
            var syncCtx = new SingleThreadSynchronizationContext();
            this.Model.VideoSource.StartGrab(syncCtx);
            this.Model.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;
            Task.Run(this.RunUI);
            syncCtx.RunOnCurrentThread();
        }

        private static SelectionPrompt<MainMenuOption> CreateMainMenuPrompt()
        {
            var prompt = new SelectionPrompt<MainMenuOption>()
            {
                Title = "Options:",
                WrapAround = true,
            };

            prompt.AddEnumChoices();

            prompt.UseStyledConverter(
                option => option switch
                {
                    MainMenuOption.Select => new Style(Color.Lime),
                    MainMenuOption.Import => new Style(Color.Orange1),
                    MainMenuOption.Create => new Style(Color.Fuchsia),
                    MainMenuOption.Exit => new Style(Color.Red),
                    _ => throw new ArgumentOutOfRangeException(),
                });

            return prompt;
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
            MainMenuOption option = this.View.Prompt(this.MainMenuPrompt);
            switch (option)
            {
                case MainMenuOption.Select:
                    break;
                case MainMenuOption.Import:
                    break;
                case MainMenuOption.Create:
                    new ConsoleModelCreator(this).AskForModelInfo();
                    break;
                case MainMenuOption.Exit:
                default:
                    return false;
            }

            return true;
        }

        private void OnFrameGrabbed(Matrix frame)
        {
            HandDetectionCollection detections = this.Model.Detector.Process(frame);
            this.View.DisplayDetections(detections, frame);
        }
    }
}
