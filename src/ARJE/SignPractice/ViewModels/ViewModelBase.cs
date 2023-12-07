using System;
using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.System;
using ReactiveUI;

namespace ARJE.SignPractice.ViewModels
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

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        void IViewModelInit.Init(ViewBase view)
        {
            this.view = (TView)view;
        }
    }
}