using Game.Core.Typing;
using Game.Services.Proxies.Abstraction;

namespace Game.Services.Proxies.CallBackTrade
{
    public interface ITradeCallBack<TRoot,TType>:  ICallBack<TRoot>
    {
        public int Amount { get; set; }
        public TType Trade { get; set; }
        public TradeCallBackResult Result { get; set; }
        public void UpdateTradeCallBack(int amount, TType resourceTrade, bool res = true);
    }
}