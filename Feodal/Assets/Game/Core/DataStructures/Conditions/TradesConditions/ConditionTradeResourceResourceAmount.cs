using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{

    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeResourceResourceAmount")]
    public class ConditionTradeResourceResourceAmount : AbstractDataStructure<ITradeResourceCondition>, ITradeResourceCondition
    {
        [field:SerializeField]public ResourceCounter Resources { get; set; }
        [field:SerializeField]public string ConditionName { get; set; }
        [field:SerializeField]public ResourceTrade ConnectedToDependency { get; set; }
        internal override string DataNamePattern => $"ConditionTrade_{ConnectedToDependency.TradeName}_{ConnectedToDependency.Value}_{Resources.resource.title}_{Resources.value}";
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
        [ContextMenu("RenameAsset")]
        public override void RenameAsset()
        {
            ConditionName = DataNamePattern;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(assetPath, DataNamePattern);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}