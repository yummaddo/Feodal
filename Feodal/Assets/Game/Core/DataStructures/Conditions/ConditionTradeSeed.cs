using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Trades;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{
    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeSeed")]
    public class ConditionTradeSeed : AbstractDataStructure<ITradeSeedCondition>, ITradeSeedCondition
    {
        [field:SerializeField]public SeedTrade ConnectedToDependency { get; set; }
        public string ConditionName => ConnectedToDependency.TradeName;
        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp TechnologyTemp { get; set; }
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