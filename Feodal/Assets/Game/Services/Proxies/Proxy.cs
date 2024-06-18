using System;
using Game.Meta;
using Game.Services.Proxies.Abstraction;
using UnityEngine;

namespace Game.Services.Proxies
{
    public static class Proxy
    {
        public static void Connect<TActor, TData>(Action<TData> connection) 
            where TActor : AbstractProvider<TData>
        {
            var stateManager = SessionStateManager.Instance;
            var di = stateManager.Container;
            if (!di.IsRegistered<TActor>())
            {
                throw new Exception($"Actor of type {typeof(TActor)} is not registered");
            }
            TActor actor = di.Resolve<TActor>();
            actor.RecipientProxyConnect(connection);
        }
        public static void Disconnect<TActor, TData>(Action<TData> connection) 
            where TActor : AbstractProvider<TData>
        {
            var stateManager = SessionStateManager.Instance;
            var di = stateManager.Container;
            if (!di.IsRegistered<TActor>())
            {
                throw new Exception($"Actor of type {typeof(TActor)} is not registered");
            }
            TActor actor = di.Resolve<TActor>();
            actor.RecipientProxyDisconnect(connection);
        }
    }
}