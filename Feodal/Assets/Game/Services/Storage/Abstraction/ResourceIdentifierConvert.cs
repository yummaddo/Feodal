using Game.Core.Abstraction;
using Game.Services.Storage.ResourcesRepository;

namespace Game.Services.Storage.Abstraction
{
    public class ResourceIdentifierConvert : IIdentifier<string,ResourceEncoded>
    {
        public ResourceIdentifierConvert() { }
        public string GetEncodedIdentifier(ResourceEncoded amount)
        {
            return amount.Title;
        }
    }
}