using System;
using System.Collections.Generic;
using ARJE.Utils.Json;
using Newtonsoft.Json;

namespace ARJE.Utils.AI.Configuration
{
    public sealed record ModelTrainingConfig<TModelConfig>(
        string Title,
        int SampleSize,
        int SamplesPerSecond,
        IReadOnlyCollection<string> Labels,
        TModelConfig ModelConfig) : IModelTrainingConfig<TModelConfig>
        where TModelConfig : IModelConfig
    {
        [JsonConverter(typeof(TypeInfoConverter), typeof(IModelConfig))]
        public TModelConfig ModelConfig { get; } = ModelConfig;

        [JsonIgnore]
        public bool Validated { get; private set; }

        public ModelTrainingConfig<TModelConfig> Validate()
        {
            if (this.Validated)
            {
                return this;
            }

            string titleTrim = this.Title.Trim();
            ArgumentException.ThrowIfNullOrEmpty(titleTrim);
            ArgumentNullException.ThrowIfNull(this.ModelConfig);
            ArgumentNullException.ThrowIfNull(this.Labels);

            if (this.Title == titleTrim)
            {
                this.Validated = true;
                return this;
            }

            return this with { Title = titleTrim, Validated = true };
        }
    }
}
