using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;

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
