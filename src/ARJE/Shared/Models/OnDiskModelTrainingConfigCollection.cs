using System.IO.Compression;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.IO;
using ARJE.Utils.Json;

namespace ARJE.Shared.Models
{
    public class OnDiskModelTrainingConfigCollection : ModelTrainingConfigCollection
    {
        public const string ConfigFileSuffix = "-config.json";

        public const string SamplesFileSuffix = "-samples.json";

        public const string ModelFileSuffix = "-model.h5";

        public const string ModelExportExtension = ".arje";

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

        public override bool IsValidTitle(string title)
        {
            return title.All(char.IsAsciiLetter);
        }

        public void Update()
        {
            this.Clear();
            foreach (DirectoryInfo directory in this.SaveDirectory.EnumerateDirectories())
            {
                FileInfo file = directory.EnumerateFiles().Single(file => file.Name.EndsWith(ConfigFileSuffix));
                var config = JsonRead.FromFile<ModelTrainingConfig<IModelConfig>>(file.FullName);
                this.Add(config);
            }
        }

        public void Train(IModelTrainingConfig<IModelConfig> trainingConfig, ModelTrainingState trainingState, int epochs)
        {
            string savePath = this.GetFullPathForConfig(trainingConfig, ModelFileSuffix);
            CustomModelCreator.Train(
                trainingConfig,
                trainingState,
                epochs,
                savePath);
        }

        public void Export(IModelTrainingConfig<IModelConfig> trainingConfig, out string exportPath, string? destinationPath = null)
        {
            string configTitle = trainingConfig.Title;
            string configDir = this.GetConfigDirectoryPath(configTitle);
            string fileName = $"{configTitle}{ModelExportExtension}";
            string tempPath = PathUtils.TempPathJoin(fileName);
            exportPath = destinationPath ?? Path.Join(configDir, fileName);

            File.Delete(tempPath);
            File.Delete(exportPath);

            ZipFile.CreateFromDirectory(
                configDir,
                tempPath,
                CompressionLevel.SmallestSize,
                includeBaseDirectory: false);

            File.Move(tempPath, exportPath, overwrite: true);
        }

        public string GetFullPathForConfig(IModelTrainingConfig<IModelConfig> config, string fileSuffix)
        {
            return this.GetFullPathForFile(config, $"{config.Title}{fileSuffix}");
        }

        private string GetFullPathForFile(IModelTrainingConfig<IModelConfig> config, string fileName)
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
            return Path.Join(configDirectoryPath, configTitle + ConfigFileSuffix);
        }

        private string GetConfigDirectoryPath(string configTitle)
        {
            return Path.Join(this.SaveDirectory.FullName, configTitle);
        }
    }
}
