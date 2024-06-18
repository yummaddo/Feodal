using System.Collections.Generic;
using Game.Core.Typing;
using UnityEngine;

namespace Game.UI.Customization
{
    
    
    [CreateAssetMenu(menuName = "UI/ItemRareSpiteAtlas")]
    public class ItemRareSpiteAtlas : ScriptableObject
    {
        [SerializeField] internal List<ItemRare> atlas = new List<ItemRare>();
        public Sprite GetSprite(ResourceRareType rareType)
        {
            var itemRare = atlas.Find(item=> item.Rare == rareType);
            return itemRare.Sprite;
        }

        [System.Serializable]
        internal class ItemRare
        {
            public ResourceRareType Rare;
            public Sprite Sprite;

            public ItemRare(ResourceRareType rare, Sprite sprite)
            {
                Rare = rare;
                Sprite = sprite;
            }
        }

    }
}