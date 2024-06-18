using System;
using UnityEngine;

namespace Game.Services.Proxies.Abstraction
{
    public abstract class AbstractProxy<TActor, TData>
        where TActor : AbstractProvider<TData>
    {
        private TActor _actor;
        private TData _data;
        protected abstract void Connect(TData data);
        protected abstract void Disconnect(TData data);
        
        protected AbstractProxy() { }

        public void SetActor(TActor actor)
        {
            _actor = actor;
            Debug.Log(actor);
        }

        internal void OnConnect(Action<TData> connection)
        {
            Connect(_data);
            _actor.RecipientProxyConnect(connection);
        }

        internal void OnDisconnect(Action<TData> connection)
        {
            Disconnect(_data);
            _actor.RecipientProxyDisconnect(connection);
        }
    }
}