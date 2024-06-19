using System.Collections.Generic;
using Game.Core.DataStructures.Conditions.Abstraction;

namespace Game.Core.DataStructures.Trades.Abstraction
{
    public interface ITrade<out TInto, TType>
    {
        public List<ICondition> Conditions { get; set; }
        public bool IsTradAble();
        public void TradeAmount( int amount );
        public void TradeAll( );
        public void Trade( );
    }
}