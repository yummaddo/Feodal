using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{
    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeBuildTechnology")]
    public class ConditionTradeBuildTechnology : AbstractDataStructure<ITradeBuildingTechnologyCondition>, ITradeBuildingTechnologyCondition
    {
        [field: SerializeField] public Technology Technology { get; set; }
        [field: SerializeField] public BuildingTrade BuildingTrade { get; set; }
        public ITechnology ConnectedToDependency { get; set; }
        [field:SerializeField]public string ConditionName { get; set; }
        internal override string DataNamePattern => $"ConditionTrade_{BuildingTrade.TradeName}_{Technology.Title}";
        protected override ITradeBuildingTechnologyCondition CompareTemplate(){ return this; }
        public bool Status()
        {
            throw new System.NotImplementedException();
        }
        public void Initialization()
        {
        }

    }
}