using Game.DataStructures.Technologies.Abstraction;

namespace Game.DataStructures.Conditions.Abstraction.Base
{
    public interface ITechnologyCondition : ICondition
    {
        public ITechnology ConnectedToDependency { get; set; }

    }
}