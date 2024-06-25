using System;
using System.Threading.Tasks;
using Game.CallBacks.CallBackTrade;
using Game.Cells;
using Game.DataStructures.Technologies.Abstraction;
using Game.RepositoryEngine.TechnologyRepositories;
using Game.Services.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Services.StorageServices;
using Game.Utility;

namespace Game.Services.CellServices.Microservice
{
    public class CellTechnologyControllingMicroservice : AbstractMicroservice<CellService>, ICallBack<ITechnologyStore>
    {
        private StorageService _microserviceStorage;
        private CellService _microserviceCell;
        private TechnologyTemp _technologyTemp;
        public Action<Port, ITechnologyStore> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
        protected override Task OnAwake(IProgress<float> progress)
        {
            SessionLifeStyleManager.AddLifeIteration(InstanceSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            return Task.CompletedTask;
        }
        protected override Task OnStart(IProgress<float> progress) { return Task.CompletedTask;}

        private Task InstanceSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            _microserviceCell = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            _microserviceStorage = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<StorageService>();
            _technologyTemp = _microserviceStorage.GetTechnologyTemp();
            DatabaseTechnologyProvider.CallBackTunneling<ITechnologyStore>(this);
            Proxy.Connect<TechnologyTradeProvider,TechnologyTradeCallBack>(OnTechnologyWasBay, Port.TradeSuccessfully);

            if (_microserviceCell.IsMapCellInitial)
                MicroserviceCellMapInitial(_microserviceCell.cellMap);
            else
                _microserviceCell.OnCellMapInitial += MicroserviceCellMapInitial;
            return Task.CompletedTask;
        }

        private void OnTechnologyWasBay(Port arg1, TechnologyTradeCallBack technologyTrade)
        {
            Debugger.Logger($"Technology was bay {technologyTrade.Trade.@into.Title}", ContextDebug.Session, Process.Update);
            var tech = technologyTrade.Trade.@into;
            _technologyTemp.SetAmountPermament(tech.Title, true);
            OnCallBackInvocation?.Invoke(Porting.Type<ITechnologyStore>(),tech.Data);
        }
        private void MicroserviceCellMapInitial(CellMap map)
        {
            
        }


    }
}