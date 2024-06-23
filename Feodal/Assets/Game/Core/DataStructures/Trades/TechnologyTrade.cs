using System.Collections.Generic;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Core.DataStructures.Trades.Map;
using Game.Services.Storage.Microservice;
using UnityEngine;

namespace Game.Core.DataStructures.Trades
{
    [System.Serializable]
    public class TechnologyTrade : AbstractTrade<Technology,ResourceTrade>
    {
        internal TechnologyTradeMap Map;
        [SerializeField] internal Sprite sprite;
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        [SerializeField] public Technology into;
        public override string TradeName => into.Title;
        internal override void Initialization(TradeMicroservice microservice)
        {
            base.Initialization(microservice);
            Map = new TechnologyTradeMap(this);
        }
        public override bool IsTradAble() => TradeMicroservice.CanTrade(Map.GetAmount(1));
        public override bool IsTradAble(int amount) => TradeMicroservice.CanTrade(Map.GetAmount(amount));
        public override bool IsTradAbleAll() => TradeMicroservice.CanTrade(Map.GetAmount(1));
        public override void TradeAmount(int amount) { TradeMicroservice.Trade(this, Map.GetAmount(amount), amount); }
        public override void TradeAll() { TradeMicroservice.Trade(this,Map.GetAmount(1),1, true); }
        public override void Trade() { TradeMicroservice.Trade(this,Map.GetAmount(1),1); }
    }
}