using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.SignTrainer.App.MVC.Console.Controller;
using ARJE.SignTrainer.App.MVC.Console.View;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
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

            using var detectionModel = new HandsModel(new HandsModelConfig());
            Task detectionModelTask = launchProxy
                ? detectionModel.StartAsync(PythonProxyApp.AppInfo)
                : detectionModel.StartNoLaunchAsync();

            if (!launchProxy)
            {
                AnsiConsole.Write("Waiting for proxy in pipe: ");
                AnsiConsole.MarkupLine($"\"[{Color.Orange1}]{detectionModel.PipeIdentifier}[/]\".");
            }

            var stopwatch = Stopwatch.StartNew();

            using IAsyncVideoSource<Matrix> videoSource = new Webcam(outputFlipType: FlipType.Horizontal);
            DirectoryInfo modelsDir = Directory.CreateDirectory("Models");
            var modelTrainingConfigCollection = new OnDiskModelTrainingConfigCollection(modelsDir);
            var model = new TrainerModel(videoSource, detectionModel, modelTrainingConfigCollection).Validate();
            var view = new ConsoleTrainerView(model.SyncCtx);
            var controller = new ConsoleTrainerController(model, view);
            detectionModelTask.Wait();

            stopwatch.Stop();
            AnsiConsole.WriteLine($"Init time: {stopwatch.Elapsed.TotalSeconds} sec");

            controller.Run();
        }
    }
}
