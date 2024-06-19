using Game.Core.DataStructures.Conditions.Abstraction.Base;
using Game.Core.DataStructures.Trades;

namespace Game.Core.DataStructures.Conditions.Abstraction.Trades
{
    public interface ITradeResourceTechnologyCondition : ITechnologyCondition
    {
        public ResourceTrade ResourceTrade { get; set; }
    }
}