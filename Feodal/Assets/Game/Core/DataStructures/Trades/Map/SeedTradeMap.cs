using System.Collections.Generic;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Services.Storage.Microservice;

namespace Game.Core.DataStructures.Trades.Map
{
    public class SeedTradeMap : AbstractTradeMap<Resource,ResourceCounter>
    {
        private SeedTrade trade;
        public SeedTradeMap( SeedTrade trade, TradeMicroservice microservice )
        {
            this.trade = trade;
            foreach (var counters in trade.resourceAmountCondition)
            {
                counters.Initialization(microservice);
            }
        }

        public ResourceTrade GetTradeByStage(int index)
        {
            return trade.resourceAmountCondition[index];
        }

        public override MapValue<Resource> Into { get; protected set; }
        public override List<MapValue<ResourceCounter>> From { get; protected set; }
    }
}