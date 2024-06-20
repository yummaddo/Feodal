using Game.Core.DataStructures.Trades;
using Game.Services.Storage.ResourcesRepository;

namespace Game.Core.DataStructures.Technologies.Abstraction
{
    public interface ITradeBuildTechnology : ITechnology
    {
        public BuildingTrade IntoBuild { get; set; }
        public ResourceTemp ResourceTemp { get; set; }
        public bool TechnologyStatusTrade();
    }
}