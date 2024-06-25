using System;
using Game.Services.ProxyServices.Abstraction;

namespace Game.Services.ProxyServices
{
    public static class Proxy
    {
        /// <summary>
        /// Establishes a connection between the specified actor and the provided callback action. 
        /// Ensures the actor is registered and invokes the actor's connection method.
        /// </summary>
        /// <typeparam name="TActor">The type of actor, which must inherit from AbstractProvider[TData] </typeparam>
        /// <typeparam name="TData">The type of data to be processed by the actor.</typeparam>
        /// <typeparam name="TCaller">The type of the caller initiating the connection.</typeparam>
        /// <param name="connection">The action to be executed on connection, taking a Port and TData as parameters.</param>
        /// <exception cref="Exception">Thrown when the specified actor type is not registered in the dependency injection container.</exception>
        public static void Connect<TActor, TData, TCaller>(Action<Port,TData> connection) 
            where TActor : AbstractProvider<TData>
        {
            var stateManager = SessionLifeStyleManager.Instance;
            var di = stateManager.ServiceLocator;
            if (!di.IsRegistered<TActor>())
            {
                throw new Exception($"Actor of type {typeof(TActor)} is not registered");
            }
            TActor actor = di.Resolve<TActor>();
            actor.RecipientProxyConnect<TCaller>(connection);
        }
        public static void Connect<TActor, TData>(Action<Port,TData> connection, Port port) 
            where TActor : AbstractProvider<TData>
        {
            var stateManager = SessionLifeStyleManager.Instance;
            var di = stateManager.ServiceLocator;
            if (!di.IsRegistered<TActor>())
            {
                throw new Exception($"Actor of type {typeof(TActor)} is not registered");
            }
            TActor actor = di.Resolve<TActor>();
            actor.RecipientProxyConnect(connection,port);
        }
        /// <summary>
        /// Disconnects a previously established connection between the specified actor and the provided callback action.
        /// Ensures the actor is registered and invokes the actor's disconnection method.
        /// </summary>
        /// <typeparam name="TActor">The type of actor, which must inherit from AbstractProvider[TData]</typeparam>
        /// <typeparam name="TData">The type of data to be processed by the actor.</typeparam>
        /// <typeparam name="TCaller">The type of the caller initiating the disconnection.</typeparam>
        /// <param name="connection">The action to be disconnected, taking a Port and TData as parameters.</param>
        /// <exception cref="Exception">Thrown when the specified actor type is not registered in the dependency injection container.</exception>
        public static void Disconnect<TActor, TData, TCaller>( Action<Port,TData> connection) 
            where TActor : AbstractProvider<TData>
        {
            var stateManager = SessionLifeStyleManager.Instance;
            var di = stateManager.ServiceLocator;
            if (!di.IsRegistered<TActor>())
            {
                throw new Exception($"Actor of type {typeof(TActor)} is not registered");
            }
            TActor actor = di.Resolve<TActor>();
            actor.RecipientProxyDisconnect<TCaller>( connection);
        }
        public static void Disconnect<TActor, TData>( Action<Port,TData> connection, Port port) 
            where TActor : AbstractProvider<TData>
        {
            var stateManager = SessionLifeStyleManager.Instance;
            var di = stateManager.ServiceLocator;
            if (!di.IsRegistered<TActor>())
            {
                throw new Exception($"Actor of type {typeof(TActor)} is not registered");
            }
            TActor actor = di.Resolve<TActor>();
            actor.RecipientProxyDisconnect( connection, port);
        }
    }
}