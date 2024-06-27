using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallBackTrade;
using Game.Cells;
using Game.DataStructures.UI;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu.BuildingCellMenuList;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuBuilding : MonoBehaviour
    {
        [SerializeField] private UICellListBuilding listBuilding;
        public Transform target;
        public bool status = false;
        public List<UICellContainer> containers;
        private IClickCallback<MenuTypes> _callbackClose;
        private IClickCallback<MenuTypes> _callbackOpen;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration( OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            _callbackClose = new ClickCallback<MenuTypes>();
            _callbackOpen = new ClickCallback<MenuTypes>();
            MenuTypesExitProvider.CallBackTunneling<UIMenuBuilding>(_callbackClose);
            MenuTypesOpenProvider.CallBackTunneling<UIMenuBuilding>(_callbackOpen);
            Proxy.Connect<BuildingTradeProvider,BuildingTradeCallBack>(OnBuildWasBay, Port.TradeSuccessfully);
            Proxy.Connect<BuildingTradeProvider,BuildingTradeCallBack>(OnBuildWasNoBy, Port.TradeFailed);
            Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>(OpenMenu);
            return Task.CompletedTask;
        }

        private void OnBuildWasNoBy(Port port, BuildingTradeCallBack callBack) => CloseMenu();
        private void OnBuildWasBay(Port port, BuildingTradeCallBack callBack) => CloseMenu();

        public void CloseMenu()
        {
            status = false;
            target.gameObject.SetActive(false);
            Debugger.Logger("CloseMenu UIMenuResource Menu", ContextDebug.Menu, Process.Action);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<UIMenuBuilding>(), MenuTypes.ResourceMenu);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<UIMenuBuilding>(), MenuTypes.BuildingMenu);
        }
        public void OpenMenu()
        {
            status = true;
            target.gameObject.SetActive(true);
            Debugger.Logger("OpenMenu UIMenuResource Menu", ContextDebug.Menu, Process.Action);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<UIMenuBuilding>(), MenuTypes.BuildingMenu);
        }
        
        private void OpenMenu(Port type, Cell cell)
        {
            OpenMenu();
            var container = cell.container.Data;
            OpenMenuWithBase(container.Initial.ExternalName);
        }
        private void OpenMenuWithBase(string nameOfContainer)
        {
            var element = containers.Find(container => container.Data.Container.initial.externalName == nameOfContainer);
            if (element != null)
            {
                listBuilding.UpdateData(element);
            }
        }
    }
}