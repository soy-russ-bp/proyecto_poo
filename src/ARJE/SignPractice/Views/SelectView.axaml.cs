using System;
using ARJE.Utils.Avalonia.MVC.Views;
using Avalonia.Controls;
using Avalonia.Interactivity;


namespace ARJE.SignPractice.Views;

public sealed partial class SelectView : ViewBase
{

    public event Action? OnBackBtnClick;

    public event Action? PointerEnter;

    private Panel hiddenPanel;

    protected override void OnInitializeComponent() {
        this.InitializeComponent();
         // Obtén una referencia al panel oculto por su nombre (definido en el archivo XAML)
        //hiddenPanel = this.FindControl<Panel>("HiddenPanel");
        // Inicialmente, oculta el panel
        //hiddenPanel.IsVisible = true;
    } 

    private void BackBtnClickHandler(object sender, RoutedEventArgs e)
    {
        this.OnBackBtnClick?.Invoke();
    }

    private void PointerEnterHandler(object sender, RoutedEventArgs e){
        this.PointerEnter?.Invoke();
        //hiddenPanel.IsVisible = true;
    }
}
