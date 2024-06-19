using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{
    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeSeed")]
    public class ConditionTradeSeed : AbstractDataStructure<ITradeSeedCondition>, ITradeSeedCondition
    {
        [field:SerializeField]public SeedTrade ConnectedToDependency { get; set; }
        [field:SerializeField]public string ConditionName { get; set; }
        internal override string DataNamePattern => $"ConditionTrade_Seed_{ConnectedToDependency.@into.title}";
        public void Initialization()
        {
        }
        protected override ITradeSeedCondition CompareTemplate()
        {
            return this;
        }

        public bool Status()
        {
            return false;
        }
    }
}