using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks;
using Game.CallBacks.CallbackClick.Button;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
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
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            _callBackInvocationButtonExitMenu = new MenuTypesCallback();
            _callBackTechnologyListElement = new UITechnologyListElementCallBack();
            
            UITechnologyElementProvider.CallBackTunneling<UITechnologyListElement>(_callBackTechnologyListElement);
            MenuTypesExitProvider.CallBackTunneling<ButtonExitMenuCallBack>(_callBackInvocationButtonExitMenu);
            
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            technologyButton.onClick.AddListener(OnTechnologyButtonClick);
            foreach (var element in elements)
            {
                element.Inject(this);
            }
            return Task.CompletedTask;
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
            // => tradeMenu
        }
        internal void TechnologyElementDisabledCall(UITechnologyListElement listElement)
        {
            _callBackTechnologyListElement.OnCallBackInvocation?.Invoke(Porting.Type<UITechnologyListElement>(),listElement);
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