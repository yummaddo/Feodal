using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions.TradesConditions;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Core.DataStructures.Trades.Map;
using UnityEngine;

namespace Game.Core.DataStructures.Trades
{
    [System.Serializable]
    public class ResourceTrade: AbstractTrade<Resource,ResourceTrade>
    {
        [SerializeField] internal List<Technology> technologyCondition;
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        private ResourceTradeMap _map;
        [SerializeField] public Resource Into;
        [SerializeField] public int Value;
        public override string TradeName => Into.title;

        internal override void Initialization()
        {
            base.Initialization();
            _map = new ResourceTradeMap(this);
        }
        public override void TradeAmount(int amount)
        {
            TradeMicroservice.Trade(_map.GetAmount(amount));
        }
        public override void TradeAll()
        {
            TradeMicroservice.Trade(_map.GetAmount(1), true);
        }
        public override void Trade()
        {
            TradeMicroservice.Trade(_map.GetAmount(1));
        }
    }
}