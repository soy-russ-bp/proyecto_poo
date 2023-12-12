using System;
using ARJE.Utils.Avalonia.MVC.Views;
using Avalonia.Interactivity;

namespace ARJE.SignPractice.Views;

public sealed partial class SelectView : ViewBase
{
    public event Action? OnBackBtnClick;

    protected override void OnInitializeComponent() => this.InitializeComponent();

    private void BackBtnClickHandler(object sender, RoutedEventArgs e)
    {
        this.OnBackBtnClick?.Invoke();
    }
}
