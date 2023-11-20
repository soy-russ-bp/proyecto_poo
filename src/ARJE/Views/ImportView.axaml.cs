using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Arje.Views;

public partial class ImportView : UserControl
{
    public ImportView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}