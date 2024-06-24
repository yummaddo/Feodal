using System;
using Game.Core.DataStructures.Trades;
using Game.Core.Typing;
using Game.Services.Proxies.Abstraction;

namespace Game.Services.Proxies.CallBackTrade
{
    public class TechnologyTradeCallBack :ITradeCallBack<TechnologyTradeCallBack,TechnologyTrade>
    {
        public TechnologyTradeCallBack()
        {
            
        }
        public TechnologyTrade Trade { get; set; }
        public TradeCallBackResult Result { get; set; }
        public void UpdateTradeCallBack(int amount, TechnologyTrade resourceTrade, bool res = true)
        {
            Amount = amount;
            Trade = resourceTrade;
            Result = res ? TradeCallBackResult.Successfully : TradeCallBackResult.Failed;
        }

        public Action<Port, TechnologyTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
        public int Amount { get; set; } = 0;
    }
}