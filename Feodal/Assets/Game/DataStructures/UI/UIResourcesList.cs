using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.Typing;
using Game.UI.Abstraction;
using UnityEngine;

namespace Game.DataStructures.UI
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
        internal override string DataNamePattern => $"{type.ToString()}_UIResourcesList";
        public IResource Universal { get; set; }
        public ResourceType Type { get; set; }
        public List<IUIResource> Resources { get; set; }
    }
}