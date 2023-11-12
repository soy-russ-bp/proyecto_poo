using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.SignTrainer.App.MVC.Console.Controller;
using ARJE.SignTrainer.App.MVC.Console.View;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Video;
using ARJE.Utils.Video.OpenCv;
using Spectre.Console;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public sealed class TrainerApp
    {
        public static void Run()
        {
            bool launchProxy = AnsiConsole.Confirm("Launch proxy?");

            var stopwatch = Stopwatch.StartNew();

            using var detectionModel = new HandsModel();
            Task detectionModelTask = launchProxy
                ? detectionModel.StartAsync(PythonProxyApp.AppInfo)
                : detectionModel.StartNoLaunchAsync();

            if (!launchProxy)
            {
                AnsiConsole.Write("Waiting for proxy in pipe: ");
                AnsiConsole.MarkupLine($"\"[{Color.Orange1}]{detectionModel.PipeName}[/]\".");
            }

            using IAsyncVideoSource<Matrix> videoSource = new Webcam(outputFlipType: FlipType.Horizontal);
            var model = new TrainerModel(videoSource, detectionModel);
            var view = new ConsoleTrainerView();
            var controller = new ConsoleTrainerController(model, view);
            detectionModelTask.Wait();

            stopwatch.Stop();
            AnsiConsole.WriteLine($"Init time: {stopwatch.Elapsed.TotalSeconds} sec");

            controller.Run();
        }
    }
}
