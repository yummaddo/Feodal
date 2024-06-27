using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Typing;
using Game.UI.Menu.ResourceListMenu;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuResource : MonoBehaviour
    {
        [SerializeField] private UIResourceListController controller;
        public Transform target;
        private IClickCallback<MenuTypes> _callbackClose;
        private IClickCallback<MenuTypes> _callbackOpen;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            controller.OnTradeFindAndProcessed += ControllerTradeFindAndProcessed;
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            _callbackClose = new ClickCallback<MenuTypes>();
            _callbackOpen = new ClickCallback<MenuTypes>();
            MenuTypesExitProvider.CallBackTunneling<UIMenuResource>(_callbackClose);
            MenuTypesOpenProvider.CallBackTunneling<UIMenuResource>(_callbackOpen);
            return Task.CompletedTask;
        }
        public void CloseMenu()
        {
            controller.Clear();
            Debugger.Logger("CloseMenu UIMenuResource Menu", ContextDebug.Menu, Process.Action);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<ButtonOpenMenuCallBack>(), MenuTypes.ResourceMenu);
            target.gameObject.SetActive(false);
        }
        public void OpenMenu()
        {
            Debugger.Logger("OpenMenu UIMenuResource Menu", ContextDebug.Menu, Process.Action);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<ButtonOpenMenuCallBack>(), MenuTypes.ResourceMenu);
            target.gameObject.SetActive(true);
            controller.Clear();
        }
        private void ControllerTradeFindAndProcessed()
        {
            CloseMenu();
        }
        public void OnClickedByUniversalResource(Resource resource)
        {
            OpenMenu();
            controller.ViewList(controller.ResourcesListCompare[resource.Data]);
        }
        private void OnClickedByUniversalResource(IResource resource)
        {
            OpenMenu();
            controller.ViewList(controller.ResourcesListCompare[resource]);
        }
    }
}