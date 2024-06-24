using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.DataStructures;
using Game.Core.Typing;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Proxies;
using Game.Services.Proxies.Abstraction;
using Game.Services.Proxies.CallBackTrade;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.Services.Proxies.Providers.TradeProviders;
using Game.Services.Storage;
using Game.Services.Storage.ResourcesRepository;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Services.CellControlling.Microservice
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
        
        protected override void OnStart() { }
        protected override void ReStart() { }
        protected override void Stop() { }
        protected override void OnAwake()
        {
            SessionStateManager.Instance.OnSceneAwakeClose += InstanceSceneAwakeMicroServiceSession;
            _microserviceCell = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
            _microserviceStorage = SessionStateManager.Instance.ServiceLocator.Resolve<StorageService>();
            
            Proxy.Connect<BuildingTradeProvider,BuildingTradeCallBack>(OnBuildWasBay, Port.TradeSuccessfully);
            CellStateProvider.CallBackTunneling<CellState>(this);
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
        
        private void InstanceSceneAwakeMicroServiceSession()
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
        }

        private void MicroserviceCellMapInitial(CellMap map)
        {
        }

        public GameObject TargetObject { get; set; }
    }
}