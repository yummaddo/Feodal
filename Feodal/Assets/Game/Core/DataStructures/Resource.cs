using Game.Core.Abstraction;
using Game.Core.Typing;
using UnityEngine;

namespace Game.Core.DataStructures
{
    [CreateAssetMenu(menuName = "Resource")]
    public class Resource : AbstractDataStructure<IResource>, IResource
    {
        protected override IResource CompareTemplate()
        {
            Title = title;
            Type = type;
            Rare = rare;
            Quantity = quantity;
            return this;
        }

        protected override string DataNamePattern => $"Resource_{type}_{Rare}_{title}";

        public string title;
        public ResourceType type;
        public ResourceRareType rare;
        public int quantity;

        #region IResource
            public string Title { get; set; }
            public ResourceType Type { get; set; }
            public ResourceRareType Rare { get; set; }
            public int Quantity { get; set; }
        #endregion
    }
}