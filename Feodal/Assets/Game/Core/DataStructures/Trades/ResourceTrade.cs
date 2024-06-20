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
        private ResourceTradeMap _map;
        [SerializeField] internal List<Technology> technologyCondition;
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        [SerializeField] public Resource Into;
        [SerializeField] public int Value;
        
        public override string TradeName => Into.title;
        
        protected override void Initialization()
        {
            base.Initialization();
            _map = new ResourceTradeMap(this);
        }
        public override bool IsTradAble() => TradeMicroservice.CanTrade(_map.GetAmount(1));
        public override bool IsTradAble(int amount) => TradeMicroservice.CanTrade(_map.GetAmount(amount));
        public override bool IsTradAbleAll() => TradeMicroservice.CanTrade(_map.GetAmount(1));
        public override void TradeAmount(int amount) { TradeMicroservice.Trade(this, _map.GetAmount(amount), amount); }
        public override void TradeAll() { TradeMicroservice.Trade(this,_map.GetAmount(1),1, true); }
        public override void Trade() { TradeMicroservice.Trade(this,_map.GetAmount(1),1); }
    }
}