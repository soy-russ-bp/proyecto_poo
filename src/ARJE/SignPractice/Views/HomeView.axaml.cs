using System;
using ARJE.Utils.Avalonia.MVC.Views;
using Avalonia.Interactivity;

namespace ARJE.SignPractice.Views;

public sealed partial class HomeView : ViewBase
{
    public event Action? OnPracticeBtnClick;

    public event Action? OnSelectBtnClick;

    public event Action? OnImportBtnClick;

    protected override void OnInitializeComponent() => this.InitializeComponent();

    private void PracticeBtnClickHandler(object sender, RoutedEventArgs e)
    {
        this.OnPracticeBtnClick?.Invoke();
    }

    private void SelectBtnClickHandler(object sender, RoutedEventArgs e)
    {
        this.OnSelectBtnClick?.Invoke();
    }

    private void ImportBtnClickHandler(object sender, RoutedEventArgs e)
    {
        this.OnImportBtnClick?.Invoke();
    }
}
