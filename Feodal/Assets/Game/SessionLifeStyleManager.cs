using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Services;
using UnityEngine;

namespace Game
{

    public enum SessionLifecycle
    {
        OnSceneAwakeSession,
        OnSceneAwakeServiceSession,
        OnSceneAwakeMicroServiceSession,
        OnSceneAwakeClose,
        OnSceneStartSession,
        OnSceneStartServiceSession,
        OnSceneStartMicroServiceSession
    }
    /// <summary>
    /// Manages the session state and dependency injection container for the application. 
    /// Handles the initialization and lifecycle events of various service sessions.
    /// </summary>
    public sealed class SessionLifeStyleManager : MonoBehaviour
    {
        [SerializeField] private SessionActivityStates currentActivityStateStatus = SessionActivityStates.Boot;
        /// <summary>
        /// The dependency injection container.
        /// </summary>
        internal DIServiceLocator ServiceLocator;
        /// <summary>
        /// Gets the current activity state of the session.
        /// </summary>
        internal SessionActivityStates CurrentActivityState => currentActivityStateStatus;
        /// <summary>
        /// Indicates whether the microservice session has been initialized.
        /// </summary>
        internal bool IsMicroServiceSessionInit = false;
        private static readonly List<Func<IProgress<float>, Task>> OnSceneAwakeSession = new List<Func<IProgress<float>, Task>>();
        private static readonly List<Func<IProgress<float>, Task>> OnSceneAwakeServiceSession = new List<Func<IProgress<float>, Task>>();
        private static readonly List<Func<IProgress<float>, Task>> OnSceneAwakeMicroServiceSession = new List<Func<IProgress<float>, Task>>();
        private static List<Func<IProgress<float>, Task>> _onSceneAwakeClose = new List<Func<IProgress<float>, Task>>();
        private static List<Func<IProgress<float>, Task>> _onSceneStartSession = new List<Func<IProgress<float>, Task>>();
        private static List<Func<IProgress<float>, Task>> _onSceneStartServiceSession = new List<Func<IProgress<float>, Task>>();
        private static List<Func<IProgress<float>, Task>> _onSceneStartMicroServiceSession = new List<Func<IProgress<float>, Task>>();
        internal Action<float, string> OnLoadProcess;
        internal void SessionAwake()
        {
            _instance = this;
        }
        public static void AddLifeIteration(Func<IProgress<float>,Task> action, SessionLifecycle sessionLifecycle)
        {
            switch (sessionLifecycle)
            {
                case SessionLifecycle.OnSceneAwakeSession:
                    OnSceneAwakeSession.Add(action);
                    break;
                case SessionLifecycle.OnSceneAwakeServiceSession:
                    OnSceneAwakeServiceSession.Add(action);
                    break;
                case SessionLifecycle.OnSceneAwakeMicroServiceSession:
                    OnSceneAwakeMicroServiceSession.Add(action);
                    break;
                case SessionLifecycle.OnSceneAwakeClose:
                    _onSceneAwakeClose.Add(action);
                    break;
                case SessionLifecycle.OnSceneStartSession:
                    _onSceneStartSession.Add(action);
                    break;
                case SessionLifecycle.OnSceneStartServiceSession:
                    _onSceneStartServiceSession.Add(action);
                    break;
                case SessionLifecycle.OnSceneStartMicroServiceSession:
                    _onSceneStartMicroServiceSession.Add(action);
                    break;
            }
        }
        /// <summary>
        /// Initializes the session state manager and triggers the awake lifecycle.
        /// </summary>
        public async Task AwakeStartOperation()
        {
            ServiceLocator = new DIServiceLocator();
            var progress = new Progress<float>(p => { OnLoadProcess?.Invoke(p,",Awake"); });
            await ExecuteAllAwakeSessionsAsync(progress);
            IsMicroServiceSessionInit = true;
            var startProgress = new Progress<float>(p => { OnLoadProcess?.Invoke(p,",Start"); });
            await ExecuteAllStartSessionsAsync(startProgress);
            
        }
        /// <summary>
        /// Starts the session after the awake phase is completed.
        /// </summary>
        private static SessionLifeStyleManager _instance;
        /// <summary>
        /// Gets the singleton instance of the SessionStateManager.
        /// </summary>
        public static SessionLifeStyleManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<SessionLifeStyleManager>();
                    _instance.name = _instance.GetType().ToString();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private async Task ExecuteAllAsync(List<Func<IProgress<float>, Task>> actions, IProgress<float> progress)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                await actions[i](progress);
                progress.Report((float)(i + 1) / actions.Count);
            }
        }
        private async Task ExecuteAllAwakeSessionsAsync(IProgress<float> progress)
        {
            await ExecuteAllAsync(OnSceneAwakeSession, progress);
            await ExecuteAllAsync(OnSceneAwakeServiceSession, progress);
            await ExecuteAllAsync(OnSceneAwakeMicroServiceSession, progress);
            await ExecuteAllAsync(_onSceneAwakeClose, progress);
        }
        private async Task ExecuteAllStartSessionsAsync(IProgress<float> progress)
        {
            await ExecuteAllAsync(_onSceneStartSession, progress);
            await ExecuteAllAsync(_onSceneStartServiceSession, progress);
            await ExecuteAllAsync(_onSceneStartMicroServiceSession, progress);
        }
        /// <summary>
        /// Cleans up the instance and container when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            _instance = null;
            ServiceLocator = null;
        }
    }
}