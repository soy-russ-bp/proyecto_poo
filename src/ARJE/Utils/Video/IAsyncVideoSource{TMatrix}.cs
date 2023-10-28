namespace ARJE.Utils.Video
{
    public interface IAsyncVideoSource<TMatrix> : IBaseVideoSource<TMatrix>
    {
        public delegate void FrameGrabbedHandler(TMatrix frame);

        public event FrameGrabbedHandler OnFrameGrabbed;

        public void StartGrab();

        public void PauseGrab();

        public void StopGrab();
    }
}
