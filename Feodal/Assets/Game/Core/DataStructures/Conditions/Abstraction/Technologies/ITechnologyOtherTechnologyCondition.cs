using Game.Core.DataStructures.Conditions.Abstraction.Base;
using Game.Core.DataStructures.Technologies;

namespace Game.Core.DataStructures.Conditions.Abstraction.Technologies
{
    public interface ITechnologyOtherTechnologyCondition : ITechnologyCondition
    {
        public Technology TechnologyDependent { get; set; }
    }
}