using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Arje.Views;

public partial class CreationView : UserControl
{
    public CreationView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}