using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Trades;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.Providers;
using Game.Services.Storage;
using Game.Services.Storage.ResourcesRepository;
using Game.UI.Menu.ResourceListMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.TradeMenu
{
    public class UITradeResourceListController : MonoBehaviour
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
        
        [Header("Detectors")] 
        public Button add;
        public Button res;
        public Button pay;
        public Button exit;
        [Header("List")] 
        public GameObject payRoot;

        private Dictionary<int, GameObject> _resourceTradeCompare = new Dictionary<int, GameObject>();
        private Dictionary<int, UITradeResource> _TradeUCompare = new Dictionary<int, UITradeResource>();
        private Dictionary<int, ResourceCounter> _TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
        private ResourceTemp _tempTemped;
        private ResourceTrade _resourceTradeTemped;
        public List<GameObject> temp = new List<GameObject>();
        public List<UITradeResource> tempResource = new List<UITradeResource>();
        internal TechnologyTrade TradeTechnology;
        internal SeedTrade TradeSeed;
        internal ResourceTrade ResourceTrade;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            _service = SessionStateManager.Instance.Container.Resolve<StorageService>();
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }
        public void SomeResourceUpdate(Port port,ResourceTempedCallBack callBack)
        {
        }
        private void ValueChangeCheck()
        {
            if (_resourceTradeTemped != null && _tempTemped != null)
            {
                maxAmount = _tempTemped.MaxTradeAmount(_resourceTradeTemped.Map.GetAmount(1));
                var tradeAmountTemped = (int)(slider.value * maxAmount);
                if (tradeAmountTemped != tradeAmount)
                {
                    amountOfSliderText.text = (tradeAmount).ToString();
                    foreach (var counterPair in _TradeUCompareCounter)
                        _TradeUCompare[counterPair.Key].UpdatePriceValue(maxAmount*counterPair.Value.value, true);
                }
            }
        }
        public void View(UIResourceListElement element)
        {
            targetRoot.SetActive(true);
            var title = element.resource.resource.title;
            _resourceTradeTemped = element.resource.resource.Data.Temp.CommonToUniversalDataSet[title];
            PresentTrade(_resourceTradeTemped, element);
        }
        public void Clear()
        {
            _TradeUCompare = new Dictionary<int, UITradeResource>();
            _TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
            _tempTemped = null;
            _resourceTradeTemped = null;
            foreach (var rGameObject in _resourceTradeCompare) Destroy(rGameObject.Value);
            _resourceTradeCompare = new Dictionary<int, GameObject>();
        }
        public void PresentTrade(ResourceTrade resourceTrade, UIResourceListElement element)
        {
            var uiResource = element.resource;
            tradeResource.sprite = element.resource.resourceImage;
            var resource = uiResource.resource;
            _tempTemped = resource.Data.Temp;
            maxAmount = _tempTemped.MaxTradeAmount(resourceTrade.Map.GetAmount(1));
            tradeAmount = (int)(slider.value * maxAmount);
            amountOfSliderText.text = (tradeAmount).ToString();
            CreateTradeResourceElements(resourceTrade.resourceAmountCondition,_tempTemped);            
        }
        public void CreateTradeResourceElements(List<ResourceCounter> resourceAmountCondition, ResourceTemp resourceTemp)
        {
            foreach (var tradeResource in resourceAmountCondition)
            {
                var element = Instantiate(targetTemplate, payRoot.transform);
                var component = element.GetComponent<UITradeResource>();
                if (component)
                {
                    _resourceTradeCompare.Add(element.GetInstanceID() ,element);
                    _TradeUCompare.Add(element.GetInstanceID() ,component);
                    _TradeUCompareCounter.Add(element.GetInstanceID(),tradeResource);
                    var ui = resourceTemp.CommonToUIResources[tradeResource.resource.title];
                    component.UpdateData(tradeResource.resource, ui.Data.ResourceImage);
                    component.UpdatePriceValue(tradeResource.value*tradeAmount , true);
                }
            }
        }
        public void PresentTrade(SeedTrade tradeSeed)
        {
        }
        public void PresentTrade(TechnologyTrade technologyTrade)
        {
        }
        public void TryTyPayTrade()
        {
        }
    }
}