using System;
using System.IO;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.IO.Extensions;
using ARJE.Utils.Json;

namespace ARJE.SignTrainer.App.MVC.Base.Model
{
    public class OnDiskModelTrainingConfigCollection : ModelTrainingConfigCollection
    {
        public OnDiskModelTrainingConfigCollection(DirectoryInfo saveDirectory)
        {
            ArgumentNullException.ThrowIfNull(saveDirectory);

            this.SaveDirectory = saveDirectory;
        }

        private DirectoryInfo SaveDirectory { get; }

        public override void Add(IModelTrainingConfig<IModelConfig> config)
        {
            base.Add(config);
            string configPath = this.GetConfigPath(config);
            JsonWrite.ToFile(configPath, config, indented: true);
        }

        public override void Remove(string configTitle)
        {
            base.Remove(configTitle);
            string configPath = this.GetConfigPath(configTitle);
            File.Delete(configPath);
        }

        public void Update()
        {
            this.Clear();
            foreach (FileInfo file in this.SaveDirectory.EnumerateFilesWithExtension("json"))
            {
                var config = JsonRead.FromFile<ModelTrainingConfig<IModelConfig>>(file.FullName);
                this.Add(config);
            }
        }

        private string GetConfigPath(IModelTrainingConfig<IModelConfig> config)
        {
            return this.GetConfigPath(config.Title);
        }

        private string GetConfigPath(string configTitle)
        {
            return Path.Join(this.SaveDirectory.FullName, configTitle + ".json");
        }
    }
}
