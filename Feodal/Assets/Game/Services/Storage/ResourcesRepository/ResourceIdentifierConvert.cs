using Game.Services.Storage.Abstraction;

namespace Game.Services.Storage.ResourcesRepository
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