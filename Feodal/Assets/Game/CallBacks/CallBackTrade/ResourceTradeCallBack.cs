using System;
using Game.DataStructures.Trades;
using Game.Services.ProxyServices;
using Game.Typing;

namespace Game.CallBacks.CallBackTrade
{
    public class ResourceTradeCallBack :ITradeCallBack<ResourceTradeCallBack,ResourceTrade >
    {
        public ResourceTradeCallBack()
        {
            
        }
        public Action<Port, ResourceTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
        public int Amount { get; set; } = 0;
        public ResourceTrade Trade { get; set; }
        public TradeCallBackResult Result { get; set; }
        public void UpdateTradeCallBack(int amount, ResourceTrade resourceTrade, bool res = true)
        {
            Amount = amount;
            Trade = resourceTrade;
            Result = res ? TradeCallBackResult.Successfully : TradeCallBackResult.Failed;
        }
    }
}