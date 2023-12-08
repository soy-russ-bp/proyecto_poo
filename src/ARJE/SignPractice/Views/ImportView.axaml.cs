using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Views;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace ARJE.SignPractice.Views
{
    public sealed partial class ImportView : ViewBase
    {
        public event Action? OnSelectBtnClick;

        public event Action? OnSaveBtnClick;

        public string? FilePathText
        {
            get => this.filePathTextBox.Text;
            set => this.filePathTextBox.Text = value;
        }

        public string? SaveResultText
        {
            get => this.saveResultTextBlock.Text;
            set => this.saveResultTextBlock.Text = value;
        }

        public async Task<string?> OpenFilePickerAsync(FilePickerOpenOptions filePickerOptions)
        {
            var topLevel = TopLevel.GetTopLevel(this)!;
            IReadOnlyList<IStorageFile> selectedFiles = await topLevel.StorageProvider.OpenFilePickerAsync(filePickerOptions);
            return selectedFiles.FirstOrDefault()?.Path.LocalPath;
        }

        protected override void OnInitializeComponent() => this.InitializeComponent();

        private void SelectBtnClickHandler(object sender, RoutedEventArgs e)
        {
            this.OnSelectBtnClick?.Invoke();
        }

        private void SaveBtnClickHandler(object sender, RoutedEventArgs e)
        {
            this.OnSaveBtnClick?.Invoke();
        }
    }
}
