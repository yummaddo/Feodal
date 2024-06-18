using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.Conditions;
using Game.Meta;
using Game.Services.Storage.Microservice;
using UnityEngine;

namespace Game.Core.DataStructures.Trades
{
    public abstract class AbstractTrade<TInto>: AbstractDataStructure<ITrade<TInto>>, ITrade<TInto>
    {
        public List<AbstractCondition<ICondition>> conditions;
        protected TradeMicroservice TradeMicroservice;
        public List<ICondition> Conditions { get; set; }
        public string TradeName => DataNamePattern;
        [field:SerializeField]
        public TInto Into { get; set; }
        [field:SerializeField]
        public int Value { get; set; }
        protected virtual void Initialization()
        {
        }
        internal  void Initialization(TradeMicroservice microservice)
        {
            TradeMicroservice = microservice;
            if (microservice == null) ConnectingToTradeService();
            Initialization();
        }
        private void ConnectingToTradeService()
        {
            TradeMicroservice =  SessionStateManager.Instance.Container.Resolve<TradeMicroservice>();
        }
        protected override string DataNamePattern => $"Into:{Into.ToString()}";
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