using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.DataStructures.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu.ResourceListMenu;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuResource : MonoBehaviour
    {
        [SerializeField] private UIResourceListController controller;
        public Transform target;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            controller.OnTradeFindAndProcessed += ControllerTradeFindAndProcessed;
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            Proxy.Connect<UniversalResourceProvider, IResource, UIMenuResource>(OnClickedByUniversalResource);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UIMenuResource>(OnClickedByMenuExit);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(OnClickedByMenuExit);
            return Task.CompletedTask;
        }
        private void CloseMenu()
        {
            controller.Clear();
            target.gameObject.SetActive(false);
        }
        private void OnClickedByMenuExit(Port type, MenuTypes obj)
        {
            if (obj == MenuTypes.ResourceMenu)
            {
                CloseMenu();
            }
        }
        private void ControllerTradeFindAndProcessed()
        {
            CloseMenu();
        }
        private void OnClickedByUniversalResource(Port type, IResource resource)
        {
            target.gameObject.SetActive(true);
            controller.Clear();
            controller.ViewList(controller.ResourcesListCompare[resource]);
        }
    }
}