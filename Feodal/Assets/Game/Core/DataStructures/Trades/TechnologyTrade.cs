using System.Collections.Generic;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Core.DataStructures.Trades.Map;
using UnityEngine;

namespace Game.Core.DataStructures.Trades
{
    [System.Serializable]
    public class TechnologyTrade : AbstractTrade<Technology,ResourceTrade>
    {
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        private TechnologyTradeMap _map;
        [SerializeField] public Technology into;
        public override string TradeName => into.Title;
        internal override void Initialization()
        {
            base.Initialization();
            _map = new TechnologyTradeMap(this);
        }
        public override void TradeAmount(int amount)
        {
            TradeMicroservice.Trade(_map.GetAmount(1));
        }
        public override void TradeAll()
        {
            TradeMicroservice.Trade(_map.GetAmount(1));
        }
        public override void Trade()
        {
            TradeMicroservice.Trade(_map.GetAmount(1));
        }
    }
}