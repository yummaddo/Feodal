using Game.DataStructures.Trades;
using Game.RepositoryEngine.ResourcesRepository;

namespace Game.DataStructures.Technologies.Abstraction
{
    public interface ITradeBuildTechnology : ITechnology
    {
        public BuildingTrade IntoBuild { get; set; }
        public ResourceTemp ResourceTemp { get; set; }
        public bool TechnologyStatusTrade();
    }
}