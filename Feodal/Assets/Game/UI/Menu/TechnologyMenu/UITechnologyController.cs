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
    public class UITechnologyController : UIElementOnEnable 
    {
        public Button technologyButton;
        public GameObject technologyRoot;
        public List<UITechnologyListElement> elements = new List<UITechnologyListElement>();
        private ICallBack<UITechnologyListElement> _callBackTechnologyListElement;
        private ICallBack<MenuTypes> _callBackInvocationButtonExitMenu;
        private UITradeMenu _tradeMenu;
        private Action<Port, ButtonExitMenuCallBack> _onCallBackInvocation;
        private bool _menuStatus = false;
        public override void OnEnableSProcess()
        {
        }
        public override void OnAwake()
        {
            _callBackInvocationButtonExitMenu = new MenuTypesCallback();
            _callBackTechnologyListElement = new UITechnologyListElementCallBack();
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        public override void UpdateOnInit()
        {
            UITechnologyElementProvider.CallBackTunneling<UITechnologyListElement>(_callBackTechnologyListElement);
            MenuTypesExitProvider.CallBackTunneling<ButtonExitMenuCallBack>(_callBackInvocationButtonExitMenu);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            UITechnologyElementProvider.CallBackTunneling<UITechnologyListElement>(_callBackTechnologyListElement);
            MenuTypesExitProvider.CallBackTunneling<ButtonExitMenuCallBack>(_callBackInvocationButtonExitMenu);
            foreach (var element in elements)
            {
                element.Inject(this);
            }
            return Task.CompletedTask;
        }
        public void OnTechnologyButtonClick()
        {
            if (_menuStatus)
            {
                _callBackInvocationButtonExitMenu.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), MenuTypes.Technology);
                CloseMenuManaged();
            }
            else OpenMenuManaged();
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
            _menuStatus = true;
            technologyRoot.SetActive(true);
        }
        public void CloseMenuManaged()
        {
            _menuStatus = false;
            technologyRoot.SetActive(false);
        }
    }
}