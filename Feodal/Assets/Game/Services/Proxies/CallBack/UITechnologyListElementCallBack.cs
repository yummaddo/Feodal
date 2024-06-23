using System;
using Game.Services.Proxies.Abstraction;
using Game.UI.Menu.TechnologyMenu;

namespace Game.Services.Proxies.CallBack
{
    public class UITechnologyListElementCallBack : ICallBack<UITechnologyListElement>
    {
        public Action<Port, UITechnologyListElement> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
    }
}