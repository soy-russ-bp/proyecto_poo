using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;
using ARJE.Utils.Video;
using Avalonia.Threading;
using OpenCvSharp.Internal.Vectors;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice.DataModels
{
    public record PracticeDataModel(
        IAsyncVideoSource<Matrix> VideoSource,
        CustomModel CustomModel)
        : DataModelBase
    {
        public AsyncGrabConfig GrabConfig { get; } = new(SynchronizationContext: new AvaloniaSynchronizationContext());

        public VectorOfByte FrameEncodeBuffer { get; } = new();

        public override void Dispose()
        {
            this.FrameEncodeBuffer.Dispose();
            base.Dispose();
        }
    }
}
