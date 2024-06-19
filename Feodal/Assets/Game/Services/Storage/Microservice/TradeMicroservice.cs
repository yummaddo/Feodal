using System;
using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Conditions;
using Game.Core.DataStructures.Trades;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Services.Abstraction.MicroService;
using UnityEngine;

namespace Game.Services.Storage.Microservice
{
    public class TradeMicroservice: AbstractMicroservice<StorageService>
    {


        protected override void OnAwake()
        {

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