using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades.Abstraction;

namespace Game.Core.DataStructures.Trades.Map
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