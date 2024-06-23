using System;
using Game.Core.DataStructures.Trades;
using Game.Services.Proxies.Abstraction;

namespace Game.Services.Proxies.CallBackTrade
{
    public class TechnologyTradeCallBack : ICallBack<TechnologyTradeCallBack>
    {
        public int Amount;
        public TechnologyTrade TechnologyTrade;

        public TechnologyTradeCallBack(int amount, TechnologyTrade technologyTrade)
        {
            Amount = amount;
            TechnologyTrade = technologyTrade;
        }

        public Action<Port, TechnologyTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
    }
}