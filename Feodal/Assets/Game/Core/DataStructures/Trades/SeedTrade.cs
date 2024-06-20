using System.Collections.Generic;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Core.DataStructures.Trades.Map;
using Game.Services.CellControlling;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Core.DataStructures.Trades
{
    [System.Serializable]
    public class SeedTrade : AbstractTrade<Resource,ResourceTrade>
    {
        public int id = 0;
        public int scaleResourceValue = 2;
        [SerializeField] public Resource into;
        
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        [SerializeField] internal List<Technology> technologyCondition;
        
        private CellService _cellService;
        private ResourceTradeMap _map;
        
        public override string TradeName => ToString();
        
        internal int CellQuantity() => _cellService.cellMap.GetCellCount(into.Data);
        internal void Inject(CellService service) => _cellService = service;

        public override string ToString()
        {
            return $"Seed_{into.title}_{id}";
        }

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