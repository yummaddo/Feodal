using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.Typing;
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
        [SerializeField] private GameObject root;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<UIListResourceElementProvider, UIResourceListElement, UIResourceListElement>(OnClickedBySimpleResource);
            Proxy.Connect<UITechnologyElementProvider, UITechnologyListElement, UITechnologyListElement>(OnClickedByTechnology);
            Proxy.Connect<UICellContainerElementProvider, IUICellContainerElement, UIMenuBuilding>( OnBuildSelected);
            
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(OnClickedByMenuExit);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, ButtonOpenMenuCallBack>(OnClickedByMenuOpen);
        }

        private void OnBuildSelected(Port port, IUICellContainerElement element)
        {
            root.SetActive(true);
            controller.ViewBuilding(element);
        }

        private void OnClickedBySimpleResource(Port port,UIResourceListElement element)
        {
            if (element.type == ResourceType.Seed)
            {
                root.SetActive(true);
                controller.ViewSeed(element);
            }
            else
            {
                root.SetActive(true);
                controller.ViewResource(element);
            }
        }
        private void OnClickedByTechnology(Port port,UITechnologyListElement element)
        {
            root.SetActive(true);
            controller.ViewTechnology(element);
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
            root.SetActive(false);
        }
    }
}