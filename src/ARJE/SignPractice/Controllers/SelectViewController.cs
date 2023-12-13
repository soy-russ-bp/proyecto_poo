using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.MVC.Controllers;
using ARJE.Utils.Avalonia.MVC.Models;

namespace ARJE.SignPractice.Controllers
{
    public sealed class SelectViewController : ViewControllerBase<SelectView, NoDataModel>
    {
        public SelectViewController()
            : base(NoDataModel.None)
        {
            this.View.OnBackBtnClick += this.OnBackBtnClick;
        }

        private void OnBackBtnClick()
        {
            HomeViewController.Instance.GoToHome();
        }
    }
}
