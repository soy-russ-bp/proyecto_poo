using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ARJE.SignPractice.Views;

public partial class SelectionView : UserControl
{
    public SelectionView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}