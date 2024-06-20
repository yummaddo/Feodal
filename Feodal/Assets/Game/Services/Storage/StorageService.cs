using System;
using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Base;
using Game.Meta;
using Game.Services.Abstraction.Service;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.MapCellsRepository;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Services.Storage
{
    public class StorageService : AbstractService
    {
        [SerializeField] private List<Resource> resourceList = new List<Resource>();
        [SerializeField] private List<Technology> technologyList = new List<Technology>();
        [SerializeField] private List<TradeBuildTechnology> technologyBuildList = new List<TradeBuildTechnology>();
            
        public event Action OnResourceRepositoryInit;
        public event Action OnCellsMapRepositoryInit;
        public event Action OnTechnologyRepositoryInit;
        [Header("Repositories")]
        [SerializeField] private MapCellRepository cellsMapRepository;
        [SerializeField] private ResourceRepository resourceRepository;
        [SerializeField] private TechnologyRepository technologyRepository;
        internal MapCellTemp GetCellMapTemp() => cellsMapRepository.temp;
        internal ResourceTemp GetResourceTemp() => resourceRepository.temp;
        internal TechnologyTemp GetTechnologyTemp() => technologyRepository.temp;
        protected override void OnAwake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += Injection;
        }
        protected override void OnStart()
        {
            InjectRepository();

        }
        private void InjectRepository()
        {

            resourceRepository.temp.Injection();
            resourceRepository.temp.InjectResource(resourceList, resourceRepository);
            OnResourceRepositoryInit?.Invoke();
            technologyRepository.temp.Injection();
            technologyRepository.temp.InjectTechnologies(technologyList, technologyRepository);
            technologyRepository.temp.InjectTechnologies(technologyBuildList, technologyRepository, resourceRepository.temp);
            OnTechnologyRepositoryInit?.Invoke();
            cellsMapRepository.temp.Injection();
            OnCellsMapRepositoryInit?.Invoke();
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
            cellsMapRepository.Initialization(new MapIdentifierConvert());
            technologyRepository.Initialization(new TechnologyIdentifierConvert(), GetEncodedTechnologyTemplate());
        }
        private void LoadRepositoriesData()
        {
            resourceRepository.LoadResourceData();
            cellsMapRepository.LoadResourceData();
            technologyRepository.LoadResourceData();
        }
        private void SaveRepositories()
        {
            Debugger.Logger("OnApplicationQuit: Save Technology Repositories", ContextDebug.Session, Process.Load);
            technologyRepository.SaveResourceData();
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
        private List<TechnologyEncoded> GetEncodedTechnologyTemplate()
        {
            var techEncoded = new List<TechnologyEncoded>();
            foreach (var tech in technologyList) techEncoded.Add(new TechnologyEncoded(tech));
            return techEncoded;
        }
        private void OnDestroy()
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