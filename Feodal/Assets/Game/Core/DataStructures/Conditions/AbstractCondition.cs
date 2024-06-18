using Game.Core.Abstraction;
using PlasticGui.Help;

namespace Game.Core.DataStructures.Conditions
{
    [System.Serializable]
    public abstract class AbstractCondition<TConditionType> : AbstractDataStructure<TConditionType>, ICondition
        where TConditionType : ICondition
    {
        public abstract string ConditionName { get; set; }
        internal abstract void Initialization();
        public abstract bool Status();
        public string GetConditionName() => DataNamePattern;
        public TConditionType GetCondition() => CompareTemplate();
    }
}