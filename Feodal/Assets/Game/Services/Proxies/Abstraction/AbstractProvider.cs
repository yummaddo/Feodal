using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Services.Abstraction.Service;
using UnityEngine;

namespace Game.Services.Proxies.Abstraction
{
    public abstract class AbstractProvider<TData> : AbstractService
    {
        private static readonly Dictionary<Port, List<ICallBack<TData>>> Callbacks =
            new Dictionary<Port, List<ICallBack<TData>>>();
        private static readonly Dictionary<Port, Action<Port,TData>> OnClickTyping = new Dictionary<Port, Action<Port,TData>>();
        protected override void Awake() { base.Awake(); }
        protected override void OnAwake() { }
        protected override void OnStart() { }
        public override string ToString()
        {
            return $"{typeof(AbstractProvider<TData>).FullName}";
        }
        
        /// <summary>
        /// Connects a callback to a specified port for a given type, allowing the callback to be invoked when data is available.
        /// </summary>
        /// <typeparam name="TType">The type of the caller or the context in which the callback is used.</typeparam>
        /// <param name="callback">The callback to be connected.</param>
        internal static void CallBackTunneling<TType>(ICallBack<TData> callback)
        {
            var port = Porting.Type<TType>();
            if (Callbacks.ContainsKey(port))
            {
                callback.OnCallBackInvocation = OnClick;
                Callbacks[port].Add(callback);
            }
            else
            {
                callback.OnCallBackInvocation = OnClick;
                Callbacks.Add(port,new List<ICallBack<TData>>());
                Callbacks[port].Add(callback);
            }
        }
        internal static void CallBackTunneling(ICallBack<TData> callback, Port port)
        {
            if (Callbacks.ContainsKey(port))
            {
                callback.OnCallBackInvocation = OnClick;
                Callbacks[port].Add(callback);
            }
            else
            {
                callback.OnCallBackInvocation = OnClick;
                Callbacks.Add(port,new List<ICallBack<TData>>());
                Callbacks[port].Add(callback);
            }
        }
        /// <summary>
        /// Removes a callback from the specified port for a given type.
        /// </summary>
        /// <typeparam name="TType">The type of the caller or the context in which the callback is used.</typeparam>
        /// <param name="callback">The callback to be removed.</param>
        internal static void CallBackDeleteTunnel<TType>(ICallBack<TData> callback)
        {
            var port = Porting.Type<TType>();
            if (Callbacks.ContainsKey(port)) 
                Callbacks.Remove(port);
        }
        /// <summary>
        /// Invokes the connected action for the given port and data, if such an action is registered.
        /// </summary>
        /// <param name="ip">The port associated with the action.</param>
        /// <param name="obj">The data to be passed to the action.</param>
        private static void OnClick(Port ip, TData obj)
        {
            if (OnClickTyping.TryGetValue(ip, out var data))
            {
                data.Invoke(ip, obj);
            }
        }
        /// <summary>
        /// Registers an action to be executed when data is available for the specified port type.
        /// </summary>
        /// <typeparam name="TType">The type of the caller or the context for the action.</typeparam>
        /// <param name="connection">The action to be registered, taking a Port and TData as parameters.</param>
        internal void RecipientProxyConnect<TType>(Action<Port,TData> connection)
        {
            var port = Porting.Type<TType>();
            if (OnClickTyping.ContainsKey(port))
            {
                OnClickTyping[port] += connection; 
            }
            else OnClickTyping.Add(port,connection);
        }
        internal void RecipientProxyConnect(Action<Port,TData> connection, Port port)
        {
            if (OnClickTyping.ContainsKey(port))
            {
                OnClickTyping[port] += connection; 
            }
            else OnClickTyping.Add(port,connection);
        }
        /// <summary>
        /// Unregisters an action from being executed when data is available for the specified port type.
        /// </summary>
        /// <typeparam name="TType">The type of the caller or the context for the action.</typeparam>
        /// <param name="connection">The action to be unregistered, taking a Port and TData as parameters.</param>
        internal void RecipientProxyDisconnect<TType>(Action<Port,TData> connection)
        {
            var port = Porting.Type<TType>();
            if (OnClickTyping.ContainsKey(port)) 
                OnClickTyping[port] -= connection;
        }
        internal void RecipientProxyDisconnect(Action<Port,TData> connection, Port port)
        {
            if (OnClickTyping.ContainsKey(port)) 
                OnClickTyping[port] -= connection;
        }
    }
}