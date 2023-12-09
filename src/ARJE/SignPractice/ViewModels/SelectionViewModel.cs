using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using System.Collections.ObjectModel;

namespace ARJE.SignPractice.ViewModels
{

    internal sealed class SelectionViewModel : ViewModelBase
    {



        public string? TitleText { get; set; }
        public string? SampleCount { get; set; }
        public string? SampleLength { get; set; }
        public string? SamplesPerSecond { get; set; }
        public string? Labels { get; set; } = string.Empty;
        public void OnstartButton()
        {

            string[] labels = Labels.Split(',');
            var config = new ModelTrainingConfig<HandsModelConfig>(TitleText, ConvertInt(SampleCount), ConvertInt(SampleLength), ConvertInt(SamplesPerSecond), labels, new HandsModelConfig(1));
            MainWindowViewModel.Instance.GoToPractice(config);
        }

        public int ConvertInt(string text)
        {
            int.TryParse(text, out var value);
            return value;
        }
    }
}

