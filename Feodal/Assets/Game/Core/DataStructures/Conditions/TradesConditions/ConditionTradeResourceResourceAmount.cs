using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Trades;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{

    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeResourceResourceAmount")]
    public class ConditionTradeResourceResourceAmount : AbstractDataStructure<ITradeResourceCondition>, ITradeResourceCondition
    {
        [field:SerializeField]public ResourceCounter Resources { get; set; }
        [field:SerializeField]public ResourceTrade ConnectedToDependency { get; set; }
        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp TechnologyTemp { get; set; }
        
        public string ConditionName => ConnectedToDependency.TradeName;

        protected override ITradeResourceCondition CompareTemplate()
        {
            return this;
        }
        public bool Status()
        {
            return true;
        }
        
        public void Initialization()
        {
        }
        internal override string DataNamePattern => $"ConditionTrade_{ConnectedToDependency.TradeName}_{ConnectedToDependency.Value}_{Resources.resource.title}_{Resources.value}";

        [ContextMenu("RenameAsset")]
        public override void RenameAsset()
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(assetPath, DataNamePattern);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}