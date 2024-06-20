using System;
using System.Threading.Tasks;
using Game.Services;
using UnityEngine;

namespace Game.Meta
{
    public sealed class SessionStateManager : MonoBehaviour
    {
        [SerializeField] private SessionActivityStates currentActivityStateStatus = SessionActivityStates.Boot;
        internal DiContainer Container;
        internal SessionActivityStates CurrentActivityState => currentActivityStateStatus;
        public async void Awake()
        {
            Container = new DiContainer();
            SceneAwakeSession();
            await Task.Delay(100);
            SceneAwakeServiceSession();
            await Task.Delay(100);
            SceneAwakeMicroServiceSession();
            await StartAfterAwake();
        }
        public event Action OnSceneAwakeSession;
        public event Action OnSceneAwakeServiceSession;
        public event Action OnSceneAwakeMicroServiceSession;
        private void SceneAwakeSession() => OnSceneAwakeSession?.Invoke();
        private void SceneAwakeServiceSession() => OnSceneAwakeServiceSession?.Invoke();
        private void SceneAwakeMicroServiceSession() => OnSceneAwakeMicroServiceSession?.Invoke();

        private async Task StartAfterAwake()
        {
            await Task.Delay(200);
            SceneStartSession();
            await Task.Delay(100);
            SceneStartServiceSession();
            await Task.Delay(100);
            SceneStartMicroServiceSession();
            await Task.Delay(100);
        }   
        public event Action OnSceneStartSession;
        public event Action OnSceneStartServiceSession;
        public event Action OnSceneStartMicroServiceSession;
        private void SceneStartSession() => OnSceneStartSession?.Invoke();
        private void SceneStartServiceSession() => OnSceneStartServiceSession?.Invoke();
        private void SceneStartMicroServiceSession() => OnSceneStartMicroServiceSession?.Invoke();

        private static SessionStateManager _instance;
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
        private void OnDestroy()
        {
            _instance = null;
            Container = null;
        }
    }
}