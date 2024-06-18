using Game.Core.DataStructures.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.ResourceListMenu
{
    public class UIResourceListElement : MonoBehaviour
    {
        public UIResource resource;
        public Image image;
        public Image rare;
        public Text title;
        public Text value;
        public void UpdateData(UIResource newResource)
        {
            resource = newResource;
            UpdateData();
        }
        public void UpdateData()
        {
            image.sprite = resource.resourceImage;
            rare.sprite = resource.resourceRareImage.GetSprite(resource.resource.rare);
            title.text = resource.Title;
            value.text = "0";
        }
    }
}