using System.Collections.Generic;
using Game.Services.Abstraction.Service;
using Game.Services.Storage.ResourcesRepository;
using UnityEngine;

namespace Game.Services.Storage
{
    public class StorageService : AbstractService
    {
        [SerializeField] private ResourceRepository resourceRepository;
        protected override void OnAwake()
        {
            InitializationRepository();
            LoadRepositoriesData();
        }
        protected override void OnStart() { }

        private void InitializationRepository()
        {
            resourceRepository.Initialization();
        }
        private void LoadRepositoriesData()
        {
            resourceRepository.LoadResourceData();
        }
        private void SaveRepositories()
        {
            Debugger.Logger("OnApplicationQuit: Save Resource Repositories", ContextDebug.Session, Process.Load);
            resourceRepository.SaveResourceData();
        }
        
        
#if UNITY_WEBGL
        private void OnDestroy()
        {
            SaveRepositories();
        }
#elif UNITY_IPHONE
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveRepositories(); 
            }
        }
#elif UNITY_EDITOR
        private void OnApplicationQuit()
        {
            SaveRepositories();
        }
#elif UNITY_STANDALONE_WIN
        private void OnApplicationQuit()
        {
            SaveRepositories();
        }
#else
        private void OnApplicationQuit()
        {
            SaveRepositories();
        }
#endif
    }
}