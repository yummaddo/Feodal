using Game.DataStructures.Conditions.Abstraction;
using Game.DataStructures.Trades;
using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Game.DataStructures.Conditions
{
    [CreateAssetMenu(menuName = "Trade/Condition/ConditionTradeResourceResourceAmount")]
    public class ConditionTradeResourceAmount : ScriptableObject, ICondition
    {
        [SerializeField] public ResourceTrade connectedToDependency;
        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp TechnologyTemp { get; set; }
        public string ConditionName => connectedToDependency.TradeName;

        private string GetName()
        {
            return ConditionName +$"{connectedToDependency.value}_"+ GetDataResource();
        }
        public bool Status()
        {
            return true;
        }
        private string GetDataResource()
        {
            string result = "";
            foreach (var aResourceCounter in connectedToDependency.resourceAmountCondition) result += aResourceCounter.ToString();
            return result;
        }
        
        public void Initialization()
        {
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
    namespace Game.Core.DataStructures.Editor
    {
        [CanEditMultipleObjects]
        [CustomEditor(typeof(ConditionTradeResourceAmount))]
        public class ConditionTradeResourceAmountEditor : UnityEditor.Editor 
        {
            private void OnEnable()
            {
            }
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                var targetTrade = (ConditionTradeResourceAmount)target;
                if (GUILayout.Button("Validate"))
                {
                    targetTrade.RenameAsset();
                }
            }

        }

    }
#endif
}