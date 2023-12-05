using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using ARJE.Utils.Video;

namespace ARJE.Utils.AI
{
    public record SamplesCollector<TDetectionCollection, TDetection, TMatrix>(
            IAsyncVideoSource<TMatrix> Camera,
            IDetectionModel<TDetectionCollection, TDetection, TMatrix> DetectionModel,
            SynchronizationContext SynchronizationContext,
            int SampleLength,
            int SamplesPerSecond,
            int WarmUpFrames = 25)
        where TDetectionCollection : IDetectionCollection<TDetection>
        where TDetection : IDetection
    {
        private readonly TDetectionCollection[] samples = new TDetectionCollection[SampleLength];

        private readonly ManualResetEventSlim samplesReadyEvent = new(initialState: false);

        public delegate void SamplesReadyHandler(ReadOnlyCollection<TDetectionCollection> samples);

        public event SamplesReadyHandler? OnSamplesReady;

        public int CollectedSamplesCount { get; private set; }

        public bool Collecting { get; private set; }

        public int WarmUpFramesLeft { get; set; } = WarmUpFrames;

        public ReadOnlyCollection<TDetectionCollection>? SamplesResult { get; private set; }

        public void Start()
        {
            if (this.Collecting)
            {
                throw new InvalidOperationException("Already started collecting.");
            }

            this.Collecting = true;
            var grabConfig = new AsyncGrabConfig(new FpsCap(this.SamplesPerSecond), this.SynchronizationContext);
            this.Camera.StartGrab(grabConfig);
            this.Camera.OnFrameGrabbed += this.OnFrameGrabbed;
        }

        public ReadOnlyCollection<TDetectionCollection> Wait()
        {
            this.samplesReadyEvent.Wait();
            return this.SamplesResult!;
        }

        private void OnFrameGrabbed(TMatrix frame)
        {
            if (this.WarmUpFramesLeft > 0)
            {
                this.WarmUpFramesLeft--;
                return;
            }

            TDetectionCollection detectionCollection = this.DetectionModel.Process(frame);
            this.samples[this.CollectedSamplesCount] = detectionCollection;
            this.CollectedSamplesCount++;

            if (this.CollectedSamplesCount == this.SampleLength)
            {
                ReadOnlyCollection<TDetectionCollection> readonlySamples = this.samples.AsReadOnly();
                this.SamplesResult = readonlySamples;
                this.samplesReadyEvent.Set();
                this.NotifySamplesReady(readonlySamples);
                this.Collecting = false;
            }
        }

        private void NotifySamplesReady(ReadOnlyCollection<TDetectionCollection> readonlySamples)
        {
            this.Camera.StopGrab();
            this.Camera.OnFrameGrabbed -= this.OnFrameGrabbed;
            this.OnSamplesReady?.Invoke(readonlySamples);
        }
    }
}