using System.IO;
using ARJE.Shared.Models;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Avalonia.MVC.Models;
using ARJE.Utils.OpenCvSharp;
using ARJE.Utils.Video;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.Models
{
    public record HomeDataModel(
        IAsyncVideoSource<Matrix> VideoSource,
        OnDiskModelTrainingConfigCollection ConfigCollection,
        HandsModel HandsModel)
        : DataModelBase
    {
        public static HomeDataModel Default
        {
            get
            {
                Webcam videoSource = new(outputFlipType: FlipType.Horizontal);
                DirectoryInfo modelsDir = Directory.CreateDirectory("Models");
                OnDiskModelTrainingConfigCollection configCollection = new(modelsDir);
                HandsModel handsModel = new(new HandsModelConfig(1));
                return new HomeDataModel(videoSource, configCollection, handsModel);
            }
        }
    }
}
