using ARJE.Shared.Models;
using ARJE.Utils.Avalonia.MVC.Models;
using ARJE.Utils.Video;
using Avalonia.Threading;
using OpenCvSharp.Internal.Vectors;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.Models
{
    public sealed record PracticeDataModel(
        IAsyncVideoSource<Matrix> VideoSource,
        CustomModel DetectionModel)
        : DataModelBase
    {
        public AsyncGrabConfig GrabConfig { get; } = new(SynchronizationContext: new AvaloniaSynchronizationContext());

        public VectorOfByte FrameEncodeBuffer { get; } = new();

        public override void Dispose()
        {
            this.FrameEncodeBuffer.Dispose();
            this.DetectionModel.Clear();
        }
    }
}
