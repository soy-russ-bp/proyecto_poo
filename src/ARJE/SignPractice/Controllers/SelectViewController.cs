using System;
using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.MVC.Views;

namespace ARJE.SignPractice.Controllers
{
    public sealed class SelectViewController : ViewControllerBase<SelectView, SelectDataModel>
    {
        public SelectViewController(SelectDataModel model)
            : base(model)
        {
            this.View.OnBackBtnClick += this.OnBackBtnClick;
            this.View.OnConfigBtnClick += this.OnConfigBtnClick;
        }

        public event Action<IModelTrainingConfig<IModelConfig>>? OnConfigSelected;

        public override void Run(IViewDisplay viewDisplay)
        {
            this.Model.ConfigCollection.Update();
            this.View.SetBtnsDisplay(this.Model.ConfigCollection.Configs);
            this.View.SelectedConfigText = this.Model.SelectedConfig?.Title;
            base.Run(viewDisplay);
        }

        private void OnConfigBtnClick(IModelTrainingConfig<IModelConfig> config)
        {
            this.OnConfigSelected?.Invoke(config);
            this.View.SelectedConfigText = config.Title;
        }

        private void OnBackBtnClick()
        {
            HomeViewController.Instance.GoToHome();
        }
    }
}
