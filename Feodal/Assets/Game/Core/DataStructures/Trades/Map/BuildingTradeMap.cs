using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Trades.Abstraction;

namespace Game.Core.DataStructures.Trades.Map
{
    public class BuildingTradeMap :  AbstractTradeMap<ICellState, IResource>
    {
        private BuildingTrade _tradeDependency;
        public BuildingTradeMap( BuildingTrade trade )
        {
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
            foreach (var pair in dataMap)
            {
                dataMap[pair.Key] = pair.Value * (value*cellAmount);
                cellAmount++;
            }
            return dataMap;
        }
        public sealed override MapValue<ICellState> Into { get; protected set; }
        public sealed override List<MapValue<IResource>> From { get; protected set; }
    }
}