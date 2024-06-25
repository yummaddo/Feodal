using System;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.UI.Menu.TechnologyMenu;

namespace Game.CallBacks
{
    public class UITechnologyListElementCallBack : ICallBack<UITechnologyListElement>
    {
        public Action<Port, UITechnologyListElement> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; } = false;
    }
}