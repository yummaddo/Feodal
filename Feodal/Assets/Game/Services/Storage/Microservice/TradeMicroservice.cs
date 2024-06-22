using System;
using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Conditions;
using Game.Core.DataStructures.Trades;
using Game.Core.DataStructures.Trades.Abstraction;
using Game.Services.Abstraction.MicroService;
using Game.Services.Storage.ResourcesRepository;
using UnityEngine;

namespace Game.Services.Storage.Microservice
{
    public class TradeMicroservice: AbstractMicroservice<StorageService>
    {
        private ResourceTemp _resourceTemp;
        private event Action<ResourceTrade, Dictionary<IResource, int>,int, bool>   OnTryResourceTrade;
        private event Action<BuildingTrade, Dictionary<IResource, int>,int, bool>   OnTryBuildingTrade;
        private event Action<SeedTrade, Dictionary<IResource, int>,int, bool>       OnTrySeedTrade;
        private event Action<TechnologyTrade, Dictionary<IResource, int>,int, bool> OnTryTechnologyTrade;
        
        internal event Action<ResourceTrade, int, bool>   OnSuccessfullyResourceTrade;
        internal event Action<BuildingTrade,int, bool>    OnSuccessfullyBuildingTrade;
        internal event Action<SeedTrade, int, bool>       OnSuccessfullySeedTrade;
        internal event Action<TechnologyTrade, int, bool> OnSuccessfullyTechnologyTrade;
        
        internal event Action<ResourceTrade, int, bool>   OnFailedResourceTrade;
        internal event Action<BuildingTrade,int, bool>    OnFailedBuildingTrade;
        internal event Action<SeedTrade, int, bool>       OnFailedSeedTrade;
        internal event Action<TechnologyTrade, int, bool> OnFailedTechnologyTrade;

        internal void InjectTrade(Action<ResourceTrade, Dictionary<IResource, int>,int, bool> trade)   => OnTryResourceTrade += trade;
        internal void InjectTrade(Action<BuildingTrade, Dictionary<IResource, int>,int, bool> trade)   => OnTryBuildingTrade += trade;
        internal void InjectTrade(Action<SeedTrade, Dictionary<IResource, int>,int, bool> trade)       => OnTrySeedTrade += trade;
        internal void InjectTrade(Action<TechnologyTrade, Dictionary<IResource, int>,int, bool> trade) => OnTryTechnologyTrade += trade;
        
        internal void InjectSuccessfully(Action<ResourceTrade, int, bool> trade)   => OnSuccessfullyResourceTrade += trade;
        internal void InjectSuccessfully(Action<BuildingTrade, int, bool> trade)   => OnSuccessfullyBuildingTrade += trade;
        internal void InjectSuccessfully(Action<SeedTrade, int, bool> trade)       => OnSuccessfullySeedTrade += trade;
        internal void InjectSuccessfully(Action<TechnologyTrade, int, bool> trade) => OnSuccessfullyTechnologyTrade += trade;
        
        internal void InjectFailed(Action<ResourceTrade, int, bool> trade)   => OnFailedResourceTrade += trade;
        internal void InjectFailed(Action<BuildingTrade, int, bool> trade)   => OnFailedBuildingTrade += trade;
        internal void InjectFailed(Action<SeedTrade, int, bool> trade)       => OnFailedSeedTrade += trade;
        internal void InjectFailed(Action<TechnologyTrade, int, bool> trade) => OnFailedTechnologyTrade += trade;
        
        protected override void OnAwake()
        {
            Service.OnResourceRepositoryInit += ServiceResourceRepositoryInit;
        }
        
        private void ServiceResourceRepositoryInit()
        {
            _resourceTemp = Service.GetResourceTemp();
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
        internal int GetMaxTrade(Dictionary<IResource, int> data) => _resourceTemp.MaxTradeAmount(data);
        internal bool CanTrade(Dictionary<IResource, int> data) => _resourceTemp.IsTradAble(data);
        internal void Trade(ResourceTrade trade,   Dictionary<IResource, int> data,int amount,  bool all = false) => OnTryResourceTrade?.Invoke(trade,data,amount, all);
        internal void Trade(BuildingTrade trade,   Dictionary<IResource, int> data,int amount,  bool all = false) => OnTryBuildingTrade?.Invoke(trade,data,amount, all);
        internal void Trade(SeedTrade trade,       Dictionary<IResource, int> data,int amount,  bool all = false) => OnTrySeedTrade?.Invoke(trade,data,amount, all);
        internal void Trade(TechnologyTrade trade, Dictionary<IResource, int> data,int amount,  bool all = false) => OnTryTechnologyTrade?.Invoke(trade,data,amount, all);
    }
}