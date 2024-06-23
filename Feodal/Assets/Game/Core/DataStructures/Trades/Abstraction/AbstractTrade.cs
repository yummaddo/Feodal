using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction;
using Game.Meta;
using Game.Services.Storage.Microservice;
using UnityEngine;

namespace Game.Core.DataStructures.Trades.Abstraction
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
            TradeMicroservice =  SessionStateManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
        }
        public abstract bool IsTradAble();
        public abstract bool IsTradAble(int amount);
        public abstract bool IsTradAbleAll();
    }
}