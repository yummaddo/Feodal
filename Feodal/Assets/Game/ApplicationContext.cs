using System;
using Game.RepositoryEngine.MapCellsRepository;
using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;
using Game.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ApplicationContext : MonoBehaviour
    {
        [SerializeField] private GameObject bootTemplate;
        [SerializeField] private GameObject menuTemplate;
        public ApplicationSetting setting;
        internal Action<float> OnLoadingGame;
        internal Action<float> OnSavingGame;
        internal Action OnGameEnd;
        
        internal Action<GameObject> OnLoadGame;
        internal Action OnExit;
        internal Action OnResetProgress;
        internal Action<GameObject> OnLoadStartMenu;
        internal Action<Boot> OnLoadGameBoot;

        private GameObject _rootGame;
        private GameObject _rootMenu;

        private static ApplicationContext _instance;
        internal static ApplicationContext Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<ApplicationContext>();
                    _instance.name = _instance.GetType().ToString();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private void Awake()
        {
            DontDestroyOnLoad(this);
            _instance = this;
            OnExit += Exit;
            OnLoadGame += LoadGame;
            OnResetProgress += ResetProgress;
            OnLoadGameBoot += OnBoot;
            OnLoadStartMenu += LoadStartMenu;
            OnLoadStartMenu?.Invoke(null);
        }
        private async void OnBoot(Boot boot)
        {
            await boot.BootOperation();
        }
        private void ResetProgress()
        {
            ResourceRepository.Reset(setting.ResourceSaveFileName);
            TechnologyRepository.Reset(setting.TechnologySaveFileName);
            MapCellRepository.Reset(setting.MapSaveFileName);
        }
        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        private void LoadGame(GameObject uiMenuGameObject)
        {
            if(uiMenuGameObject)
                Destroy(uiMenuGameObject);
            _rootGame = Instantiate(bootTemplate);
        }
        private void LoadStartMenu(GameObject gameGameObject)
        {
            
            if (gameGameObject != null)
            {
                SceneManager.LoadScene("boot");
            }
            else
            {
                _rootMenu = Instantiate(menuTemplate);
            }
        }
    }
}