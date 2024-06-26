using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks;
using Game.CallBacks.CallBackTrade;
using Game.DataStructures;
using Game.DataStructures.Trades;
using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;
using Game.Services.StorageServices;
using Game.Services.StorageServices.Microservice;
using Game.Typing;
using Game.UI.Abstraction;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.TradeMenu
{
    public class UITradeListController : UIElementOnEnable
    {
        public GameObject targetTradeTemplate;
        public GameObject targetTechnologyTemplate;
        [Header("Control Elements")] public GameObject targetRoot;
        public GameObject amountSlider;
        public GameObject payRoot;
        public Slider slider;
        public Image tradeResource;
        public Text amountOfSliderText;
        [Header("Values")] public int tradeAmount;
        [Range(1, 10)] public int itemInListRootMax = 4;
        public int maxAmount = 0;
        public int elementHeight = 240;
        public int elementTechnologyHeight = 240;
        [Header("Detectors")] public Button add;
        public Button res;

        public Button payAmountMany;
        public Button payUnic;

        public Button exit;
        [Header("List")] public RectTransform resourceListRect;
        public RectTransform techListRect;

        internal ResourceTemp TempResourceTemped;
        internal TechnologyTemp TempTechnologyTemped;

        #region Internal

        internal Dictionary<int, GameObject> TechnologyTradeCompare = new Dictionary<int, GameObject>();
        internal Dictionary<int, GameObject> ResourceTradeCompare = new Dictionary<int, GameObject>();
        internal Dictionary<int, UITradeResource> TradeUCompare = new Dictionary<int, UITradeResource>();

        internal Dictionary<int, UITechnologyListElement> TechnologyCompare =
            new Dictionary<int, UITechnologyListElement>();

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
        public override void OnEnableSProcess()
        {
            SliderValueChangeCheck();
        }
        public override void OnAwake()
        {
        }
        public override void UpdateOnInit()
        {
            SessionLifeStyleManager.AddLifeIteration(Inject, SessionLifecycle.OnSceneAwakeClose);
            slider.onValueChanged.AddListener(delegate { SliderValueChangeCheck(); });
        }
        private Task Inject(IProgress<float> progress)
        {
            _service = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<StorageService>();
            _tradeMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            TempResourceTemped = _service.GetResourceTemp();
            TempTechnologyTemped = _service.GetTechnologyTemp();
            return Task.CompletedTask;
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
        public void ViewResourceUpdate(ResourceTempedCallBack element)
        {
            if (!gameObject.activeSelf) return;
            targetRoot.SetActive(true);
            payRoot.SetActive(false);
            amountSlider.SetActive(true);
            ResourceTradeTemped = element.Resource.Temp.GetResourceTrade[element.Resource.Title];
            _tradeType = TradeType.Resource;
            this.PresentResourceTradeUpdate(ResourceTradeTemped);
        }
        public void ViewSeedUpdate(Seed element)
        {
            if (!gameObject.activeSelf) return;
            targetRoot.SetActive(true);
            payRoot.SetActive(true);
            amountSlider.SetActive(false);
            TradeSeedTemped = element.Data.Temp.GetSeedTrade[element.title];
            _tradeType = TradeType.Seed;
            this.PresentSeedTradeUpdate(TradeSeedTemped);
        }

        public void ViewBuildingUpdate(BuildingTradeCallBack element) => Clear();
        public void ViewTechnologyUpdate(TechnologyTradeCallBack element) => Clear();
        public void Clear()
        {
            if (!gameObject.activeSelf) return;
            Reset();
        }
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
        }
    }
}