using System;
using Game.Core.DataStructures.Technologies;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.Abstraction;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.TechnologyMenu
{
    public class UITechnologyListElement : MonoBehaviour
    {
        public Button click;
        public Technology technology;
        public Text text;
        public bool status = false;
        public Image image;
        public event Action<UITechnologyListElement> OnButtonCallActive;
        public event Action<UITechnologyListElement> OnButtonCallDisable;
        private UITechnologyController _controller;
        internal void Inject(UITechnologyController controller)
        {
            _controller = controller;
        }

        public void UpdateValue(Technology newTechnology)
        {
            this.technology = newTechnology;
            image.sprite = newTechnology.Trade.sprite;
            text.text = newTechnology.Title;
            status = this.technology.Status();
        }
        private void OnEnable()
        {
            if (_controller)
            {
                click.onClick.AddListener(ButtonCall);
                OnButtonCallActive += _controller.TechnologyElementActiveCall;
                OnButtonCallDisable += _controller.TechnologyElementDisabledCall;
            }
        }
        private void OnDisable()
        {
            if (_controller)
            {
                click.onClick.RemoveListener(ButtonCall);
                OnButtonCallActive -= _controller.TechnologyElementActiveCall;
                OnButtonCallDisable -= _controller.TechnologyElementDisabledCall;
            }
        }
        private void ButtonCall()
        {
            if (status == false)
            {
                Debug.Log("OnButtonCallActive?.Invoke(this);");
                OnButtonCallActive?.Invoke(this);
            }
            else
            {
                OnButtonCallDisable?.Invoke(this);;
            }
        }
    }
}