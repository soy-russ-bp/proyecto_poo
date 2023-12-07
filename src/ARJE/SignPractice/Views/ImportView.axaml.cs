using System.Collections.Generic;
using System.IO;
using System.Linq;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Views;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace ARJE.SignPractice.Views
{
    public sealed partial class ImportView : ViewBase
    {
        protected override void OnInitializeComponent() => this.InitializeComponent();

        private static FilePickerFileType ArjeModelsFilePicker { get; } = new("ARJE Models")
        {
            Patterns = new[] { "*.arje" },
        };

        private static FilePickerOpenOptions FilePickerOptions { get; } = new()
        {
            Title = "Open Model File",
            AllowMultiple = false,
            FileTypeFilter = new[] { ArjeModelsFilePicker },
        };

        private async void OnSelectBtnClick(object sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this)!;
            IReadOnlyList<IStorageFile> selectedFiles = await topLevel.StorageProvider.OpenFilePickerAsync(FilePickerOptions);

            string? pathToFile = selectedFiles.FirstOrDefault()?.Path.LocalPath;

            if (pathToFile == null)
            {
                return;
            }

            this.fileNameTextBox.Text = pathToFile;
        }

        private void SaveInAppStorage(object sender, RoutedEventArgs e)
        {
            string? pathToFile = this.fileNameTextBox.Text;
            if (!File.Exists(pathToFile))
            {
                return;
            }

            string modelsDir = Directory.CreateDirectory("Models").FullName;
            string fileName = Path.GetFileName(pathToFile);
            string destinationPath = Path.Combine(modelsDir, fileName);

            if (!File.Exists(destinationPath))
            {
                File.Copy(pathToFile, destinationPath);
            }

            this.fileNameTextBox.Clear();
        }
    }
}
