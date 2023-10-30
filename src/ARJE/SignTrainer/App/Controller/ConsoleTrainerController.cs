using System;
using ARJE.SignTrainer.App.Model;
using ARJE.SignTrainer.App.View;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Spectre.Console.Extensions;
using Spectre.Console;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App.Controller
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
            this.Model.VideoSource.StartGrab();
            this.Model.VideoSource.OnFrameGrabbed += this.OnFrameGrabbed;

            while (this.RunLoop())
            {
            }
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

        private bool RunLoop()
        {
            MainMenuOption option = this.View.Prompt(this.MainMenuPrompt);
            switch (option)
            {
                case MainMenuOption.Select:
                    break;
                case MainMenuOption.Import:
                    break;
                case MainMenuOption.Create:
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
