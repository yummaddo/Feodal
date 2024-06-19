using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Trades.Abstraction;

namespace Game.Core.DataStructures.Trades.Map
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
        public ResourceTradeMap( SeedTrade trade )
        {
            var scale = trade.scaleResourceValue;
            int count = trade.CellQuantity();
            From = new List<MapValue<IResource>>();
            foreach (var element in trade.resourceAmountCondition)
            {
                From.Add(new MapValue<IResource>(element.resource, element.value * count * scale));
            }
            Into = new MapValue<IResource>(trade.@into.Data, 1);
        }
        public Dictionary<IResource, int> GetSeedAmount()
        {
            return base.GetAmount(1);
        }
        public sealed override MapValue<IResource> Into { get; protected set; }
        public sealed override List<MapValue<IResource>> From { get; protected set; }
    }
}