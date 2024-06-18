using Game.Core.Abstraction;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Core.DataStructures.Conditions
{
    public class ResourceAmountCondition : AbstractCondition<ResourceAmountCondition>
    {
        [field:SerializeField]
        public override string ConditionName { get; set; }
        public Resource resource;
        public int amount;
        internal override void Initialization()
        {
        }
        public override bool Status()
        {
            return true;
        }
        protected override ResourceAmountCondition CompareTemplate() => this;
        protected override string DataNamePattern => $"Condition:{resource.Data.Title}:{amount}";
    }
}