using ARJE.Utils.Video;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.DataModels
{
    public record PracticeDataModel(
        IAsyncVideoSource<Matrix> VideoSource,
        CustomModel CustomModel)
        : DataModelBase
    {
    }
}
