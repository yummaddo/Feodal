using System.Collections.Generic;

namespace Game.Core.Abstraction
{
    public interface ITechnology
    {
        public List<ICondition> ConditionsToResearch { get; set; }
        public string TechnologyName { get;}
    }
}