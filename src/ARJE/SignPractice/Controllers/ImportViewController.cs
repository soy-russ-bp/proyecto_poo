using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Controllers;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;

namespace ARJE.SignPractice.Controllers
{
    public sealed class ImportViewController : ViewControllerBase<ImportView, NoDataModel, ImportViewModel>
    {
        public ImportViewController()
            : base(NoDataModel.None, new ImportViewModel())
        {
        }
    }
}
