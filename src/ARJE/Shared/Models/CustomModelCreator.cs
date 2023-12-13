using System.Collections.ObjectModel;
using System.Numerics;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Collections.Extensions;
using Keras;
using Keras.Layers;
using Keras.Models;
using Keras.Utils;
using Numpy;
using NumpyShape = Numpy.Models.Shape;

namespace ARJE.Shared.Models
{
    public static class CustomModelCreator
    {
        private static readonly string[] Metrics = new[] { "categorical_accuracy" };

        public static void Train(IModelTrainingConfig<IModelConfig> trainingConfig, ModelTrainingState trainingData, int epochs, string savePath)
        {
            NumpyShape featuresShape = new(
                trainingConfig.Labels.Count * trainingConfig.SampleCount,
                trainingConfig.SampleLength,
                trainingConfig.ModelConfig.LandmarkCount * 3);
            Preprocess(trainingConfig, trainingData, featuresShape, out float[] sequences, out int[] labels);
            NDarray npFeatures = new NDarray<float>(sequences).reshape(featuresShape);
            NDarray npLabels = Util.ToCategorical(labels, dtype: "int32");
            Train(npFeatures, npLabels, epochs, savePath);
        }

        private static void Preprocess(
            IModelTrainingConfig<IModelConfig> trainingConfig,
            ModelTrainingState trainingData,
            NumpyShape featuresShape,
            out float[] sequences,
            out int[] labels)
        {
            ReadOnlyDictionary<string, int> labelMap = CreateLabelMap(trainingConfig.Labels).AsReadOnly();
            int sequenceLength = featuresShape[0] * featuresShape[1] * featuresShape[2];
            List<float> sequencesList = new(sequenceLength);
            List<int> labelsList = new(featuresShape[0]);
            foreach (string action in trainingConfig.Labels)
            {
                foreach (List<ReadOnlyCollection<Vector3>> sequence in trainingData.GetSamples(action))
                {
                    sequence.ForEach(frame => frame.ForEach(sequencesList.AddXYZ));
                    labelsList.Add(labelMap[action]);
                }
            }

            sequences = sequencesList.ToArray();
            labels = labelsList.ToArray();
        }

        private static Dictionary<string, int> CreateLabelMap(IEnumerable<string> labels)
        {
            return labels.Enumerated().ToDictionary(l => l.Item, l => l.Index);
        }

        private static void Train(NDarray features, NDarray labels, int epochs, string savePath)
        {
            var inputShape = new Shape(features.shape[1], features.shape[2]);
            var model = new Sequential();
            model.Add(new LSTM(64, return_sequences: true, activation: "relu", input_shape: inputShape));
            model.Add(new LSTM(128, return_sequences: true, activation: "relu"));
            model.Add(new LSTM(64, activation: "relu"));
            model.Add(new Dense(64, activation: "relu"));
            model.Add(new Dense(32, activation: "relu"));
            model.Add(new Dense(labels.shape[1], activation: "softmax"));

            model.Compile("Adam", "categorical_crossentropy", Metrics);
            model.Fit(features, labels, epochs: epochs);
            model.Save(savePath);
        }
    }
}
