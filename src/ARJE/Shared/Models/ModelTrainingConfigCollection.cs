using ARJE.Utils.AI.Configuration;

namespace ARJE.Shared.Models
{
    public class ModelTrainingConfigCollection
    {
        public ModelTrainingConfigCollection()
        {
            this.Configs = this.ConfigsDict.Values;
        }

        public IReadOnlyCollection<IModelTrainingConfig<IModelConfig>> Configs { get; }

        private Dictionary<string, IModelTrainingConfig<IModelConfig>> ConfigsDict { get; } = new(StringComparer.OrdinalIgnoreCase);

        public virtual void Add(IModelTrainingConfig<IModelConfig> config)
        {
            this.ConfigsDict.Add(config.Title, config);
        }

        public virtual void Remove(string configTitle)
        {
            this.Remove(configTitle);
        }

        public bool TitleAvailable(string title)
        {
            return this.ConfigsDict.ContainsKey(title);
        }

        protected void Clear()
        {
            this.ConfigsDict.Clear();
        }
    }
}
