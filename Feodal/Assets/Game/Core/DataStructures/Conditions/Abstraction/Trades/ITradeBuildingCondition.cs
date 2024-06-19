using Game.Core.DataStructures.Conditions.Abstraction.Base;

namespace Game.Core.DataStructures.Conditions.Abstraction.Trades
{
    public interface ITradeBuildingCondition : IBuildingCondition
    {
        public ResourceCounter ResourceCounter { get; set; }
    }
}