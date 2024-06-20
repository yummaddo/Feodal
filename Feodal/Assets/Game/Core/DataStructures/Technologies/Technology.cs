using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Core.DataStructures.Technologies
{
    [CreateAssetMenu(menuName = "Technology/Technology")]
    public class Technology : AbstractDataStructure<ITechnologyStore>, ITechnologyStore
    {
        public int stage;
        [field:SerializeField] public bool CurrentStatus { get; set; }
        [field:SerializeField] public string Title { get; set; }
        [field:SerializeField] public TechnologyTrade Trade { get; set; }
        
        public TechnologyTemp Temp { get; set; }
        public TechnologyRepository Repository { get; set; }
        protected override ITechnologyStore CompareTemplate()
        {
            if (Temp == null)
            {
                return this;
            }
            return Temp.TechnologyStores[Title];
        }
        public bool Status()
        {
            if (Temp != null)
                return Temp.TechnologyStores[this.Title].CurrentStatus;
            return false;
        }
        internal override string DataNamePattern => $"Technology_stage[{stage}][{Title}]";
    }
}