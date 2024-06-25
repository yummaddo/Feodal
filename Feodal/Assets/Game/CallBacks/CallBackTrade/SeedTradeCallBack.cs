using System;
using Game.DataStructures.Trades;
using Game.Services.ProxyServices;
using Game.Typing;

namespace Game.CallBacks.CallBackTrade
{
    public class SeedTradeCallBack : ITradeCallBack<SeedTradeCallBack,SeedTrade >
    {
        public SeedTradeCallBack(){}
        public Action<Port, SeedTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
        public int Amount { get; set; } = 0;
        public SeedTrade Trade { get; set; }
        public TradeCallBackResult Result { get; set; }
        public void UpdateTradeCallBack(int amount, SeedTrade resourceTrade, bool res = true)
        {            
            Amount = amount;
            Trade = resourceTrade;
            Result = res ? TradeCallBackResult.Successfully : TradeCallBackResult.Failed;
        }
    }
}