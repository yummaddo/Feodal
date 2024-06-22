using System;
using System.Collections.Generic;
using Game.Services.Proxies.Abstraction;
using Game.Services.Proxies.Providers;
using UnityEngine;

namespace Game.Services.Proxies.ClickCallback.Abstraction
{
    public abstract class SimpleClickCallback<TData> : MonoBehaviour, IClickCallback<TData>
    {
        // public Action<TD,TData> OnClick { get; set; } = data =>
        //     
        public GameObject TargetObject { get; set; }
        protected virtual void Awake()
        {
            Initialization(this.gameObject);
        }
        protected void Initialization(GameObject targetGameObject)
        {
            TargetObject = targetGameObject;
            Initialization();
        }
        /// <summary>
        /// Tunnelling init
        /// </summary>
        public abstract void Initialization();
        public Action<Port, TData> OnClick { get; set; } = (type, data) =>
        {
            Debugger.Logger($"{type}: {typeof(TData)}, {data.ToString()}",  ContextDebug.Session,Process.Action);
        };

        public bool IsInit { get; set; } = false;
    }
}