using System.Threading;

namespace ARJE.Utils.Video
{
    public interface IAsyncVideoSource<TMatrix> : IBaseVideoSource<TMatrix>
    {
        public delegate void FrameGrabbedHandler(TMatrix frame);

        public event FrameGrabbedHandler OnFrameGrabbed;

        public GrabState GrabState { get; }

        public void StartGrab(AsyncGrabConfig grabConfig);

        public void PauseGrab();

        public void StopGrab();
    }
}
