using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ARJE.SignPractice.Views;

public partial class PracticeView : UserControl
{
    public PracticeView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}