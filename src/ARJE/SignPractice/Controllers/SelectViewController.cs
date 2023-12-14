using System;
using ARJE.SignPractice.Models;
using ARJE.SignPractice.Views;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Avalonia.MVC.Controllers;

namespace ARJE.SignPractice.Controllers
{
    public sealed class SelectViewController : ViewControllerBase<SelectView, SelectDataModel>
    {
        public SelectViewController(SelectDataModel model)
            : base(model)
        {
            this.View.OnBackBtnClick += this.OnBackBtnClick;
            this.View.OnConfigBtnClick += this.OnConfigBtnClick;

            model.ConfigCollection.Update();
            this.View.SetBtnsDisplay(model.ConfigCollection.Configs);
        }

        public event Action<IModelTrainingConfig<IModelConfig>>? OnConfigSelected;

        private void OnConfigBtnClick(IModelTrainingConfig<IModelConfig> config)
        {
            this.OnConfigSelected?.Invoke(config);
        }

        private void OnBackBtnClick()
        {
            HomeViewController.Instance.GoToHome();
        }
    }
}
