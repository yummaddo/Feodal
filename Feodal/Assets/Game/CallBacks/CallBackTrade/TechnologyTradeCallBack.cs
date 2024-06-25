using System;
using Game.DataStructures.Trades;
using Game.Services.ProxyServices;
using Game.Typing;

namespace Game.CallBacks.CallBackTrade
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