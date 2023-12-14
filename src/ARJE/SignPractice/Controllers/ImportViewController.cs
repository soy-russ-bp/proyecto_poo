using System;
using System.IO;
using System.IO.Compression;
using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.MVC.Controllers;

namespace ARJE.SignPractice.Controllers
{
    public sealed class ImportViewController : ViewControllerBase<ImportView, ImportDataModel>
    {
        public ImportViewController(ImportDataModel model)
            : base(model)
        {
            this.View.OnBackBtnClick += this.OnBackBtnClick;
            this.View.OnSelectBtnClick += this.PickFile;
            this.View.OnSaveBtnClick += this.SaveModelInAppStorage;
        }

        private void OnBackBtnClick()
        {
            HomeViewController.Instance.GoToHome();
        }

        private async void PickFile()
        {
            string? selectedFile = await this.View.OpenFilePickerAsync(this.Model.FilePickerOptions);
            if (selectedFile == null)
            {
                return;
            }

            this.View.FilePathText = selectedFile;
        }

        private void SaveModelInAppStorage()
        {
            string? pathToFile = this.View.FilePathText;
            if (string.IsNullOrEmpty(pathToFile))
            {
                return;
            }

            this.Model.ModelsDirectory.Create();
            string modelsDir = this.Model.ModelsDirectory.FullName;
            string fileName = Path.GetFileNameWithoutExtension(pathToFile);
            string destinationPath = Path.Combine(modelsDir, fileName);

            try
            {
                ZipFile.ExtractToDirectory(pathToFile, destinationPath, true);
            }
            catch (Exception e)
            {
                this.View.SaveResultText = e.Message;
                return;
            }

            this.View.SaveResultText = $"Saved {fileName}.";
        }
    }
}
