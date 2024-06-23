using System;
using Game.Core;
using Game.Core.Cells;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuContainer : MonoBehaviour
    {
        public Transform target;
        public SimpleMenuTypesOpenCallBack menuTypesOpenedCallBack;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<CellProvider, Cell, UIMenuContainer>(OnUpdateSelect);
            Proxy.Connect<CellAddDetectorProvider, CellAddDetector, CellAddDetector>(PlayerClickedByAddCellObject);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UIMenuContainer>       (ExitMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(ExitMenu);
        }
        private void OnUpdateSelect(Port type, Cell obj) => CloseMenu(); 
        private void PlayerClickedByAddCellObject(Port type, CellAddDetector obj) => OpenMenu();
        
        private void ExitMenu(Port type, MenuTypes obj)
        {
            if (obj == MenuTypes.ContainerMenu || obj == MenuTypes.BuildingMenu) CloseMenu();
        }
        
        private void OpenMenu()
        {
            menuTypesOpenedCallBack.OnCallBackInvocation?.Invoke(Porting.Type<ButtonOpenMenuCallBack>() ,MenuTypes.ContainerMenu);
            target.gameObject.SetActive(true);
            
            Debugger.Logger("OpenMenu ContainerMenu Menu", ContextDebug.Menu, Process.Action);
        }
        private void CloseMenu()
        {
            target.gameObject.SetActive(false);
            Debugger.Logger("CloseMenu ContainerMenu Menu", ContextDebug.Menu, Process.Action);
        }
    }
}