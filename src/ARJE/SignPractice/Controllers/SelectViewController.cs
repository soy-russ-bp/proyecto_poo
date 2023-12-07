using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;

namespace ARJE.SignPractice.Controllers
{
    public sealed class SelectViewController : ViewControllerBase<SelectionView, NoDataModel, SelectionViewModel>
    {
        public SelectViewController()
            : base(NoDataModel.None)
        {
        }
    }
}
