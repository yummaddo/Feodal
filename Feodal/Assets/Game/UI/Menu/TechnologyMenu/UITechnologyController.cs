using System;
using System.Collections.Generic;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.Abstraction;
using Game.Services.Proxies.CallBack;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.TechnologyMenu
{
    public class UITechnologyController : MonoBehaviour 
    {
        public Button technologyButton;
        public GameObject technologyRoot;
        public List<UITechnologyListElement> elements = new List<UITechnologyListElement>();

        private ICallBack<UITechnologyListElement> _callBackTechnologyListElement;
        private ICallBack<MenuTypes> _callBackInvocationButtonExitMenu;
        private UITradeMenu _tradeMenu;
        private bool _statusMenu = false;
        private Action<Port, ButtonExitMenuCallBack> _onCallBackInvocation;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
            _callBackInvocationButtonExitMenu = new MenuTypesCallback();
            _callBackTechnologyListElement = new UITechnologyListElementCallBack();
            
            UITechnologyElementProvider.CallBackTunneling<UITechnologyListElement>(_callBackTechnologyListElement);
            MenuTypesExitProvider.CallBackTunneling<ButtonExitMenuCallBack>(_callBackInvocationButtonExitMenu);
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            technologyButton.onClick.AddListener(OnTechnologyButtonClick);
            foreach (var element in elements)
            {
                element.Inject(this);
            }
        }
        private void OnTechnologyButtonClick()
        {
            if (_statusMenu)
            {
                _callBackInvocationButtonExitMenu.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), MenuTypes.Technology);
                CloseMenuManaged();
            }
            else
                OpenMenuManaged();
        }
        internal void TechnologyElementActiveCall(UITechnologyListElement listElement)
        {
            _callBackTechnologyListElement.OnCallBackInvocation?.Invoke(Porting.Type<UITechnologyListElement>(),listElement);
            // => tradeMenu
        }
        internal void TechnologyElementDisabledCall(UITechnologyListElement listElement)
        {
            // OnCallBackInvocation?.Invoke(Porting.Type<UITechnologyElement>(),element);
        }
        public void OpenMenuManaged()
        {
            technologyRoot.SetActive(true);
            _statusMenu = true;
            // _tradeMenu = tradeMenu;
        }
        public void CloseMenuManaged()
        {
            technologyRoot.SetActive(false);
            _statusMenu = false;
        }

        public GameObject TargetObject { get; set; }
    }
}