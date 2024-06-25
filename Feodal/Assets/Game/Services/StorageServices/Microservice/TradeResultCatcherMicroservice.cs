using System;
using System.Threading.Tasks;
using Game.CallBacks.CallBackTrade;
using Game.DataStructures.Trades;
using Game.Services.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.Utility;

namespace Game.Services.StorageServices.Microservice
{
    public class TradeResultCatcherMicroservice : AbstractMicroservice<StorageService>
    {
        private TradeMicroservice _tradeMicroservice;
        private BuildingTradeCallBack _onBuildingTrade;
        private ResourceTradeCallBack _onResourceTrade;
        private SeedTradeCallBack _onSeedTrade;
        private TechnologyTradeCallBack _onTechnologyTrade;

        private BuildingTradeCallBack _onBuildingTradeFailed;
        private ResourceTradeCallBack _onResourceTradeFailed;
        private SeedTradeCallBack _onSeedTradeFiled;
        private TechnologyTradeCallBack _onTechnologyTradeFailed;
        protected override Task OnAwake(IProgress<float> progress)
        {
            _onBuildingTrade = new BuildingTradeCallBack();
            _onResourceTrade = new ResourceTradeCallBack();
            _onSeedTrade = new SeedTradeCallBack();
            _onTechnologyTrade = new TechnologyTradeCallBack();
            
            _onBuildingTradeFailed = new BuildingTradeCallBack();
            _onResourceTradeFailed = new ResourceTradeCallBack();
            _onSeedTradeFiled = new SeedTradeCallBack();
            _onTechnologyTradeFailed = new TechnologyTradeCallBack();
            SessionLifeStyleManager.AddLifeIteration(ConnectionToTradeEvents,  SessionLifecycle.OnSceneAwakeClose);
            return Task.CompletedTask;
        }

        protected override Task OnStart(IProgress<float> progress)
        {
            return Task.CompletedTask;
        }

        private void TradeMicroserviceFailedResource(ResourceTrade trade, int i, bool arg3)
        {
            _onResourceTradeFailed.UpdateTradeCallBack(i, trade, false);
            _onResourceTradeFailed.OnCallBackInvocation?.Invoke(Porting.Type(_onResourceTradeFailed.Result),_onResourceTradeFailed);
            Debugger.Logger("[TradeMicroservice] OnFailedResourceTrade", Process.Action);

        }
        private void TradeMicroserviceFailedTechnologyTrade(TechnologyTrade trade, int i, bool arg3)
        {
            _onTechnologyTradeFailed.UpdateTradeCallBack(i, trade,false);
            _onTechnologyTradeFailed.OnCallBackInvocation?.Invoke(Porting.Type(_onTechnologyTradeFailed.Result),_onTechnologyTradeFailed);
            Debugger.Logger("[TradeMicroservice] OnFailedTechnologyTrade", Process.Action);
        }
        private void TradeMicroserviceFailedBuildTrade(BuildingTrade trade, int i, bool arg3)
        {
            _onBuildingTradeFailed.UpdateTradeCallBack(i,trade, false);
            _onBuildingTradeFailed.OnCallBackInvocation?.Invoke(Port.TradeFailed,_onBuildingTradeFailed);
            Debugger.Logger("[TradeMicroservice] OnFailedBuildingTrade", Process.Action);
        }
        private void TradeMicroserviceFailedSeedTrade(SeedTrade trade, int i, bool arg3)
        {
            _onSeedTradeFiled.UpdateTradeCallBack(i, trade,false);
            _onSeedTradeFiled.OnCallBackInvocation?.Invoke(Porting.Type(_onSeedTradeFiled.Result), _onSeedTradeFiled);
            Debugger.Logger("[TradeMicroservice] OnFailedSeedTrade", Process.Action);
        }
        
        private Task ConnectionToTradeEvents(IProgress<float> progress)
        {
            _tradeMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            BuildingTradeProvider.CallBackTunneling(_onBuildingTrade, Port.TradeSuccessfully);
            ResourceTradeProvider.CallBackTunneling(_onResourceTrade, Port.TradeSuccessfully);
            SeedTradeProvider.CallBackTunneling(_onSeedTrade, Port.TradeSuccessfully);
            TechnologyTradeProvider.CallBackTunneling(_onTechnologyTrade, Port.TradeSuccessfully);
            
            BuildingTradeProvider.CallBackTunneling(_onBuildingTradeFailed, Port.TradeFailed);
            ResourceTradeProvider.CallBackTunneling(_onResourceTradeFailed, Port.TradeFailed);
            SeedTradeProvider.CallBackTunneling(_onSeedTradeFiled, Port.TradeFailed);
            TechnologyTradeProvider.CallBackTunneling(_onTechnologyTradeFailed, Port.TradeFailed);
            _tradeMicroservice.OnFailedBuildingTrade += TradeMicroserviceFailedBuildTrade;
            _tradeMicroservice.OnFailedResourceTrade += TradeMicroserviceFailedResource;
            _tradeMicroservice.OnFailedSeedTrade += TradeMicroserviceFailedSeedTrade;
            _tradeMicroservice.OnFailedTechnologyTrade += TradeMicroserviceFailedTechnologyTrade;
            
            _tradeMicroservice.OnSuccessfullyBuildingTrade += TradeMicroserviceSuccessfullyBuildTrade;
            _tradeMicroservice.OnSuccessfullyResourceTrade += TradeMicroserviceSuccessfullyResource;
            _tradeMicroservice.OnSuccessfullySeedTrade += TradeMicroserviceSuccessfullySeedTrade;
            _tradeMicroservice.OnSuccessfullyTechnologyTrade += TradeMicroserviceSuccessfullyTechnologyTrade;
            return Task.CompletedTask;
        }
        private void TradeMicroserviceSuccessfullyResource(ResourceTrade trade, int i, bool arg3)
        {
            Debugger.Logger("[TradeMicroservice] OnSuccessfullyResourceTrade", Process.Action);
            _onResourceTrade.UpdateTradeCallBack(i, trade);
            _onResourceTrade.OnCallBackInvocation?.Invoke(Porting.Type(_onResourceTrade.Result),_onResourceTrade);
        }
        private void TradeMicroserviceSuccessfullyTechnologyTrade(TechnologyTrade trade, int i, bool arg3)
        {
            Debugger.Logger("[TradeMicroservice] OnSuccessfullyTechnologyTrade", Process.Action);
            _onTechnologyTrade.UpdateTradeCallBack(i, trade);
            _onTechnologyTrade.OnCallBackInvocation?.Invoke(Port.TradeSuccessfully,_onTechnologyTrade);
        }
        private void TradeMicroserviceSuccessfullyBuildTrade(BuildingTrade trade, int i, bool arg3)
        {
            Debugger.Logger("[TradeMicroservice] OnSuccessfullyBuildingTrade", Process.Action);
            _onBuildingTrade.UpdateTradeCallBack(i,trade, true);
            BuildingTradeProvider.CallBackTunneling(_onBuildingTrade, Port.TradeSuccessfully);
            _onBuildingTrade.OnCallBackInvocation?.Invoke(Port.TradeSuccessfully,_onBuildingTrade);
        }
        private void TradeMicroserviceSuccessfullySeedTrade(SeedTrade trade, int i, bool arg3)
        {
            Debugger.Logger("[TradeMicroservice] OnSuccessfullySeedTrade", Process.Action);
            _onSeedTrade.UpdateTradeCallBack(i, trade);
            _onSeedTrade.OnCallBackInvocation?.Invoke(Port.TradeSuccessfully, _onSeedTrade);
        }
    }
}