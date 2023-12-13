using System;
using ARJE.Utils.Avalonia.MVC.Models;
using ARJE.Utils.Avalonia.MVC.Views;

namespace ARJE.Utils.Avalonia.MVC.Controllers
{
    public abstract class ViewControllerBase<TView, TModel> : IDisposable
        where TView : ViewBase, new()
        where TModel : IDataModel
    {
        public ViewControllerBase(TModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            this.Model = model;
            this.View = new TView();
        }

        public TModel Model { get; }

        public TView View { get; }

        public void Run(IViewDisplay viewDisplay)
        {
            viewDisplay.SetContent(this.View, this);
        }

        public virtual void Dispose()
        {
            this.Model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
