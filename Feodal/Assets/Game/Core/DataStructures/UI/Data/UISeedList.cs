using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.Typing;
using UnityEngine;

namespace Game.Core.DataStructures.UI.Data
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