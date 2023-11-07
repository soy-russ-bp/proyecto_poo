using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using OpenCvSharp;
using Spectre.Console;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App.View
{
    public sealed class ConsoleTrainerView : BaseTrainerView
    {
        public void Clear()
        {
            AnsiConsole.Clear();
        }

        public T Prompt<T>(IPrompt<T> prompt)
        {
            return AnsiConsole.Prompt(prompt);
        }

        public override void DisplayDetections(HandDetectionCollection detections, Matrix frame)
        {
            foreach (HandDetection detection in detections)
            {
                DetectionDrawer.Draw(frame, detection);
            }

            Cv2.ImShow("Video test", frame);
            Cv2.WaitKey(1);
        }
    }
}
