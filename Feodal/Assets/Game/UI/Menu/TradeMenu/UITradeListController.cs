using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Trades;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.Providers;
using Game.Services.Storage;
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
        private StorageService _service;
        [Header("Control Elements")]
        public GameObject targetRoot;
        public GameObject targetRootTechnologyTrade;
        public Slider slider;
        public Image tradeResource;
        public Text amountOfSliderText;
        public GameObject targetTemplate;
        [Header("Values")]
        public int tradeAmount;
        public int itemInListRootMax = 4;
        public int maxAmount = 0;
        public int elementHeight = 240;
        [Header("Detectors")] 
        public Button add;
        public Button res;
        public Button pay;
        public Button exit;
        [Header("List")] 
        public GameObject payRoot;
        public RectTransform payListRect;
        
        internal ResourceTemp TempResourceTemped;
        internal TechnologyTemp TempTechnologyTemped;

        
        #region Internal 

        internal Dictionary<int, GameObject> ResourceTradeCompare = new Dictionary<int, GameObject>();
        internal Dictionary<int, UITradeResource> TradeUCompare = new Dictionary<int, UITradeResource>();
        internal Dictionary<int, ResourceCounter> TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
        
        internal ResourceTrade ResourceTradeTemped;
        internal TechnologyTrade TechnologyTradeTemped;

        internal List<GameObject> Temp = new List<GameObject>();
        internal List<UITradeResource> TempResource = new List<UITradeResource>();
        
        internal TechnologyTrade TradeTechnology;
        internal SeedTrade TradeSeed;
        internal ResourceTrade ResourceTrade;  

        #endregion

        private void Awake()
        {
            SessionStateManager.Instance.OnSceneStartMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
                _service = SessionStateManager.Instance.ServiceLocator.Resolve<StorageService>();
                Debug.Log(_service);
                TempResourceTemped = _service.GetResourceTemp();
            TempTechnologyTemped = _service.GetTechnologyTemp();
            
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            slider.onValueChanged.AddListener(delegate
            {
                SliderValueChangeCheck();
            });
        }
        private void SomeResourceUpdate(Port port,ResourceTempedCallBack callBack)
        {
            
        }
        private void SliderValueChangeCheck()
        {
            if (ResourceTradeTemped != null && TempResourceTemped != null)
            {
                maxAmount = TempResourceTemped.MaxTradeAmount(ResourceTradeTemped.Map.GetAmount(1));
                var tradeAmountTemped = (int)(slider.value * maxAmount);
                if (tradeAmountTemped != tradeAmount)
                {
                    amountOfSliderText.text = (tradeAmount).ToString();
                    foreach (var counterPair in TradeUCompareCounter)
                        TradeUCompare[counterPair.Key].UpdatePriceValue(maxAmount*counterPair.Value.value, true);
                }
            }
        }
        public void View(UIResourceListElement element)
        {
            targetRoot.SetActive(true);
            Reset();
            var title = element.resource.resource.title;
            ResourceTradeTemped = element.resource.resource.Data.Temp.CommonToUniversalDataSet[title];
            this.PresentTrade(ResourceTradeTemped, element);
        }

        public void View(UITechnologyListElement listElement)
        {
            targetRoot.SetActive(true);
            targetTemplate.SetActive(true);
            Reset();
            TempTechnologyTemped = listElement.technology.Data.Temp;
            TechnologyTradeTemped = TempTechnologyTemped.TechnologyStores[listElement.technology.Data.Title].Trade;
            this.PresentTrade(TechnologyTradeTemped);
        }

        public void Reset()
        {
            TradeUCompare = new Dictionary<int, UITradeResource>();
            TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
            ResourceTradeTemped = null;
            foreach (var rGameObject in ResourceTradeCompare) Destroy(rGameObject.Value);
            ResourceTradeCompare = new Dictionary<int, GameObject>();
        }
        public void Clear()
        {
            TradeUCompare = new Dictionary<int, UITradeResource>();
            TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
            ResourceTradeTemped = null;
            foreach (var rGameObject in ResourceTradeCompare) Destroy(rGameObject.Value);
            ResourceTradeCompare = new Dictionary<int, GameObject>();
            targetRoot.SetActive(false);
            targetRootTechnologyTrade.SetActive(false);
        }

        public void TryTyPayTrade()
        {
        }
    }
}