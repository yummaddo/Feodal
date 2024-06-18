using System;
using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Conditions;
using Game.Core.DataStructures.Trades;
using Game.Services.Abstraction.MicroService;
using UnityEngine;

namespace Game.Services.Storage.Microservice
{
    public class TradeMicroservice: AbstractMicroservice<StorageService>
    {
        public List<AbstractTrade<CellState>> cellTrades;
        public List<AbstractTrade<Resource>> resourceTrades;
        public Dictionary<int, AbstractTrade<CellState>> CellTradeHash;
        public Dictionary<int, AbstractTrade<Resource>> ResourceTradeHash;

        protected override void OnAwake()
        {
            CellTradeHash = new Dictionary<int, AbstractTrade<CellState>>();
            ResourceTradeHash = new Dictionary<int, AbstractTrade<Resource>>();
        }

        protected override void OnStart()
        {
        }

        protected override void ReStart()
        {
        }

        protected override void Stop()
        {
        }

        public void Trade(Dictionary<IResource, int> data, bool all = false)
        {
            throw new NotImplementedException();
        }
    }
}