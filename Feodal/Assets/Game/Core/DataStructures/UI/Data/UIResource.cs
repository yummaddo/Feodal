using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.UI.Customization;
using UnityEngine;

namespace Game.Core.DataStructures.UI.Data
{
    [CreateAssetMenu(menuName = "UI/UIResource")]
    public class UIResource : AbstractDataStructure<IUIResource>, IUIResource
    {
        public Resource resource;
        public Sprite resourceImage;
        public ItemRareSpiteAtlas resourceRareImage;
        protected override IUIResource CompareTemplate()
        {
            ResourceImage = resourceImage;
            ResourceRareImage = resourceRareImage;
            Resource = resource;
            return this;
        }
        public IResource Resource { get; set; }
        public Sprite ResourceImage { get; set; }
        public ItemRareSpiteAtlas ResourceRareImage { get; set; }
        public string Title { get; set; }
        internal override string DataNamePattern => $"{resource.title}_UIResource";
    }
}