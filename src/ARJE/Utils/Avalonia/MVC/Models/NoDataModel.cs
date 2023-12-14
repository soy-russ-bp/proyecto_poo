namespace ARJE.Utils.Avalonia.MVC.Models
{
    public sealed record NoDataModel : DataModelBase
    {
        private NoDataModel()
        {
        }

        public static NoDataModel None { get; } = new();
    }
}
