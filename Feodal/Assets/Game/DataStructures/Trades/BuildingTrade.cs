using System.Collections.Generic;
using Game.DataStructures.Technologies;
using Game.DataStructures.Trades.Abstraction;
using Game.DataStructures.Trades.Map;
using Game.Services.CellServices;
using Game.Services.StorageServices.Microservice;
using UnityEngine;

namespace Game.DataStructures.Trades
{
    [System.Serializable]
    public class BuildingTrade: AbstractTrade<CellState,BuildingTrade>
    {
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition;
        [SerializeField] internal List<Technology> technologyCondition;
        [SerializeField] public CellState Into;
        [SerializeField] public int Value;
        
        private CellService _cellService;
        internal BuildingTradeMap Map;

        public override string TradeName => Into.externalName;
        internal void Inject(CellService service)
        {
            _cellService = service;
        }

        internal override void Initialization(TradeMicroservice microservice)
        {
            base.Initialization(microservice);
            
            Map = new BuildingTradeMap(this);
        }
        public int CellStateQuantity() => _cellService.cellMap.GetCountOfCellState(Into.Data);
        public override bool IsTradAble() => TradeMicroservice.CanTrade(Map.GetAmount(1));
        public override bool IsTradAble(int amount) => TradeMicroservice.CanTrade(Map.GetAmount(amount));
        public override bool IsTradAbleAll() => TradeMicroservice.CanTrade(Map.GetAmount(1));
        public override void TradeAmount(int amount) { TradeMicroservice.Trade(this, Map.GetAmount(amount), amount); }
        public override void TradeAll() { TradeMicroservice.Trade(this,Map.GetAmount(1),1, true); }
        public override void Trade() { TradeMicroservice.Trade(this,Map.GetAmount(1),1); }
    }
}