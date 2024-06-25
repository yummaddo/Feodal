using Game.RepositoryEngine.Abstraction;

namespace Game.RepositoryEngine.ResourcesRepository
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