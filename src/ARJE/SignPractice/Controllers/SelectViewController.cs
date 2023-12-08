using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Controllers;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;

namespace ARJE.SignPractice.Controllers
{
    public sealed class SelectViewController : ViewControllerBase<SelectView, NoDataModel, SelectViewModel>
    {
        public SelectViewController()
            : base(NoDataModel.None, new SelectViewModel())
        {
        }
    }
}
