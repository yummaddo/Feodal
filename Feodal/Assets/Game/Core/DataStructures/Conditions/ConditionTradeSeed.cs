using System.Reflection;
using Game.Core.DataStructures.Conditions.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Editor;
using Game.Core.DataStructures.Trades;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using UnityEditor;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{
    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeSeed")]
    public class ConditionTradeSeed : ScriptableObject,ICondition
    {
        [SerializeField] public SeedTrade connectedToDependency;
        public string ConditionName => connectedToDependency.TradeName;
        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp TechnologyTemp { get; set; }
        // internal override string DataNamePattern => $"ConditionTrade_Seed_{ConnectedToDependency.@into.title}";
        public void Initialization()
        {
        }
        public bool Status()
        {
            return false;
        }
        private string GetName()
        {
            return ConditionName;
        }
        internal void RenameAsset()
        {
            string assetPath = AssetDatabase.GetAssetPath(this);
            Debug.Log(GetName());
            AssetDatabase.RenameAsset(assetPath, GetName());
            AssetDatabase.SaveAssets();
        }
    }
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ConditionTradeSeed))]
    public class ConditionTradeSeedAmountEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var targetTrade = (ConditionTradeSeed)target;
            if (GUILayout.Button("Validate"))
            {
                targetTrade.RenameAsset();
            }
        }
    }
}