using System;
using Game.Meta;
using Game.Services.Abstraction.Service;
using UnityEngine;

namespace Game.Services.Abstraction.MicroService
{
    public abstract class AbstractMicroservice<TService> : AbstractServiceBoot
     
       where TService : AbstractService, IStopSession
    {
        [SerializeField] private ServiceStatus status = ServiceStatus.Play; 
        internal event Action OnMicroServiceStop;
        internal event Action OnMicroServiceReStart;
        protected TService Service;
        protected SessionStateManager StateManager => Service.StateManager;
        protected virtual void Awake()
        {
            var stateManager = SessionStateManager.Instance;
            Debugger.Logger($"Registering type {this.GetType()}",  ContextDebug.Session,Process.Action);
            stateManager.ServiceLocator.RegisterInstance(this.GetType(), this);
            stateManager.OnSceneAwakeMicroServiceSession += SceneAwakeMicroServiceSession;
            stateManager.OnSceneStartMicroServiceSession += OnStart;
        }

        private void SceneAwakeMicroServiceSession()
        {
            GetParentService();
            OnAwake();
        }

        private void OnDestroy()
        {
            var stateManager = SessionStateManager.Instance;
            stateManager.OnSceneAwakeMicroServiceSession -= SceneAwakeMicroServiceSession;
            stateManager.OnSceneStartMicroServiceSession -= OnStart;
        }

        private void GetParentService()
        {
            var stateManager = SessionStateManager.Instance;
            Service = stateManager.ServiceLocator.Resolve<TService>();
        }

        private void MicroServiceStop()
        {
            status = ServiceStatus.Pause;
            Stop();
            OnMicroServiceStop?.Invoke();
        }
        private void MicroServiceReStart()
        {
            status = ServiceStatus.Play;
            ReStart();
            OnMicroServiceReStart?.Invoke();
        }
        protected override bool Active()
        {
            return status == ServiceStatus.Play; 
        }
        protected virtual void Freezing() { }
        protected virtual void UnFreezing() { }
        protected abstract void ReStart();
        protected abstract void Stop();
    }
}