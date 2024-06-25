using Game.DataStructures.Abstraction;
using Game.DataStructures.Technologies.Abstraction;
using Game.DataStructures.Trades;
using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;
using UnityEngine;

namespace Game.DataStructures.Technologies
{
    [CreateAssetMenu(menuName = "Technology/TradeBuild")]
    public class TradeBuildTechnology : AbstractDataStructure<ITradeBuildTechnology>, ITradeBuildTechnology
    {
        public string title;

        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp Temp { get; set; }
        public TechnologyRepository Repository { get; set; }
        public bool CurrentStatus { get; set; }
        public string Title { get; set; }
        [field: SerializeField] public BuildingTrade IntoBuild { get; set; }
        
        internal override string DataNamePattern => $"TradeBuild_{IntoBuild.ToString()}";
        public bool TechnologyStatusTrade()
        {
            if (Status() && ResourceTemp != null)
            {
                return true;
            }
            return false;
        }
        public bool Status()
        {
            if (Temp != null)
                return Temp.TechnologyStores[this.Title].CurrentStatus;
            return false;
        }
        protected override ITradeBuildTechnology CompareTemplate()
        {
            Title = title;
            if (Temp == null) { return this; }
            return Temp.TechnologyBuild[Title];
        }
    }
}