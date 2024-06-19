using Game.Core.DataStructures.Trades;

namespace Game.Core.DataStructures.Technologies.Abstraction
{
    public interface ITradeBuildTechnology : ITechnology
    {
        public BuildingTrade IntoBuild { get; set; }
    }
}