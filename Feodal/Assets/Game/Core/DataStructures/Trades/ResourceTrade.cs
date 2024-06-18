using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions;
using Game.Meta;
using Game.Services.Storage.Microservice;
using UnityEngine;

namespace Game.Core.DataStructures.Trades
{
    public class ResourceTrade: AbstractTrade<Resource>
    {
        [SerializeField] internal List<ResourceAmountCondition> resourceAmountCondition;
        private ResourceTradeMap _map;
        protected override ITrade<Resource> CompareTemplate()
        {
            return this;
        }
        protected override void Initialization()
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