using System;
using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.System;

namespace ARJE.SignPractice.Controllers
{
    public abstract class ViewControllerBase<TView, TDataModel, TViewModel>
        where TView : ViewBase
        where TDataModel : DataModelBase
        where TViewModel : ViewModelBase<TDataModel, TView>
    {
        public ViewControllerBase(TDataModel dataModel)
        {
            ArgumentNullException.ThrowIfNull(dataModel);

            this.DataModel = dataModel;
            this.ViewModel = CreateViewModel(dataModel);
        }

        public TDataModel DataModel { get; }

        public TViewModel ViewModel { get; }

        public void Run(IViewDisplay contentDisplay)
        {
            contentDisplay.SetContent(this.ViewModel);
        }

        private static TViewModel CreateViewModel(TDataModel dataModel)
        {
            return (dataModel is NoDataModel)
                ? Activator.CreateInstance<TViewModel>()
                : ActivatorUtils.CreateInstance<TViewModel>(dataModel);
        }
    }
}
