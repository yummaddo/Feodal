using System.Collections.Generic;
using System.Linq.Expressions;
using Game.Core.Abstraction;

namespace Game.Core.DataStructures.Trades
{
    public class ResourceTradeMap : AbstractTradeMap<IResource, IResource>
    {
        public ResourceTradeMap( ResourceTrade trade )
        {
            From = new List<MapValue<IResource>>();
            foreach (var element in trade.resourceAmountCondition)
            {
                From.Add(new MapValue<IResource>(element.resource.Data, element.amount ));
            }
            Into = new MapValue<IResource>(trade.Into.Data, trade.Value);
        }
        public sealed override MapValue<IResource> Into { get; protected set; }
        public sealed override List<MapValue<IResource>> From { get; protected set; }
    }
}