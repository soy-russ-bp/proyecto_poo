using System;
using System.Collections.Generic;
using System.Linq;
using ARJE.Utils.Text;

namespace ARJE.SignTrainer.App.MVC.Console.Controller
{
    public class ConsoleModelCreator
    {
        public ConsoleModelCreator(ConsoleTrainerController controller)
        {
            ArgumentNullException.ThrowIfNull(controller);

            this.Controller = controller;
        }

        public ConsoleTrainerController Controller { get; }

        public void AskForModelInfo()
        {
            string title = this.AskForTitle();
            int sampleSize = this.AskForSampleSize();
            int samplesPerSecond = this.AskForSamplesPerSecond();
            int handCount = this.AskForHandCount();
            List<string> labels = this.AskForLabels();
        }

        private string AskForTitle()
        {
            return this.Controller.View.TextPrompt<string>("Title:").Trim();
        }

        private int AskForSampleSize()
        {
            return this.Controller.View.TextPrompt("Sample size:", 30, input => input is > 0);
        }

        private int AskForSamplesPerSecond()
        {
            return this.Controller.View.TextPrompt("Samples per second (video fps):", 20, input => input is >= 10 and <= 60);
        }

        private int AskForHandCount()
        {
            return this.Controller.View.TextPrompt("Hand count (1-2):", 2, input => input is 1 or 2);
        }

        private List<string> AskForLabels()
        {
            int labelCount = this.Controller.View.TextPrompt<int>("Label count:", input => input is > 0);
            var labels = new List<string>(labelCount);
            do
            {
                this.Controller.View.Clear();
                this.Controller.View.DisplayMsg($"Labels: {ArgsUtils.Join(labels)}");
                string label = this.Controller.View.TextPrompt<string>($"Label ({labels.Count + 1} / {labelCount}):");
                if (!labels.Contains(label, StringComparer.OrdinalIgnoreCase))
                {
                    labels.Add(label);
                }
                else
                {
                    this.Controller.View.DisplayErrorMsg($"\"{label}\" already exists.");
                }
            }
            while (labels.Count < labelCount);

            return labels;
        }
    }
}
