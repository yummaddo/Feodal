using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Meta;
using Game.Services.Abstraction.Service;
using Game.Services.CellControlling;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.MapCellsRepository;
using Game.Services.Storage.ResourcesRepository;
using HexEngine;
using UnityEngine;

namespace Game.Services.Storage
{
    public class StorageService : AbstractService
    {
        [SerializeField] private List<Resource> resourceList = new List<Resource>();
        [Header("Repositories")]
        [SerializeField] private ResourceRepository resourceRepository;
        public event Action OnResourceRepositoryInit;
        [SerializeField] private MapCellRepository cellsMapRepository;
        public event Action OnCellsMapRepositoryInit;
        protected override void OnAwake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += Injection;
        }
        internal void OnMine(IResource resource, int amount)
        {
            resourceRepository.
        }
        
#if UNITY_EDITOR
        [ContextMenu("XORfunk")]
        public void XORfunk()
        {
            // string publicRsaKey = ApplicationSetting.PublicRsa;
            // Debug.Log("Public RSA Key:");
            // Debug.Log(publicRsaKey);
            // string privateRsaKey = ApplicationSetting.PrivateRsa;
            // Debug.Log("Private RSA Key:");
            // Debug.Log(privateRsaKey);
            // string xorKey = ApplicationSetting.key;
            // string encryptedXorKey = ApplicationSetting.EncryptXorKey(xorKey);
            // Debug.Log("Encrypted XOR Key:");
            // Debug.Log(encryptedXorKey);
            // Debug.Log($" XOR Key: {ApplicationSetting.DecryptXorKey(ApplicationSetting.keyEncrypt)}");
            // Debug.Log($" XOR Key: {ApplicationSetting.DecryptXorKey(ApplicationSetting.keyEncrypt) == ApplicationSetting.key}");
        }
#endif  
        protected override void OnStart()
        {
            resourceRepository.temp.Injection();
            OnResourceRepositoryInit?.Invoke();
            cellsMapRepository.temp.Injection();
            OnCellsMapRepositoryInit?.Invoke();
        }
        internal Temp<MapCellEncoded, HexCoords, string> GetCellMapTemp()
        {
            return cellsMapRepository.temp;
        }
        private void Injection()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession -= Injection;
            InitializationRepository();
            LoadRepositoriesData();
        }
        private void InitializationRepository()
        {
            resourceRepository.Initialization(new ResourceIdentifierConvert(),GetEncodedResourcesTemplate());
            cellsMapRepository.Initialization(new CellIdentifierConvert());
        }
        private void LoadRepositoriesData()
        {
            resourceRepository.LoadResourceData();
            cellsMapRepository.LoadResourceData();
        }
        private void SaveRepositories()
        {
            Debugger.Logger("OnApplicationQuit: Save Resource Repositories", ContextDebug.Session, Process.Load);
            resourceRepository.SaveResourceData();
            Debugger.Logger("OnApplicationQuit: Save Cells Repositories", ContextDebug.Session, Process.Load);
            cellsMapRepository.SaveResourceData();
        }
        private List<ResourceEncoded> GetEncodedResourcesTemplate()
        {
            var resourceEncoded = new List<ResourceEncoded>();
            foreach (var resource in resourceList) resourceEncoded.Add(new ResourceEncoded(resource.Data));
            return resourceEncoded;
        }
        void OnDestroy()
        {
            resourceRepository.Dispose();
            cellsMapRepository.Dispose();
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