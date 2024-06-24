using System;
using System.Threading.Tasks;
using Game.Services;
using UnityEngine;

namespace Game.Meta
{
    /// <summary>
    /// Manages the session state and dependency injection container for the application. 
    /// Handles the initialization and lifecycle events of various service sessions.
    /// </summary>
    public sealed class SessionStateManager : MonoBehaviour
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
        /// <summary>
        /// Initializes the session state manager and triggers the awake lifecycle events.
        /// </summary>
        public async void Awake()
        {
            ServiceLocator = new DIServiceLocator();
            SceneAwakeSession();
            await Task.Delay(100);
            SceneAwakeServiceSession();
            await Task.Delay(100);
            SceneAwakeMicroServiceSession();
            IsMicroServiceSessionInit = true;
            await Task.Delay(60);
            await StartAfterAwake();
            SceneAwakeMicroClose();
        }
        /// <summary>
        /// Event triggered during the scene awake session.
        /// </summary>
        public event Action OnSceneAwakeSession;
        /// <summary>
        /// Event triggered during the service session awake phase.
        /// </summary>
        public event Action OnSceneAwakeServiceSession;
        /// <summary>
        /// Event triggered during the microservice session awake phase.
        /// </summary>
        public event Action OnSceneAwakeMicroServiceSession;
        
        public event Action OnSceneAwakeClose;

        private void SceneAwakeSession() => OnSceneAwakeSession?.Invoke();
        private void SceneAwakeServiceSession() => OnSceneAwakeServiceSession?.Invoke();
        private void SceneAwakeMicroServiceSession() => OnSceneAwakeMicroServiceSession?.Invoke();
        private void SceneAwakeMicroClose() => OnSceneAwakeClose?.Invoke();

        /// <summary>
        /// Starts the session after the awake phase is completed.
        /// </summary>
        private async Task StartAfterAwake()
        {
            await Task.Delay(20);
            SceneStartSession();
            await Task.Delay(100);
            SceneStartServiceSession();
            await Task.Delay(100);
            SceneStartMicroServiceSession();
            await Task.Delay(100);
        }   
        /// <summary>
        /// Event triggered during the scene start session.
        /// </summary>
        public event Action OnSceneStartSession;
        /// <summary>
        /// Event triggered during the microservice session start phase.
        /// </summary>
        public event Action OnSceneStartServiceSession;
        /// <summary>
        /// Triggers the scene start session event.
        /// </summary>
        public event Action OnSceneStartMicroServiceSession;
        
        private void SceneStartSession() => OnSceneStartSession?.Invoke();
        private void SceneStartServiceSession() => OnSceneStartServiceSession?.Invoke();
        private void SceneStartMicroServiceSession() => OnSceneStartMicroServiceSession?.Invoke();

        private static SessionStateManager _instance;
        /// <summary>
        /// Gets the singleton instance of the SessionStateManager.
        /// </summary>
        public static SessionStateManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<SessionStateManager>();
                    _instance.name = _instance.GetType().ToString();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
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