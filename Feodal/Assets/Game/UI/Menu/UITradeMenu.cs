using Game.Core.Abstraction;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;
using Game.UI.Menu.TradeMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu
{
    public class UITradeMenu : MonoBehaviour
    {
        [SerializeField] private UITradeListController controller;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<UIListResourceElementProvider, UIResourceListElement, UIResourceListElement>(OnClickedBySimpleResource);
            Proxy.Connect<UITechnologyElementProvider, UITechnologyListElement, UITechnologyListElement>(OnClickedByTechnology);

            
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(OnClickedByMenuExit);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, ButtonOpenMenuCallBack>(OnClickedByMenuOpen);
        }
        private void OnClickedBySimpleResource(Port port,UIResourceListElement element)
        {
            controller.View(element);
        }
        private void OnClickedByTechnology(Port port,UITechnologyListElement listElement)
        {
            controller.View(listElement);
        }
        
        private void OnClickedByMenuExit(Port type, MenuTypes obj)
        {
            if (obj == MenuTypes.TradeMenu || obj == MenuTypes.Technology)
            {
                CloseMenu();
            }
        }
        private void OnClickedByMenuOpen(Port type, MenuTypes obj)
        {
            if (obj == MenuTypes.Technology)
            {
                CloseMenu();
            }
        }
        private void CloseMenu()
        {
            controller.Clear();
        }
    }
}