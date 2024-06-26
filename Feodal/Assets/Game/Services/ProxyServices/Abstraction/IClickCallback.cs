using System;
using UnityEngine;

namespace Game.Services.ProxyServices.Abstraction
{
    public interface IClickCallback<TData> : ICallBack<TData>
    {
        public GameObject TargetObject { get; set; }
    }
}