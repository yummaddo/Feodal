using System;
using Game.Core.DataStructures.Trades;
using Game.Services.Proxies.Abstraction;

namespace Game.Services.Proxies.CallBackTrade
{
    public class BuildingTradeCallBack : ICallBack<BuildingTradeCallBack>
    {
        public int Amount;
        public BuildingTrade BuildingTrade;

        public BuildingTradeCallBack(int amount, BuildingTrade buildingTrade)
        {
            Amount = amount;
            BuildingTrade = buildingTrade;
        }

        public Action<Port, BuildingTradeCallBack> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
    }
}