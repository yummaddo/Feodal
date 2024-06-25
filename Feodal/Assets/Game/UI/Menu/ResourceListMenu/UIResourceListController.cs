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
    public class UIResourceListController : MonoBehaviour
    {
        [SerializeField] private List<UIResourcesList> resourcesLists;
        [SerializeField] private UISeedList seedListsSeed;
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject targetUniversal;
        [SerializeField] private GameObject templateUIResource;
        internal Dictionary<IResource,UIResourcesList> ResourcesListCompare;
        [Range(1,10)]public int itemInListRoot = 4;
        public List<Transform> targetsListRoot;
        internal List<GameObject> temp = new List<GameObject>();
        internal List<GameObject> tempController = new List<GameObject>();
        internal List<UIResourceListElement> tempControllerElements;
        internal event Action OnTradeFindAndProcessed;
        private void Awake()
        {
            ResourcesListCompare = new Dictionary<IResource, UIResourcesList>();
            foreach (var list in resourcesLists)
                ResourcesListCompare.Add(list.universal, list);
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            SessionLifeStyleManager.AddLifeIteration(OnSceneStart, SessionLifecycle.OnSceneStartSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            tempControllerElements = new List<UIResourceListElement>();
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
                foreach (var element in tempControllerElements)
                    if (element.TryUpdate(callBack.Resource, callBack.Value))
                    { OnTradeFindAndProcessed?.Invoke(); break; }
            }
        }
        internal void Clear()
        {
            foreach (var obj in temp)
                Destroy(obj);
            foreach (var list in targetsListRoot)
                list.gameObject.SetActive(false);
            tempControllerElements.Clear();
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
            temp.Add(newElementInList);
            var newElementController = newElementInList.GetComponent<UIResourceListElement>();
            tempControllerElements.Add(newElementController);
            newElementController.UpdateData(list.seeds[i]);
        }
        private void CreateNewListElements(int i, UIResourcesList list)
        {
            int currentElementOfList = i / itemInListRoot;
            targetsListRoot[currentElementOfList].gameObject.SetActive(true);
            var newElementInList = Instantiate(templateUIResource, targetsListRoot[currentElementOfList]);
            temp.Add(newElementInList);
            var newElementController = newElementInList.GetComponent<UIResourceListElement>();
            tempControllerElements.Add(newElementController);
            newElementController.UpdateData(list.resources[i]);
        }
    }
}