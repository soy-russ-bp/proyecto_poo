using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;

namespace ARJE.SignPractice.Controllers
{
    public sealed class HomeViewController : ViewControllerBase<HomeView, NoDataModel, HomeViewModel>
    {
        public HomeViewController()
            : base(NoDataModel.None)
        {
        }
    }
}
