using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.Cells;
using Game.Services.InputServices.Microservice;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu.CellSetterMenu;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UICellSetterMenu : MonoBehaviour
    {
        private IClickCallback<MenuTypes> _clickExitCallback;
        private IClickCallback<Cell> _clickCellDeleteContainerCallback;
        private IClickCallback<Cell> _clickCellDeleteBuildingCallback;
        private bool _active = false;
        [SerializeField] private UICellSetterMenuController controller;
        [SerializeField] private GameObject root;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeCloseSession, SessionLifecycle.OnSceneAwakeClose);
            _clickExitCallback                = new ClickCallback<MenuTypes>();
            _clickCellDeleteContainerCallback = new ClickCallback<Cell>();
            _clickCellDeleteBuildingCallback  = new ClickCallback<Cell>();
        }
        
        private Task OnSceneAwakeCloseSession(IProgress<float> progress)
        {
            CellProvider.CallBackTunneling(_clickCellDeleteContainerCallback, Port.CellDeleteContainer);
            CellProvider.CallBackTunneling(_clickCellDeleteBuildingCallback, Port.CellDeleteBuilding);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UICellSetterMenu>(OnExitClick);
            
            Proxy.Connect<CellProvider, Cell>(OnOpenMenu,  Port.SelectionCellBase);
            
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UIMenuBuilding>(SetStatus);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UIMenuContainer>(SetStatus);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, ButtonExitMenuCallBack>(SetStatus);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, CellMap>(SetStatus);
            return Task.CompletedTask;
        }
        private Cell _temp = null;
        private void OnExitClick(Port arg1, MenuTypes arg2) => OnExitClick();
        private void OnExitClick()
        {
            Debugger.Logger("Exit UICellSetterMenu", ContextDebug.Menu, Process.Action);
            _active = false;
            root.SetActive(false); 
        }
        
        private void OnOpenMenu(Port arg1, Cell arg2)
        {
            Debugger.Logger("Open UICellSetterMenu", ContextDebug.Menu, Process.Action);
            if (!_active)
            {
                _active = true;
                _temp = arg2;
                root.SetActive(true);
            }
        }
        
        public void OnRemoveCellClick()
        {
            if (_active)
                _clickCellDeleteContainerCallback.OnCallBackInvocation?.Invoke(Port.CellDeleteContainer, _temp);
            OnExitClick();
        }
        
        public void OnRemoveBuildClick()
        {
            if (_active)
                _clickCellDeleteBuildingCallback.OnCallBackInvocation?.Invoke(Port.CellDeleteBuilding, _temp);
            OnExitClick();
        }
        
        private void SetStatus(Port arg1, MenuTypes arg2)
        {
            _active = false;
            _clickExitCallback.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), MenuTypes.CellSetterMenu);
            root.SetActive(false);
        }
    }
}