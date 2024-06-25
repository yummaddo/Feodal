using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;

namespace Game.DataStructures.Conditions.Abstraction
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