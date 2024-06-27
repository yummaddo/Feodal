using System;
using System.Collections.Generic;
using Game.CallBacks.CallBackTrade;
using Game.DataStructures.UI;
using Game.Services.CellServices.Microservice;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using UnityEngine;

namespace Game.UI.Menu.BuildingCellMenuList
{
    public class UICellListBuilding : UIElementOnEnable
    {
        private List<UICellListBuildingElement> _elements = new List<UICellListBuildingElement>();
        private List<GameObject> _elementsSource = new List<GameObject>();
        public GameObject buildingTemplate;
        public UICellContainer currentViewContainer;
        private CellBuildControllingMicroservice _service;
        protected override void OnEnableSProcess()
        {
            
        }
        protected override void OnAwake()
        {
        }

        protected override void UpdateOnInit()
        {
            
            isInit = true;
        }
        
        public void UpdateData(UICellContainer container)
        {
            currentViewContainer = container;
            _service = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellBuildControllingMicroservice>();
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
                listElement.UpdateElement(element, _service);
                _elements.Add(listElement);
            }
        }
    }
}