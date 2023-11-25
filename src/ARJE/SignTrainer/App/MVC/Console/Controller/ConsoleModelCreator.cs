using System;
using System.Collections.Generic;
using System.Linq;
using ARJE.Utils.AI.Configuration;
using ARJE.Utils.AI.Solutions.Hands;
using ARJE.Utils.Spectre.Console.Extensions;
using ARJE.Utils.Text;
using Spectre.Console;

namespace ARJE.SignTrainer.App.MVC.Console.Controller
{
    public class ConsoleModelCreator
    {
        public ConsoleModelCreator(ConsoleTrainerController controller)
        {
            ArgumentNullException.ThrowIfNull(controller);

            this.Controller = controller;
        }

        public enum HandsModelConfigOption
        {
            One = 1,
            Both = 2,
        }

        public static SelectionPrompt<HandsModelConfigOption> HandsModelConfigPrompt { get; } = CreateHandsModelConfigPrompt();

        public ConsoleTrainerController Controller { get; }

        public ModelTrainingConfig<HandsModelConfig> AskForTrainingConfig()
        {
            string title = this.AskForTitle();
            int sampleCount = this.AskForSampleSize();
            int samplesPerSecond = this.AskForSamplesPerSecond();
            HandsModelConfig handsModelConfig = this.AskForHandsModelConfig();
            List<string> labels = this.AskForLabels();
            return new ModelTrainingConfig<HandsModelConfig>(title, sampleCount, samplesPerSecond, labels, handsModelConfig).Validate();
        }

        private static SelectionPrompt<HandsModelConfigOption> CreateHandsModelConfigPrompt()
        {
            var prompt = new SelectionPrompt<HandsModelConfigOption>()
            {
                Title = "Hands model config:",
                WrapAround = true,
            };

            prompt.AddEnumChoices();

            return prompt;
        }

        private string AskForTitle()
        {
            return this.Controller.View.TextPrompt<string>("Title:");
        }

        private int AskForSampleSize()
        {
            return this.Controller.View.TextPrompt("Sample count:", 30, input => input is > 0);
        }

        private int AskForSamplesPerSecond()
        {
            return this.Controller.View.TextPrompt("Samples per second (video fps):", 20, input => input is >= 10 and <= 60);
        }

        private HandsModelConfig AskForHandsModelConfig()
        {
            HandsModelConfigOption modelConfigOption = this.Controller.View.Prompt(HandsModelConfigPrompt);
            this.Controller.View.DisplayMsg($"{HandsModelConfigPrompt.Title} {modelConfigOption}");
            return new HandsModelConfig(MaxNumHands: (int)modelConfigOption);
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
