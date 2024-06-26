using System;
using UnityEngine;

namespace Game.Services.ProxyServices.Abstraction
{
    public class ClickCallback<TData> : IClickCallback<TData>
    {
        public Action<Port, TData> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
        public GameObject TargetObject { get; set; }
    }
}