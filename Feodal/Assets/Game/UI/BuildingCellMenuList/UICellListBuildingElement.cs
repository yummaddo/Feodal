using Game.Core.DataStructures;
using Game.Core.DataStructures.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.BuildingCellMenuList
{

    public class UICellListBuildingElement : MonoBehaviour
    {
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
            UpdateElement();
        }
    }
}