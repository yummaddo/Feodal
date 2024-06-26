using System.Collections.Generic;
using Game.DataStructures.Trades.Abstraction;
using Game.Services.StorageServices.Microservice;

namespace Game.DataStructures.Trades.Map
{
    public class SeedTradeMap : AbstractTradeMap<Resource,ResourceCounter>
    {
        private SeedTrade trade;
        public SeedTradeMap( SeedTrade trade, TradeMicroservice microservice )
        {
            this.trade = trade;
            int indexation = 1;
            trade.Trades.Clear();
            foreach (var counters in trade.resourceAmountCondition)
            {
                trade.Trades.Add(indexation, counters);
                counters.Initialization(microservice);
                indexation++;
            }
        }

        public ResourceTrade GetTradeByStage(int index)
        {
            return trade.resourceAmountCondition[index-1];
        }

        public override MapValue<Resource> Into { get; protected set; }
        public override List<MapValue<ResourceCounter>> From { get; protected set; }
    }
}