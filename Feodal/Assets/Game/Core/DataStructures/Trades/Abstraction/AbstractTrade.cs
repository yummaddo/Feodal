using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions.Abstraction;
using Game.Meta;
using Game.Services.Storage.Microservice;
using UnityEngine;

namespace Game.Core.DataStructures.Trades.Abstraction
{
    public abstract class AbstractTrade<TInto, TBase>:  ITrade<TInto,TBase>
    {
        public List<ICondition> Conditions { get; set; }
        protected TradeMicroservice TradeMicroservice;
        public abstract string TradeName { get; }
        public override string ToString() { return TradeName; }
        internal virtual void Initialization() { }
        internal  void Initialization(TradeMicroservice microservice)
        {
            TradeMicroservice = microservice;
            if (microservice == null) ConnectingToTradeService();
            Initialization();
        }
        private void ConnectingToTradeService() { TradeMicroservice =  SessionStateManager.Instance.Container.Resolve<TradeMicroservice>(); }
        public bool IsTradAble()
        {
            foreach (var condition in Conditions) { if (condition.Status()) return true; }
            return false;
        }
        public abstract void TradeAmount(int amount);
        public abstract void TradeAll();
        public abstract void Trade();
    }
    
}