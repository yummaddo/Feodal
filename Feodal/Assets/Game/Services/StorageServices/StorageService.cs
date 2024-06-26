using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.DataStructures;
using Game.DataStructures.Conditions;
using Game.DataStructures.Technologies;
using Game.DataStructures.UI;
using Game.RepositoryEngine.MapCellsRepository;
using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;
using Game.Services.Abstraction;
using Game.Services.CellServices;
using Game.Services.StorageServices.Microservice;
using Game.Utility;
using UnityEngine;

namespace Game.Services.StorageServices
{
    public class StorageService : AbstractService
    {
        [SerializeField] private bool save = true;
        [SerializeField] private bool load = true;

        [SerializeField] private List<Resource> resourceList = new List<Resource>();
        [SerializeField] private List<Seed> seedList = new List<Seed>();
        [SerializeField] private List<Technology> technologyList = new List<Technology>();
        [SerializeField] private List<TradeBuildTechnology> technologyBuildList = new List<TradeBuildTechnology>();
        [SerializeField] private List<ConditionTradeResourceAmount> tradeResourceAmounts = new List<ConditionTradeResourceAmount>();
        [SerializeField] private List<ConditionTradeSeed> tradeSeeds = new List<ConditionTradeSeed>();
        [SerializeField] private List<UIResource> resourcesUI = new List<UIResource>();

        private TradeMicroservice _tradeMicroservice;
        private CellService _cellMicroservice;

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
        
        protected override Task OnAwake(IProgress<float> progress)
        {
            SessionLifeStyleManager.AddLifeIteration(Injection, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            return Task.CompletedTask;
        }
        protected override Task OnStart(IProgress<float> progress)
        {
            InjectRepository();
            OnInit?.Invoke();
            return Task.CompletedTask;
        }
        private void InjectRepository()
        {
            resourceRepository.temp.Injection();
            resourceRepository.temp.InjectionResource(resourceList, resourceRepository);
            resourceRepository.temp.InjectionResource(seedList, resourceRepository);
            resourceRepository.temp.InjectionResource(resourcesUI);
            resourceRepository.temp.InjectionInMicroservice(_tradeMicroservice,_cellMicroservice, gameObject);
            
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
        private Task Injection(IProgress<float> progress)
        {
            _tradeMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            _cellMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            InitializationRepository();
            LoadRepositoriesData();
            return Task.CompletedTask;
        }
        private void InitializationRepository()
        {
            resourceRepository.Initialization(new ResourceIdentifierConvert(),this.GetEncodedResourcesTemplate(resourceList,seedList));
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
        public void SaveRepositories()
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
            technologyRepository.Dispose();
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