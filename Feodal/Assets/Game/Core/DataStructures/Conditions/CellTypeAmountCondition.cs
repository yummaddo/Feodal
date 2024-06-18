using Game.Core.Abstraction;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions
{
    public class CellTypeAmountCondition : AbstractCondition<CellTypeAmountCondition>
    {
        [field:SerializeField]
        public override string ConditionName { get; set; }
        public int amount;
        public CellContainer cellContainer;
        protected override CellTypeAmountCondition CompareTemplate() => this;
        public override bool Status()
        {
            return false;
        }
        internal override void Initialization()
        {
        }
        protected override string DataNamePattern => $"Condition:{cellContainer.Data.Initial.ExternalName}:{amount}";
    }
}