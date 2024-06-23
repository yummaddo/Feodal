using System;
using Game.Core.DataStructures.Trades;
using Game.Services.Proxies.Abstraction;

namespace Game.Services.Proxies.CallBackTrade
{
    public class ResourceTradeCallBack : ICallBack<ResourceTradeCallBack>
    {
        public int Amount;
        public ResourceTrade ResourceTrade;

        public ResourceTradeCallBack(int amount, ResourceTrade resourceTrade)
        {
            Amount = amount;
            ResourceTrade = resourceTrade;
        }

        public Action<Port, ResourceTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
    }
}