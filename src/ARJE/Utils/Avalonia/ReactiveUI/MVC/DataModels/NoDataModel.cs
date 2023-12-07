namespace ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels
{
    public sealed record NoDataModel : DataModelBase
    {
        private NoDataModel()
        {
        }

        public static NoDataModel None { get; } = new();
    }
}
