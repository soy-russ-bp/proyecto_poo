using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Video;

namespace ARJE.Utils.AI
{
    public record SampleCollector<TDetectionCollection, TDetection, TMatrix>(
            IAsyncVideoSource<TMatrix> Camera,
            IDetectionModel<TDetectionCollection, TDetection, TMatrix> DetectionModel,
            int SamplesNumber,
            int SamplesPerSecond)
        where TDetectionCollection : IDetectionCollection<TDetection>
        where TDetection : IDetection

        // inicio de la clase:
        {
        private readonly TDetectionCollection[] samples = new TDetectionCollection[SamplesNumber];

        public int GrabbedFramesCount { get; private set; } // propiedad, su valor por defecto es 0

        public delegate void SamplesReadyHandler(ReadOnlyCollection<TDetectionCollection> samples); // lo que le llega a los suscriptores ;//tipo de función que va a usar mi evento

        public event SamplesReadyHandler OnSamplesReady; // le avisa a los suscriptores de que ya están listos los samples

        public void Start()
        {
             var grabConfig = new AsyncGrabConfig(this.SamplesPerSecond); // configura el thread que agarra los frames, le paso la cantidad max de fps (samplesPerSecond)
             this.Camera.StartGrab(grabConfig); // inicio el startgrab porque es lo que empieza a agarrar los frames de la cámara

             this.Camera.OnFrameGrabbed += this.OnFrameGrabbed; // me suscribo al evento (el de la derecha es el suscriptor)
        }

        private void OnFrameGrabbed(TMatrix frame)
        {
            TDetectionCollection detectionCollection = this.DetectionModel.Process(frame); // procesa el frame y devuelve los puntos
            this.samples[this.GrabbedFramesCount] = detectionCollection;
            this.GrabbedFramesCount += 1;

            if (this.GrabbedFramesCount == this.SamplesNumber)
            {
                this.Camera.StopGrab();
                this.Camera.OnFrameGrabbed -= this.OnFrameGrabbed; // me desuscribo al evento
                this.OnSamplesReady.Invoke(this.samples.AsReadOnly()); // a los suscriptores del evento les paso los samples como AsReadOnly
            }
        }
    }
}