using Game.Services.ProxyServices.Abstraction;
using Game.Typing;

namespace Game.CallBacks.CallBackTrade
{
    public interface ITradeCallBack<TRoot,TType>:  ICallBack<TRoot>
    {
        public int Amount { get; set; }
        public TType Trade { get; set; }
        public TradeCallBackResult Result { get; set; }
        public void UpdateTradeCallBack(int amount, TType resourceTrade, bool res = true);
    }
}