using System;

namespace ARJE.SignTrainer.App.MVC.Base
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
