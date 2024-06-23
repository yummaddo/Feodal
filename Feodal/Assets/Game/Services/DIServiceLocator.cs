using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    /// <summary>
    /// A simple dependency injection container for managing service registrations and resolutions.
    /// Supports singleton and transient service lifetimes.
    /// </summary>
    public class DIServiceLocator : IDisposable
    {
        private readonly Dictionary<Type, Func<object>> _registeredServices = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> _singletonInstances = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a singleton service with the specified instance creator.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="instanceCreator">A function that creates an instance of the service.</param>
        public void RegisterSingleton<TService>(Func<TService> instanceCreator) where TService : class
        {
            _registeredServices[typeof(TService)] = () => instanceCreator();
        }

        /// <summary>
        /// Registers a transient service with the specified instance creator.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="instanceCreator">A function that creates an instance of the service.</param>
        public void RegisterTransient<TService>(Func<TService> instanceCreator) where TService : class
        {
            _registeredServices[typeof(TService)] = () => instanceCreator();
        }
        
        /// <summary>
        /// Resolves an instance of the specified service type. If the service is registered as a singleton, returns the same instance on subsequent calls.
        /// </summary>
        /// <typeparam name="TService">The type of service to resolve.</typeparam>
        /// <returns>An instance of the requested service.</returns>
        /// <exception cref="Exception">Thrown when the service type is not registered.</exception>
        public TService Resolve<TService>() where TService : class
        {
            if (_singletonInstances.ContainsKey(typeof(TService)))
            {
                return (TService)_singletonInstances[typeof(TService)];
            }

            if (_registeredServices.ContainsKey(typeof(TService)))
            {
                var service = _registeredServices[typeof(TService)]();
                if (service is TService singletonService)
                {
                    _singletonInstances[typeof(TService)] = singletonService;
                    return singletonService;
                }
            }

            throw new Exception($"Service of type {typeof(TService)} is not registered");
        }
        
        /// <summary>
        /// Registers an existing instance of a service.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="type">The type of the service instance.</param>
        /// <param name="instance">The service instance to register.</param>
        public void RegisterInstance<TService>(Type type, TService instance) where TService : class
        {
            _singletonInstances[type] = instance;
        }
        
        /// <summary>
        /// Removes the singleton instance of the specified service type.
        /// </summary>
        /// <typeparam name="TService">The type of service to reset.</typeparam>
        public void ResetSingleton<TService>() where TService : class
        {
            _singletonInstances.Remove(typeof(TService));
        }
        
        /// <summary>
        /// Checks if the specified service type is registered.
        /// </summary>
        /// <typeparam name="TService">The type of service to check.</typeparam>
        /// <returns>True if the service is registered, otherwise false.</returns>
        public bool IsRegistered<TService>() where TService : class
        {
            return _singletonInstances.ContainsKey(typeof(TService)) || _registeredServices.ContainsKey(typeof(TService));
        }
        /// <summary>
        /// Disposes the container, clearing all registered services and singleton instances.
        /// </summary>
        public void Dispose()
        {
            _singletonInstances.Clear();
            _registeredServices.Clear();
        }
        /// <summary>
        /// Destructor to ensure resources are released.
        /// </summary>
        ~DIServiceLocator()
        {
            Dispose();
        }
    }
}