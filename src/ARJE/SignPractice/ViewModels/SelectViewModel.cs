using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;

namespace ARJE.SignPractice.ViewModels
{
    public sealed class SelectViewModel : ViewModelBase<NoDataModel, SelectView>
    {
        public SelectViewModel()
            : base(NoDataModel.None)
        {
        }
    }
}
