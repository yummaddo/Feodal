using Game.UI.Customization;
using UnityEngine;

namespace Game.Core.Abstraction.UI
{
    public interface IUIResource
    {
        public IResource Resource { get; set; }
        public Sprite ResourceImage { get; set; }
        public ItemRareSpiteAtlas ResourceRareImage { get; set; }
        public string Title { get; set; }
    }
}