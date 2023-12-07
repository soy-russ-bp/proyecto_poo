#pragma warning disable CA1822
using System;
using System.Collections.Generic;
using System.Threading;
using ARJE.SignTrainer.App.MVC.Base.View;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.System;
using ARJE.Utils.Threading;
using OpenCvSharp;
using Spectre.Console;
using Matrix = OpenCvSharp.Mat;
using SConsole = System.Console;

namespace ARJE.SignTrainer.App.MVC.Console.View
{
    public sealed class ConsoleTrainerView : BaseTrainerView
    {
        private const string WinId = nameof(DisplayCollectionState);

        public ConsoleTrainerView(SingleThreadSynchronizationContext syncCtx)
        {
            this.SyncCtx = syncCtx;
        }

        public SingleThreadSynchronizationContext SyncCtx { get; } = new();

        public void Clear()
        {
            AnsiConsole.Clear();
        }

        public T Prompt<T>(IPrompt<T> prompt)
        {
            return AnsiConsole.Prompt(prompt);
        }

        public T SelectionPrompt<T>(string title, IEnumerable<T> choices, Func<T, string>? displaySelector = null)
            where T : notnull
        {
            var prompt = new SelectionPrompt<T>()
            {
                Title = title,
                WrapAround = true,
            };

            prompt.AddChoices(choices);
            prompt.UseConverter(displaySelector);

            return this.Prompt(prompt);
        }

        public T TextPrompt<T>(string prompt, Func<T, bool>? validator = null)
        {
            var textPrompt = new TextPrompt<T>(prompt);
            if (validator != null)
            {
                textPrompt.Validate(validator);
            }

            return this.Prompt(textPrompt);
        }

        public T TextPrompt<T>(string prompt, T defaultValue, Func<T, bool>? validator = null)
        {
            var textPrompt = new TextPrompt<T>(prompt);
            textPrompt.DefaultValue(defaultValue);
            if (validator != null)
            {
                textPrompt.Validate(validator);
            }

            return this.Prompt(textPrompt);
        }

        public bool Confirm(string prompt, bool defaultValue = true)
        {
            return AnsiConsole.Confirm(prompt);
        }

        public void WaitKey(string? message = null)
        {
            if (message != null)
            {
                this.DisplayMsg(message);
            }

            SConsole.ReadKey(true);
        }

        public void DisplayMsg(string message)
        {
            AnsiConsole.WriteLine(message);
        }

        public void DisplayErrorMsg(string error)
        {
            AnsiConsole.MarkupLine($"[{Color.White} on {Color.Red}]{error}[/]");
            SConsole.Beep();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            ConsoleUtils.FlushInput();
        }

        public override void DisplayCollectionState(string title, HandDetectionCollection detections, Matrix frame)
        {
            foreach (HandDetection detection in detections)
            {
                DetectionDrawer.Draw(frame, detection);
            }

            Cv2.ImShow(WinId, frame);
            Cv2.SetWindowTitle(WinId, title);
            Cv2.WaitKey(1);
        }

        public override void CollectionEnded()
        {
            this.SyncCtx.Post(_ => Cv2.DestroyAllWindows(), null);
        }
    }
}
