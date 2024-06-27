using Game.CallBacks.CallbackClick.Button;
using Game.DataStructures.UI;
using Game.Services.CellServices.Microservice;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.BuildingCellMenuList
{

    public class UICellListBuildingElement : MonoBehaviour
    {
        public Image cellImage;
        public Image resource;
        public Image universalResource;
        public Text cellTitle;
        public UICellContainerElement element;
        public Button onClick;
        private CellBuildControllingMicroservice _cellBuildControllingMicroservice;
        public void OnClick()
        {
            _cellBuildControllingMicroservice.BuildSelected(element.Data);
        }

        private void UpdateElement()
        {
            cellImage.sprite = element.cellImage;
            resource.sprite = element.cellResource;
            universalResource.sprite = element.cellUniversalResource;
            cellTitle.text = element.cellTitle;
        }
        public void UpdateElement(UICellContainerElement newElement,
            CellBuildControllingMicroservice cellBuildControllingMicroservice)
        {
            element = newElement;
            _cellBuildControllingMicroservice = cellBuildControllingMicroservice;
            UpdateElement();
        }
    }
}