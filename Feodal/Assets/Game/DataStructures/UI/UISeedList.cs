using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.Typing;
using Game.UI.Abstraction;
using UnityEngine;

namespace Game.DataStructures.UI
{
    [CreateAssetMenu(menuName = "UI/UISeedList")]
    public class UISeedList : AbstractDataStructure<IUIResourceList>, IUIResourceList
    {
        public List<UISeed> seeds = new List<UISeed>();
        protected override IUIResourceList CompareTemplate()
        {
            Type = ResourceType.Seed;
            Resources = new List<IUIResource>();
            
            foreach (var eResource in seeds)
            {
                Resources.Add(eResource.Data);
            }

            return this;
        }

        internal override string DataNamePattern => $"Seed_UIResourcesList";
        public IResource Universal { get; set; }
        public ResourceType Type { get; set; }
        public List<IUIResource> Resources { get; set; }
    }
}