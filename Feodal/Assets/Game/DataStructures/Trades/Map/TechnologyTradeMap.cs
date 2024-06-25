using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.DataStructures.Technologies;
using Game.DataStructures.Trades.Abstraction;

namespace Game.DataStructures.Trades.Map
{
    public class TechnologyTradeMap : AbstractTradeMap<Technology, IResource>
    {
        public sealed override MapValue<Technology> Into { get; protected set; }
        public sealed override List<MapValue<IResource>> From { get; protected set; }
        
        public TechnologyTradeMap( TechnologyTrade trade )
        {
            From = new List<MapValue<IResource>>();
            foreach (var element in trade.resourceAmountCondition)
                From.Add(new MapValue<IResource>(element.resource.Data, element.value ));
            Into = new MapValue<Technology>(trade.@into, 1);
        }

    }
}