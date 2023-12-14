using System.IO;
using ARJE.Utils.Avalonia.MVC.Models;
using Avalonia.Platform.Storage;

namespace ARJE.SignPractice.DataModels
{
    public record ImportDataModel(DirectoryInfo ModelsDirectory)
        : DataModelBase
    {
        public FilePickerOpenOptions FilePickerOptions { get; } = new()
        {
            Title = "Open Model File",
            AllowMultiple = false,
            FileTypeFilter = new[] { ArjeModelsFilePicker! },
        };

        private static FilePickerFileType ArjeModelsFilePicker { get; } = new("ARJE Models")
        {
            Patterns = new[] { "*.arje" },
        };
    }
}
