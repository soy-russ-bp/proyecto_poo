using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;

namespace ARJE.SignPractice.ViewModels
{
    public class ImportViewModel : ViewModelBase<NoDataModel, ImportView>
    {
        public ImportViewModel()
            : base(NoDataModel.None)
        {
        }
    }
}
