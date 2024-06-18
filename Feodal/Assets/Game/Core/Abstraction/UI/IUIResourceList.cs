using System.Collections.Generic;
using Game.Core.Typing;

namespace Game.Core.Abstraction.UI
{
    public interface IUIResourceList
    {

        public IResource Universal { get; set; }
        public ResourceType Type { get; set; }
        public List<IUIResource> Resources { get; set; }
    }
}