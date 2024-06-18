using Game.Core.Abstraction;
using Game.Core.Typing;

namespace Game.Services.Storage.ResourcesRepository
{

    [System.Serializable]
    public class ResourceEncoded : IResource
    {
        public string Title { get; set; }
        public ResourceType Type { get; set; }
        public ResourceRareType Rare { get; set; }
        public int Quantity { get; set; }
        public ResourceEncoded(string title, ResourceType type, ResourceRareType rare, int quantity)
        {
            Title = title;
            Type = type;
            Rare = rare;
            Quantity = quantity;
        }
        public ResourceEncoded(IResource resource)
        {
            Title = resource.Title;
            Type = resource.Type;
            Rare = resource.Rare;
            Quantity = resource.Quantity;
        }
        public override string ToString()
        {
            return $"{Title}_{Type}_{Rare},{Quantity}";
        }
    }
}