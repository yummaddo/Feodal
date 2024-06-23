using System.Collections.Generic;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Core.Typing;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Proxies;
using Game.Services.Proxies.CallBackTrade;
using Game.Services.Proxies.Providers.TradeProviders;
using Game.Services.Storage;
using Game.Services.Storage.Microservice;
using Game.Services.Storage.ResourcesRepository;

namespace Game.Services.CellControlling.Microservice
{
    public class CellSeedControlling : AbstractMicroservice<CellService>
    {
        private TradeMicroservice _microserviceTrades;
        private StorageService _microserviceStorage;
        private CellService _microserviceCell;
        private ResourceTemp _resourceTemp;
        public List<Seed> seeds;

        internal Dictionary<Seed, long> SeedToAmount;
        internal Dictionary<Seed, long> SeedToAmountUsage;

        internal delegate int GetStateInMap(ICellState state);
        internal GetStateInMap MapStates;
        
        protected override void OnAwake()
        {
            _microserviceTrades = SessionStateManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            _microserviceCell = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
            SeedToAmount = new Dictionary<Seed, long>();
            SeedToAmountUsage = new Dictionary<Seed, long>();
            
            _microserviceStorage.OnInit += ServiceResourceRepositoryInit;
            _microserviceCell.OnCellMapInitial += MicroserviceCellMapInitial;
            _resourceTemp = _microserviceStorage.GetResourceTemp();
            
            Proxy.Connect<SeedTradeProvider,SeedTradeCallBack,SeedTradeCallBack>(OnSeedWasBay);
        }

        private void MicroserviceCellMapInitial(CellMap obj) => MapStates = new GetStateInMap(obj.GetCountOfCellState);
        private void OnSeedWasBay(Port arg1, SeedTradeCallBack arg2)
        {
            var seed = arg2.SeedTrade.@into.Data;
            var stages = arg2.SeedTrade.resourceAmountCondition.Count;

        }
        /// <summary>
        /// Get All Data about seed amounts for fictive control and inject to dependency 
        /// </summary>
        private void ServiceResourceRepositoryInit()
        {
            foreach (var resourceKey in _resourceTemp.Resources)
            {
                if (resourceKey.Value.Type == ResourceType.Seed)
                {
                    var find = seeds.Find((seedIter) => seedIter.Title == resourceKey.Key);
                    SeedToAmount.Add(find,_resourceTemp.GetValueAmount(resourceKey.Key));
                }
            }
            foreach (var seedKey in SeedToAmount)
            {
            }
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
    }
}