using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu.TechnologyMenu;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UITechnologyMenu :  MonoBehaviour
    {
        [SerializeField] private UITechnologyController controller;
        private IClickCallback<MenuTypes> _callbackClose;
        private IClickCallback<MenuTypes> _callbackOpen;
        public bool status = false;
        private void Awake()
        {

            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        public void OnTechnologyButtonClick()
        {
            if (status)
                CloseMenu();
            else 
                OpenMenu();
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            _callbackClose = new ClickCallback<MenuTypes>();
            _callbackOpen = new ClickCallback<MenuTypes>();
            MenuTypesExitProvider.CallBackTunneling<UITechnologyMenu>(_callbackClose);
            MenuTypesOpenProvider.CallBackTunneling<UITechnologyMenu>(_callbackOpen);
            
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UITradeMenu>(ExternalCallToOpen);
            return Task.CompletedTask;
        }
        private void ExternalCallToOpen(Port port, MenuTypes type)
        {
            if (type != MenuTypes.Technology) 
                return;
            OpenMenu();
        }
        public void CloseMenu()
        {
            status = false;
            controller.technologyRoot.SetActive(false);
            Debugger.Logger("CloseMenu UITechnologyMenu Menu", ContextDebug.Menu, Process.Action);
            _callbackClose.OnCallBackInvocation?.Invoke(Porting.Type<UITechnologyMenu>(), MenuTypes.Technology);
        }
        public void OpenMenu()
        {
            status = true;
            controller.technologyRoot.SetActive(true);
            Debugger.Logger("OpenMenu UITechnologyMenu Menu", ContextDebug.Menu, Process.Action);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<UITechnologyMenu>(), MenuTypes.Technology);
        }
    }
}