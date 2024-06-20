using Game.Core.Abstraction;
using Game.Services.Storage.Abstraction;

namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceTemp : Temp<ResourceEncoded,string, long>
    {
        protected override long SumAmounts(long a, long b)
        {
            return a + b;
        }
        protected override string GetIdentifierByEncoded(ResourceEncoded encoded)
        {
            return encoded.Title;
        }

        internal void AddAmounts(IResource resource, int amount)
        {
            DataByIdentifier[resource.Title] += amount;
        }
        internal void GetAmounts(IResource resource, int amount)
        {
            DataByIdentifier[resource.Title] -= amount;
        }
        
        
        
        // internal void GetAmounts(IResource resource, int amount)
        // {
        //     DataByIdentifier[resource.Title] -= amount;
        // }
        
        internal ResourceTemp(IIdentifier<string, ResourceEncoded> identifier) : base(identifier)
        {
            
        }
    }
}