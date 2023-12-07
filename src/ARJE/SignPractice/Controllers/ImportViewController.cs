using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;

namespace ARJE.SignPractice.Controllers
{
    public sealed class ImportViewController : ViewControllerBase<ImportView, NoDataModel, ImportViewModel>
    {
        public ImportViewController()
            : base(NoDataModel.None)
        {
        }
    }
}
