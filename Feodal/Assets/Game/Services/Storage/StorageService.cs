using System;
using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Conditions;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Trades;
using Game.Core.DataStructures.UI.Data;
using Game.Meta;
using Game.Services.Abstraction.Service;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.MapCellsRepository;
using Game.Services.Storage.Microservice;
using Game.Services.Storage.ResourcesRepository;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Services.Storage
{
    public class StorageService : AbstractService
    {
        [SerializeField] private bool save = true;
        [SerializeField] private bool load = true;

        [SerializeField] private List<Resource> resourceList = new List<Resource>();
        [SerializeField] private List<Technology> technologyList = new List<Technology>();
        [SerializeField] private List<TradeBuildTechnology> technologyBuildList = new List<TradeBuildTechnology>();
        [SerializeField] private List<ConditionTradeResourceAmount> tradeResourceAmounts = new List<ConditionTradeResourceAmount>();
        [SerializeField] private List<ConditionTradeSeed> tradeSeeds = new List<ConditionTradeSeed>();
        [SerializeField] private List<UIResource> resourcesUI = new List<UIResource>();

        private TradeMicroservice _tradeMicroservice;
        
        public event Action OnResourceRepositoryInit;
        public event Action OnCellsMapRepositoryInit;
        public event Action OnTechnologyRepositoryInit;
        public event Action OnInit;
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
            OnInit?.Invoke();
        }
        private void InjectRepository()
        {
            resourceRepository.temp.Injection();
            resourceRepository.temp.InjectResource(resourceList, resourceRepository);
            resourceRepository.temp.InjectionResource(resourcesUI);
            resourceRepository.temp.InjectionInMicroservice(_tradeMicroservice, gameObject);
            
            resourceRepository.temp.InjectionTrade(this.GetTradeSetTemplate(tradeResourceAmounts));
            resourceRepository.temp.InjectionTrade(this.GetTradeSetTemplate(technologyBuildList));
            resourceRepository.temp.InjectionTrade(this.GetTradeSetTemplate(tradeSeeds));
            resourceRepository.temp.InjectionTrade(this.GetTradeSetTemplate(technologyList));
            
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
            _tradeMicroservice = SessionStateManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            InitializationRepository();
            LoadRepositoriesData();
        }
        private void InitializationRepository()
        {
            resourceRepository.Initialization(new ResourceIdentifierConvert(),this.GetEncodedResourcesTemplate(resourceList));
            cellsMapRepository.Initialization(new MapIdentifierConvert());
            technologyRepository.Initialization(new TechnologyIdentifierConvert(), this.GetEncodedTechnologyTemplate(technologyList));
        }
        private void LoadRepositoriesData()
        {
            if (load)
            {
                resourceRepository.LoadResourceData();
                cellsMapRepository.LoadResourceData();
                technologyRepository.LoadResourceData();
            }
        }
        private void SaveRepositories()
        {
            if (save)
            {
                Debugger.Logger("OnApplicationQuit: Save Technology Repositories", ContextDebug.Session, Process.Load);
                technologyRepository.SaveResourceData();
                Debugger.Logger("OnApplicationQuit: Save Resource Repositories", ContextDebug.Session, Process.Load);
                resourceRepository.SaveResourceData();
                Debugger.Logger("OnApplicationQuit: Save Cells Repositories", ContextDebug.Session, Process.Load);
                cellsMapRepository.SaveResourceData();
            }
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