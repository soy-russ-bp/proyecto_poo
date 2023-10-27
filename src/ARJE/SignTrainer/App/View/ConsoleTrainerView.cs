using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using Emgu.CV;
using Spectre.Console;

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

        public override void DisplayDetections(HandDetectionCollection detections, Mat frame)
        {
            foreach (HandDetection detection in detections)
            {
                DetectionDrawer.Draw(frame, detection);
            }

            CvInvoke.Imshow("Video test", frame);
            CvInvoke.WaitKey(1);
        }
    }
}
