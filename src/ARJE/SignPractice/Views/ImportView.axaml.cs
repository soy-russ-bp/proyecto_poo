using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media;

namespace ARJE.SignPractice.Views
{
    public partial class ImportView : UserControl
    {
        String selectedFilePath;
        public ImportView()
        {
            this.InitializeComponent();
        }

        private async void MyButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filters.Add(new FileDialogFilter() { Name = "JSON Files", Extensions = { "json" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });
            dlg.AllowMultiple = false;

            // Obtén la ventana principal de tu aplicación (ajusta esto según la estructura de tu aplicación)
            var mainWindow = (MainWindow)this.VisualRoot;

            // Asegúrate de que mainWindow no sea nulo antes de pasarla a ShowAsync
            if (mainWindow != null)
            {
                var selectedFiles = await dlg.ShowAsync(mainWindow);

                // Si el usuario seleccionó al menos un archivo
                if (selectedFiles != null && selectedFiles.Length > 0)
                {
                    // Obtener la ruta del archivo seleccionado
                    selectedFilePath = selectedFiles[0];

                    // Hacer algo con la ruta del archivo, como guardarlo en la memoria de tu programa
                    // Puedes asignar la ruta a tu TextBox o realizar otras operaciones según tus necesidades
                    var fileNameTextBox = this.FindControl<TextBox>("File_Name");
                    fileNameTextBox.Text = selectedFilePath;
                 }
            }
        }

        private async void SaveInMemoryApp(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var fileNameTextBox = this.FindControl<TextBox>("File_Name");
            if(fileNameTextBox != null){
                if(File.Exists(selectedFilePath)){
                    Directory.CreateDirectory("Models_json");
                    // Obtener el nombre del archivo seleccionado
                    string fileName = Path.GetFileName(selectedFilePath);

                    // Combinar la ruta de la carpeta "Models" con el nombre del archivo
                    string destinationPath = Path.Combine("Models_json", fileName);

                    File.Copy(selectedFilePath, destinationPath);
                    fileNameTextBox.Text = null;
                }
                
            }
        }
    }
}
