using System.Collections.Generic;

namespace ARJE.Utils.AI.Configuration
{
    public interface IModelTrainingConfig<out TModelConfig>
        where TModelConfig : IModelConfig
    {
        public string Title { get; }

        public int SampleCount { get; }

        public int SampleLength { get; }

        public int SamplesPerSecond { get; }

        public IReadOnlyCollection<string> Labels { get; }

        public TModelConfig ModelConfig { get; }

        public string InfoPrint();
    }
}
