using System;
using System.Collections.Generic;
using Game.Core.Abstraction.UI;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Trades;
using Game.Core.Typing;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.Providers;
using Game.Services.Storage;
using Game.Services.Storage.Microservice;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.TradeMenu
{
    public class UITradeListController : MonoBehaviour
    {
        public GameObject targetTradeTemplate;
        public GameObject targetTechnologyTemplate;
        [Header("Control Elements")]
        public GameObject targetRoot;
        public GameObject amountSlider;
        public GameObject payRoot;
        public Slider slider;
        public Image tradeResource;
        public Text amountOfSliderText;
        [Header("Values")]
        public int tradeAmount;
        [Range(1,10)]public int itemInListRootMax = 4;
        public int maxAmount = 0;
        public int elementHeight = 240;
        public int elementTechnologyHeight = 240;
        [Header("Detectors")] 
        public Button add;
        public Button res;

        public Button payAmountMany;
        public Button payUnic;

        public Button exit;
        [Header("List")] 
        public RectTransform resourceListRect;
        public RectTransform techListRect;

        internal ResourceTemp TempResourceTemped;
        internal TechnologyTemp TempTechnologyTemped;

        #region Internal 
        internal Dictionary<int, GameObject> TechnologyTradeCompare = new Dictionary<int, GameObject>();
        internal Dictionary<int, GameObject> ResourceTradeCompare = new Dictionary<int, GameObject>();
        internal Dictionary<int, UITradeResource> TradeUCompare = new Dictionary<int, UITradeResource>();
        internal Dictionary<int, UITechnologyListElement> TechnologyCompare = new Dictionary<int, UITechnologyListElement>();
        internal Dictionary<int, ResourceCounter> TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
        
        internal ResourceTrade ResourceTradeTemped;
        internal SeedTrade TradeSeedTemped;
        internal BuildingTrade TradeBuildTemped;
        internal TechnologyTrade TechnologyTradeTemped;

        internal List<GameObject> Temp = new List<GameObject>();
        internal List<UITradeResource> TempResource = new List<UITradeResource>();

        private TradeType _tradeType = TradeType.None;
        private StorageService _service;
        private TradeMicroservice _tradeMicroservice;

        #endregion
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneStartMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            slider.onValueChanged.AddListener(delegate { SliderValueChangeCheck(); });
            _service = SessionStateManager.Instance.ServiceLocator.Resolve<StorageService>();
            _tradeMicroservice = SessionStateManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            TempResourceTemped = _service.GetResourceTemp();
            TempTechnologyTemped = _service.GetTechnologyTemp();
        }
        
        private void Reset()
        {
            foreach (var rGameObject in ResourceTradeCompare) Destroy(rGameObject.Value);
            foreach (var rGameObject in TechnologyTradeCompare) Destroy(rGameObject.Value);
            TradeUCompare = new Dictionary<int, UITradeResource>();
            TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
            ResourceTradeCompare = new Dictionary<int, GameObject>();
            TechnologyCompare = new Dictionary<int, UITechnologyListElement>();
            ResourceTradeTemped = null;
            TradeSeedTemped = null;
            TradeBuildTemped = null;
            TechnologyTradeTemped = null;
            _tradeType = TradeType.None;
        }
        private void SomeResourceUpdate(Port port,ResourceTempedCallBack callBack) { }
        private void OnEnable()
        {
            SliderValueChangeCheck();
        }
        private void SliderValueChangeCheck()
        {
            if (ResourceTradeTemped != null && TempResourceTemped != null)
            {
                maxAmount = TempResourceTemped.MaxTradeAmount(ResourceTradeTemped.Map.GetAmount(1));
                tradeAmount = (int)(slider.value * maxAmount);
                amountOfSliderText.text = (tradeAmount).ToString();
            }
        }
        public void ViewResource(UIResourceListElement element)
        {
            Reset();
            targetRoot.SetActive(true);
            payRoot.SetActive(false);
            amountSlider.SetActive(true);            
            var title = element.UiResource.Title;
            ResourceTradeTemped = element.UiResource.Resource.Temp.GetResourceTrade[title];
            _tradeType = TradeType.Resource;
            this.PresentTrade(ResourceTradeTemped, element);
        }
        public void ViewSeed(UIResourceListElement element)
        {
            Reset();
            targetRoot.SetActive(true);
            payRoot.SetActive(true);
            amountSlider.SetActive(false);
            var title = element.UiResource.Title;
            TradeSeedTemped = element.UiResource.Resource.Temp.GetSeedTrade[title];
            _tradeType = TradeType.Seed;
            this.PresentTrade(TradeSeedTemped);
        }
        public void ViewTechnology(UITechnologyListElement listElement)
        {
            Reset();
            targetRoot.SetActive(true);
            payRoot.SetActive(true);
            amountSlider.SetActive(false);
            TempTechnologyTemped = listElement.technology.Data.Temp;
            TechnologyTradeTemped = TempTechnologyTemped.TechnologyStores[listElement.technology.Data.Title].Trade;
            _tradeType = TradeType.Technology;
            this.PresentTrade(TechnologyTradeTemped);
        }
        public void ViewBuilding(IUICellContainerElement element)
        {
            Reset();
            targetRoot.SetActive(true);
            payRoot.SetActive(true);
            amountSlider.SetActive(false);
            TradeBuildTemped = TempResourceTemped.GetBuildingTrade[element.State.Data.ExternalName];
            Debugger.Logger(TradeBuildTemped.TradeName);
            _tradeType = TradeType.Building;
            this.PresentTrade(element);
        }
        public void Clear()
        {
            Reset();
        }
        // internal ResourceTrade ResourceTradeTemped;
        // internal SeedTrade TradeSeedTemped;
        // internal BuildingTrade TradeBuildTemped;
        // internal TechnologyTrade TechnologyTradeTemped;
        public void TryTyPayTrade()
        {
            if (_tradeType == TradeType.Resource)
                _tradeMicroservice.Trade(ResourceTradeTemped, ResourceTradeTemped.Map.GetAmount(tradeAmount),
                    tradeAmount, false);
            else if (_tradeType == TradeType.Seed)
            {
                var currentTrade = TradeSeedTemped.Trades[TradeSeedTemped.CurrentStage()];
                var mapTrade =  currentTrade.Map.GetAmount(1);
                _tradeMicroservice.Trade(TradeSeedTemped, mapTrade, 1, false);
            }
            else if (_tradeType == TradeType.Building)
                _tradeMicroservice.Trade(TradeBuildTemped, TradeBuildTemped.Map.GetAmount(1), 1, false);
            else if (_tradeType == TradeType.Technology)
                _tradeMicroservice.Trade(TechnologyTradeTemped, TechnologyTradeTemped.Map.GetAmount(1), 1, false);
            else
            {
                // no one trade was be not provide
            }
        }
    }
}