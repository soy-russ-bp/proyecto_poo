using ARJE.Utils.AI.Configuration;
using Newtonsoft.Json;

namespace ARJE.Utils.AI.Solutions.Hands
{
    public sealed record HandsModelConfig(int MaxNumHands = 2) : IModelConfig
    {
        public int MaxNumHands { get; set; } = MaxNumHands;

        [JsonIgnore]
        public int LandmarkCount => 21;

        public string InfoPrint()
        {
            return "Max num hands: " + this.MaxNumHands;
        }

        public void CopyTo(HandsModelConfig target)
        {
            target.MaxNumHands = this.MaxNumHands;
        }
    }
}
