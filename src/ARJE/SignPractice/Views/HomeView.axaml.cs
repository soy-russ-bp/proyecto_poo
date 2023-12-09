using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ARJE.SignPractice.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}