using System.Collections.Generic;
using JetBrains.Annotations;

namespace Game.Core.DataStructures.Trades.Abstraction
{
    public abstract class AbstractTradeMap<TKey,TValue>
    {
        public abstract MapValue<TKey> Into { get; protected set; }

        [ItemCanBeNull]
        public abstract List<MapValue<TValue>> From { get; protected set; }
        public class MapValue<TElement>
        {
            public TElement Value { get; }
            public int Amount { get; }
            public MapValue(TElement value, int amount)
            {
                Value = value;
                Amount = amount;
            }
        }
        public virtual Dictionary<TValue, int> GetAmount(int amount)
        {
            var result = new Dictionary<TValue, int>();
            foreach (var value in From)
            {
                if (value != null) 
                    result.Add(value.Value, value.Amount * amount);
            }
            return result;
        }
    }
}