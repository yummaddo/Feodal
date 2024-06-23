using System;
using Game.Services.Proxies.Abstraction;
using Game.UI.Menu;

namespace Game.Services.Proxies.CallBack
{
    public class MenuTypesCallback : ICallBack<MenuTypes>
    {
        public Action<Port, MenuTypes> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
    }
}