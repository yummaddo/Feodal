using System.Collections.Generic;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Proxies;
using Game.Services.Proxies.CallBackTrade;
using Game.Services.Proxies.Providers.TradeProviders;
using Game.Services.Storage;
using Game.Services.Storage.ResourcesRepository;

namespace Game.Services.CellControlling.Microservice
{
    public class CellSeedControllingMicroservice : AbstractMicroservice<CellService>
    {
        private StorageService _microserviceStorage;
        private CellService _microserviceCell;
        private ResourceTemp _resourceTemp;
        public List<Seed> seeds;
        private Dictionary<string, long> _seedToAmount;
        private Dictionary<string, long> _seedToAmountUsage;
        private CellMap _map;
        protected override void OnStart() { }
        protected override void ReStart() { }
        protected override void Stop() { }
        protected override void OnAwake()
        {
            _seedToAmount = new Dictionary<string, long>();
            _seedToAmountUsage = new Dictionary<string, long>();
            SessionStateManager.Instance.OnSceneAwakeClose+= InstanceSceneAwakeMicroServiceSession;
        }
        private void InstanceSceneAwakeMicroServiceSession()
        {
            // _microserviceTrades = SessionStateManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            _microserviceCell = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
            _microserviceStorage = SessionStateManager.Instance.ServiceLocator.Resolve<StorageService>();
            Proxy.Connect<SeedTradeProvider,SeedTradeCallBack>(OnSeedWasBay, Port.TradeSuccessfully);
            _resourceTemp = _microserviceStorage.GetResourceTemp();
            if (_microserviceCell.IsMapCellInitial)
                MicroserviceCellMapInitial(_microserviceCell.cellMap);
            else
                _microserviceCell.OnCellMapInitial += MicroserviceCellMapInitial;
        }
        private void MicroserviceCellCellChange(ICellState from, ICellState to, Cell into) => UpdateAmountByCellSeed(@into);
        private void MicroserviceCellCellDestroy(Cell into) => UpdateAmountByCellSeed(@into);
        private void MicroserviceCellCellCreate(Cell into) => UpdateAmountByCellSeed(@into);
        public bool CanCreateNewSeed(Seed seed) => _seedToAmount[seed.Data.Title] > _seedToAmountUsage[seed.Data.Title];

        private void UpdateAmountByCellSeed(Cell @into)
        {
            var seedToUpdate = @into.container.seed;
            var amount = _map.GetCellCount(seedToUpdate);
            _seedToAmountUsage[seedToUpdate.Data.Title] = amount;
            Debugger.Logger($"CellSeed:{seedToUpdate.Data.Title}  amount ={amount}", Process.Update);
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
            foreach (var seedKey in seeds)
            {
                var amount =  _resourceTemp.GetValueAmount(seedKey.Data.Title);
                _seedToAmount.Add(seedKey.Data.Title, amount);
            }
        }
        private void OnSeedWasBay(Port port, SeedTradeCallBack successfullyCellTrade)
        {
            Debugger.Logger($"Cell was bay {successfullyCellTrade.Trade.@into.title}", ContextDebug.Session, Process.Update);
            var seed = successfullyCellTrade.Trade.@into.Data;
            _resourceTemp.AddAmount(seed.Title, successfullyCellTrade.Amount);
            _seedToAmount[seed.Title] = _resourceTemp.GetValueAmount(seed.Title);
        }
    }
}