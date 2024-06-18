using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions;
using Game.Services.CellControlling;
using UnityEngine;

namespace Game.Core.DataStructures.Trades
{
    public class BuildingTrade: AbstractTrade<CellState>
    {
        [SerializeField] internal List<ResourceAmountCondition> resourceAmountCondition;
        private CellService _cellService;
        private BuildingTradeMap _map;
        protected override ITrade<CellState> CompareTemplate()
        {
            return this;
        }
        internal void Inject(CellService service) => _cellService = service;
        protected override void Initialization()
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