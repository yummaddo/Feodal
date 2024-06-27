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
        public UITechnologyMenu menu;
        public Button technologyButton;
        public GameObject technologyRoot;
        public List<UITechnologyListElement> elements = new List<UITechnologyListElement>();
        
        private ICallBack<UITechnologyListElement> _callBackTechnologyListElement;
        
        private UITradeMenu _tradeMenu;
        
        protected override void OnEnableSProcess()
        {
        }
        protected override void OnAwake()
        {
            _callBackTechnologyListElement = new UITechnologyListElementCallBack();
            UITechnologyElementProvider.CallBackTunneling<UITechnologyMenu>(_callBackTechnologyListElement);
        }
        protected override void UpdateOnInit()
        {
            isInit = true;
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            foreach (var element in elements)
                element.Inject(this);
            return Task.CompletedTask;
        }
        internal void TechnologyElementActiveCall(UITechnologyListElement listElement)
        {
            // => tradeMenu
        }
        internal void TechnologyElementDisabledCall(UITechnologyListElement listElement)
        {
            _callBackTechnologyListElement.OnCallBackInvocation?.Invoke(Porting.Type<UITechnologyMenu>(),listElement);
            menu.CloseMenu();
        }
    }
}