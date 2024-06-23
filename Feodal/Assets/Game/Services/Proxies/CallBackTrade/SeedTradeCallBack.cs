using System;
using Game.Core.DataStructures.Trades;
using Game.Services.Proxies.Abstraction;

namespace Game.Services.Proxies.CallBackTrade
{
    public class SeedTradeCallBack : ICallBack<SeedTradeCallBack>
    {
        public int Amount;
        public SeedTrade SeedTrade;

        public SeedTradeCallBack(int amount, SeedTrade seedTrade)
        {
            Amount = amount;
            SeedTrade = seedTrade;
        }

        public Action<Port, SeedTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
    }
}