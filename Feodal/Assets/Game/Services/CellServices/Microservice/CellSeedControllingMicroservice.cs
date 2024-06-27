using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Game.CallBacks.CallBackTrade;
using Game.Cells;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.RepositoryEngine.ResourcesRepository;
using Game.Services.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Services.StorageServices;
using Game.UI.Abstraction;
using Game.UI.Menu;
using Game.Utility;

namespace Game.Services.CellServices.Microservice
{
    public class CellSeedControllingMicroservice : AbstractMicroservice<CellService>, ICallBack<Seed>
    {
        private StorageService _microserviceStorage;
        private CellService _microserviceCell;
        private ResourceTemp _resourceTemp;
        public List<Seed> seeds;
        private Dictionary<string, long> _seedToAmountUsage;
        private CellMap _map;
        public Action<Port, Seed> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
        private IClickCallback<IUICellContainer> _callbackContainer;

        protected override Task OnStart(IProgress<float> progress)
        {
            return Task.CompletedTask;
        }

        public void ContainerClick(IUICellContainer cellContainerElement)
        {
            _callbackContainer.OnCallBackInvocation?.Invoke(Porting.Type<UIMenuBuilding>(),cellContainerElement);
        }

        protected override Task OnAwake(IProgress<float> progress)
        {
            _callbackContainer = new ClickCallback<IUICellContainer>();
            _seedToAmountUsage = new Dictionary<string, long>();
            SessionLifeStyleManager.AddLifeIteration(InstanceSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeClose);
            DatabaseSeedProvider.CallBackTunneling<Seed>(this);
            return Task.CompletedTask;
        }
        private Task InstanceSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            _microserviceCell = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            _microserviceStorage = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<StorageService>();
            
            Proxy.Connect<SeedTradeProvider,SeedTradeCallBack>(OnSeedWasBay, Port.TradeSuccessfully);
            UICellContainerProvider.CallBackTunneling<UIMenuBuilding>(_callbackContainer);

            _resourceTemp = _microserviceStorage.GetResourceTemp();
            if (_microserviceCell.IsMapCellInitial)
                MicroserviceCellMapInitial(_microserviceCell.cellMap);
            else
                _microserviceCell.OnCellMapInitial += MicroserviceCellMapInitial;
            return Task.CompletedTask;
        }
        private void MicroserviceCellCellChange(ICellState from, ICellState to, Cell into) => UpdateAmountByCellSeed(@into);
        private void MicroserviceCellCellDestroy(Cell into) => UpdateAmountByCellSeed(@into);
        private void MicroserviceCellCellCreate(Cell into) => UpdateAmountByCellSeed(@into);
        public bool CanCreateNewSeed(Seed seed) =>  _resourceTemp.GetValueAmount(seed.Title) > _seedToAmountUsage[seed.Data.Title];

        private void UpdateAmountByCellSeed(Cell @into)
        {
            var seedToUpdate = @into.container.seed;
            var amount = _map.GetCellCount(seedToUpdate);
            _seedToAmountUsage[seedToUpdate.Data.Title] = amount;
            Debugger.Logger($"UsageCellSeed:{seedToUpdate.Data.Title}  amount ={amount}", Process.Update);
        }
        
        private void MicroserviceCellMapInitial(CellMap map)
        {
            _microserviceCell.OnCellCreate += MicroserviceCellCellCreate;
            _microserviceCell.OnCellDestroy += MicroserviceCellCellDestroy;
            _microserviceCell.OnCellChange += MicroserviceCellCellChange;
            _map = map;
            foreach (var seedKey in seeds)
            {
                var amount = _map.GetCellCount(seedKey);
                _seedToAmountUsage.Add(seedKey.Data.Title, amount);
            }
        }
        private void OnSeedWasBay(Port port, SeedTradeCallBack successfullyCellTrade)
        {
            Debugger.Logger($"Cell was bay {successfullyCellTrade.Trade.@into.title}", ContextDebug.Session, Process.Update);
            var seed = successfullyCellTrade.Trade.@into.Data;
            _resourceTemp.AddAmount(successfullyCellTrade.Trade.@into.title, 1);
            OnCallBackInvocation?.Invoke(Porting.Type<Seed>(),successfullyCellTrade.Trade.@into);
        }
    }
}