using System;
using UnityEngine;

namespace Game.Services.ProxyServices.Abstraction
{
    public interface IClickCallback<TData> : ICallBack<TData>
    {
        public GameObject TargetObject { get; set; }
    }
    public interface ICallBack<TData>
    {
        public Action<Port, TData> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
    }

    public class ClickCallback<TData> : IClickCallback<TData>
    {
        public Action<Port, TData> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
        public GameObject TargetObject { get; set; }
    }
}