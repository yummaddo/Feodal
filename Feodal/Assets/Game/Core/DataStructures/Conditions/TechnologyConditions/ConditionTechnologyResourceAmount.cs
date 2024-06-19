
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction.Technologies;
using Game.Core.DataStructures.Editor;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using UnityEditor;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TechnologyConditions
{
    [System.Serializable]
    public class ConditionTechnologyResourceAmount : ITechnologyResourceAmountCondition
    {
        public Technology technology;
        [field: SerializeField]public string ConditionName { get; set; }
        [field: SerializeField]public ITechnology ConnectedToDependency { get; set; }
        [field: SerializeField]public ResourceCounter ResourceCounter { get; set; }
        public bool Status()
        {
            return true;
        }

        public void Initialization()
        {
        }
    }
}