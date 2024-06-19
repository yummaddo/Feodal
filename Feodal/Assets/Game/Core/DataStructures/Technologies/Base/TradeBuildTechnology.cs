using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Technologies.Base
{
    [CreateAssetMenu(menuName = "Technology/TradeBuild")]
    public class TradeBuildTechnology : AbstractDataStructure<ITradeBuildTechnology>, ITradeBuildTechnology
    {
        public bool CurrentStatus { get; set; }
        [field: SerializeField] public BuildingTrade IntoBuild { get; set; }
        [field: SerializeField] public string Title { get; set; }
        internal override string DataNamePattern => $"TradeBuild_{IntoBuild.ToString()}";
        protected override ITradeBuildTechnology CompareTemplate()
        {
            return this;
        }
        public bool Status()
        {
            throw new System.NotImplementedException();
        }

    }
}