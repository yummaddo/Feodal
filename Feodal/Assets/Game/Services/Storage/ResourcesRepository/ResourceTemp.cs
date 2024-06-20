using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Trades;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.Microservice;

namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceTemp : Temp<ResourceEncoded, string, long>
    {
        internal Dictionary<string, IResource> Resources;
        private HashSet<ResourceTrade> _resourceTrades = new HashSet<ResourceTrade>();
        private HashSet<BuildingTrade> _buildingTrade = new HashSet<BuildingTrade>();
        private HashSet<TechnologyTrade> _technologyTrade = new HashSet<TechnologyTrade>();
        private HashSet<SeedTrade> _seedTrade = new HashSet<SeedTrade>();

        #region Internal Actions

        private event Action<ResourceTrade, int, bool> OnFailedResourceTrade;
        private event Action<BuildingTrade, int, bool> OnFailedBuildingTrade;
        private event Action<SeedTrade, int, bool> OnFailedSeedTrade;
        private event Action<TechnologyTrade, int, bool> OnFailedTechnologyTrade;
        private event Action<ResourceTrade, int, bool> OnSuccessfullyResourceTrade;
        private event Action<BuildingTrade, int, bool> OnSuccessfullyBuildingTrade;
        private event Action<SeedTrade, int, bool> OnSuccessfullySeedTrade;
        private event Action<TechnologyTrade, int, bool> OnSuccessfullyTechnologyTrade;

        #endregion
        private void FarmResource(CellResourcePackaging farmer) => ProvideAddAmounts(farmer.Resource, farmer.Value);
        
        internal void InjectionTrade(List<ResourceTrade> trades) { foreach (var trade in trades) _resourceTrades.Add(trade); }
        internal void InjectionTrade(List<BuildingTrade> trades) { foreach (var trade in trades) _buildingTrade.Add(trade); }
        internal void InjectionTrade(List<TechnologyTrade> trades) { foreach (var trade in trades) _technologyTrade.Add(trade); }
        internal void InjectionTrade(List<SeedTrade> trades) { foreach (var trade in trades) _seedTrade.Add(trade); }

        internal void InjectionInMicroservice(TradeMicroservice tradeMicroservice)
        {
            Proxy.Connect<ResourceFarmProvider, CellResourcePackaging>(FarmResource);
            
            tradeMicroservice.InjectTrade(ResourceTradeByTradeMap);
            tradeMicroservice.InjectTrade(BuildingTradeByTradeMap);
            tradeMicroservice.InjectTrade(TechnologyTradeByTradeMap);
            tradeMicroservice.InjectTrade(SeedTradeByTradeMap);

            tradeMicroservice.InjectSuccessfully(OnSuccessfullyResourceTrade);
            tradeMicroservice.InjectSuccessfully(OnSuccessfullyBuildingTrade);
            tradeMicroservice.InjectSuccessfully(OnSuccessfullySeedTrade);
            tradeMicroservice.InjectSuccessfully(OnSuccessfullyTechnologyTrade);
            tradeMicroservice.InjectFailed(OnFailedResourceTrade);

            tradeMicroservice.InjectFailed(OnFailedBuildingTrade);
            tradeMicroservice.InjectFailed(OnFailedSeedTrade);
            tradeMicroservice.InjectFailed(OnFailedTechnologyTrade);
            foreach (var trade in _resourceTrades) trade.Initialization(tradeMicroservice);
            foreach (var trade in _buildingTrade) trade.Initialization(tradeMicroservice);
            foreach (var trade in _technologyTrade) trade.Initialization(tradeMicroservice);
            foreach (var trade in _seedTrade) trade.Initialization(tradeMicroservice);
        }

        internal void InjectResource(List<Resource> resources, ResourceRepository repository)
        {
            Resources = new Dictionary<string, IResource>();
            foreach (var rTechnology in resources)
            {
                var temped = rTechnology.Data;
                temped.Temp = this;
                temped.Repository = repository;
                Resources.Add(temped.Title, temped);
            }
        }

        #region ResourceTradeByTradeMap
        public void ResourceTradeByTradeMap(ResourceTrade trade, Dictionary<IResource, int> tradeMap,int amount, bool all = false)
        {
            int amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0) OnSuccessfullyResourceTrade?.Invoke(trade, amount, all);
            else OnFailedResourceTrade?.Invoke(trade, amount, all);
        }
        public void BuildingTradeByTradeMap(BuildingTrade trade, Dictionary<IResource, int> tradeMap, int amount, bool all = false)
        {
            var amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0) OnSuccessfullyBuildingTrade?.Invoke(trade, amount, all);
            else OnFailedBuildingTrade?.Invoke(trade, amount, all);
        }
        public void TechnologyTradeByTradeMap(TechnologyTrade trade, Dictionary<IResource, int> tradeMap,int amount, bool all = false)
        {
            int amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0) OnSuccessfullyTechnologyTrade?.Invoke(trade, amount, all);
            else OnFailedTechnologyTrade?.Invoke(trade, amount, all);
        }
        public void SeedTradeByTradeMap(SeedTrade trade, Dictionary<IResource, int> tradeMap,int amount, bool all = false)
        {
            int amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0) OnSuccessfullySeedTrade?.Invoke(trade, amount, all);
            else OnFailedSeedTrade?.Invoke(trade, amount, all);
        }
        #endregion
        private int TradeResource(Dictionary<IResource, int> tradeMap, int amount, bool all = false)
        {
            if (!CanTradeResource(tradeMap)) return 0;
            if (all) return TradeAll(tradeMap);
            foreach (var tradeMapElement in tradeMap)
            {
            }
            return 0;
        }
        private bool CanTradeResource(Dictionary<IResource, int> tradeMap)
        {
            foreach (var tradeComponent in tradeMap)
                if (!CanPayAmounts(tradeComponent.Key, tradeComponent.Value))
                    return false;
            return true;
        }
        private bool CanTradeAmountResource(Dictionary<IResource, int> tradeMap, int amount)
        {
            foreach (var tradeComponent in tradeMap)
                if (!CanPayAmounts(tradeComponent.Key, tradeComponent.Value * amount))
                    return false;
            return true;
        }
        private int TradeAll(Dictionary<IResource, int> tradeMap)
        {
            int max = MaxTradeAmount(tradeMap);
            foreach (var tradeMapElement in tradeMap) ProvidePayAmounts(tradeMapElement.Key, tradeMapElement.Value * max);
            return max;
        }
        internal int MaxTradeAmount(Dictionary<IResource, int> trade)
        {
            long maxAmount = 0;
            int iteration = 0;
            foreach (var tradeEntry in trade)
            {
                var value = GetAmount(tradeEntry.Key.Title);
                var startPrice = tradeEntry.Value;
                var resource = value / startPrice;
                maxAmount = iteration == 0 ? resource : Math.Min(maxAmount, resource);
                iteration++;
            }
            return (int)maxAmount;
        }
        internal bool IsTradAble(Dictionary<IResource, int> tradeMap) => CanTradeResource(tradeMap);
        #region Temp Signature

        protected override long SummedAmounts(long a, long b) { return a + b; }
        protected override long SubtractionAmounts(long a, long b) { return a - b; }
        protected override string GetIdentifierByEncoded(ResourceEncoded encoded) { return encoded.Title; }
        protected void ProvideAddAmounts(IResource resource, int amount) { SummedAmountData(resource.Title, amount); }
        protected void ProvidePayAmounts(IResource resource, int amount) { SubtractionAmountData(resource.Title, amount); }
        protected bool CanPayAmounts(IResource resource, int amount) { return CanAddAmounts(resource) && DataByIdentifier[resource.Title] >= amount; }
        protected bool CanAddAmounts(IResource resource) { return DataByIdentifier.ContainsKey(resource.Title); }
        internal ResourceTemp(IIdentifier<string, ResourceEncoded> identifier) : base(identifier) { }
        
        #endregion
    }
}