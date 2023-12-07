using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Views;
using ARJE.Utils.System;
using ReactiveUI;

namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.ViewModels
{
    public abstract class ViewModelBase<TDataModel, TView> : ReactiveObject, IViewModelInit, IViewModel<TView>
        where TDataModel : DataModelBase
        where TView : ViewBase
    {
        private TView? view;

        protected ViewModelBase(TDataModel dataModel)
        {
            this.DataModel = dataModel;
        }

        public TDataModel DataModel { get; }

        public TView View => ThrowUtils.ReturnOnlyIfNotNull(this.view);

        void IViewModelInit.Init(ViewBase view)
        {
            this.view = (TView)view;
        }
    }
}