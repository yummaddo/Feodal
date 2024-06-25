using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallBackTrade;
using Game.Cells;
using Game.DataStructures;
using Game.RepositoryEngine.ResourcesRepository;
using Game.Services.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Services.StorageServices;
using Game.Typing;
using Game.UI.Menu;
using Game.Utility;
using UnityEngine;

namespace Game.Services.CellServices.Microservice
{
    public class CellBuildControllingMicroservice : AbstractMicroservice<CellService>, ICallBack<CellState>
    {
        private StorageService _microserviceStorage;
        private CellService _microserviceCell;
        private ResourceTemp _resourceTemp;
        private CellMap _map;
        private Action<Port, MenuTypes> _onCallBackInvocation;
        public Action<Port, CellState> OnCallBackInvocation { get; set; }
        private IClickCallback<MenuTypes> _menuExit;
        public bool IsInit { get; set; }
        
        protected override Task OnStart(IProgress<float> progress) {return Task.CompletedTask; }
        protected override Task OnAwake(IProgress<float> progress)
        {
            SessionLifeStyleManager.AddLifeIteration(InstanceSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeClose);
            _microserviceCell = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            _microserviceStorage = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<StorageService>();
            Proxy.Connect<BuildingTradeProvider,BuildingTradeCallBack>(OnBuildWasBay, Port.TradeSuccessfully);
            CellStateProvider.CallBackTunneling<CellState>(this);
            return Task.CompletedTask;
        }
        
        private void OnBuildWasBay(Port port, BuildingTradeCallBack successfullyCellTrade)
        {
            if (successfullyCellTrade.Result != TradeCallBackResult.Successfully) return;
            Debugger.Logger($"Build was bay {successfullyCellTrade.Trade.Into.externalName}", ContextDebug.Session, Process.Update);
            var cellState = successfullyCellTrade.Trade.Into;
            OnCallBackInvocation?.Invoke(Porting.Type<CellState>(),cellState);
            _menuExit.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), MenuTypes.Technology);
            _menuExit.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), MenuTypes.TradeMenu);
            _menuExit.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), MenuTypes.BuildingMenu);
        }
        
        private Task InstanceSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            _menuExit = new ClickCallback<MenuTypes>();
            MenuTypesExitProvider.CallBackTunneling<ButtonExitMenuCallBack>(_menuExit);
            _resourceTemp = _microserviceStorage.GetResourceTemp();
            if (_microserviceCell.IsMapCellInitial)
            {
                MicroserviceCellMapInitial(_microserviceCell.cellMap);
            }
            else
            {
                _microserviceCell.OnCellMapInitial += MicroserviceCellMapInitial;
            }
            return Task.CompletedTask;
        }

        private void MicroserviceCellMapInitial(CellMap map)
        {
        }

        public GameObject TargetObject { get; set; }
    }
}