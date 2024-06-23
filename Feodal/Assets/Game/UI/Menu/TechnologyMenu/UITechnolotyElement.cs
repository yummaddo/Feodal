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
        private void OnEnable()
        {
            click.onClick.AddListener(ButtonCall);
            OnButtonCallActive += _controller.TechnologyElementActiveCall;
            OnButtonCallDisable += _controller.TechnologyElementDisabledCall;
        }
        private void OnDisable()
        {
            click.onClick.RemoveListener(ButtonCall);
            OnButtonCallActive -= _controller.TechnologyElementActiveCall;
            OnButtonCallDisable -= _controller.TechnologyElementDisabledCall;
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