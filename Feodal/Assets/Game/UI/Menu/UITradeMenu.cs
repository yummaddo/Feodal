using System;
using System.Threading.Tasks;
using Game.CallBacks;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallBackTrade;
using Game.DataStructures;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Typing;
using Game.UI.Abstraction;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;
using Game.UI.Menu.TradeMenu;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu
{
    public class UITradeMenu : MonoBehaviour
    {
        [SerializeField] private UITradeListController controller;
        [SerializeField] private GameObject root;
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
            
            MenuTypesExitProvider.CallBackTunneling<UITradeMenu>(_callbackClose);
            MenuTypesOpenProvider.CallBackTunneling<UITradeMenu>(_callbackOpen);
            
            Proxy.Connect<UITechnologyElementProvider,    UITechnologyListElement, UITechnologyMenu>     (OnClickedByTechnology);
            Proxy.Connect<UIListResourceElementProvider,  UIResourceListElement,   UIResourceListElement>(OnClickedByResource);
            Proxy.Connect<UICellContainerElementProvider, IUICellContainerElement, UIMenuBuilding>       (OnBuildSelected);
            
            Proxy.Connect<DatabaseSeedProvider,Seed, Seed>(OnSeedWasBay);
            return Task.CompletedTask;
        }
        private void OnTechnologyWasBay(Port arg1, TechnologyTradeCallBack arg2) => controller.ViewTechnologyUpdate(arg2);
        private void OnBuildWasBay(Port arg1, BuildingTradeCallBack arg2) => controller.ViewBuildingUpdate(arg2);
        
        private void OnResourceUpdate(Port arg1, ResourceTempedCallBack arg2) => controller.ViewResourceUpdate(arg2);
        private void OnSeedWasBay(Port arg1, Seed arg2) => controller.ViewSeedUpdate(arg2);
        
        public void OpenTechnology(UITechnologyListElement uiTechnologyListElement)
        {
            Debugger.Logger("OpenMenu UITechnologyMenu from UITradeMenu Menu", ContextDebug.Menu, Process.Action);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<UITradeMenu>(), MenuTypes.Technology);
            CloseMenu();
        }
        private void OnBuildSelected(Port port, IUICellContainerElement element)
        {
            root.SetActive(true);
            controller.ViewBuilding(element);
        }
        private void OnClickedByResource(Port port,UIResourceListElement element)
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

            OpenMenu();
        }
        private void OnClickedByTechnology(Port port,UITechnologyListElement element)
        {
            OpenMenu();
            controller.ViewTechnology(element);
        }

        private void OpenMenu()
        {
            Debugger.Logger("OpenMenu OnClickedByTechnology UITradeMenu Menu", ContextDebug.Menu, Process.Action);
            root.SetActive(true);
            _callbackOpen.OnCallBackInvocation?.Invoke(Porting.Type<UITradeMenu>(), MenuTypes.TradeMenu);
            status = true;
        }

        public void CloseMenu()
        {
            status = false;
            Debugger.Logger("CloseMenu UITradeMenu Menu", ContextDebug.Menu, Process.Action);
            _callbackClose.OnCallBackInvocation?.Invoke(Porting.Type<UITradeMenu>(), MenuTypes.TradeMenu);
            controller.Clear();
            root.SetActive(false);
        }
    }
}