using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Versioning;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Solutions.Python;
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
            Console.WriteLine("Start");

            DirectoryInfo proxyDir = new(Path.Combine(PathUtils.GoUpToFolder(AppContext.BaseDirectory, "ARJE"), "PythonProxy"));
            PythonAppInfo<VenvInfo> appInfo = new(new VenvInfo(".venv"), proxyDir, "app");

            var holisticModel = HolisticModel.Start(appInfo);
            // var holisticModel = HolisticModel.StartNoLaunch();

            using Matrix videoFrame = new();
            using VideoCapture videoCapture = new();

            while (videoCapture.IsOpened)
            {
                videoCapture.Read(videoFrame);
                CvInvoke.Flip(videoFrame, videoFrame, FlipType.Horizontal);

                ReadOnlyCollection<Detection> detections = holisticModel.Process(videoFrame);
                foreach (Detection detection in detections)
                {
                    DetectionDrawer.Draw(videoFrame, detection);
                }

                CvInvoke.Imshow("Video test", videoFrame);
                CvInvoke.WaitKey(1);
            }
        }
    }
}
