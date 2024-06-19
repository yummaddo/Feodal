using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{
    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeResourceTechnology")]
    public class ConditionTradeResourceTechnology :AbstractDataStructure<ITradeResourceTechnologyCondition>, ITradeResourceTechnologyCondition
    {
        public Technology technology;
        [field:SerializeField]public ResourceTrade ResourceTrade { get; set; }
        [field:SerializeField]public string ConditionName { get; set; }
        public ITechnology ConnectedToDependency { get; set; }
        internal override string DataNamePattern => $"ConditionTrade_{ResourceTrade.TradeName}_{technology.Title}";
        protected override ITradeResourceTechnologyCondition CompareTemplate()
        {
            ConnectedToDependency = technology.Data;
            return this;
        }
        public bool Status()
        {
            throw new System.NotImplementedException();
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