using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ARJE.SignPractice.Controllers
{
    public class PracticeController
    {
        private string targetSign;

        private int matchCount;

        public PracticeController(PracticeViewController controller)
        {
            this.Controller = controller;
            this.TargetSign = this.GetRandomLabel();
        }

        public PracticeViewController Controller { get; }

        public string TargetSign
        {
            get => this.targetSign;

            [MemberNotNull(nameof(targetSign))]
            private set => this.Controller.View.SignText = this.targetSign = value;
        }

        public int MatchCount
        {
            get => this.matchCount;
            private set => this.Controller.View.SignProgress = this.matchCount = Math.Max(0, value);
        }

        public void Update(string? detectedSign)
        {
            if (detectedSign != this.TargetSign)
            {
                this.MatchCount--;
                return;
            }

            this.MatchCount++;
            if (this.MatchCount < 10)
            {
                return;
            }

            this.MatchCount = 0;
            this.TargetSign = this.GetRandomLabel();
        }

        private string GetRandomLabel()
        {
            IReadOnlyList<string> labels = this.Controller.Model.DetectionModel.TrainingConfig.Labels;
            int randLabelI = Random.Shared.Next(labels.Count);
            string label = labels[randLabelI];
            if (this.TargetSign == null || label != this.TargetSign)
            {
                return label;
            }

            return this.GetRandomLabel();
        }
    }
}
