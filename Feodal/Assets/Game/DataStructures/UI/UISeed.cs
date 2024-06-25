using Game.DataStructures.Abstraction;
using Game.UI.Abstraction;
using Game.UI.Customization;
using UnityEngine;

namespace Game.DataStructures.UI
{
    [CreateAssetMenu(menuName = "UI/UISeed")]
    public class UISeed : AbstractDataStructure<UISeed>, IUIResource
    {
        public Sprite resourceImage;
        public Resource resource;
        public ItemRareSpiteAtlas resourceRareImage;
        public IResource Resource { get; set; }
        public Sprite ResourceImage { get; set; }
        public ItemRareSpiteAtlas ResourceRareImage { get; set; }
        public string Title { get; set; }

        protected override UISeed CompareTemplate()
        {
            Resource = resource.Data;
            ResourceImage = resourceImage;
            ResourceRareImage = resourceRareImage;
            Title = resource.title;
            return this;
        }
        internal override string DataNamePattern => $"{resource.title}_UIResource";

    }
}