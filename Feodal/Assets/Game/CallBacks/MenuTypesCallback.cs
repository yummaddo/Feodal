using System;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.UI.Menu;

namespace Game.CallBacks
{
    public class MenuTypesCallback : ICallBack<MenuTypes>
    {
        public Action<Port, MenuTypes> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
    }
}