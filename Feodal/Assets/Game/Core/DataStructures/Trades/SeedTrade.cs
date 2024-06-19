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
        [SerializeField] public Resource into;
        public int scaleResourceValue = 2;
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        [SerializeField] internal List<Technology> technologyCondition;
        private CellService _cellService;
        private ResourceTradeMap _map;
        internal void Inject(CellService service) => _cellService = service;
        public override string TradeName => into.title;
        public int CellQuantity() => _cellService.cellMap.GetCellCount(into.Data);
        internal override void Initialization()
        {
            base.Initialization();
            _map = new ResourceTradeMap(this);
        }
        public override void TradeAmount(int amount)
        {
            _map.GetSeedAmount();
        }
        public override void TradeAll()
        {
            _map.GetSeedAmount();
        }
        public override void Trade()
        {
            _map.GetSeedAmount();
        }
    }
}