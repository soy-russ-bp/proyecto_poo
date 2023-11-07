using System;
using ARJE.SignTrainer.App.Model;
using ARJE.SignTrainer.App.View;

namespace ARJE.SignTrainer.App.Controller
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

        protected TrainerModel Model { get; }

        protected TView View { get; }

        public abstract void Run();
    }
}
