using System;
using System.Collections.Generic;
using System.Linq;
using Game.CallBacks;
using Game.Cells;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.DataStructures.Technologies;
using Game.DataStructures.Trades;
using Game.DataStructures.UI;
using Game.RepositoryEngine.Abstraction;
using Game.Services.CellServices;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Services.StorageServices.Microservice;
using Game.Utility;
using UnityEngine;

namespace Game.RepositoryEngine.ResourcesRepository
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
        // #region Internal Actions
        // private event Action<ResourceTrade, int, bool> OnFailedResourceTrade;
        // private event Action<BuildingTrade, int, bool> OnFailedBuildingTrade;
        // private event Action<SeedTrade, int, bool> OnFailedSeedTrade;
        // private event Action<TechnologyTrade, int, bool> OnFailedTechnologyTrade;
        // private event Action<ResourceTrade, int, bool> OnSuccessfullyResourceTrade;
        // private event Action<BuildingTrade, int, bool> OnSuccessfullyBuildingTrade;
        // private event Action<SeedTrade, int, bool> OnSuccessfullySeedTrade;
        // private event Action<TechnologyTrade, int, bool> OnSuccessfullyTechnologyTrade;
        // #endregion
        private TradeMicroservice _tradeMicroservice;
        private CellService _cellService;
        public bool IsInit { get; set; }
        public GameObject TargetObject { get; set; }
        
        public Action<Port, ResourceTempedCallBack> OnCallBackInvocation { get; set; } = (port, callback) =>
        {
            Debugger.Logger($"[{port}]=>callback:{callback.Resource.Title}=>{callback.Value}", ContextDebug.Session, Process.Update);
        };

        
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
                trade.Inject(_cellService);
                _buildingTrade.Add(trade);
                GetBuildingTrade.Add(trade.@into.Data.ExternalName, trade);
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
                trade.Inject(_cellService);
                _seedTrade.Add(trade);
                GetSeedTrade.Add(trade.into.title, trade);
            }
        }
        internal void InjectionResource(List<UIResource> resourcesUI)
        {
            foreach (var resourceUI in resourcesUI) 
                CommonToUIResources.Add(resourceUI.resource.title, resourceUI);
        }
        public void InjectionResource(List<Seed> resourcesUI, ResourceRepository repository)
        {
            foreach (var rTechnology in resourcesUI)
            {
                var temped = rTechnology.Data;
                temped.Temp = this;
                temped.Repository = repository;
                Resources.Add(rTechnology.title, temped);
            }
        }
        internal void InjectionResource(List<Resource> resources, ResourceRepository repository)
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
        internal void InjectionInMicroservice(TradeMicroservice tradeMicroservice, CellService cellService ,GameObject target)
        {
            _tradeMicroservice = tradeMicroservice;
            _cellService = cellService;
            OnEncodeChangeData = EncodeChange;
            OnEncodeChangeElement = EncodeChangeElement;
            
            DatabaseResourceProvider.CallBackTunneling<ResourceTempedCallBack>(this);
            Proxy.Connect<CellResourcePackagingProvider, CellResourcePackaging, CellResourceFarmer>(FarmResource);

            tradeMicroservice.OnTryResourceTrade += ResourceTradeByTradeMap;
            tradeMicroservice.OnTryBuildingTrade += BuildingTradeByTradeMap;
            tradeMicroservice.OnTryTechnologyTrade += TechnologyTradeByTradeMap;
            tradeMicroservice.OnTrySeedTrade += SeedTradeByTradeMap;
        }
        
        #region ResourceTradeByTradeMap

        public bool CheckTechnology( List<Technology>technologies)
        {
            if (technologies.Any((tech) => tech.Status() == false))
            {
                return false;
            }
            return true;
        }

        public void ResourceTradeByTradeMap(ResourceTrade trade, Dictionary<IResource, int> tradeMap,int amount, bool all = false)
        {
            if (!CheckTechnology(trade.technologyCondition))
            {
                Debugger.Logger($"{trade.TradeName}:Technology unavailable");
                _tradeMicroservice.FailedInvoke(trade, amount, all);
                return;
            }
            int amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0)
            {
                _tradeMicroservice.SuccessfullyInvoke(trade, amount, all);
                ProvideAddAmounts(trade.@into.Data, amountResource);
            }
            else
            {
                _tradeMicroservice.FailedInvoke(trade, amount, all);
            }
        }
        public void BuildingTradeByTradeMap(BuildingTrade trade, Dictionary<IResource, int> tradeMap, int amount, bool all = false)
        {
            if (!CheckTechnology(trade.technologyCondition))
            {
                Debugger.Logger($"{trade.TradeName}:Technology unavailable");
                _tradeMicroservice.FailedInvoke(trade, amount, all);
                return;
            }
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
            var currentTrade = trade.Trades[trade.CurrentStage()];
            if (!CheckTechnology(currentTrade.technologyCondition))
            {
                Debugger.Logger($"{trade.TradeName} [Stage:{trade.CurrentStage()}]:Technology unavailable");
                _tradeMicroservice.FailedInvoke(trade, amount, all);
                return;
            }
            int amountResource = TradeResource(tradeMap, amount, all);
            if (amountResource != 0)
            {
                _tradeMicroservice.SuccessfullyInvoke(trade, amount, all);
                // ProvideAddAmounts(trade.@into.Data, 1);
            }
            else
                _tradeMicroservice.FailedInvoke(trade, amount, all);
        }
        #endregion

        private void EncodeChangeElement(string arg1, ResourceEncoded resourceEncoded)
        {
            var title = arg1;
            var resource = Resources[resourceEncoded.Title];
            var value = Data[resourceEncoded];
            OnCallBackInvocation?.Invoke(Porting.Type<ResourceTempedCallBack>(),new ResourceTempedCallBack(title, resource, value));
        }
        private void EncodeChange(string arg1, long arg2)
        {
        }
        private int TradeResource(Dictionary<IResource, int> tradeMap, int amount, bool all = false)
        {
            if (!CanTradeResource(tradeMap)) return 0;
            if (all) return TradeAll(tradeMap);
            foreach (var tradeMapElement in tradeMap)
                    ProvidePayAmounts(tradeMapElement.Key, tradeMapElement.Value);
            return amount;
        }
        private bool CanTradeResource(Dictionary<IResource, int> tradeMap)
        {
            foreach (var tradeComponent in tradeMap)
                if (!CanPayAmounts(tradeComponent.Key, tradeComponent.Value))
                {
                    Debugger.Logger($"{tradeComponent.Key}:{tradeComponent.Value} == {Data[EncodeByIdentifier[tradeComponent.Key.Title]]}");
                    return false;
                }
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
            foreach (var tradeMapElement in tradeMap) 
                ProvidePayAmounts(tradeMapElement.Key, tradeMapElement.Value * max);
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
        private void FarmResource(Port type, CellResourcePackaging farmer) => ProvideAddAmounts(farmer.Resource, farmer.Value);

        #region Temp Signature

        protected override long SummedAmounts(long a, long b) { return a + b; }
        protected override long SubtractionAmounts(long a, long b) { return a - b; }
        protected override string GetIdentifierByEncoded(ResourceEncoded encoded) { return encoded.Title; }
        protected void ProvideAddAmounts(IResource resource, int amount) { SummedAmountData(resource.Title, amount); }
        protected void ProvidePayAmounts(IResource resource, int amount) { SubtractionAmountData(resource.Title, amount); }
        protected bool CanPayAmounts(IResource resource, int amount) 
        { 
            return  Data[EncodeByIdentifier[resource.Title]] >= amount; 
        }
        protected bool CanAddAmounts(IResource resource) { return DataByIdentifier.ContainsKey(resource.Title); }
        internal ResourceTemp(IIdentifier<string, ResourceEncoded> identifier) : base(identifier) { }
        
        #endregion


    }
}