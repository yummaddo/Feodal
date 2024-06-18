using System;
using System.Collections.Generic;
using Game.Services.Abstraction.Service;
using UnityEngine;

namespace Game.Services.Proxies.Abstraction
{
    public abstract class AbstractProvider<TData> : AbstractService
    {
        private static readonly List<IClickCallback<TData>> Callbacks = new List<IClickCallback<TData>>();
        private static event Action<IClickCallback<TData>> OnAddNewCallBack;
        private static event Action<IClickCallback<TData>> OnDeleteCallBack;
        private event Action<TData> OnClick;
        
        protected override void Awake()
        {
            OnAddNewCallBack = TunnelingWithProvider;
            OnDeleteCallBack = DeleteTunnelFormProvider;
            base.Awake();
        }
        protected override void OnAwake() // before OnServiceAwake invocation
        {

        }
        protected override void OnStart() // before OnServiceAwake invocation
        {
            foreach (var preInitCallback in Callbacks)
            {
                preInitCallback.OnClick += OnClickCall;
            }
        }
        internal static void CallBackTunneling(IClickCallback<TData> callback)
        {
            Callbacks.Add(callback);
            OnAddNewCallBack?.Invoke(callback);
        }
        internal static void CallBackDeleteTunnel(IClickCallback<TData> callback)
        {
            Callbacks.Remove(callback);
            OnDeleteCallBack?.Invoke(callback);
        }
        private void TunnelingWithProvider(IClickCallback<TData> newTunnel) => newTunnel.OnClick = OnClickCall;
        private void DeleteTunnelFormProvider(IClickCallback<TData> newTunnel) => newTunnel.OnClick -= OnClickCall;
        internal void RecipientProxyConnect(Action<TData> connection) => OnClick += connection;
        internal void RecipientProxyDisconnect(Action<TData> connection) => OnClick -= connection;
        private  void OnClickCall(TData obj)
        {
            if (OnClick != null) OnClick?.Invoke(obj);
        }
    }
}