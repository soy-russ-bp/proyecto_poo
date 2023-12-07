using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;

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
