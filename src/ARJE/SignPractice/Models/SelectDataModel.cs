using ARJE.Shared.Models;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.Avalonia.MVC.Models;

namespace ARJE.SignPractice.Models
{
    public sealed record SelectDataModel(
        OnDiskModelTrainingConfigCollection ConfigCollection,
        IModelTrainingConfig<IModelConfig>? SelectedConfig)
        : DataModelBase
    {
    }
}
