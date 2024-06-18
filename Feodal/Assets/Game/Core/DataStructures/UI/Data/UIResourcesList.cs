using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.Typing;
using Game.UI.Customization;
using UnityEngine;

namespace Game.Core.DataStructures.UI.Data
{
    [CreateAssetMenu(menuName = "UI/UIResourcesList")]
    public class UIResourcesList : AbstractDataStructure<IUIResourceList>, IUIResourceList
    {
        [SerializeField] internal ResourceType type;
        [SerializeField] internal Resource universal;
        [SerializeField] internal List<UIResource> resources;

        protected override IUIResourceList CompareTemplate()
        {
            Type = type;
            Resources = new List<IUIResource>();
            foreach (var eResource in resources)
            {
                Resources.Add(eResource);
            }

            Universal = universal.Data;
            return this;
        }
        protected override string DataNamePattern => $"{type.ToString()}_UIResourcesList";
        public IResource Universal { get; set; }
        public ResourceType Type { get; set; }
        public List<IUIResource> Resources { get; set; }
    }
}