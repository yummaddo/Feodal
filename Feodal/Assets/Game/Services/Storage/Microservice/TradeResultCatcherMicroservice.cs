using System;
using System.Collections.Generic;
using Game.Core.DataStructures.Conditions;
using Game.Core.DataStructures.Trades;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Proxies;
using Game.Services.Proxies.Abstraction;
using Game.Services.Proxies.CallBackTrade;
using Game.Services.Proxies.Providers.TradeProviders;

namespace Game.Services.Storage.Microservice
{
    public class TradeResultCatcherMicroservice : AbstractMicroservice<StorageService>
    {
        private TradeMicroservice _tradeMicroservice;
        private BuildingTradeCallBack _onBuildingTrade;
        private ResourceTradeCallBack _onResourceTrade;
        private SeedTradeCallBack _onSeedTrade;
        private TechnologyTradeCallBack _onTechnologyTrade;
        public bool IsInit { get; set; }
        protected override void OnAwake()
        {
            Service.OnResourceRepositoryInit += ServiceResourceRepositoryInit;
        }
        private void ServiceResourceRepositoryInit()
        {
            _tradeMicroservice = SessionStateManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            BuildingTradeProvider.CallBackTunneling<BuildingTradeCallBack>(_onBuildingTrade);
            ResourceTradeProvider.CallBackTunneling<ResourceTradeCallBack>(_onResourceTrade);
            SeedTradeProvider.CallBackTunneling<SeedTradeCallBack>(_onSeedTrade);
            TechnologyTradeProvider.CallBackTunneling<TechnologyTradeCallBack>(_onTechnologyTrade);

        }
        private void ConnectionToTradeEvents()
        {
            _tradeMicroservice.OnFailedBuildingTrade += delegate(BuildingTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnFailedBuildingTrade", Process.Action);
            };
            _tradeMicroservice.OnFailedResourceTrade += delegate(ResourceTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnFailedResourceTrade", Process.Action);
            };
            _tradeMicroservice.OnFailedSeedTrade += delegate(SeedTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnFailedSeedTrade", Process.Action);
            };
            _tradeMicroservice.OnFailedTechnologyTrade += delegate(TechnologyTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnFailedTechnologyTrade", Process.Action);
            };
            _tradeMicroservice.OnSuccessfullyBuildingTrade += delegate(BuildingTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnSuccessfullyBuildingTrade", Process.Action);
                _onBuildingTrade = new BuildingTradeCallBack(i,trade);
                _onBuildingTrade.OnCallBackInvocation?.Invoke(Porting.Type<BuildingTradeCallBack>(),_onBuildingTrade);
            };
            _tradeMicroservice.OnSuccessfullyResourceTrade += delegate(ResourceTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnSuccessfullyResourceTrade", Process.Action);
                _onResourceTrade = new ResourceTradeCallBack(i, trade);
                _onResourceTrade.OnCallBackInvocation?.Invoke(Porting.Type<ResourceTradeCallBack>(),_onResourceTrade);
            };
            _tradeMicroservice.OnSuccessfullySeedTrade += delegate(SeedTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnSuccessfullySeedTrade", Process.Action);
                _onSeedTrade = new SeedTradeCallBack(i,trade);
                _onSeedTrade.OnCallBackInvocation?.Invoke(Porting.Type<SeedTradeCallBack>(),_onSeedTrade);

            };
            _tradeMicroservice.OnSuccessfullyTechnologyTrade += delegate(TechnologyTrade trade, int i, bool arg3)
            {
                Debugger.Logger("[TradeMicroservice] OnSuccessfullyTechnologyTrade", Process.Action);
                _onTechnologyTrade = new TechnologyTradeCallBack(i, trade);
                _onTechnologyTrade.OnCallBackInvocation?.Invoke(Porting.Type<TechnologyTradeCallBack>(),_onTechnologyTrade);
            };
        }
        protected override void OnStart() { }
        protected override void ReStart() { }
        protected override void Stop() { }
    }
}