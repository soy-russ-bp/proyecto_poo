using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;

namespace ARJE.SignPractice.ViewModels
{
    public class HomeViewModel : ViewModelBase<NoDataModel, HomeView>
    {
        public HomeViewModel()
            : base(NoDataModel.None)
        {
        }
    }
}
