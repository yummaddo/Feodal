using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallbackClick.Simple;
using Game.Cells;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuContainer : MonoBehaviour
    {
        public Transform target;
        private IClickCallback<MenuTypes> _callbackClose;
        private IClickCallback<MenuTypes> _callbackOpen;
        public bool status = false;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            _callbackClose = new ClickCallback<MenuTypes>();
            _callbackOpen = new ClickCallback<MenuTypes>();
            MenuTypesExitProvider.CallBackTunneling<UIMenuContainer>(_callbackClose);
            MenuTypesOpenProvider.CallBackTunneling<UIMenuContainer>(_callbackOpen);
            
            Proxy.Connect<CellProvider, Cell, UIMenuContainer>(OnUpdateSelect);
            Proxy.Connect<CellAddDetectorProvider, CellAddDetector, CellAddDetector>(PlayerClickedByAddCellObject);
            return Task.CompletedTask;
        }
        private void OnUpdateSelect(Port type, Cell obj) => CloseMenu(); 
        
        private void PlayerClickedByAddCellObject(Port type, CellAddDetector obj)
        {
            OpenMenu();
        }

        private void ExitMenu(Port type, MenuTypes obj)
        {
            if (obj == MenuTypes.ContainerMenu || obj == MenuTypes.BuildingMenu) CloseMenu();
        }
        
        public void OpenMenu()
        {
            status = true;
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<UIMenuContainer>() ,MenuTypes.ContainerMenu);
            target.gameObject.SetActive(true);
            Debugger.Logger("OpenMenu ContainerMenu Menu", ContextDebug.Menu, Process.Action);
        }
        public void CloseMenu()
        {
            status = false;
            _callbackClose.OnCallBackInvocation?.Invoke(Porting.Type<UIMenuContainer>() ,MenuTypes.ContainerMenu);
            target.gameObject.SetActive(false);
            Debugger.Logger("CloseMenu ContainerMenu Menu", ContextDebug.Menu, Process.Action);
        }
    }
}