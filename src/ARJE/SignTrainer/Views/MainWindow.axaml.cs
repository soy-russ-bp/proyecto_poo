using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ARJE.SignPractice.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}