using System;
using System.IO;
using System.Runtime.Versioning;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.IO;
using ARJE.Utils.Python.Environment;
using ARJE.Utils.Python.Launcher;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Matrix = Emgu.CV.Mat;

namespace ARJE.SignTrainer
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    internal class Program
    {
        private static void Main()
        {
            try
            {
                App();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
                throw;
            }
        }

        private static void App()
        {
            Console.WriteLine("Start");

            DirectoryInfo proxyDir = new(Path.Combine(PathUtils.GoUpToFolder(AppContext.BaseDirectory, "ARJE"), "PythonProxy"));
            PythonAppInfo<VenvInfo> appInfo = new(new VenvInfo(".venv"), proxyDir, "app");

            Console.Write("Launch proxy? (y/n): ");
            char key = char.ToLower(Console.ReadKey().KeyChar);
            var model = key switch
            {
                'n' => HandsModel.StartNoLaunch(),
                _ => HandsModel.Start(appInfo),
            };

            using Matrix videoFrame = new();
            using VideoCapture videoCapture = new();

            while (videoCapture.IsOpened)
            {
                videoCapture.Read(videoFrame);
                CvInvoke.Flip(videoFrame, videoFrame, FlipType.Horizontal);

                HandsDetectionResult detections = model.Process(videoFrame);
                foreach (HandDetection detection in detections)
                {
                    DetectionDrawer.Draw(videoFrame, detection);
                }

                CvInvoke.Imshow("Video test", videoFrame);
                CvInvoke.WaitKey(1);
            }
        }
    }
}
