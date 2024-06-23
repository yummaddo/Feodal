using System;
using Game.Core.Abstraction;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
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
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
            controller.OnTradeFindAndProcessed += ControllerTradeFindAndProcessed;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<UniversalResourceProvider, IResource,UIMenuResource>(OnClickedByUniversalResource);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(OnClickedByMenuExit);
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