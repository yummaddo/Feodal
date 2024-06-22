using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Services.Proxies.Abstraction
{
    public interface IClickCallback<TData>
    {
        public Action<Port, TData> OnClick { get; set; }
        public bool IsInit { get; set; }
        public GameObject TargetObject { get; set; }
    }
}