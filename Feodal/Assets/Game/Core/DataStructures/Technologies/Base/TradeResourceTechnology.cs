using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Trades;
using UnityEngine;

namespace Game.Core.DataStructures.Technologies.Base
{
    [CreateAssetMenu(menuName = "Technology/TradeResource")]
    public class TradeResourceTechnology :  AbstractDataStructure<ITradeResourceTechnology>, ITradeResourceTechnology
    {
        [field:SerializeField] public ResourceTrade ResourceTrade { get; set; }
        [field:SerializeField] public string Title { get; set; }
        public bool CurrentStatus { get; set; }
        internal override string DataNamePattern => $"Technology_TradeResource_{ResourceTrade.TradeName}";
        protected override ITradeResourceTechnology CompareTemplate()
        {
            return this;
        }
        public bool Status()
        {
            return false;
        }
    }
}