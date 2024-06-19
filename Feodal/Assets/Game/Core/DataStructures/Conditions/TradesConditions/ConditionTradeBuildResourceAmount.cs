using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TradesConditions
{
    [System.Serializable]
    public class ConditionTradeToBuildResourceAmount : ITradeBuildingCondition
    {
        [field:SerializeField]public string ConditionName { get; set; }
        [field:SerializeField]public BuildingTrade ConnectedToDependency { get; set; }
        [field:SerializeField]public ResourceCounter ResourceCounter { get; set; }
        public bool Status() { return true;}
        public void Initialization()
        {
        }
    }
}