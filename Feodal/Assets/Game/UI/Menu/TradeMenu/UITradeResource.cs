using System;
using Game.Core.DataStructures;
using Game.Core.DataStructures.UI.Data;
using Game.Services.Proxies.ClickCallback.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.TradeMenu
{
    public class UITradeResource : MonoBehaviour
    {
        public Resource resource;
        public Image image;
        public Text title;
        public Text value;
        public GameObject NotAmound;
        public void UpdateData(Resource newResource, Sprite sprite)
        {
            resource = newResource;
            image.sprite = sprite;
            UpdateData();
        }
        public void UpdatePriceValue(int newValue, bool avaiable)
        {
            NotAmound.SetActive(!avaiable);
            value.text = newValue.ToString();
        }

        private void UpdateData()
        {
            title.text = resource.Title;
        }
    }
}