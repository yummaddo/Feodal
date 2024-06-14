using System;
using System.Collections.Generic;
using Game.Core.DataStructures.UI.Data;
using UnityEngine;

namespace Game.UI.BuildingCellMenuList
{
    public class UICellListBuilding : MonoBehaviour
    {
        private List<UICellListBuildingElement> _elements = new List<UICellListBuildingElement>();
        private List<GameObject> _elementsSource = new List<GameObject>();
        public GameObject buildingTemplate;
        public UICellContainer currentViewContainer;
        public void UpdateData()
        {
            foreach (var sGameObject in _elementsSource) { Destroy(sGameObject); }

            foreach (var element in currentViewContainer.uIContainer)
            {
                var obj = Instantiate(buildingTemplate, this.transform);
                _elementsSource.Add(obj);
                var listElement = obj.GetComponent<UICellListBuildingElement>();
                if (listElement == null)
                {
                    throw new InvalidOperationException("The instantiated template element does not contain a UiListBuildingElement component.");
                }
                listElement.UpdateElement(element);
                _elements.Add(listElement);
            }
        }
        public void OnEnable()
        {
            UpdateData();
        }
    }
}