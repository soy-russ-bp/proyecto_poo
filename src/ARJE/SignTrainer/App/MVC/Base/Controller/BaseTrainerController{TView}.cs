using System;
using ARJE.SignTrainer.App.MVC.Base.Model;
using ARJE.SignTrainer.App.MVC.Base.View;

namespace ARJE.SignTrainer.App.MVC.Base.Controller
{
    public abstract class BaseTrainerController<TView>
        where TView : BaseTrainerView
    {
        public BaseTrainerController(TrainerModel model, TView view)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(view);

            this.Model = model;
            this.View = view;
        }

        public TrainerModel Model { get; }

        public TView View { get; }

        public abstract void Run();
    }
}
