using System;
using Game.Core.Abstraction;
using Game.Core.DataStructures.UI.Data;
using Game.Meta;
using Game.Services.Proxies.ClickCallback.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.ResourceListMenu
{

    public class UIResourceListElement : MonoBehaviour
    {
        public UIResource resource;
        public Image image;
        public Image rare;
        public Text title;
        public Text value;
        public ButtonListResourceElementCallBack buttonResourceCallBack;

        public void UpdateData(UIResource newResource)
        {
            resource = newResource;
            buttonResourceCallBack.DataInitialization(this);
            UpdateData();
        }
        private void UpdateData()
        {
            image.sprite = resource.resourceImage;
            rare.sprite = resource.resourceRareImage.GetSprite(resource.resource.rare);
            title.text = resource.Title;
            var valueAmount = resource.resource.Temp.GetAmount(resource.resource.title);
            value.text = valueAmount.ToString();
        }
        public void TryUpdate()
        {
        }
        public void UpdateValue(long valueElement)
        {
            value.text = valueElement.ToString();
        }
        public bool TryUpdate(IResource callBackResource, long callBackValue)
        {
            if (resource.resource.Data.Title == callBackResource.Title)
            {
                value.text = callBackValue.ToString();
                return true;
            }
            return false;
        }
    }
}