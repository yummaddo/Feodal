using Game.DataStructures.Conditions.Abstraction.Base;
using Game.DataStructures.Trades;

namespace Game.DataStructures.Conditions.Abstraction.Trades
{
    public interface ITradeResourceTechnologyCondition : ITechnologyCondition
    {
        public ResourceTrade ResourceTrade { get; set; }
    }
}