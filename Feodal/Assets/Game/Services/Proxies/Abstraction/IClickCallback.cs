using System;
using UnityEngine.PlayerLoop;

namespace Game.Services.Proxies.Abstraction
{
    public interface IClickCallback<TData>
    {
        public Action<TData> OnClick { get; set; }
        public void Initialization();
    }
}