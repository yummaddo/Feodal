using Game.Core.DataStructures.Conditions.Abstraction.Base;

namespace Game.Core.DataStructures.Conditions.Abstraction.Technologies
{
    public interface ITechnologyResourceAmountCondition : ITechnologyCondition
    {
        public ResourceCounter ResourceCounter { get; set; }
    }
}