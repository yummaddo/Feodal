using System;
using System.Collections;
using Game.Meta;
using UnityEngine;

namespace Game.Services.Abstraction.Service
{
    public abstract class AbstractService : AbstractServiceBoot, IService, IStopSession
    {
        [SerializeField] private SessionActivityStates[] expected =
        {
            SessionActivityStates.Playing
        };
        [SerializeField] private ServiceStatus status = ServiceStatus.Play;
        internal SessionStateManager StateManager;
        internal event Action OnServiceAwake;
        internal event Action OnServiceStart;
        internal event Action OnServiceStop;
        internal event Action OnServiceReStart;
        protected virtual void Freezing() { }
        protected virtual void UnFreezing() { }
        protected virtual void Awake()
        {
            StateManager = SessionStateManager.Instance;
            Debugger.Logger($"Registering type {this.GetType().Name}",  ContextDebug.Session,Process.Initial);
            SessionStateManager.Instance.ServiceLocator.RegisterInstance(this.GetType(), this);
            StateManager.OnSceneAwakeServiceSession += OnAwakeCaller;
            StateManager.OnSceneStartServiceSession += OnStartCaller;
        }
        internal bool GetStatus()
        {
            return Active();
        }
        internal bool GetExpectationStatus()
        {
            return Active() && ((IList)expected).Contains(StateManager.CurrentActivityState);
        }
        protected override bool Active()
        {
            return status == ServiceStatus.Play; 
        }
        public void OnAwakeCaller()
        {

            OnAwake();
            OnServiceAwake?.Invoke();
        }
        public void OnStartCaller()
        {
            OnStart();
            OnServiceStart?.Invoke();
        }
        private void OnDestroy()
        {
            StateManager.OnSceneAwakeServiceSession -= OnAwakeCaller;
            StateManager.OnSceneStartServiceSession -= OnStartCaller;
        }
        public void Stop()
        {
            status = ServiceStatus.Pause;
            OnServiceStop?.Invoke();
        }
        public void ReStart()
        {
            status = ServiceStatus.Play;
            OnServiceReStart?.Invoke();
        }
    }
}