using System.Collections.Generic;
using Game.DataStructures;
using Game.DataStructures.Technologies;
using Game.DataStructures.Trades;
using Game.UI.Abstraction;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu.TradeMenu
{
    public static class UITradeListControllerExtension
    {
        internal static void PresentSeedTradeUpdate(this UITradeListController controller, SeedTrade tradeSeedTemped)
        {
            UpdateReset(controller);
            var currentTrade = tradeSeedTemped.Trades[tradeSeedTemped.CurrentStage()];
            Debugger.Logger($"PresentSeedTradeUpdate stage = {tradeSeedTemped.CurrentStage()}");
            controller.maxAmount = controller.TempResourceTemped.MaxTradeAmount(currentTrade.Map.GetAmount(1));
            CreateTradeElements(controller, currentTrade.resourceAmountCondition);
            CreateTechnologyElements(controller, currentTrade.technologyCondition);
        }
        internal static void PresentResourceTradeUpdate(this UITradeListController controller,
            ResourceTrade tradeResourceTemped)
        {
            UpdateReset(controller);
            var map = tradeResourceTemped.Map.GetAmount(1);
            controller.maxAmount = controller.TempResourceTemped.MaxTradeAmount(map);
            controller.slider.value = 1.0f;
            controller.tradeAmount = (int)(controller.slider.value * controller.maxAmount);
            controller.amountOfSliderText.text = (controller.maxAmount).ToString();
            CreateTradeElements(controller,tradeResourceTemped.resourceAmountCondition, controller.tradeAmount);
            CreateTechnologyElements(controller, tradeResourceTemped.technologyCondition);

        }

        internal static void PresentTrade(this UITradeListController controller,  ResourceTrade resourceTrade, UIResourceListElement element)
        {
            controller.tradeResource.sprite = controller.TempResourceTemped.CommonToUIResources[resourceTrade.@into.title].resourceImage;
            var map = resourceTrade.Map.GetAmount(1);
            controller.maxAmount = controller.TempResourceTemped.MaxTradeAmount(map);
            controller.slider.value = 1.0f;
            controller.tradeAmount = (int)(controller.slider.value * controller.maxAmount);
            
            controller.amountOfSliderText.text = (controller.maxAmount).ToString();
            CreateTradeElements(controller,resourceTrade.resourceAmountCondition, controller.tradeAmount);
            CreateTechnologyElements(controller, resourceTrade.technologyCondition);

        }
        internal static void PresentTrade(this UITradeListController controller,  SeedTrade tradeSeed)
        {
            controller.tradeResource.sprite = tradeSeed.@into.image;
            var currentTrade = tradeSeed.Trades[tradeSeed.CurrentStage()];
            controller.maxAmount = controller.TempResourceTemped.MaxTradeAmount(currentTrade.Map.GetAmount(1));
            CreateTradeElements(controller, currentTrade.resourceAmountCondition);
            CreateTechnologyElements(controller, currentTrade.technologyCondition);
        }
        internal static void PresentTrade(this UITradeListController controller,  IUICellContainerElement buildeSeed)
        {
             controller.tradeResource.sprite = buildeSeed.CellImage;
             CreateTradeElements(controller, controller.TradeBuildTemped.resourceAmountCondition, 1);
             CreateTechnologyElements(controller, controller.TradeBuildTemped.technologyCondition);
        }
        
        internal static void PresentTrade(this UITradeListController controller, TechnologyTrade technologyTrade)
        {
            controller.tradeResource.sprite = technologyTrade.sprite;
            CreateTradeElements(controller,technologyTrade.resourceAmountCondition);
        }
        
        private static void CreateTradeElements(UITradeListController controller, List<ResourceCounter> resourceAmountCondition, int amount = 1)
        {
            foreach (var tradeResource in resourceAmountCondition)
            {
                var element = Object.Instantiate(controller.targetTradeTemplate, controller.resourceListRect.transform);
                var component = element.GetComponent<UITradeResource>();
                if (component)
                {
                    controller.ResourceTradeCompare.Add(element.GetInstanceID(), element);
                    controller.TradeUCompare.Add(element.GetInstanceID(), component);
                    controller.TradeUCompareCounter.Add(element.GetInstanceID(), tradeResource);
                    var ui = controller.TempResourceTemped.CommonToUIResources[tradeResource.resource.title];
                    component.UpdateData(tradeResource.resource, ui.Data.ResourceImage);
                    component.UpdatePriceValue(tradeResource.value * amount, true);
                }
            }
            Resize<ResourceCounter>(resourceAmountCondition, controller.elementHeight,controller.resourceListRect);
        }

        private static void CreateTechnologyElements(UITradeListController controller,
            List<Technology> conditions, int amount = 1)
        {
            foreach (var condition in conditions)
            {
                var element = Object.Instantiate(controller.targetTechnologyTemplate, controller.techListRect.transform);
                var component = element.GetComponent<UITechnologyListElement>();
                if (component)
                {
                    controller.TechnologyCompare.Add(element.GetInstanceID(), component);
                    controller.TechnologyTradeCompare.Add(element.GetInstanceID(), element);
                    controller.TechnologyTradeTemped = condition.Data.Trade;
                    var ui = controller.TechnologyTradeTemped.sprite;
                    component.InjectFromTrade(controller.menu);
                    component.UpdateValue(condition);
                    component.UpdateStatus();
                }
            }
            Resize<Technology>(conditions, controller.elementTechnologyHeight,controller.techListRect);
        }
        private static void Resize<T>(List<T> elements, float height, RectTransform transform)
        {
            var newRect = transform.rect;
            newRect.height = height * elements.Count;
            Vector2 sizeDelta = transform.sizeDelta;
            sizeDelta.y = newRect.height;
            Vector2 pos = transform.anchoredPosition;
            pos.y = newRect.height / 2;
            transform.anchoredPosition = pos;
            transform.sizeDelta = sizeDelta;
        }
        private static void UpdateReset(UITradeListController controller)
        {
            foreach (var rGameObject in controller.ResourceTradeCompare)
                Object.Destroy(rGameObject.Value);
            foreach (var rGameObject in controller.TechnologyTradeCompare)
                Object.Destroy(rGameObject.Value);
            controller.TradeUCompare = new Dictionary<int, UITradeResource>();
            controller.TradeUCompareCounter = new Dictionary<int, ResourceCounter>();
            controller.ResourceTradeCompare = new Dictionary<int, GameObject>();
            controller.TechnologyCompare = new Dictionary<int, UITechnologyListElement>();
        }
    }
}