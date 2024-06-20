using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{
    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeBuildTechnology")]
    public class ConditionTradeBuildTechnology : AbstractDataStructure<ITradeBuildingTechnologyCondition>, ITradeBuildingTechnologyCondition
    {
        [field: SerializeField] public Technology Technology { get; set; }
        [field: SerializeField] public BuildingTrade BuildingTrade { get; set; }
        public ITechnology ConnectedToDependency { get; set; }
        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp TechnologyTemp { get; set; }
        public string ConditionName => BuildingTrade.TradeName;
        internal override string DataNamePattern => $"ConditionTrade_{BuildingTrade.TradeName}_{Technology.Title}";

        protected override ITradeBuildingTechnologyCondition CompareTemplate()
        {
            return this;
        }
        
        public bool Status()
        {
            throw new System.NotImplementedException();
        }
        public void Initialization()
        {
        }

    }
}