using System;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using ARJE.SignTrainer.App.Controller;
using ARJE.SignTrainer.App.Model;
using ARJE.SignTrainer.App.View;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.IO;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;
using ARJE.Utils.Video;
using ARJE.Utils.Video.OpenCV;
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
            using var detectionModel = new HandsModel();
            Task detectionModelTask = launchProxy
                ? detectionModel.StartAsync(GetProxyAppInfo())
                : detectionModel.StartNoLaunchAsync();

            if (!launchProxy)
            {
                AnsiConsole.Write("Waiting for proxy in pipe: ");
                AnsiConsole.MarkupLine($"\"[{Color.Orange1}]{detectionModel.PipeName}[/]\".");
            }

            detectionModelTask.Wait();

            using IAsyncVideoSource<Matrix> videoSource = new Webcam(outputFlipType: FlipType.Horizontal);
            var model = new TrainerModel(
                videoSource,
                detectionModel);
            var view = new ConsoleTrainerView();
            var controller = new ConsoleTrainerController(model, view);
            controller.Run();
        }

        private static PythonAppInfo<VenvInfo> GetProxyAppInfo()
        {
            string searchPath = GetProxySearchPath();
            DirectoryInfo proxyDir = new(Path.Combine(searchPath, "PythonProxy"));
            PythonAppInfo<VenvInfo> appInfo = new(new VenvInfo(".venv"), proxyDir, "app");
            return appInfo;
        }

        private static string GetProxySearchPath()
        {
            string searchPath = AppContext.BaseDirectory;
#if DEBUG
            searchPath = PathUtils.GoUpToFolder(searchPath, "ARJE");
#endif
            return searchPath;
        }
    }
}
