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
    }
}