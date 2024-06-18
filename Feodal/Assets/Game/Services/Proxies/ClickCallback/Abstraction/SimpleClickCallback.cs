using System;
using Game.Services.Proxies.Abstraction;
using UnityEngine;

namespace Game.Services.Proxies.ClickCallback.Abstraction
{
    public abstract class SimpleClickCallback<TData> : MonoBehaviour, IClickCallback<TData>
    {
        private void Awake()
        {
            Initialization();
        }

        public Action<TData> OnClick { get; set; } = data =>
            Debugger.Logger($"ButtonClickCallback: {typeof(TData)}, {data.ToString()}",  ContextDebug.Session,Process.Action);

        public abstract void Initialization();
    }
}