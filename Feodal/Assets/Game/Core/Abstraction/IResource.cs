using Game.Core.Typing;

namespace Game.Core.Abstraction
{
    public interface IResource
    {
        public string Title { get; set; }
        public ResourceType Type { get; set; }
        public ResourceRareType Rare { get; set; }
        
        public int Quantity { get; set; }
    }
}