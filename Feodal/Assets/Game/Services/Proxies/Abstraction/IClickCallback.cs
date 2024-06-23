using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Services.Proxies.Abstraction
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
}