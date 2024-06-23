using System.Collections.Generic;
using Game.Core.Abstraction.UI;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;
using UnityEngine;

namespace Game.UI.Menu.TradeMenu
{
    public static class UITradeListControllerExtension
    {
        internal static void PresentTrade(this UITradeListController controller,  ResourceTrade resourceTrade, UIResourceListElement element)
        {
            controller.tradeResource.sprite = controller.TempResourceTemped.CommonToUIResources[resourceTrade.Into.title].resourceImage;
            controller.maxAmount = controller.TempResourceTemped.MaxTradeAmount(resourceTrade.Map.GetAmount(1));
            controller.tradeAmount = (int)(controller.slider.value * controller.maxAmount);
            controller.amountOfSliderText.text = (controller.tradeAmount).ToString();
            CreateTradeElements(controller,resourceTrade.resourceAmountCondition, controller.tradeAmount);
            CreateTechnologyElements(controller, resourceTrade.technologyCondition);

        }
        internal static void PresentTrade(this UITradeListController controller,  SeedTrade tradeSeed)
        {
            controller.tradeResource.sprite = tradeSeed.@into.image;
            var currentTrade = tradeSeed.Trades[tradeSeed.currentStage];
            controller.maxAmount = controller.TempResourceTemped.MaxTradeAmount(currentTrade.Map.GetAmount(1));
            CreateTradeElements(controller, currentTrade.resourceAmountCondition);
            CreateTechnologyElements(controller, currentTrade.technologyCondition);
        }
        internal static void PresentTrade(this UITradeListController controller,  IUICellContainerElement tradeSeed)
        {
             controller.tradeResource.sprite = tradeSeed.CellImage;
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
                    component.UpdateValue(condition);
                }
            }
            Resize<Technology>(conditions, controller.elementTechnologyHeight,controller.techListRect);
        }
        
        public static void Resize<T>(List<T> elements, float height, RectTransform transform)
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
    }
}