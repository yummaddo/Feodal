using Game.Core.DataStructures.Trades;

namespace Game.Core.DataStructures.Conditions.Abstraction.Base
{
    public interface IBuildingCondition : ICondition
    {
        public BuildingTrade ConnectedToDependency { get; set; }
    }
}