using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.DataStructures.Trades.Abstraction;

namespace Game.DataStructures.Trades.Map
{
    public class ResourceTradeMap : AbstractTradeMap<IResource, IResource>
    {
        public ResourceTradeMap( ResourceTrade trade )
        {
            From = new List<MapValue<IResource>>();
            foreach (var element in trade.resourceAmountCondition)
                From.Add(new MapValue<IResource>(element.resource, element.value ));
            Into = new MapValue<IResource>(trade.Into.Data, trade.Value);
        }
        public sealed override MapValue<IResource> Into { get; protected set; }
        public sealed override List<MapValue<IResource>> From { get; protected set; }
    }
}