using Game.Core.Typing;
using Game.Services.Storage.ResourcesRepository;

namespace Game.Core.Abstraction
{
    public interface IResource
    {
        public string Title { get; set; }
        public ResourceTemp Temp { get; set; }
        public ResourceRepository Repository { get; set; }
        public ResourceType Type { get; set; }
        public ResourceRareType Rare { get; set; }
        public int Quantity { get; set; }
    }
}