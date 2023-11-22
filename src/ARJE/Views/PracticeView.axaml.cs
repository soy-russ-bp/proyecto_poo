using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Arje.Views;

public partial class PracticeView : UserControl
{
    public PracticeView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}