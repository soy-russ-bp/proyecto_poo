﻿using System.IO;
using System.Threading.Tasks;
using ARJE.Shared.Models;
using ARJE.Shared.Proxy;
using ARJE.SignTrainer.App.MVC.Base;
using ARJE.SignTrainer.App.MVC.Console;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using Spectre.Console;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignTrainer.App
{
    public sealed class TrainerApp
    {
        public static void Run()
        {
            bool launchProxy = AnsiConsole.Confirm("Launch proxy?");

            using var detectionModel = new HandsModel(new HandsModelConfig(MaxNumHands: 1));

            Task detectionModelTask = launchProxy
                ? detectionModel.StartAsync(PythonProxyApp.AppInfo)
                : detectionModel.StartNoLaunchAsync();

            if (!launchProxy)
            {
                AnsiConsole.Write("Waiting for proxy in pipe: ");
                AnsiConsole.MarkupLine($"\"[{Color.Orange1}]{detectionModel.PipeIdentifier}[/]\".");
            }

            using IAsyncVideoSource<Matrix> videoSource = new Webcam(outputFlipType: FlipType.Horizontal);
            DirectoryInfo modelsDir = Directory.CreateDirectory("Models");
            var modelTrainingConfigCollection = new OnDiskModelTrainingConfigCollection(modelsDir);

            var model = new TrainerModel(videoSource, detectionModel, modelTrainingConfigCollection).Validate();
            var view = new ConsoleTrainerView(model.SyncCtx);
            var controller = new ConsoleTrainerController(model, view);
            detectionModelTask.Wait();

            controller.Run();
        }
    }
}
