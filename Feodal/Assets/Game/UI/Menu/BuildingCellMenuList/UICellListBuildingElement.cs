using Game.CallBacks.CallbackClick.Button;
using Game.DataStructures.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.BuildingCellMenuList
{

    public class UICellListBuildingElement : MonoBehaviour
    {
        public ButtonListContainerElementCallBack elementCallBack;
        public Image cellImage;
        public Image resource;
        public Image universalResource;
        public Text cellTitle;
        public UICellContainerElement element;
        public void UpdateElement()
        {
            cellImage.sprite = element.cellImage;
            resource.sprite = element.cellResource;
            universalResource.sprite = element.cellUniversalResource;
            cellTitle.text = element.cellTitle;
        }
        public void UpdateElement(UICellContainerElement newElement)
        {
            element = newElement;
            elementCallBack.DataInitialization(newElement.Data);
            UpdateElement();
        }
    }
}