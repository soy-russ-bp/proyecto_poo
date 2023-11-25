using System;
using System.IO;
using System.Linq;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Json;

namespace ARJE.SignTrainer.App.MVC.Base.Model
{
    public class OnDiskModelTrainingConfigCollection : ModelTrainingConfigCollection
    {
        private const string FileConfigSuffix = "-config.json";

        public OnDiskModelTrainingConfigCollection(DirectoryInfo saveDirectory)
        {
            ArgumentNullException.ThrowIfNull(saveDirectory);

            this.SaveDirectory = saveDirectory;
        }

        private DirectoryInfo SaveDirectory { get; }

        public override void Add(IModelTrainingConfig<IModelConfig> config)
        {
            base.Add(config);
            string configPath = this.CreateConfigPath(config);
            JsonWrite.ToFile(configPath, config, indented: true);
        }

        public override void Remove(string configTitle)
        {
            base.Remove(configTitle);
            string configDirectoryPath = this.GetConfigDirectoryPath(configTitle);
            Directory.Delete(configDirectoryPath, recursive: true);
        }

        public void Update()
        {
            this.Clear();
            foreach (DirectoryInfo directory in this.SaveDirectory.EnumerateDirectories())
            {
                FileInfo file = directory.EnumerateFiles().Single(file => file.Name.EndsWith(FileConfigSuffix));
                var config = JsonRead.FromFile<ModelTrainingConfig<IModelConfig>>(file.FullName);
                this.Add(config);
            }
        }

        public string GetFullPathForFile(IModelTrainingConfig<IModelConfig> config, string fileName)
        {
            string configDirectoryPath = this.GetConfigDirectoryPath(config.Title);
            return Path.Join(configDirectoryPath, fileName);
        }

        private string CreateConfigPath(IModelTrainingConfig<IModelConfig> config)
        {
            return this.CreateConfigPath(config.Title);
        }

        private string CreateConfigPath(string configTitle)
        {
            string configDirectoryPath = this.GetConfigDirectoryPath(configTitle);
            Directory.CreateDirectory(configDirectoryPath);
            return Path.Join(configDirectoryPath, configTitle + FileConfigSuffix);
        }

        private string GetConfigDirectoryPath(string configTitle)
        {
            return Path.Join(this.SaveDirectory.FullName, configTitle);
        }
    }
}
