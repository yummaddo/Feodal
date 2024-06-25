using System;
using System.Threading.Tasks;
using Game.CallBacks;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallBackTrade;
using Game.DataStructures;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Typing;
using Game.UI.Abstraction;
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
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            Proxy.Connect<UIListResourceElementProvider, UIResourceListElement, UIResourceListElement>(OnClickedByResource);
            Proxy.Connect<UITechnologyElementProvider, UITechnologyListElement, UITechnologyListElement>(OnClickedByTechnology);
            Proxy.Connect<UICellContainerElementProvider, IUICellContainerElement, UIMenuBuilding>( OnBuildSelected);
            
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UITradeMenu>(OnClickedByMenuExit);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UITradeMenu>(OnClickedByMenuExit);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, ButtonExitMenuCallBack>(OnClickedByMenuOpen);
            // TradeSuccessfully
            Proxy.Connect<DatabaseSeedProvider,Seed, Seed>(OnSeedWasBay);
            // Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(OnResourceUpdate);
            // Proxy.Connect<SeedTradeProvider,SeedTradeCallBack>(OnSeedWasBay, Port.TradeSuccessfully);
            // Proxy.Connect<BuildingTradeProvider,BuildingTradeCallBack>(OnBuildWasBay, Port.TradeSuccessfully);
            // Proxy.Connect<TechnologyTradeProvider,TechnologyTradeCallBack>(OnTechnologyWasBay, Port.TradeSuccessfully);
            return Task.CompletedTask;
        }
        private void OnTechnologyWasBay(Port arg1, TechnologyTradeCallBack arg2) => controller.ViewTechnologyUpdate(arg2);
        private void OnBuildWasBay(Port arg1, BuildingTradeCallBack arg2) => controller.ViewBuildingUpdate(arg2);
        
        private void OnResourceUpdate(Port arg1, ResourceTempedCallBack arg2) => controller.ViewResourceUpdate(arg2);
        private void OnSeedWasBay(Port arg1, Seed arg2) => controller.ViewSeedUpdate(arg2);
        
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
            if (obj == MenuTypes.TradeMenu)
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