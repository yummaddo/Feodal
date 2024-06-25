using Game.DataStructures.Trades;

namespace Game.DataStructures.Conditions.Abstraction.Base
{
    public interface IBuildingCondition : ICondition
    {
        public BuildingTrade ConnectedToDependency { get; set; }
    }
}