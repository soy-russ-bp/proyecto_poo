using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;

namespace ARJE.SignPractice.ViewModels
{
    public class ImportViewModel : ViewModelBase<ImportDataModel, ImportView>
    {
        public ImportViewModel(ImportDataModel dataModel)
            : base(dataModel)
        {
        }
    }
}
