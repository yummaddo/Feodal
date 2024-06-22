using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Services.Abstraction.Service;
using UnityEngine;

namespace Game.Services.Proxies.Abstraction
{
    public abstract class AbstractProvider<TData> : AbstractService
    {
        private static readonly Dictionary<Port, List<IClickCallback<TData>>> Callbacks =
            new Dictionary<Port, List<IClickCallback<TData>>>();
        private static readonly Dictionary<Port, Action<Port,TData>> OnClickTyping = new Dictionary<Port, Action<Port,TData>>();
        protected override void Awake() { base.Awake(); }
        protected override void OnAwake() { }
        protected override void OnStart() { }
        
        internal static void CallBackTunneling<TType>(IClickCallback<TData> callback)
        {
            var port = Porting.Type<TType>();
            if (Callbacks.ContainsKey(port))
            {
                callback.OnClick = OnClick;
                Callbacks[port].Add(callback);
            }
            else
            {
                callback.OnClick = OnClick;
                Callbacks.Add(port,new List<IClickCallback<TData>>());
                Callbacks[port].Add(callback);
            }

        }
        internal static void CallBackDeleteTunnel<TType>(IClickCallback<TData> callback)
        {
            var port = Porting.Type<TType>();
            if (Callbacks.ContainsKey(port)) 
                Callbacks.Remove(port);
        }
        private static void OnClick(Port ip, TData obj)
        {
            if (OnClickTyping.TryGetValue(ip, out var data))
            {
                data.Invoke(ip, obj);
            }
        }
        internal void RecipientProxyConnect<TType>(Action<Port,TData> connection)
        {
            var port = Porting.Type<TType>();
            if (OnClickTyping.ContainsKey(port))
            {
                OnClickTyping[port] += connection; 
            }
            else OnClickTyping.Add(port,connection);
        }

        internal void RecipientProxyDisconnect<TType>(Action<Port,TData> connection)
        {
            var port = Porting.Type<TType>();
            if (OnClickTyping.ContainsKey(port)) 
                OnClickTyping[port] -= connection;
        }
    }
}