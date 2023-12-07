using System;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Views;

namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.Controllers
{
    public abstract class ViewControllerBase<TView, TDataModel, TViewModel> : IDisposable
        where TView : ViewBase
        where TDataModel : DataModelBase
        where TViewModel : ViewModelBase<TDataModel, TView>
    {
        public ViewControllerBase(TDataModel dataModel, TViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(dataModel);
            ArgumentNullException.ThrowIfNull(viewModel);

            this.DataModel = dataModel;
            this.ViewModel = viewModel;
        }

        public TDataModel DataModel { get; }

        public TViewModel ViewModel { get; }

        public void Run(IViewDisplay contentDisplay)
        {
            contentDisplay.SetContent(this.ViewModel, this);
        }

        public virtual void Dispose()
        {
            this.DataModel.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
