using System.Collections.ObjectModel;
using System.Numerics;
using ARJE.Utils.AI;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Json;

namespace ARJE.Shared.Models
{
    public sealed class ModelTrainingState
    {
        public ModelTrainingState(
            OnDiskModelTrainingConfigCollection configCollection,
            IModelTrainingConfig<IModelConfig> trainingConfig)
        {
            this.TrainingConfig = trainingConfig;
            this.SamplesFilePath = configCollection.GetFullPathForFile(
                this.TrainingConfig,
                $"{this.TrainingConfig.Title}-samples.json");
            this.Samples = this.LoadSamples();
        }

        public IModelTrainingConfig<IModelConfig> TrainingConfig { get; }

        public bool Completed => this.Samples.All(sample => sample.Value.Count == this.TrainingConfig.SampleCount);

        private string SamplesFilePath { get; }

        private Dictionary<string, List<List<ReadOnlyCollection<Vector3>>>> Samples { get; }

        public List<List<ReadOnlyCollection<Vector3>>> GetSamples(string label)
        {
            return this.Samples[label];
        }

        public bool AddSamples<TDetection>(string label, IEnumerable<IDetectionCollection<TDetection>> samples)
            where TDetection : IDetection
        {
            List<List<ReadOnlyCollection<Vector3>>> labelSamples = this.GetSamples(label);
            List<ReadOnlyCollection<Vector3>> packedSamples = new();
            foreach (IDetectionCollection<TDetection> sample in samples)
            {
                if (sample.Count == 0)
                {
                    return false;
                }

                TDetection detection = sample[0]; // TODO
                packedSamples.Add(detection.Landmarks.Positions);
            }

            labelSamples.Add(packedSamples);
            return true;
        }

        public IEnumerable<KeyValuePair<string, IReadOnlyList<List<ReadOnlyCollection<Vector3>>>>> EnumerateSamples()
        {
            foreach (var sample in this.Samples)
            {
                yield return new(sample.Key, sample.Value.AsReadOnly());
            }
        }

        public IEnumerable<KeyValuePair<string, IReadOnlyList<List<ReadOnlyCollection<Vector3>>>>> EnumerateIncompleteSamples()
        {
            return this.EnumerateSamples().Where(sample => sample.Value.Count < this.TrainingConfig.SampleCount);
        }

        public void Save()
        {
            JsonWrite.ToFile(this.SamplesFilePath, this.Samples, indented: true);
        }

        private Dictionary<string, List<List<ReadOnlyCollection<Vector3>>>> LoadSamples()
        {
            if (!File.Exists(this.SamplesFilePath))
            {
                return this.TrainingConfig.Labels
                        .ToDictionary(
                            label => label,
                            label => new List<List<ReadOnlyCollection<Vector3>>>());
            }

            return JsonRead.FromFile<Dictionary<string, List<List<ReadOnlyCollection<Vector3>>>>>(this.SamplesFilePath);
        }
    }
}
