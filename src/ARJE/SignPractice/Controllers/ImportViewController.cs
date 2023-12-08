using System;
using System.IO;
using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Controllers;

namespace ARJE.SignPractice.Controllers
{
    public sealed class ImportViewController : ViewControllerBase<ImportView, ImportDataModel, ImportViewModel>
    {
        public ImportViewController()
            : this(new ImportDataModel())
        {
        }

        private ImportViewController(ImportDataModel dataModel)
            : base(dataModel, new ImportViewModel(dataModel))
        {
        }

        protected override void OnViewInit()
        {
            base.OnViewInit();
            this.ViewModel.View.OnSelectBtnClick += this.PickFile;
            this.ViewModel.View.OnSaveBtnClick += this.SaveModelInAppStorage;
        }

        private async void PickFile()
        {
            string? selectedFile = await this.ViewModel.View.OpenFilePickerAsync(this.DataModel.FilePickerOptions);
            if (selectedFile == null)
            {
                return;
            }

            this.ViewModel.View.FilePathText = selectedFile;
        }

        private void SaveModelInAppStorage()
        {
            string? pathToFile = this.ViewModel.View.FilePathText;
            if (string.IsNullOrEmpty(pathToFile))
            {
                return;
            }

            string modelsDir = Directory.CreateDirectory("Models").FullName;
            string fileName = Path.GetFileName(pathToFile);
            string destinationPath = Path.Combine(modelsDir, fileName);

            try
            {
                File.Copy(pathToFile, destinationPath);
            }
            catch (Exception e)
            {
                this.ViewModel.View.SaveResultText = e.Message;
                return;
            }

            this.ViewModel.View.SaveResultText = $"Saved {fileName}.";
        }
    }
}
