using System;
using ARJE.Utils.Avalonia.MVC.Views;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace ARJE.SignPractice.Views;

public sealed partial class PracticeView : ViewBase
{
    public event Action? OnBackBtnClick;

    public string? SignText
    {
        get => this.signTextBox.Text;
        set => this.signTextBox.Text = value;
    }

    private IImage? FrameSource
    {
        get => this.frameImg.Source;
        set => this.frameImg.Source = value;
    }

    public void SetFrameAndDisposeLast(IImage frame)
    {
        (this.FrameSource as IDisposable)?.Dispose();
        this.FrameSource = frame;
    }

    protected override void OnInitializeComponent() => this.InitializeComponent();

    private void BackBtnClickHandler(object sender, RoutedEventArgs e)
    {
        this.OnBackBtnClick?.Invoke();
    }
}
