using System;
using Game.DataStructures.Abstraction;
using Game.DataStructures.Technologies.Abstraction;
using Game.DataStructures.Trades;
using Game.RepositoryEngine.TechnologyRepositories;
using UnityEngine;

namespace Game.DataStructures.Technologies
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
            try
            {
                if (Temp != null)
                    return Temp.GetAmount(Title);
                return false;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

            return false;
        }
        internal override string DataNamePattern => $"Technology_stage[{stage}][{Title}]";
    }
}