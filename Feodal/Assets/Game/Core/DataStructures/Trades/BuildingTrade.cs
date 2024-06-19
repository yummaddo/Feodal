using System.Collections.Generic;
using Game.Core.DataStructures.Conditions.TradesConditions;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Core.DataStructures.Trades.Map;
using Game.Services.CellControlling;
using UnityEngine;

namespace Game.Core.DataStructures.Trades
{
    [System.Serializable]
    public class BuildingTrade: AbstractTrade<CellState,BuildingTrade>
    {
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        [SerializeField] internal List<Technology> technologyCondition;
        private CellService _cellService;
        private BuildingTradeMap _map;
        [SerializeField] public CellState Into;
        [SerializeField] public int Value;
        internal void Inject(CellService service) => _cellService = service;
        public override string TradeName => Into.externalName;
        internal override void Initialization()
        {
            base.Initialization();
            _map = new BuildingTradeMap(this);
        }
        public int CellStateQuantity() => _cellService.cellMap.GetCountOfCellState(Into.Data);
        public override void TradeAmount(int amount) => Trade();
        public override void TradeAll() => Trade();
        public override void Trade() => TradeMicroservice.Trade(_map.GetAmount(1));
    }
}