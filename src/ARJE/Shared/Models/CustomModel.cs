using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
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
        public CustomModel(
            HandsModel handsModel,
            OnDiskModelTrainingConfigCollection configCollection,
            IModelTrainingConfig<IModelConfig> trainingConfig)
        {
            InitializePythonEngine();

            this.HandsModel = handsModel;
            this.ConfigCollection = configCollection;
            this.TrainingConfig = trainingConfig;
            this.Sequence = new Queue<float[]>(trainingConfig.SampleLength);
        }

        public static bool PythonEngineInitialized { get; private set; }

        public OnDiskModelTrainingConfigCollection ConfigCollection { get; }

        public IModelTrainingConfig<IModelConfig> TrainingConfig { get; }

        [MemberNotNullWhen(true, nameof(ActionsModel))]
        public bool ModelLoaded => this.ActionsModel != null;

        public bool IsProxyConnected => this.HandsModel.IsProxyConnected;

        public bool Ready => this.IsProxyConnected && this.ModelLoaded;

        private HandsModel HandsModel { get; }

        private BaseModel? ActionsModel { get; set; }

        private Queue<float[]> Sequence { get; }

        public static void InitializePythonEngine()
        {
            if (PythonEngineInitialized)
            {
                return;
            }

            PythonEngineInitialized = true;

            Keras.Keras.DisablePySysConsoleLog = true;
            np.arange(1);
            PythonEngine.BeginAllowThreads();
        }

        [MemberNotNull(nameof(ActionsModel))]
        public void LoadModel()
        {
            if (this.ModelLoaded)
            {
                return;
            }

            string modelPath = this.ConfigCollection.GetFullPathForConfig(
                this.TrainingConfig,
                OnDiskModelTrainingConfigCollection.ModelFileSuffix);

            using (Py.GIL())
            {
                this.ActionsModel = BaseModel.LoadModel(modelPath);
            }
        }

        public string? ProcessFrame(Matrix frame, out IDetection? detection)
        {
            this.LoadModel();

            HandDetection? hand = this.HandsModel.Process(frame).FirstOrDefault();
            detection = hand;

            if (hand == null)
            {
                this.Clear();
                return null;
            }

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
