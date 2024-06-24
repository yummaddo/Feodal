using System;
using Game.Core.DataStructures.Trades;
using Game.Core.Typing;
using Game.Services.Proxies.Abstraction;

namespace Game.Services.Proxies.CallBackTrade
{
    public class BuildingTradeCallBack :  ITradeCallBack<BuildingTradeCallBack,BuildingTrade >
    {
        public BuildingTradeCallBack()
        {
            
        }
        public Action<Port, BuildingTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
        public int Amount { get; set; } = 0;
        public BuildingTrade Trade { get; set; }
        public TradeCallBackResult Result { get; set; }
        public void UpdateTradeCallBack(int amount, BuildingTrade resourceTrade, bool res = true)
        {
            Amount = amount;
            Trade = resourceTrade;
            Result = res ? TradeCallBackResult.Successfully : TradeCallBackResult.Failed;
        }
    }
}