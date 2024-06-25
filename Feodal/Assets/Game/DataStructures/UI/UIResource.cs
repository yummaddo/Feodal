using Game.DataStructures.Abstraction;
using Game.UI.Abstraction;
using Game.UI.Customization;
using UnityEngine;

namespace Game.DataStructures.UI
{
    [CreateAssetMenu(menuName = "UI/UIResource")]
    public class UIResource : AbstractDataStructure<IUIResource>, IUIResource
    {
        public Resource resource;
        public Sprite resourceImage;
        public ItemRareSpiteAtlas resourceRareImage;
        
        public IResource Resource { get; set; }
        public Sprite ResourceImage { get; set; }
        public ItemRareSpiteAtlas ResourceRareImage { get; set; }
        public string Title { get; set; }

        protected override IUIResource CompareTemplate()
        {
            ResourceImage = resourceImage;
            ResourceRareImage = resourceRareImage;
            Resource = resource.Data;
            Title = resource.title;
            return this;
        }

        internal override string DataNamePattern => $"{resource.title}_UIResource";
    }
}