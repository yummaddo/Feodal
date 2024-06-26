using System;

namespace Game.Services.ProxyServices.Abstraction
{
    public interface ICallBack<TData>
    {
        public Action<Port, TData> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
    }
}