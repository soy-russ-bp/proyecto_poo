namespace ARJE.Utils.Video
{
    public interface IAsyncVideoSource<TMatrix> : IVideoSource<TMatrix>
    {
        public delegate void FrameGrabbedHandler(TMatrix frame);

        public event FrameGrabbedHandler OnFrameGrabbed;

        public void Start();

        public void Pause();

        public void Stop();
    }
}
