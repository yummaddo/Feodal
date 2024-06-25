using Game.DataStructures.Conditions.Abstraction;
using Game.DataStructures.Trades;
using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;

#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;

namespace Game.DataStructures.Conditions
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
#if UNITY_EDITOR
        internal void RenameAsset()
        {
            string assetPath = AssetDatabase.GetAssetPath(this);
            Debug.Log(GetName());
            AssetDatabase.RenameAsset(assetPath, GetName());
            AssetDatabase.SaveAssets();
        }
#endif

    }
#if UNITY_EDITOR
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
#endif
}