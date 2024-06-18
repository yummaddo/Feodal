using System;
using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures.UI.Data;
using Game.Core.Typing;
using UnityEngine;

namespace Game.UI.Menu.ResourceListMenu
{
    public class UIResourceListController : MonoBehaviour
    {
        [SerializeField] private List<UIResourcesList> resourcesLists;
        internal Dictionary<IResource,UIResourcesList> ResourcesListCompare;
        public int itemInListRoot = 4;
        public List<Transform> targetsListRoot;
        public GameObject templateUIResource;
        public List<GameObject> temp = new List<GameObject>();
        public List<GameObject> tempController = new List<GameObject>();

        private void Awake()
        {
            ResourcesListCompare = new Dictionary<IResource, UIResourcesList>();
            foreach (var list in resourcesLists)
            {
                ResourcesListCompare.Add(list.universal, list);
            }
        }

        internal void Clear()
        {
            foreach (var obj in temp)
            {
                Destroy(obj);
            }
            foreach (var list in targetsListRoot)
            {
                list.gameObject.SetActive(false);
            }
        }
        internal void ViewList(UIResourcesList resourcesList)
        {
            int countOfResource = resourcesList.resources.Count;
            for (int i = 1; i <= countOfResource; i++)
            {
                int currentElementOfList = i / itemInListRoot;
                targetsListRoot[currentElementOfList].gameObject.SetActive(true);
                var newElementInList = Instantiate( templateUIResource, targetsListRoot[currentElementOfList] );
                temp.Add(newElementInList);
                var newElementController = newElementInList.GetComponent<UIResourceListElement>();
                newElementController.UpdateData(resourcesList.resources[i-1]);
            }
        }
    }
}