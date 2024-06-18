using System.Collections.Generic;

namespace Game.Core.Abstraction
{
    public interface ITrade<out TInto>
    {
        public List<ICondition> Conditions { get; set; }
        public TInto Into { get; }
        public bool IsTradAble();
        public void TradeAmount( int amount );
        public void TradeAll( );
        public void Trade( );
    }
}