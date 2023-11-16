using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ARJE.SignPractice.Views;

public partial class CreationView : UserControl
{
    public CreationView()
    {
        this.InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}