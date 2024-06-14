using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public class DiContainer
    {
        private readonly Dictionary<Type, Func<object>> _registeredServices = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> _singletonInstances = new Dictionary<Type, object>();

        public void RegisterSingleton<TService>(Func<TService> instanceCreator) where TService : class
        {
            _registeredServices[typeof(TService)] = () => instanceCreator();
        }

        public void RegisterTransient<TService>(Func<TService> instanceCreator) where TService : class
        {
            _registeredServices[typeof(TService)] = () => instanceCreator();
        }

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
        public void RegisterInstance<TService>(Type type, TService instance) where TService : class
        {
            _singletonInstances[type] = instance;
        }
        public void ResetSingleton<TService>() where TService : class
        {
            _singletonInstances.Remove(typeof(TService));
        }
    }
}