using System.Collections.Generic;
using Game.DataStructures.Conditions.Abstraction;
using Game.Services.StorageServices.Microservice;

namespace Game.DataStructures.Trades.Abstraction
{
    [System.Serializable]
    public abstract class AbstractTrade<TInto, TBase>:  ITrade<TInto,TBase>
    {
        public List<ICondition> Conditions { get; set; }
        protected TradeMicroservice TradeMicroservice;
        public abstract string TradeName { get; }
        public abstract void TradeAmount(int amount);
        public abstract void TradeAll();
        public abstract void Trade();

        public override string ToString()
        {
            return TradeName;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        internal virtual void Initialization(TradeMicroservice microservice)
        {
            TradeMicroservice = microservice;
            if (microservice == null) ConnectingToTradeService();
        }
        
        private void ConnectingToTradeService()
        {
            TradeMicroservice =  SessionLifeStyleManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
        }
        public abstract bool IsTradAble();
        public abstract bool IsTradAble(int amount);
        public abstract bool IsTradAbleAll();
    }
}