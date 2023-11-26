using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Collections.Extensions;
using Keras;
using Keras.Layers;
using Keras.Models;
using Keras.Utils;
using Numpy;

namespace ARJE.SignTrainer.App.MVC.Base.Model
{
    public static class CustomModelCreator
    {
        private static readonly string[] Metrics = new string[] { "categorical_accuracy" };

        public static void Train(IModelTrainingConfig<IModelConfig> trainingConfig, ModelTrainingState trainingData, string savePath)
        {
            Preprocess(trainingConfig, trainingData, out float[] sequences, out int[] labels);
            var featuresShape = new Numpy.Models.Shape(
                trainingConfig.Labels.Count * trainingConfig.SampleCount,
                trainingConfig.SampleCount,
                trainingConfig.LandmarkCount * 3);
            NDarray npFeatures = new NDarray<float>(sequences).reshape(featuresShape);
            NDarray npLabels = Util.ToCategorical(labels, dtype: "int32");
            Train(npFeatures, npLabels, savePath);
        }

        private static void Preprocess(
            IModelTrainingConfig<IModelConfig> trainingConfig,
            ModelTrainingState trainingData,
            out float[] sequences,
            out int[] labels)
        {
            ReadOnlyDictionary<string, int> labelMap = CreateLabelMap(trainingConfig.Labels).AsReadOnly();
            var sequencesList = new List<float>();
            var labelsList = new List<int>();
            foreach (string action in trainingConfig.Labels)
            {
                foreach (var sequence in trainingData.GetSamples(action))
                {
                    List<float> window = new();
                    foreach (ReadOnlyCollection<Vector3> frame in sequence)
                    {
                        foreach (var pos in frame)
                        {
                            window.Add(pos.X);
                            window.Add(pos.Y);
                            window.Add(pos.Z);
                        }
                    }

                    sequencesList.AddRange(window);
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

        private static void Train(NDarray features, NDarray labels, string savePath)
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
            model.Fit(features, labels, epochs: 250);
            model.Save(savePath);
        }
    }
}
