using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.Typing;

namespace Game.UI.Abstraction
{
    public interface IUIResourceList
    {

        public IResource Universal { get; set; }
        public ResourceType Type { get; set; }
        public List<IUIResource> Resources { get; set; }
    }
}