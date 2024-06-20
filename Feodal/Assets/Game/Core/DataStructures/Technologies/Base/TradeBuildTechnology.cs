using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Core.DataStructures.Technologies.Base
{
    [CreateAssetMenu(menuName = "Technology/TradeBuild")]
    public class TradeBuildTechnology : AbstractDataStructure<ITradeBuildTechnology>, ITradeBuildTechnology
    {
        public ResourceTemp ResourceTemp { get; set; }
        public TechnologyTemp Temp { get; set; }
        public TechnologyRepository Repository { get; set; }
        public bool CurrentStatus { get; set; }
        [field: SerializeField] public string Title { get; set; }
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
            if (Temp == null)
            {
                return this;
            }
            return Temp.TechnologyBuild[Title];
        }
    }
}