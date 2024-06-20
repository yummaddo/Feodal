using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;

namespace Game.Core.DataStructures.Conditions.Abstraction
{
    public interface ICondition
    {
        public bool Status();
        public string ConditionName { get; }
        public void Initialization();
        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp TechnologyTemp { get; set; }
    }
}