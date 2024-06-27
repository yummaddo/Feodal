using System;
using System.Threading.Tasks;
using Game.DataStructures.Technologies;
using Game.DataStructures.Technologies.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.TechnologyMenu
{
    public class UITechnologyListElement : UIElementOnEnable
    {
        public Button click;
        public Technology technology;
        public Text text;
        public bool status = false;
        public Image image;
        public Color activeColor;
        public Color disableColor;
        public event Action<UITechnologyListElement> OnButtonCallActive;
        public event Action<UITechnologyListElement> OnButtonCallDisable;
        
        private UITechnologyController _controller;
        
        protected override void OnAwake()
        {
        }

        protected override void UpdateOnInit()
        {
            isInit = true;
        }

        protected override void OnEnableSProcess()
        {
            if (_controller)
            {
                if (technology != null)
                {
                    status = technology.Status();
                    image.color = status ? activeColor : disableColor;
                }
            }
        }
        internal void InjectFromTrade( UITradeMenu menu )
        {
            OnButtonCallDisable += menu.OpenTechnology;
        }
        internal void Inject(UITechnologyController controller)
        {
            isInit = true;
            _controller = controller;
            Proxy.Connect<DatabaseTechnologyProvider,ITechnologyStore,ITechnologyStore>(SomeResourceUpdate);
            OnButtonCallActive += _controller.TechnologyElementActiveCall;
            OnButtonCallDisable += _controller.TechnologyElementDisabledCall;
            UpdateStatus();
        }
        public void UpdateValue(Technology newTechnology)
        {
            if (!gameObject.activeSelf) return;
            this.technology = newTechnology;
            image.sprite = newTechnology.Trade.sprite;
            text.text = newTechnology.Title;
            status = this.technology.Status();
            image.color = status ? activeColor : disableColor;
        }
        private void SomeResourceUpdate(Port arg1, ITechnologyStore arg2)
        {
            try
            {
                status = technology.Status();
                image.color = status ? activeColor : disableColor;
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        public void UpdateStatus()
        {
            if (!gameObject.activeSelf) return;
            if (technology != null)
            {
                status = technology.Status();
                image.color = status ? activeColor : disableColor;
            }
        }
        public void ButtonCall()
        {
            if (!gameObject.activeSelf) return;
            if (status == false)
            {
                OnButtonCallDisable?.Invoke(this);;
            }
            else 
                OnButtonCallActive?.Invoke(this);
            
        }
    }
}