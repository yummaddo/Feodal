using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Trades;
using Game.UI.Menu.ResourceListMenu;
using UnityEngine;

namespace Game.UI.Menu.TradeMenu
{
    public static class UITradeListControllerExtension
    {
        internal static void PresentTrade(this UITradeListController controller,  ResourceTrade resourceTrade, UIResourceListElement element)
        {
            var uiResource = element.resource;
            var resource = uiResource.resource;
            controller.TempResourceTemped = resource.Data.Temp;
            controller.tradeResource.sprite = controller.TempResourceTemped.CommonToUIResources[resourceTrade.Into.title].resourceImage;
            controller.maxAmount = controller.TempResourceTemped.MaxTradeAmount(resourceTrade.Map.GetAmount(1));
            controller.tradeAmount = (int)(controller.slider.value * controller.maxAmount);
            controller.amountOfSliderText.text = (controller.tradeAmount).ToString();
            CreateTradeElements(controller,resourceTrade.resourceAmountCondition, controller.tradeAmount);            
        }
        internal static void PresentTrade(this UITradeListController controller,  SeedTrade tradeSeed)
        {
        }
        internal static void PresentTrade(this UITradeListController controller, TechnologyTrade technologyTrade)
        {
            var into  = technologyTrade.into;
            controller.tradeResource.sprite = technologyTrade.sprite;
            CreateTradeElements(controller,technologyTrade.resourceAmountCondition);
        }
        private static void CreateTradeElements(UITradeListController controller, List<ResourceCounter> resourceAmountCondition, int amount = 1)
        {
            foreach (var tradeResource in resourceAmountCondition)
            {
                var element = Object.Instantiate(controller.targetTemplate, controller.payRoot.transform);
                {
                    var component = element.GetComponent<UITradeResource>();
                    if (component)
                    {
                        controller.ResourceTradeCompare.Add(element.GetInstanceID() ,element);
                        controller.TradeUCompare.Add(element.GetInstanceID() ,component);
                        controller.TradeUCompareCounter.Add(element.GetInstanceID(),tradeResource);
                        Debug.Log( controller.TempResourceTemped);
                        Debug.Log( controller.TempResourceTemped.CommonToUIResources);
                        Debug.Log( controller.TempResourceTemped.CommonToUIResources);
                        Debug.Log(tradeResource.resource);
                        Debug.Log(tradeResource.resource.title);
                        Debug.Log(tradeResource.resource.title);
                        Debug.Log(controller.TempResourceTemped.CommonToUIResources[tradeResource.resource.title]);
                        var ui =  controller.TempResourceTemped.CommonToUIResources[tradeResource.resource.title];
                        component.UpdateData(tradeResource.resource, ui.Data.ResourceImage);
                        component.UpdatePriceValue(tradeResource.value * amount, true);
                    }
                }
            }
            Resize(controller, resourceAmountCondition);
        }
        private static void Resize(UITradeListController controller, List<ResourceCounter> resourceAmountCondition)
        {
            var newRect = controller.payListRect.rect;
            newRect.height = controller.elementHeight * resourceAmountCondition.Count;
            Vector2 sizeDelta = controller.payListRect.sizeDelta;
            sizeDelta.y = newRect.height;
            Vector2 pos = controller.payListRect.anchoredPosition;
            pos.y = newRect.height / 2;
            Debug.Log(pos);
            controller.payListRect.anchoredPosition = pos;
            Debug.Log( controller.payListRect.anchoredPosition);
            controller.payListRect.sizeDelta = sizeDelta;
        }
    }
}