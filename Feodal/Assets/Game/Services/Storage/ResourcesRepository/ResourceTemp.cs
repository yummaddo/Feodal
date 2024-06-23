using System;
using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Trades;
using Game.Core.DataStructures.UI.Data;
using Game.Services.Proxies;
using Game.Services.Proxies.Abstraction;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.Providers;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.Microservice;
using UnityEngine;

namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceTemp : Temp<ResourceEncoded, string, long>, ICallBack<ResourceTempedCallBack>
    {
        internal Dictionary<string, IResource> Resources = new Dictionary<string, IResource>();
        internal Dictionary<string, ResourceTrade> GetResourceTrade = new Dictionary<string, ResourceTrade>();
        internal Dictionary<string, SeedTrade> GetSeedTrade = new Dictionary<string, SeedTrade>();
        internal Dictionary<string, BuildingTrade> GetBuildingTrade = new Dictionary<string, BuildingTrade>();
        
        internal Dictionary<string, UIResource> CommonToUIResources = new Dictionary<string, UIResource>();
        
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

        public Action<Port, ResourceTempedCallBack> OnCallBackInvocation { get; set; } = (port, callback) =>
        {
            Debugger.Logger($"[{port}]=>callback:{callback.Resource.Title}=>{callback.Value}", ContextDebug.Session, Process.Update);
        };
        public bool IsInit { get; set; }
        public GameObject TargetObject { get; set; }
        
        private void FarmResource(Port type, CellResourcePackaging farmer) => ProvideAddAmounts(farmer.Resource, farmer.Value);
        internal void InjectionTrade(List<ResourceTrade> trades)
        {
            foreach (var trade in trades)
            {
                trade.Initialization(_tradeMicroservice);
                _resourceTrades.Add(trade);
                foreach (var resourceCounter in trade.resourceAmountCondition) GetResourceTrade.Add(resourceCounter.resource.Title,trade);
            }
        }
        internal void InjectionTrade(List<BuildingTrade> trades)
        {
            foreach (var trade in trades)
            {
                trade.Initialization(_tradeMicroservice);
                _buildingTrade.Add(trade);
                GetBuildingTrade.Add(trade.Into.Data.ExternalName, trade);
            }
        }
        internal void InjectionTrade(List<TechnologyTrade> trades)
        {
            foreach (var trade in trades)
            {
                trade.Initialization(_tradeMicroservice);
                _technologyTrade.Add(trade);
            }
        }
        internal void InjectionTrade(List<SeedTrade> trades)
        {
            foreach (var trade in trades)
            {
                trade.Initialization(_tradeMicroservice);
                _seedTrade.Add(trade);
                GetSeedTrade.Add(trade.into.title, trade);
            }
        }
        internal void InjectionResource(List<UIResource> resourcesUI)
        {
            foreach (var resourceUI in resourcesUI) 
                CommonToUIResources.Add(resourceUI.resource.title, resourceUI);
        }
        private TradeMicroservice _tradeMicroservice;
        internal void InjectionInMicroservice(TradeMicroservice tradeMicroservice,GameObject target)
        {
            _tradeMicroservice = tradeMicroservice;
            OnEncodeChangeData = EncodeChange;
            OnEncodeChangeElement = EncodeChangeElement;
            
            DatabaseResourceProvider.CallBackTunneling<ResourceTempedCallBack>(this);
            Proxy.Connect<CellResourcePackagingProvider, CellResourcePackaging, CellResourceFarmer>(FarmResource);

            tradeMicroservice.OnTryResourceTrade += ResourceTradeByTradeMap;
            tradeMicroservice.OnTryBuildingTrade += BuildingTradeByTradeMap;
            tradeMicroservice.OnTryTechnologyTrade += TechnologyTradeByTradeMap;
            tradeMicroservice.OnTrySeedTrade += SeedTradeByTradeMap;
        }
        private void EncodeChange(string arg1, long arg2)
        {
        }
        private void EncodeChangeElement(string arg1, ResourceEncoded resourceEncoded)
        {
            var title = arg1;
            var resource = Resources[resourceEncoded.Title];
            var value = Data[resourceEncoded];
            OnCallBackInvocation?.Invoke(Porting.Type<ResourceTempedCallBack>(),new ResourceTempedCallBack(title, resource, value));
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
            if (amountResource != 0)
            {
                _tradeMicroservice.SuccessfullyInvoke(trade, amount, all);
                ProvideAddAmounts(trade.Into.Data, amountResource);
            }
            else _tradeMicroservice.FailedInvoke(trade, amount, all);
        }
        public void BuildingTradeByTradeMap(BuildingTrade trade, Dictionary<IResource, int> tradeMap, int amount, bool all = false)
        {
            var amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0)
            {
                _tradeMicroservice.SuccessfullyInvoke(trade, amount, all);
                // ProvideAddAmounts(trade.Into)
            }
            else _tradeMicroservice.FailedInvoke(trade, amount, all);
        }
        public void TechnologyTradeByTradeMap(TechnologyTrade trade, Dictionary<IResource, int> tradeMap,int amount, bool all = false)
        {
            int amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0)
            {
                _tradeMicroservice.SuccessfullyInvoke(trade, amount, all);
            }
            else _tradeMicroservice.FailedInvoke(trade, amount, all);
        }
        public void SeedTradeByTradeMap(SeedTrade trade, Dictionary<IResource, int> tradeMap,int amount, bool all = false)
        {
            int amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0)
            {
                _tradeMicroservice.SuccessfullyInvoke(trade, amount, all);
                ProvideAddAmounts(trade.@into.Data, 1);
                trade.stages++;
            }
            else _tradeMicroservice.FailedInvoke(trade, amount, all);
        }
        #endregion
        private int TradeResource(Dictionary<IResource, int> tradeMap, int amount, bool all = false)
        {
            if (all) 
                return TradeAll(tradeMap);
            int resultAmount = 0;
            for (int iter = 0; iter < amount; iter++)
            {
                if (!CanTradeResource(tradeMap)) 
                    return 0;
                foreach (var tradeMapElement in tradeMap)
                    ProvidePayAmounts(tradeMapElement.Key, tradeMapElement.Value);
                resultAmount++;
            }
            Debug.Log($"Result TradeResource: {resultAmount}");
            return resultAmount;
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

        internal long GetValueAmount(string resource)
        {
            return GetAmount(resource);
        }

        internal int MaxTradeAmount(Dictionary<IResource, int> trade)
        {
            long maxAmount = 0;
            int iteration = 0;
            foreach (var tradeEntry in trade)
            {
                var value = GetAmount(tradeEntry.Key.Title);
                var startPrice = tradeEntry.Value;
                long resource = 1;
                if (startPrice != 0)
                    resource = (long)(value / (long)startPrice);
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