using System;
using System.Collections.Generic;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Avalonia.MVC.Views;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ARJE.SignPractice.Views;

public sealed partial class SelectView : ViewBase
{
    public event Action? OnBackBtnClick;

    public event Action<IModelTrainingConfig<IModelConfig>>? OnConfigBtnClick;

    public void SetBtnsDisplay(IReadOnlyCollection<IModelTrainingConfig<IModelConfig>> configs)
    {
        this.btnsPanel.Children.Clear();

        foreach (IModelTrainingConfig<IModelConfig> config in configs)
        {
            Button button = this.CreateBtnForConfig(config);
            this.btnsPanel.Children.Add(button);
        }
    }

    protected override void OnInitializeComponent() => this.InitializeComponent();

    private Button CreateBtnForConfig(IModelTrainingConfig<IModelConfig> config)
    {
        var button = new Button
        {
            Content = new TextBlock()
            {
                Text = config.Title,
            },
        };

        button.Click += (_, _) => this.ConfigBtnClickHandler(config);
        return button;
    }

    private void ConfigBtnClickHandler(IModelTrainingConfig<IModelConfig> selectedConfig)
    {
        this.OnConfigBtnClick?.Invoke(selectedConfig);
    }

    private void BackBtnClickHandler(object sender, RoutedEventArgs e)
    {
        this.OnBackBtnClick?.Invoke();
    }
}
