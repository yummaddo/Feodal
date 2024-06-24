using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Trades.Abstraction;

namespace Game.Core.DataStructures.Trades.Map
{
    public class BuildingTradeMap :  AbstractTradeMap<ICellState, IResource>
    {
        private readonly BuildingTrade _tradeDependency;
        public BuildingTradeMap( BuildingTrade trade)
        {
            _tradeDependency = trade;
            From = new List<MapValue<IResource>>();
            foreach (var element in trade.resourceAmountCondition)
            {
                From.Add(new MapValue<IResource>(element.resource.Data, element.value));
            }
            Into = new MapValue<ICellState>(trade.Into, 1);
        }
        public override Dictionary<IResource, int> GetAmount(int amount)
        {
            var cellAmount = _tradeDependency.CellStateQuantity();
            var value = Into.Value.Resource.Quantity;
            var dataMap =  base.GetAmount(amount);
            Dictionary<IResource, int> newDict = new Dictionary<IResource, int>();
            foreach (var pair in dataMap)
            {
                newDict.Add(pair.Key, pair.Value * (value*cellAmount));
                cellAmount++;
            }
            dataMap.Clear();
            return newDict;
        }
        public sealed override MapValue<ICellState> Into { get; protected set; }
        public sealed override List<MapValue<IResource>> From { get; protected set; }
    }
}