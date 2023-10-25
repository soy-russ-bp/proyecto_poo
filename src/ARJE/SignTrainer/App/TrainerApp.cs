using System;
using System.IO;
using System.Runtime.Versioning;
using ARJE.SignTrainer.App.Model;
using ARJE.SignTrainer.App.View;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.IO;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;
using ARJE.Utils.Video;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer.App
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public sealed class TrainerApp
    {
        public static void Run()
        {
            Console.WriteLine("Start");

            Console.Write("Launch proxy? (y/n): ");
            char key = char.ToLower(Console.ReadKey().KeyChar);
            HandsModel detectionModel = key switch
            {
                'n' => HandsModel.StartNoLaunch(),
                _ => HandsModel.Start(GetProxyAppInfo()),
            };

            using IVideoSource<Matrix> videoSource = new Camera(flipHorizontally: true);
            var model = new TrainerModel(
                videoSource,
                detectionModel);
            var view = new TrainerView(model);
            view.Run();
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
