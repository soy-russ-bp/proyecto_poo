using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.Versioning;
using ARJE.SignTrainer.App;
using ARJE.Utils.AI.Solutions.Hands;
using Keras.Models;
using Numpy;
using Python.Runtime;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.SignPractice
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public sealed class CustomModel
    {
        public CustomModel()
        {
            this.HandsModel.Start(PythonProxyApp.AppInfo);

            np.arange(1);
            PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                Keras.Keras.DisablePySysConsoleLog = true;
                this.ActionsModel = BaseModel.LoadModel("LSM-model.h5");
            }
        }

        private HandsModel HandsModel { get; } = new HandsModel(new HandsModelConfig(1));

        private BaseModel ActionsModel { get; }

        private Queue<float[]> Sequence { get; } = new();

        public void ProcessFrame(Matrix frame)
        {
            const float threshold = 0.4f; // TODO
            HandDetectionCollection hands = this.HandsModel.Process(frame);
            if (hands.Count == 0)
            {
                this.Clear();
                return;
            }

            HandDetection hand = hands.First();
            float[] rawDetection = ToFlatArray(hand.Landmarks.Positions);
            this.Sequence.Enqueue(rawDetection);
            this.Sequence.Enqueue(rawDetection);
            if (this.Sequence.Count < 30)
            {
                return;
            }

            if (this.Sequence.Count > 30)
            {
                this.Sequence.Dequeue();
                this.Sequence.Dequeue();
            }

            using (Py.GIL())
            {
                NDarray predictions = this.ActionsModel.Predict(this.GetSequenceForPrediction())[0];
                int prediction = int.Parse(np.argmax(predictions).repr, CultureInfo.InvariantCulture);
                Debug.WriteLine(new string[] { "a", "b", "c" }[prediction]);
            }
        }

        public void Clear()
        {
            this.Sequence.Clear();
        }

        private static float[] ToFlatArray(IReadOnlyList<Vector3> positions)
        {
            var flatArray = new float[positions.Count * 3];
            int flatI = 0;
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3 pos = positions[i];
                flatArray[flatI++] = pos.X;
                flatArray[flatI++] = pos.Y;
                flatArray[flatI++] = pos.Z;
            }

            return flatArray;
        }

        private NDarray GetSequenceForPrediction()
        {
            float[] flatArray = this.Sequence.ToArray().SelectMany(e => e).ToArray();
            var npArray = new NDarray<float>(flatArray).reshape(30, 21 * 3);
            return np.expand_dims(npArray, 0);
        }
    }
}
