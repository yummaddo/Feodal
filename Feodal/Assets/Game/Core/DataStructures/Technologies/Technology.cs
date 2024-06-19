using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction;
using Game.Core.DataStructures.Conditions.TechnologyConditions;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Technologies.Base;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Technologies
{
    [CreateAssetMenu(menuName = "Technology/Technology")]
    public class Technology : AbstractDataStructure<ITechnologyStore>, ITechnologyStore
    {
        public int stage;
        public bool CurrentStatus { get; set; }
        public bool Status() { return false; }
        [field:SerializeField]public string Title { get; set; }
        [field:SerializeField]public TechnologyTrade Trade { get; set; }
        protected override ITechnologyStore CompareTemplate()
        {
            return this;
        }
        
        internal override string DataNamePattern => $"Technology_stage[{stage}][{Title}]";
    }
}