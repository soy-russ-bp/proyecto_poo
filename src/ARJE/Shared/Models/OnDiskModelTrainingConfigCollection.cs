using System.IO.Compression;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.IO;
using ARJE.Utils.Json;

namespace ARJE.Shared.Models
{
    public class OnDiskModelTrainingConfigCollection : ModelTrainingConfigCollection
    {
        public const string FileConfigSuffix = "-config.json";

        public const string ModelExportSuffix = ".arje";

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

        public void Train(string title)
        {
        }

        public void Export(string configTitle, out string exportPath, string? destinationPath = null)
        {
            string configDir = this.GetConfigDirectoryPath(configTitle);
            string fileName = $"{configTitle}{ModelExportSuffix}";
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
