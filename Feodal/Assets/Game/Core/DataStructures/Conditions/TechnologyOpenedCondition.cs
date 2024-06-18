using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Technology;
using UnityEngine;

namespace Game.Core.DataStructures.Conditions
{
    public class TechnologyOpenedCondition : AbstractCondition<TechnologyOpenedCondition>
    {
        [field:SerializeField]
        public override string ConditionName { get; set; }
        public List<AbstractTechnology<ITechnology>> technology;
        protected override TechnologyOpenedCondition CompareTemplate() => this;
        internal override void Initialization()
        {
        }
        public override bool Status()
        {
            return true;
        }
        protected override string DataNamePattern => $"Condition: TechnologyOpenedCondition {ConditionName}";
    }
}