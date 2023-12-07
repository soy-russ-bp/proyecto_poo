using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;

namespace ARJE.SignPractice.ViewModels
{
    public sealed class SelectionViewModel : ViewModelBase<NoDataModel, SelectionView>
    {
        public SelectionViewModel()
            : base(NoDataModel.None)
        {
        }
    }
}
