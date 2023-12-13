using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using ARJE.Shared.Proxy;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using Keras.Models;
using Numpy;
using Numpy.Models;
using Python.Runtime;
using Matrix = OpenCvSharp.Mat;

namespace ARJE.Shared.Models
{
    public sealed class CustomModel
    {
        static CustomModel()
        {
            np.arange(1);
            PythonEngine.BeginAllowThreads();
            Keras.Keras.DisablePySysConsoleLog = true;
        }

        public CustomModel(
            OnDiskModelTrainingConfigCollection configCollection,
            IModelTrainingConfig<IModelConfig> trainingConfig)
        {
            this.TrainingConfig = trainingConfig;
            this.HandsModel.Start(PythonProxyApp.AppInfo);

            string modelPath = configCollection.GetFullPathForConfig(
                trainingConfig,
                OnDiskModelTrainingConfigCollection.ModelFileSuffix);

            using (Py.GIL())
            {
                this.ActionsModel = BaseModel.LoadModel(modelPath);
            }

            this.Sequence = new Queue<float[]>(trainingConfig.SampleLength);
        }

        public IModelTrainingConfig<IModelConfig> TrainingConfig { get; }

        private HandsModel HandsModel { get; } = new HandsModel(new HandsModelConfig(1));

        private BaseModel ActionsModel { get; }

        private Queue<float[]> Sequence { get; }

        public string? ProcessFrame(Matrix frame, out IDetection? detection)
        {
            HandDetectionCollection hands = this.HandsModel.Process(frame);
            if (hands.Count == 0)
            {
                this.Clear();
                detection = null;
                return null;
            }

            HandDetection hand = hands.First();
            detection = hand;
            float[] rawDetection = ToFlatArray(hand.Landmarks.Positions);
            this.Sequence.Enqueue(rawDetection);
            if (this.Sequence.Count < this.TrainingConfig.SampleLength)
            {
                return null;
            }

            if (this.Sequence.Count > this.TrainingConfig.SampleLength)
            {
                this.Sequence.Dequeue();
            }

            using (Py.GIL())
            {
                NDarray sequenceArray = this.GetSequenceForPrediction();
                NDarray predictions = this.ActionsModel.Predict(sequenceArray)[0];
                int prediction = GetMaxPredictionIndex(predictions, out float confidence);
                Debug.WriteLine(this.TrainingConfig.Labels[prediction] + ": " + confidence);
                return this.TrainingConfig.Labels[prediction];
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

        private static int GetMaxPredictionIndex(NDarray predictions, out float confidence)
        {
            string predictionStr = np.argmax(predictions).repr;
            int predictionI = ParseInt(predictionStr);
            string confidenceStr = predictions[predictionI].repr;
            confidence = ParseFloat(confidenceStr);
            return predictionI;

            static int ParseInt(string str) => int.Parse(str, CultureInfo.InvariantCulture);
            static float ParseFloat(string str) => float.Parse(str, CultureInfo.InvariantCulture);
        }

        private NDarray GetSequenceForPrediction()
        {
            float[] flatArray = this.Sequence.ToArray().SelectMany(e => e).ToArray();
            Shape arrayShape = new(
                this.TrainingConfig.SampleLength,
                this.TrainingConfig.ModelConfig.LandmarkCount * 3);
            var npArray = new NDarray<float>(flatArray).reshape(arrayShape);
            return np.expand_dims(npArray, 0);
        }
    }
}
