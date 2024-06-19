using Game.Core.DataStructures.Technologies.Abstraction;

namespace Game.Core.DataStructures.Conditions.Abstraction.Base
{
    public interface ITechnologyCondition : ICondition
    {
        public ITechnology ConnectedToDependency { get; set; }

    }
}