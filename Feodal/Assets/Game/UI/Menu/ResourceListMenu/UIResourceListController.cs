using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks;
using Game.DataStructures.Abstraction;
using Game.DataStructures.UI;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using UnityEngine;

namespace Game.UI.Menu.ResourceListMenu
{
    public class UIResourceListController : UIElementOnEnable
    {
        [SerializeField] private List<UIResourcesList> resourcesLists;
        [SerializeField] private UISeedList seedListsSeed;
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject targetUniversal;
        [SerializeField] private GameObject templateUIResource;
        
        [Range(1,10)]public int itemInListRoot = 4;
        public List<Transform> targetsListRoot;
        
        private List<UIResourceListElement> _tempControllerElements;
        private List<GameObject> _temp;
        
        internal Dictionary<IResource,UIResourcesList> ResourcesListCompare;
        internal event Action OnTradeFindAndProcessed;
        public override void OnAwake()
        {
            ResourcesListCompare = new Dictionary<IResource, UIResourcesList>();
            _tempControllerElements = new List<UIResourceListElement>();
            _temp = new List<GameObject>();
            foreach (var list in resourcesLists)
                ResourcesListCompare.Add(list.universal, list);
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            SessionLifeStyleManager.AddLifeIteration(OnSceneStart, SessionLifecycle.OnSceneStartSession);
        }
        public override void UpdateOnInit()
        {
        }
        public override void OnEnableSProcess()
        {
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            return Task.CompletedTask;
        }
        private Task OnSceneStart(IProgress<float> progress)
        {
            targetUniversal.SetActive(true);
            return Task.CompletedTask;
        }

        private void SomeResourceUpdate(Port port,ResourceTempedCallBack callBack)
        {
            if (enabled)
            {
                foreach (var element in _tempControllerElements)
                    if (element.TryUpdate(callBack.Resource, callBack.Value))
                    { OnTradeFindAndProcessed?.Invoke(); break; }
            }
        }
        internal void Clear()
        {
            foreach (var obj in _temp)
                Destroy(obj);
            foreach (var list in targetsListRoot)
                list.gameObject.SetActive(false);
            _tempControllerElements.Clear();
        }
        
        public void ViewSeedList()
        {            
            target.SetActive(true);
            Clear();
            int countOfResource = seedListsSeed.seeds.Count;
            for (int i = 0; i < countOfResource; i++)
                CreateNewListElements(i,seedListsSeed);
        }
        internal void ViewList(UIResourcesList resourcesList)
        {
            target.SetActive(true);
            int countOfResource = resourcesList.resources.Count;
            for (int i = 0; i < countOfResource; i++)
                CreateNewListElements(i,resourcesList);
        }
        private void CreateNewListElements(int i, UISeedList list)
        {
            int currentElementOfList = i / itemInListRoot;
            targetsListRoot[currentElementOfList].gameObject.SetActive(true);
            var newElementInList = Instantiate(templateUIResource, targetsListRoot[currentElementOfList]);
            _temp.Add(newElementInList);
            var newElementController = newElementInList.GetComponent<UIResourceListElement>();
            _tempControllerElements.Add(newElementController);
            newElementController.UpdateData(list.seeds[i]);
        }
        private void CreateNewListElements(int i, UIResourcesList list)
        {
            int currentElementOfList = i / itemInListRoot;
            targetsListRoot[currentElementOfList].gameObject.SetActive(true);
            var newElementInList = Instantiate(templateUIResource, targetsListRoot[currentElementOfList]);
            _temp.Add(newElementInList);
            var newElementController = newElementInList.GetComponent<UIResourceListElement>();
            _tempControllerElements.Add(newElementController);
            newElementController.UpdateData(list.resources[i]);
        }
    }
}