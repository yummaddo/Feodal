﻿using System;
using System.Collections.Generic;
using Game.DataStructures.UI;
using UnityEngine;

namespace Game.UI.Menu.BuildingCellMenuList
{
    public class UICellListBuilding : MonoBehaviour
    {
        private List<UICellListBuildingElement> _elements = new List<UICellListBuildingElement>();
        private List<GameObject> _elementsSource = new List<GameObject>();
        public GameObject buildingTemplate;
        public UICellContainer currentViewContainer;
        public void UpdateData(UICellContainer container)
        {
            currentViewContainer = container;
            UpdateData();
        }

        private void UpdateData()
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
    }
}