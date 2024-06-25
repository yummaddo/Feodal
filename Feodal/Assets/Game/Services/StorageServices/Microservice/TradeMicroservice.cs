using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.DataStructures.Abstraction;
using Game.DataStructures.Trades;
using Game.RepositoryEngine.ResourcesRepository;
using Game.Services.Abstraction;

namespace Game.Services.StorageServices.Microservice
{
    public class TradeMicroservice: AbstractMicroservice<StorageService>
    {
        private ResourceTemp _resourceTemp;
        internal event Action<ResourceTrade, Dictionary<IResource, int>,int, bool>   OnTryResourceTrade;
        internal event Action<BuildingTrade, Dictionary<IResource, int>,int, bool>   OnTryBuildingTrade;
        internal event Action<SeedTrade, Dictionary<IResource, int>,int, bool>       OnTrySeedTrade;
        internal event Action<TechnologyTrade, Dictionary<IResource, int>,int, bool> OnTryTechnologyTrade;
        
        internal event Action<ResourceTrade, int, bool>   OnSuccessfullyResourceTrade;
        internal event Action<BuildingTrade,int, bool>    OnSuccessfullyBuildingTrade;
        internal event Action<SeedTrade, int, bool>       OnSuccessfullySeedTrade;
        internal event Action<TechnologyTrade, int, bool> OnSuccessfullyTechnologyTrade;
        
        internal event Action<ResourceTrade, int, bool>   OnFailedResourceTrade;
        internal event Action<BuildingTrade,int, bool>    OnFailedBuildingTrade;
        internal event Action<SeedTrade, int, bool>       OnFailedSeedTrade;
        internal event Action<TechnologyTrade, int, bool> OnFailedTechnologyTrade;
        
        internal void SuccessfullyInvoke(ResourceTrade arg, int amount, bool all) => OnSuccessfullyResourceTrade?.Invoke(arg, amount,all);
        internal void SuccessfullyInvoke(BuildingTrade arg, int amount, bool all) => OnSuccessfullyBuildingTrade?.Invoke(arg, amount,all);
        internal void SuccessfullyInvoke(SeedTrade arg, int amount, bool all)
        {
            OnSuccessfullySeedTrade?.Invoke(arg, amount, all);
        }
        internal void SuccessfullyInvoke(TechnologyTrade arg, int amount, bool all) => OnSuccessfullyTechnologyTrade?.Invoke(arg, amount,all);
        internal void FailedInvoke(ResourceTrade arg, int amount, bool all) => OnFailedResourceTrade?.Invoke(arg, amount,all);
        internal void FailedInvoke(BuildingTrade arg, int amount, bool all) => OnFailedBuildingTrade?.Invoke(arg, amount,all);
        internal void FailedInvoke(SeedTrade arg, int amount, bool all) => OnFailedSeedTrade?.Invoke(arg, amount,all);
        internal void FailedInvoke(TechnologyTrade arg, int amount, bool all) => OnFailedTechnologyTrade?.Invoke(arg, amount,all);

        protected override Task OnAwake(IProgress<float> progress) { Service.OnResourceRepositoryInit += ServiceResourceRepositoryInit; return Task.CompletedTask; }
        private void ServiceResourceRepositoryInit() { _resourceTemp = Service.GetResourceTemp(); }
        protected override Task OnStart(IProgress<float> progress) { return Task.CompletedTask; }

        internal int GetMaxTrade(Dictionary<IResource, int> data) => _resourceTemp.MaxTradeAmount(data);
        internal bool CanTrade(Dictionary<IResource, int> data) => _resourceTemp.IsTradAble(data);
        internal void Trade(ResourceTrade trade,   Dictionary<IResource, int> data,int amount,  bool all = false) => OnTryResourceTrade?.Invoke(trade,data,amount, all);
        internal void Trade(BuildingTrade trade,   Dictionary<IResource, int> data,int amount,  bool all = false) => OnTryBuildingTrade?.Invoke(trade,data,amount, all);
        internal void Trade(SeedTrade trade,       Dictionary<IResource, int> data,int amount,  bool all = false) => OnTrySeedTrade?.Invoke(trade,data,amount, all);
        internal void Trade(TechnologyTrade trade, Dictionary<IResource, int> data,int amount,  bool all = false) => OnTryTechnologyTrade?.Invoke(trade,data,amount, all);
    }
}