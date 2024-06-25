using System;
using System.Threading.Tasks;
using Game.DataStructures.Technologies;
using Game.DataStructures.Technologies.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
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
        public Color activeColor;
        public Color disableColor;
        public event Action<UITechnologyListElement> OnButtonCallActive;
        public event Action<UITechnologyListElement> OnButtonCallDisable;
        private UITechnologyController _controller;
        private bool _isInit = false;

        internal void Inject(UITechnologyController controller)
        {
            _controller = controller;
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
        private void OnEnable()
        {
            if (_controller)
            {
                if (technology != null)
                {
                    status = technology.Status();
                    image.color = status ? activeColor : disableColor;
                }
                click.onClick.AddListener(ButtonCall);
                OnButtonCallActive += _controller.TechnologyElementActiveCall;
                OnButtonCallDisable += _controller.TechnologyElementDisabledCall;
            }
            
            var sessionManager = SessionLifeStyleManager.Instance;
            if (sessionManager.IsMicroServiceSessionInit && !_isInit)
            {
                UpdateOnInit();
            }
            else if (!_isInit)
            {
                SessionLifeStyleManager.AddLifeIteration(UpdateOnInit, SessionLifecycle.OnSceneStartSession);
            }
        }
        private Task UpdateOnInit(IProgress<float> progress)
        {
            _isInit = true;
            Proxy.Connect<DatabaseTechnologyProvider,ITechnologyStore,ITechnologyStore>(SomeResourceUpdate);
            UpdateStatus();
            return Task.CompletedTask;
        }
        private void UpdateOnInit()
        {
            _isInit = true;
            Proxy.Connect<DatabaseTechnologyProvider,ITechnologyStore,ITechnologyStore>(SomeResourceUpdate);
            UpdateStatus();
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
        private void OnDisable()
        {
            if (!gameObject.activeSelf) return;
            if (_controller)
            {
                click.onClick.RemoveListener(ButtonCall);
                OnButtonCallActive -= _controller.TechnologyElementActiveCall;
                OnButtonCallDisable -= _controller.TechnologyElementDisabledCall;
            }
        }
        private void ButtonCall()
        {
            if (!gameObject.activeSelf) return;
            if (status == false)
            {
                Debug.Log("OnButtonCallActive?.Invoke(this);");
                OnButtonCallDisable?.Invoke(this);;
            }
            else
            {
                OnButtonCallActive?.Invoke(this);
            }
        }
    }
}