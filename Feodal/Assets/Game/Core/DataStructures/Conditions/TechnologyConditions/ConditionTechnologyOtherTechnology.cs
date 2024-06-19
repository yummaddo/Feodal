
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction.Technologies;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Abstraction;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions.TechnologyConditions
{
    [System.Serializable]
    public class ConditionTechnologyOtherTechnology :  ITechnologyOtherTechnologyCondition
    {
        [field: SerializeField]public string ConditionName { get; set; }
        [field: SerializeField]public Technology Technology { get; set; }
        [field: SerializeField]public Technology TechnologyDependent { get; set; }
        public ITechnology ConnectedToDependency { get; set; }
        public bool Status()
        {
            throw new System.NotImplementedException();
        }
        public void Initialization()
        {
        }
    }
}