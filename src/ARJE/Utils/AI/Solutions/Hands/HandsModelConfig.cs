using ARJE.Utils.AI.Configuration;

namespace ARJE.Utils.AI.Solutions.Hands
{
    public sealed record HandsModelConfig(int MaxNumHands = 2) : IModelConfig
    {
    }
}
